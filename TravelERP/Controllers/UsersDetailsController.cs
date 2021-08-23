using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TravelERP.Data;
using TravelERP.Models;

namespace TravelERP.Controllers
{
    public class UsersDetailsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _usermanager;

        public UsersDetailsController(ApplicationDbContext context, UserManager<ApplicationUser> usermanager)
        {
            _context = context;
            _usermanager = usermanager;
        }

        // GET: UsersDetails
        public async Task<IActionResult> Index()
        {
            var CompanyId =(await _context.Users.SingleOrDefaultAsync(a => a.Id == _usermanager.GetUserId(User))).CompanyId;
            var applicationDbContext =await _context.UsersDetails.Include(u => u.User).Where(a => a.CompanyID == CompanyId).ToListAsync();
            return View( applicationDbContext);
        }

        // GET: UsersDetails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usersDetail = await _context.UsersDetails
                .Include(u => u.User)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (usersDetail == null)
            {
                return NotFound();
            }

            return View(usersDetail);
        }

        // GET: UsersDetails/Create
        public async Task<IActionResult> Create()
        {
            var CompanyId =(await _context.Users.SingleOrDefaultAsync(a => a.Id == _usermanager.GetUserId(User))).CompanyId;
            ViewData["UserId"] = new SelectList(_context.Users.Where(a=>a.CompanyId == CompanyId), "Id", "UserName");
            return View();
        }

        // POST: UsersDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,Commission")] UsersDetail usersDetail)
        {
            var CompanyId = (await _context.Users.SingleOrDefaultAsync(a => a.Id == _usermanager.GetUserId(User))).CompanyId;

            if (ModelState.IsValid)
            {
                usersDetail.CompanyID = CompanyId;
                _context.Add(usersDetail);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users.Where(a => a.CompanyId == CompanyId), "Id", "UserName", usersDetail.UserId);
            return View(usersDetail);
        }

        // GET: UsersDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var CompanyId = (await _context.Users.SingleOrDefaultAsync(a => a.Id == _usermanager.GetUserId(User))).CompanyId;
            if (id == null)
            {
                return NotFound();
            }

            var usersDetail = await _context.UsersDetails.SingleOrDefaultAsync(m => m.Id == id);
            if (usersDetail == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users.Where(a => a.CompanyId == CompanyId), "Id", "UserName", usersDetail.UserId);
            return View(usersDetail);
        }

        // POST: UsersDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,Commission,CompanyID")] UsersDetail usersDetail)
        {
            var CompanyId = (await _context.Users.SingleOrDefaultAsync(a => a.Id == _usermanager.GetUserId(User))).CompanyId;

            if (id != usersDetail.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usersDetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsersDetailExists(usersDetail.Id))
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
            ViewData["UserId"] = new SelectList(_context.Users.Where(a => a.CompanyId == CompanyId), "Id", "UserName", usersDetail.UserId);
            return View(usersDetail);
        }

        // GET: UsersDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usersDetail = await _context.UsersDetails
                .Include(u => u.User)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (usersDetail == null)
            {
                return NotFound();
            }

            return View(usersDetail);
        }

        // POST: UsersDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usersDetail = await _context.UsersDetails.SingleOrDefaultAsync(m => m.Id == id);
            _context.UsersDetails.Remove(usersDetail);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsersDetailExists(int id)
        {
            return _context.UsersDetails.Any(e => e.Id == id);
        }
    }
}
