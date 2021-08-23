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
    [Authorize(Roles = CustomRoles.Admin + "," + CustomRoles.BranchManager + "," + CustomRoles.Ezns)]
    public class EznForEsalsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;


        public EznForEsalsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;

        }

        //// GET: EznForEsals
        //public async Task<IActionResult> Index()
        //{
        //    var applicationDbContext = _context.EznsForEsals.Include(e => e.Company).Include(e => e.MenuLE0).Include(e => e.MenuLE1).Include(e => e.MenuLE2).Include(e => e.PaymentMethod).Include(e => e.User).Include(e=>e.CustomerOrSupplier).Include(e=>e.CustomerSupplier);
        //    return View(await applicationDbContext.ToListAsync());
        //}

        //// GET: EznForEsals/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var eznForEsal = await _context.EznsForEsals
        //        .Include(e => e.Company)
        //        .Include(e => e.MenuLE0)
        //        .Include(e => e.MenuLE1)
        //        .Include(e => e.MenuLE2)
        //        .Include(e => e.PaymentMethod)
        //        .Include(e => e.User)
        //        .SingleOrDefaultAsync(m => m.Id == id);
        //    if (eznForEsal == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(eznForEsal);
        //}

        // GET: EznForEsals/Create
        public IActionResult Create()
        {
            ViewData["CompanyID"] = new SelectList(_context.Companies, "Id", "Company_Name");
            ViewData["MenuLE0Id"] = new SelectList(_context.MenuLE0, "Id", "M0_Name");
            ViewData["MenuLE1Id"] = new SelectList(_context.MenuLE1, "Id", "M1_Name");
            ViewData["MenuLE2Id"] = new SelectList(_context.MenuLE2, "Id", "M2_Name");
            ViewData["PaymentMethodId"] = new SelectList(_context.paymentMethods, "Id", "Name");
            ViewData["CustomerOrSupplierId"] = new SelectList(_context.CustomerOrSuppliers, "Id", "Name");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: EznForEsals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EznId,EznDate,Name,MenuLE0Id,MenuLE1Id,MenuLE2Id,ExpenseName,AmountWithdrawn,PaymentMethodId,CustomerOrSupplierId,CustomerSupplierId")] Ezn eznForEsal)
        {
            if (ModelState.IsValid)
            {
                if (_context.Ezns.Where(a => a.CompanyID == _context.Users.SingleOrDefault(b => b.Id == _userManager.GetUserId(User)).CompanyId).Count() == 0)
                {
                    eznForEsal.EznId = 1;
                }
                else
                {
                   eznForEsal.EznId = _context.Ezns.Where(a => a.CompanyID == _context.Users.SingleOrDefault(b => b.Id == _userManager.GetUserId(User)).CompanyId).Max(a => a.EznId) + 1;

                }

                //if (_context.Ezns.Where(a => a.CompanyID == _context.Users.SingleOrDefault(b => b.Id == _userManager.GetUserId(User)).CompanyId).Count() == 0 & _context.EznsForEsals.Where(a => a.CompanyID == _context.Users.SingleOrDefault(b => b.Id == _userManager.GetUserId(User)).CompanyId).Count() == 0)
                //{
                //    eznForEsal.EznId = 1;
                //}
                //else
                //{
                //    if (_context.Ezns.Where(a => a.CompanyID == _context.Users.SingleOrDefault(b => b.Id == _userManager.GetUserId(User)).CompanyId).Count() == 0)
                //    {
                //        eznForEsal.EznId = _context.EznsForEsals.Where(a => a.CompanyID == _context.Users.SingleOrDefault(b => b.Id == _userManager.GetUserId(User)).CompanyId).Max(a => a.EznId) + 1;

                //    }
                //    else if (_context.EznsForEsals.Where(a => a.CompanyID == _context.Users.SingleOrDefault(b => b.Id == _userManager.GetUserId(User)).CompanyId).Count() == 0)
                //    {
                //        eznForEsal.EznId = _context.Ezns.Where(a => a.CompanyID == _context.Users.SingleOrDefault(b => b.Id == _userManager.GetUserId(User)).CompanyId).Max(a => a.EznId) + 1;

                //    }
                //    else if (_context.Ezns.Where(a => a.CompanyID == _context.Users.SingleOrDefault(b => b.Id == _userManager.GetUserId(User)).CompanyId).Max(a => a.EznId) > _context.EznsForEsals.Where(a => a.CompanyID == _context.Users.SingleOrDefault(b => b.Id == _userManager.GetUserId(User)).CompanyId).Max(a => a.EznId))
                //    {
                //        eznForEsal.EznId = _context.Ezns.Where(a => a.CompanyID == _context.Users.SingleOrDefault(b => b.Id == _userManager.GetUserId(User)).CompanyId).Max(a => a.EznId) + 1;

                //    }
                //    else if (_context.Ezns.Where(a => a.CompanyID == _context.Users.SingleOrDefault(b => b.Id == _userManager.GetUserId(User)).CompanyId).Max(a => a.EznId) < _context.EznsForEsals.Where(a => a.CompanyID == _context.Users.SingleOrDefault(b => b.Id == _userManager.GetUserId(User)).CompanyId).Max(a => a.EznId))
                //    {
                //        eznForEsal.EznId = _context.EznsForEsals.Where(a => a.CompanyID == _context.Users.SingleOrDefault(b => b.Id == _userManager.GetUserId(User)).CompanyId).Max(a => a.EznId) + 1;

                //    }

                //}

                eznForEsal.UserId = _userManager.GetUserId(User);
                eznForEsal.CompanyID =(await _userManager.Users.SingleOrDefaultAsync(a => a.Id == _userManager.GetUserId(User))).CompanyId;

                eznForEsal.EznDate = DateTime.Now;

                _context.Add(eznForEsal);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index","Ezns");
            }
            ViewData["CompanyID"] = new SelectList(_context.Companies, "Id", "Company_Name", eznForEsal.CompanyID);
            ViewData["MenuLE0Id"] = new SelectList(_context.MenuLE0, "Id", "M0_Name", eznForEsal.MenuLE0Id);
            ViewData["CustomerOrSupplierId"] = new SelectList(_context.CustomerOrSuppliers, "Id", "Name",eznForEsal.CustomerOrSupplierId);
            ViewData["CustomerSupplierId"] = new SelectList(_context.CustomersSuppliers, "Id", "Name",eznForEsal.CustomerSupplierId);

            ViewData["PaymentMethodId"] = new SelectList(_context.paymentMethods, "Id", "Name", eznForEsal.PaymentMethodId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", eznForEsal.UserId);
            return View(eznForEsal);
        }

        [Authorize(Roles = CustomRoles.Admin + "," + CustomRoles.BranchManager + "," + CustomRoles.Edits)]
        // GET: EznForEsals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eznForEsal = await _context.EznsForEsals.SingleOrDefaultAsync(m => m.EznId == id);
            if (eznForEsal == null)
            {
                return NotFound();
            }
            ViewData["CompanyID"] = new SelectList(_context.Companies, "Id", "Company_Name", eznForEsal.CompanyID);
            ViewData["MenuLE0Id"] = new SelectList(_context.MenuLE0, "Id", "M0_Name", eznForEsal.MenuLE0Id);
            ViewData["PaymentMethodId"] = new SelectList(_context.paymentMethods, "Id", "Name", eznForEsal.PaymentMethodId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", eznForEsal.UserId);
            return View(eznForEsal);
        }

        [Authorize(Roles = CustomRoles.Admin + "," + CustomRoles.BranchManager + "," + CustomRoles.Edits)]
        // POST: EznForEsals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EznId,EznDate,Name,MenuLE0Id,ExpenseName,AmountWithdrawn,PaymentMethodId")] EznForEsal eznForEsal)
        {
            if (id != eznForEsal.EznId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(eznForEsal);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EznForEsalExists(eznForEsal.EznId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Ezns");
            }
            ViewData["CompanyID"] = new SelectList(_context.Companies, "Id", "Company_Name", eznForEsal.CompanyID);
            ViewData["MenuLE0Id"] = new SelectList(_context.MenuLE0, "Id", "M0_Name", eznForEsal.MenuLE0Id);
            ViewData["PaymentMethodId"] = new SelectList(_context.paymentMethods, "Id", "Name", eznForEsal.PaymentMethodId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", eznForEsal.UserId);
            return View(eznForEsal);
        }

        //// GET: EznForEsals/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var eznForEsal = await _context.EznsForEsals
        //        .Include(e => e.Company)
        //        .Include(e => e.MenuLE0)
        //        .Include(e => e.MenuLE1)
        //        .Include(e => e.MenuLE2)
        //        .Include(e => e.PaymentMethod)
        //        .Include(e => e.User)
        //        .SingleOrDefaultAsync(m => m.Id == id);
        //    if (eznForEsal == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(eznForEsal);
        //}

        //// POST: EznForEsals/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var eznForEsal = await _context.EznsForEsals.SingleOrDefaultAsync(m => m.Id == id);
        //    _context.EznsForEsals.Remove(eznForEsal);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction("Index", "Ezns");
        //}
        // GET: EznForEsals/PrintEznForEsal/5
        public async Task<IActionResult> PrintEznForEsal(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eznForEsal = await _context.EznsForEsals
                .Include(e => e.Company)
                .Include(e => e.MenuLE0)
                .Include(e => e.PaymentMethod)
                .Include(e => e.User)
                .Include(e=>e.CustomerOrSupplier)
                .Include(e=>e.CustomerSupplier)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (eznForEsal == null)
            {
                return NotFound();
            }

            ViewData["CompanyLogo"] = "~/" + TravelERP.Properties.Resources.ImgFolder + "/" + eznForEsal.Company.CompanyLogo;
            ViewData["CompanyNameE"] =(await _context.Companies.SingleOrDefaultAsync(a => a.Id == eznForEsal.CompanyID)).Company_NameE;
            ViewData["CompanyName"] =(await _context.Companies.SingleOrDefaultAsync(a => a.Id == eznForEsal.CompanyID)).Company_Name;
            ViewData["CompanyA4EsalImage"] = "~/" + TravelERP.Properties.Resources.ImgFolder + "/" + _context.Companies.SingleOrDefault(a => a.Id == eznForEsal.CompanyID).CompanyA4EsalImage;
            //ViewData["PriceName"] = this.Tafgeet(eznForEsal.AmountWithdrawn, "جنيه فقط لا غير", "قرش");
            var Currency = eznForEsal.CompanyID == 6 ? "درهم فقط لا غير" : "جنيه فقط لا غير";
            ViewData["PriceName"] = MOJ.General.ConvertToLetters(eznForEsal.AmountWithdrawn) + " " + Currency;

            return View(eznForEsal);
        }


        private bool EznForEsalExists(int id)
        {
            return _context.EznsForEsals.Any(e => e.Id == id);
        }

        private string Tafgeet(Double Amount, string Curr, string SubCurr)
        {
            string Value = Math.Floor(Amount).ToString();
            string NewValue = null;
            string[] Part1 = new string[10] { "", "إحدي ", "اثنا", "ثلاث", "أربعة", "خمسة", "ستة", "سبعة", "ثمانية", "تسعة" };
            string[] Part2 = new string[10] { "", "عشر", "عشرون", "ثلاثون", "أربعون", "خمسون", "ستون", "سبعون", "ثمانون", "تسعون" };
            string[] Part3 = new string[10] { "", "مئة", "مئان", "ثلاث مائة", "أربع مائة", "خمس مائة", "ست مائة", "سبع مائة", "ثمان مائة", "تسع مائة" };
            string[] Part4 = new string[10] { "", "ألف", "ألفان", "ثلاثة آلاف", "أربع آلاف", "خمسة آلاف", "ستة آلاف", "سبع آلاف", "ثمانية آلاف", "تسعة آلاف" };

            //---------------------------< ....... الملاييييييييييييين ........ >
            if (Value.Length == 7)
            {
                if (Value.Substring(0, 1) == "1")
                    NewValue = "مليون ";
                else if (Value.Substring(0, 1) == "2")
                    NewValue = "مليونان ";
                else
                    NewValue = Part1[int.Parse(Value.Substring(0, 1))] + " ملايين";

                Value = Value.Substring(1, Value.Length - 1);
            }
            //------------------------------------< ....... مئات الألوف....... >
            if (Value.Length == 6)
            {
                NewValue += " " + Part3[int.Parse(Value.Substring(0, 1))] + " و";
                Value = Value.Substring(1, Value.Length - 1);
            }
            //----------------------------------
            if (Value.Length == 5)
            {
                NewValue += " " + Part1[int.Parse(Value.Substring(1, 1))];
                NewValue += " و" + Part2[int.Parse(Value.Substring(0, 1))] + " ألفا و";
                Value = Value.Substring(1, Value.Length - 1);
                Value = Value.Substring(1, Value.Length - 1);
            }
            //---------------------------------
            if (Value.Length == 4)
            {
                NewValue += " " + Part4[int.Parse(Value.Substring(0, 1))];
                Value = Value.Substring(1, Value.Length - 1);
            }
            if (Value.Length == 3)
            {
                NewValue += " " + Part3[int.Parse(Value.Substring(0, 1))] + " ";
                Value = Value.Substring(1, Value.Length - 1);
            }
            //-----------------------------------
            if (Value.Length >= 1 && Value.Length <= 2)
            {
                NewValue += Part1[int.Parse(Value.Substring(1, 1))];
                NewValue += " " + Part2[int.Parse(Value.Substring(0, 1))];
                Value = Value.Substring(1, Value.Length - 1);
                Value = Value.Substring(1, Value.Length - 1);
            }
            NewValue += " " + Curr;
            //الكسر العشري
            string Real = Amount.ToString("");
            int Pos = Real.IndexOf('.', 0, Real.Length);
            if (Pos != -1)
            {
                Pos++;
                try
                {
                    string NewReal = (float.Parse(Real.Substring(Pos, Real.Length - Pos))).ToString();
                    if (NewReal.Length == 1)
                        NewValue += " و " + Part1[int.Parse(NewReal.Substring(0, 1))];
                    else
                    {
                        NewValue += " و " + Part1[int.Parse(NewReal.Substring(1, 1))];
                        NewValue += " و " + Part2[int.Parse(NewReal.Substring(0, 1))];
                    }
                    NewValue += " " + SubCurr;
                }
                catch { }
            }
            return NewValue;
        }

    }
}
