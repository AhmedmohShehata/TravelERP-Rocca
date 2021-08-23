using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TravelERP.Data;
using TravelERP.Models;

namespace TravelERP.Controllers
{
    public class SalaryAttendancesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _usermanager;

        public SalaryAttendancesController(ApplicationDbContext context, UserManager<ApplicationUser> usermanager)
        {
            _context = context;
            _usermanager = usermanager;
        }

        // GET: SalaryAttendances
        public async Task<IActionResult> Index()
        {
            var CompanyId = (await _context.Users.SingleOrDefaultAsync(a => a.Id == _usermanager.GetUserId(User))).CompanyId;

            var applicationDbContext =await _context.salaryAttendances.Include(s => s.Company).Where(a=>a.CompanyId == CompanyId).ToListAsync();
            return View(applicationDbContext);
        }

        // GET: SalaryAttendances/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salaryAttendance = await _context.salaryAttendances
                .Include(s => s.Company)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (salaryAttendance == null)
            {
                return NotFound();
            }

            return View(salaryAttendance);
        }

        // GET: SalaryAttendances/Create
        public IActionResult Create()
        {
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Company_Name");
            return View();
        }

        // POST: SalaryAttendances/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Normal,Actual,Late,Early,Absent,OT,CompanyId")] SalaryAttendance salaryAttendance)
        {
            if (ModelState.IsValid)
            {
                var CompanyId = (await _context.Users.SingleOrDefaultAsync(a => a.Id == _usermanager.GetUserId(User))).CompanyId;

                salaryAttendance.CompanyId = CompanyId;
                _context.Add(salaryAttendance);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(salaryAttendance);
        }

        // GET: SalaryAttendances/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salaryAttendance = await _context.salaryAttendances.SingleOrDefaultAsync(m => m.Id == id);
            if (salaryAttendance == null)
            {
                return NotFound();
            }
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Company_Name", salaryAttendance.CompanyId);
            return View(salaryAttendance);
        }

        // POST: SalaryAttendances/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Normal,Actual,Late,Early,Absent,OT,CompanyId")] SalaryAttendance salaryAttendance)
        {
            if (id != salaryAttendance.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(salaryAttendance);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SalaryAttendanceExists(salaryAttendance.Id))
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
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Company_Name", salaryAttendance.CompanyId);
            return View(salaryAttendance);
        }

        // GET: SalaryAttendances/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salaryAttendance = await _context.salaryAttendances
                .Include(s => s.Company)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (salaryAttendance == null)
            {
                return NotFound();
            }

            return View(salaryAttendance);
        }

        // POST: SalaryAttendances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var salaryAttendance = await _context.salaryAttendances.SingleOrDefaultAsync(m => m.Id == id);
            _context.salaryAttendances.Remove(salaryAttendance);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SalaryAttendanceExists(int id)
        {
            return _context.salaryAttendances.Any(e => e.Id == id);
        }
    }
}
