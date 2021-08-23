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
    [Authorize(Roles = CustomRoles.Admin + "," + CustomRoles.BranchManager + "," + CustomRoles.BillReligious)]

    public class BillReligiousController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public BillReligiousController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        // GET: BillReligious/Index   **** الصفحه الرئيسيه لفواتير السياحه الدينيه ****
        public async Task<IActionResult> Index()
        {
            var CompanyId =(await _userManager.Users.SingleOrDefaultAsync(a => a.Id == _userManager.GetUserId(User))).CompanyId;
            var CompanyCuntry =(await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyId)).DateTimeCountry;
            var info = TimeZoneInfo.FindSystemTimeZoneById(CompanyCuntry);
            DateTimeOffset localServerTime = DateTimeOffset.Now;
            DateTimeOffset localTime = TimeZoneInfo.ConvertTime(localServerTime, info);
            DateTime datetimenow = localTime.DateTime;
            ViewData["DateTimeNow"] = datetimenow.Date;

            return View();
        }

        // GET: BillReligious/Details/5   **** تفاصيل فاتوره السياحه الدينيه ****
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var billReligious = await _context.BillReligious
                .Include(b => b.CustomerOrSupplier)
                .Include(b => b.CustomerSupplier)
                .Include(b => b.MenuLE0)
                .Include(b => b.MenuLE1)
                .Include(b => b.MenuLE2)
                .Include(b => b.User)
                .Include(b => b.Company)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (billReligious == null)
            {
                return NotFound();
            }

            return View(billReligious);
        }


        [Authorize(Roles = CustomRoles.Admin + "," + CustomRoles.BranchManager + "," + CustomRoles.EsalReligious)]
        // GET: BillReligious/Create **** إنشاء فاتوره سياحه دينيه جديده *****
        public IActionResult Create()
        {
            ViewData["CustomerOrSupplierId"] = new SelectList(_context.CustomerOrSuppliers, "Id", "Name");
            ViewData["CustomerSupplierId"] = new SelectList(_context.CustomersSuppliers.Where(A => A.CompanyId == _userManager.Users.SingleOrDefault(a => a.Id == _userManager.GetUserId(User)).CompanyId).Where(a => a.CustomerOrSupplier.Name == "مندوب").OrderBy(a => a.Name), "Id", "Name");
            ViewData["MenuLE0Id"] = new SelectList(_context.MenuLE0, "Id", "M0_Name");
            ViewData["MenuLE1Id"] = new SelectList(_context.MenuLE1.Where(a => a.MenuLE0.M0_Name == "سياحة دينية"), "Id", "M1_Name");
            ViewData["MenuLE2Id"] = new SelectList(_context.MenuLE2, "Id", "M2_Name");
            return View();
        }



        [Authorize(Roles = CustomRoles.Admin + "," + CustomRoles.BranchManager + "," + CustomRoles.EsalReligious)]
        // POST: BillReligious/Create **** إنشاء فاتوره سياحه دينيه جديده *****
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CustomerSupplierId,MenuLE0Id,MenuLE1Id,MenuLE2Id,CustomerPrice,Commnets")] BillReligious billReligious)
        {
            if (ModelState.IsValid)
            {
                var CompanyId =(await _userManager.Users.SingleOrDefaultAsync(a => a.Id == _userManager.GetUserId(User))).CompanyId;
                var CompanyCuntry =(await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyId)).DateTimeCountry;
                var info = TimeZoneInfo.FindSystemTimeZoneById(CompanyCuntry);
                DateTimeOffset localServerTime = DateTimeOffset.Now;
                DateTimeOffset localTime = TimeZoneInfo.ConvertTime(localServerTime, info);
                billReligious.BillDate = localTime.DateTime;
                billReligious.MenuLE0Id =(await _context.MenuLE0.SingleOrDefaultAsync(a => a.M0_Name == "سياحة دينية")).Id;
                billReligious.CustomerOrSupplierId =(await _context.CustomerOrSuppliers.SingleOrDefaultAsync(a => a.Name == "مندوب")).Id;
                billReligious.UserId = _userManager.GetUserId(User);
                billReligious.CompanyID = CompanyId;
                if (_context.BillReligious.Where(a => a.CompanyID == billReligious.CompanyID).Count() == 0)
                {
                    billReligious.BillId = 1;
                    TempData["BillId"] = 1;
                }
                else
                {
                    billReligious.BillId = _context.BillReligious.Where(a => a.CompanyID == billReligious.CompanyID).Max(a => a.BillId) + 1;
                    TempData["BillId"] = billReligious.BillId;
                }
                _context.Add(billReligious);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", new RouteValueDictionary(
                        new { controller = "BillReligious", action = "Details", Id = billReligious.Id }));
            }
            ViewData["CustomerOrSupplierId"] = new SelectList(_context.CustomerOrSuppliers, "Id", "Name", billReligious.CustomerOrSupplierId);
            ViewData["CustomerSupplierId"] = new SelectList(_context.CustomersSuppliers, "Id", "Name", billReligious.CustomerSupplierId);
            ViewData["MenuLE0Id"] = new SelectList(_context.MenuLE0, "Id", "M0_Name", billReligious.MenuLE0Id);
            ViewData["MenuLE1Id"] = new SelectList(_context.MenuLE1, "Id", "M1_Name", billReligious.MenuLE1Id);
            ViewData["MenuLE2Id"] = new SelectList(_context.MenuLE2, "Id", "M2_Name", billReligious.MenuLE2Id);
            return View(billReligious);
        }



        [Authorize(Roles = CustomRoles.Admin + "," + CustomRoles.BranchManager + "," + CustomRoles.Edits)]
        // GET: BillReligious/Edit/5    **** تعديل فاتوره سياحه دينيه ****
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var billReligious = await _context.BillReligious.SingleOrDefaultAsync(m => m.Id == id);
            if (billReligious == null)
            {
                return NotFound();
            }
            var CompanyId =(await _userManager.Users.SingleOrDefaultAsync(a => a.Id == billReligious.UserId)).CompanyId;
            ViewData["CustomerOrSupplierId"] = new SelectList(_context.CustomerOrSuppliers, "Id", "Name", billReligious.CustomerOrSupplierId);
            ViewData["CustomerSupplierId"] = new SelectList(_context.CustomersSuppliers.Where(A => A.CompanyId == CompanyId).Where(a => a.CustomerOrSupplier.Name == "مندوب").OrderBy(a => a.Name), "Id", "Name");
            ViewData["MenuLE0Id"] = new SelectList(_context.MenuLE0, "Id", "M0_Name", billReligious.MenuLE0Id);
            ViewData["MenuLE1Id"] = new SelectList(_context.MenuLE1.Where(a => a.MenuLE0.M0_Name == "سياحة دينية"), "Id", "M1_Name", billReligious.MenuLE1Id);
            ViewData["MenuLE2Id"] = new SelectList(_context.MenuLE2.Where(a => a.MenuLE1Id == billReligious.MenuLE1Id).OrderBy(a => a.M2_Name), "Id", "M2_Name", billReligious.MenuLE2Id);
            ViewData["UserId"] = new SelectList(_context.Users.Where(a => a.CompanyId == CompanyId), "Id", "UserName", billReligious.UserId);
            return View(billReligious);
        }



        [Authorize(Roles = CustomRoles.Admin + "," + CustomRoles.BranchManager + "," + CustomRoles.Edits)]
        // POST: BillReligious/Edit/5    **** تعديل فاتوره سياحه دينيه ****
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BillId,BillDate,CustomerOrSupplierId,CustomerSupplierId,MenuLE0Id,MenuLE1Id,MenuLE2Id,CustomerPrice,Commnets,CompanyID,UserId")] BillReligious billReligious)
        {
            if (id != billReligious.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(billReligious);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BillReligiousExists(billReligious.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", new RouteValueDictionary(
                        new { controller = "BillReligious", action = "Details", Id = billReligious.Id }));
            }
            var CompanyId =(await _userManager.Users.SingleOrDefaultAsync(a => a.Id == billReligious.UserId)).CompanyId;
            ViewData["CustomerOrSupplierId"] = new SelectList(_context.CustomerOrSuppliers, "Id", "Name", billReligious.CustomerOrSupplierId);
            ViewData["CustomerSupplierId"] = new SelectList(_context.CustomersSuppliers.Where(A => A.CompanyId == CompanyId).Where(a => a.CustomerOrSupplier.Name == "مندوب").OrderBy(a => a.Name), "Id", "Name", billReligious.CustomerSupplierId);
            ViewData["MenuLE0Id"] = new SelectList(_context.MenuLE0, "Id", "M0_Name", billReligious.MenuLE0Id);
            ViewData["MenuLE1Id"] = new SelectList(_context.MenuLE1, "Id", "M1_Name", billReligious.MenuLE1Id);
            ViewData["MenuLE2Id"] = new SelectList(_context.MenuLE2, "Id", "M2_Name", billReligious.MenuLE2Id);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", billReligious.UserId);
            return View(billReligious);
        }



        // GET: BillReligious/CreateEsal/5    **** إنشاء ايصال لفاتوره سياحه دينيه  ****
        [Authorize(Roles = CustomRoles.Admin + "," + CustomRoles.BranchManager + "," + CustomRoles.EsalReligious)]
        public async Task<IActionResult> CreateEsal(int? Id)
        {
            var bill =await _context.BillReligious.SingleOrDefaultAsync(a => a.Id == Id);

            ViewData["IDBill"] = bill.Id;
            ViewData["BillID"] = bill.BillId;
            ViewData["MenuLE0Id"] = bill.MenuLE0Id;
            ViewData["MenuLE1Id"] = bill.MenuLE1Id;
            ViewData["MenuLE2Id"] = bill.MenuLE2Id;
            ViewData["CustomerSupplierId"] = bill.CustomerSupplierId;
            ViewData["CustomerOrSupplierId"] = bill.CustomerOrSupplierId;
            ViewData["CustomerName"] = " " + _context.CustomersSuppliers.SingleOrDefault(a => a.Id == bill.CustomerSupplierId).Name + " - " + _context.CustomerOrSuppliers.SingleOrDefault(a => a.Id == bill.CustomerOrSupplierId).Name;
            ViewData["Byan"] = "ذلك مقابل حجز " + _context.MenuLE0.SingleOrDefault(a => a.Id == bill.MenuLE0Id).M0_Name + " - " + _context.MenuLE2.SingleOrDefault(a => a.Id == bill.MenuLE2Id).M2_Name + " - وذلك مقابل " + (bill.CompanyID == 6 ? "درهم" : " ج ");
            ViewData["BillDate"] = bill.BillDate;
            ViewData["PaymentMethodId"] = new SelectList(_context.paymentMethods, "Id", "Name");
            return View();
        }

        [Authorize(Roles = CustomRoles.Admin + "," + CustomRoles.BranchManager + "," + CustomRoles.EsalReligious)]
        // POST: BillReligious/CreateEsal/5     **** إنشاء ايصال لفاتوره سياحه دينيه  ****
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEsal([Bind("BillId,BillIdId,MenuLE0Id,MenuLE1Id,MenuLE2Id,CustomerOrSupplierId,CustomerSupplierId,AmountPaid,PaymentMethodId,DepositDesc")] Esal esal)
        {
            if (ModelState.IsValid)
            {
                var CompanyIdi =(await _userManager.Users.SingleOrDefaultAsync(a => a.Id == _userManager.GetUserId(User))).CompanyId;

                if (_context.Esals.Where(a => a.CompanyID == _context.Users.SingleOrDefault(b => b.Id == _userManager.GetUserId(User)).CompanyId).Count() == 0)
                {
                    esal.EsalId = 1;
                }
                else
                {
                    esal.EsalId = _context.Esals.Where(a => a.CompanyID == CompanyIdi).Max(a => a.EsalId) + 1;
                }
                esal.UserId = _userManager.GetUserId(User);
                var CompanyId =(await _userManager.Users.SingleOrDefaultAsync(a => a.Id == esal.UserId)).CompanyId;
                var CompanyCuntry =(await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyId)).DateTimeCountry;
                var info = TimeZoneInfo.FindSystemTimeZoneById(CompanyCuntry);
                DateTimeOffset localServerTime = DateTimeOffset.Now;
                DateTimeOffset localTime = TimeZoneInfo.ConvertTime(localServerTime, info);
                esal.EsalDate = localTime.DateTime;
                esal.CompanyID = CompanyId;
                _context.Add(esal);
                await _context.SaveChangesAsync();
                return RedirectToAction("Print", new RouteValueDictionary(
                new { controller = "Esals", action = "Print", Id = esal.Id }));
            }
            var bill =await _context.BillReligious.SingleOrDefaultAsync(a => a.Id == esal.BillId);

            ViewData["IDBill"] = bill.Id;
            ViewData["BillID"] = bill.BillId;
            ViewData["MenuLE0Id"] = bill.MenuLE0Id;
            ViewData["MenuLE1Id"] = bill.MenuLE1Id;
            ViewData["MenuLE2Id"] = bill.MenuLE2Id;
            ViewData["CustomerSupplierId"] = bill.CustomerSupplierId;
            ViewData["CustomerOrSupplierId"] = bill.CustomerOrSupplierId;
            ViewData["CustomerName"] = " " + _context.CustomersSuppliers.SingleOrDefault(a => a.Id == bill.CustomerSupplierId).Name + " - " + _context.CustomerOrSuppliers.SingleOrDefault(a => a.Id == bill.CustomerOrSupplierId).Name;
            ViewData["Byan"] = "ذلك مقابل حجز " + _context.MenuLE0.SingleOrDefault(a => a.Id == bill.MenuLE0Id).M0_Name + " - " + _context.MenuLE2.SingleOrDefault(a => a.Id == bill.MenuLE2Id).M2_Name + " - وذلك مقابل " + (bill.CompanyID == 6 ? "درهم" : " ج ");
            ViewData["BillDate"] = bill.BillDate;
            ViewData["PaymentMethodId"] = new SelectList(_context.paymentMethods, "Id", "Name");
            return View(esal);
        }



        // GET: BillReligious/CSStatementDate  **** التقارير / التقارير المفصله / سياحه دينيه ****
        public async Task<IActionResult> CSStatementDate()
        {
            var CompanyId =(await _userManager.Users.SingleOrDefaultAsync(a => a.Id == _userManager.GetUserId(User))).CompanyId;
            ViewData["CustomerOrSupplierId"] = new SelectList(_context.CustomerOrSuppliers, "Id", "Name");
            var CustomersReligious = (from x in _context.Esals.Where(a => a.CompanyID == CompanyId).Where(a => a.CustomerOrSupplier.Name == "عميل").Where(a => a.MenuLE0.M0_Name == "سياحة دينية").GroupBy(a => a.CustomerSupplierId) select new CustomerSupplier { Id = x.Key, Name = _context.CustomersSuppliers.SingleOrDefault(a => a.Id == x.Key).Name });
            ViewData["CustomerId"] = new SelectList(CustomersReligious.OrderBy(a => a.Name), "Id", "Name");
            ViewData["CustomerSupplierId"] = new SelectList(_context.CustomersSuppliers.Where(a => a.CustomerOrSupplier.Name == "مندوب").Where(A => A.CompanyId == _userManager.Users.SingleOrDefault(a => a.Id == _userManager.GetUserId(User)).CompanyId).OrderBy(a => a.Name), "Id", "Name");
            ViewData["MenuLE1Id"] = new SelectList(_context.MenuLE1.Where(a => a.MenuLE0.M0_Name == "سياحة دينية"), "Id", "M1_Name");
            ViewData["MenuLE2Id"] = new SelectList(_context.MenuLE2.Where(a => a.MenuLE1.M1_Name == "سياحة دينية").OrderBy(a => a.M2_Name), "Id", "M2_Name");

            return View();
        }


        // POST: BillReligious/CSStatementDate  **** التقارير / التقارير المفصله / سياحه دينيه ****
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CSStatementDate([Bind("StartDate,EndDate,Name1,Name2")] DatesSearch datesSearch)
        {
            if (ModelState.IsValid)
            {
                TempData["StartDate"] = datesSearch.StartDate.Value.Date.ToString();
                TempData["EndDate"] = datesSearch.EndDate.Value.Date.ToString();
                TempData["Name1"] = datesSearch.Name1;
                TempData["Name2"] = datesSearch.Name2;
                TempData["Name4"] = datesSearch.Name4;
                await _context.SaveChangesAsync();
                return RedirectToAction("CSStatement", "BillReligious");
            }
            return View(datesSearch);
        }


        // **** التقارير / التقارير المفصله / سياحه دينيه/ تقرير معاملات عملاء الحج والعمره ****


        // POST: BillReligious/CStatementDate  **** التقارير / التقارير المفصله / سياحه دينيه/ تقرير معاملات عملاء الحج والعمره ****
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CStatementDate([Bind("EndDate,EndDate,Name4")] DatesSearch datesSearch)
        {
            if (ModelState.IsValid)
            {
                TempData["StartDate"] = new DateTime(2019, 11, 27);
                TempData["EndDate"] = datesSearch.EndDate.Value.Date.ToString();
                TempData["Name4"] = datesSearch.Name4;
                await _context.SaveChangesAsync();
                return RedirectToAction("CStatement", "BillReligious");
            }
            return View(datesSearch);
        }


        // POST: BillReligious/CStatement  **** التقارير / التقارير المفصله / سياحه دينيه/ تقرير معاملات عملاء الحج والعمره ****
        public async Task<ActionResult> CStatement()
        {
            var ezn =await (from x in _context.Ezns.Where(a => a.MenuLE0.M0_Name == "سياحة دينية").Where(d => d.EznDate >= Convert.ToDateTime(TempData["StartDate"])).Where(d => d.EznDate.Date <= Convert.ToDateTime(TempData["EndDate"])).Where(a => a.CustomerOrSupplier.Name == "عميل").Where(A => A.CompanyID == _userManager.Users.SingleOrDefault(a => a.Id == _userManager.GetUserId(User)).CompanyId).Where(d => d.MenuLE2Id == Convert.ToInt32(TempData["Name4"])) select new Models.ViewModel.Transactions { EId = x.EznId, Type = "إذن ", EDate = x.EznDate, Statement = string.Concat(x.MenuLE0.M0_Name, " - ", x.MenuLE1.M1_Name, " - ", x.MenuLE2.M2_Name, " - ", x.ExpenseName), Debit = x.AmountWithdrawn * -1, Name3 = x.CustomerSupplier.Name }).ToListAsync();
            var esal =await (from x in _context.Esals.Where(a => a.MenuLE0.M0_Name == "سياحة دينية").Where(d => d.EsalDate >= Convert.ToDateTime(TempData["StartDate"])).Where(d => d.EsalDate.Date <= Convert.ToDateTime(TempData["EndDate"])).Where(a => a.CustomerOrSupplier.Name == "عميل").Where(A => A.CompanyID == _userManager.Users.SingleOrDefault(a => a.Id == _userManager.GetUserId(User)).CompanyId).Where(d => d.MenuLE2Id == Convert.ToInt32(TempData["Name4"])) select new Models.ViewModel.Transactions { EId = x.EsalId, Type = "إيصال", EDate = x.EsalDate, Statement = string.Concat(x.MenuLE0.M0_Name, " - ", x.MenuLE1.M1_Name, " - ", x.MenuLE2.M2_Name, " - ", x.DepositDesc), Debit = x.AmountPaid, Name3 = x.CustomerSupplier.Name }).ToListAsync();
            var Outbut = (esal.Concat(ezn).OrderBy(a => a.Name3));
            TempData["NameCS"] =(await _context.MenuLE2.SingleOrDefaultAsync(a => a.Id == Convert.ToInt32(TempData["Name4"]))).M2_Name;
            var UserId = _userManager.GetUserId(User);
            var CompanyID =(await _userManager.Users.SingleOrDefaultAsync(a => a.Id == _userManager.GetUserId(User))).CompanyId;
            if (Outbut == null)
            {
                return NotFound();
            }
            ViewData["CompanyLogo"] = "~/" + TravelERP.Properties.Resources.ImgFolder + "/" + _context.Companies.SingleOrDefault(a => a.Id == CompanyID).CompanyLogo;
            ViewData["CompanyA4EsalImage"] = "~/" + TravelERP.Properties.Resources.ImgFolder + "/" + _context.Companies.SingleOrDefault(a => a.Id == CompanyID).CompanyA4EsalImage;
            ViewData["CompanyNameE"] =(await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyID)).Company_NameE;
            ViewData["CompanyName"] =(await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyID)).Company_Name;
            ViewData["CompanyId"] = CompanyID;
            return View(Outbut);
        }



        //  **** التقارير / التقارير المفصله / سياحه دينيه/ تقرير معاملات مندوب الحج والعمره ****

        // POST: BillReligious/CXStatementDate  **** التقارير / التقارير المفصله / سياحه دينيه/ تقرير معاملات مندوب الحج والعمره ****
        public async Task<IActionResult> CXStatementDate([Bind("EndDate,Name1,Name2")] DatesSearch datesSearch)
        {
            if (ModelState.IsValid)
            {
                TempData["StartDate"] = new DateTime(2019, 11, 27);
                TempData["EndDate"] = datesSearch.EndDate.Value.Date.ToString();
                TempData["Name2"] = datesSearch.Name2;
                TempData["Name1"] = datesSearch.Name1;


                //_context.Add(datesSearch);
                await _context.SaveChangesAsync();
                return RedirectToAction("CXStatement", "BillReligious");
            }
            return View(datesSearch);
        }

        // POST: BillReligious/CXStatement  **** التقارير / التقارير المفصله / سياحه دينيه/ تقرير معاملات مندوب الحج والعمره ****
        public async Task <ActionResult> CXStatement()
        {
            //StartTransactions
            var startBillReligiousEsal =await ((from x in _context.BillReligious.Where(A => A.CompanyID == _userManager.Users.SingleOrDefault(a => a.Id == _userManager.GetUserId(User)).CompanyId).Where(a => a.MenuLE1Id == Convert.ToInt32(TempData["Name1"])) select new Models.ViewModel.Transactions { EDate = x.BillDate, Credit = x.CustomerPrice, Name2 = x.CustomerSupplierId }).Where(d => d.EDate < Convert.ToDateTime(TempData["StartDate"])).Where(d => d.Name2 == Convert.ToInt32(TempData["Name2"]))).ToListAsync();
            var startEsal =await ((from x in _context.Esals.Where(a => a.MenuLE0.M0_Name == "سياحة دينية").Where(A => A.CompanyID == _userManager.Users.SingleOrDefault(a => a.Id == _userManager.GetUserId(User)).CompanyId).Where(a => a.MenuLE1Id == Convert.ToInt32(TempData["Name1"])) select new Models.ViewModel.Transactions { EDate = x.EsalDate, Debit = x.AmountPaid, Name2 = x.CustomerSupplierId }).Where(d => d.EDate < Convert.ToDateTime(TempData["StartDate"])).Where(d => d.Name2 == Convert.ToInt32(TempData["Name2"]))).ToListAsync();
            var startEzn =await ((from x in _context.Ezns.Where(a => a.MenuLE0.M0_Name == "سياحة دينية").Where(A => A.CompanyID == _userManager.Users.SingleOrDefault(a => a.Id == _userManager.GetUserId(User)).CompanyId).Where(a => a.MenuLE1Id == Convert.ToInt32(TempData["Name1"])) select new Models.ViewModel.Transactions { EDate = x.EznDate, Credit = x.AmountWithdrawn, Name2 = x.CustomerSupplierId }).Where(d => d.EDate < Convert.ToDateTime(TempData["StartDate"])).Where(d => d.Name2 == Convert.ToInt32(TempData["Name2"]))).ToListAsync();
            ViewData["startTransactions"] = (startEsal.Sum(a => a.Debit)) - (startEzn.Sum(a => a.Credit) + startBillReligiousEsal.Sum(a => a.Credit));
            ViewData["OpeningBalance"] = (from x in _context.OpeningBalances.Where(a => a.MenuLE0.M0_Name == "سياحة دينية").Where(A => A.CompanyID == _userManager.Users.SingleOrDefault(a => a.Id == _userManager.GetUserId(User)).CompanyId) select new { x.Balance, Name2 = x.CustomerSupplierId }).Where(d => d.Name2 == Convert.ToInt32(TempData["Name2"])).Sum(a => a.Balance);

            //For Head Name
            TempData["NameCS"] = _context.CustomersSuppliers.SingleOrDefault(a => a.Id == Convert.ToInt32(TempData["Name2"])).Name;

            var billReligiousEsal =await ((from x in _context.BillReligious.Where(A => A.CompanyID == _userManager.Users.SingleOrDefault(a => a.Id == _userManager.GetUserId(User)).CompanyId).Where(a => a.MenuLE1Id == Convert.ToInt32(TempData["Name1"])) select new Models.ViewModel.Transactions { EId = x.BillId, Type = "فاتوره", EDate = x.BillDate, Statement = string.Concat(x.MenuLE0.M0_Name, " - ", x.MenuLE1.M1_Name, " - ", x.MenuLE2.M2_Name, " - ", x.Commnets), Credit = x.CustomerPrice, Name2 = x.CustomerSupplierId }).Where(d => d.EDate >= Convert.ToDateTime(TempData["StartDate"])).Where(d => d.EDate.Date <= Convert.ToDateTime(TempData["EndDate"])).Where(d => d.Name2 == Convert.ToInt32(TempData["Name2"]))).ToListAsync();
            var esalBill = await ((from x in _context.Esals.Where(a => a.BillId != null).Where(a => a.MenuLE0.M0_Name == "سياحة دينية").Where(A => A.CompanyID == _userManager.Users.SingleOrDefault(a => a.Id == _userManager.GetUserId(User)).CompanyId).Where(a => a.MenuLE1Id == Convert.ToInt32(TempData["Name1"])) select new Models.ViewModel.Transactions { EId = x.EsalId, Type = "إيصال", EDate = x.EsalDate, Statement = string.Concat(x.MenuLE0.M0_Name, " - ", x.MenuLE1.M1_Name, " - ", x.MenuLE2.M2_Name), Debit = x.AmountPaid, Name1 = x.CustomerOrSupplierId, Name2 = x.CustomerSupplierId }).Where(d => d.EDate >= Convert.ToDateTime(TempData["StartDate"])).Where(d => d.EDate.Date <= Convert.ToDateTime(TempData["EndDate"])).Where(d => d.Name2 == Convert.ToInt32(TempData["Name2"]))).ToListAsync();
            var esal = await ((from x in _context.Esals.Where(a => a.BillId == null).Where(a => a.MenuLE0.M0_Name == "سياحة دينية").Where(A => A.CompanyID == _userManager.Users.SingleOrDefault(a => a.Id == _userManager.GetUserId(User)).CompanyId).Where(a => a.MenuLE1Id == Convert.ToInt32(TempData["Name1"])) select new Models.ViewModel.Transactions { EId = x.EsalId, Type = "إيصال", EDate = x.EsalDate, Statement = string.Concat(x.MenuLE0.M0_Name, " - ", x.DepositDesc), Debit = x.AmountPaid, Name1 = x.CustomerOrSupplierId, Name2 = x.CustomerSupplierId }).Where(d => d.EDate >= Convert.ToDateTime(TempData["StartDate"])).Where(d => d.EDate.Date <= Convert.ToDateTime(TempData["EndDate"])).Where(d => d.Name2 == Convert.ToInt32(TempData["Name2"]))).ToListAsync();
            var ezn = await ((from x in _context.Ezns.Where(a => a.MenuLE0.M0_Name == "سياحة دينية").Where(A => A.CompanyID == _userManager.Users.SingleOrDefault(a => a.Id == _userManager.GetUserId(User)).CompanyId).Where(a => a.MenuLE1Id == Convert.ToInt32(TempData["Name1"])) select new Models.ViewModel.Transactions { EId = x.EznId, Type = "إذن صرف", EDate = x.EznDate, Statement = string.Concat(x.MenuLE0.M0_Name, " - ", x.ExpenseName), Credit = x.AmountWithdrawn, Name1 = x.CustomerOrSupplierId, Name2 = x.CustomerSupplierId }).Where(d => d.EDate >= Convert.ToDateTime(TempData["StartDate"])).Where(d => d.EDate.Date <= Convert.ToDateTime(TempData["EndDate"])).Where(d => d.Name2 == Convert.ToInt32(TempData["Name2"]))).ToListAsync();
            var Outbut =((billReligiousEsal.Concat(esal).Concat(esalBill).Concat(ezn)).OrderBy(a => a.EDate));

            var UserId = _userManager.GetUserId(User);
            var CompanyID = _userManager.Users.SingleOrDefault(a => a.Id == _userManager.GetUserId(User)).CompanyId;
            if (Outbut == null)
            {
                return NotFound();
            }
            ViewData["CompanyLogo"] = "~/" + TravelERP.Properties.Resources.ImgFolder + "/" + _context.Companies.SingleOrDefault(a => a.Id == CompanyID).CompanyLogo;
            ViewData["CompanyA4EsalImage"] = "~/" + TravelERP.Properties.Resources.ImgFolder + "/" + _context.Companies.SingleOrDefault(a => a.Id == CompanyID).CompanyA4EsalImage;
            ViewData["CompanyNameE"] =(await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyID)).Company_NameE;
            ViewData["CompanyName"] =(await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyID)).Company_Name;
            ViewData["CompanyId"] = CompanyID;


            return View(Outbut);
        }




        //  **** التقارير / التقارير المفصله / سياحه دينيه/ تقرير معاملات مندوب الحج والعمره - بالاسم ****

        // POST: BillReligious/OmraReportByNameDate  **** التقارير / التقارير المفصله / سياحه دينيه/ تقرير معاملات مندوب الحج والعمره ****
        public async Task<IActionResult> OmraReportByNameDate([Bind("EndDate,Name5,Name6")] DatesSearch datesSearch)
        {
            if (ModelState.IsValid)
            {
                TempData["Name5"] = datesSearch.Name5;
                TempData["Name6"] = datesSearch.Name6;
                //_context.Add(datesSearch);
                await _context.SaveChangesAsync();
                return RedirectToAction("OmraReportByName", "BillReligious");
            }
            return View(datesSearch);
        }

        // POST: BillReligious/OmraReportByName  **** التقارير / التقارير المفصله / سياحه دينيه/ تقرير معاملات مندوب الحج والعمره ****
        public async Task<ActionResult> OmraReportByName()
        {
            var CompanyId =(await _userManager.Users.SingleOrDefaultAsync(a => a.Id == _userManager.GetUserId(User))).CompanyId;
            var ezn =await( (from x in _context.Ezns.Where(a => a.MenuLE0.M0_Name == "سياحة دينية").Where(a => a.CustomerOrSupplier.Name == "عميل").Where(A => A.CompanyID == CompanyId).Where(a => a.CustomerSupplierId == Convert.ToInt32(TempData["Name5"])).Where(d => d.MenuLE1Id == Convert.ToInt32(TempData["Name6"])) select new Models.ViewModel.Transactions { EId = x.EznId, Type = "إذن ", EDate = x.EznDate, Statement = string.Concat(x.MenuLE0.M0_Name, " - ", x.MenuLE1.M1_Name, " - ", x.MenuLE2.M2_Name, " - ", x.ExpenseName), Credit = x.AmountWithdrawn * -1, Name3 = x.MenuLE2.M2_Name })).ToListAsync();
            var esal = await ((from x in _context.Esals.Where(a => a.MenuLE0.M0_Name == "سياحة دينية").Where(a => a.CustomerOrSupplier.Name == "عميل").Where(A => A.CompanyID == CompanyId).Where(a => a.CustomerSupplierId == Convert.ToInt32(TempData["Name5"])).Where(d => d.MenuLE1Id == Convert.ToInt32(TempData["Name6"])) select new Models.ViewModel.Transactions { EId = x.EsalId, Type = "إيصال", EDate = x.EsalDate, Statement = string.Concat(x.MenuLE0.M0_Name, " - ", x.MenuLE1.M1_Name, " - ", x.MenuLE2.M2_Name, " - ", x.DepositDesc), Debit = x.AmountPaid, Name3 = x.MenuLE2.M2_Name })).ToListAsync();
            var Outbut = esal.Concat(ezn).OrderBy(a => a.EDate);
            TempData["NameCS"] =(await _context.CustomersSuppliers.SingleOrDefaultAsync(a => a.Id == Convert.ToInt32(TempData["Name5"]))).Name;
            TempData["NameMenuLE1"] =(await _context.MenuLE1.SingleOrDefaultAsync(a => a.Id == Convert.ToInt32(TempData["Name6"]))).M1_Name;

            var UserId = _userManager.GetUserId(User);
            if (Outbut == null)
            {
                return NotFound();
            }
            ViewData["CompanyLogo"] = "~/" + TravelERP.Properties.Resources.ImgFolder + "/" + _context.Companies.SingleOrDefault(a => a.Id == CompanyId).CompanyLogo;
            ViewData["CompanyA4EsalImage"] = "~/" + TravelERP.Properties.Resources.ImgFolder + "/" + _context.Companies.SingleOrDefault(a => a.Id == CompanyId).CompanyA4EsalImage;
            ViewData["CompanyNameE"] =(await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyId)).Company_NameE;
            ViewData["CompanyName"] =(await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyId)).Company_Name;
            ViewData["CompanyId"] = CompanyId;
            return View(Outbut);
        }




        ///////////////////////////  **** التقارير / التقارير المجمعه / سياحه دينيه/ **** ///////////////////////////////



        // GET: BillReligious/CSTStatementDate  **** التقارير / التقارير المجمعه / سياحه دينيه/ ****
        public IActionResult CXTStatementDate()
        {
            ViewData["CustomerOrSupplierId"] = new SelectList(_context.CustomerOrSuppliers, "Id", "Name");
            ViewData["CustomerSupplierId"] = new SelectList(_context.CustomersSuppliers.Where(a => a.CustomerOrSupplier.Name == "مندوب").Where(A => A.CompanyId == _userManager.Users.SingleOrDefault(a => a.Id == _userManager.GetUserId(User)).CompanyId), "Id", "Name");
            ViewData["MenuLE1Id"] = new SelectList(_context.MenuLE1.Where(a => a.MenuLE0.M0_Name == "سياحة دينية"), "Id", "M1_Name");
            return View();
        }


        // POST: BillReligious/CSTStatementDate  **** التقارير / التقارير المجمعه / سياحه دينيه/ ****
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CXTStatementDate([Bind("StartDate,Name1")] DatesSearch datesSearch)
        {
            if (datesSearch.StartDate == null)
            {
                ModelState.AddModelError("StartDate", "من فضلك اختر تاريخ للبحث");
            }
            if (datesSearch.Name1 == 0)
            {
                ModelState.AddModelError("Name1", "اختر اسم من القائمه");
            }

            if (ModelState.IsValid)
            {
                TempData["StartDate"] = datesSearch.StartDate.Value.Date.ToString();
                TempData["Name1"] = datesSearch.Name1;
                await _context.SaveChangesAsync();
                return RedirectToAction("CXTStatement", "BillReligious");
            }
            ViewData["MenuLE1Id"] = new SelectList(_context.MenuLE1.Where(a => a.MenuLE0.M0_Name == "سياحة دينية"), "Id", "M1_Name", datesSearch.Name1);

            return View(datesSearch);
        }

        // POST: BillReligious/CXTStatement  **** التقارير / التقارير المجمعه / سياحه دينيه/ ****
        public async Task<ActionResult> CXTStatement()
        {
            var ConcatBillEsal =await ((from x in _context.BillReligious.Where(a => a.CustomerOrSupplier.Name == "مندوب").Where(d => d.BillDate.Date <= Convert.ToDateTime(TempData["StartDate"]).Date).Where(A => A.CompanyID == _userManager.Users.SingleOrDefault(a => a.Id == _userManager.GetUserId(User)).CompanyId).Where(a => a.MenuLE1Id == Convert.ToInt32(TempData["Name1"])) select new { Name2 = x.CustomerSupplierId, Credit = -x.CustomerPrice })
                             .Concat(from x in _context.Esals.Where(a => a.CustomerOrSupplier.Name == "مندوب").Where(a => a.MenuLE0.M0_Name == "سياحة دينية").Where(d => d.EsalDate.Date <= Convert.ToDateTime(TempData["StartDate"]).Date).Where(A => A.CompanyID == _userManager.Users.SingleOrDefault(a => a.Id == _userManager.GetUserId(User)).CompanyId).Where(a => a.MenuLE1Id == Convert.ToInt32(TempData["Name1"])) select new { Name2 = x.CustomerSupplierId, Credit = x.AmountPaid })
                 //.Concat(from x in _context.BillReligious.Where(d => d.BillDate.Date <= Convert.ToDateTime(TempData["StartDate"]).Date).Where(A => A.CompanyID == _userManager.Users.SingleOrDefault(a => a.Id == _userManager.GetUserId(User)).CompanyId) select new { Name2 = x.TicketExportId, Credit = x.NetPrice })
                 .Concat(from x in _context.Ezns.Where(a => a.CustomerOrSupplier.Name == "مندوب").Where(a => a.MenuLE0.M0_Name == "سياحة دينية").Where(d => d.EznDate.Date <= Convert.ToDateTime(TempData["StartDate"]).Date).Where(A => A.CompanyID == _userManager.Users.SingleOrDefault(a => a.Id == _userManager.GetUserId(User)).CompanyId).Where(a => a.MenuLE1Id == Convert.ToInt32(TempData["Name1"])) select new { Name2 = (int)x.CustomerSupplierId, Credit = -x.AmountWithdrawn })
                 .Concat(from x in _context.OpeningBalances.Where(a => a.MenuLE0.M0_Name == "سياحة دينية").Where(A => A.CompanyID == _userManager.Users.SingleOrDefault(a => a.Id == _userManager.GetUserId(User)).CompanyId) select new { Name2 = x.CustomerSupplierId, Credit = x.Balance })
                 .OrderByDescending(p => p.Name2)
                 .GroupBy(x => x.Name2)
                 .Select(a =>
                     new
                     {
                         Name2 = a.Key,
                         Credit = a.Sum(x => x.Credit)
                     })).ToListAsync();

            var Outbut = (from x in ConcatBillEsal select new Models.ViewModel.Transactions { Statement = _context.CustomersSuppliers.SingleOrDefault(a => a.Id == x.Name2).Name, Credit = x.Credit }).OrderBy(a => a.Credit);

            var UserId = _userManager.GetUserId(User);
            var CompanyID =(await _userManager.Users.SingleOrDefaultAsync(a => a.Id == _userManager.GetUserId(User))).CompanyId;
            if (Outbut == null)
            {
                return NotFound();
            }
            ViewData["CompanyLogo"] = "~/" + TravelERP.Properties.Resources.ImgFolder + "/" + _context.Companies.SingleOrDefault(a => a.Id == CompanyID).CompanyLogo;
            ViewData["CompanyA4EsalImage"] = "~/" + TravelERP.Properties.Resources.ImgFolder + "/" + _context.Companies.SingleOrDefault(a => a.Id == CompanyID).CompanyA4EsalImage;
            ViewData["CompanyNameE"] = (await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyID)).Company_NameE;
            ViewData["CompanyName"] = (await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyID)).Company_Name;
            ViewData["CompanyId"] = CompanyID;

            return View(Outbut);
        }



        /////////////////////////////////////////////////////////////////////////////////////////////////
        /// BillReligious/BillsAllDate **** المبيعات - مبيعات لجميع الموظفين - سياحه دينيه **** ///
        public IActionResult BillsAllDate()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BillsAllDate([Bind("StartDate,EndDate")] DatesSearch datesSearch)
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
                await _context.SaveChangesAsync();
                return RedirectToAction("BillsAll", "BillReligious");
            }
            return View(datesSearch);
        }
        public async Task<ActionResult> BillsAll()
        {
            var UserId = _userManager.GetUserId(User);
            var CompanyId =(await _userManager.Users.SingleOrDefaultAsync(a => a.Id == _userManager.GetUserId(User))).CompanyId;
            var billReligious =await _context.BillReligious
                .Include(b => b.CustomerOrSupplier)
                .Include(b => b.CustomerSupplier)
                .Include(b => b.MenuLE0)
                .Include(b => b.MenuLE1)
                .Include(b => b.MenuLE2)
                .Include(b => b.User)
                .Include(b => b.Company)
                .Where(d => d.BillDate.Date >= Convert.ToDateTime(TempData["StartDate"]))
                .Where(d => d.BillDate.Date <= Convert.ToDateTime(TempData["EndDate"]))
                .Where(A => A.CompanyID == CompanyId).ToListAsync()
                ;
            if (billReligious == null)
            {
                return NotFound();
            }
            ViewData["CompanyLogo"] = "~/" + TravelERP.Properties.Resources.ImgFolder + "/" + _context.Companies.SingleOrDefault(a => a.Id == CompanyId).CompanyLogo;
            ViewData["CompanyA4EsalImage"] = "~/" + TravelERP.Properties.Resources.ImgFolder + "/" + _context.Companies.SingleOrDefault(a => a.Id == CompanyId).CompanyA4EsalImage;
            ViewData["CompanyNameE"] =(await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyId)).Company_NameE;
            ViewData["CompanyName"] =(await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyId)).Company_Name;
            ViewData["CompanyId"] = CompanyId;

            return View(billReligious);
        }



        ////////////////////////////////////////////////////////////////////////////////////////////
        /// BillReligious/UserBillsDate **** المبيعات - مبيعات الموظف - سياحه دينيه **** //////
        public IActionResult UserBillsDate()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserBillsDate([Bind("StartDate,EndDate")] DatesSearch datesSearch)
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
                return RedirectToAction("UserBills", "BillReligious");
            }
            return View(datesSearch);
        }
        public async Task<ActionResult> UserBills()
        {
            var UserId = _userManager.GetUserId(User);
            var CompanyId =(await _userManager.Users.SingleOrDefaultAsync(a => a.Id == _userManager.GetUserId(User))).CompanyId;
            var billReligious =await _context.BillReligious
                .Include(b => b.CustomerOrSupplier)
                .Include(b => b.CustomerSupplier)
                .Include(b => b.MenuLE0)
                .Include(b => b.MenuLE1)
                .Include(b => b.MenuLE2)
                .Include(b => b.User)
                .Include(b => b.Company)
                .Where(d => d.BillDate.Date >= Convert.ToDateTime(TempData["StartDate"]))
                .Where(d => d.BillDate.Date <= Convert.ToDateTime(TempData["EndDate"]))
                .Where(d => d.UserId == UserId)
                .Where(A => A.CompanyID == CompanyId).ToListAsync()
                ;
            if (billReligious == null)
            {
                return NotFound();
            }
            ViewData["CompanyLogo"] = "~/" + TravelERP.Properties.Resources.ImgFolder + "/" + _context.Companies.SingleOrDefault(a => a.Id == CompanyId).CompanyLogo;
            ViewData["CompanyA4EsalImage"] = "~/" + TravelERP.Properties.Resources.ImgFolder + "/" + _context.Companies.SingleOrDefault(a => a.Id == CompanyId).CompanyA4EsalImage;
            ViewData["CompanyNameE"] =(await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyId)).Company_NameE;
            ViewData["CompanyName"] =(await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyId)).Company_Name;
            ViewData["CompanyId"] = CompanyId;

            return View(billReligious);
        }


        [Authorize]
        public JsonResult GetMenuLE2(int MenuLE1Id)

        {
            List<MenuLE2> menuLE2 = new List<MenuLE2>();
            menuLE2 = _context.MenuLE2.Where(a => a.MenuLE1.Id == MenuLE1Id).ToList();
            return Json(new SelectList(menuLE2, "Id", "M2_Name"));
        }



        private bool BillReligiousExists(int id)
        {
            return _context.BillReligious.Any(e => e.Id == id);
        }



        [Authorize]
        public async Task<JsonResult> CustomersReligious (int id)

        {
            var CompanyId =(await _userManager.Users.SingleOrDefaultAsync(a => a.Id == _userManager.GetUserId(User))).CompanyId;
            var CustomersReligious =await ((from x in _context.Esals.Where(a => a.CompanyID == CompanyId).Where(a => a.CustomerOrSupplier.Name == "عميل").Where(a => a.MenuLE0.M0_Name == "سياحة دينية").GroupBy(a => a.CustomerSupplierId) select new CustomerSupplier { Id = x.Key, Name = _context.CustomersSuppliers.SingleOrDefault(a => a.Id == x.Key).Name })).ToListAsync();
            ViewData["CustomerId"] = new SelectList(CustomersReligious.OrderBy(a => a.Name), "Id", "Name");

            List<CustomerSupplier> customersReligious = new List<CustomerSupplier>();
            customersReligious = CustomersReligious.ToList();
            customersReligious.Insert(0, new CustomerSupplier { Id = 0, Name = "من فضلك اختر من القائمه ..." });
            return Json(new SelectList(customersReligious, "Id", "Name"));
        }


    }
}

