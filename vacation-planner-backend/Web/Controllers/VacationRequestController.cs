using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DTO.DataAccess;
using DataAccess;

namespace Web.Controllers
{
    public class VacationRequestController : Controller
    {
        private readonly AppDbContext _context;

        public VacationRequestController(AppDbContext context)
        {
            _context = context;
        }

        // GET: VacationRequest
        public async Task<IActionResult> Index()
        {
            return View(await _context.VacationRequests.ToListAsync());
        }

        // GET: VacationRequest/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vacationRequestEntity = await _context.VacationRequests
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vacationRequestEntity == null)
            {
                return NotFound();
            }

            return View(vacationRequestEntity);
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
        public async Task<IActionResult> Create([Bind("EmployeeId,StartDate,EndDate,Comment,Status,CreatedBy,CreatedAt,UpdatedBy,UpdatedAt,Id")] VacationRequestEntity vacationRequestEntity)
        {
            if (ModelState.IsValid)
            {
                vacationRequestEntity.Id = Guid.NewGuid();
                _context.Add(vacationRequestEntity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vacationRequestEntity);
        }

        // GET: VacationRequest/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vacationRequestEntity = await _context.VacationRequests.FindAsync(id);
            if (vacationRequestEntity == null)
            {
                return NotFound();
            }
            return View(vacationRequestEntity);
        }

        // POST: VacationRequest/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("EmployeeId,StartDate,EndDate,Comment,Status,CreatedBy,CreatedAt,UpdatedBy,UpdatedAt,Id")] VacationRequestEntity vacationRequestEntity)
        {
            if (id != vacationRequestEntity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vacationRequestEntity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VacationRequestEntityExists(vacationRequestEntity.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(vacationRequestEntity);
        }

        // GET: VacationRequest/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vacationRequestEntity = await _context.VacationRequests
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vacationRequestEntity == null)
            {
                return NotFound();
            }

            return View(vacationRequestEntity);
        }

        // POST: VacationRequest/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var vacationRequestEntity = await _context.VacationRequests.FindAsync(id);
            if (vacationRequestEntity != null)
            {
                _context.VacationRequests.Remove(vacationRequestEntity);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VacationRequestEntityExists(Guid id)
        {
            return _context.VacationRequests.Any(e => e.Id == id);
        }
    }
}
