using Domain;
using DTO.DataAccess;
using DTO.Presentation;
using DTO.Presentation.Mappers;

namespace Test;

public class MapperTest
{
    [Test]
    public void VacationRequestMapper_ShouldMapDomainToEntity_AndBack()
    {
        var mapper = TestHelpers.CreateDataAccessMapper(defaultVacationLength: 10);
        var source = new VacationRequest(defaultVacationLength: 10)
        {
            Id = Guid.NewGuid(),
            EmployeeId = Guid.NewGuid(),
            StartDate = new DateOnly(2026, 03, 10),
            EndDate = new DateOnly(2026, 03, 14),
            Comment = "Mapper",
            Status = EVacationStatus.Approved
        };

        var entity = mapper.Map(source);
        var mappedBack = mapper.Map(entity);

        Assert.Multiple(() =>
        {
            Assert.That(entity, Is.Not.Null);
            Assert.That(entity!.Id, Is.EqualTo(source.Id));
            Assert.That(entity.Comment, Is.EqualTo(source.Comment));
            Assert.That(mappedBack, Is.Not.Null);
            Assert.That(mappedBack!.EmployeeId, Is.EqualTo(source.EmployeeId));
            Assert.That(mappedBack.Status, Is.EqualTo(source.Status));
        });
    }

    [Test]
    public void VacationRequestDtoMapper_ShouldMapDomainToDto_WithComputedFields()
    {
        var mapper = TestHelpers.CreateDtoMapper(defaultVacationLength: 2);
        var source = new VacationRequest(defaultVacationLength: 2)
        {
            Id = Guid.NewGuid(),
            EmployeeId = Guid.NewGuid(),
            StartDate = new DateOnly(2026, 04, 01),
            EndDate = new DateOnly(2026, 04, 05),
            Comment = "DTO",
            Status = EVacationStatus.Pending
        };

        var dto = mapper.Map(source);

        Assert.Multiple(() =>
        {
            Assert.That(dto, Is.Not.Null);
            Assert.That(dto!.DurationDays, Is.EqualTo(4));
            Assert.That(dto.IsOverTime, Is.True);
            Assert.That(dto.Comment, Is.EqualTo(source.Comment));
        });
    }

    [Test]
    public void VacationRequestWebDtoMapper_ShouldApplyDefaults_WhenOptionalFieldsAreMissing()
    {
        var mapper = new VacationRequestWebDtoMapper();
        var webDto = new VacationRequestWebDto
        {
            Id = null,
            EmployeeId = Guid.NewGuid(),
            StartDate = new DateOnly(2026, 05, 01),
            EndDate = new DateOnly(2026, 05, 03),
            Status = null
        };

        var dto = mapper.Map(webDto);

        Assert.Multiple(() =>
        {
            Assert.That(dto, Is.Not.Null);
            Assert.That(dto!.Id, Is.EqualTo(Guid.Empty));
            Assert.That(dto.Status, Is.EqualTo(EVacationStatus.Pending));
        });
    }

    [Test]
    public void Mappers_ShouldReturnNull_WhenInputIsNull()
    {
        var dataMapper = TestHelpers.CreateDataAccessMapper();
        var dtoMapper = TestHelpers.CreateDtoMapper();
        var webMapper = new VacationRequestWebDtoMapper();

        VacationRequest? domainNull = null;
        VacationRequestEntity? entityNull = null;
        VacationRequestDto? dtoNull = null;
        VacationRequestWebDto? webNull = null;

        Assert.Multiple(() =>
        {
            Assert.That(dataMapper.Map(domainNull), Is.Null);
            Assert.That(dataMapper.Map(entityNull), Is.Null);
            Assert.That(dtoMapper.Map(domainNull), Is.Null);
            Assert.That(dtoMapper.Map(dtoNull), Is.Null);
            Assert.That(webMapper.Map(dtoNull), Is.Null);
            Assert.That(webMapper.Map(webNull), Is.Null);
        });
    }

    [Test]
    public void MapperCollections_ShouldMapAllItems()
    {
        var dtoMapper = TestHelpers.CreateDtoMapper(defaultVacationLength: 10);
        var webMapper = new VacationRequestWebDtoMapper();

        var domainItems = new[]
        {
            new VacationRequest(defaultVacationLength: 10)
            {
                Id = Guid.NewGuid(),
                EmployeeId = Guid.NewGuid(),
                StartDate = new DateOnly(2026, 06, 01),
                EndDate = new DateOnly(2026, 06, 03),
                Status = EVacationStatus.Pending
            },
            new VacationRequest(defaultVacationLength: 10)
            {
                Id = Guid.NewGuid(),
                EmployeeId = Guid.NewGuid(),
                StartDate = new DateOnly(2026, 07, 01),
                EndDate = new DateOnly(2026, 07, 04),
                Status = EVacationStatus.Rejected
            }
        };

        var mappedDtos = dtoMapper.Map(domainItems)!.ToList();
        var mappedWeb = webMapper.Map(mappedDtos)!.ToList();

        Assert.Multiple(() =>
        {
            Assert.That(mappedDtos.Count, Is.EqualTo(2));
            Assert.That(mappedWeb.Count, Is.EqualTo(2));
            Assert.That(mappedWeb[1].Status, Is.EqualTo(EVacationStatus.Rejected));
        });
    }
}