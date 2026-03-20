using Base.Contracts.DTO;
using Domain;

namespace DTO.Presentation.Mappers;

public class VacationRequestWebDtoMapper : IMapper<VacationRequestDto, VacationRequestWebDto>
{
    public VacationRequestDto? Map(VacationRequestWebDto? entity)
    {
        if (entity == null) return null;

        return new VacationRequestDto
        {
            Id = entity.Id ?? new Guid(),
            StartDate = entity.StartDate,
            EndDate = entity.EndDate,
            Comment = entity.Comment,
            Status = entity.Status ?? EVacationStatus.Pending,
            EmployeeId = entity.EmployeeId,
            DurationDays = entity.DurationDays,
            IsOverTime = entity.IsOverTime
        };
    }

    public IEnumerable<VacationRequestDto>? Map(IEnumerable<VacationRequestWebDto>? entities)
    {
        if (entities == null) return null;
        return entities.Select(e => Map(e)!);
    }

    public VacationRequestWebDto? Map(VacationRequestDto? entity)
    {
        if (entity == null) return null;

        return new VacationRequestWebDto
        {
            Id = entity.Id,
            StartDate = entity.StartDate,
            EndDate = entity.EndDate,
            Comment = entity.Comment,
            Status = entity.Status,
            EmployeeId = entity.EmployeeId,
            DurationDays = entity.DurationDays,
            IsOverTime = entity.IsOverTime
        };    
    }

    public IEnumerable<VacationRequestWebDto>? Map(IEnumerable<VacationRequestDto>? entities)
    {
        if (entities == null) return null;
        return entities.Select(e => Map(e)!);    
    }
}