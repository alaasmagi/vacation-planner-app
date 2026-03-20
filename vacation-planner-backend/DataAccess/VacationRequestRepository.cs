using Base.Contracts.DTO;
using Base.DataAccess.EF;
using Base.DTO;
using Contract.DataAccess;
using Domain;
using DTO.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace DataAccess; 

public class VacationRequestRepository : BaseRepository<VacationRequest, VacationRequestEntity, IMapper<VacationRequest, VacationRequestEntity>>, IVacationRequestRepository
{
    public VacationRequestRepository(AppDbContext repositoryDbContext, IMapper<VacationRequest, VacationRequestEntity> repositoryMapper) 
        : base(repositoryDbContext, repositoryMapper)
    {
    }

    public async Task<IMethodResponse<bool>> ExistsAsync(Guid employeeId, DateOnly startDate, DateOnly endDate)
    {
        var exists = await GetQuery().AnyAsync(vr => vr.EmployeeId == employeeId &&
                                                       vr.StartDate <= endDate &&
                                                       vr.EndDate >= startDate);
        return MethodResponse<bool>.Success(exists);
    }
}