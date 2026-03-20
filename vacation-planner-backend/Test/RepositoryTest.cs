using DataAccess;
using Domain;

namespace Test;

public class RepositoryTest
{
    private AppDbContext _dbContext = null!;
    private VacationRequestRepository _repository = null!;
    private IDisposable _connection = null!;

    [SetUp]
    public void Setup()
    {
        var (context, connection) = TestHelpers.CreateSqliteContext();
        _dbContext = context;
        _connection = connection;
        _repository = new VacationRequestRepository(_dbContext, TestHelpers.CreateDataAccessMapper());
    }

    [TearDown]
    public void TearDown()
    {
        _dbContext.Dispose();
        _connection.Dispose();
    }

    [Test]
    public async Task CreateAndGetById_ShouldPersistEntity()
    {
        var employeeId = Guid.NewGuid();
        var request = CreateRequest(employeeId, new DateOnly(2026, 07, 01), new DateOnly(2026, 07, 05));

        var createResponse = await _repository.CreateAsync(request);
        await _dbContext.SaveChangesAsync();
        var getByIdResponse = await _repository.GetByIdAsync(request.Id);

        Assert.Multiple(() =>
        {
            Assert.That(createResponse.Successful, Is.True);
            Assert.That(getByIdResponse.Successful, Is.True);
            Assert.That(getByIdResponse.Value, Is.Not.Null);
            Assert.That(getByIdResponse.Value!.EmployeeId, Is.EqualTo(employeeId));
        });
    }

    [Test]
    public async Task GetAll_ShouldReturnAllPersistedEntities()
    {
        await _repository.CreateAsync(CreateRequest(Guid.NewGuid(), new DateOnly(2026, 08, 01), new DateOnly(2026, 08, 03)));
        await _repository.CreateAsync(CreateRequest(Guid.NewGuid(), new DateOnly(2026, 08, 04), new DateOnly(2026, 08, 07)));
        await _dbContext.SaveChangesAsync();

        var response = await _repository.GetAllAsync();

        Assert.Multiple(() =>
        {
            Assert.That(response.Successful, Is.True);
            Assert.That(response.Value, Is.Not.Null);
            Assert.That(response.Value!.Count(), Is.EqualTo(2));
        });
    }

    [Test]
    public async Task Update_ShouldChangePersistedValues()
    {
        var request = CreateRequest(Guid.NewGuid(), new DateOnly(2026, 09, 01), new DateOnly(2026, 09, 05));
        await _repository.CreateAsync(request);
        await _dbContext.SaveChangesAsync();
        _dbContext.ChangeTracker.Clear();

        request.Comment = "Updated comment";
        request.Status = EVacationStatus.Approved;

        var updateResponse = await _repository.UpdateAsync(request.Id, request);
        await _dbContext.SaveChangesAsync();
        _dbContext.ChangeTracker.Clear();
        var getByIdResponse = await _repository.GetByIdAsync(request.Id);

        Assert.Multiple(() =>
        {
            Assert.That(updateResponse.Successful, Is.True);
            Assert.That(getByIdResponse.Successful, Is.True);
            Assert.That(getByIdResponse.Value!.Comment, Is.EqualTo("Updated comment"));
            Assert.That(getByIdResponse.Value.Status, Is.EqualTo(EVacationStatus.Approved));
        });
    }

    [Test]
    public async Task Remove_ShouldDeleteEntity()
    {
        var request = CreateRequest(Guid.NewGuid(), new DateOnly(2026, 10, 01), new DateOnly(2026, 10, 05));
        await _repository.CreateAsync(request);
        await _dbContext.SaveChangesAsync();
        _dbContext.ChangeTracker.Clear();

        var removeResponse = await _repository.RemoveAsync(request.Id);
        await _dbContext.SaveChangesAsync();
        _dbContext.ChangeTracker.Clear();
        var getByIdResponse = await _repository.GetByIdAsync(request.Id);

        Assert.Multiple(() =>
        {
            Assert.That(removeResponse.Successful, Is.True);
            Assert.That(getByIdResponse.Successful, Is.False);
        });
    }

    [Test]
    public async Task ExistsAsync_ShouldReturnTrue_WhenDateRangesOverlapForSameEmployee()
    {
        var employeeId = Guid.NewGuid();
        await _repository.CreateAsync(CreateRequest(employeeId, new DateOnly(2026, 11, 10), new DateOnly(2026, 11, 15)));
        await _dbContext.SaveChangesAsync();

        var response = await _repository.ExistsAsync(employeeId, new DateOnly(2026, 11, 14), new DateOnly(2026, 11, 20));

        Assert.Multiple(() =>
        {
            Assert.That(response.Successful, Is.True);
            Assert.That(response.Value, Is.True);
        });
    }

    [Test]
    public async Task ExistsAsync_ShouldReturnFalse_WhenRangesDoNotOverlap()
    {
        var employeeId = Guid.NewGuid();
        await _repository.CreateAsync(CreateRequest(employeeId, new DateOnly(2026, 12, 01), new DateOnly(2026, 12, 05)));
        await _dbContext.SaveChangesAsync();

        var response = await _repository.ExistsAsync(employeeId, new DateOnly(2026, 12, 06), new DateOnly(2026, 12, 10));

        Assert.Multiple(() =>
        {
            Assert.That(response.Successful, Is.True);
            Assert.That(response.Value, Is.False);
        });
    }

    private static VacationRequest CreateRequest(Guid employeeId, DateOnly startDate, DateOnly endDate)
    {
        return new VacationRequest(defaultVacationLength: 14)
        {
            Id = Guid.NewGuid(),
            EmployeeId = employeeId,
            StartDate = startDate,
            EndDate = endDate,
            Comment = "Vacation",
            Status = EVacationStatus.Pending
        };
    }
}