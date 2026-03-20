using Base.Contracts.DataAccess;
using Base.Contracts.DTO;
using Domain;

namespace Contract.DataAccess;

public interface IVacationRequestRepository : IBaseRepository<VacationRequest>
{
    Task<IMethodResponse<bool>> ExistsAsync(Guid employeeId, DateOnly startDate, DateOnly endDate);
}