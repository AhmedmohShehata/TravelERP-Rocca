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
    [Authorize(Roles = CustomRoles.Admin + "," + CustomRoles.BranchManager)]
    public class OpeningBalancesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public OpeningBalancesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: OpeningBalances
        public async Task<IActionResult> Index()
        {
            var CompanyId =(await _userManager.Users.SingleOrDefaultAsync(a => a.Id == _userManager.GetUserId(User))).CompanyId;

            var applicationDbContext =await _context.OpeningBalances.Include(o => o.CustomerOrSupplier).Include(o => o.CustomerSupplier).Include(o => o.MenuLE0).Include(a=>a.StatementType).Include(a=>a.Company).Where(A => A.CompanyID == CompanyId).ToListAsync();
            return View( applicationDbContext);
        }

        // GET: OpeningBalances/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var openingBalance = await _context.OpeningBalances
        //        .Include(o => o.CustomerOrSupplier)
        //        .Include(o => o.CustomerSupplier)
        //        .Include(o => o.MenuLE0)
        //        .Include(a => a.StatementType)
        //        .SingleOrDefaultAsync(m => m.Id == id);
        //    if (openingBalance == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(openingBalance);
        //}

        // GET: OpeningBalances/Create
        public IActionResult Create()
        {
            ViewData["CustomerOrSupplierId"] = new SelectList(_context.CustomerOrSuppliers, "Id", "Name");
            ViewData["CustomerSupplierId"] = new SelectList(_context.CustomersSuppliers.Where(a => a.CustomerOrSupplier.Name == "مورد او وكيل").Where(a => a.CompanyId == _context.Users.SingleOrDefault(b => b.Id == _userManager.GetUserId(User)).CompanyId).OrderBy(a => a.Name), "Id", "Name");
            ViewData["MenuLE0Id"] = new SelectList(_context.MenuLE0, "Id", "M0_Name");
            ViewData["StatementTypeId"] = new SelectList(_context.StatementTypes, "Id", "Name");

            return View();
            
        }

        // POST: OpeningBalances/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CustomerOrSupplierId,CustomerSupplierId,MenuLE0Id,StatementTypeId,Balance")] OpeningBalance openingBalance)
        {
            openingBalance.CustomerOrSupplierId = 2;
            openingBalance.CompanyID =(await _userManager.Users.SingleOrDefaultAsync(a => a.Id == _userManager.GetUserId(User))).CompanyId;
            if (ModelState.IsValid)
            {
                _context.Add(openingBalance);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerOrSupplierId"] = new SelectList(_context.CustomerOrSuppliers, "Id", "Name", openingBalance.CustomerOrSupplierId);
            ViewData["CustomerSupplierId"] = new SelectList(_context.CustomersSuppliers, "Id", "Name", openingBalance.CustomerSupplierId);
            ViewData["MenuLE0Id"] = new SelectList(_context.MenuLE0, "Id", "M0_Name", openingBalance.MenuLE0Id);
            ViewData["StatementTypeId"] = new SelectList(_context.StatementTypes, "Id", "Name" , openingBalance.StatementTypeId);

            return View(openingBalance);
        }

        //// GET: OpeningBalances/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var openingBalance = await _context.OpeningBalances.SingleOrDefaultAsync(m => m.Id == id);
        //    if (openingBalance == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["CustomerOrSupplierId"] = new SelectList(_context.CustomerOrSuppliers, "Id", "Name", openingBalance.CustomerOrSupplierId);
        //    ViewData["CustomerSupplierId"] = new SelectList(_context.CustomersSuppliers, "Id", "Name", openingBalance.CustomerSupplierId);
        //    ViewData["MenuLE0Id"] = new SelectList(_context.MenuLE0, "Id", "M0_Name", openingBalance.MenuLE0Id);
        //    return View(openingBalance);
        //}

        //// POST: OpeningBalances/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,CustomerOrSupplierId,CustomerSupplierId,MenuLE0Id,Balance")] OpeningBalance openingBalance)
        //{
        //    if (id != openingBalance.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(openingBalance);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!OpeningBalanceExists(openingBalance.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["CustomerOrSupplierId"] = new SelectList(_context.CustomerOrSuppliers, "Id", "Name", openingBalance.CustomerOrSupplierId);
        //    ViewData["CustomerSupplierId"] = new SelectList(_context.CustomersSuppliers, "Id", "Name", openingBalance.CustomerSupplierId);
        //    ViewData["MenuLE0Id"] = new SelectList(_context.MenuLE0, "Id", "M0_Name", openingBalance.MenuLE0Id);
        //    return View(openingBalance);
        //}

        // GET: OpeningBalances/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var openingBalance = await _context.OpeningBalances
                .Include(o => o.CustomerOrSupplier)
                .Include(o => o.CustomerSupplier)
                .Include(o => o.MenuLE0)
                .Include(a => a.StatementType)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (openingBalance == null)
            {
                return NotFound();
            }

            return View(openingBalance);
        }

        // POST: OpeningBalances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var openingBalance = await _context.OpeningBalances.SingleOrDefaultAsync(m => m.Id == id);
            _context.OpeningBalances.Remove(openingBalance);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OpeningBalanceExists(int id)
        {
            return _context.OpeningBalances.Any(e => e.Id == id);
        }
    }
}
