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
    public class MenuLE0Controller : Controller
    {
        private readonly ApplicationDbContext _context;

        public MenuLE0Controller(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MenuLE0
        public async Task<IActionResult> Index()
        {
            return View(await _context.MenuLE0.ToListAsync());
        }

        //// GET: MenuLE0/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var menuLE0 = await _context.MenuLE0
        //        .SingleOrDefaultAsync(m => m.Id == id);
        //    if (menuLE0 == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(menuLE0);
        //}

        // GET: MenuLE0/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MenuLE0/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,M0_Name")] MenuLE0 menuLE0)
        {
            if (_context.MenuLE0.Any(a => a.M0_Name == menuLE0.M0_Name))
            {
                ModelState.AddModelError("الاسم مستخدم", "هذا الاسم موجود من قبل اختر اسم اخر");
            }

            if (ModelState.IsValid)
            {
                _context.Add(menuLE0);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(menuLE0);
        }

        //// GET: MenuLE0/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var menuLE0 = await _context.MenuLE0.SingleOrDefaultAsync(m => m.Id == id);
        //    if (menuLE0 == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(menuLE0);
        //}

        //// POST: MenuLE0/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,M0_Name")] MenuLE0 menuLE0)
        //{
        //    if (id != menuLE0.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(menuLE0);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!MenuLE0Exists(menuLE0.Id))
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
        //    return View(menuLE0);
        //}

        //// GET: MenuLE0/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var menuLE0 = await _context.MenuLE0
        //        .SingleOrDefaultAsync(m => m.Id == id);
        //    if (menuLE0 == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(menuLE0);
        //}

        //// POST: MenuLE0/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var menuLE0 = await _context.MenuLE0.SingleOrDefaultAsync(m => m.Id == id);
        //    _context.MenuLE0.Remove(menuLE0);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        private bool MenuLE0Exists(int id)
        {
            return _context.MenuLE0.Any(e => e.Id == id);
        }
    }
}
