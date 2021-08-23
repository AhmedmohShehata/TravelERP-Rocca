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
    [Authorize(Roles = CustomRoles.Admin + "," + CustomRoles.BranchManager + "," + CustomRoles.BillOutsides)]
    public class BillForeignsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public BillForeignsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;

        }

        // GET: BillForeign/ Index **** الصفحه الرئيسيه لفواتير السياحه الخارجيه ****
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

        // GET: BillForeign/Details/5   **** تفاصيل فاتوره السياحه الخارجيه ****
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var billForeign = await _context.BillForeigns
                .Include(b => b.CustomerOrSupplier)
                .Include(b => b.CustomerSupplier)
                .Include(b => b.MenuLE0)
                .Include(b => b.MenuLE1)
                .Include(b => b.MenuLE2)
                .Include(b => b.TicketExport)
                .Include(b => b.User)
                .Include(b => b.Company)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (billForeign == null)
            {
                return NotFound();
            }

            return View(billForeign);
        }


        [Authorize(Roles = CustomRoles.Admin + "," + CustomRoles.BranchManager + "," + CustomRoles.EsalOutsides)]
        // GET: BillForeigns/Create  **** إنشاء فاتوره سياحه خارجيه جديده *****
        public async Task<IActionResult> Create()
        {
            var CompanyId =(await _context.Users.SingleOrDefaultAsync(b => b.Id == _userManager.GetUserId(User))).CompanyId;
            var customersSuppliers =await (from user in _context.CustomersSuppliers.Where(a => a.CompanyId == CompanyId)
                                      where !_context.NonSuppliers.Any(f => f.CustomerSupplierId == user.Id)
                                      select new
                                      {
                                          user.Id,
                                          user.Name,
                                          user.CustomerOrSupplier,
                                          user.CustomerOrSupplierId
                                      }).ToListAsync();

            ViewData["CustomerOrSupplierId"] = new SelectList(_context.CustomerOrSuppliers, "Id", "Name");
            ViewData["MenuLE0Id"] = new SelectList(_context.MenuLE0, "Id", "M0_Name");
            ViewData["MenuLE1Id"] = new SelectList(_context.MenuLE1.Where(a => a.MenuLE0.M0_Name == "سياحة خارجية"), "Id", "M1_Name");
            ViewData["MenuLE2Id"] = new SelectList(_context.MenuLE2, "Id", "M2_Name");
            ViewData["TicketExportId"] = new SelectList(_context.CustomersSuppliers.Where(a => a.CompanyId == CompanyId).Where(a => a.CustomerOrSupplier.Name == "مورد او وكيل").OrderBy(a => a.Name), "Id", "Name");
            return View();
        }
        [Authorize(Roles = CustomRoles.Admin + "," + CustomRoles.BranchManager + "," + CustomRoles.EsalOutsides)]

        // POST: BillForeigns/Create   **** إنشاء فاتوره سياحه خارجيه جديده *****
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CustomerOrSupplierId,CustomerSupplierId,MenuLE0Id,MenuLE1Id,MenuLE2Id,TicketExportId,AdultN,ChildN,CustomerPrice,NetPrice,TicketFrom,TicketTo,Commnets")] BillForeign billForeign)
        {
            var UserId = _userManager.GetUserId(User);
            var CompanyId =(await _userManager.Users.SingleOrDefaultAsync(a => a.Id == UserId)).CompanyId;

            if (ModelState.IsValid)
            {
                billForeign.UserId = UserId;
                var CompanyCuntry =(await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyId)).DateTimeCountry;
                var info = TimeZoneInfo.FindSystemTimeZoneById(CompanyCuntry);
                DateTimeOffset localServerTime = DateTimeOffset.Now;
                DateTimeOffset localTime = TimeZoneInfo.ConvertTime(localServerTime, info);
                billForeign.BillDate = localTime.DateTime;
                billForeign.MenuLE0Id =(await _context.MenuLE0.SingleOrDefaultAsync(a => a.M0_Name == "سياحة خارجية")).Id;
                billForeign.EMPCommission = (billForeign.CustomerPrice - billForeign.NetPrice) * ((_context.UsersDetails.Where(a => a.UserId == billForeign.UserId).Count()) != 0 ? _context.UsersDetails.SingleOrDefault(a => a.UserId == billForeign.UserId).Commission : 0);
                billForeign.CompanyID = CompanyId;
                if (await _context.BillForeigns.Where(a => a.CompanyID == billForeign.CompanyID).CountAsync() == 0)
                {
                    billForeign.BillId = 1;
                    TempData["BillId"] = 1;
                }
                else
                {
                    billForeign.BillId =await _context.BillForeigns.Where(a => a.CompanyID == billForeign.CompanyID).MaxAsync(a => a.BillId) + 1;
                    TempData["BillId"] = billForeign.BillId;

                }
                _context.Add(billForeign);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", new RouteValueDictionary(
                        new { controller = "BillForeigns", action = "Details", Id = billForeign.Id }));

            }
            var customersSuppliers =await (from user in _context.CustomersSuppliers.Where(a => a.CompanyId == CompanyId)
                                      //where !_context.NonSuppliers.Any(f => f.CustomerSupplierId == user.Id)
                                      select new
                                      {
                                          user.Id,
                                          user.Name,
                                          user.CustomerOrSupplier,
                                          user.CustomerOrSupplierId
                                      }).ToListAsync();

            ViewData["CustomerOrSupplierId"] = new SelectList(_context.CustomerOrSuppliers, "Id", "Name", billForeign.CustomerOrSupplierId);
            ViewData["CustomerSupplierId"] = new SelectList(customersSuppliers.Where(a=>a.CustomerOrSupplierId == billForeign.CustomerOrSupplierId).OrderBy(a=>a.Name), "Id", "Name", billForeign.CustomerSupplierId);
            ViewData["MenuLE0Id"] = new SelectList(_context.MenuLE0, "Id", "M0_Name", billForeign.MenuLE0Id);
            ViewData["MenuLE1Id"] = new SelectList(_context.MenuLE1, "Id", "M1_Name", billForeign.MenuLE1Id);
            ViewData["MenuLE2Id"] = new SelectList(_context.MenuLE2, "Id", "M2_Name", billForeign.MenuLE2Id);
            ViewData["TicketExportId"] = new SelectList(customersSuppliers.Where(a => a.CustomerOrSupplier.Name == "مورد او وكيل").OrderBy(a => a.Name), "Id", "Name", billForeign.TicketExportId);
            return View(billForeign);
        }
        
        
        [Authorize(Roles = CustomRoles.Admin + "," + CustomRoles.BranchManager + "," + CustomRoles.Edits)]
        // GET: BillForeigns/Edit/5   **** تعديل فاتوره سياحه الخارجيه ****
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var billForeign = await _context.BillForeigns.SingleOrDefaultAsync(m => m.Id == id);
            if (billForeign == null)
            {
                return NotFound();
            }
            var CompanyId =(await _userManager.Users.SingleOrDefaultAsync(a => a.Id == billForeign.UserId)).CompanyId;
            var customersSuppliers =await (from user in _context.CustomersSuppliers.Where(a => a.CompanyId == CompanyId)
                                      //where !_context.NonSuppliers.Any(f => f.CustomerSupplierId == user.Id)
                                      select new
                                      {
                                          user.Id,
                                          user.Name,
                                          user.CustomerOrSupplier,
                                          user.CustomerOrSupplierId
                                      }).ToListAsync();

            ViewData["CustomerOrSupplierId"] = new SelectList(_context.CustomerOrSuppliers.Where(a => a.Name != "مندوب"), "Id", "Name", billForeign.CustomerOrSupplierId);
            ViewData["CustomerSupplierId"] = new SelectList(customersSuppliers.Where(a => a.CustomerOrSupplierId == billForeign.CustomerOrSupplierId).OrderBy(a => a.Name), "Id", "Name", billForeign.CustomerSupplierId);
            ViewData["MenuLE0Id"] = new SelectList(_context.MenuLE0, "Id", "M0_Name", billForeign.MenuLE0Id);
            ViewData["MenuLE1Id"] = new SelectList(_context.MenuLE1.Where(a => a.MenuLE0Id == billForeign.MenuLE0Id), "Id", "M1_Name", billForeign.MenuLE1Id);
            ViewData["MenuLE2Id"] = new SelectList(_context.MenuLE2.Where(a => a.MenuLE1Id == billForeign.MenuLE1Id), "Id", "M2_Name", billForeign.MenuLE2Id);
            ViewData["TicketExportId"] = new SelectList(customersSuppliers.Where(a => a.CustomerOrSupplier.Name == "مورد او وكيل").OrderBy(a => a.Name), "Id", "Name", billForeign.TicketExportId);
            ViewData["UserId"] = new SelectList(_context.Users.Where(a => a.CompanyId == CompanyId), "Id", "UserName", billForeign.UserId);
            return View(billForeign);
        }



        [Authorize(Roles = CustomRoles.Admin + "," + CustomRoles.BranchManager + "," + CustomRoles.Edits)]
        // POST: BillForeigns/Edit/5   **** تعديل فاتوره سياحه خارجيه ****
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BillId,BillDate,CustomerOrSupplierId,CustomerSupplierId,MenuLE0Id,MenuLE1Id,MenuLE2Id,TicketExportId,AdultN,ChildN,CustomerPrice,NetPrice,TicketFrom,TicketTo,Commnets,UserId,CompanyID")] BillForeign billForeign)
        {
            if (id != billForeign.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    billForeign.EMPCommission = (billForeign.CustomerPrice - billForeign.NetPrice) * ((_context.UsersDetails.Where(a => a.UserId == billForeign.UserId).Count()) != 0 ?(await _context.UsersDetails.SingleOrDefaultAsync(a => a.UserId == billForeign.UserId)).Commission : 0);
                    _context.Update(billForeign);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BillForeignExists(billForeign.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", new RouteValueDictionary(
                        new { controller = "BillForeigns", action = "Details", Id = billForeign.Id }));
            }
            var CompanyId =(await _userManager.Users.SingleOrDefaultAsync(a => a.Id == billForeign.UserId)).CompanyId;
            var customersSuppliers =await (from user in _context.CustomersSuppliers.Where(a => a.CompanyId == CompanyId)
                                      where !_context.NonSuppliers.Any(f => f.CustomerSupplierId == user.Id)
                                      select new
                                      {
                                          user.Id,
                                          user.Name,
                                          user.CustomerOrSupplier,
                                          user.CustomerOrSupplierId
                                      }).ToListAsync();

            ViewData["CustomerOrSupplierId"] = new SelectList(_context.CustomerOrSuppliers, "Id", "Name", billForeign.CustomerOrSupplierId);
            ViewData["CustomerSupplierId"] = new SelectList(customersSuppliers.Where(a => a.CustomerOrSupplierId == billForeign.CustomerOrSupplierId).OrderBy(a => a.Name), "Id", "Name", billForeign.CustomerSupplierId);
            ViewData["MenuLE0Id"] = new SelectList(_context.MenuLE0, "Id", "M0_Name", billForeign.MenuLE0Id);
            ViewData["MenuLE1Id"] = new SelectList(_context.MenuLE1, "Id", "M1_Name", billForeign.MenuLE1Id);
            ViewData["MenuLE2Id"] = new SelectList(_context.MenuLE2, "Id", "M2_Name", billForeign.MenuLE2Id);
            ViewData["TicketExportId"] = new SelectList(customersSuppliers.Where(a => a.CustomerOrSupplier.Name == "مورد او وكيل").OrderBy(a => a.Name), "Id", "Name", billForeign.TicketExportId);
            ViewData["UserId"] = new SelectList(_context.Users.Where(a => a.CompanyId == CompanyId), "Id", "UserName", billForeign.UserId);
            return View(billForeign);
        }



        [Authorize(Roles = CustomRoles.Admin + "," + CustomRoles.BranchManager + "," + CustomRoles.EsalOutsides)]
        // GET: BillForeigns/CreateEsal/5   **** إنشاء ايصال لفاتوره السياحه الخارجيه  ****
        public async Task<IActionResult> CreateEsal(int? Id)
        {
            var bill =await _context.BillForeigns.SingleOrDefaultAsync(a => a.Id == Id);

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
        [Authorize(Roles = CustomRoles.Admin + "," + CustomRoles.BranchManager + "," + CustomRoles.EsalOutsides)]
        // POST: BillForeigns/CreateEsal/5   **** إنشاء ايصال لفاتوره السياحه الخارجيه  ****
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEsal([Bind("BillId,BillIdId,MenuLE0Id,MenuLE1Id,MenuLE2Id,CustomerOrSupplierId,CustomerSupplierId,TicketExportId,AmountPaid,PaymentMethodId,DepositDesc")] Esal esal)
        {
            esal.UserId = _userManager.GetUserId(User);
            var CompanyId =(await _userManager.Users.SingleOrDefaultAsync(a => a.Id == esal.UserId)).CompanyId;
            var CompanyCuntry =(await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyId)).DateTimeCountry;
            var info = TimeZoneInfo.FindSystemTimeZoneById(CompanyCuntry);
            DateTimeOffset localServerTime = DateTimeOffset.Now;
            DateTimeOffset localTime = TimeZoneInfo.ConvertTime(localServerTime, info);
            esal.EsalDate = localTime.DateTime;

            if (ModelState.IsValid)
            {
                if (_context.Esals.Where(a => a.CompanyID == CompanyId).Count() == 0)
                {
                    esal.EsalId = 1;
                }
                else
                {
                    esal.EsalId = _context.Esals.Where(a => a.CompanyID == CompanyId).Max(a => a.EsalId) + 1;
                }
                esal.CompanyID = CompanyId;
                _context.Add(esal);
                await _context.SaveChangesAsync();
                return RedirectToAction("Print", new RouteValueDictionary(
                new { controller = "Esals", action = "Print", Id = esal.Id }));
            }
            var bill =await _context.BillForeigns.SingleOrDefaultAsync(a => a.Id == esal.BillId);

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


        private bool BillForeignExists(int id)
        {
            return _context.BillForeigns.Any(e => e.Id == id);
        }


    }
}
