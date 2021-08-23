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
    [Authorize(Roles = CustomRoles.Admin + "," + CustomRoles.AllMenus)]
    public class MenuLE1Controller : Controller
    {
        private readonly ApplicationDbContext _context;

        public MenuLE1Controller(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MenuLE1
        public async Task<IActionResult> Index()
        {
            return View(await _context.MenuLE1.Include(m => m.MenuLE0).ToListAsync());
        }

        //// GET: MenuLE1/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var menuLE1 = await _context.MenuLE1
        //        .Include(m => m.MenuLE0)
        //        .SingleOrDefaultAsync(m => m.Id == id);
        //    if (menuLE1 == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(menuLE1);
        //}

        // GET: MenuLE1/Create
        public IActionResult Create()
        {
            ViewData["MenuLE0Id"] = new SelectList(_context.MenuLE0, "Id", "M0_Name");
            return View();
        }

        // POST: MenuLE1/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,M1_Name,MenuLE0Id")] MenuLE1 menuLE1)
        {
            if (_context.MenuLE1.Any(a => a.M1_Name == menuLE1.M1_Name))
            {
                ModelState.AddModelError("الاسم مستخدم", "هذا الاسم موجود من قبل اختر اسم اخر");
            }

            if (ModelState.IsValid)
            {
                _context.Add(menuLE1);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MenuLE0Id"] = new SelectList(_context.MenuLE0, "Id", "M0_Name", menuLE1.MenuLE0Id);
            return View(menuLE1);
        }

        // GET: MenuLE1/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menuLE1 = await _context.MenuLE1.SingleOrDefaultAsync(m => m.Id == id);
            if (menuLE1 == null)
            {
                return NotFound();
            }
            ViewData["MenuLE0Id"] = new SelectList(_context.MenuLE0, "Id", "M0_Name", menuLE1.MenuLE0Id);
            return View(menuLE1);
        }

        // POST: MenuLE1/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,M1_Name,MenuLE0Id")] MenuLE1 menuLE1)
        {
            if (id != menuLE1.Id)
            {
                return NotFound();
            }

            if (_context.MenuLE1.Any(a => a.M1_Name == menuLE1.M1_Name))
            {
                ModelState.AddModelError("الاسم مستخدم", "هذا الاسم موجود من قبل اختر اسم اخر");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(menuLE1);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MenuLE1Exists(menuLE1.Id))
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
            ViewData["MenuLE0Id"] = new SelectList(_context.MenuLE0, "Id", "M0_Name", menuLE1.MenuLE0Id);
            return View(menuLE1);
        }

        //// GET: MenuLE1/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var menuLE1 = await _context.MenuLE1
        //        .Include(m => m.MenuLE0)
        //        .SingleOrDefaultAsync(m => m.Id == id);
        //    if (menuLE1 == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(menuLE1);
        //}

        //// POST: MenuLE1/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var menuLE1 = await _context.MenuLE1.SingleOrDefaultAsync(m => m.Id == id);
        //    _context.MenuLE1.Remove(menuLE1);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        private bool MenuLE1Exists(int id)
        {
            return _context.MenuLE1.Any(e => e.Id == id);
        }
    }
}
