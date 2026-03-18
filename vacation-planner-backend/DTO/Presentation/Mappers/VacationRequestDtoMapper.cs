using Base.Contracts.DTO;
using Domain;

namespace DTO.Presentation.Mappers;

public class VacationRequestDtoMapper : IMapper<VacationRequestDto, VacationRequest>
{
    public VacationRequestDto? Map(VacationRequest? entity)
    {
        if (entity == null) return null;
    
        return new VacationRequestDto
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

    public IEnumerable<VacationRequestDto>? Map(IEnumerable<VacationRequest>? entities)
    {
        if (entities == null) return null;
        return entities.Select(e => Map(e)!);
    }

    public VacationRequest? Map(VacationRequestDto? entity)
    {
        if (entity == null) return null;
    
        return new VacationRequest
        {
            Id = entity.Id,
            StartDate = entity.StartDate,
            EndDate = entity.EndDate,
            Comment = entity.Comment,
            Status = entity.Status,
            EmployeeId = entity.EmployeeId,
        };
    }

    public IEnumerable<VacationRequest>? Map(IEnumerable<VacationRequestDto>? entities)
    {
        if (entities == null) return null;
        return entities.Select(e => Map(e)!);
    }
}