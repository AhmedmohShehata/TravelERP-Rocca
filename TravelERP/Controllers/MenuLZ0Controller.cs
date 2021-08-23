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
    public class MenuLZ0Controller : Controller
    {
        private readonly ApplicationDbContext _context;

        public MenuLZ0Controller(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MenuLZ0
        public async Task<IActionResult> Index()
        {
            return View(await _context.MenuLZ0.ToListAsync());
        }

        //// GET: MenuLZ0/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var menuLZ0 = await _context.MenuLZ0
        //        .SingleOrDefaultAsync(m => m.Id == id);
        //    if (menuLZ0 == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(menuLZ0);
        //}

        // GET: MenuLZ0/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MenuLZ0/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,M0_Name")] MenuLZ0 menuLZ0)
        {
            if (_context.MenuLZ0.Any(a => a.M0_Name == menuLZ0.M0_Name))
            {
                ModelState.AddModelError("الاسم مستخدم", "هذا الاسم موجود من قبل اختر اسم اخر");
            }

            if (ModelState.IsValid)
            {
                _context.Add(menuLZ0);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(menuLZ0);
        }

        //// GET: MenuLZ0/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var menuLZ0 = await _context.MenuLZ0.SingleOrDefaultAsync(m => m.Id == id);
        //    if (menuLZ0 == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(menuLZ0);
        //}

        //// POST: MenuLZ0/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,M0_Name")] MenuLZ0 menuLZ0)
        //{
        //    if (id != menuLZ0.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(menuLZ0);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!MenuLZ0Exists(menuLZ0.Id))
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
        //    return View(menuLZ0);
        //}

        //// GET: MenuLZ0/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var menuLZ0 = await _context.MenuLZ0
        //        .SingleOrDefaultAsync(m => m.Id == id);
        //    if (menuLZ0 == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(menuLZ0);
        //}

        //// POST: MenuLZ0/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var menuLZ0 = await _context.MenuLZ0.SingleOrDefaultAsync(m => m.Id == id);
        //    _context.MenuLZ0.Remove(menuLZ0);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        private bool MenuLZ0Exists(int id)
        {
            return _context.MenuLZ0.Any(e => e.Id == id);
        }
    }
}
