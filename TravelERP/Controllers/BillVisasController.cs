using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using TravelERP.Data;
using TravelERP.Models;
using static TravelERP.Controllers.AccountController;

namespace TravelERP.Controllers
{
    [Authorize(Roles = CustomRoles.Admin + "," + CustomRoles.BranchManager + "," + CustomRoles.BillVisas)]
    public class BillVisasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public BillVisasController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        // GET:BillVisas/Index **** الصفحه الرئيسيه لفواتير التأشيرات ****
        public async Task<IActionResult> Index()
        {
            var CompanyId =(await _userManager.Users.SingleOrDefaultAsync(a => a.Id == _userManager.GetUserId(User))).CompanyId;
            var CompanyCuntry = (await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyId)).DateTimeCountry;
            var info = TimeZoneInfo.FindSystemTimeZoneById(CompanyCuntry);
            DateTimeOffset localServerTime = DateTimeOffset.Now;
            DateTimeOffset localTime = TimeZoneInfo.ConvertTime(localServerTime, info);
            DateTime datetimenow = localTime.DateTime;
            ViewData["DateTimeNow"] = datetimenow.Date.ToShortDateString();
            return View();
        }


        // GET: BillVisas/Details/5 **** تفاصيل فاتوره تأشيرات ****
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var billVisa = await _context.BillVisas
                .Include(b => b.CustomerOrSupplier)
                .Include(b => b.CustomerSupplier)
                .Include(b => b.MenuLE0)
                .Include(b => b.MenuLE1)
                .Include(b => b.MenuLE2)
                .Include(b => b.TicketExport)
                .Include(b => b.User)
                .Include(b => b.Company)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (billVisa == null)
            {
                return NotFound();
            }
            return View(billVisa);
        }


        [Authorize(Roles = CustomRoles.Admin + "," + CustomRoles.BranchManager + "," + CustomRoles.EsalVisas)]
        // GET: BillVisas/Create **** إنشاء فاتوره تأشيرات جديده *****
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
            ViewData["MenuLE1Id"] = new SelectList(_context.MenuLE1.Where(a => a.MenuLE0.M0_Name == "تأشيرات"), "Id", "M1_Name");
            ViewData["MenuLE2Id"] = new SelectList(_context.MenuLE2, "Id", "M2_Name");
            ViewData["TicketExportId"] = new SelectList(_context.CustomersSuppliers.Where(a => a.CompanyId == CompanyId).Where(a => a.CustomerOrSupplier.Name == "مورد او وكيل").OrderBy(a => a.Name), "Id", "Name");
            return View();
        }

        [Authorize(Roles = CustomRoles.Admin + "," + CustomRoles.BranchManager + "," + CustomRoles.EsalVisas)]
        // POST: BillVisas/Create **** إنشاء فاتوره تأشيرات جديده *****
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CustomerOrSupplierId,CustomerSupplierId,MenuLE0Id,MenuLE1Id,MenuLE2Id,TicketExportId,AdultN,ChildN,CustomerPrice,NetPrice,PassportNo,Commnets")] BillVisa billVisa)
        {
            var UserId = _userManager.GetUserId(User);
            var CompanyId =(await _userManager.Users.SingleOrDefaultAsync(a => a.Id == UserId)).CompanyId;

            if (ModelState.IsValid)
            {
                billVisa.UserId = UserId;
                var CompanyCuntry =(await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyId)).DateTimeCountry;
                var info = TimeZoneInfo.FindSystemTimeZoneById(CompanyCuntry);
                DateTimeOffset localServerTime = DateTimeOffset.Now;
                DateTimeOffset localTime = TimeZoneInfo.ConvertTime(localServerTime, info);
                billVisa.BillDate = localTime.DateTime;
                billVisa.ApprovedDate = billVisa.BillDate;
                billVisa.AdultN = 1;
                billVisa.ChildN = 0;
                billVisa.MenuLE0Id =(await _context.MenuLE0.SingleOrDefaultAsync(a => a.M0_Name == "تأشيرات")).Id;
                billVisa.EMPCommission = (billVisa.CustomerPrice - billVisa.NetPrice) * ((await _context.UsersDetails.Where(a => a.UserId == billVisa.UserId).CountAsync()) != 0 ?(await _context.UsersDetails.SingleOrDefaultAsync(a => a.UserId == billVisa.UserId)).Commission : 0);
                billVisa.CompanyID = CompanyId;
                if (await _context.BillVisas.Where(a => a.CompanyID == billVisa.CompanyID).CountAsync() == 0)
                {
                    billVisa.BillId = 1;
                    TempData["BillId"] = 1;
                }
                else
                {
                    billVisa.BillId = await _context.BillVisas.Where(a => a.CompanyID == billVisa.CompanyID).MaxAsync(a => a.BillId) + 1;
                    TempData["BillId"] = billVisa.BillId;

                }
                _context.Add(billVisa);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", new RouteValueDictionary(
                                        new { controller = "BillVisas", action = "Details", Id = billVisa.Id }));
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

            ViewData["CustomerOrSupplierId"] = new SelectList(_context.CustomerOrSuppliers, "Id", "Name", billVisa.CustomerOrSupplierId);
            ViewData["CustomerSupplierId"] = new SelectList(customersSuppliers.Where(a=>a.CustomerOrSupplierId == billVisa.CustomerOrSupplierId).OrderBy(x=>x.Name), "Id", "Name", billVisa.CustomerSupplierId);
            ViewData["MenuLE0Id"] = new SelectList(_context.MenuLE0, "Id", "M0_Name", billVisa.MenuLE0Id);
            ViewData["MenuLE1Id"] = new SelectList(_context.MenuLE1, "Id", "M1_Name", billVisa.MenuLE1Id);
            ViewData["MenuLE2Id"] = new SelectList(_context.MenuLE2, "Id", "M2_Name", billVisa.MenuLE2Id);
            ViewData["TicketExportId"] = new SelectList(customersSuppliers.Where(a => a.CustomerOrSupplier.Name == "مورد او وكيل").OrderBy(x=>x.Name), "Id", "Name", billVisa.TicketExportId);
            return View(billVisa);
        }



        [Authorize(Roles = CustomRoles.Admin + "," + CustomRoles.BranchManager + "," + CustomRoles.Edits)]
        // GET: BillVisas/Edit/5   **** تعديل فاتوره تأشيرات ****
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var billVisa = await _context.BillVisas.SingleOrDefaultAsync(m => m.Id == id);
            if (billVisa == null)
            {
                return NotFound();
            }
            var CompanyId =(await _userManager.Users.SingleOrDefaultAsync(a => a.Id == billVisa.UserId)).CompanyId;
            var customersSuppliers =await (from user in _context.CustomersSuppliers.Where(a => a.CompanyId == CompanyId)
                                      //where !_context.NonSuppliers.Any(f => f.CustomerSupplierId == user.Id)
                                      select new
                                      {
                                          user.Id,
                                          user.Name,
                                          user.CustomerOrSupplier,
                                          user.CustomerOrSupplierId
                                      }).ToListAsync();

            ViewData["CustomerOrSupplierId"] = new SelectList(_context.CustomerOrSuppliers.Where(a => a.Name != "مندوب"), "Id", "Name", billVisa.CustomerOrSupplierId);
            ViewData["CustomerSupplierId"] = new SelectList(customersSuppliers.Where(a=>a.CustomerOrSupplierId == billVisa.CustomerOrSupplierId).OrderBy(a => a.Name), "Id", "Name", billVisa.CustomerSupplierId);
            ViewData["MenuLE0Id"] = new SelectList(_context.MenuLE0, "Id", "M0_Name", billVisa.MenuLE0Id);
            ViewData["MenuLE1Id"] = new SelectList(_context.MenuLE1.Where(a=>a.MenuLE0Id == billVisa.MenuLE0Id), "Id", "M1_Name", billVisa.MenuLE1Id);
            ViewData["MenuLE2Id"] = new SelectList(_context.MenuLE2.Where(a=>a.MenuLE1Id == billVisa.MenuLE1Id), "Id", "M2_Name", billVisa.MenuLE2Id);
            ViewData["TicketExportId"] = new SelectList(customersSuppliers.Where(a => a.CustomerOrSupplier.Name == "مورد او وكيل").OrderBy(a=>a.Name), "Id", "Name", billVisa.TicketExportId);
            ViewData["UserId"] = new SelectList(_context.Users.Where(a=>a.CompanyId == CompanyId), "Id", "UserName", billVisa.UserId);
            return View(billVisa);
        }

        [Authorize(Roles = CustomRoles.Admin + "," + CustomRoles.BranchManager + "," + CustomRoles.EsalVisas)]
        // POST: BillVisas/Edit/5  **** تعديل فاتوره تأشيرات ****
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BillId,BillDate,ApprovedDate,CustomerOrSupplierId,CustomerSupplierId,MenuLE0Id,MenuLE1Id,MenuLE2Id,TicketExportId,AdultN,ChildN,CustomerPrice,NetPrice,Commnets,UserId,PassportNo,BillState,CompanyID")] BillVisa billVisa)
        {
            if (id != billVisa.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    billVisa.EMPCommission = (billVisa.CustomerPrice - billVisa.NetPrice) * ((_context.UsersDetails.Where(a => a.UserId == billVisa.UserId).Count()) != 0 ? _context.UsersDetails.SingleOrDefault(a => a.UserId == billVisa.UserId).Commission : 0);
                    _context.Update(billVisa);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BillVisaExists(billVisa.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", new RouteValueDictionary(
                                        new { controller = "BillVisas", action = "Details", Id = billVisa.Id }));
            }
            var CompanyId =(await _userManager.Users.SingleOrDefaultAsync(a => a.Id == billVisa.UserId)).CompanyId;
            var customersSuppliers =await (from user in _context.CustomersSuppliers.Where(a => a.CompanyId == CompanyId)
                                      //where !_context.NonSuppliers.Any(f => f.CustomerSupplierId == user.Id)
                                      select new
                                      {
                                          user.Id,
                                          user.Name,
                                          user.CustomerOrSupplier,
                                          user.CustomerOrSupplierId
                                      }).ToListAsync();

            ViewData["CustomerOrSupplierId"] = new SelectList(_context.CustomerOrSuppliers, "Id", "Name", billVisa.CustomerOrSupplierId);
            ViewData["CustomerSupplierId"] = new SelectList(customersSuppliers.Where(a => a.CustomerOrSupplierId == billVisa.CustomerOrSupplierId).OrderBy(a => a.Name), "Id", "Name", billVisa.CustomerSupplierId);
            ViewData["MenuLE0Id"] = new SelectList(_context.MenuLE0, "Id", "M0_Name", billVisa.MenuLE0Id);
            ViewData["MenuLE1Id"] = new SelectList(_context.MenuLE1, "Id", "M1_Name", billVisa.MenuLE1Id);
            ViewData["MenuLE2Id"] = new SelectList(_context.MenuLE2, "Id", "M2_Name", billVisa.MenuLE2Id);
            ViewData["TicketExportId"] = new SelectList(customersSuppliers.Where(a => a.CustomerOrSupplier.Name == "مورد او وكيل").OrderBy(x=>x.Name), "Id", "Name", billVisa.TicketExportId);
            ViewData["UserId"] = new SelectList(_context.Users.Where(a => a.CompanyId == CompanyId), "Id", "UserName", billVisa.UserId);
            return View(billVisa);
        }


        // GET: BillAirLines/CreateEsal/5   **** إنشاء ايصال لفاتوره تأشيرات  ****
        [Authorize(Roles = CustomRoles.Admin + "," + CustomRoles.BranchManager + "," + CustomRoles.EsalVisas)]
        public async Task<IActionResult> CreateEsal(int? Id)
        {
            var bill =await _context.BillVisas.SingleOrDefaultAsync(a => a.Id == Id);

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
            return View();
        }


        [Authorize(Roles = CustomRoles.Admin + "," + CustomRoles.BranchManager + "," + CustomRoles.EsalVisas)]
        // POST: BillAirLines/CreateEsal/5 **** إنشاء ايصال لفاتوره تأشيرات  ****
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEsal([Bind("BillId,BillIdId,MenuLE0Id,MenuLE1Id,MenuLE2Id,CustomerOrSupplierId,CustomerSupplierId,TicketExportId,AmountPaid,PaymentMethodId,DepositDesc")] Esal esal)
        {
            if (ModelState.IsValid)
            {
                esal.UserId = _userManager.GetUserId(User);
                var CompanyId =(await _userManager.Users.SingleOrDefaultAsync(a => a.Id == esal.UserId)).CompanyId;

                if (_context.Esals.Where(a => a.CompanyID == CompanyId).Count() == 0)
                {
                    esal.EsalId = 1;
                }
                else
                {
                    esal.EsalId = _context.Esals.Where(a => a.CompanyID == CompanyId).Max(a => a.EsalId) + 1;
                }
                var CompanyCuntry =(await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyId)).DateTimeCountry;
                var info = TimeZoneInfo.FindSystemTimeZoneById(CompanyCuntry);
                DateTimeOffset localServerTime = DateTimeOffset.Now;
                DateTimeOffset localTime = TimeZoneInfo.ConvertTime(localServerTime, info);
                esal.CompanyID = CompanyId;
                esal.EsalDate = localTime.DateTime;
                _context.Add(esal);
                await _context.SaveChangesAsync();
                return RedirectToAction("Print", new RouteValueDictionary(
    new { controller = "Esals", action = "Print", Id = esal.Id }));
            }
            var bill =await _context.BillVisas.SingleOrDefaultAsync(a => a.Id == esal.BillId);

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
            var bill = await _context.BillVisas.SingleOrDefaultAsync(a => a.Id == Id);

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
            var bill = await _context.BillVisas.SingleOrDefaultAsync(a => a.Id == ezn.BillId);

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


        //BillVisas/BillStatePaindingUser

        public async Task<IActionResult> BillStatePaindingUser ()
        {
            var CompanyId =(await _context.Users.SingleOrDefaultAsync(a => a.Id == _userManager.GetUserId(User))).CompanyId;
            var UserId = _userManager.GetUserId(User);

            var billvisas = await _context.BillVisas.Include(a=>a.CustomerSupplier).Include(a=>a.TicketExport).Where(a => a.BillState == false).Where(a => a.CompanyID == CompanyId).Where(a => a.UserId == UserId).ToListAsync();
            return View(billvisas);
        }

        [Authorize(Roles = CustomRoles.Admin)]
        public async Task<IActionResult> BillStatePaindingAdmin()
        {
            var CompanyId =(await _context.Users.SingleOrDefaultAsync(a => a.Id == _userManager.GetUserId(User))).CompanyId;
            var UserId = _userManager.GetUserId(User);

            var billvisas = await _context.BillVisas.Include(a => a.CustomerSupplier).Include(a => a.TicketExport).Include(a=>a.User).Include(a=>a.Company).Where(a => a.BillState == false).ToListAsync();
            return View(billvisas);
        }
        [Authorize(Roles = CustomRoles.BranchManager)]
        public async Task<IActionResult> BillStatePainding()
        {
            var CompanyId =(await _context.Users.SingleOrDefaultAsync(a => a.Id == _userManager.GetUserId(User))).CompanyId;
            var UserId = _userManager.GetUserId(User);

            var billvisas = await _context.BillVisas.Include(a => a.CustomerSupplier).Include(a => a.TicketExport).Include(a=>a.User).Where(a => a.BillState == false).Where(a => a.CompanyID == CompanyId).ToListAsync();
            return View(billvisas);
        }

        private bool BillVisaExists(int id)
        {
            return _context.BillVisas.Any(e => e.Id == id);
        }

        [HttpGet]
        public async Task<IActionResult> BillApproved (int? id)
        {
            var billvisa = await _context.BillVisas.Include(a=>a.MenuLE0).Include(a=>a.MenuLE1).Include(a=>a.MenuLE2).Include(a => a.CustomerOrSupplier).Include(a => a.CustomerSupplier).Include(a => a.User).Include(a => a.Company).Include(a => a.TicketExport).FirstOrDefaultAsync(x => x.Id == id);

            return PartialView("_VisasPendingEdit", billvisa);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BillApproved (int id, BillVisa billVisa)
        {
            if (billVisa == null)
            {
                return NotFound();
            }
            var UserId = _userManager.GetUserId(User);
            var CompanyId =(await _userManager.Users.SingleOrDefaultAsync(a => a.Id == UserId)).CompanyId;
            var CompanyCuntry =(await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyId)).DateTimeCountry;
            var info = TimeZoneInfo.FindSystemTimeZoneById(CompanyCuntry);
            DateTimeOffset localServerTime = DateTimeOffset.Now;
            DateTimeOffset localTime = TimeZoneInfo.ConvertTime(localServerTime, info);
            billVisa.ApprovedDate = localTime.DateTime;
            billVisa.BillState = true;
            billVisa.EMPCommission = (billVisa.CustomerPrice - billVisa.NetPrice) * ((_context.UsersDetails.Where(a => a.UserId == billVisa.UserId).Count()) != 0 ? _context.UsersDetails.SingleOrDefault(a => a.UserId == billVisa.UserId).Commission : 0);

            _context.Update(billVisa);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(BillStatePaindingUser));
        }
    }
}
