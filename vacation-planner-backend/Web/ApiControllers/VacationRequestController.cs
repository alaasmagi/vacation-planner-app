using Base.Contracts.DTO;
using Contract.Application;
using Contract.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DTO.Presentation;

namespace Web.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VacationRequestController : ControllerBase
    {
        private readonly IVacationRequestService _service;
        private readonly IMapper<VacationRequestDto, VacationRequestWebDto> _mapper;

        public VacationRequestController(IVacationRequestService service, IMapper<VacationRequestDto, VacationRequestWebDto> mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        // GET: api/VacationRequest
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VacationRequestDto>>> GetVacationRequests()
        {
            var response = await _service.GetAllAsync();

            if (!response.Successful)
            {
                return BadRequest(response.Error);
            }
            
            var result = _mapper.Map(response.Value);
            return Ok(result);
        }

        // GET: api/VacationRequest/ID
        [HttpGet("{id}")]
        public async Task<ActionResult<VacationRequestWebDto?>> GetVacationRequestEntity(Guid id)
        {
            var response = await _service.GetByIdAsync(id);
            
            if (!response.Successful)
            {
                return BadRequest(response.Error);
            }

            var result = _mapper.Map(response.Value);
            return Ok(result);
        }

        // PUT: api/VacationRequest/ID
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVacationRequestEntity(Guid id, VacationRequestWebDto vacationRequest)
        {
            if (id != vacationRequest.Id)
            {
                return BadRequest();
            }

            var mappedRequest = _mapper.Map(vacationRequest);
            var response = await _service.UpdateAsync(id, mappedRequest!, vacationRequest.EmployeeId);
            
            if (!response.Successful)
            {
                return BadRequest(response.Error);
            }

            var result = _mapper.Map(response.Value);
            return Ok(result);
        }

        // POST: api/VacationRequest
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<VacationRequestWebDto>> PostVacationRequestEntity(VacationRequestWebDto vacationRequest)
        {
            var mappedRequest = _mapper.Map(vacationRequest);
            var response = await _service.CreateWithValidationAsync(mappedRequest!, vacationRequest.EmployeeId);
            
            if (!response.Successful)
            {
                return BadRequest(response.Error);
            }
            
            var result = _mapper.Map(response.Value);
            return Ok(result);
        }

        // DELETE: api/VacationRequest/ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVacationRequestEntity(Guid id)
        {
            var response = await _service.RemoveAsync(id);
            
            if (!response.Successful)
            {
                return BadRequest(response.Error);
            }

            return NoContent();
        }
    }
}
