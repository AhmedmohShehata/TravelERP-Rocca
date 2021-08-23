using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TravelERP.Data;
using TravelERP.Models;
using static TravelERP.Controllers.AccountController;

namespace TravelERP.Controllers
{
    [Authorize(Roles = CustomRoles.Admin + "," + CustomRoles.Salary)]

    public class SalaryDatasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _usermangar;
        public SalaryDatasController(ApplicationDbContext context ,UserManager<ApplicationUser> usermangar)
        {
            _context = context;
            _usermangar = usermangar;
        }
        // GET: SalaryDatas
        public async Task<IActionResult> Index()
        {
            var CompanyId = (await _context.Users.SingleOrDefaultAsync(a => a.Id == _usermangar.GetUserId(User))).CompanyId;

            var applicationDbContext =await _context.salaryDatas.Include(s => s.User).Where(x => x.CompanyId == CompanyId).ToListAsync();
            return View( applicationDbContext);
        }

        // GET: SalaryDatas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salaryData = await _context.salaryDatas
                .Include(s => s.User)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (salaryData == null)
            {
                return NotFound();
            }

            return View(salaryData);
        }

        // GET: SalaryDatas/Create
        public async Task<IActionResult> Create()
        {
            var CompanyId = (await _context.Users.SingleOrDefaultAsync(a => a.Id == _usermangar.GetUserId(User))).CompanyId;

            ViewData["EMP_FingerPrintName"] = new SelectList(_context.salaryAttendances.Where(x => x.CompanyId == CompanyId).OrderBy(a=>a.Name), "Name", "Name");
            ViewData["UserId"] = new SelectList(_context.Users.Where(x=>x.CompanyId ==CompanyId).OrderBy(a=>a.UserName), "Id", "UserName");
            ViewData["EMP_Name"] = new SelectList(_context.MenuLZ2.Where(a=>a.MenuLZ1.M1_Name == "سلفيات موظفين").OrderBy(a=>a.M2_Name), "M2_Name", "M2_Name");

            return View();
        }

        // POST: SalaryDatas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,EMP_FingerPrintName,EMP_Name,EMP_Salary,EMP_OverTime,EMP_Early,EMP_Late,EMP_Absent,EMP_WhatsApp,EMP_insurance")] SalaryData salaryData)
        {
            if (ModelState.IsValid)
            {
                salaryData.CompanyId =(await _context.Users.SingleOrDefaultAsync(a => a.Id == _usermangar.GetUserId(User))).CompanyId;
                _context.Add(salaryData);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", salaryData.UserId);
            return View(salaryData);
        }

        // GET: SalaryDatas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var CompanyId = (await _context.Users.SingleOrDefaultAsync(a => a.Id == _usermangar.GetUserId(User))).CompanyId;

            if (id == null)
            {
                return NotFound();
            }

            var salaryData = await _context.salaryDatas.SingleOrDefaultAsync(m => m.Id == id);
            if (salaryData == null)
            {
                return NotFound();
            }
            ViewData["EMP_FingerPrintName"] = new SelectList(_context.salaryAttendances.Where(x => x.CompanyId == CompanyId).OrderBy(a => a.Name), "Name", "Name");
            ViewData["UserId"] = new SelectList(_context.Users.Where(x => x.CompanyId == CompanyId).OrderBy(a => a.UserName), "Id", "UserName");
            ViewData["EMP_Name"] = new SelectList(_context.MenuLZ2.Where(a => a.MenuLZ1.M1_Name == "سلفيات موظفين").OrderBy(a => a.M2_Name), "M2_Name", "M2_Name");
            return View(salaryData);
        }

        // POST: SalaryDatas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,EMP_FingerPrintName,EMP_Name,EMP_Salary,EMP_OverTime,EMP_Early,EMP_Late,EMP_Absent,EMP_WhatsApp,EMP_insurance,CompanyId")] SalaryData salaryData)
        {
            var CompanyId = (await _context.Users.SingleOrDefaultAsync(a => a.Id == _usermangar.GetUserId(User))).CompanyId;

            if (id != salaryData.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(salaryData);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SalaryDataExists(salaryData.Id))
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
            ViewData["EMP_FingerPrintName"] = new SelectList(_context.salaryAttendances.Where(x => x.CompanyId == CompanyId).OrderBy(a => a.Name), "Name", "Name",salaryData.EMP_FingerPrintName);
            ViewData["UserId"] = new SelectList(_context.Users.Where(x => x.CompanyId == CompanyId).OrderBy(a => a.UserName), "Id", "UserName",salaryData.UserId);
            ViewData["EMP_Name"] = new SelectList(_context.MenuLZ2.Where(a => a.MenuLZ1.M1_Name == "سلفيات موظفين").OrderBy(a => a.M2_Name), "M2_Name", "M2_Name",salaryData.EMP_Name);

            return View(salaryData);
        }

        // GET: SalaryDatas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salaryData = await _context.salaryDatas
                .Include(s => s.User)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (salaryData == null)
            {
                return NotFound();
            }

            return View(salaryData);
        }

        // POST: SalaryDatas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var salaryData = await _context.salaryDatas.SingleOrDefaultAsync(m => m.Id == id);
            _context.salaryDatas.Remove(salaryData);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SalaryDataExists(int id)
        {
            return _context.salaryDatas.Any(e => e.Id == id);
        }

        // تقرير مبيعات للموظفين
        public IActionResult SalaryReportDate()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> SalaryReportDate([Bind("StartDate,EndDate")] DatesSearch datesSearch)
        {
            if (ModelState.IsValid)
            {
                TempData["StartDate"] = datesSearch.StartDate.Value.Date.ToString();
                TempData["EndDate"] = datesSearch.EndDate.Value.Date.ToString();

                //_context.Add(datesSearch);
                await _context.SaveChangesAsync();
                return RedirectToAction("SalaryReport", "SalaryDatas");
            }
            return View(datesSearch);
        }


        public async Task<ActionResult> SalaryReport()
        {
            var Outbut =await (from x in _context.salaryAddandCuts.Where(F => F.CompanyId == _context.Users.SingleOrDefault(w => w.Id == _usermangar.GetUserId(User)).CompanyId) select new Models.ViewModel.SalaryReportViewModel { EMP_Name = x.EMP_Name, Omra_Add = x.Omra_Add, Bonus_Add = x.Bonus_Add, Whatsapp_Cut = x.Whatsapp_Cut, Other_Cut = x.Other_Cut })
                 .Concat(await( from x in _context.salaryAttendances.Where(F => F.CompanyId == _context.Users.SingleOrDefault(w => w.Id == _usermangar.GetUserId(User)).CompanyId)
                         select new Models.ViewModel.SalaryReportViewModel
                         {
                             EMP_Name = _context.salaryDatas.Where(f => f.CompanyId == _context.Users.SingleOrDefault(w => w.Id == _usermangar.GetUserId(User)).CompanyId).SingleOrDefault(a => a.EMP_FingerPrintName == x.Name).EMP_Name,
                             EMP_OverTime = ((float)x.OT / 60) * (_context.salaryDatas.Where(f => f.CompanyId == _context.Users.SingleOrDefault(w => w.Id == _usermangar.GetUserId(User)).CompanyId).SingleOrDefault(a => a.EMP_FingerPrintName == x.Name).EMP_OverTime),
                             EMP_Late = ((float)x.Late / 60) * (_context.salaryDatas.Where(f => f.CompanyId == _context.Users.SingleOrDefault(w => w.Id == _usermangar.GetUserId(User)).CompanyId).SingleOrDefault(a => a.EMP_FingerPrintName == x.Name).EMP_Late),
                             EMP_Early = ((float)x.Early) / 60 * _context.salaryDatas.Where(f => f.CompanyId == _context.Users.SingleOrDefault(w => w.Id == _usermangar.GetUserId(User)).CompanyId).SingleOrDefault(a => a.EMP_FingerPrintName == x.Name).EMP_Early,
                             EMP_Absent = x.Absent * _context.salaryDatas.Where(f => f.CompanyId == _context.Users.SingleOrDefault(w => w.Id == _usermangar.GetUserId(User)).CompanyId).SingleOrDefault(a => a.EMP_FingerPrintName == x.Name).EMP_Absent
                         }).ToListAsync())
                 .Concat(await( from x in _context.salaryDatas.Where(f => f.CompanyId == _context.Users.SingleOrDefault(w => w.Id == _usermangar.GetUserId(User)).CompanyId) select new Models.ViewModel.SalaryReportViewModel { EMP_Name = x.EMP_Name, EMP_Salary = x.EMP_Salary, EMP_WhatsApp = x.EMP_WhatsApp, EMP_insurance = x.EMP_insurance }).ToListAsync())
                 .Concat(await (from x in _context.BillAirLines.Where(f => f.CompanyID == _context.Users.SingleOrDefault(w => w.Id == _usermangar.GetUserId(User)).CompanyId).Where(d => d.BillDate.Date >= Convert.ToDateTime(TempData["StartDate"])).Where(d => d.BillDate.Date <= Convert.ToDateTime(TempData["EndDate"])) select new Models.ViewModel.SalaryReportViewModel { EMP_Name = _context.salaryDatas.Where(f => f.CompanyId == _context.Users.SingleOrDefault(w => w.Id == _usermangar.GetUserId(User)).CompanyId).SingleOrDefault(a => a.UserId == x.UserId).EMP_Name, Commission = x.EMPCommission }).ToListAsync())
                 .Concat(await (from x in _context.BillVisas.Where(f => f.CompanyID == _context.Users.SingleOrDefault(w => w.Id == _usermangar.GetUserId(User)).CompanyId).Where(a=>a.BillState == true).Where(d => d.ApprovedDate.Date >= Convert.ToDateTime(TempData["StartDate"])).Where(d => d.ApprovedDate.Date <= Convert.ToDateTime(TempData["EndDate"])) select new Models.ViewModel.SalaryReportViewModel { EMP_Name = _context.salaryDatas.Where(f => f.CompanyId == _context.Users.SingleOrDefault(w => w.Id == _usermangar.GetUserId(User)).CompanyId).SingleOrDefault(a => a.UserId == x.UserId).EMP_Name, Commission = x.EMPCommission }).ToListAsync())
                 .Concat(await (from x in _context.BillDomestic.Where(f => f.CompanyID == _context.Users.SingleOrDefault(w => w.Id == _usermangar.GetUserId(User)).CompanyId).Where(d => d.BillDate.Date >= Convert.ToDateTime(TempData["StartDate"])).Where(d => d.BillDate.Date <= Convert.ToDateTime(TempData["EndDate"])) select new Models.ViewModel.SalaryReportViewModel { EMP_Name = _context.salaryDatas.Where(f => f.CompanyId == _context.Users.SingleOrDefault(w => w.Id == _usermangar.GetUserId(User)).CompanyId).SingleOrDefault(a => a.UserId == x.UserId).EMP_Name, Commission = x.EMPCommission }).ToListAsync())
                 .Concat(await (from x in _context.Ezns.Where(f => f.CompanyID == _context.Users.SingleOrDefault(w => w.Id == _usermangar.GetUserId(User)).CompanyId).Where(d => d.EznDate.Date >= Convert.ToDateTime(TempData["StartDate"])).Where(d => d.EznDate.Date <= Convert.ToDateTime(TempData["EndDate"])).Include(a => a.MenuLZ1).Include(a => a.MenuLZ2).Where(a => a.MenuLZ1.M1_Name == "سلفيات موظفين") select new Models.ViewModel.SalaryReportViewModel { EMP_Name = x.MenuLZ2.M2_Name, loans = x.AmountWithdrawn }).ToListAsync())

                 .OrderByDescending(p => p.EMP_Name)
                 .GroupBy(x => x.EMP_Name)
                 .Select(a =>
                     new Models.ViewModel.SalaryReportViewModel
                     {
                         EMP_Name = a.Key,
                         EMP_Salary = a.Sum(x => x.EMP_Salary),
                         EMP_WhatsApp = a.Sum(x => x.EMP_WhatsApp),
                         EMP_OverTime = a.Sum(x => x.EMP_OverTime),
                         Commission = a.Sum(x => x.Commission),
                         Omra_Add = a.Sum(x => x.Omra_Add),
                         Bonus_Add = a.Sum(x => x.Bonus_Add),
                         EMP_Late = a.Sum(x => x.EMP_Late),
                         EMP_Early = a.Sum(x => x.EMP_Early),
                         EMP_Absent = a.Sum(x => x.EMP_Absent),
                         Whatsapp_Cut = a.Sum(x => x.Whatsapp_Cut),
                         EMP_insurance = a.Sum(x => x.EMP_insurance),
                         loans = a.Sum(x => x.loans),
                         Other_Cut = a.Sum(x => x.Other_Cut)
                     }).ToListAsync();
            var list =await (from x in _context.salaryDatas.Where(a => a.CompanyId == _context.Users.SingleOrDefault(d => d.Id == _usermangar.GetUserId(User)).CompanyId)
                        select new Models.ViewModel.SalaryReportViewModel {
                EMP_Name = Outbut.SingleOrDefault(a => a.EMP_Name == x.EMP_Name).EMP_Name,
                Bonus_Add = Outbut.SingleOrDefault(a => a.EMP_Name == x.EMP_Name).Bonus_Add,
                Omra_Add = Outbut.SingleOrDefault(a => a.EMP_Name == x.EMP_Name).Omra_Add,
                Other_Cut = Outbut.SingleOrDefault(a => a.EMP_Name == x.EMP_Name).Other_Cut,
                Commission = Outbut.SingleOrDefault(a=>a.EMP_Name == x.EMP_Name).Commission,
                EMP_Absent = Outbut.SingleOrDefault(a => a.EMP_Name == x.EMP_Name).EMP_Absent,
                EMP_Early = Outbut.SingleOrDefault(a => a.EMP_Name == x.EMP_Name).EMP_Early,
                EMP_OverTime = Outbut.SingleOrDefault(a => a.EMP_Name == x.EMP_Name).EMP_OverTime,
                EMP_insurance = Outbut.SingleOrDefault(a => a.EMP_Name == x.EMP_Name).EMP_insurance,
                EMP_Late = Outbut.SingleOrDefault(a => a.EMP_Name == x.EMP_Name).EMP_Late,
                EMP_Salary = Outbut.SingleOrDefault(a => a.EMP_Name == x.EMP_Name).EMP_Salary,
                loans = Outbut.SingleOrDefault(a => a.EMP_Name == x.EMP_Name).loans,
                EMP_WhatsApp = Outbut.SingleOrDefault(a => a.EMP_Name == x.EMP_Name).EMP_WhatsApp,
                Whatsapp_Cut = Outbut.SingleOrDefault(a => a.EMP_Name == x.EMP_Name).Whatsapp_Cut,

            }).ToListAsync();

            return View(list);
        }



    }
}
