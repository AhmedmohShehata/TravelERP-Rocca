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
    public class MenuLE2Controller : Controller
    {
        private readonly ApplicationDbContext _context;

        public MenuLE2Controller(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MenuLE2
        public async Task<IActionResult> Index()
        {
            return View(await _context.MenuLE2.Include(m => m.MenuLE0).Include(m => m.MenuLE1).ToListAsync());
        }


        // GET: MenuLE2/Create
        public IActionResult Create()
        {
            ViewData["MenuLE0Id"] = new SelectList(_context.MenuLE0, "Id", "M0_Name");
            ViewData["MenuLE1Id"] = new SelectList(_context.MenuLE1, "Id", "M1_Name");
            return View();
        }

        // POST: MenuLE2/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,M2_Name,MenuLE1Id,MenuLE0Id")] MenuLE2 menuLE2)
        {
            //if (_context.MenuLE2.Any(a => a.M2_Name == menuLE2.M2_Name))
            //{
            //    ModelState.AddModelError("الاسم مستخدم", "هذا الاسم موجود من قبل اختر اسم اخر");
            //}
            if (ModelState.IsValid)
            {

                _context.Add(menuLE2);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["MenuLE0Id"] = new SelectList(_context.MenuLE0, "Id", "M0_Name");
            ViewData["MenuLE1Id"] = new SelectList(_context.MenuLE1, "Id", "M1_Name");
            return View(menuLE2);
        }

        // GET: MenuLE2/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menuLE2 = await _context.MenuLE2.SingleOrDefaultAsync(m => m.Id == id);
            if (menuLE2 == null)
            {
                return NotFound();
            }
            ViewData["MenuLE0Id"] = new SelectList(_context.MenuLE0, "Id", "M0_Name", menuLE2.MenuLE0Id);
            ViewData["MenuLE1Id"] = new SelectList(_context.MenuLE1, "Id", "M1_Name", menuLE2.MenuLE1Id);
            return View(menuLE2);
        }

        // POST: MenuLE2/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,M2_Name,MenuLE1Id,MenuLE0Id")] MenuLE2 menuLE2)
        {
            if (id != menuLE2.Id)
            {
                return NotFound();
            }
            if (_context.MenuLE2.Any(a => a.M2_Name == menuLE2.M2_Name))
            {
                ModelState.AddModelError("الاسم مستخدم", "هذا الاسم موجود من قبل اختر اسم اخر");
            }


            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(menuLE2);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MenuLE2Exists(menuLE2.Id))
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
            ViewData["MenuLE0Id"] = new SelectList(_context.MenuLE0, "Id", "M0_Name", menuLE2.MenuLE0Id);
            ViewData["MenuLE1Id"] = new SelectList(_context.MenuLE1, "Id", "M1_Name", menuLE2.MenuLE1Id);
            return View(menuLE2);
        }

        //// GET: MenuLE2/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var menuLE2 = await _context.MenuLE2
        //        .Include(m => m.MenuLE0)
        //        .Include(m => m.MenuLE1)
        //        .SingleOrDefaultAsync(m => m.Id == id);
        //    if (menuLE2 == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(menuLE2);
        //}

        //// POST: MenuLE2/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var menuLE2 = await _context.MenuLE2.SingleOrDefaultAsync(m => m.Id == id);
        //    _context.MenuLE2.Remove(menuLE2);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        private bool MenuLE2Exists(int id)
        {
            return _context.MenuLE2.Any(e => e.Id == id);
        }
        [Authorize]
        public async Task<JsonResult> GetMenuLE1(int id)

        {
            List<MenuLE1> menuLE1 = new List<MenuLE1>();
            menuLE1 =await _context.MenuLE1.Where(a => a.MenuLE0.Id == id).ToListAsync();
            menuLE1.Insert(0, new MenuLE1 { Id = 0, M1_Name = "من فضلك اختر من القائمه ..." });
            return Json(new SelectList(menuLE1, "Id", "M1_Name"));
        }
        [Authorize]
        public async Task<JsonResult> GetMenuLE1Religious(int id)

        {
            List<MenuLE1> menuLE1 = new List<MenuLE1>();
            menuLE1 =await _context.MenuLE1.Where(a => a.MenuLE0.Id == id).Where(a=>a.MenuLE0.M0_Name== "سياحة دينية").ToListAsync();
            menuLE1.Insert(0, new MenuLE1 { Id = 0, M1_Name = "من فضلك اختر من القائمه ..." });
            return Json(new SelectList(menuLE1, "Id", "M1_Name"));
        }

        [Authorize]
        public async Task<JsonResult> GetMenuLE2(int id)

        {
            List<MenuLE2> menuLE2 = new List<MenuLE2>();
            menuLE2 =await _context.MenuLE2.Where(a => a.MenuLE1.Id == id).ToListAsync();
            menuLE2.Insert(0, new MenuLE2 { Id = 0, M2_Name = "من فضلك اختر من القائمه ..." });
            return Json(new SelectList(menuLE2, "Id", "M2_Name"));
        }


    }
}
