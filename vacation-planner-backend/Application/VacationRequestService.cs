using DTO.Presentation;
using Base.Application;
using Base.Contracts.DataAccess;
using Base.Contracts.DTO;
using Contract.Application;
using Contract.DataAccess;
using Domain;

namespace Application;

public class VacationRequestService : BaseService<VacationRequestDto, VacationRequest, IVacationRequestRepository>, IVacationRequestService
{
    public VacationRequestService(IBaseUow serviceUow, IVacationRequestRepository serviceRepository, IMapper<VacationRequestDto, VacationRequest> serviceMapper) : base(serviceUow, serviceRepository, serviceMapper)
    {
    }
}