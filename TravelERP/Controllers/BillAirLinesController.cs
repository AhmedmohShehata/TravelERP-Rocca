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
    [Authorize(Roles = CustomRoles.Admin + "," + CustomRoles.BranchManager + "," + CustomRoles.BillAirLines)]
    public class BillAirLinesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public BillAirLinesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize(Roles = CustomRoles.Admin + "," + CustomRoles.BranchManager + "," + CustomRoles.BillAirLines)]
        // GET: BillAirLines/ Index **** الصفحه الرئيسيه لفواتير الطيران ****
        public IActionResult Index()
        {
            var CompanyId = _userManager.Users.SingleOrDefault(a => a.Id == _userManager.GetUserId(User)).CompanyId;
            var CompanyCuntry = _context.Companies.SingleOrDefault(a => a.Id == CompanyId).DateTimeCountry;
            var info = TimeZoneInfo.FindSystemTimeZoneById(CompanyCuntry);
            DateTimeOffset localServerTime = DateTimeOffset.Now;
            DateTimeOffset localTime = TimeZoneInfo.ConvertTime(localServerTime, info);
            DateTime datetimenow = localTime.DateTime;
            ViewData["DateTimeNow"] = datetimenow.Date.ToShortDateString();
            return View();
        }


        [Authorize(Roles = CustomRoles.Admin + "," + CustomRoles.BranchManager + "," + CustomRoles.BillAirLines)]
        // GET: BillAirLines/Details/5  **** تفاصيل فاتوره الطيران ****
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var billAirLine = await _context.BillAirLines
                .Include(b => b.CustomerOrSupplier)
                .Include(b => b.CustomerSupplier)
                .Include(b => b.MenuLE0)
                .Include(b => b.MenuLE1)
                .Include(b => b.MenuLE2)
                .Include(b => b.TicketExport)
                .Include(b => b.User)
                .Include(b => b.Company)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (billAirLine == null)
            {
                return NotFound();
            }

            return View(billAirLine);
        }



        [Authorize(Roles = CustomRoles.Admin + "," + CustomRoles.BranchManager + "," + CustomRoles.BillAirLines)]
        // GET: BillAirLines/Create  **** إنشاء فاتوره طيران جديده *****
        public async Task<IActionResult> Create()
        {
            var CompanyId =(await _context.Users.SingleOrDefaultAsync(b => b.Id == _userManager.GetUserId(User))).CompanyId;
            var customersSuppliers =await (from user in _context.CustomersSuppliers.Where(a => a.CompanyId == CompanyId)
                        //where !_context.NonSuppliers.Any(f => f.CustomerSupplierId == user.Id)
                        select new
                        {
                            user.Id,
                            user.Name,
                            user.CustomerOrSupplier,
                            user.CustomerOrSupplierId
                        }).ToListAsync();

            ViewData["CustomerOrSupplierId"] = new SelectList(_context.CustomerOrSuppliers, "Id", "Name");
            ViewData["MenuLE0Id"] = new SelectList(_context.MenuLE0, "Id", "M0_Name");
            ViewData["MenuLE1Id"] = new SelectList(_context.MenuLE1.Where(a => a.MenuLE0.M0_Name == "طيران"), "Id", "M1_Name");
            ViewData["MenuLE2Id"] = new SelectList(_context.MenuLE2, "Id", "M2_Name");
            ViewData["TicketExportId"] = new SelectList(_context.CustomersSuppliers.Where(a => a.CompanyId == CompanyId).Where(a => a.CustomerOrSupplier.Name == "مورد او وكيل").OrderBy(a => a.Name), "Id", "Name");
            return View();
        }

        [Authorize(Roles = CustomRoles.Admin + "," + CustomRoles.BranchManager + "," + CustomRoles.BillAirLines)]
        // POST: BillAirLines/Create  **** إنشاء فاتوره طيران جديده *****
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CustomerOrSupplierId,CustomerSupplierId,MenuLE0Id,MenuLE1Id,MenuLE2Id,Direction,PNR,TicketExportId,AdultN,ChildN,CustomerPrice,NetPrice,TicketFrom,TicketTo,Commnets,eTicketNumber")] BillAirLine billAirLine)
        {
            var UserId = _userManager.GetUserId(User);
            var CompanyId =(await _userManager.Users.SingleOrDefaultAsync(a => a.Id == UserId)).CompanyId;
            if (ModelState.IsValid)
            {
                billAirLine.UserId = UserId;
                var CompanyCuntry =(await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyId)).DateTimeCountry;
                var info = TimeZoneInfo.FindSystemTimeZoneById(CompanyCuntry);
                DateTimeOffset localServerTime = DateTimeOffset.Now;
                DateTimeOffset localTime = TimeZoneInfo.ConvertTime(localServerTime, info);
                billAirLine.BillDate = localTime.DateTime;

                billAirLine.MenuLE0Id =(await _context.MenuLE0.SingleOrDefaultAsync(a => a.M0_Name== "طيران")).Id;
                billAirLine.EMPCommission = (billAirLine.CustomerPrice - billAirLine.NetPrice) * ((await _context.UsersDetails.Where(a =>a.UserId == billAirLine.UserId).CountAsync()) != 0 ?(await _context.UsersDetails.SingleOrDefaultAsync(a => a.UserId == billAirLine.UserId)).Commission : 0);
                billAirLine.CompanyID =(byte) _userManager.Users.SingleOrDefault(a=>a.Id== billAirLine.UserId).CompanyId;
                if (await _context.BillAirLines.Where(a => a.CompanyID == billAirLine.CompanyID).CountAsync() == 0)
                {
                    billAirLine.BillId = 1;
                    TempData["BillId"] = 1;
                }
                else
                {
                    billAirLine.BillId =await _context.BillAirLines.Where(a => a.CompanyID == billAirLine.CompanyID).MaxAsync(a => a.BillId) + 1;
                    TempData["BillId"] = billAirLine.BillId;
                }
                _context.Add(billAirLine);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", new RouteValueDictionary(
                        new { controller = "BillAirLines", action = "Details", Id = billAirLine.Id }));
            }
            var customersSuppliers =await (from user in _context.CustomersSuppliers.Where(a => a.CompanyId == CompanyId)
                                      where !_context.NonSuppliers.Any(f => f.CustomerSupplierId == user.Id)
                                      select new
                                      {
                                          user.Id,
                                          user.Name,
                                          user.CustomerOrSupplier,
                                          user.CustomerOrSupplierId
                                      }).ToListAsync();

            ViewData["CustomerOrSupplierId"] = new SelectList(customersSuppliers, "Id", "Name", billAirLine.CustomerOrSupplierId);
            ViewData["CustomerSupplierId"] = new SelectList(customersSuppliers.Where(a => a.CustomerOrSupplierId == billAirLine.CustomerOrSupplierId).OrderBy(x => x.Name), "Id", "Name", billAirLine.CustomerSupplierId);
            ViewData["MenuLE0Id"] = new SelectList(_context.MenuLE0, "Id", "M0_Name", billAirLine.MenuLE0Id);
            ViewData["MenuLE1Id"] = new SelectList(_context.MenuLE1, "Id", "M1_Name", billAirLine.MenuLE1Id);
            ViewData["MenuLE2Id"] = new SelectList(_context.MenuLE2, "Id", "M2_Name", billAirLine.MenuLE2Id);
            ViewData["TicketExportId"] = new SelectList(customersSuppliers.Where(a => a.CustomerOrSupplier.Name == "مورد او وكيل").OrderBy(a => a.Name), "Id", "Name", billAirLine.TicketExportId);
            return View(billAirLine);
        }


        [Authorize(Roles = CustomRoles.Admin + "," + CustomRoles.BranchManager + "," + CustomRoles.Edits)]
        // GET: BillAirLines/Edit/5  **** تعديل فاتوره طيران ****
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var billAirLine = await _context.BillAirLines.SingleOrDefaultAsync(m => m.Id == id);
            if (billAirLine == null)
            {
                return NotFound();
            }
            var CompanyId = _userManager.Users.SingleOrDefault(a => a.Id == billAirLine.UserId).CompanyId;
            var customersSuppliers = (from user in _context.CustomersSuppliers.Where(a => a.CompanyId == CompanyId)
                                      //where !_context.NonSuppliers.Any(f => f.CustomerSupplierId == user.Id)
                                      select new
                                      {
                                          user.Id,
                                          user.Name,
                                          user.CustomerOrSupplier,
                                          user.CustomerOrSupplierId
                                      }).ToList();

            ViewData["CustomerOrSupplierId"] = new SelectList(_context.CustomerOrSuppliers.Where(a => a.Name != "مندوب"), "Id", "Name", billAirLine.CustomerOrSupplierId);
            ViewData["CustomerSupplierId"] = new SelectList(customersSuppliers.Where(a => a.CustomerOrSupplierId == billAirLine.CustomerOrSupplierId).OrderBy(a => a.Name), "Id", "Name", billAirLine.CustomerSupplierId);
            ViewData["MenuLE0Id"] = new SelectList(_context.MenuLE0, "Id", "M0_Name", billAirLine.MenuLE0Id);
            ViewData["MenuLE1Id"] = new SelectList(_context.MenuLE1.Where(a => a.MenuLE0Id == billAirLine.MenuLE0Id), "Id", "M1_Name", billAirLine.MenuLE1Id);
            ViewData["MenuLE2Id"] = new SelectList(_context.MenuLE2.Where(a => a.MenuLE1Id == billAirLine.MenuLE1Id), "Id", "M2_Name", billAirLine.MenuLE2Id);
            ViewData["TicketExportId"] = new SelectList(customersSuppliers.Where(a => a.CustomerOrSupplier.Name == "مورد او وكيل").OrderBy(a=>a.Name), "Id", "Name", billAirLine.TicketExportId);
            ViewData["UserId"] = new SelectList(_context.Users.Where(a => a.CompanyId == CompanyId), "Id", "UserName", billAirLine.UserId);
            return View(billAirLine);
        }

        [Authorize(Roles = CustomRoles.Admin + "," + CustomRoles.BranchManager + "," + CustomRoles.Edits)]
        // POST: BillAirLines/Edit/5   **** تعديل فاتوره طيران ****
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BillId,BillDate,CustomerOrSupplierId,CustomerSupplierId,MenuLE0Id,MenuLE1Id,MenuLE2Id,Direction,PNR,TicketExportId,AdultN,ChildN,CustomerPrice,NetPrice,TicketFrom,TicketTo,Commnets,UserId,CompanyID,eTicketNumber")] BillAirLine billAirLine)
        {
            if (id != billAirLine.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    billAirLine.EMPCommission = (billAirLine.CustomerPrice - billAirLine.NetPrice) * ((_context.UsersDetails.Where(a => a.UserId == billAirLine.UserId).Count()) != 0 ? _context.UsersDetails.SingleOrDefault(a => a.UserId == billAirLine.UserId).Commission : 0);
                    _context.Update(billAirLine);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BillAirLineExists(billAirLine.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", new RouteValueDictionary(
                        new { controller = "BillAirLines", action = "Details", Id = billAirLine.Id }));
            }
            var CompanyId = _userManager.Users.SingleOrDefault(a => a.Id == billAirLine.UserId).CompanyId;
            var customersSuppliers = (from user in _context.CustomersSuppliers.Where(a => a.CompanyId == CompanyId)
                                      //where !_context.NonSuppliers.Any(f => f.CustomerSupplierId == user.Id)
                                      select new
                                      {
                                          user.Id,
                                          user.Name,
                                          user.CustomerOrSupplier,
                                          user.CustomerOrSupplierId
                                      }).ToList();

            ViewData["CustomerOrSupplierId"] = new SelectList(_context.CustomerOrSuppliers, "Id", "Name", billAirLine.CustomerOrSupplierId);
            ViewData["MenuLE0Id"] = new SelectList(_context.MenuLE0, "Id", "M0_Name", billAirLine.MenuLE0Id);
            ViewData["MenuLE1Id"] = new SelectList(_context.MenuLE1, "Id", "M1_Name", billAirLine.MenuLE1Id);
            //ViewData["MenuLE2Id"] = new SelectList(_context.MenuLE2, "Id", "M2_Name", billAirLine.MenuLE2Id);
            ViewData["TicketExportId"] = new SelectList(customersSuppliers.Where(a => a.CustomerOrSupplier.Name == "مورد او وكيل").OrderBy(a => a.Name), "Id", "Name", billAirLine.TicketExportId);
            ViewData["UserId"] = new SelectList(_context.Users.Where(a => a.CompanyId == CompanyId), "Id", "UserName", billAirLine.UserId);

            return View(billAirLine);
        }


        [Authorize(Roles = CustomRoles.Admin + "," + CustomRoles.BranchManager + "," + CustomRoles.EsalAirLines)]
        // GET: BillAirLines/CreateEsal/5   **** إنشاء ايصال لفاتوره طيران  ****
        public async Task<IActionResult> CreateEsal(int? Id)
        {
            var bill =await _context.BillAirLines.SingleOrDefaultAsync(a => a.Id == Id);

            ViewData["IDBill"] = bill.Id;
            ViewData["BillID"] = bill.BillId;
            ViewData["MenuLE0Id"] = bill.MenuLE0Id;
            ViewData["MenuLE1Id"] = bill.MenuLE1Id;
            ViewData["MenuLE2Id"] = bill.MenuLE2Id;
            ViewData["CustomerSupplierId"] = bill.CustomerSupplierId;
            ViewData["CustomerOrSupplierId"] = bill.CustomerOrSupplierId;
            ViewData["TicketExportId"] = bill.TicketExportId;
            ViewData["CustomerName"] = " " + _context.CustomersSuppliers.SingleOrDefault(a => a.Id == bill.CustomerSupplierId).Name + " - " + _context.CustomerOrSuppliers.SingleOrDefault(a => a.Id == bill.CustomerOrSupplierId).Name;
            ViewData["Byan"] = "ذلك مقابل حجز " + _context.MenuLE0.SingleOrDefault(a => a.Id == bill.MenuLE0Id).M0_Name + " - " + _context.MenuLE2.SingleOrDefault(a => a.Id == bill.MenuLE2Id).M2_Name + " - عدد البالغين " + bill.AdultN + " - عدد الاطفال " + bill.ChildN + " - وذلك مقابل " + bill.CustomerPrice + (bill.CompanyID == 6? "درهم" : " ج ");
            ViewData["BillDate"] = bill.BillDate;
            ViewData["PaymentMethodId"] = new SelectList(_context.paymentMethods, "Id", "Name");
            return View();
        }



        // POST: BillAirLines/CreateEsal/5   **** إنشاء ايصال لفاتوره طيران  ****
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = CustomRoles.Admin + "," + CustomRoles.BranchManager + "," + CustomRoles.EsalAirLines)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEsal([Bind("BillId,BillIdId,MenuLE0Id,MenuLE1Id,MenuLE2Id,CustomerOrSupplierId,CustomerSupplierId,TicketExportId,AmountPaid,PaymentMethodId,DepositDesc")] Esal esal)
        {
            var UserId = _userManager.GetUserId(User);
            var CompanyId =(await _userManager.Users.SingleOrDefaultAsync(a => a.Id == UserId)).CompanyId;
            var CompanyCuntry =(await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyId)).DateTimeCountry;
            var info = TimeZoneInfo.FindSystemTimeZoneById(CompanyCuntry);
            DateTimeOffset localServerTime = DateTimeOffset.Now;
            DateTimeOffset localTime = TimeZoneInfo.ConvertTime(localServerTime, info);

            if (ModelState.IsValid)
            {
                if (_context.Esals.Where(a => a.CompanyID == _context.Users.SingleOrDefault(b => b.Id == _userManager.GetUserId(User)).CompanyId).Count() == 0)
                {
                    esal.EsalId = 1;
                }
                else
                {
                    esal.EsalId = _context.Esals.Where(a => a.CompanyID == _context.Users.SingleOrDefault(b => b.Id == _userManager.GetUserId(User)).CompanyId).Max(a => a.EsalId) + 1;
                }
                esal.CompanyID = CompanyId;
                esal.UserId = UserId;
                esal.EsalDate = localTime.DateTime;

                _context.Add(esal);
                await _context.SaveChangesAsync();
                return RedirectToAction("Print", new RouteValueDictionary(
                new { controller = "Esals", action = "Print", Id = esal.Id }));
            }
            var bill =(await _context.BillAirLines.SingleOrDefaultAsync(a => a.Id == esal.BillId));

            ViewData["IDBill"] = bill.Id;
            ViewData["BillID"] = bill.BillId;
            ViewData["MenuLE0Id"] = bill.MenuLE0Id;
            ViewData["MenuLE1Id"] = bill.MenuLE1Id;
            ViewData["MenuLE2Id"] = bill.MenuLE2Id;
            ViewData["CustomerSupplierId"] = bill.CustomerSupplierId;
            ViewData["CustomerOrSupplierId"] = bill.CustomerOrSupplierId;
            ViewData["TicketExportId"] = bill.TicketExportId;
            ViewData["CustomerName"] = " " + _context.CustomersSuppliers.SingleOrDefault(a => a.Id == bill.CustomerSupplierId).Name + " - " + _context.CustomerOrSuppliers.SingleOrDefault(a => a.Id == bill.CustomerOrSupplierId).Name;
            ViewData["Byan"] = "ذلك مقابل حجز " + _context.MenuLE0.SingleOrDefault(a => a.Id == bill.MenuLE0Id).M0_Name + " - " + _context.MenuLE2.SingleOrDefault(a => a.Id == bill.MenuLE2Id).M2_Name + " - عدد البالغين " + bill.AdultN + " - عدد الاطفال " + bill.ChildN + " - وذلك مقابل " + bill.CustomerPrice + (bill.CompanyID == 6 ? "درهم" : " ج ");
            ViewData["BillDate"] = bill.BillDate;
            ViewData["PaymentMethodId"] = new SelectList(_context.paymentMethods, "Id", "Name");
            return View(esal);

        }

        //////////////////////////// Ezns/CreateEzn **** الإذون - إنشاء اذن لمصروف **** ////////////////////////////////
        public async Task<IActionResult> CreateEzn(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }
            var bill = await _context.BillAirLines.SingleOrDefaultAsync(a => a.Id == Id);

            ViewData["IDBill"] = bill.Id;
            ViewData["BillID"] = bill.BillId;
            ViewData["MenuLE0Id"] = bill.MenuLE0Id;
            ViewData["MenuLE1Id"] = bill.MenuLE1Id;
            ViewData["MenuLE2Id"] = bill.MenuLE2Id;
            ViewData["CustomerOrSupplierId"] = new SelectList(_context.CustomerOrSuppliers.Where(a => a.Name != "مندوب"), "Id", "Name");
            ViewData["MenuLE"] = (await _context.MenuLE0.SingleOrDefaultAsync(x => x.Id == bill.MenuLE0Id)).M0_Name + " - " + (await _context.MenuLE1.SingleOrDefaultAsync(x => x.Id == bill.MenuLE1Id)).M1_Name + " - " + (await _context.MenuLE2.SingleOrDefaultAsync(x => x.Id == bill.MenuLE2Id)).M2_Name;
            ViewData["PaymentMethodId"] = new SelectList(_context.paymentMethods, "Id", "Name");

            return View();
        }

        // POST: Ezns/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEzn([Bind("BillId,BillIdId,MenuLE0Id,MenuLE1Id,MenuLE2Id,CustomerOrSupplierId,CustomerSupplierId,ExpenseName,AmountWithdrawn,PaymentMethodId")] Ezn ezn)
        {
            if (ezn.CustomerOrSupplierId == 0 || ezn.CustomerOrSupplierId == null)
            {
                ModelState.AddModelError("CustomerOrSupplierId", "من فضلك اختر المورد او العميل");
            }
            if (ezn.CustomerSupplierId == 0 || ezn.CustomerSupplierId == null)
            {
                ModelState.AddModelError("CustomerSupplierId", "من فضلك اختر اسم المورد او العميل");
            }
            if (ModelState.IsValid)
            {
                var CompanyId = (await _userManager.Users.SingleOrDefaultAsync(a => a.Id == _userManager.GetUserId(User))).CompanyId;
                var CompanyCuntry = (await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyId)).DateTimeCountry;
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
                    ezn.EznId = await _context.Ezns.Where(a => a.CompanyID == ezn.CompanyID).MaxAsync(a => a.EznId) + 1;

                }
                _context.Add(ezn);
                await _context.SaveChangesAsync();
                return RedirectToAction("PrintEzn", new RouteValueDictionary(
                new { controller = "Ezns", action = "PrintEzn", Id = ezn.Id }));
            }
            var bill = await _context.BillAirLines.SingleOrDefaultAsync(a => a.Id == ezn.BillId);

            ViewData["IDBill"] = bill.Id;
            ViewData["BillID"] = bill.BillId;
            ViewData["MenuLE0Id"] = bill.MenuLE0Id;
            ViewData["MenuLE1Id"] = bill.MenuLE1Id;
            ViewData["MenuLE2Id"] = bill.MenuLE2Id;
            ViewData["CustomerOrSupplierId"] = new SelectList(_context.CustomerOrSuppliers.Where(a => a.Name != "مندوب"), "Id", "Name");
            ViewData["MenuLE"] = (await _context.MenuLE0.SingleOrDefaultAsync(x => x.Id == bill.MenuLE0Id)).M0_Name + " - " + (await _context.MenuLE1.SingleOrDefaultAsync(x => x.Id == bill.MenuLE1Id)).M1_Name + " - " + (await _context.MenuLE2.SingleOrDefaultAsync(x => x.Id == bill.MenuLE2Id)).M2_Name;
            ViewData["PaymentMethodId"] = new SelectList(_context.paymentMethods, "Id", "Name", ezn.PaymentMethodId);
            return View(ezn);
        }

        //////////////////////////////////////////////////////////////////////////////////////
        /// AllBills/AirLinesStatementDate **** التقارير - تقرير موردين لكل الشركات **** ///
        public async Task<IActionResult> AirLinesStatementDate()
        {
            var CompanyId = (await _context.Users.SingleOrDefaultAsync(b => b.Id == _userManager.GetUserId(User))).CompanyId;

            ViewData["TicketExportId"] = new SelectList(_context.CustomersSuppliers.Where(a => a.CompanyId == CompanyId).Where(a => a.CustomerOrSupplier.Name == "مورد او وكيل").OrderBy(a => a.Name), "Id", "Name");
            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AirLinesStatementDate([Bind("StartDate,EndDate,Name1")] DatesSearch datesSearch)
        {
            var CompanyId = (await _context.Users.SingleOrDefaultAsync(b => b.Id == _userManager.GetUserId(User))).CompanyId;

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
                TempData["Name1"] = datesSearch.Name1;



                //_context.Add(datesSearch);
                await _context.SaveChangesAsync();
                return RedirectToAction("AirLinesStatement", "BillAirLines");
            }
            ViewData["TicketExportId"] = new SelectList(_context.CustomersSuppliers.Where(a => a.CompanyId == CompanyId).Where(a => a.CustomerOrSupplier.Name == "مورد او وكيل").OrderBy(a => a.Name), "Id", "Name",datesSearch.Name1);

            return View(datesSearch);
        }
        public async Task<ActionResult> AirLinesStatement()
        {
            if (Convert.ToInt32(TempData["Name1"]) != 0 )
            {
                var SuppliersName = (await _context.CustomersSuppliers.SingleOrDefaultAsync(a => a.Id == Convert.ToInt32(TempData["Name1"]))).Name;
                TempData["Name3"] = SuppliersName;

                //StartTransactions
                //var startBillAirLineEzn = await (from x in _context.BillAirLines select new Models.ViewModel.Transactions { EDate = x.BillDate, Debit = x.NetPrice, Name3 = x.TicketExport.Name }).Where(d => d.EDate < Convert.ToDateTime(TempData["StartDate"])).Where(d => d.Name3 == SuppliersName).ToListAsync();
                //var startBillAirLineEsal = await (from x in _context.BillAirLines select new Models.ViewModel.Transactions { EDate = x.BillDate, Credit = x.CustomerPrice, Name3 = x.CustomerSupplier.Name }).Where(d => d.EDate < Convert.ToDateTime(TempData["StartDate"])).Where(d => d.Name3 == SuppliersName).ToListAsync();

                //var startEsal = await (from x in _context.Esals.Where(a => a.MenuLE0.M0_Name == "طيران" || a.MenuLE0.M0_Name == "تأشيرات") select new Models.ViewModel.Transactions { EDate = x.EsalDate, Debit = x.AmountPaid, Name3 = x.CustomerSupplier.Name }).Where(d => d.EDate < Convert.ToDateTime(TempData["StartDate"])).Where(d => d.Name3 == SuppliersName).ToListAsync();
                //var startEzn = await (from x in _context.Ezns.Where(a => a.MenuLE0.M0_Name == "طيران" || a.MenuLE0.M0_Name == "تأشيرات") select new Models.ViewModel.Transactions { EDate = x.EznDate, Credit = x.AmountWithdrawn, Name3 = x.CustomerSupplier.Name }).Where(d => d.EDate < Convert.ToDateTime(TempData["StartDate"])).Where(d => d.Name3 == SuppliersName).ToListAsync();
                //ViewData["startTransactions"] = (startEsal.Sum(a => a.Debit) + (startBillAirLineEzn.Sum(a => a.Debit))) - (startEzn.Sum(a => a.Credit) + (startBillAirLineEsal.Sum(a => a.Credit)));
                //ViewData["OpeningBalance"] = (from x in _context.OpeningBalances select new { x.Balance, Name3 = x.CustomerSupplier.Name }).Where(d => d.Name3 == SuppliersName).Sum(a => a.Balance);
                //For Head Name
                //var billVisasEzn = await (from x in _context.BillVisas select new Models.ViewModel.Transactions { EId = x.BillId, Type = "ف تأشيرات", EDate = x.BillDate, ADate = x.ApprovedDate, Statement = string.Concat(x.MenuLE0.M0_Name, " - ", x.MenuLE1.M1_Name, " - ", x.MenuLE2.M2_Name, " - ", x.CustomerOrSupplier.Name, " - ", x.CustomerSupplier.Name, " - ", x.Commnets, " - ", x.Company.Company_Name, " - ", x.PassportNo), Debit = x.NetPrice, Name3 = x.TicketExport.Name }).Where(d => d.EDate >= Convert.ToDateTime(TempData["StartDate"])).Where(d => d.EDate.Date <= Convert.ToDateTime(TempData["EndDate"])).Where(d => d.Name3 == SuppliersName).ToListAsync();
                //var billVisasEsal = await (from x in _context.BillVisas select new Models.ViewModel.Transactions { EId = x.BillId, Type = "ف تأشيرات", EDate = x.BillDate, ADate = x.ApprovedDate, Statement = string.Concat(x.MenuLE0.M0_Name, " - ", x.MenuLE1.M1_Name, " - ", x.MenuLE2.M2_Name, " - ", x.CustomerOrSupplier.Name, " - ", x.CustomerSupplier.Name, " - ", x.Commnets, " - ", x.Company.Company_Name, " - ", x.PassportNo), Credit = x.CustomerPrice, Name3 = x.CustomerSupplier.Name }).Where(d => d.EDate >= Convert.ToDateTime(TempData["StartDate"])).Where(d => d.EDate.Date <= Convert.ToDateTime(TempData["EndDate"])).Where(d => d.Name3 == SuppliersName).ToListAsync();

                var billAirLine = await (from x in _context.BillAirLines select new Models.ViewModel.Transactions { EId = x.BillId, Type = "ف طيران", EDate = x.BillDate,Direction = x.Direction, eTicketNumber = x.eTicketNumber,PNR = x.PNR, TicketExportName = x.TicketExport.Name ,CustomerName = x.Commnets,CustomerOrSupplierName = x.CustomerOrSupplier.Name + " - " + x.CustomerSupplier.Name, Statement = string.Concat(x.MenuLE0.M0_Name, " - ", x.MenuLE1.M1_Name, " - ", x.MenuLE2.M2_Name), Debit = x.NetPrice,Credit = x.CustomerPrice }).Where(d => d.EDate >= Convert.ToDateTime(TempData["StartDate"])).Where(d => d.EDate.Date <= Convert.ToDateTime(TempData["EndDate"])).Where(d => d.TicketExportName == SuppliersName).ToListAsync();
            //var billAirLineEsal = await (from x in _context.BillAirLines select new Models.ViewModel.Transactions { EId = x.BillId, Type = "ف طيران", EDate = x.BillDate, Statement = string.Concat(x.MenuLE0.M0_Name, " - ", x.MenuLE1.M1_Name, " - ", x.MenuLE2.M2_Name, " - ", x.Direction, " - ", x.PNR, " - ", x.CustomerOrSupplier.Name, " - ", x.CustomerSupplier.Name, " - ", x.Commnets, " - ", x.Company.Company_Name), Credit = x.CustomerPrice, Name3 = x.CustomerSupplier.Name }).Where(d => d.EDate >= Convert.ToDateTime(TempData["StartDate"])).Where(d => d.EDate.Date <= Convert.ToDateTime(TempData["EndDate"])).Where(d => d.Name3 == SuppliersName).ToListAsync();
            //var esalAirLine = await (from x in _context.Esals.Where(a => a.MenuLE0.M0_Name == "طيران" || a.MenuLE0.M0_Name == "تأشيرات") select new Models.ViewModel.Transactions { EId = x.EsalId, Type = "إيصال", EDate = x.EsalDate, Statement = string.Concat(x.MenuLE0.M0_Name, " - ", x.MenuLE1.M1_Name, " - ", x.MenuLE2.M2_Name, " - ", x.DepositDesc, " - ", x.Company.Company_Name), Debit = x.AmountPaid, Name1 = x.CustomerOrSupplierId, Name3 = x.CustomerSupplier.Name }).Where(d => d.EDate >= Convert.ToDateTime(TempData["StartDate"])).Where(d => d.EDate.Date <= Convert.ToDateTime(TempData["EndDate"])).Where(d => d.Name3 == SuppliersName).ToListAsync();
            //var eznAirLine = await (from x in _context.Ezns.Where(a => a.MenuLE0.M0_Name == "طيران" || a.MenuLE0.M0_Name == "تأشيرات") select new Models.ViewModel.Transactions { EId = x.EznId, Type = "إذن صرف", EDate = x.EznDate, Statement = string.Concat(x.MenuLE0.M0_Name, " - ", x.ExpenseName, " - ", x.Company.Company_Name), Credit = x.AmountWithdrawn, Name1 = x.CustomerOrSupplierId, Name3 = x.CustomerSupplier.Name }).Where(d => d.EDate >= Convert.ToDateTime(TempData["StartDate"])).Where(d => d.EDate.Date <= Convert.ToDateTime(TempData["EndDate"])).Where(d => d.Name3 == SuppliersName).ToListAsync();

             var Outbut = billAirLine.OrderBy(a => a.EDate);
                if (Outbut == null)
                {
                    return NotFound();
                }
                return View(Outbut);

            }
            if (Convert.ToInt32(TempData["Name1"]) == 0)
            {
                var billAirLine = await (from x in _context.BillAirLines select new Models.ViewModel.Transactions { EId = x.BillId, Type = "ف طيران", EDate = x.BillDate, Direction = x.Direction, eTicketNumber = x.eTicketNumber, PNR = x.PNR, TicketExportName = x.TicketExport.Name, CustomerName = x.Commnets, CustomerOrSupplierName = x.CustomerOrSupplier.Name + " - " + x.CustomerSupplier.Name, Statement = string.Concat(x.MenuLE0.M0_Name, " - ", x.MenuLE1.M1_Name, " - ", x.MenuLE2.M2_Name), Debit = x.NetPrice, Credit = x.CustomerPrice }).Where(d => d.EDate >= Convert.ToDateTime(TempData["StartDate"])).Where(d => d.EDate.Date <= Convert.ToDateTime(TempData["EndDate"])).ToListAsync();
                var Outbut = billAirLine.OrderBy(a => a.EDate);
                return View(Outbut);

            }

            var UserId = _userManager.GetUserId(User);
            var CompanyID = (await _userManager.Users.SingleOrDefaultAsync(a => a.Id == UserId)).CompanyId;
            ViewData["CompanyLogo"] = "~/" + TravelERP.Properties.Resources.ImgFolder + "/" + (await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyID)).CompanyLogo;
            ViewData["CompanyA4EsalImage"] = "~/" + TravelERP.Properties.Resources.ImgFolder + "/" + (await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyID)).CompanyA4EsalImage;
            ViewData["CompanyNameE"] = (await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyID)).Company_NameE;
            ViewData["CompanyName"] = (await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyID)).Company_Name;


            return View();
        }


        private bool BillAirLineExists(int id)
        {
            return _context.BillAirLines.Any(e => e.Id == id);
        }


    }
}
