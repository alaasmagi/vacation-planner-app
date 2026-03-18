using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DTO.DataAccess;
using DataAccess;

namespace Web.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VacationRequestController : ControllerBase
    {
        private readonly AppDbContext _context;

        public VacationRequestController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/VacationRequest
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VacationRequestEntity>>> GetVacationRequests()
        {
            return await _context.VacationRequests.ToListAsync();
        }

        // GET: api/VacationRequest/5
        [HttpGet("{id}")]
        public async Task<ActionResult<VacationRequestEntity>> GetVacationRequestEntity(Guid id)
        {
            var vacationRequestEntity = await _context.VacationRequests.FindAsync(id);

            if (vacationRequestEntity == null)
            {
                return NotFound();
            }

            return vacationRequestEntity;
        }

        // PUT: api/VacationRequest/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVacationRequestEntity(Guid id, VacationRequestEntity vacationRequestEntity)
        {
            if (id != vacationRequestEntity.Id)
            {
                return BadRequest();
            }

            _context.Entry(vacationRequestEntity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VacationRequestEntityExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/VacationRequest
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<VacationRequestEntity>> PostVacationRequestEntity(VacationRequestEntity vacationRequestEntity)
        {
            _context.VacationRequests.Add(vacationRequestEntity);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVacationRequestEntity", new { id = vacationRequestEntity.Id }, vacationRequestEntity);
        }

        // DELETE: api/VacationRequest/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVacationRequestEntity(Guid id)
        {
            var vacationRequestEntity = await _context.VacationRequests.FindAsync(id);
            if (vacationRequestEntity == null)
            {
                return NotFound();
            }

            _context.VacationRequests.Remove(vacationRequestEntity);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VacationRequestEntityExists(Guid id)
        {
            return _context.VacationRequests.Any(e => e.Id == id);
        }
    }
}
