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
    public class SalaryAddandCutsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public SalaryAddandCutsController(ApplicationDbContext context ,UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: SalaryAddandCuts
        public async Task<IActionResult> Index()
        {
            return View(await _context.salaryAddandCuts.ToListAsync());
        }

        // GET: SalaryAddandCuts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salaryAddandCut = await _context.salaryAddandCuts
                .SingleOrDefaultAsync(m => m.Id == id);
            if (salaryAddandCut == null)
            {
                return NotFound();
            }

            return View(salaryAddandCut);
        }

        // GET: SalaryAddandCuts/Create
        public async Task<IActionResult> Create()
        {
            var CompanyId =(await _context.Users.SingleOrDefaultAsync(a => a.Id == _userManager.GetUserId(User))).CompanyId;

            ViewData["EMP_Name"] = new SelectList(_context.salaryDatas.Where(x => x.CompanyId == CompanyId).OrderBy(a => a.EMP_Name), "EMP_Name", "EMP_Name");

            return View();
        }

        // POST: SalaryAddandCuts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EMP_Name,FromDate,ToDate,Omra_Add,Bonus_Add,Whatsapp_Cut,Other_Cut")] SalaryAddandCut salaryAddandCut)
        {
            var CompanyId = (await _context.Users.SingleOrDefaultAsync(a => a.Id == _userManager.GetUserId(User))).CompanyId;
            if (ModelState.IsValid)
            {
                salaryAddandCut.CompanyId = CompanyId;
                _context.Add(salaryAddandCut);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EMP_Name"] = new SelectList(_context.salaryDatas.Where(x => x.CompanyId == CompanyId).OrderBy(a => a.EMP_Name), "EMP_Name", "EMP_Name",salaryAddandCut.EMP_Name);

            return View(salaryAddandCut);
        }

        // GET: SalaryAddandCuts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salaryAddandCut = await _context.salaryAddandCuts.SingleOrDefaultAsync(m => m.Id == id);
            if (salaryAddandCut == null)
            {
                return NotFound();
            }
            ViewData["EMP_Name"] = new SelectList(_context.salaryDatas.Where(x => x.CompanyId == _context.Users.SingleOrDefault(w => w.Id == _userManager.GetUserId(User)).CompanyId).OrderBy(a => a.EMP_Name), "EMP_Name", "EMP_Name");

            return View(salaryAddandCut);
        }

        // POST: SalaryAddandCuts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EMP_Name,FromDate,ToDate,Omra_Add,Bonus_Add,Whatsapp_Cut,Other_Cut,CustomerId")] SalaryAddandCut salaryAddandCut)
        {
            if (id != salaryAddandCut.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(salaryAddandCut);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SalaryAddandCutExists(salaryAddandCut.Id))
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
            var CompanyId = (await _context.Users.SingleOrDefaultAsync(a => a.Id == _userManager.GetUserId(User))).CompanyId;
            ViewData["EMP_Name"] = new SelectList(_context.salaryDatas.Where(x => x.CompanyId == CompanyId).OrderBy(a => a.EMP_Name), "EMP_Name", "EMP_Name", salaryAddandCut.EMP_Name);
            return View(salaryAddandCut);
        }

        // GET: SalaryAddandCuts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salaryAddandCut = await _context.salaryAddandCuts
                .SingleOrDefaultAsync(m => m.Id == id);
            if (salaryAddandCut == null)
            {
                return NotFound();
            }

            return View(salaryAddandCut);
        }

        // POST: SalaryAddandCuts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var salaryAddandCut = await _context.salaryAddandCuts.SingleOrDefaultAsync(m => m.Id == id);
            _context.salaryAddandCuts.Remove(salaryAddandCut);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SalaryAddandCutExists(int id)
        {
            return _context.salaryAddandCuts.Any(e => e.Id == id);
        }
    }
}
