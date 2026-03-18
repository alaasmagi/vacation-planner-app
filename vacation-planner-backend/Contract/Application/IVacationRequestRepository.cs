using Base.Contracts.DataAccess;
using Domain;

namespace Contract.Application;

public interface IVacationRequestRepository : IBaseRepository<VacationRequest>
{
}