using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TravelERP.Data;
using TravelERP.Models;
using static TravelERP.Controllers.AccountController;

namespace TravelERP.Controllers
{
    [Authorize]

    public class CustomerSuppliersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;


        public CustomerSuppliersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;

        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////// CustomerSuppliers/Index **** القوائم - العملاء والموردين **** ///////////////////
        public IActionResult Index()
        {
            //var applicationDbContext = _context.CustomersSuppliers.Include(c => c.CustomerOrSupplier).Where(A => A.CompanyId == _userManager.Users.SingleOrDefault(a => a.Id == _userManager.GetUserId(User)).CompanyId);
            return View();
        }


        ///////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////// CustomerSuppliers/Details **** القوائم - العملاء والموردين - تفاصيل **** /////////////
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customerSupplier = await _context.CustomersSuppliers
                .Include(c => c.CustomerOrSupplier)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (customerSupplier == null)
            {
                return NotFound();
            }

            return View(customerSupplier);
        }


        ///////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////// CustomerSuppliers/Create **** القوائم - العملاء والموردين - إنشاء **** ////////////////
        public IActionResult Create()
        {
            ViewData["CustomerOrSupplierId"] = new SelectList(_context.CustomerOrSuppliers, "Id", "Name");
            ViewData["CustomerId"] = new SelectList(_context.CustomerOrSuppliers.Where(a => a.Id == 1), "Id", "Name");

            return View();
        }
        // POST: CustomerSuppliers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,PhoneNumber1,PhoneNumber2,Email,Adrress,CustomerOrSupplierId,PassportNo,PassportExDate")] CustomerSupplier customerSupplier)
        {
            if (ModelState.IsValid)
            {
                customerSupplier.CompanyId =(await _userManager.Users.SingleOrDefaultAsync(a => a.Id == _userManager.GetUserId(User))).CompanyId;

                _context.Add(customerSupplier);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerOrSupplierId"] = new SelectList(_context.CustomerOrSuppliers, "Id", "Name", customerSupplier.CustomerOrSupplierId);

            ViewData["CustomerId"] = new SelectList(_context.CustomerOrSuppliers.Where(a=>a.Id==1), "Id", "Name", customerSupplier.CustomerOrSupplierId);

            return View(customerSupplier);
        }



        //////////////////////////////////////////////////////////////////////////////////////////////////
        //////////// CustomerSuppliers/Edit **** القوائم - العملاء والموردين - إنشاء **** /////////////
        [Authorize(Roles = CustomRoles.Edits)]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customerSupplier = await _context.CustomersSuppliers.SingleOrDefaultAsync(m => m.Id == id);
            if (customerSupplier == null)
            {
                return NotFound();
            }
            ViewData["CustomerOrSupplierId"] = new SelectList(_context.CustomerOrSuppliers, "Id", "Name", customerSupplier.CustomerOrSupplierId);
            return View(customerSupplier);
        }

        [Authorize(Roles = CustomRoles.Edits)]
        // POST: CustomerSuppliers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,PhoneNumber1,PhoneNumber2,Email,Adrress,PassportNo,PassportExDate,CustomerOrSupplierId,CompanyId")] CustomerSupplier customerSupplier)
        {
            if (id != customerSupplier.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customerSupplier);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerSupplierExists(customerSupplier.Id))
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
            ViewData["CustomerOrSupplierId"] = new SelectList(_context.CustomerOrSuppliers, "Id", "Name", customerSupplier.CustomerOrSupplierId);
            return View(customerSupplier);
        }


        //////////////////////////////////////////////////////////////////////////////////////////////////
        //////////// CustomerSuppliers/Delete **** القوائم - العملاء والموردين - حذف **** /////////////
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var CustomerSupplier = await _context.CustomersSuppliers
                .Include(u => u.CustomerOrSupplier)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (CustomerSupplier == null)
            {
                return NotFound();
            }

            return View(CustomerSupplier);
        }

        // POST: UsersDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customersuppier = await _context.CustomersSuppliers.SingleOrDefaultAsync(m => m.Id == id);
            _context.CustomersSuppliers.Remove(customersuppier);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



        private bool CustomerSupplierExists(int id)
        {
            return _context.CustomersSuppliers.Any(e => e.Id == id);
        }
        [HttpGet]
        public async Task<JsonResult> GetCustomerSuppliers(int id)

        {
            var CompanyId =(await _userManager.Users.SingleOrDefaultAsync(a => a.Id == _userManager.GetUserId(User))).CompanyId;

            var customersSuppliers =await ((from user in _context.CustomersSuppliers.Where(a => a.CustomerOrSupplier.Id == id).Where(a => a.CompanyId == CompanyId)
                                      select new CustomerSupplier
                                      {
                                          Id = user.Id,
                                          Name = user.Name,
                                      }).OrderBy(a => a.Name)).ToListAsync();


            List<CustomerSupplier> customersupplier = new List<CustomerSupplier>();
            customersupplier = customersSuppliers;
            //customersupplier.Insert(0, new CustomerSupplier { Id = 0, Name = "من فضلك اختر من القائمه ..." });
            return Json(new SelectList(customersupplier, "Id", "Name"));
        }

        public async Task<JsonResult> GetCustomerSuppliersOutNON(int id)

        {
            var CompanyId =(await _userManager.Users.SingleOrDefaultAsync(a => a.Id == _userManager.GetUserId(User))).CompanyId;

            var customersSuppliers =await ((from user in _context.CustomersSuppliers.Where(a => a.CustomerOrSupplier.Id == id).Where(a => a.CompanyId == CompanyId)
                                      where !_context.NonSuppliers.Any(f => f.CustomerSupplierId == user.Id)
                                      select new CustomerSupplier
                                      {
                                          Id = user.Id,
                                          Name = user.Name,
                                      }).OrderBy(a => a.Name)).ToListAsync();


            List<CustomerSupplier> customersupplier = new List<CustomerSupplier>();
            customersupplier = customersSuppliers;
            //customersupplier.Insert(0, new CustomerSupplier { Id = 0, Name = "من فضلك اختر من القائمه ..." });
            return Json(new SelectList(customersupplier, "Id", "Name"));
        }

    }
}
