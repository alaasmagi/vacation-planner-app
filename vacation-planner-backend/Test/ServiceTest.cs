using Application;
using Base.Contracts.DataAccess;
using Base.Contracts.DTO;
using Base.DTO;
using Contract.DataAccess;
using DataAccess;
using Domain;
using DTO.Presentation;
using Moq;

namespace Test;

public class ServiceTest
{
    private AppDbContext _dbContext = null!;
    private IDisposable _connection = null!;
    private VacationRequestService _service = null!;

    [SetUp]
    public void Setup()
    {
        var (context, connection) = TestHelpers.CreateSqliteContext();
        _dbContext = context;
        _connection = connection;

        var repository = new VacationRequestRepository(_dbContext, TestHelpers.CreateDataAccessMapper());
        var uow = new DataAccessUow(_dbContext);
        var mapper = TestHelpers.CreateDtoMapper();

        _service = new VacationRequestService(uow, repository, mapper);
    }

    [TearDown]
    public void TearDown()
    {
        _dbContext.Dispose();
        _connection.Dispose();
    }

    [Test]
    public async Task CreateWithValidationAsync_ShouldFail_WhenDuplicateExists()
    {
        var uowMock = new Mock<IBaseUow>();
        var repositoryMock = new Mock<IVacationRequestRepository>();
        var mapperMock = new Mock<IMapper<VacationRequestDto, VacationRequest>>();

        repositoryMock
            .Setup(r => r.ExistsAsync(It.IsAny<Guid>(), It.IsAny<DateOnly>(), It.IsAny<DateOnly>()))
            .ReturnsAsync(MethodResponse<bool>.Success(true));

        var service = new VacationRequestService(uowMock.Object, repositoryMock.Object, mapperMock.Object);
        var dto = CreateDto(Guid.NewGuid(), new DateOnly(2026, 07, 01), new DateOnly(2026, 07, 05));

        var response = await service.CreateWithValidationAsync(dto, null);

        Assert.That(response.Successful, Is.False);
    }

    [Test]
    public async Task CreateWithValidationAsync_ShouldCreate_WhenNoOverlapExists()
    {
        var dto = CreateDto(Guid.NewGuid(), new DateOnly(2026, 08, 10), new DateOnly(2026, 08, 15));

        var response = await _service.CreateWithValidationAsync(dto, null);

        Assert.Multiple(() =>
        {
            Assert.That(response.Successful, Is.True);
            Assert.That(response.Value, Is.Not.Null);
            Assert.That(response.Value!.Id, Is.EqualTo(dto.Id));
        });
    }

    [Test]
    public async Task GetAllAndGetById_ShouldReturnCreatedItems()
    {
        var first = CreateDto(Guid.NewGuid(), new DateOnly(2026, 09, 01), new DateOnly(2026, 09, 03));
        var second = CreateDto(Guid.NewGuid(), new DateOnly(2026, 09, 05), new DateOnly(2026, 09, 08));

        await _service.CreateAsync(first);
        await _service.CreateAsync(second);

        var allResponse = await _service.GetAllAsync();
        var byIdResponse = await _service.GetByIdAsync(first.Id);

        Assert.Multiple(() =>
        {
            Assert.That(allResponse.Successful, Is.True);
            Assert.That(allResponse.Value, Is.Not.Null);
            Assert.That(allResponse.Value!.Count(), Is.EqualTo(2));
            Assert.That(byIdResponse.Successful, Is.True);
            Assert.That(byIdResponse.Value!.EmployeeId, Is.EqualTo(first.EmployeeId));
        });
    }

    [Test]
    public async Task UpdateAsync_ShouldPersistChanges()
    {
        var dto = CreateDto(Guid.NewGuid(), new DateOnly(2026, 10, 01), new DateOnly(2026, 10, 05));
        await _service.CreateAsync(dto);
        _dbContext.ChangeTracker.Clear();

        var updatedDto = new VacationRequestDto
        {
            Id = dto.Id,
            EmployeeId = dto.EmployeeId,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            Comment = "Updated via service",
            Status = EVacationStatus.Approved
        };

        var updateResponse = await _service.UpdateAsync(updatedDto.Id, updatedDto);
        _dbContext.ChangeTracker.Clear();
        var getResponse = await _service.GetByIdAsync(updatedDto.Id);

        Assert.Multiple(() =>
        {
            Assert.That(updateResponse.Successful, Is.True);
            Assert.That(getResponse.Successful, Is.True);
            Assert.That(getResponse.Value!.Comment, Is.EqualTo("Updated via service"));
            Assert.That(getResponse.Value.Status, Is.EqualTo(EVacationStatus.Approved));
        });
    }

    [Test]
    public async Task RemoveAsync_ShouldDeleteEntity()
    {
        var dto = CreateDto(Guid.NewGuid(), new DateOnly(2026, 11, 01), new DateOnly(2026, 11, 05));
        await _service.CreateAsync(dto);

        var removeResponse = await _service.RemoveAsync(dto.Id);
        var getResponse = await _service.GetByIdAsync(dto.Id);

        Assert.Multiple(() =>
        {
            Assert.That(removeResponse.Successful, Is.True);
            Assert.That(getResponse.Successful, Is.False);
        });
    }

    private static VacationRequestDto CreateDto(Guid employeeId, DateOnly startDate, DateOnly endDate)
    {
        return new VacationRequestDto
        {
            Id = Guid.NewGuid(),
            EmployeeId = employeeId,
            StartDate = startDate,
            EndDate = endDate,
            Comment = "Service test",
            Status = EVacationStatus.Pending
        };
    }
}