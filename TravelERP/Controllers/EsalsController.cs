using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    [Authorize(Roles = CustomRoles.Admin + "," + CustomRoles.BranchManager + "," + CustomRoles.Esals)]
    public class EsalsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public EsalsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;

        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////// Esals/Index **** الإيصالات  **** /////////////////////////////////////////
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
        ////////////////////////////// Esals/Details **** الإيصالات - تفاصيل  **** ////////////////////////////////////
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var esal = await _context.Esals
                .Include(e => e.MenuLE0)
                .Include(e => e.MenuLE1)
                .Include(e => e.MenuLE2)
                .Include(e => e.PaymentMethod)
                .Include(e => e.User)
                .Include(e => e.CustomerOrSupplier)
                .Include(e => e.CustomerSupplier)
                .SingleOrDefaultAsync(m => m.Id == id);
            //ViewData["PriceName"] = this.Tafgeet(esal.AmountPaid, esal.CompanyID == 6 ? "درهم فقط لا غير" : "جنيه فقط لا غير", esal.CompanyID == 6 ? "فلس" : "قرش");
            var Currency = esal.CompanyID == 6 ? "درهم فقط لا غير" : "جنيه فقط لا غير";
            ViewData["PriceName"] = MOJ.General.ConvertToLetters(esal.AmountPaid) + " " + Currency;

            if (esal.BillId == null)
            {
                ViewData["Byan"] = string.Concat(esal.MenuLE0.M0_Name, " - ", esal.DepositDesc, " - ", " بقيمه ", esal.AmountPaid, (esal.CompanyID == 6 ? "درهم" : " ج "));
            }

            if (esal.MenuLE0.M0_Name == "طيران" && esal.BillId != null)
            {
                var Bill =await _context.BillAirLines.SingleOrDefaultAsync(a => a.Id == esal.BillId);
                ViewData["BillDate"] = Bill.BillDate;
                ViewData["Byan"] = "ذلك مقابل حجز " +(await _context.MenuLE0.SingleOrDefaultAsync(a => a.Id == Bill.MenuLE0Id)).M0_Name + " - " +(await _context.MenuLE2.SingleOrDefaultAsync(a => a.Id == Bill.MenuLE2Id)).M2_Name + " - عدد البالغين " + Bill.AdultN + " - عدد الاطفال " + Bill.ChildN + " - وذلك مقابل " + (esal.CompanyID == 6 ? "درهم" : " ج ");
                ViewData["DeveloperContacs"] = (await _context.Developer.SingleOrDefaultAsync(a => a.Title == "1")).Message;
            }

            if (esal.MenuLE0.M0_Name == "سياحة داخلية" && esal.BillId != null)
            {
                var Bill =await _context.BillDomestic.SingleOrDefaultAsync(a => a.Id == esal.BillId);
                ViewData["BillDate"] = Bill.BillDate;
                ViewData["Byan"] = "ذلك مقابل حجز " +(await _context.MenuLE0.SingleOrDefaultAsync(a => a.Id == Bill.MenuLE0Id)).M0_Name + " - " +(await _context.MenuLE2.SingleOrDefaultAsync(a => a.Id == Bill.MenuLE2Id)).M2_Name + " - عدد البالغين " + Bill.AdultN + " - عدد الاطفال " + Bill.ChildN + " - وذلك مقابل " + Bill.CustomerPrice + (esal.CompanyID == 6 ? "درهم" : " ج ");
                ViewData["DeveloperContacs"] = (await _context.Developer.SingleOrDefaultAsync(a => a.Title == "1")).Message;
            }
            if (esal.MenuLE0.M0_Name == "سياحة خارجية" && esal.BillId != null)
            {
                var Bill = await _context.BillForeigns.SingleOrDefaultAsync(a => a.Id == esal.BillId);
                ViewData["BillDate"] = Bill.BillDate;
                ViewData["Byan"] = "ذلك مقابل حجز " + (await _context.MenuLE0.SingleOrDefaultAsync(a => a.Id == Bill.MenuLE0Id)).M0_Name + " - " + (await _context.MenuLE2.SingleOrDefaultAsync(a => a.Id == Bill.MenuLE2Id)).M2_Name + " - عدد البالغين " + Bill.AdultN + " - عدد الاطفال " + Bill.ChildN + " - وذلك مقابل " + Bill.CustomerPrice + (esal.CompanyID == 6 ? "درهم" : " ج ");
                ViewData["DeveloperContacs"] = (await _context.Developer.SingleOrDefaultAsync(a => a.Title == "1")).Message;
            }

            if (esal.MenuLE0.M0_Name == "سياحة دينية" && esal.BillId != null)
            {
                var Bill =await _context.BillReligious.SingleOrDefaultAsync(a => a.Id == esal.BillId);
                ViewData["BillDate"] = Bill.BillDate;
                ViewData["Byan"] = "ذلك مقابل حجز " +(await _context.MenuLE0.SingleOrDefaultAsync(a => a.Id == Bill.MenuLE0Id)).M0_Name + " - " +(await _context.MenuLE2.SingleOrDefaultAsync(a => a.Id == Bill.MenuLE2Id)).M2_Name + " - وذلك مقابل " + Bill.CustomerPrice + (esal.CompanyID == 6 ? "درهم" : " ج ");
                ViewData["DeveloperContacs"] = (await _context.Developer.SingleOrDefaultAsync(a => a.Title == "1")).Message;
            }
            if (esal.MenuLE0.M0_Name == "تأشيرات" && esal.BillId != null)
            {
                var Bill =await _context.BillVisas.SingleOrDefaultAsync(a => a.Id == esal.BillId);
                ViewData["BillDate"] = Bill.BillDate;
                ViewData["Byan"] = "ذلك مقابل حجز " +(await _context.MenuLE0.SingleOrDefaultAsync(a => a.Id == Bill.MenuLE0Id)).M0_Name + " - " +(await _context.MenuLE2.SingleOrDefaultAsync(a => a.Id == Bill.MenuLE2Id)).M2_Name + " - عدد البالغين " + Bill.AdultN + " - عدد الاطفال " + Bill.ChildN + " - وذلك مقابل " + Bill.CustomerPrice + (esal.CompanyID == 6 ? "درهم" : " ج ");
                ViewData["DeveloperContacs"] = (await _context.Developer.SingleOrDefaultAsync(a => a.Title == "1")).Message;
            }
            if (esal == null)
            {
                return NotFound();
            }


            return View(esal);
        }



        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////// Esals/Print **** الإيصالات - طباعه  **** ///////////////////////////////////////
        public async Task<IActionResult> Print(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var esal = await _context.Esals
                //.Include(e => e.BillAirLines)
                .Include(e => e.MenuLE0)
                .Include(e => e.MenuLE1)
                .Include(e => e.MenuLE2)
                .Include(e => e.PaymentMethod)
                .Include(e => e.User)
                .Include(e => e.CustomerOrSupplier)
                .Include(e => e.CustomerSupplier)
                .SingleOrDefaultAsync(m => m.Id == id);
            //ViewData["PriceName"] = this.Tafgeet(esal.AmountPaid, esal.CompanyID == 6 ? "درهم فقط لا غير" : "جنيه فقط لا غير", esal.CompanyID == 6 ? "فلس" : "قرش");
            var Currency = esal.CompanyID == 6 ? "درهم فقط لا غير" : "جنيه فقط لا غير";
            ViewData["PriceName"] = MOJ.General.ConvertToLetters(esal.AmountPaid) + " " + Currency;

            if (esal.BillId == null )
            {
                ViewData["Byan"] = string.Concat("ذلك مقابل حجز ", (await _context.MenuLE0.SingleOrDefaultAsync(a => a.Id == esal.MenuLE0Id)).M0_Name + " - ", (esal.MenuLE1Id != null ? _context.MenuLE1.SingleOrDefault(a => a.Id == esal.MenuLE1Id).M1_Name: " ") + " - ", (esal.MenuLE2Id != null ? (await _context.MenuLE2.SingleOrDefaultAsync(a => a.Id == esal.MenuLE2Id)).M2_Name : " "), " - ", esal.DepositDesc, " - ", " بقيمه ", esal.AmountPaid, (esal.CompanyID == 6 ? "درهم" : " ج "));
            }

            if ( esal.MenuLE0.M0_Name == "طيران" && esal.BillId != null)
            {
                var Bill =await _context.BillAirLines.SingleOrDefaultAsync(a => a.Id == esal.BillId);
                ViewData["BillDate"] = Bill.BillDate;
                ViewData["Byan"] =string.Concat("ذلك مقابل حجز " ,(await _context.MenuLE0.SingleOrDefaultAsync(a => a.Id == Bill.MenuLE0Id)).M0_Name + " - ", _context.MenuLE1.SingleOrDefault(a => a.Id == Bill.MenuLE1Id).M1_Name + " - ", (await _context.MenuLE2.SingleOrDefaultAsync(a => a.Id == Bill.MenuLE2Id)).M2_Name , " - عدد البالغين " + Bill.AdultN , " - عدد الاطفال " + Bill.ChildN , " - وذلك مقابل " + Bill.CustomerPrice + (Bill.CompanyID == 6 ? "درهم" : " ج "));
                ViewData["DeveloperContacs"] = (await _context.Developer.SingleOrDefaultAsync(a => a.Title == "1")).Message;
            }

            if (esal.MenuLE0.M0_Name == "سياحة داخلية" && esal.BillId != null)
            {
                var Bill =await _context.BillDomestic.SingleOrDefaultAsync(a => a.Id == esal.BillId);
                ViewData["BillDate"] = Bill.BillDate;
                ViewData["Byan"] =string.Concat("ذلك مقابل حجز " ,(await _context.MenuLE0.SingleOrDefaultAsync(a => a.Id == Bill.MenuLE0Id)).M0_Name + " - " , _context.MenuLE1.SingleOrDefault(a => a.Id == Bill.MenuLE1Id).M1_Name + " - " , (await _context.MenuLE2.SingleOrDefaultAsync(a => a.Id == Bill.MenuLE2Id)).M2_Name , " - عدد البالغين " + Bill.AdultN , " - عدد الاطفال " + Bill.ChildN , " - وذلك مقابل " + Bill.CustomerPrice , (Bill.CompanyID == 6 ? "درهم" : " ج "));
                ViewData["DeveloperContacs"] = (await _context.Developer.SingleOrDefaultAsync(a => a.Title == "1")).Message;
            }
            if (esal.MenuLE0.M0_Name == "سياحة خارجية" && esal.BillId != null)
            {
                var Bill = await _context.BillForeigns.SingleOrDefaultAsync(a => a.Id == esal.BillId);
                ViewData["BillDate"] = Bill.BillDate;
                ViewData["Byan"] = "ذلك مقابل حجز " +string.Concat( (await _context.MenuLE0.SingleOrDefaultAsync(a => a.Id == Bill.MenuLE0Id)).M0_Name + " - " , _context.MenuLE1.SingleOrDefault(a => a.Id == Bill.MenuLE1Id).M1_Name + " - " , (await _context.MenuLE2.SingleOrDefaultAsync(a => a.Id == Bill.MenuLE2Id)).M2_Name , " - عدد البالغين " + Bill.AdultN , " - عدد الاطفال " + Bill.ChildN , " - وذلك مقابل " + Bill.CustomerPrice + (Bill.CompanyID == 6 ? "درهم" : " ج "));
                ViewData["DeveloperContacs"] = (await _context.Developer.SingleOrDefaultAsync(a => a.Title == "1")).Message;
            }

            if (esal.MenuLE0.M0_Name == "سياحة دينية" && esal.BillId != null)
            {
                var Bill =await _context.BillReligious.SingleOrDefaultAsync(a => a.Id == esal.BillId);
                ViewData["BillDate"] = Bill.BillDate;
                ViewData["Byan"] =string.Concat("ذلك مقابل حجز " ,(await _context.MenuLE0.SingleOrDefaultAsync(a => a.Id == Bill.MenuLE0Id)).M0_Name + " - " , _context.MenuLE1.SingleOrDefault(a => a.Id == Bill.MenuLE1Id).M1_Name + " - " , (await _context.MenuLE2.SingleOrDefaultAsync(a => a.Id == Bill.MenuLE2Id)).M2_Name  , " - وذلك مقابل " + Bill.CustomerPrice , (Bill.CompanyID == 6 ? "درهم" : " ج "));
                ViewData["DeveloperContacs"] =(await _context.Developer.SingleOrDefaultAsync(a => a.Title == "1")).Message;
            }
            if (esal.MenuLE0.M0_Name == "تأشيرات" && esal.BillId != null)
            {
                var Bill =await _context.BillVisas.SingleOrDefaultAsync(a => a.Id == esal.BillId);
                ViewData["BillDate"] = Bill.BillDate;
                ViewData["Byan"] =string.Concat("ذلك مقابل حجز " , _context.MenuLE0.SingleOrDefault(a => a.Id == Bill.MenuLE0Id).M0_Name + " - " , _context.MenuLE1.SingleOrDefault(a => a.Id == Bill.MenuLE1Id).M1_Name + " - " , (await _context.MenuLE2.SingleOrDefaultAsync(a => a.Id == Bill.MenuLE2Id)).M2_Name, " - عدد البالغين " + Bill.AdultN , " - عدد الاطفال " + Bill.ChildN , " - وذلك مقابل " + Bill.CustomerPrice , (Bill.CompanyID == 6 ? "درهم" : " ج "));
                ViewData["DeveloperContacs"] = (await _context.Developer.SingleOrDefaultAsync(a => a.Title == "1")).Message;
            }
            ViewData["CompanyLogo"] = "~/" + TravelERP.Properties.Resources.ImgFolder + "/" + _context.Companies.SingleOrDefault(a => a.Id == esal.CompanyID).CompanyLogo;
            ViewData["CompanyA4EsalImage"] = "~/" + TravelERP.Properties.Resources.ImgFolder + "/" + _context.Companies.SingleOrDefault(a => a.Id == esal.CompanyID).CompanyA4EsalImage;
            ViewData["CompanyNameE"] =(await  _context.Companies.SingleOrDefaultAsync(a => a.Id == esal.CompanyID)).Company_NameE;
            ViewData["CompanyName"] =(await _context.Companies.SingleOrDefaultAsync(a => a.Id == esal.CompanyID)).Company_Name;

            if (esal == null)
            {
                return NotFound();
            }

            return View(esal);
        }



        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////// Esals/CreateReligious **** الإيصالات - إنشاء إيصال سياحه دينيه  **** /////////////////////////////////
        public IActionResult CreateReligious()
        {
            ViewData["CompanyID"] = new SelectList(_context.Companies, "Id", "Company_Name");
            ViewData["CustomerOrSupplierId"] = new SelectList(_context.CustomerOrSuppliers.Where(a => a.Name != "مورد او وكيل").Where(a => a.Name != "جارى الشركاء"), "Id", "Name");
            ViewData["CustomerSupplierId"] = new SelectList("", "Id", "Name");
            ViewData["MenuLE0Id"] = new SelectList(_context.MenuLE0.Where(a => a.M0_Name == "سياحة دينية"), "Id", "M0_Name");
            ViewData["MenuLE1Id"] = new SelectList("", "Id", "M0_Name");
            ViewData["MenuLE2Id"] = new SelectList("", "Id", "M0_Name");
            ViewData["PaymentMethodId"] = new SelectList(_context.paymentMethods, "Id", "Name");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }
        // POST: Esals1/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateReligious([Bind("Id,CustomerOrSupplierId,CustomerSupplierId,MenuLE0Id,MenuLE1Id,MenuLE2Id,AmountPaid,PaymentMethodId,DepositDesc")] Esal esal)
        {
            if (esal.MenuLE1Id is null || esal.MenuLE1Id == 0)
            {
                ModelState.AddModelError("MenuLE1Id", "من فضلك ادخل اسم القائمه الفرعيه 1");
            }
            if (esal.MenuLE2Id == null || esal.MenuLE2Id == 0)
            {
                ModelState.AddModelError("MenuLE2Id", "من فضلك ادخل اسم القائمه الفرعيه 2");
            }
            var CompanyId =(await _userManager.Users.SingleOrDefaultAsync(a => a.Id == _userManager.GetUserId(User))).CompanyId;

            if (ModelState.IsValid)
            {
                var CompanyCuntry =(await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyId)).DateTimeCountry;
                var info = TimeZoneInfo.FindSystemTimeZoneById(CompanyCuntry);
                DateTimeOffset localServerTime = DateTimeOffset.Now;
                DateTimeOffset localTime = TimeZoneInfo.ConvertTime(localServerTime, info);
                esal.EsalDate = localTime.DateTime;
                esal.UserId = _userManager.GetUserId(User);
                esal.CompanyID = CompanyId;
                if (_context.Esals.Where(a => a.CompanyID == esal.CompanyID).Count() == 0)
                {
                    esal.EsalId = 1;
                }
                else
                {
                    esal.EsalId = _context.Esals.Where(a => a.CompanyID == esal.CompanyID).Max(a => a.EsalId) + 1;
                }
                _context.Add(esal);
                await _context.SaveChangesAsync();
                return RedirectToAction("Print", new RouteValueDictionary(
                new { controller = "Esals", action = "Print", Id = esal.Id }));
            }
            ViewData["CustomerOrSupplierId"] = new SelectList(_context.CustomerOrSuppliers.Where(a => a.Name != "مورد او وكيل").Where(a => a.Name != "جارى الشركاء"), "Id", "Name", esal.CustomerOrSupplierId);
            ViewData["CustomerSupplierId"] = new SelectList(_context.CustomersSuppliers.Where(a=>a.CompanyId == CompanyId).Where(x=>x.CustomerOrSupplierId == esal.CustomerOrSupplierId), "Id", "Name", esal.CustomerSupplierId);
            ViewData["MenuLE0Id"] = new SelectList(_context.MenuLE0.Where(a => a.M0_Name == "سياحة دينية"), "Id", "M0_Name", esal.MenuLE0Id);
            ViewData["MenuLE1Id"] = new SelectList(_context.MenuLE1.Where(a => a.M1_Name == "سياحة دينية").Where(a=>a.MenuLE0Id == esal.MenuLE0Id), "Id", "M0_Name", esal.MenuLE0Id);
            ViewData["MenuLE2Id"] = new SelectList(_context.MenuLE2.Where(a => a.M2_Name == "سياحة دينية").Where(a => a.MenuLE1Id == esal.MenuLE1Id), "Id", "M0_Name", esal.MenuLE0Id);
            ViewData["PaymentMethodId"] = new SelectList(_context.paymentMethods, "Id", "Name", esal.PaymentMethodId);
            return View(esal);
        }



        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////// Esals/Create **** الإيصالات - إنشاء  **** /////////////////////////////////////
        public IActionResult Create()
        {
            ViewData["CompanyID"] = new SelectList(_context.Companies, "Id", "Company_Name");
            ViewData["CustomerOrSupplierId"] = new SelectList(_context.CustomerOrSuppliers.Where(a => a.Name != "مندوب"), "Id", "Name");
            ViewData["CustomerSupplierId"] = new SelectList(_context.CustomersSuppliers, "Id", "Name");
            ViewData["MenuLE0Id"] = new SelectList(_context.MenuLE0.Where(a => a.M0_Name != "سياحة دينية"), "Id", "M0_Name");
            ViewData["PaymentMethodId"] = new SelectList(_context.paymentMethods, "Id", "Name");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Esals1/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CustomerOrSupplierId,CustomerSupplierId,MenuLE0Id,MenuLE1Id,MenuLE2Id,AmountPaid,PaymentMethodId,DepositDesc")] Esal esal)
        {
            if (esal.DepositDesc == null)
            {
                ModelState.AddModelError("ادخل وصف", "اكمل جميع القوائم");
            }
            if (ModelState.IsValid)
            {
                var CompanyId =(await _userManager.Users.SingleOrDefaultAsync(a => a.Id == _userManager.GetUserId(User))).CompanyId;
                var CompanyCuntry =(await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyId)).DateTimeCountry;
                var info = TimeZoneInfo.FindSystemTimeZoneById(CompanyCuntry);
                DateTimeOffset localServerTime = DateTimeOffset.Now;
                DateTimeOffset localTime = TimeZoneInfo.ConvertTime(localServerTime, info);
                esal.EsalDate = localTime.DateTime;
                esal.UserId = _userManager.GetUserId(User);
                esal.CompanyID = CompanyId;
                if (_context.Esals.Where(a => a.CompanyID == esal.CompanyID).Count() == 0)
                {
                    esal.EsalId = 1;
                }
                else
                {
                    esal.EsalId = _context.Esals.Where(a => a.CompanyID == esal.CompanyID).Max(a => a.EsalId) + 1;
                }
                _context.Add(esal);
                await _context.SaveChangesAsync();
                return RedirectToAction("Print", new RouteValueDictionary(
                new { controller = "Esals", action = "Print", Id = esal.Id }));
            }
            ViewData["CustomerOrSupplierId"] = new SelectList(_context.CustomerOrSuppliers, "Id", "Name", esal.CustomerOrSupplierId);
            ViewData["CustomerSupplierId"] = new SelectList(_context.CustomersSuppliers, "Id", "Name", esal.CustomerSupplierId);
            ViewData["MenuLE0Id"] = new SelectList(_context.MenuLE0.Where(a => a.M0_Name != "سياحة دينية"), "Id", "M0_Name", esal.MenuLE0Id);
            ViewData["PaymentMethodId"] = new SelectList(_context.paymentMethods, "Id", "Name", esal.PaymentMethodId);
            return View(esal);
        }



        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////// Esals/CreateGroup **** الإيصالات لمجموعه - إنشاء   **** /////////////////////////////////////
        public IActionResult CreateGroup()
        {
            ViewData["CompanyID"] = new SelectList(_context.Companies, "Id", "Company_Name");
            ViewData["CustomerOrSupplierId"] = new SelectList(_context.CustomerOrSuppliers.Where(a => a.Name != "مندوب"), "Id", "Name");
            ViewData["CustomerSupplierId"] = new SelectList(_context.CustomersSuppliers, "Id", "Name");
            ViewData["MenuLE0Id"] = new SelectList(_context.MenuLE0.Where(a => a.M0_Name != "سياحة دينية"), "Id", "M0_Name");
            ViewData["PaymentMethodId"] = new SelectList(_context.paymentMethods, "Id", "Name");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Esals1/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateGroup([Bind("Id,CustomerOrSupplierId,CustomerSupplierId,MenuLE0Id,MenuLE1Id,MenuLE2Id,AmountPaid,PaymentMethodId,DepositDesc")] Esal esal)
        {
            if (esal.DepositDesc == null)
            {
                ModelState.AddModelError("ادخل وصف", "اكمل جميع القوائم");
            }
            if (esal.MenuLE1Id == null || esal.MenuLE1Id == 0)
            {
                ModelState.AddModelError("ادخل وصف", "من فضلك اختر قيمه من القائمه المنسدله 1");
            }

            if (esal.MenuLE2Id == null || esal.MenuLE2Id == 0)
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
                esal.EsalDate = localTime.DateTime;
                esal.UserId = _userManager.GetUserId(User);
                esal.CompanyID = CompanyId;
                if (_context.Esals.Where(a => a.CompanyID == esal.CompanyID).Count() == 0)
                {
                    esal.EsalId = 1;
                }
                else
                {
                    esal.EsalId = _context.Esals.Where(a => a.CompanyID == esal.CompanyID).Max(a => a.EsalId) + 1;
                }
                _context.Add(esal);
                await _context.SaveChangesAsync();
                return RedirectToAction("Print", new RouteValueDictionary(
                new { controller = "Esals", action = "Print", Id = esal.Id }));
            }
            ViewData["CustomerOrSupplierId"] = new SelectList(_context.CustomerOrSuppliers, "Id", "Name", esal.CustomerOrSupplierId);
            ViewData["CustomerSupplierId"] = new SelectList(_context.CustomersSuppliers, "Id", "Name", esal.CustomerSupplierId);
            ViewData["MenuLE0Id"] = new SelectList(_context.MenuLE0.Where(a => a.M0_Name != "سياحة دينية"), "Id", "M0_Name", esal.MenuLE0Id);
            ViewData["MenuLE1Id"] = new SelectList(_context.MenuLE1.Where(a=>a.MenuLE0Id == esal.MenuLE0Id), "Id", "M1_Name",esal.MenuLE1Id);
            ViewData["MenuLE2Id"] = new SelectList(_context.MenuLE2.Where(a=>a.MenuLE1Id == esal.MenuLE1Id), "Id", "M2_Name",esal.MenuLE2Id);
            ViewData["PaymentMethodId"] = new SelectList(_context.paymentMethods, "Id", "Name", esal.PaymentMethodId);
            return View(esal);
        }




        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////// Esals/Edit **** الإيصالات - تعديل  **** /////////////////////////////////////

        [Authorize(Roles = CustomRoles.Admin + "," + CustomRoles.BranchManager + "," + CustomRoles.Edits)]
        // GET: Esals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var esal = await _context.Esals.SingleOrDefaultAsync(m => m.Id == id);
            if (esal == null)
            {
                return NotFound();
            }
            ViewData["CustomerOrSupplierId"] = new SelectList(_context.CustomerOrSuppliers, "Id", "Name", esal.CustomerOrSupplierId);
            ViewData["CustomerSupplierId"] = new SelectList(_context.CustomersSuppliers.Where(a => a.CustomerOrSupplierId == esal.CustomerOrSupplierId).Where(A => A.CompanyId == _userManager.Users.SingleOrDefault(a => a.Id == _userManager.GetUserId(User)).CompanyId).OrderBy(a => a.Name), "Id", "Name", esal.CustomerSupplierId);
            ViewData["MenuLE0Id"] = new SelectList(_context.MenuLE0, "Id", "M0_Name", esal.MenuLE0Id);
            ViewData["MenuLE1Id"] = new SelectList(_context.MenuLE1.Where(a => a.MenuLE0Id == esal.MenuLE0Id), "Id", "M1_Name", esal.MenuLE1Id);
            ViewData["MenuLE2Id"] = new SelectList(_context.MenuLE2.Where(a => a.MenuLE1Id == esal.MenuLE1Id), "Id", "M2_Name", esal.MenuLE2Id);
            ViewData["TicketExportId"] = new SelectList(_context.CustomersSuppliers.Where(A => A.CompanyId == _userManager.Users.SingleOrDefault(a => a.Id == _userManager.GetUserId(User)).CompanyId).OrderBy(a => a.Name), "Id", "Name", esal.TicketExportId);
            ViewData["PaymentMethodId"] = new SelectList(_context.paymentMethods, "Id", "Name", esal.PaymentMethodId);
            return View(esal);
        }

        [Authorize(Roles = CustomRoles.Admin + "," + CustomRoles.BranchManager + "," + CustomRoles.Edits)]
        // POST: Esals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EsalId,EsalDate,BillId,BillIdId,CustomerOrSupplierId,CustomerSupplierId,TicketExportId,MenuLE0Id,MenuLE1Id,MenuLE2Id,DepositDesc,AmountPaid,PaymentMethodId,CompanyID,UserId")] Esal esal)
        {
            if (id != esal.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(esal);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EsalExists(esal.Id))
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
            ViewData["BillId"] = new SelectList(_context.BillAirLines, "Id", "Id", esal.BillId);
            ViewData["MenuLE0Id"] = new SelectList(_context.MenuLE0, "Id", "M0_Name", esal.MenuLE0Id);
            ViewData["PaymentMethodId"] = new SelectList(_context.paymentMethods, "Id", "Name", esal.PaymentMethodId);
            return View(esal);
        }



        // GET: Esals
        public async Task<IActionResult> PrintEsalByDate()
        {
         
            var applicationDbContext = _context.Esals.Include(e => e.MenuLE0).Include(e => e.PaymentMethod).Include(e => e.CustomerSupplier).Where(d => d.EsalDate.Date >= Convert.ToDateTime(TempData["StartDate"])).Where(d => d.EsalDate.Date <= Convert.ToDateTime(TempData["EndDate"]));
            var UserId = _userManager.GetUserId(User);
            var CompanyID =(await _userManager.Users.SingleOrDefaultAsync(a => a.Id == _userManager.GetUserId(User))).CompanyId;
            ViewData["CompanyLogo"] = "~/" + TravelERP.Properties.Resources.ImgFolder + "/" + _context.Companies.SingleOrDefault(a => a.Id == CompanyID).CompanyLogo;
            ViewData["CompanyA4EsalImage"] = "~/" + TravelERP.Properties.Resources.ImgFolder + "/" + _context.Companies.SingleOrDefault(a => a.Id == CompanyID).CompanyA4EsalImage;
            ViewData["CompanyNameE"] =(await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyID)).Company_NameE;
            ViewData["CompanyName"] =(await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyID)).Company_Name;

            return View(await applicationDbContext.ToListAsync());

        }
        // GET: EsalDate
        [HttpGet]
        public IActionResult EsalDate()
        {
            return View();
        }

        // Post: EsalDate
        [HttpPost]

        public IActionResult EsalDate(DatesSearch DS)
        {
            TempData["StartDate"] = DS.StartDate.Value.Date;

            return RedirectToAction("PrintEsalByDate");
        }

        //////////////////////////////////////////////////////////////////////////////////////
        /////////// Esals/EsalsDate **** التقارير - تقارير إيصالات الاستلام **** ////////////
        public IActionResult EsalsDate()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EsalsDate([Bind("StartDate,EndDate")] DatesSearch datesSearch)
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
                return RedirectToAction("StatementEsals", "Esals");
            }
            return View(datesSearch);
        }
        public async Task<ActionResult> StatementEsals()
        {
            var CompanyID =(await _context.Users.SingleOrDefaultAsync(a => a.Id == _userManager.GetUserId(User))).CompanyId;
            var ezns =await( from x in _context.Esals
                   .Where(a=>a.CompanyID == CompanyID)
                   .Where(d => d.EsalDate.Date >= Convert.ToDateTime(TempData["StartDate"]))
                   .Where(d => d.EsalDate.Date <= Convert.ToDateTime(TempData["EndDate"]))
                       select new Models.ViewModel.Transactions { EId = x.EsalId, EDate = x.EsalDate, Name3 = string.Concat(x.CustomerOrSupplier.Name + " - ", x.CustomerSupplier.Name), Statement = string.Concat( x.MenuLE0.M0_Name + " - ", x.MenuLE1.M1_Name + " - ", x.MenuLE2.M2_Name + " - ", x.DepositDesc), Credit = x.AmountPaid }).ToListAsync();
            ViewData["CompanyLogo"] = "~/" + TravelERP.Properties.Resources.ImgFolder + "/" + _context.Companies.SingleOrDefault(a => a.Id == CompanyID).CompanyLogo;
            ViewData["CompanyA4EsalImage"] = "~/" + TravelERP.Properties.Resources.ImgFolder + "/" + _context.Companies.SingleOrDefault(a => a.Id == CompanyID).CompanyA4EsalImage;
            ViewData["CompanyNameE"] =(await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyID)).Company_NameE;
            ViewData["CompanyName"] =(await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyID)).Company_Name;
            ViewData["CompanyId"] = CompanyID;

            return View(ezns);
        }




        private bool EsalExists(int id)
        {
            return _context.Esals.Any(e => e.Id == id);
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
