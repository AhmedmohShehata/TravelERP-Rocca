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
    [Authorize(Roles = CustomRoles.Admin + "," + CustomRoles.BranchManager)]
    public class TransportMethodsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TransportMethodsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TransportMethods
        public async Task<IActionResult> Index()
        {
            return View(await _context.TransportMethods.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        // POST: TransportMethods/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] TransportMethod transportMethod)
        {
            if (_context.TransportMethods.Any(a => a.Name == transportMethod.Name))
            {
                ModelState.AddModelError("الاسم مستخدم", "هذا الاسم موجود من قبل اختر اسم اخر");
            }

            if (ModelState.IsValid)
            {
                _context.Add(transportMethod);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(transportMethod);
        }

        // GET: TransportMethods/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transportMethod = await _context.TransportMethods.SingleOrDefaultAsync(m => m.Id == id);
            if (transportMethod == null)
            {
                return NotFound();
            }
            return View(transportMethod);
        }

        // POST: TransportMethods/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] TransportMethod transportMethod)
        {
            if (id != transportMethod.Id)
            {
                return NotFound();
            }
            if (_context.TransportMethods.Any(a => a.Name == transportMethod.Name))
            {
                ModelState.AddModelError("الاسم مستخدم", "هذا الاسم موجود من قبل اختر اسم اخر");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(transportMethod);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransportMethodExists(transportMethod.Id))
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
            return View(transportMethod);
        }


        private bool TransportMethodExists(int id)
        {
            return _context.TransportMethods.Any(e => e.Id == id);
        }
    }
}
