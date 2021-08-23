using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using TravelERP.Data;
using TravelERP.Models;
using static TravelERP.Controllers.AccountController;

namespace TravelERP.Controllers
{
    [Authorize(Roles = CustomRoles.Admin + "," + CustomRoles.BranchManager + "," + CustomRoles.Ezns)]
    public class EznsController : Controller
    {
         private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;


       public EznsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;

        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////// Ezns/Index **** الإذون - الرئيسيه **** ///////////////////////////////////
        public async Task<IActionResult> Index()
        {
            var CompanyId =(await _userManager.Users.SingleOrDefaultAsync(a => a.Id == _userManager.GetUserId(User))).CompanyId;
            var CompanyCuntry =(await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyId)).DateTimeCountry;
            var info = TimeZoneInfo.FindSystemTimeZoneById(CompanyCuntry);
            DateTimeOffset localServerTime = DateTimeOffset.Now;
            DateTimeOffset localTime = TimeZoneInfo.ConvertTime(localServerTime, info);
            DateTime datetimenow = localTime.DateTime;
            ViewData["DateTimeNow"] = datetimenow.Date.ToShortDateString();
            return View();
        }


        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////// Ezns/Details **** الإذون - تفاصيل **** ///////////////////////////////////
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ezns =await _context.Ezns.Include(e => e.Company)
                .Include(e => e.MenuLZ0)
                .Include(e => e.MenuLZ1)
                .Include(e => e.MenuLZ2)
                .Include(e => e.MenuLE0)
                .Include(e => e.CustomerOrSupplier)
                .Include(e => e.CustomerSupplier)
                .Include(e => e.MenuLE1)
                .Include(e => e.MenuLE2)
                .Include(e => e.PaymentMethod)
                .Include(e => e.User)
                .Include(e => e.Company)
                .Where(A => A.CompanyID == _userManager.Users.SingleOrDefault(a => a.Id == _userManager.GetUserId(User)).CompanyId).SingleOrDefaultAsync(m => m.Id == id);
            var ezn = ezns;

            if (ezn == null)
            {
                return NotFound();
            }

