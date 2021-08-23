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
    public class MenuLZ1Controller : Controller
    {
        private readonly ApplicationDbContext _context;

        public MenuLZ1Controller(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MenuLZ1
        public async Task<IActionResult> Index()
        {
            return View(await _context.MenuLZ1.Include(m => m.MenuLZ0).ToListAsync());
        }

        //// GET: MenuLZ1/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var menuLZ1 = await _context.MenuLZ1
        //        .Include(m => m.MenuLZ0)
        //        .SingleOrDefaultAsync(m => m.Id == id);
        //    if (menuLZ1 == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(menuLZ1);
        //}

        // GET: MenuLZ1/Create
        public IActionResult Create()
        {
            ViewData["MenuLZ0Id"] = new SelectList(_context.MenuLZ0, "Id", "M0_Name");
            return View();
        }

        // POST: MenuLZ1/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,M1_Name,MenuLZ0Id")] MenuLZ1 menuLZ1)
        {
            if (_context.MenuLZ1.Any(a => a.M1_Name == menuLZ1.M1_Name))
            {
                ModelState.AddModelError("الاسم مستخدم", "هذا الاسم موجود من قبل اختر اسم اخر");
            }
            if (ModelState.IsValid)
            {
                _context.Add(menuLZ1);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MenuLZ0Id"] = new SelectList(_context.MenuLZ0, "Id", "M0_Name", menuLZ1.MenuLZ0Id);
            return View(menuLZ1);
        }

        // GET: MenuLZ1/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menuLZ1 = await _context.MenuLZ1.SingleOrDefaultAsync(m => m.Id == id);
            if (menuLZ1 == null)
            {
                return NotFound();
            }
            ViewData["MenuLZ0Id"] = new SelectList(_context.MenuLZ0, "Id", "M0_Name", menuLZ1.MenuLZ0Id);
            return View(menuLZ1);
        }

        // POST: MenuLZ1/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,M1_Name,MenuLZ0Id")] MenuLZ1 menuLZ1)
        {
            if (id != menuLZ1.Id)
            {
                return NotFound();
            }
            if ( _context.MenuLZ1.Any(a => a.M1_Name == menuLZ1.M1_Name))
            {
                ModelState.AddModelError("الاسم مستخدم", "هذا الاسم موجود من قبل اختر اسم اخر");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(menuLZ1);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MenuLZ1Exists(menuLZ1.Id))
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
            ViewData["MenuLZ0Id"] = new SelectList(_context.MenuLZ0, "Id", "M0_Name", menuLZ1.MenuLZ0Id);
            return View(menuLZ1);
        }

        //// GET: MenuLZ1/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var menuLZ1 = await _context.MenuLZ1
        //        .Include(m => m.MenuLZ0)
        //        .SingleOrDefaultAsync(m => m.Id == id);
        //    if (menuLZ1 == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(menuLZ1);
        //}

        //// POST: MenuLZ1/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var menuLZ1 = await _context.MenuLZ1.SingleOrDefaultAsync(m => m.Id == id);
        //    _context.MenuLZ1.Remove(menuLZ1);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        private bool MenuLZ1Exists(int id)
        {
            return _context.MenuLZ1.Any(e => e.Id == id);
        }
    }
}
