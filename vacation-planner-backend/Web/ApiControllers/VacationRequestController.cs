using Contract.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DTO.DataAccess;
using DTO.Presentation;

namespace Web.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VacationRequestController : ControllerBase
    {
        private readonly IVacationRequestService _service;

        public VacationRequestController(IVacationRequestService service)
        {
            _service = service;
        }

        // GET: api/VacationRequest
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VacationRequestDto>>> GetVacationRequests()
        {
            var vacationRequests = await _service.GetAllAsync();
            return Ok(vacationRequests);
        }

        // GET: api/VacationRequest/ID
        [HttpGet("{id}")]
        public async Task<ActionResult<VacationRequestDto>> GetVacationRequestEntity(Guid id)
        {
            var vacationRequest = await _service.GetByIdAsync(id);

            if (vacationRequest == null)
            {
                return NotFound();
            }

            return vacationRequest;
        }

        // PUT: api/VacationRequest/ID
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVacationRequestEntity(Guid id, VacationRequestDto vacationRequest)
        {
            if (id != vacationRequest.Id)
            {
                return BadRequest();
            }

            try
            {
                await _service.UpdateAsync(id, vacationRequest);
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
        public async Task<ActionResult<VacationRequestEntity>> PostVacationRequestEntity(VacationRequestDto vacationRequest)
        {
            await _service.CreateAsync(vacationRequest);
            
            return CreatedAtAction("GetVacationRequestEntity", new { id = vacationRequest.Id }, vacationRequest);
        }

        // DELETE: api/VacationRequest/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVacationRequestEntity(Guid id)
        {
            if (await _service.RemoveAsync(id))
            {
                return NotFound();
            }

            return NoContent();
        }

        private async Task<bool> VacationRequestEntityExists(Guid id)
        {
            return await _service.ExistsAsync(id);
        }
    }
}
