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
    public class TransportMethodTripsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TransportMethodTripsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TransportMethodTrips
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.TransportMethodTrips.Include(t => t.MenuLE0).Include(t => t.MenuLE1).Include(t => t.MenuLE2);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: TransportMethodTrips/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transportMethodTrip = await _context.TransportMethodTrips
                .Include(t => t.MenuLE0)
                .Include(t => t.MenuLE1)
                .Include(t => t.MenuLE2)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (transportMethodTrip == null)
            {
                return NotFound();
            }

            return View(transportMethodTrip);
        }

        // GET: TransportMethodTrips/Create
        public IActionResult Create()
        {
            ViewData["MenuLE0Id"] = new SelectList(_context.MenuLE0, "Id", "M0_Name");
            return View();
        }

        // POST: TransportMethodTrips/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,MenuLE0Id,MenuLE1Id,MenuLE2Id,BillId,BillIdId,IsBus")] TransportMethodTrip transportMethodTrip)
        {
            if (transportMethodTrip.MenuLE1Id == 0)
            {
                ModelState.AddModelError("ادخل وصف", "من فضلك اختر قيمه من القائمه المنسدله 1");
            }

            if (transportMethodTrip.MenuLE2Id == 0)
            {
                ModelState.AddModelError("ادخل وصف", "من فضلك اختر قيمه من القائمه المنسدله 2");
            }

            if (ModelState.IsValid)
            {
                _context.Add(transportMethodTrip);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MenuLE0Id"] = new SelectList(_context.MenuLE0, "Id", "M0_Name", transportMethodTrip.MenuLE0Id);
            ViewData["MenuLE1Id"] = new SelectList(_context.MenuLE1.Where(a=>a.MenuLE0Id == transportMethodTrip.MenuLE0Id), "Id", "M1_Name", transportMethodTrip.MenuLE1Id);
            ViewData["MenuLE2Id"] = new SelectList(_context.MenuLE2.Where(a => a.MenuLE1Id == transportMethodTrip.MenuLE1Id), "Id", "M2_Name", transportMethodTrip.MenuLE2Id);
            return View(transportMethodTrip);
        }

        // GET: TransportMethodTrips/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transportMethodTrip = await _context.TransportMethodTrips.SingleOrDefaultAsync(m => m.Id == id);
            if (transportMethodTrip == null)
            {
                return NotFound();
            }
            ViewData["MenuLE0Id"] = new SelectList(_context.MenuLE0, "Id", "M0_Name", transportMethodTrip.MenuLE0Id);
            ViewData["MenuLE1Id"] = new SelectList(_context.MenuLE1, "Id", "M1_Name", transportMethodTrip.MenuLE1Id);
            ViewData["MenuLE2Id"] = new SelectList(_context.MenuLE2, "Id", "M2_Name", transportMethodTrip.MenuLE2Id);
            return View(transportMethodTrip);
        }

        // POST: TransportMethodTrips/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MenuLE0Id,MenuLE1Id,MenuLE2Id,BillId,BillIdId,IsBus")] TransportMethodTrip transportMethodTrip)
        {
            if (id != transportMethodTrip.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(transportMethodTrip);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransportMethodTripExists(transportMethodTrip.Id))
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
            ViewData["MenuLE0Id"] = new SelectList(_context.MenuLE0, "Id", "M0_Name", transportMethodTrip.MenuLE0Id);
            ViewData["MenuLE1Id"] = new SelectList(_context.MenuLE1, "Id", "M1_Name", transportMethodTrip.MenuLE1Id);
            ViewData["MenuLE2Id"] = new SelectList(_context.MenuLE2, "Id", "M2_Name", transportMethodTrip.MenuLE2Id);
            return View(transportMethodTrip);
        }

        // GET: TransportMethodTrips/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transportMethodTrip = await _context.TransportMethodTrips
                .Include(t => t.MenuLE0)
                .Include(t => t.MenuLE1)
                .Include(t => t.MenuLE2)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (transportMethodTrip == null)
            {
                return NotFound();
            }

            return View(transportMethodTrip);
        }

        // POST: TransportMethodTrips/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transportMethodTrip = await _context.TransportMethodTrips.SingleOrDefaultAsync(m => m.Id == id);
            _context.TransportMethodTrips.Remove(transportMethodTrip);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TransportMethodTripExists(int id)
        {
            return _context.TransportMethodTrips.Any(e => e.Id == id);
        }
    }
}
