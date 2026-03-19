using Base.Contracts.DTO;
using Domain;
using Helpers;

namespace DTO.DataAccess.Mappers;

public class VacationRequestMapper(EnvInitializer envInitializer) : IMapper<VacationRequest, VacationRequestEntity>
{
    public VacationRequest? Map(VacationRequestEntity? entity)
    {
        if (entity == null) return null;
    
        return new VacationRequest(envInitializer.DefaultVacationLength)
        {
            Id = entity.Id,
            StartDate = entity.StartDate,
            EndDate = entity.EndDate,
            Comment = entity.Comment,
            Status = entity.Status,
            EmployeeId = entity.EmployeeId
        };
    }

    public IEnumerable<VacationRequest>? Map(IEnumerable<VacationRequestEntity>? entities)
    {
        if (entities == null) return null;
        return entities.Select(e => Map(e)!);
    }

    public VacationRequestEntity? Map(VacationRequest? entity)
    {
        if (entity == null) return null;
    
        return new VacationRequestEntity
        {
            Id = entity.Id,
            StartDate = entity.StartDate,
            EndDate = entity.EndDate,
            Comment = entity.Comment,
            Status = entity.Status,
            EmployeeId = entity.EmployeeId
        };
    }

    public IEnumerable<VacationRequestEntity>? Map(IEnumerable<VacationRequest>? entities)
    {
        if (entities == null) return null;
        return entities.Select(e => Map(e)!);
    }
}