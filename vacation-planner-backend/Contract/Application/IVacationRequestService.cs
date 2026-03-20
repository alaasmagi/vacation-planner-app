using Base.Contracts.Application;
using Base.Contracts.DTO;
using DTO.Presentation;

namespace Contract.Application;

public interface IVacationRequestService : IBaseService<VacationRequestDto>
{
    Task<IMethodResponse<VacationRequestDto>> CreateWithValidationAsync(VacationRequestDto dto, Guid? actor = default!);
}