using DTO.Presentation;
using Base.Application;
using Base.Contracts.DataAccess;
using Base.Contracts.DTO;
using Base.DTO;
using Contract.Application;
using Contract.DataAccess;
using Domain;
using DTO.Error;

namespace Application;

public class VacationRequestService : BaseService<VacationRequestDto, VacationRequest, IVacationRequestRepository>, IVacationRequestService
{
    private readonly IVacationRequestRepository _repository;
    public VacationRequestService(IBaseUow serviceUow, IVacationRequestRepository serviceRepository, IMapper<VacationRequestDto, VacationRequest> serviceMapper) : base(serviceUow, serviceRepository, serviceMapper)
    {
        _repository = serviceRepository;
    }

    public async Task<IMethodResponse<VacationRequestDto>> CreateWithValidationAsync(VacationRequestDto dto)
    {
        var exists = await _repository.ExistsAsync(dto.EmployeeId, dto.StartDate, dto.EndDate);
        
        if (exists.Value)
        {
            return MethodResponse<VacationRequestDto>.Failure(CreateError(VacationRequestErrors.Codes.DuplicateEntry,
                VacationRequestErrors.Messages.DuplicateEntry));
        }

        return await base.CreateAsync(dto);
    }
}