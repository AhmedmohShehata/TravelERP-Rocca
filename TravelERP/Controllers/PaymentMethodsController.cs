using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TravelERP.Data;
using TravelERP.Models;
using static TravelERP.Controllers.AccountController;

namespace TravelERP.Controllers
{
    [Authorize(Roles = CustomRoles.Admin + "," + CustomRoles.BranchManager)]
    public class PaymentMethodsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PaymentMethodsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PaymentMethods
        public async Task<IActionResult> Index()
        {
            return View(await _context.paymentMethods.ToListAsync());
        }

        //// GET: PaymentMethods/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var paymentMethod = await _context.paymentMethods
        //        .SingleOrDefaultAsync(m => m.Id == id);
        //    if (paymentMethod == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(paymentMethod);
        //}

        // GET: PaymentMethods/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PaymentMethods/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] PaymentMethod paymentMethod)
        {
            if (_context.paymentMethods.Any(a => a.Name == paymentMethod.Name))
            {
                ModelState.AddModelError("الاسم مستخدم", "هذا الاسم موجود من قبل اختر اسم اخر");
            }

            if (ModelState.IsValid)
            {
                _context.Add(paymentMethod);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(paymentMethod);
        }

        // GET: PaymentMethods/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paymentMethod = await _context.paymentMethods.SingleOrDefaultAsync(m => m.Id == id);
            if (paymentMethod == null)
            {
                return NotFound();
            }
            return View(paymentMethod);
        }

        // POST: PaymentMethods/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] PaymentMethod paymentMethod)
        {
            if (id != paymentMethod.Id)
            {
                return NotFound();
            }
            if (_context.paymentMethods.Any(a => a.Name == paymentMethod.Name))
            {
                ModelState.AddModelError("الاسم مستخدم", "هذا الاسم موجود من قبل اختر اسم اخر");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(paymentMethod);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PaymentMethodExists(paymentMethod.Id))
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
            return View(paymentMethod);
        }

        //// GET: PaymentMethods/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var paymentMethod = await _context.paymentMethods
        //        .SingleOrDefaultAsync(m => m.Id == id);
        //    if (paymentMethod == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(paymentMethod);
        //}

        //// POST: PaymentMethods/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var paymentMethod = await _context.paymentMethods.SingleOrDefaultAsync(m => m.Id == id);
        //    _context.paymentMethods.Remove(paymentMethod);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        private bool PaymentMethodExists(int id)
        {
            return _context.paymentMethods.Any(e => e.Id == id);
        }
    }
}
