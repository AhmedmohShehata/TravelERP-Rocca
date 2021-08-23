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
    public class DatesSearchesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DatesSearchesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DatesSearches
        public async Task<IActionResult> Index()
        {
            return View(await _context.DatesSearches.ToListAsync());
        }

        // GET: DatesSearches/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var datesSearch = await _context.DatesSearches
                .SingleOrDefaultAsync(m => m.Id == id);
            if (datesSearch == null)
            {
                return NotFound();
            }

            return View(datesSearch);
        }



        // GET: EsalDate
        public IActionResult EsalDate()
        {
            return View();
        }

        // Post: EsalDate





        // GET: DatesSearches/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DatesSearches/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EsalDate([Bind("Id,StartDate,EndDate")] DatesSearch datesSearch)
        {
            if (ModelState.IsValid)
            {
                TempData["StartDate"] = datesSearch.StartDate.Value.Date.ToString();
                TempData["EndDate"] = datesSearch.EndDate.Value.Date.ToString();

                //_context.Add(datesSearch);
                await _context.SaveChangesAsync();
                return RedirectToAction("PrintEsalByDate","Esals");
            }
            return View(datesSearch);
        }

        public IActionResult StatementDate()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> StatementDate([Bind("StartDate")] DatesSearch datesSearch)
        {
            if (ModelState.IsValid)
            {
                TempData["StartDate"] = datesSearch.StartDate.Value.Date.ToString();

                //_context.Add(datesSearch);
                await _context.SaveChangesAsync();
                return RedirectToAction("DailyStatement", "Transactions");
            }
            return View(datesSearch);
        }

        // GET: DatesSearches/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var datesSearch = await _context.DatesSearches.SingleOrDefaultAsync(m => m.Id == id);
            if (datesSearch == null)
            {
                return NotFound();
            }
            return View(datesSearch);
        }

        // POST: DatesSearches/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StartDate,EndDate")] DatesSearch datesSearch)
        {
            if (id != datesSearch.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(datesSearch);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DatesSearchExists(datesSearch.Id))
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
            return View(datesSearch);
        }

        // GET: DatesSearches/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var datesSearch = await _context.DatesSearches
                .SingleOrDefaultAsync(m => m.Id == id);
            if (datesSearch == null)
            {
                return NotFound();
            }

            return View(datesSearch);
        }

        // POST: DatesSearches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var datesSearch = await _context.DatesSearches.SingleOrDefaultAsync(m => m.Id == id);
            _context.DatesSearches.Remove(datesSearch);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DatesSearchExists(int id)
        {
            return _context.DatesSearches.Any(e => e.Id == id);
        }
    }
}
