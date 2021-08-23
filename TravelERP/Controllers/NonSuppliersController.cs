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
    public class NonSuppliersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public NonSuppliersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: NonSuppliers **** القائمه الرئيسيه لفصل الموردين ****
        public async Task<IActionResult> Index()
        {
            var CompanyId =(await _userManager.Users.SingleOrDefaultAsync(a => a.Id == _userManager.GetUserId(User))).CompanyId;
            var applicationDbContext =await (_context.NonSuppliers.Include(n => n.Company).Include(a=>a.CustomerSupplier)
                .Where(a => a.CompanyId == CompanyId)).ToListAsync();
            return View(applicationDbContext);
        }



        // GET: NonSuppliers/Create   ****  إضافه مورد جديد للفصل  ****
        public async Task<IActionResult> Create()
        {
            var list =await (from user in _context.CustomersSuppliers.Where(a => a.CustomerOrSupplier.Name == "مورد او وكيل").Where(a => a.CompanyId == _context.Users.SingleOrDefault(b => b.Id == _userManager.GetUserId(User)).CompanyId)
                        where !_context.NonSuppliers.Any(f => f.CustomerSupplierId == user.Id)
                        select new
                        {
                            user.Id,
                            user.Name
                        }).ToListAsync();

            ViewData["CustomerSupplierId"] = new SelectList(list.OrderBy(a=>a.Name), "Id", "Name");
            return View();
        }

        // POST: NonSuppliers/Create  ****  إضافه مورد جديد للفصل  ****
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CustomerSupplierId")] NonSupplier nonSupplier)
        {
            var CompanyId = (await _userManager.Users.SingleOrDefaultAsync(a => a.Id == _userManager.GetUserId(User))).CompanyId;

            if (ModelState.IsValid)
            {
                nonSupplier.CompanyId = CompanyId;
                _context.Add(nonSupplier);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerSupplierId"] = new SelectList(_context.CustomersSuppliers.Where(a => a.CustomerOrSupplier.Name == "مورد او وكيل").Where(a => a.CompanyId == CompanyId).OrderBy(a => a.Name), "Id", "Name",nonSupplier.CustomerSupplierId);
            return View(nonSupplier);
        }


        // GET: NonSuppliers/Delete/5 ****  الغاء مورد   ****
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nonSupplier = await _context.NonSuppliers
                .Include(n => n.Company)
                .Include(n => n.CustomerSupplier)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (nonSupplier == null)
            {
                return NotFound();
            }

            return View(nonSupplier);
        }

        // POST: NonSuppliers/Delete/5   ****  الغاء مورد   ****
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var nonSupplier = await _context.NonSuppliers.SingleOrDefaultAsync(m => m.Id == id);
            _context.NonSuppliers.Remove(nonSupplier);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NonSupplierExists(int id)
        {
            return _context.NonSuppliers.Any(e => e.Id == id);
        }
    }
}