            return View(ezn);
        }


        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////// Ezns/Create **** الإذون - إنشاء اذن لمصروف **** ////////////////////////////////
        public IActionResult Create()
        {
            ViewData["CompanyID"] = new SelectList(_context.Companies, "Id", "Company_Name");
            ViewData["MenuLZ0Id"] = new SelectList(_context.MenuLZ0, "Id", "M0_Name");
            ViewData["MenuLZ1Id"] = new SelectList("", "Id", "M1_Name");
            ViewData["MenuLZ2Id"] = new SelectList("", "Id", "M2_Name");
            ViewData["PaymentMethodId"] = new SelectList(_context.paymentMethods, "Id", "Name");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Ezns/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EznId,EznDate,Name,MenuLZ0Id,MenuLZ1Id,MenuLZ2Id,ExpenseName,AmountWithdrawn,PaymentMethodId")] Ezn ezn)
        {
            if (ezn.Name == null)
            {
                ModelState.AddModelError("Name", "من فضلك ادخل اسم صحيح");
            }
            if (ezn.MenuLZ0Id == 0 || ezn.MenuLZ0Id == null)
            {
                ModelState.AddModelError("MenuLZ0Id", "من فضلك اختر من القائمه الرئيسيه");
            }
            if (ezn.MenuLZ1Id == 0 || ezn.MenuLZ1Id == null)
            {
                ModelState.AddModelError("MenuLZ1Id", "من فضلك اختر من القائمه الفرعيه 1");
            }
            if (ezn.MenuLZ2Id == 0 || ezn.MenuLZ2Id == null)
            {
                ModelState.AddModelError("MenuLZ2Id", "من فضلك اختر من القائمه الفرعيه 2");
            }

            if (ModelState.IsValid)
            {
                var CompanyId =(await _userManager.Users.SingleOrDefaultAsync(a => a.Id == _userManager.GetUserId(User))).CompanyId;
                var CompanyCuntry =(await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyId)).DateTimeCountry;
                var info = TimeZoneInfo.FindSystemTimeZoneById(CompanyCuntry);
                DateTimeOffset localServerTime = DateTimeOffset.Now;
                DateTimeOffset localTime = TimeZoneInfo.ConvertTime(localServerTime, info);
                ezn.UserId = _userManager.GetUserId(User);
                ezn.CompanyID = CompanyId;
                ezn.EznDate = localTime.DateTime;
                if (_context.Ezns.Where(a => a.CompanyID == ezn.CompanyID).Count() == 0)
                {
                    ezn.EznId = 1;
                }
                else
                {
                    ezn.EznId = _context.Ezns.Where(a => a.CompanyID == ezn.CompanyID).Max(a => a.EznId) + 1;

                }
                _context.Add(ezn);
                await _context.SaveChangesAsync();
                return RedirectToAction("PrintEzn", new RouteValueDictionary(
                new { controller = "Ezns", action = "PrintEzn", Id = ezn.Id }));
            }
            ViewData["CompanyID"] = new SelectList(_context.Companies, "Id", "Company_Name", ezn.CompanyID);
            ViewData["MenuLZ0Id"] = new SelectList(_context.MenuLZ0, "Id", "M0_Name", ezn.MenuLZ0Id);
            ViewData["MenuLZ1Id"] = new SelectList(_context.MenuLZ1.Where(d=>d.MenuLZ0Id == ezn.MenuLZ0Id), "Id", "M1_Name", ezn.MenuLZ1Id);
            ViewData["MenuLZ2Id"] = new SelectList(_context.MenuLZ2.Where(d => d.MenuLZ1Id == ezn.MenuLZ1Id), "Id", "M2_Name", ezn.MenuLZ2Id);
            ViewData["PaymentMethodId"] = new SelectList(_context.paymentMethods, "Id", "Name", ezn.PaymentMethodId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", ezn.UserId);
            return View(ezn);
        }


        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////// Ezns/CreateEFE **** الإذون - إنشاء اذن لرحله **** ////////////////////////////
        public IActionResult CreateEFE()
        {
            ViewData["CompanyID"] = new SelectList(_context.Companies, "Id", "Company_Name");
            ViewData["MenuLE0Id"] = new SelectList(_context.MenuLE0.Where(a => a.M0_Name != "سياحة دينية"), "Id", "M0_Name");
            ViewData["PaymentMethodId"] = new SelectList(_context.paymentMethods, "Id", "Name");
            ViewData["CustomerOrSupplierId"] = new SelectList(_context.CustomerOrSuppliers.Where(a => a.Name != "مندوب"), "Id", "Name");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: EznForEsals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEFE([Bind("Id,EznId,EznDate,Name,MenuLE0Id,MenuLE1Id,ExpenseName,AmountWithdrawn,PaymentMethodId,CustomerOrSupplierId,CustomerSupplierId")] Ezn ezn)
        {
            if (ezn.CustomerOrSupplierId == 0 || ezn.CustomerOrSupplierId == null)
            {
                ModelState.AddModelError("CustomerOrSupplierId", "من فضلك اختر المورد او العميل");
            }
            if (ezn.CustomerSupplierId == 0 || ezn.CustomerSupplierId == null)
            {
                ModelState.AddModelError("CustomerSupplierId", "من فضلك اختر اسم المورد او العميل");
            }

            if (ezn.MenuLE0Id == 0 || ezn.MenuLE0Id == null)
            {
                ModelState.AddModelError("MenuLE0Id", "من فضلك اختر من القائمه الرئيسيه");
            }

            if (ModelState.IsValid)
            {
                var CompanyId =(await _userManager.Users.SingleOrDefaultAsync(a => a.Id == _userManager.GetUserId(User))).CompanyId;
                var CompanyCuntry =(await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyId)).DateTimeCountry;
                var info = TimeZoneInfo.FindSystemTimeZoneById(CompanyCuntry);
                DateTimeOffset localServerTime = DateTimeOffset.Now;
                DateTimeOffset localTime = TimeZoneInfo.ConvertTime(localServerTime, info);
                ezn.EznDate = localTime.DateTime;
                ezn.UserId = _userManager.GetUserId(User);
                ezn.CompanyID = CompanyId;
                if (_context.Ezns.Where(a => a.CompanyID == ezn.CompanyID).Count() == 0)
                {
                    ezn.EznId = 1;
                }
                else
                {
                    ezn.EznId = _context.Ezns.Where(a => a.CompanyID == ezn.CompanyID).Max(a => a.EznId) + 1;

                }
                _context.Add(ezn);
                await _context.SaveChangesAsync();
                return RedirectToAction("PrintEzn", new RouteValueDictionary(
                new { controller = "Ezns", action = "PrintEzn", Id = ezn.Id }));
            }
            ViewData["CompanyID"] = new SelectList(_context.Companies, "Id", "Company_Name", ezn.CompanyID);
            ViewData["MenuLE0Id"] = new SelectList(_context.MenuLE0, "Id", "M0_Name", ezn.MenuLE0Id);
            ViewData["CustomerOrSupplierId"] = new SelectList(_context.CustomerOrSuppliers.Where(a => a.Name != "مندوب"), "Id", "Name", ezn.CustomerOrSupplierId);
            ViewData["CustomerSupplierId"] = new SelectList(_context.CustomersSuppliers.Where(a=>a.CustomerOrSupplierId == ezn.CustomerOrSupplierId), "Id", "Name", ezn.CustomerSupplierId);
            ViewData["PaymentMethodId"] = new SelectList(_context.paymentMethods, "Id", "Name", ezn.PaymentMethodId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", ezn.UserId);
            return View(ezn);
        }


        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////// Ezns/CreateGroup **** الإذون - إنشاء اذن لرحله مجموعه**** ////////////////////////////
        public IActionResult CreateGroup()
        {
            ViewData["CompanyID"] = new SelectList(_context.Companies, "Id", "Company_Name");
            ViewData["MenuLE0Id"] = new SelectList(_context.MenuLE0.Where(a => a.M0_Name != "سياحة دينية"), "Id", "M0_Name");
            ViewData["PaymentMethodId"] = new SelectList(_context.paymentMethods, "Id", "Name");
            ViewData["CustomerOrSupplierId"] = new SelectList(_context.CustomerOrSuppliers.Where(a => a.Name != "مندوب"), "Id", "Name");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Ezn/CreateGroup
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateGroup([Bind("Id,EznId,EznDate,Name,MenuLE0Id,MenuLE1Id,MenuLE2Id,ExpenseName,AmountWithdrawn,PaymentMethodId,CustomerOrSupplierId,CustomerSupplierId")] Ezn ezn)
        {
            if (ezn.CustomerOrSupplierId == 0 || ezn.CustomerOrSupplierId == null)
            {
                ModelState.AddModelError("CustomerOrSupplierId", "من فضلك اختر المورد او العميل");
            }
            if (ezn.ExpenseName == null)
            {
                ModelState.AddModelError("ادخل وصف", "اكمل جميع القوائم");
            }
            if (ezn.CustomerSupplierId == 0 || ezn.CustomerSupplierId == null)
            {
                ModelState.AddModelError("CustomerSupplierId", "من فضلك اختر اسم المورد او العميل");
            }

            if (ezn.MenuLE0Id == 0 || ezn.MenuLE0Id == null)
            {
                ModelState.AddModelError("MenuLE0Id", "من فضلك اختر من القائمه الرئيسيه");
            }
            if (ezn.MenuLE1Id == null || ezn.MenuLE1Id == 0)
            {
                ModelState.AddModelError("ادخل وصف", "من فضلك اختر قيمه من القائمه المنسدله 1");
            }

            if (ezn.MenuLE2Id == null || ezn.MenuLE2Id == 0)
            {
                ModelState.AddModelError("ادخل وصف", "من فضلك اختر قيمه من القائمه المنسدله 2");
            }

            if (ModelState.IsValid)
            {
                var CompanyId = (await _userManager.Users.SingleOrDefaultAsync(a => a.Id == _userManager.GetUserId(User))).CompanyId;
                var CompanyCuntry = (await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyId)).DateTimeCountry;
                var info = TimeZoneInfo.FindSystemTimeZoneById(CompanyCuntry);
                DateTimeOffset localServerTime = DateTimeOffset.Now;
                DateTimeOffset localTime = TimeZoneInfo.ConvertTime(localServerTime, info);
                ezn.EznDate = localTime.DateTime;
                ezn.UserId = _userManager.GetUserId(User);
                ezn.CompanyID = CompanyId;
                if (_context.Ezns.Where(a => a.CompanyID == ezn.CompanyID).Count() == 0)
                {
                    ezn.EznId = 1;
                }
                else
                {
                    ezn.EznId = _context.Ezns.Where(a => a.CompanyID == ezn.CompanyID).Max(a => a.EznId) + 1;

                }
                _context.Add(ezn);
                await _context.SaveChangesAsync();
                return RedirectToAction("PrintEzn", new RouteValueDictionary(
                new { controller = "Ezns", action = "PrintEzn", Id = ezn.Id }));
            }
            ViewData["MenuLE0Id"] = new SelectList(_context.MenuLE0, "Id", "M0_Name", ezn.MenuLE0Id);
            ViewData["MenuLE1Id"] = new SelectList(_context.MenuLE1.Where(a => a.MenuLE0Id == ezn.MenuLE0Id), "Id", "M1_Name", ezn.MenuLE1Id);
            ViewData["MenuLE2Id"] = new SelectList(_context.MenuLE2.Where(a => a.MenuLE1Id == ezn.MenuLE1Id), "Id", "M2_Name", ezn.MenuLE2Id);
            ViewData["CustomerOrSupplierId"] = new SelectList(_context.CustomerOrSuppliers.Where(a => a.Name != "مندوب"), "Id", "Name", ezn.CustomerOrSupplierId);
            ViewData["CustomerSupplierId"] = new SelectList(_context.CustomersSuppliers.Where(a => a.CustomerOrSupplierId == ezn.CustomerOrSupplierId), "Id", "Name", ezn.CustomerSupplierId);
            ViewData["PaymentMethodId"] = new SelectList(_context.paymentMethods, "Id", "Name", ezn.PaymentMethodId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", ezn.UserId);
            return View(ezn);
        }


        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////// // Ezns/CreateEFEReligious **** الإذون - إنشاء اذن لسياحه دينيه **** ///////////////////////
        public IActionResult CreateEFEReligious()
        {
            ViewData["CompanyID"] = new SelectList(_context.Companies, "Id", "Company_Name");
            ViewData["MenuLE0Id"] = new SelectList(_context.MenuLE0.Where(a => a.M0_Name == "سياحة دينية"), "Id", "M0_Name");
            ViewData["MenuLE1Id"] = new SelectList(_context.MenuLE1.Where(a => a.MenuLE0.M0_Name == "سياحة دينية"), "Id", "M1_Name");
            ViewData["PaymentMethodId"] = new SelectList(_context.paymentMethods, "Id", "Name");
            ViewData["CustomerOrSupplierId"] = new SelectList(_context.CustomerOrSuppliers.Where(a => a.Name != "مورد او وكيل").Where(a => a.Name != "جارى الشركاء"), "Id", "Name");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: EznForEsals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEFEReligious([Bind("Id,EznId,EznDate,Name,MenuLE0Id,MenuLE1Id,MenuLE2Id,ExpenseName,AmountWithdrawn,PaymentMethodId,CustomerOrSupplierId,CustomerSupplierId")] Ezn ezn)
        {
            if (ezn.MenuLE0Id == 0 || ezn.MenuLE0Id == null)
            {
                ModelState.AddModelError("MenuLE0Id", "من فضلك اختر من القائمه الرئيسيه");
            }


            if (ezn.MenuLE1Id == 0 || ezn.MenuLE1Id == null)
            {
                ModelState.AddModelError("MenuLE1Id", "من فضلك اختر من القائمه الفرعيه 1");
            }

            if (ezn.MenuLE2Id == 0 || ezn.MenuLE2Id == null)
            {
                ModelState.AddModelError("MenuLE2Id", "من فضلك اختر من القائمه الفرعيه 2");
            }

            if (ezn.CustomerOrSupplierId == 0 || ezn.CustomerOrSupplierId == null)
            {
                ModelState.AddModelError("CustomerOrSupplierId", "من فضلك اختر المورد او العميل");
            }
            if (ezn.CustomerSupplierId == 0 || ezn.CustomerSupplierId == null)
            {
                ModelState.AddModelError("CustomerSupplierId", "من فضلك اختر اسم المورد او العميل");
            }
            var CompanyId =(await _userManager.Users.SingleOrDefaultAsync(a => a.Id == _userManager.GetUserId(User))).CompanyId;
            if (ModelState.IsValid)
            {
                var CompanyCuntry =(await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyId)).DateTimeCountry;
                var info = TimeZoneInfo.FindSystemTimeZoneById(CompanyCuntry);
                DateTimeOffset localServerTime = DateTimeOffset.Now;
                DateTimeOffset localTime = TimeZoneInfo.ConvertTime(localServerTime, info);
                ezn.EznDate = localTime.DateTime;
                ezn.UserId = _userManager.GetUserId(User);
                ezn.CompanyID = CompanyId;
                if (_context.Ezns.Where(a => a.CompanyID == ezn.CompanyID).Count() == 0)
                {
                    ezn.EznId = 1;
                }
                else
                {
                    ezn.EznId = _context.Ezns.Where(a => a.CompanyID == ezn.CompanyID).Max(a => a.EznId) + 1;

                }

                _context.Add(ezn);
                await _context.SaveChangesAsync();
                return RedirectToAction("PrintEzn", new RouteValueDictionary(
                new { controller = "Ezns", action = "PrintEzn", Id = ezn.Id }));
            }
            ViewData["CompanyID"] = new SelectList(_context.Companies, "Id", "Company_Name", ezn.CompanyID);
            ViewData["MenuLE0Id"] = new SelectList(_context.MenuLE0.Where(a => a.M0_Name == "سياحة دينية"), "Id", "M0_Name", ezn.MenuLE0Id);
            ViewData["MenuLE1Id"] = new SelectList(_context.MenuLE1.Where(a => a.MenuLE0.M0_Name == "سياحة دينية"), "Id", "M1_Name", ezn.MenuLE1Id);
            ViewData["MenuLE2Id"] = new SelectList(_context.MenuLE2.Where(a => a.MenuLE1Id == ezn.MenuLE1Id), "Id", "M2_Name", ezn.MenuLE2Id);
            ViewData["CustomerOrSupplierId"] = new SelectList(_context.CustomerOrSuppliers.Where(a => a.Name != "مورد او وكيل").Where(a => a.Name != "جارى الشركاء"), "Id", "Name", ezn.CustomerOrSupplierId);
            ViewData["CustomerSupplierId"] = new SelectList(_context.CustomersSuppliers.Where(e => e.CompanyId == CompanyId).Where(a=>a.CustomerOrSupplierId == ezn.CustomerOrSupplierId).OrderBy(a=>a.Name), "Id", "Name", ezn.CustomerSupplierId);
            ViewData["PaymentMethodId"] = new SelectList(_context.paymentMethods, "Id", "Name", ezn.PaymentMethodId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", ezn.UserId);
            return View(ezn);
        }



        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////// Ezns/PrintEzn **** الإذون - طباعه **** ///////////////////////////////////
        public async Task<ActionResult> PrintEzn(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var Ezns =await (from x in _context.Ezns select new Models.ViewModel.Ezns { Id = x.Id, EznId = x.EznId, EznDate = x.EznDate, Name = string.Concat(x.CustomerOrSupplier.Name + " - " + x.CustomerSupplier.Name, x.Name), ExpenseName = x.ExpenseName, Menu0 = string.Concat(x.MenuLE0.M0_Name, " - " + x.MenuLE1.M1_Name, " - " + x.MenuLE2.M2_Name, x.MenuLZ0.M0_Name, " - " + x.MenuLZ1.M1_Name, " - " + x.MenuLZ2.M2_Name), AmountWithdrawn = x.AmountWithdrawn, CompanyId = x.CompanyID, CompanyName = x.Company.Company_Name, PaymentMethod = x.PaymentMethod.Name, UserName = x.User.UserName }).SingleOrDefaultAsync(m => m.Id == id);
            ViewData["DeveloperContacs"] = _context.Developer.SingleOrDefault(a => a.Title == "1").Message;
            var ezn = Ezns;


            if (ezn == null)
            {
                return NotFound();
            }

            var CompanyId = _userManager.Users.SingleOrDefault(a => a.Id == _userManager.GetUserId(User)).CompanyId;
            ViewData["CompanyLogo"] = "~/" + TravelERP.Properties.Resources.ImgFolder + "/" +(await _context.Companies.SingleOrDefaultAsync(a => a.Id == ezn.CompanyId)).CompanyLogo;
            ViewData["CompanyNameE"] =(await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyId)).Company_NameE;
            ViewData["CompanyName"] =(await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyId)).Company_Name;
            ViewData["CompanyA4EsalImage"] = "~/" + TravelERP.Properties.Resources.ImgFolder + "/" +(await _context.Companies.SingleOrDefaultAsync(a => a.Id == ezn.CompanyId)).CompanyA4EsalImage;
            //ViewData["PriceName"] = this.Tafgeet(ezn.AmountWithdrawn, ezn.CompanyId == 6 ? "درهم فقط لا غير" : "جنيه فقط لا غير", ezn.CompanyId == 6 ? "فلس" : "قرش");
            var Currency = ezn.CompanyId == 6 ? "درهم فقط لا غير" : "جنيه فقط لا غير";
            //var Subcurrency = ezn.CompanyId == 6 ? "فلس" : "قرش";

            ViewData["PriceName"] = MOJ.General.ConvertToLetters(ezn.AmountWithdrawn) + " " + Currency ;
            
            ViewData["CompanyAddress"] =(await _context.Companies.SingleOrDefaultAsync(a => a.Id == ezn.CompanyId)).Company_Address;
            ViewData["CompanyPhones"] =(await _context.Companies.SingleOrDefaultAsync(a => a.Id == ezn.CompanyId)).Company_PhonesNumber;

            return View(ezn);
        }



        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////// Ezns/Edit **** الإذون - تعديل **** ////////////////////////////////////
        [Authorize(Roles = CustomRoles.Admin + "," + CustomRoles.BranchManager + "," + CustomRoles.Edits)]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ezn = await _context.Ezns.Include(a => a.MenuLE0).SingleOrDefaultAsync(m => m.Id == id);
            if (ezn == null)
            {
                return NotFound();
            }
            ViewData["CustomerOrSupplierId"] = new SelectList(_context.CustomerOrSuppliers.Where(a => a.Name != "مندوب"), "Id", "Name", ezn.CustomerOrSupplierId);
            ViewData["CustomerOrSupplierIdReligious"] = new SelectList(_context.CustomerOrSuppliers.Where(a => a.Name == "مندوب" || a.Name == "عميل"), "Id", "Name", ezn.CustomerOrSupplierId);
            ViewData["CustomerSupplierId"] = new SelectList(_context.CustomersSuppliers.Where(a => a.CustomerOrSupplierId == ezn.CustomerOrSupplierId).Where(A => A.CompanyId == _userManager.Users.SingleOrDefault(a => a.Id == _userManager.GetUserId(User)).CompanyId).OrderBy(a => a.Name), "Id", "Name", ezn.CustomerSupplierId);
            ViewData["MenuLE0Id"] = new SelectList(_context.MenuLE0, "Id", "M0_Name", ezn.MenuLE0Id);
            ViewData["MenuLE0IdReligious"] = new SelectList(_context.MenuLE0.Where(a=>a.M0_Name == "سياحة دينية"), "Id", "M0_Name", ezn.MenuLE0Id);
            ViewData["MenuLE1Id"] = new SelectList(_context.MenuLE1.Where(a => a.MenuLE0Id == ezn.MenuLE0Id), "Id", "M1_Name", ezn.MenuLE1Id);
            ViewData["MenuLE2Id"] = new SelectList(_context.MenuLE2.Where(a => a.MenuLE1Id == ezn.MenuLE1Id), "Id", "M2_Name", ezn.MenuLE2Id);
            ViewData["MenuLZ0Id"] = new SelectList(_context.MenuLZ0, "Id", "M0_Name", ezn.MenuLZ0Id);
            ViewData["MenuLZ1Id"] = new SelectList(_context.MenuLZ1.Where(a => a.MenuLZ0Id == ezn.MenuLZ0Id), "Id", "M1_Name", ezn.MenuLZ1Id);
            ViewData["MenuLZ2Id"] = new SelectList(_context.MenuLZ2.Where(a => a.MenuLZ1Id == ezn.MenuLZ1Id), "Id", "M2_Name", ezn.MenuLZ2Id);
            ViewData["PaymentMethodId"] = new SelectList(_context.paymentMethods.Where(f => f.Name == "نقدى"), "Id", "Name", ezn.PaymentMethodId);
            return View(ezn);
        }
        [Authorize(Roles = CustomRoles.Admin + "," + CustomRoles.BranchManager + "," + CustomRoles.Edits)]
        // POST: Ezns/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EznId,BillId,BillIdId,EznDate,Name,MenuLZ0Id,MenuLZ1Id,MenuLZ2Id,ExpenseName,AmountWithdrawn,PaymentMethodId,CustomerOrSupplierId,CustomerSupplierId,MenuLE0Id,MenuLE1Id,MenuLE2Id,UserId,CompanyID")] Ezn ezn)
        {
            if (id != ezn.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ezn);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EznExists(ezn.Id))
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
            ViewData["CompanyID"] = new SelectList(_context.Companies, "Id", "Company_Name", ezn.CompanyID);
            ViewData["MenuLZ0Id"] = new SelectList(_context.MenuLZ0, "Id", "M0_Name", ezn.MenuLZ0Id);
            ViewData["MenuLZ1Id"] = new SelectList(_context.MenuLZ1, "Id", "M1_Name", ezn.MenuLZ1Id);
            ViewData["MenuLZ2Id"] = new SelectList(_context.MenuLZ2, "Id", "M2_Name", ezn.MenuLZ2Id);
            ViewData["PaymentMethodId"] = new SelectList(_context.paymentMethods.Where(f => f.Name == "نقدى"), "Id", "Name", ezn.PaymentMethodId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", ezn.UserId);
            return View(ezn);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////// Ezns/EznsDate **** التقارير - تقارير اذون الصرف **** ////////////////////////
        public IActionResult EznsDate()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EznsDate([Bind("StartDate,EndDate")] DatesSearch datesSearch)
        {
            if (datesSearch.StartDate == null)
            {
                ModelState.AddModelError("StartDate", "من فضلك اختر تاريخ من للبحث");
            }
            if (datesSearch.EndDate == null)
            {
                ModelState.AddModelError("EndDate", "من فضلك اختر تاريخ الى للبحث");
            }
            if (ModelState.IsValid)
            {
                TempData["StartDate"] = datesSearch.StartDate.Value.Date.ToString();
                TempData["EndDate"] = datesSearch.EndDate.Value.Date.ToString();

                //_context.Add(datesSearch);
                await _context.SaveChangesAsync();
                return RedirectToAction("StatementEzns", "Ezns");
            }
            return View(datesSearch);
        }
        public async Task<ActionResult> StatementEzns()
        {
            var UserId = _userManager.GetUserId(User);
            var CompanyId =(await _userManager.Users.SingleOrDefaultAsync(a => a.Id == UserId)).CompanyId;
            var ezns =await( from x in _context.Ezns
                   .Where(d => d.CompanyID == CompanyId)
                   .Where(d => d.EznDate.Date >= Convert.ToDateTime(TempData["StartDate"]))
                   .Where(d => d.EznDate.Date <= Convert.ToDateTime(TempData["EndDate"]))
                       select new Models.ViewModel.Transactions { EId = x.EznId, EDate = x.EznDate, Type = string.Concat(x.MenuLE0.M0_Name, x.MenuLZ1.M1_Name), Name3 = string.Concat(x.Name, x.CustomerOrSupplier.Name + " - ", x.CustomerSupplier.Name), Statement = string.Concat(x.MenuLZ0.M0_Name + " - ", x.MenuLZ1.M1_Name + " - ", x.MenuLZ2.M2_Name + " - ", x.MenuLE0.M0_Name + " - ", x.MenuLE1.M1_Name + " - ", x.MenuLE2.M2_Name + " - ", x.ExpenseName), Credit = x.AmountWithdrawn }).ToListAsync();
            ViewData["CompanyLogo"] = "~/" + TravelERP.Properties.Resources.ImgFolder + "/" +(await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyId)).CompanyLogo;
            ViewData["CompanyA4EsalImage"] = "~/" + TravelERP.Properties.Resources.ImgFolder + "/" + (await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyId)).CompanyA4EsalImage;
            ViewData["CompanyNameE"] = (await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyId)).Company_NameE;
            ViewData["CompanyName"] = (await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyId)).Company_Name;
            ViewData["CompanyId"] = CompanyId;

            return View( ezns);
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
                NewValue += " " + Part2[int.Parse(Value.Substring(0, 1))] + " الاف";

                Value = Value.Substring(1, Value.Length - 1);
                Value = Value.Substring(1, Value.Length - 1);
            }
            //---------------------------------
            if (Value.Length == 4)
            {
                NewValue += "  " + Part4[int.Parse(Value.Substring(0, 1))];
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


        public JsonResult GetMenuLE1(int id)

        {
            List<MenuLE1> menuLE1 = new List<MenuLE1>();
            menuLE1 = _context.MenuLE1.Where(a => a.MenuLE0.Id == id).ToList();
            return Json(new SelectList(menuLE1, "Id", "M1_Name"));
        }


        private bool EznExists(int id)
        {
            return _context.Ezns.Any(e => e.EznId == id);
        }

    }
}
