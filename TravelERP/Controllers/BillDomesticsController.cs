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
    [Authorize(Roles = CustomRoles.Admin + "," + CustomRoles.BranchManager + "," + CustomRoles.BillDomestics)]
    public class BillDomesticsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public BillDomesticsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;

        }

        // GET: BillDomestic/ Index **** الصفحه الرئيسيه لفواتير السياحه الداخليه ****
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

        // GET: BillDomestic/Details/5   **** تفاصيل فاتوره السياحه الداخليه ****
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var billDomestic = await _context.BillDomestic
                .Include(b => b.CustomerOrSupplier)
                .Include(b => b.CustomerSupplier)
                .Include(b => b.MenuLE0)
                .Include(b => b.MenuLE1)
                .Include(b => b.MenuLE2)
                .Include(b => b.TicketExport)
                .Include(b => b.User)
                .Include(b => b.Company)
                .Include(b=> b.TransportMethod)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (billDomestic == null)
            {
                return NotFound();
            }

            return View(billDomestic);
        }


        [Authorize(Roles = CustomRoles.Admin + "," + CustomRoles.BranchManager + "," + CustomRoles.EsalDomestics)]
        // GET: BillDomestics/Create  **** إنشاء فاتوره سياحه داخليه جديده *****
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
            ViewData["MenuLE1Id"] = new SelectList(_context.MenuLE1.Where(a => a.MenuLE0.M0_Name == "سياحة داخلية"), "Id", "M1_Name");
            ViewData["MenuLE2Id"] = new SelectList(_context.MenuLE2, "Id", "M2_Name");
            ViewData["TicketExportId"] = new SelectList(_context.CustomersSuppliers.Where(a => a.CompanyId == CompanyId).Where(a => a.CustomerOrSupplier.Name == "مورد او وكيل").OrderBy(a => a.Name), "Id", "Name");
            return View();
        }
        [Authorize(Roles = CustomRoles.Admin + "," + CustomRoles.BranchManager + "," + CustomRoles.EsalDomestics)]

        // POST: BillDomestics/Create   **** إنشاء فاتوره سياحه داخليه جديده *****
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CustomerOrSupplierId,CustomerSupplierId,MenuLE0Id,MenuLE1Id,MenuLE2Id,TicketExportId,AdultN,ChildN,CustomerPrice,NetPrice,TicketFrom,TicketTo,Commnets")] BillDomestic billDomestic)
        {
            var UserId = _userManager.GetUserId(User);
            var CompanyId =(await _userManager.Users.SingleOrDefaultAsync(a => a.Id == UserId)).CompanyId;

            if (ModelState.IsValid)
            {
                billDomestic.UserId = UserId;
                var CompanyCuntry =(await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyId)).DateTimeCountry;
                var info = TimeZoneInfo.FindSystemTimeZoneById(CompanyCuntry);
                DateTimeOffset localServerTime = DateTimeOffset.Now;
                DateTimeOffset localTime = TimeZoneInfo.ConvertTime(localServerTime, info);
                billDomestic.BillDate = localTime.DateTime;
                billDomestic.MenuLE0Id =(await _context.MenuLE0.SingleOrDefaultAsync(a => a.M0_Name == "سياحة داخلية")).Id;
                billDomestic.EMPCommission = (billDomestic.CustomerPrice - billDomestic.NetPrice) * ((_context.UsersDetails.Where(a => a.UserId == billDomestic.UserId).Count()) != 0 ? _context.UsersDetails.SingleOrDefault(a => a.UserId == billDomestic.UserId).Commission : 0);
                billDomestic.CompanyID = CompanyId;
                if (await _context.BillDomestic.Where(a => a.CompanyID == billDomestic.CompanyID).CountAsync() == 0)
                {
                    billDomestic.BillId = 1;
                    TempData["BillId"] = 1;
                }
                else
                {
                    billDomestic.BillId =await _context.BillDomestic.Where(a => a.CompanyID == billDomestic.CompanyID).MaxAsync(a => a.BillId) + 1;
                    TempData["BillId"] = billDomestic.BillId;

                }
                _context.Add(billDomestic);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", new RouteValueDictionary(
                        new { controller = "BillDomestics", action = "Details", Id = billDomestic.Id }));

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

            ViewData["CustomerOrSupplierId"] = new SelectList(_context.CustomerOrSuppliers, "Id", "Name", billDomestic.CustomerOrSupplierId);
            ViewData["CustomerSupplierId"] = new SelectList(customersSuppliers.Where(a=>a.CustomerOrSupplierId == billDomestic.CustomerOrSupplierId).OrderBy(a=>a.Name), "Id", "Name", billDomestic.CustomerSupplierId);
            ViewData["MenuLE0Id"] = new SelectList(_context.MenuLE0, "Id", "M0_Name", billDomestic.MenuLE0Id);
            ViewData["MenuLE1Id"] = new SelectList(_context.MenuLE1, "Id", "M1_Name", billDomestic.MenuLE1Id);
            ViewData["MenuLE2Id"] = new SelectList(_context.MenuLE2, "Id", "M2_Name", billDomestic.MenuLE2Id);
            ViewData["TicketExportId"] = new SelectList(customersSuppliers.Where(a => a.CustomerOrSupplier.Name == "مورد او وكيل").OrderBy(a => a.Name), "Id", "Name", billDomestic.TicketExportId);
            return View(billDomestic);
        }


        [Authorize(Roles = CustomRoles.Admin + "," + CustomRoles.BranchManager + "," + CustomRoles.EsalDomestics)]
        // GET: BillDomestics/CreateIndividual  **** إنشاء فاتوره سياحه داخليه جديده فردى *****
        public async Task<IActionResult> CreateIndividual()
        {
            var CompanyId = (await _context.Users.SingleOrDefaultAsync(b => b.Id == _userManager.GetUserId(User))).CompanyId;
            var customersSuppliers = await (from user in _context.CustomersSuppliers.Where(a => a.CompanyId == CompanyId)
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
            ViewData["TransportMethodId"] = new SelectList(_context.TransportMethods, "Id", "Name");
            ViewData["MenuLE1Id"] = new SelectList(_context.MenuLE1.Where(a => a.MenuLE0.M0_Name == "سياحة داخلية"), "Id", "M1_Name");
            ViewData["MenuLE2Id"] = new SelectList(_context.MenuLE2, "Id", "M2_Name");
            ViewData["TicketExportId"] = new SelectList(_context.CustomersSuppliers.Where(a => a.CompanyId == CompanyId).Where(a => a.CustomerOrSupplier.Name == "مورد او وكيل").OrderBy(a => a.Name), "Id", "Name");
            return View();
        }
        [Authorize(Roles = CustomRoles.Admin + "," + CustomRoles.BranchManager + "," + CustomRoles.EsalDomestics)]

        // POST: BillDomestics/CreateIndividual   **** إنشاء فاتوره سياحه داخليه جديده فردى *****
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateIndividual([Bind("Id,CustomerOrSupplierId,CustomerSupplierId,MenuLE0Id,MenuLE1Id,MenuLE2Id,TicketExportId,AdultN,ChildN,CustomerPrice,NetPrice,TicketFrom,TicketTo,Commnets,TransportMethodId,TMNetPrice,TMCustomerPrice,Excursion,ENetPrice,ECustomerPrice,DomesticTransportMethod,DTMNetPrice,DTMCustomerPrice,Accommodation,ANetPrice,ACustomerPrice")] BillDomestic billDomestic)
        {
            var UserId = _userManager.GetUserId(User);
            var CompanyId = (await _userManager.Users.SingleOrDefaultAsync(a => a.Id == UserId)).CompanyId;

            if (billDomestic.ANetPrice == null || billDomestic.DTMNetPrice == null || billDomestic.ENetPrice == null || billDomestic.TMNetPrice == null)
            {
                ModelState.AddModelError("الاسم مستخدم", "من فضلك قم بملئ سعر النت لوسيله السفر و الرحلات الداخليه و النقل و وسيله السفر الداخليه بشكل سليم");
            }

            if (billDomestic.ACustomerPrice == null || billDomestic.DTMCustomerPrice == null || billDomestic.ECustomerPrice == null || billDomestic.TMCustomerPrice == null)
            {
                ModelState.AddModelError("الاسم مستخدم", "من فضلك قم بملئ سعر البيع لوسيله السفر و الرحلات الداخليه و النقل و وسيله السفر الداخليه بشكل سليم");
            }

            if (billDomestic.Accommodation == null || billDomestic.DomesticTransportMethod == null || billDomestic.Excursion == null || billDomestic.TransportMethodId == null)
            {
                ModelState.AddModelError("الاسم مستخدم", "من فضلك قم بملئ وسيله السفر و الرحلات الداخليه و النقل و وسيله السفر الداخليه بشكل سليم");
            }


            if (ModelState.IsValid)
            {
                billDomestic.NetPrice = Convert.ToInt32(billDomestic.ANetPrice + billDomestic.DTMNetPrice + billDomestic.ENetPrice + billDomestic.TMNetPrice);
                billDomestic.CustomerPrice = Convert.ToInt32(billDomestic.ACustomerPrice + billDomestic.DTMCustomerPrice + billDomestic.ECustomerPrice + billDomestic.TMCustomerPrice);
                
                billDomestic.UserId = UserId;
                var CompanyCuntry = (await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyId)).DateTimeCountry;
                var info = TimeZoneInfo.FindSystemTimeZoneById(CompanyCuntry);
                DateTimeOffset localServerTime = DateTimeOffset.Now;
                DateTimeOffset localTime = TimeZoneInfo.ConvertTime(localServerTime, info);
                billDomestic.BillDate = localTime.DateTime;
                billDomestic.MenuLE0Id = (await _context.MenuLE0.SingleOrDefaultAsync(a => a.M0_Name == "سياحة داخلية")).Id;
                billDomestic.EMPCommission = (billDomestic.CustomerPrice - billDomestic.NetPrice) * ((_context.UsersDetails.Where(a => a.UserId == billDomestic.UserId).Count()) != 0 ? _context.UsersDetails.SingleOrDefault(a => a.UserId == billDomestic.UserId).Commission : 0);
                billDomestic.CompanyID = CompanyId;
                billDomestic.IndividualStatus = true;
                if (await _context.BillDomestic.Where(a => a.CompanyID == billDomestic.CompanyID).CountAsync() == 0)
                {
                    billDomestic.BillId = 1;
                    TempData["BillId"] = 1;
                }
                else
                {
                    billDomestic.BillId = await _context.BillDomestic.Where(a => a.CompanyID == billDomestic.CompanyID).MaxAsync(a => a.BillId) + 1;
                    TempData["BillId"] = billDomestic.BillId;

                }
                _context.Add(billDomestic);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", new RouteValueDictionary(
                        new { controller = "BillDomestics", action = "Details", Id = billDomestic.Id }));

            }
            var customersSuppliers = await (from user in _context.CustomersSuppliers.Where(a => a.CompanyId == CompanyId)
                                                //where !_context.NonSuppliers.Any(f => f.CustomerSupplierId == user.Id)
                                            select new
                                            {
                                                user.Id,
                                                user.Name,
                                                user.CustomerOrSupplier,
                                                user.CustomerOrSupplierId
                                            }).ToListAsync();

            ViewData["CustomerOrSupplierId"] = new SelectList(_context.CustomerOrSuppliers, "Id", "Name", billDomestic.CustomerOrSupplierId);
            ViewData["CustomerSupplierId"] = new SelectList(customersSuppliers.Where(a => a.CustomerOrSupplierId == billDomestic.CustomerOrSupplierId).OrderBy(a => a.Name), "Id", "Name", billDomestic.CustomerSupplierId);
            ViewData["MenuLE0Id"] = new SelectList(_context.MenuLE0, "Id", "M0_Name", billDomestic.MenuLE0Id);
            ViewData["MenuLE1Id"] = new SelectList(_context.MenuLE1, "Id", "M1_Name", billDomestic.MenuLE1Id);
            ViewData["MenuLE2Id"] = new SelectList(_context.MenuLE2, "Id", "M2_Name", billDomestic.MenuLE2Id);
            ViewData["TransportMethodId"] = new SelectList(_context.TransportMethods, "Id", "Name",billDomestic.TransportMethodId);
            ViewData["TicketExportId"] = new SelectList(customersSuppliers.Where(a => a.CustomerOrSupplier.Name == "مورد او وكيل").OrderBy(a => a.Name), "Id", "Name", billDomestic.TicketExportId);
            return View(billDomestic);
        }


        [Authorize(Roles = CustomRoles.Admin + "," + CustomRoles.BranchManager + "," + CustomRoles.Edits)]
        // GET: BillDomestics/Edit/5   **** تعديل فاتوره سياحه داخليه ****
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var billDomestic = await _context.BillDomestic.SingleOrDefaultAsync(m => m.Id == id);
            if (billDomestic == null)
            {
                return NotFound();
            }
            var CompanyId =(await _userManager.Users.SingleOrDefaultAsync(a => a.Id == billDomestic.UserId)).CompanyId;
            var customersSuppliers =await (from user in _context.CustomersSuppliers.Where(a => a.CompanyId == CompanyId)
                                      //where !_context.NonSuppliers.Any(f => f.CustomerSupplierId == user.Id)
                                      select new
                                      {
                                          user.Id,
                                          user.Name,
                                          user.CustomerOrSupplier,
                                          user.CustomerOrSupplierId
                                      }).ToListAsync();

            ViewData["CustomerOrSupplierId"] = new SelectList(_context.CustomerOrSuppliers.Where(a => a.Name != "مندوب"), "Id", "Name", billDomestic.CustomerOrSupplierId);
            ViewData["CustomerSupplierId"] = new SelectList(customersSuppliers.Where(a => a.CustomerOrSupplierId == billDomestic.CustomerOrSupplierId).OrderBy(a => a.Name), "Id", "Name", billDomestic.CustomerSupplierId);
            ViewData["MenuLE0Id"] = new SelectList(_context.MenuLE0, "Id", "M0_Name", billDomestic.MenuLE0Id);
            ViewData["MenuLE1Id"] = new SelectList(_context.MenuLE1.Where(a => a.MenuLE0Id == billDomestic.MenuLE0Id), "Id", "M1_Name", billDomestic.MenuLE1Id);
            ViewData["MenuLE2Id"] = new SelectList(_context.MenuLE2.Where(a => a.MenuLE1Id == billDomestic.MenuLE1Id), "Id", "M2_Name", billDomestic.MenuLE2Id);
            ViewData["TicketExportId"] = new SelectList(customersSuppliers.Where(a => a.CustomerOrSupplier.Name == "مورد او وكيل").OrderBy(a => a.Name), "Id", "Name", billDomestic.TicketExportId);
            ViewData["UserId"] = new SelectList(_context.Users.Where(a => a.CompanyId == CompanyId), "Id", "UserName", billDomestic.UserId);
            return View(billDomestic);
        }



        [Authorize(Roles = CustomRoles.Admin + "," + CustomRoles.BranchManager + "," + CustomRoles.Edits)]
        // POST: BillDomestics/Edit/5   **** تعديل فاتوره سياحه داخليه ****
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BillId,BillDate,CustomerOrSupplierId,CustomerSupplierId,MenuLE0Id,MenuLE1Id,MenuLE2Id,TicketExportId,AdultN,ChildN,CustomerPrice,NetPrice,TicketFrom,TicketTo,Commnets,UserId,CompanyID")] BillDomestic billDomestic)
        {
            if (id != billDomestic.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    billDomestic.EMPCommission = (billDomestic.CustomerPrice - billDomestic.NetPrice) * ((_context.UsersDetails.Where(a => a.UserId == billDomestic.UserId).Count()) != 0 ?(await _context.UsersDetails.SingleOrDefaultAsync(a => a.UserId == billDomestic.UserId)).Commission : 0);
                    _context.Update(billDomestic);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BillDomesticExists(billDomestic.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", new RouteValueDictionary(
                        new { controller = "BillDomestics", action = "Details", Id = billDomestic.Id }));
            }
            var CompanyId =(await _userManager.Users.SingleOrDefaultAsync(a => a.Id == billDomestic.UserId)).CompanyId;
            var customersSuppliers =await (from user in _context.CustomersSuppliers.Where(a => a.CompanyId == CompanyId)
                                      where !_context.NonSuppliers.Any(f => f.CustomerSupplierId == user.Id)
                                      select new
                                      {
                                          user.Id,
                                          user.Name,
                                          user.CustomerOrSupplier,
                                          user.CustomerOrSupplierId
                                      }).ToListAsync();

            ViewData["CustomerOrSupplierId"] = new SelectList(_context.CustomerOrSuppliers, "Id", "Name", billDomestic.CustomerOrSupplierId);
            ViewData["CustomerSupplierId"] = new SelectList(customersSuppliers.Where(a => a.CustomerOrSupplierId == billDomestic.CustomerOrSupplierId).OrderBy(a => a.Name), "Id", "Name", billDomestic.CustomerSupplierId);
            ViewData["MenuLE0Id"] = new SelectList(_context.MenuLE0, "Id", "M0_Name", billDomestic.MenuLE0Id);
            ViewData["MenuLE1Id"] = new SelectList(_context.MenuLE1, "Id", "M1_Name", billDomestic.MenuLE1Id);
            ViewData["MenuLE2Id"] = new SelectList(_context.MenuLE2, "Id", "M2_Name", billDomestic.MenuLE2Id);
            ViewData["TicketExportId"] = new SelectList(customersSuppliers.Where(a => a.CustomerOrSupplier.Name == "مورد او وكيل").OrderBy(a => a.Name), "Id", "Name", billDomestic.TicketExportId);
            ViewData["UserId"] = new SelectList(_context.Users.Where(a => a.CompanyId == CompanyId), "Id", "UserName", billDomestic.UserId);
            return View(billDomestic);
        }



        [Authorize(Roles = CustomRoles.Admin + "," + CustomRoles.BranchManager + "," + CustomRoles.EsalDomestics)]
        // GET: BillDomestics/CreateEsal/5   **** إنشاء ايصال لفاتوره السياحه الداخليه  ****
        public async Task<IActionResult> CreateEsal(int? Id)
        {
            var bill =await _context.BillDomestic.SingleOrDefaultAsync(a => a.Id == Id);

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
        [Authorize(Roles = CustomRoles.Admin + "," + CustomRoles.BranchManager + "," + CustomRoles.EsalDomestics)]
        // POST: BillDomestics/CreateEsal/5   **** إنشاء ايصال لفاتوره السياحه الداخليه  ****
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
                if (_context.Esals.Where(a => a.CompanyID == _context.Users.SingleOrDefault(b => b.Id == _userManager.GetUserId(User)).CompanyId).Count() == 0)
                {
                    esal.EsalId = 1;
                }
                else
                {
                    esal.EsalId = _context.Esals.Where(a => a.CompanyID == _context.Users.SingleOrDefault(b => b.Id == _userManager.GetUserId(User)).CompanyId).Max(a => a.EsalId) + 1;
                }
                esal.CompanyID = CompanyId;
                _context.Add(esal);
                await _context.SaveChangesAsync();
                return RedirectToAction("Print", new RouteValueDictionary(
                new { controller = "Esals", action = "Print", Id = esal.Id }));
            }
            var bill =await _context.BillDomestic.SingleOrDefaultAsync(a => a.Id == esal.BillId);

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
        public async Task <IActionResult> CreateEzn(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }
            var bill = await _context.BillDomestic.SingleOrDefaultAsync(a => a.Id == Id);

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
                    ezn.EznId =await _context.Ezns.Where(a => a.CompanyID == ezn.CompanyID).MaxAsync(a => a.EznId) + 1;

                }
                _context.Add(ezn);
                await _context.SaveChangesAsync();
                return RedirectToAction("PrintEzn", new RouteValueDictionary(
                new { controller = "Ezns", action = "PrintEzn", Id = ezn.Id }));
            }
            var bill = await _context.BillDomestic.SingleOrDefaultAsync(a => a.Id == ezn.BillId);

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




        [Authorize(Roles = CustomRoles.StatementDate)]
        public async Task<ActionResult> BillReport(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }
            var CompanyId = (await _userManager.Users.SingleOrDefaultAsync(a => a.Id == _userManager.GetUserId(User))).CompanyId;
            var Domestics = (await _context.MenuLE0.SingleOrDefaultAsync(a => a.M0_Name == "سياحة داخلية")).Id;
            var bill = await _context.BillDomestic.Include(a=>a.TransportMethod).SingleOrDefaultAsync(x => x.Id == Id);
          
            var ezns = await (from x in _context.Ezns.Where(A => A.CompanyID == CompanyId).Where(a=>a.MenuLE0Id == Domestics).Where(x => x.BillId == Id) select new Models.ViewModel.Transactions { EId = x.EznId, EDate = x.EznDate, Statement = string.Concat(x.Name, x.CustomerOrSupplier.Name, " - " + x.CustomerSupplier.Name, " - " + x.ExpenseName, " - " + x.MenuLE0.M0_Name, " - " + x.MenuLE1.M1_Name, " - " + x.MenuLE2.M2_Name, " - "  + x.PaymentMethod.Name, " - " + x.User.UserName), Credit = x.AmountWithdrawn, Type = x.PaymentMethod.Name }).ToListAsync();
            var esals = await (from x in _context.Esals.Where(A => A.CompanyID == CompanyId).Where(a => a.MenuLE0Id == Domestics).Where(x => x.BillId == Id) select new Models.ViewModel.Transactions { EId = x.EsalId, EDate = x.EsalDate, Statement = string.Concat(x.CustomerOrSupplier.Name, " - " + x.CustomerSupplier.Name, " - " + x.MenuLE0.M0_Name, " - " + x.MenuLE1.M1_Name, " - " + x.MenuLE2.M2_Name, " - " + x.DepositDesc, " - " + x.PaymentMethod.Name, " - " + x.User.UserName), Debit = x.AmountPaid, Type = x.PaymentMethod.Name }).ToListAsync();

            var All = (ezns.Concat(esals)).OrderBy(a => a.EDate);

            if (All == null)
            {
                return NotFound();
            }
            ViewData["Bill"] = bill;
            ViewData["HeaderName"] =string.Concat((await _context.CustomersSuppliers.SingleOrDefaultAsync(x => x.Id == bill.CustomerSupplierId)).Name, " - " + (await _context.MenuLE0.SingleOrDefaultAsync(x => x.Id == bill.MenuLE0Id)).M0_Name , " - " + (await _context.MenuLE1.SingleOrDefaultAsync(x => x.Id == bill.MenuLE1Id)).M1_Name , " - " + (await _context.MenuLE2.SingleOrDefaultAsync(x => x.Id == bill.MenuLE2Id)).M2_Name);
            ViewData["CompanyId"] = CompanyId;
            ViewData["CompanyLogo"] = "~/" + TravelERP.Properties.Resources.ImgFolder + "/" + _context.Companies.SingleOrDefault(a => a.Id == CompanyId).CompanyLogo;
            ViewData["CompanyA4EsalImage"] = "~/" + TravelERP.Properties.Resources.ImgFolder + "/" + _context.Companies.SingleOrDefault(a => a.Id == CompanyId).CompanyA4EsalImage;
            ViewData["CompanyNameE"] = (await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyId)).Company_NameE;
            ViewData["CompanyName"] = (await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyId)).Company_Name;


            return View(All);
        }


        public async Task<ActionResult> ReservationForm(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }
            var CompanyId = (await _userManager.Users.SingleOrDefaultAsync(a => a.Id == _userManager.GetUserId(User))).CompanyId;
            var Domestics = (await _context.MenuLE0.SingleOrDefaultAsync(a => a.M0_Name == "سياحة داخلية")).Id;
            var bill = await _context.BillDomestic.Include(a => a.TransportMethod).Include(a=>a.CustomerSupplier).Include(a=>a.MenuLE0).Include(a=>a.MenuLE1).Include(a=>a.MenuLE2).SingleOrDefaultAsync(x => x.Id == Id);

            var esals = await (from x in _context.Esals.Where(A => A.CompanyID == CompanyId).Where(a => a.MenuLE0Id == Domestics).Where(x => x.BillId == Id) select new Models.ViewModel.Transactions { EId = x.EsalId, EDate = x.EsalDate, Debit = x.AmountPaid}).ToListAsync();

            var All = esals.OrderBy(a => a.EDate);

            if (All == null)
            {
                return NotFound();
            }
            ViewData["Bill"] = bill;
            ViewData["HeaderName"] = string.Concat((await _context.CustomersSuppliers.SingleOrDefaultAsync(x => x.Id == bill.CustomerSupplierId)).Name, " - " + (await _context.MenuLE0.SingleOrDefaultAsync(x => x.Id == bill.MenuLE0Id)).M0_Name, " - " + (await _context.MenuLE1.SingleOrDefaultAsync(x => x.Id == bill.MenuLE1Id)).M1_Name, " - " + (await _context.MenuLE2.SingleOrDefaultAsync(x => x.Id == bill.MenuLE2Id)).M2_Name);
            ViewData["CompanyId"] = CompanyId;
            ViewData["CompanyLogo"] = "~/" + TravelERP.Properties.Resources.ImgFolder + "/" + _context.Companies.SingleOrDefault(a => a.Id == CompanyId).CompanyLogo;
            ViewData["CompanyA4EsalImage"] = "~/" + TravelERP.Properties.Resources.ImgFolder + "/" + _context.Companies.SingleOrDefault(a => a.Id == CompanyId).CompanyA4EsalImage;
            ViewData["CompanyNameE"] = (await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyId)).Company_NameE;
            ViewData["CompanyName"] = (await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyId)).Company_Name;
            ViewData["CompanyAddress"] = (await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyId)).Company_Address;
            ViewData["CompanyPhones"] = (await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyId)).Company_PhonesNumber ;

            return View(All);
        }

        private bool BillDomesticExists(int id)
        {
            return _context.BillDomestic.Any(e => e.Id == id);
        }


    }
}
