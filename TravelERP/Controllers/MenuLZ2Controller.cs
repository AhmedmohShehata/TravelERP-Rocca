using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TravelERP.Data;
using TravelERP.Models;
using static TravelERP.Controllers.AccountController;

namespace TravelERP.Controllers
{
    [Authorize(Roles = CustomRoles.Admin + "," + CustomRoles.AllMenus)]
    public class MenuLZ2Controller : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;


        public MenuLZ2Controller(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: MenuLZ2
        public async Task<IActionResult> Index()
        {
            return View(await _context.MenuLZ2.Include(m => m.MenuLZ0).Include(m => m.MenuLZ1).ToListAsync());
        }

        //// GET: MenuLZ2/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var menuLZ2 = await _context.MenuLZ2
        //        .Include(m => m.MenuLZ0)
        //        .Include(m => m.MenuLZ1)
        //        .SingleOrDefaultAsync(m => m.Id == id);
        //    if (menuLZ2 == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(menuLZ2);
        //}

        // GET: MenuLZ2/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.MenuLZ0 =await _context.MenuLZ0.ToListAsync();
            ViewData["MenuLZ0Id"] = new SelectList(_context.MenuLZ0, "Id", "M0_Name");
            ViewData["MenuLZ1Id"] = new SelectList(_context.MenuLZ1, "Id", "M1_Name");
            return View();
        }

        // POST: MenuLZ2/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,M2_Name,MenuLZ1Id,MenuLZ0Id")] MenuLZ2 menuLZ2)
        {
            //if ( menuLZ2.MenuLZ1Id==0)
            //{
            //    ModelState.AddModelError("الاسم مستخدم", "هذا الاسم موجود من قبل اختر اسم اخر");
            //}
            if (ModelState.IsValid)
            {
                _context.Add(menuLZ2);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MenuLZ0Id"] = new SelectList(_context.MenuLZ0, "Id", "M0_Name", menuLZ2.MenuLZ0Id);
            ViewData["MenuLZ1Id"] = new SelectList(_context.MenuLZ1, "Id", "M1_Name", menuLZ2.MenuLZ1Id);
            return View(menuLZ2);
        }

        // GET: MenuLZ2/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menuLZ2 = await _context.MenuLZ2.SingleOrDefaultAsync(m => m.Id == id);
            if (menuLZ2 == null)
            {
                return NotFound();
            }
            ViewData["MenuLZ0Id"] = new SelectList(_context.MenuLZ0, "Id", "M0_Name", menuLZ2.MenuLZ0Id);
            ViewData["MenuLZ1Id"] = new SelectList(_context.MenuLZ1, "Id", "M1_Name", menuLZ2.MenuLZ1Id);
            return View(menuLZ2);
        }

        // POST: MenuLZ2/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,M2_Name,MenuLZ1Id,MenuLZ0Id")] MenuLZ2 menuLZ2)
        {
            if (id != menuLZ2.Id)
            {
                return NotFound();
            }
            //if (_context.MenuLZ2.Any(a => a.M2_Name == menuLZ2.M2_Name) || menuLZ2.MenuLZ1Id == 0)
            //{
            //    ModelState.AddModelError("الاسم مستخدم", "هذا الاسم موجود من قبل اختر اسم اخر");
            //}
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(menuLZ2);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MenuLZ2Exists(menuLZ2.Id))
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
            ViewData["MenuLZ0Id"] = new SelectList(_context.MenuLZ0, "Id", "M0_Name", menuLZ2.MenuLZ0Id);
            ViewData["MenuLZ1Id"] = new SelectList(_context.MenuLZ1, "Id", "M1_Name", menuLZ2.MenuLZ1Id);
            return View(menuLZ2);
        }

        // GET: MenuLZ2/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var menuLZ2 = await _context.MenuLZ2
        //        .Include(m => m.MenuLZ0)
        //        .Include(m => m.MenuLZ1)
        //        .SingleOrDefaultAsync(m => m.Id == id);
        //    if (menuLZ2 == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(menuLZ2);
        //}

        //// POST: MenuLZ2/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var menuLZ2 = await _context.MenuLZ2.SingleOrDefaultAsync(m => m.Id == id);
        //    _context.MenuLZ2.Remove(menuLZ2);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        private bool MenuLZ2Exists(int id)
        {
            return _context.MenuLZ2.Any(e => e.Id == id);
        }
        [Authorize]
        public async Task<JsonResult> GetMenuLZ1(int id)

        {
            List<MenuLZ1> menuLZ1 = new List<MenuLZ1>();
            menuLZ1 =await _context.MenuLZ1.Where(a => a.MenuLZ0.Id == id).ToListAsync();
            menuLZ1.Insert(0, new MenuLZ1 { Id = 0, M1_Name = "من فضلك اختر من القائمه ..." });
            return Json(new SelectList(menuLZ1, "Id", "M1_Name"));
        }
        [Authorize]
        public async Task<JsonResult> GetMenuLZ2(int id)

        {
            List<MenuLZ2> menuLZ2 = new List<MenuLZ2>();
            //var companyId = _userManager.Users.SingleOrDefault(a => a.Id == _userManager.GetUserId(User)).CompanyId;
            menuLZ2 =await _context.MenuLZ2.Where(a => a.MenuLZ1.Id == id).ToListAsync();
            menuLZ2.Insert(0, new MenuLZ2 { Id = 0, M2_Name = "من فضلك اختر من القائمه ..." });
            return Json(new SelectList(menuLZ2, "Id", "M2_Name"));
        }


    }
}
