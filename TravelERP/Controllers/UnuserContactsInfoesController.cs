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
    [Authorize(Roles = CustomRoles.Admin)]

    public class UnuserContactsInfoesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UnuserContactsInfoesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: UnuserContactsInfoes
        public async Task<IActionResult> Index()
        {
            return View(await _context.UnuserContactsInfos.ToListAsync());
        }
        [AllowAnonymous]
        public IActionResult Success()
        {
            return View();
        }


        // GET: UnuserContactsInfoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var unuserContactsInfo = await _context.UnuserContactsInfos
                .SingleOrDefaultAsync(m => m.Id == id);
            if (unuserContactsInfo == null)
            {
                return NotFound();
            }

            return View(unuserContactsInfo);
        }
        [AllowAnonymous]
        // GET: UnuserContactsInfoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UnuserContactsInfoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Email,PhoneNumber,subscribe")] UnuserContactsInfo unuserContactsInfo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(unuserContactsInfo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Success));
            }
            return View(unuserContactsInfo);
        }

        //// GET: UnuserContactsInfoes/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var unuserContactsInfo = await _context.UnuserContactsInfos.SingleOrDefaultAsync(m => m.Id == id);
        //    if (unuserContactsInfo == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(unuserContactsInfo);
        //}

        //// POST: UnuserContactsInfoes/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Email,PhoneNumber,subscribe")] UnuserContactsInfo unuserContactsInfo)
        //{
        //    if (id != unuserContactsInfo.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(unuserContactsInfo);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!UnuserContactsInfoExists(unuserContactsInfo.Id))
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
        //    return View(unuserContactsInfo);
        //}

        //// GET: UnuserContactsInfoes/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var unuserContactsInfo = await _context.UnuserContactsInfos
        //        .SingleOrDefaultAsync(m => m.Id == id);
        //    if (unuserContactsInfo == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(unuserContactsInfo);
        //}

        //// POST: UnuserContactsInfoes/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var unuserContactsInfo = await _context.UnuserContactsInfos.SingleOrDefaultAsync(m => m.Id == id);
        //    _context.UnuserContactsInfos.Remove(unuserContactsInfo);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        private bool UnuserContactsInfoExists(int id)
        {
            return _context.UnuserContactsInfos.Any(e => e.Id == id);
        }
    }
}
