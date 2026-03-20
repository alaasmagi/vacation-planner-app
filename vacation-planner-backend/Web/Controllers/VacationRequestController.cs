using Contract.Application;
using Contract.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DTO.DataAccess;
using DTO.Presentation;

namespace Web.Controllers
{
    public class VacationRequestController : Controller
    {
        private readonly IVacationRequestService _service;

        public VacationRequestController(IVacationRequestService service)
        {
            _service = service;
        }

        // GET: VacationRequest
        public async Task<IActionResult> Index()
        {
            var response = await _service.GetAllAsync();
            return View(response.Value);
        }

        // GET: VacationRequest/Details/ID
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response = await _service.GetByIdAsync(id.Value);
            if (!response.Successful)
            {
                return BadRequest(response.Error);
            }

            return View(response.Value);
        }

        // GET: VacationRequest/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: VacationRequest/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmployeeId,StartDate,EndDate,Comment,Status,CreatedBy,CreatedAt,UpdatedBy,UpdatedAt,Id")] VacationRequestDto vacationRequest)
        {
            if (ModelState.IsValid)
            {
                vacationRequest.Id = Guid.NewGuid();
                await _service.CreateAsync(vacationRequest, vacationRequest.EmployeeId);
                return RedirectToAction(nameof(Index));
            }
            return View(vacationRequest);
        }

        // GET: VacationRequest/Edit/ID
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response = await _service.GetByIdAsync(id.Value);
            if (!response.Successful)
            {
                return BadRequest(response.Error);
            }
            return View(response.Value);
        }

        // POST: VacationRequest/Edit/ID
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("EmployeeId,StartDate,EndDate,Comment,Status,CreatedBy,CreatedAt,UpdatedBy,UpdatedAt,Id")] VacationRequestDto vacationRequest)
        {
            if (id != vacationRequest.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _service.UpdateAsync(id, vacationRequest, vacationRequest.EmployeeId);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await VacationRequestEntityExists(vacationRequest.Id))
                    {
                        return NotFound();
                    }

                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(vacationRequest);
        }

        // GET: VacationRequest/Delete/ID
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response = await _service.GetByIdAsync(id.Value);
            if (!response.Successful)
            {
                return BadRequest(response.Error);
            }

            return View(response.Value);
        }

        // POST: VacationRequest/Delete/ID
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var response = await _service.GetByIdAsync(id);
            
            if (response.Successful)
            {
                await _service.RemoveAsync(id);
            }
            
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> VacationRequestEntityExists(Guid id)
        {
            var response = await _service.ExistsAsync(id);
            return response.Successful;
        }
    }
}
