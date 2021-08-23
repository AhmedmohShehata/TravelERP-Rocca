using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TravelERP.Data;
using TravelERP.Models;

namespace TravelERP.Controllers
{
    public class CustomerOrSuppliersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomerOrSuppliersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CustomerOrSuppliers
        public async Task<IActionResult> Index()
        {
            return View(await _context.CustomerOrSuppliers.ToListAsync());
        }

        // GET: CustomerOrSuppliers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customerOrSupplier = await _context.CustomerOrSuppliers
                .SingleOrDefaultAsync(m => m.Id == id);
            if (customerOrSupplier == null)
            {
                return NotFound();
            }

            return View(customerOrSupplier);
        }

        // GET: CustomerOrSuppliers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CustomerOrSuppliers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] CustomerOrSupplier customerOrSupplier)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customerOrSupplier);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customerOrSupplier);
        }

        // GET: CustomerOrSuppliers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customerOrSupplier = await _context.CustomerOrSuppliers.SingleOrDefaultAsync(m => m.Id == id);
            if (customerOrSupplier == null)
            {
                return NotFound();
            }
            return View(customerOrSupplier);
        }

        // POST: CustomerOrSuppliers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] CustomerOrSupplier customerOrSupplier)
        {
            if (id != customerOrSupplier.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customerOrSupplier);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerOrSupplierExists(customerOrSupplier.Id))
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
            return View(customerOrSupplier);
        }

        // GET: CustomerOrSuppliers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customerOrSupplier = await _context.CustomerOrSuppliers
                .SingleOrDefaultAsync(m => m.Id == id);
            if (customerOrSupplier == null)
            {
                return NotFound();
            }

            return View(customerOrSupplier);
        }

        // POST: CustomerOrSuppliers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customerOrSupplier = await _context.CustomerOrSuppliers.SingleOrDefaultAsync(m => m.Id == id);
            _context.CustomerOrSuppliers.Remove(customerOrSupplier);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerOrSupplierExists(int id)
        {
            return _context.CustomerOrSuppliers.Any(e => e.Id == id);
        }
    }
}
