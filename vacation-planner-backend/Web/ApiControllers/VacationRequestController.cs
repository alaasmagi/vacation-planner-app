using Base.Contracts.DTO;
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
            return result;
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

            try
            {
                var mappedRequest = _mapper.Map(vacationRequest);
                await _service.UpdateAsync(id, mappedRequest!);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await VacationRequestEntityExists(id))
                {
                    return NotFound();
                }
                
                throw;
            }

            return NoContent();
        }

        // POST: api/VacationRequest
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<VacationRequestWebDto>> PostVacationRequestEntity(VacationRequestWebDto vacationRequest)
        {
            var mappedRequest = _mapper.Map(vacationRequest);
            await _service.CreateAsync(mappedRequest!);
            
            return CreatedAtAction("GetVacationRequestEntity", new { id = vacationRequest.Id }, mappedRequest);
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

        private async Task<bool> VacationRequestEntityExists(Guid id)
        {
            var response = await _service.ExistsAsync(id);
            return response.Successful;
        }
    }
}
