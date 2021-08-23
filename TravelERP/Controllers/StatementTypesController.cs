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
    [Authorize(Roles = CustomRoles.Developer)]
    public class StatementTypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StatementTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: StatementTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.StatementTypes.ToListAsync());
        }

        //// GET: StatementTypes/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var statementType = await _context.StatementTypes
        //        .SingleOrDefaultAsync(m => m.Id == id);
        //    if (statementType == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(statementType);
        //}

        // GET: StatementTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: StatementTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NameId,Name")] StatementType statementType)
        {
            if (_context.StatementTypes.Any(a => a.Name == statementType.Name))
            {
                ModelState.AddModelError("الاسم مستخدم", "هذا الاسم موجود من قبل اختر اسم اخر");
            }

            if (ModelState.IsValid)
            {
                _context.Add(statementType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(statementType);
        }

        // GET: StatementTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var statementType = await _context.StatementTypes.SingleOrDefaultAsync(m => m.Id == id);
            if (statementType == null)
            {
                return NotFound();
            }
            return View(statementType);
        }

        // POST: StatementTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NameId,Name")] StatementType statementType)
        {
            if (id != statementType.Id)
            {
                return NotFound();
            }
            if (_context.StatementTypes.Any(a => a.Name == statementType.Name))
            {
                ModelState.AddModelError("الاسم مستخدم", "هذا الاسم موجود من قبل اختر اسم اخر");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(statementType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StatementTypeExists(statementType.Id))
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
            return View(statementType);
        }

        //// GET: StatementTypes/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var statementType = await _context.StatementTypes
        //        .SingleOrDefaultAsync(m => m.Id == id);
        //    if (statementType == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(statementType);
        //}

        //// POST: StatementTypes/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var statementType = await _context.StatementTypes.SingleOrDefaultAsync(m => m.Id == id);
        //    _context.StatementTypes.Remove(statementType);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        private bool StatementTypeExists(int id)
        {
            return _context.StatementTypes.Any(e => e.Id == id);
        }
    }
}
