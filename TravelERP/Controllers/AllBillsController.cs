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
    [Authorize]

    public class AllBillsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AllBillsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////// AllBills/BillsAllDate **** المبيعات - مبيعات لجميع الموظفين - مبيعات شامل **** /////////////
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

                //_context.Add(datesSearch);
                await _context.SaveChangesAsync();
                return RedirectToAction("BillsAll", "AllBills");
            }
            return View(datesSearch);
        }
        public async Task<ActionResult> BillsAll()
        {
            var UserId = _userManager.GetUserId(User);
            var CompanyId =(await _userManager.Users.SingleOrDefaultAsync(a => a.Id == UserId)).CompanyId;

            var billAirLine =await (from x in _context.BillAirLines
                               .Where(A => A.CompanyID == CompanyId)
                               .Where(d => d.BillDate >= Convert.ToDateTime(TempData["StartDate"]))
                               .Where(d => d.BillDate.Date <= Convert.ToDateTime(TempData["EndDate"]))
                               select new Models.ViewModel.AllBillsViewModel { BillId = x.BillId, BillType = "طيران", BillDate = x.BillDate, CustomerOrSupplierName = x.CustomerOrSupplier.Name, CustomerSupplierName = x.CustomerSupplier.Name, MenuLE0Name = x.MenuLE0.M0_Name, MenuLE1Name = x.MenuLE1.M1_Name, MenuLE2Name = x.MenuLE2.M2_Name, Direction = x.Direction, PNR = x.PNR, TicketExportName = x.TicketExport.Name, AdultN = x.AdultN, ChildN = x.ChildN, CustomerPrice = x.CustomerPrice, NetPrice = x.NetPrice, Commission = x.EMPCommission, TicketFrom = x.TicketFrom, TicketTo = x.TicketTo, Commnets = x.Commnets, UserName = x.User.UserName, CompanyID = x.CompanyID ,BillState = true}).ToListAsync();

            var billDomestic =await (from x in _context.BillDomestic
                   .Where(A => A.CompanyID == CompanyId)
                   .Where(d => d.BillDate >= Convert.ToDateTime(TempData["StartDate"]))
                   .Where(d => d.BillDate.Date <= Convert.ToDateTime(TempData["EndDate"]))
                                select new Models.ViewModel.AllBillsViewModel { BillId = x.BillId, BillType = "داخلى", BillDate = x.BillDate, CustomerOrSupplierName = x.CustomerOrSupplier.Name, CustomerSupplierName = x.CustomerSupplier.Name, MenuLE0Name = x.MenuLE0.M0_Name, MenuLE1Name = x.MenuLE1.M1_Name, MenuLE2Name = x.MenuLE2.M2_Name, TicketExportName = x.TicketExport.Name, AdultN = x.AdultN, ChildN = x.ChildN, CustomerPrice = x.CustomerPrice, NetPrice = x.NetPrice, Commission = x.EMPCommission, TicketFrom = x.TicketFrom, TicketTo = x.TicketTo, Commnets = x.Commnets, UserName = x.User.UserName, CompanyID = x.CompanyID, BillState = true }).ToListAsync();
           
            var billForeign = await (from x in _context.BillForeigns
                    .Where(A => A.CompanyID == CompanyId)
                    .Where(d => d.BillDate >= Convert.ToDateTime(TempData["StartDate"]))
                    .Where(d => d.BillDate.Date <= Convert.ToDateTime(TempData["EndDate"]))
                                      select new Models.ViewModel.AllBillsViewModel { BillId = x.BillId, BillType = "خارجى", BillDate = x.BillDate, CustomerOrSupplierName = x.CustomerOrSupplier.Name, CustomerSupplierName = x.CustomerSupplier.Name, MenuLE0Name = x.MenuLE0.M0_Name, MenuLE1Name = x.MenuLE1.M1_Name, MenuLE2Name = x.MenuLE2.M2_Name, TicketExportName = x.TicketExport.Name, AdultN = x.AdultN, ChildN = x.ChildN, CustomerPrice = x.CustomerPrice, NetPrice = x.NetPrice, Commission = x.EMPCommission, TicketFrom = x.TicketFrom, TicketTo = x.TicketTo, Commnets = x.Commnets, UserName = x.User.UserName, CompanyID = x.CompanyID, BillState = true }).ToListAsync();

            var billVisas =await (from x in _context.BillVisas
                   .Where(A => A.CompanyID == CompanyId)
                   .Where(d => d.ApprovedDate >= Convert.ToDateTime(TempData["StartDate"]))
                   .Where(d => d.ApprovedDate.Date <= Convert.ToDateTime(TempData["EndDate"]))
                             select new Models.ViewModel.AllBillsViewModel { BillId = x.BillId, BillType = "تأشيرات", BillDate = x.ApprovedDate, CustomerOrSupplierName = x.CustomerOrSupplier.Name, CustomerSupplierName = x.CustomerSupplier.Name, MenuLE0Name = x.MenuLE0.M0_Name, MenuLE1Name = x.MenuLE1.M1_Name, MenuLE2Name = x.MenuLE2.M2_Name, TicketExportName = x.TicketExport.Name, AdultN = x.AdultN, ChildN = x.ChildN, CustomerPrice = x.CustomerPrice, NetPrice = x.NetPrice, Commission = x.EMPCommission, Commnets = x.Commnets +" - " + x.PassportNo, UserName = ((x.BillState == true) ? "Approved" : "Pending") + " - " + x.User.UserName, CompanyID = x.CompanyID, BillState = x.BillState }).ToListAsync();
            ViewData["billAirLine"] = billAirLine.Sum(a => a.CustomerPrice - a.NetPrice - a.Commission);
            ViewData["billDomestic"] = billDomestic.Sum(a => a.CustomerPrice - a.NetPrice - a.Commission);
            ViewData["billVisas"] = billVisas.Sum(a => a.CustomerPrice - a.NetPrice - a.Commission);
            ViewData["billForeigns"] = billForeign.Sum(a => a.CustomerPrice - a.NetPrice - a.Commission);

            ViewData["CompanyLogo"] = "~/" + TravelERP.Properties.Resources.ImgFolder + "/" + _context.Companies.SingleOrDefault(a => a.Id == CompanyId).CompanyLogo;
            ViewData["CompanyA4EsalImage"] = "~/" + TravelERP.Properties.Resources.ImgFolder + "/" + _context.Companies.SingleOrDefault(a => a.Id == CompanyId).CompanyA4EsalImage;
            ViewData["CompanyNameE"] =(await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyId)).Company_NameE;
            ViewData["CompanyName"] =(await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyId)).Company_Name;
            ViewData["CompanyId"] = CompanyId;

            var Outbut = (billAirLine.Concat(billVisas).Concat(billDomestic).Concat(billForeign).OrderBy(a => a.BillDate));
            return View(Outbut);
        }


        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////// AllBills/BillsAllByUserDate **** المبيعات - مبيعات لجميع الموظفين لكل موظف- مبيعات شامل **** /////////////
        [Authorize(Roles = CustomRoles.Admin + "," + CustomRoles.BranchManager)]
        public IActionResult BillsAllByUserDate()
        {
            ViewData["UsersId"] = new SelectList(_context.Users.Where(a=>a.UserName != "Developer"), "Id", "UserName");

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BillsAllByUserDate([Bind("Name7,StartDate,EndDate")] DatesSearch datesSearch)
        {
            if (datesSearch.Name7 == null)
            {
                ModelState.AddModelError("Name7", "من فضلك اختر اسم الموظف");
            }

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
                TempData["Name7"] = datesSearch.Name7.ToString();

                //_context.Add(datesSearch);
                await _context.SaveChangesAsync();
                return RedirectToAction("BillsByUserAll", "AllBills");
            }
            ViewData["UsersId"] = new SelectList(_context.Users.Where(a => a.UserName != "Developer"), "Id", "UserName",datesSearch.Name7);

            return View(datesSearch);
        }

        public async Task<ActionResult> BillsByUserAll()
        {
            var UserId = _userManager.GetUserId(User);
            var CompanyId = (await _userManager.Users.SingleOrDefaultAsync(a => a.Id == UserId)).CompanyId;
            var SelectedUser = TempData["Name7"].ToString();

            var billAirLine = await (from x in _context.BillAirLines
                                .Where(A => A.CompanyID == CompanyId)
                                .Where(x=>x.UserId == SelectedUser)
                                .Where(d => d.BillDate >= Convert.ToDateTime(TempData["StartDate"]))
                                .Where(d => d.BillDate.Date <= Convert.ToDateTime(TempData["EndDate"]))
                                     select new Models.ViewModel.AllBillsViewModel { BillId = x.BillId, BillType = "طيران", BillDate = x.BillDate, CustomerOrSupplierName = x.CustomerOrSupplier.Name, CustomerSupplierName = x.CustomerSupplier.Name, MenuLE0Name = x.MenuLE0.M0_Name, MenuLE1Name = x.MenuLE1.M1_Name, MenuLE2Name = x.MenuLE2.M2_Name, Direction = x.Direction, PNR = x.PNR, TicketExportName = x.TicketExport.Name, AdultN = x.AdultN, ChildN = x.ChildN, CustomerPrice = x.CustomerPrice, NetPrice = x.NetPrice, Commission = x.EMPCommission, TicketFrom = x.TicketFrom, TicketTo = x.TicketTo, Commnets = x.Commnets, UserName = x.User.UserName, CompanyID = x.CompanyID, BillState = true }).ToListAsync();

            var billDomestic = await (from x in _context.BillDomestic
                    .Where(A => A.CompanyID == CompanyId)
                    .Where(x => x.UserId == SelectedUser)
                    .Where(d => d.BillDate >= Convert.ToDateTime(TempData["StartDate"]))
                    .Where(d => d.BillDate.Date <= Convert.ToDateTime(TempData["EndDate"]))
                                      select new Models.ViewModel.AllBillsViewModel { BillId = x.BillId, BillType = "داخلى", BillDate = x.BillDate, CustomerOrSupplierName = x.CustomerOrSupplier.Name, CustomerSupplierName = x.CustomerSupplier.Name, MenuLE0Name = x.MenuLE0.M0_Name, MenuLE1Name = x.MenuLE1.M1_Name, MenuLE2Name = x.MenuLE2.M2_Name, TicketExportName = x.TicketExport.Name, AdultN = x.AdultN, ChildN = x.ChildN, CustomerPrice = x.CustomerPrice, NetPrice = x.NetPrice, Commission = x.EMPCommission, TicketFrom = x.TicketFrom, TicketTo = x.TicketTo, Commnets = x.Commnets, UserName = x.User.UserName, CompanyID = x.CompanyID, BillState = true }).ToListAsync();

            var billForeign = await (from x in _context.BillForeigns
                    .Where(A => A.CompanyID == CompanyId)
                    .Where(x => x.UserId == SelectedUser)
                    .Where(d => d.BillDate >= Convert.ToDateTime(TempData["StartDate"]))
                    .Where(d => d.BillDate.Date <= Convert.ToDateTime(TempData["EndDate"]))
                                     select new Models.ViewModel.AllBillsViewModel { BillId = x.BillId, BillType = "خارجى", BillDate = x.BillDate, CustomerOrSupplierName = x.CustomerOrSupplier.Name, CustomerSupplierName = x.CustomerSupplier.Name, MenuLE0Name = x.MenuLE0.M0_Name, MenuLE1Name = x.MenuLE1.M1_Name, MenuLE2Name = x.MenuLE2.M2_Name, TicketExportName = x.TicketExport.Name, AdultN = x.AdultN, ChildN = x.ChildN, CustomerPrice = x.CustomerPrice, NetPrice = x.NetPrice, Commission = x.EMPCommission, TicketFrom = x.TicketFrom, TicketTo = x.TicketTo, Commnets = x.Commnets, UserName = x.User.UserName, CompanyID = x.CompanyID, BillState = true }).ToListAsync();

            var billVisas = await (from x in _context.BillVisas
                    .Where(A => A.CompanyID == CompanyId)
                    .Where(x => x.UserId == SelectedUser)
                    .Where(d => d.ApprovedDate >= Convert.ToDateTime(TempData["StartDate"]))
                    .Where(d => d.ApprovedDate.Date <= Convert.ToDateTime(TempData["EndDate"]))
                                   select new Models.ViewModel.AllBillsViewModel { BillId = x.BillId, BillType = "تأشيرات", BillDate = x.ApprovedDate, CustomerOrSupplierName = x.CustomerOrSupplier.Name, CustomerSupplierName = x.CustomerSupplier.Name, MenuLE0Name = x.MenuLE0.M0_Name, MenuLE1Name = x.MenuLE1.M1_Name, MenuLE2Name = x.MenuLE2.M2_Name, TicketExportName = x.TicketExport.Name, AdultN = x.AdultN, ChildN = x.ChildN, CustomerPrice = x.CustomerPrice, NetPrice = x.NetPrice, Commission = x.EMPCommission, Commnets = x.Commnets + " - " + x.PassportNo, UserName = ((x.BillState == true) ? "Approved" : "Pending") + " - " + x.User.UserName, CompanyID = x.CompanyID, BillState = x.BillState }).ToListAsync();
            ViewData["billAirLine"] = billAirLine.Sum(a => a.CustomerPrice - a.NetPrice - a.Commission);
            ViewData["billDomestic"] = billDomestic.Sum(a => a.CustomerPrice - a.NetPrice - a.Commission);
            ViewData["billVisas"] = billVisas.Sum(a => a.CustomerPrice - a.NetPrice - a.Commission);
            ViewData["billForeigns"] = billForeign.Sum(a => a.CustomerPrice - a.NetPrice - a.Commission);
            ViewData["UserName"] = (await _context.Users.SingleOrDefaultAsync(u => u.Id == SelectedUser)).UserName;
            ViewData["CompanyLogo"] = "~/" + TravelERP.Properties.Resources.ImgFolder + "/" + _context.Companies.SingleOrDefault(a => a.Id == CompanyId).CompanyLogo;
            ViewData["CompanyA4EsalImage"] = "~/" + TravelERP.Properties.Resources.ImgFolder + "/" + _context.Companies.SingleOrDefault(a => a.Id == CompanyId).CompanyA4EsalImage;
            ViewData["CompanyNameE"] = (await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyId)).Company_NameE;
            ViewData["CompanyName"] = (await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyId)).Company_Name;
            ViewData["CompanyId"] = CompanyId;

            var Outbut = (billAirLine.Concat(billVisas).Concat(billDomestic).Concat(billForeign).OrderBy(a => a.BillDate));
            return View(Outbut);
        }

        //////////////////////////////////////////////////////////////////////////////////////
        /// AllBills/UserBillsDate **** المبيعات - مبيعات الموظف - مبيعات شامل **** //////
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
                return RedirectToAction("UserBills", "AllBills");
            }
            return View(datesSearch);
        }
        public async Task<ActionResult> UserBills()
        {
            var UserId = _userManager.GetUserId(User);
            var CompanyId =(await _userManager.Users.SingleOrDefaultAsync(a => a.Id == UserId)).CompanyId;

            var billAirLine =await (from x in _context.BillAirLines
                               .Where(d => d.UserId == UserId)
                               .Where(A => A.CompanyID == CompanyId)
                               .Where(d => d.BillDate >= Convert.ToDateTime(TempData["StartDate"]))
                               .Where(d => d.BillDate.Date <= Convert.ToDateTime(TempData["EndDate"]))
                               select new Models.ViewModel.AllBillsViewModel { BillId = x.BillId, BillType = "طيران", BillDate = x.BillDate, CustomerOrSupplierName = x.CustomerOrSupplier.Name, CustomerSupplierName = x.CustomerSupplier.Name, MenuLE0Name = x.MenuLE0.M0_Name, MenuLE1Name = x.MenuLE1.M1_Name, MenuLE2Name = x.MenuLE2.M2_Name, Direction = x.Direction, PNR = x.PNR, TicketExportName = x.TicketExport.Name, AdultN = x.AdultN, ChildN = x.ChildN, CustomerPrice = x.CustomerPrice, NetPrice = x.NetPrice, Commission = x.EMPCommission, TicketFrom = x.TicketFrom, TicketTo = x.TicketTo, Commnets = x.Commnets, UserName = x.User.UserName, CompanyID = x.CompanyID,BillState = true }).ToListAsync();

            var billDomestic =await (from x in _context.BillDomestic
                   .Where(d => d.UserId == UserId)
                   .Where(A => A.CompanyID == CompanyId)
                   .Where(d => d.BillDate >= Convert.ToDateTime(TempData["StartDate"]))
                   .Where(d => d.BillDate.Date <= Convert.ToDateTime(TempData["EndDate"]))
                                select new Models.ViewModel.AllBillsViewModel { BillId = x.BillId, BillType = "داخلى", BillDate = x.BillDate, CustomerOrSupplierName = x.CustomerOrSupplier.Name, CustomerSupplierName = x.CustomerSupplier.Name, MenuLE0Name = x.MenuLE0.M0_Name, MenuLE1Name = x.MenuLE1.M1_Name, MenuLE2Name = x.MenuLE2.M2_Name, TicketExportName = x.TicketExport.Name, AdultN = x.AdultN, ChildN = x.ChildN, CustomerPrice = x.CustomerPrice, NetPrice = x.NetPrice, Commission = x.EMPCommission, TicketFrom = x.TicketFrom, TicketTo = x.TicketTo, Commnets = x.Commnets, UserName = x.User.UserName, CompanyID = x.CompanyID, BillState = true }).ToListAsync();
           
            var billForeign = await (from x in _context.BillForeigns
                    .Where(d => d.UserId == UserId)
                    .Where(A => A.CompanyID == CompanyId)
                    .Where(d => d.BillDate >= Convert.ToDateTime(TempData["StartDate"]))
                    .Where(d => d.BillDate.Date <= Convert.ToDateTime(TempData["EndDate"]))
                                      select new Models.ViewModel.AllBillsViewModel { BillId = x.BillId, BillType = "خارجى", BillDate = x.BillDate, CustomerOrSupplierName = x.CustomerOrSupplier.Name, CustomerSupplierName = x.CustomerSupplier.Name, MenuLE0Name = x.MenuLE0.M0_Name, MenuLE1Name = x.MenuLE1.M1_Name, MenuLE2Name = x.MenuLE2.M2_Name, TicketExportName = x.TicketExport.Name, AdultN = x.AdultN, ChildN = x.ChildN, CustomerPrice = x.CustomerPrice, NetPrice = x.NetPrice, Commission = x.EMPCommission, TicketFrom = x.TicketFrom, TicketTo = x.TicketTo, Commnets = x.Commnets, UserName = x.User.UserName, CompanyID = x.CompanyID, BillState = true }).ToListAsync();

            var billVisas =await (from x in _context.BillVisas
                   .Where(d => d.UserId == UserId)
                   .Where(A => A.CompanyID == CompanyId)
                   .Where(d => d.ApprovedDate >= Convert.ToDateTime(TempData["StartDate"]))
                   .Where(d => d.ApprovedDate.Date <= Convert.ToDateTime(TempData["EndDate"]))
                             select new Models.ViewModel.AllBillsViewModel { BillId = x.BillId, BillType = "تأشيرات", BillDate = x.ApprovedDate, CustomerOrSupplierName = x.CustomerOrSupplier.Name, CustomerSupplierName = x.CustomerSupplier.Name, MenuLE0Name = x.MenuLE0.M0_Name, MenuLE1Name = x.MenuLE1.M1_Name, MenuLE2Name = x.MenuLE2.M2_Name, TicketExportName = x.TicketExport.Name, AdultN = x.AdultN, ChildN = x.ChildN, CustomerPrice = x.CustomerPrice, NetPrice = x.NetPrice, Commission = x.EMPCommission, Commnets = x.Commnets + " - " + x.PassportNo, UserName = x.User.UserName, CompanyID = x.CompanyID, BillState = x.BillState }).ToListAsync();

            ViewData["billAirLine"] = billAirLine.Sum(a => a.CustomerPrice - a.NetPrice - a.Commission);
            ViewData["billDomestic"] = billDomestic.Sum(a => a.CustomerPrice - a.NetPrice - a.Commission);
            ViewData["billVisas"] = billVisas.Sum(a => a.CustomerPrice - a.NetPrice - a.Commission);
            ViewData["billForeign"] = billForeign.Sum(a => a.CustomerPrice - a.NetPrice - a.Commission);

            ViewData["CompanyLogo"] = "~/" + TravelERP.Properties.Resources.ImgFolder + "/" + _context.Companies.SingleOrDefault(a => a.Id == CompanyId).CompanyLogo;
            ViewData["CompanyA4EsalImage"] = "~/" + TravelERP.Properties.Resources.ImgFolder + "/" + _context.Companies.SingleOrDefault(a => a.Id == CompanyId).CompanyA4EsalImage;
            ViewData["CompanyNameE"] =(await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyId)).Company_NameE;
            ViewData["CompanyName"] =(await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyId)).Company_Name;
            ViewData["CompanyId"] = CompanyId;
            var Outbut = (billAirLine.Concat(billVisas).Concat(billDomestic).Concat(billForeign).OrderBy(a => a.BillDate));
            return View(Outbut);
        }



        //////////////////////////////////////////////////////////////////////////////////////
        //////////// AllBills/CommissionDate **** العمولات - عمولات الموظف **** /////////////
        public IActionResult CommissionDate()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CommissionDate([Bind("StartDate,EndDate")] DatesSearch datesSearch)
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
                return RedirectToAction("StatementCommission", "AllBills");
            }
            return View(datesSearch);
        }
        public async Task<ActionResult> StatementCommission()
        {

            var UserId = _userManager.GetUserId(User);
            var CompanyId =(await _userManager.Users.SingleOrDefaultAsync(a => a.Id == UserId)).CompanyId;

            var billAirLine =await (from x in _context.BillAirLines
                   .Where(d => d.UserId == UserId)
                   .Where(A => A.CompanyID == CompanyId)
                   .Where(d => d.BillDate >= Convert.ToDateTime(TempData["StartDate"]))
                   .Where(d => d.BillDate.Date <= Convert.ToDateTime(TempData["EndDate"]))
                               select new Models.ViewModel.AllBillsViewModel { BillId = x.BillId, BillType = "طيران", BillDate = x.BillDate, CustomerOrSupplierName = x.CustomerOrSupplier.Name, CustomerSupplierName = x.CustomerSupplier.Name, MenuLE0Name = x.MenuLE0.M0_Name, MenuLE1Name = x.MenuLE1.M1_Name, MenuLE2Name = x.MenuLE2.M2_Name, Direction = x.Direction, PNR = x.PNR, TicketExportName = x.TicketExport.Name, AdultN = x.AdultN, ChildN = x.ChildN, CustomerPrice = x.CustomerPrice, NetPrice = x.NetPrice, Commission = x.EMPCommission, TicketFrom = x.TicketFrom, TicketTo = x.TicketTo, Commnets = x.Commnets, UserName = x.User.UserName, CompanyID = x.CompanyID }).ToListAsync();
            ViewData["billAirLine"] = billAirLine.Sum(a => a.Commission);

            var billDomestic =await (from x in _context.BillDomestic
                   .Where(d => d.UserId == UserId)
                   .Where(A => A.CompanyID == CompanyId)
                   .Where(d => d.BillDate >= Convert.ToDateTime(TempData["StartDate"]))
                   .Where(d => d.BillDate.Date <= Convert.ToDateTime(TempData["EndDate"]))
                                select new Models.ViewModel.AllBillsViewModel { BillId = x.BillId, BillType = "داخلى", BillDate = x.BillDate, CustomerOrSupplierName = x.CustomerOrSupplier.Name, CustomerSupplierName = x.CustomerSupplier.Name, MenuLE0Name = x.MenuLE0.M0_Name, MenuLE1Name = x.MenuLE1.M1_Name, MenuLE2Name = x.MenuLE2.M2_Name, TicketExportName = x.TicketExport.Name, AdultN = x.AdultN, ChildN = x.ChildN, CustomerPrice = x.CustomerPrice, NetPrice = x.NetPrice, Commission = x.EMPCommission, TicketFrom = x.TicketFrom, TicketTo = x.TicketTo, Commnets = x.Commnets, UserName = x.User.UserName, CompanyID = x.CompanyID }).ToListAsync();
            ViewData["billDomestic"] = billDomestic.Sum(a => a.Commission);

            var billForeign = await (from x in _context.BillForeigns
                    .Where(d => d.UserId == UserId)
                    .Where(A => A.CompanyID == CompanyId)
                    .Where(d => d.BillDate >= Convert.ToDateTime(TempData["StartDate"]))
                    .Where(d => d.BillDate.Date <= Convert.ToDateTime(TempData["EndDate"]))
                                      select new Models.ViewModel.AllBillsViewModel { BillId = x.BillId, BillType = "خارجى", BillDate = x.BillDate, CustomerOrSupplierName = x.CustomerOrSupplier.Name, CustomerSupplierName = x.CustomerSupplier.Name, MenuLE0Name = x.MenuLE0.M0_Name, MenuLE1Name = x.MenuLE1.M1_Name, MenuLE2Name = x.MenuLE2.M2_Name, TicketExportName = x.TicketExport.Name, AdultN = x.AdultN, ChildN = x.ChildN, CustomerPrice = x.CustomerPrice, NetPrice = x.NetPrice, Commission = x.EMPCommission, TicketFrom = x.TicketFrom, TicketTo = x.TicketTo, Commnets = x.Commnets, UserName = x.User.UserName, CompanyID = x.CompanyID }).ToListAsync();
            ViewData["billForeign"] = billForeign.Sum(a => a.Commission);

            var billVisas =await (from x in _context.BillVisas
                   .Where(d => d.UserId == UserId)
                   .Where(A => A.CompanyID == CompanyId)
                   .Where (a=>a.BillState == true)
                   .Where(d => d.ApprovedDate >= Convert.ToDateTime(TempData["StartDate"]))
                   .Where(d => d.ApprovedDate.Date <= Convert.ToDateTime(TempData["EndDate"]))
                             select new Models.ViewModel.AllBillsViewModel { BillId = x.BillId, BillType = "تأشيرات", BillDate = x.ApprovedDate, CustomerOrSupplierName = x.CustomerOrSupplier.Name, CustomerSupplierName = x.CustomerSupplier.Name, MenuLE0Name = x.MenuLE0.M0_Name, MenuLE1Name = x.MenuLE1.M1_Name, MenuLE2Name = x.MenuLE2.M2_Name, TicketExportName = x.TicketExport.Name, AdultN = x.AdultN, ChildN = x.ChildN, CustomerPrice = x.CustomerPrice, NetPrice = x.NetPrice, Commission = x.EMPCommission, Commnets = x.Commnets, UserName = x.User.UserName, CompanyID = x.CompanyID }).ToListAsync();
            ViewData["billVisas"] = billVisas.Sum(a => a.Commission);


            var Outbut = (billAirLine.Concat(billVisas).Concat(billDomestic).Concat(billForeign).OrderBy(a => a.BillDate));
            ViewData["CompanyLogo"] = "~/" + TravelERP.Properties.Resources.ImgFolder + "/" + _context.Companies.SingleOrDefault(a => a.Id == CompanyId).CompanyLogo;
            ViewData["CompanyA4EsalImage"] = "~/" + TravelERP.Properties.Resources.ImgFolder + "/" + _context.Companies.SingleOrDefault(a => a.Id == CompanyId).CompanyA4EsalImage;
            ViewData["CompanyNameE"] =(await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyId)).Company_NameE;
            ViewData["CompanyName"] =(await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyId)).Company_Name;
            ViewData["CompanyId"] = CompanyId;


            return View(Outbut);
        }


        //////////////////////////////////////////////////////////////////////////////////////
        ///// AllBills/CommissionDateAll **** العمولات - عمولات لجميع الموظفين **** ////////
        public IActionResult CommissionDateAll()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CommissionDateAll([Bind("StartDate,EndDate")] DatesSearch datesSearch)
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
                return RedirectToAction("StatementCommissionAll", "AllBills");
            }
            return View(datesSearch);
        }
        public async Task<ActionResult> StatementCommissionAll()
        {
            var UserId = _userManager.GetUserId(User);
            var CompanyId =(await _userManager.Users.SingleOrDefaultAsync(a => a.Id == UserId)).CompanyId;

            var billAirLine =await (from x in _context.BillAirLines
                               .Where(A => A.CompanyID == CompanyId)
                               .Where(d => d.BillDate >= Convert.ToDateTime(TempData["StartDate"]))
                               .Where(d => d.BillDate.Date <= Convert.ToDateTime(TempData["EndDate"]))
                               select new Models.ViewModel.AllBillsViewModel { BillId = x.BillId, BillType = "طيران", BillDate = x.BillDate, CustomerOrSupplierName = x.CustomerOrSupplier.Name, CustomerSupplierName = x.CustomerSupplier.Name, MenuLE0Name = x.MenuLE0.M0_Name, MenuLE1Name = x.MenuLE1.M1_Name, MenuLE2Name = x.MenuLE2.M2_Name, Direction = x.Direction, PNR = x.PNR, TicketExportName = x.TicketExport.Name, AdultN = x.AdultN, ChildN = x.ChildN, CustomerPrice = x.CustomerPrice, NetPrice = x.NetPrice, Commission = x.EMPCommission, TicketFrom = x.TicketFrom, TicketTo = x.TicketTo, Commnets = x.Commnets, UserName = x.User.UserName, CompanyID = x.CompanyID }).ToListAsync();
            ViewData["billAirLine"] = (billAirLine.Sum(a => a.Commission));

            var billDomestic =await (from x in _context.BillDomestic
                   .Where(A => A.CompanyID == CompanyId)
                   .Where(d => d.BillDate >= Convert.ToDateTime(TempData["StartDate"]))
                   .Where(d => d.BillDate.Date <= Convert.ToDateTime(TempData["EndDate"]))
                                select new Models.ViewModel.AllBillsViewModel { BillId = x.BillId, BillType = "داخلى", BillDate = x.BillDate, CustomerOrSupplierName = x.CustomerOrSupplier.Name, CustomerSupplierName = x.CustomerSupplier.Name, MenuLE0Name = x.MenuLE0.M0_Name, MenuLE1Name = x.MenuLE1.M1_Name, MenuLE2Name = x.MenuLE2.M2_Name, TicketExportName = x.TicketExport.Name, AdultN = x.AdultN, ChildN = x.ChildN, CustomerPrice = x.CustomerPrice, NetPrice = x.NetPrice, Commission = x.EMPCommission, TicketFrom = x.TicketFrom, TicketTo = x.TicketTo, Commnets = x.Commnets, UserName = x.User.UserName, CompanyID = x.CompanyID }).ToListAsync();
            ViewData["billDomestic"] = (billDomestic.Sum(a => a.Commission));

            var billForeign = await (from x in _context.BillForeigns
                    .Where(A => A.CompanyID == CompanyId)
                    .Where(d => d.BillDate >= Convert.ToDateTime(TempData["StartDate"]))
                    .Where(d => d.BillDate.Date <= Convert.ToDateTime(TempData["EndDate"]))
                                      select new Models.ViewModel.AllBillsViewModel { BillId = x.BillId, BillType = "خارجى", BillDate = x.BillDate, CustomerOrSupplierName = x.CustomerOrSupplier.Name, CustomerSupplierName = x.CustomerSupplier.Name, MenuLE0Name = x.MenuLE0.M0_Name, MenuLE1Name = x.MenuLE1.M1_Name, MenuLE2Name = x.MenuLE2.M2_Name, TicketExportName = x.TicketExport.Name, AdultN = x.AdultN, ChildN = x.ChildN, CustomerPrice = x.CustomerPrice, NetPrice = x.NetPrice, Commission = x.EMPCommission, TicketFrom = x.TicketFrom, TicketTo = x.TicketTo, Commnets = x.Commnets, UserName = x.User.UserName, CompanyID = x.CompanyID }).ToListAsync();
            ViewData["billForeign"] = (billForeign.Sum(a => a.Commission));

            var billVisas =await (from x in _context.BillVisas
                   .Where(A => A.CompanyID == CompanyId)
                   .Where (a=>a.BillState == true)
                   .Where(d => d.ApprovedDate >= Convert.ToDateTime(TempData["StartDate"]))
                   .Where(d => d.ApprovedDate.Date <= Convert.ToDateTime(TempData["EndDate"]))
                             select new Models.ViewModel.AllBillsViewModel { BillId = x.BillId, BillType = "تأشيرات", BillDate = x.ApprovedDate, CustomerOrSupplierName = x.CustomerOrSupplier.Name, CustomerSupplierName = x.CustomerSupplier.Name, MenuLE0Name = x.MenuLE0.M0_Name, MenuLE1Name = x.MenuLE1.M1_Name, MenuLE2Name = x.MenuLE2.M2_Name, TicketExportName = x.TicketExport.Name, AdultN = x.AdultN, ChildN = x.ChildN, CustomerPrice = x.CustomerPrice, NetPrice = x.NetPrice, Commission = x.EMPCommission, Commnets = x.Commnets, UserName = x.User.UserName, CompanyID = x.CompanyID }).ToListAsync();

            ViewData["billVisas"] = (billVisas.Sum(a => a.Commission));
            ViewData["CompanyLogo"] = "~/" + TravelERP.Properties.Resources.ImgFolder + "/" + _context.Companies.SingleOrDefault(a => a.Id == CompanyId).CompanyLogo;
            ViewData["CompanyA4EsalImage"] = "~/" + TravelERP.Properties.Resources.ImgFolder + "/" + _context.Companies.SingleOrDefault(a => a.Id == CompanyId).CompanyA4EsalImage;
            ViewData["CompanyNameE"] =(await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyId)).Company_NameE;
            ViewData["CompanyName"] =(await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyId)).Company_Name;
            ViewData["CompanyId"] = CompanyId;


            var Outbut = (billAirLine.Concat(billVisas).Concat(billDomestic).OrderBy(a => a.BillDate));



            return View(Outbut);

        }


        //////////////////////////////////////////////////////////////////////////////////////
        // AllBills/CXTStatementDate **** التقارير - التقارير المجمعه - تقرير شامل **** /
        public IActionResult CXTStatementDate()
        {
            ViewData["CustomerOrSupplierId"] = new SelectList(_context.CustomerOrSuppliers.Where(a=>a.Name != "مندوب" && a.Name != "جارى الشركاء"), "Id", "Name");
            ViewData["CustomerSupplierId"] = new SelectList(_context.CustomersSuppliers, "Id", "Name");
            ViewData["TicketExportId"] = new SelectList(_context.CustomersSuppliers.Where(a => a.CustomerOrSupplier.Name == "مورد او وكيل"), "Id", "Name");

            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CXTStatementDate([Bind("StartDate,Name1")] DatesSearch datesSearch)
        {
            if (datesSearch.StartDate == null)
            {
                ModelState.AddModelError("StartDate", "من فضلك ادخل تاريج صحيح");
            }
            if (ModelState.IsValid)
            {
                TempData["StartDate"] = datesSearch.StartDate.Value.Date.ToString();
                TempData["Name1"] = datesSearch.Name1;

                //_context.Add(datesSearch);
                await _context.SaveChangesAsync();
                return RedirectToAction("CXTStatement", "AllBills");
            }
            return View(datesSearch);
        }

        public async Task<IActionResult> CXTStatement()
        {
            var UserId = _userManager.GetUserId(User);
            var CompanyId =(await _userManager.Users.SingleOrDefaultAsync(a => a.Id == UserId)).CompanyId;
            var customersSuppliers =await (from user in _context.CustomersSuppliers.Where(a => a.CompanyId == CompanyId)
                                      where !_context.NonSuppliers.Any(f => f.CustomerSupplierId == user.Id)
                                      select new
                                      {
                                          user.Id,
                                          user.Name,
                                          user.CustomerOrSupplier,
                                          user.CustomerOrSupplierId
                                      }).ToListAsync();

            var ConcatBillEsal = (await( from x in _context.BillAirLines.Where(a => a.CustomerOrSupplierId == Convert.ToInt32(TempData["Name1"])).Where(d => d.BillDate.Date <= Convert.ToDateTime(TempData["StartDate"]).Date).Where(A => A.CompanyID == CompanyId) select new { Name2 = x.CustomerSupplierId, Credit = -x.CustomerPrice }).ToListAsync())
                             .Concat(await (from x in _context.BillVisas.Where(a => a.CustomerOrSupplierId == Convert.ToInt32(TempData["Name1"])).Where(d => d.BillDate.Date <= Convert.ToDateTime(TempData["StartDate"]).Date).Where(A => A.CompanyID == CompanyId) select new { Name2 = x.CustomerSupplierId, Credit = -x.CustomerPrice }).ToListAsync())
                             .Concat(await (from x in _context.BillDomestic.Where(a => a.CustomerOrSupplierId == Convert.ToInt32(TempData["Name1"])).Where(d => d.BillDate.Date <= Convert.ToDateTime(TempData["StartDate"]).Date).Where(A => A.CompanyID == CompanyId) select new { Name2 = x.CustomerSupplierId, Credit = -x.CustomerPrice }).ToListAsync())
                             .Concat(await (from x in _context.BillForeigns.Where(a => a.CustomerOrSupplierId == Convert.ToInt32(TempData["Name1"])).Where(d => d.BillDate.Date <= Convert.ToDateTime(TempData["StartDate"]).Date).Where(A => A.CompanyID == CompanyId) select new { Name2 = x.CustomerSupplierId, Credit = -x.CustomerPrice }).ToListAsync())
                             //.Concat(from x in _context.BillReligious.Where(a => a.CustomerOrSupplier.Name == "مورد او وكيل").Where(d => d.BillDate.Date <= Convert.ToDateTime(TempData["StartDate"]).Date).Where(A => A.CompanyID == _userManager.Users.SingleOrDefault(a => a.Id == _userManager.GetUserId(User)).CompanyId) select new { Name2 = x.CustomerSupplierId, Credit = -x.CustomerPrice })
                             .Concat(await (from x in _context.Esals.Where(a => a.CustomerOrSupplierId == Convert.ToInt32(TempData["Name1"])).Where(d => d.EsalDate.Date <= Convert.ToDateTime(TempData["StartDate"]).Date).Where(A => A.CompanyID == CompanyId) select new { Name2 = x.CustomerSupplierId, Credit = x.AmountPaid }).ToListAsync())
                             .Concat(await (from x in _context.BillAirLines.Where(a => a.TicketExport.CustomerOrSupplierId == Convert.ToInt32(TempData["Name1"])).Where(d => d.BillDate.Date <= Convert.ToDateTime(TempData["StartDate"]).Date).Where(A => A.CompanyID == CompanyId) select new { Name2 = x.TicketExportId, Credit = x.NetPrice }).ToListAsync())
                             .Concat(await (from x in _context.BillVisas.Where(a => a.TicketExport.CustomerOrSupplierId == Convert.ToInt32(TempData["Name1"])).Where(d => d.BillDate.Date <= Convert.ToDateTime(TempData["StartDate"]).Date).Where(A => A.CompanyID == CompanyId) select new { Name2 = x.TicketExportId, Credit = x.NetPrice }).ToListAsync())
                             .Concat(await (from x in _context.BillDomestic.Where(a => a.TicketExport.CustomerOrSupplierId == Convert.ToInt32(TempData["Name1"])).Where(d => d.BillDate.Date <= Convert.ToDateTime(TempData["StartDate"]).Date).Where(A => A.CompanyID == CompanyId) select new { Name2 = x.TicketExportId, Credit = x.NetPrice }).ToListAsync())
                             .Concat(await (from x in _context.BillForeigns.Where(a => a.TicketExport.CustomerOrSupplierId == Convert.ToInt32(TempData["Name1"])).Where(d => d.BillDate.Date <= Convert.ToDateTime(TempData["StartDate"]).Date).Where(A => A.CompanyID == CompanyId) select new { Name2 = x.TicketExportId, Credit = x.NetPrice }).ToListAsync())
                             //.Concat(from x in _context.BillReligious.Where(d => d.BillDate.Date <= Convert.ToDateTime(TempData["StartDate"]).Date).Where(A => A.CompanyID == _userManager.Users.SingleOrDefault(a => a.Id == _userManager.GetUserId(User)).CompanyId) select new { Name2 = x.TicketExportId, Credit = x.NetPrice })
                             .Concat(await (from x in _context.Ezns.Where(a => a.MenuLE0Id != null).Where(a => a.CustomerOrSupplierId == Convert.ToInt32(TempData["Name1"])).Where(d => d.EznDate.Date <= Convert.ToDateTime(TempData["StartDate"]).Date).Where(A => A.CompanyID == CompanyId) select new { Name2 = (int)x.CustomerSupplierId, Credit = -x.AmountWithdrawn }).ToListAsync())
                 .Concat(await (from x in _context.OpeningBalances.Where(a=>a.CustomerOrSupplierId == Convert.ToInt32(TempData["Name1"])).Where(a => a.StatementType.Name != "الخزينه").Where(A => A.CompanyID == CompanyId) select new { Name2 = x.CustomerSupplierId, Credit = x.Balance }).ToListAsync())
                 .OrderByDescending(p => p.Name2).ToList()
                 .GroupBy(x => x.Name2)
                 .Select(a =>
                     new
                     {
                         Name2 = a.Key,
                         Credit = a.Sum(x => x.Credit)
                     });

            var Outbut = (from x in ConcatBillEsal where !_context.NonSuppliers.Any(f => f.CustomerSupplierId == x.Name2) select new Models.ViewModel.Transactions { Statement = _context.CustomersSuppliers.SingleOrDefault(a => a.Id == x.Name2).Name, Credit = x.Credit }).OrderBy(d => d.Credit);
            if (Outbut == null)
            {
                return NotFound();
            }
            ViewData["CompanyLogo"] = "~/" + TravelERP.Properties.Resources.ImgFolder + "/" +(await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyId)).CompanyLogo;
            ViewData["CompanyA4EsalImage"] = "~/" + TravelERP.Properties.Resources.ImgFolder + "/" +(await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyId)).CompanyA4EsalImage;
            ViewData["CompanyNameE"] =(await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyId)).Company_NameE;
            ViewData["CompanyName"] =(await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyId)).Company_Name;
            ViewData["CompanyId"] = CompanyId;

            return View( Outbut);
        }

        //////////////////////////////////////////////////////////////////////////////////////
        // AllBills/CXStatementDate **** التقارير - التقارير المفصله - تقرير شامل **** //

        public async Task<IActionResult> CXStatementDate()
        {

            var CompanyId =(await _userManager.Users.SingleOrDefaultAsync(a => a.Id == _userManager.GetUserId(User))).CompanyId;
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
            //ViewData["CustomerSupplierId"] = new SelectList(_context.CustomersSuppliers, "Id", "Name");
            ViewData["CustomerSupplierId"] = new SelectList(customersSuppliers.OrderBy(d => d.Name), "Id", "Name");

            return View();

        }

        [HttpPost]
        public async Task<IActionResult> CXStatementDate([Bind("StartDate,EndDate,Name1,Name2")] DatesSearch datesSearch)
        {
            if (datesSearch.Name1 == 0)
            {
                ModelState.AddModelError("Name1", "من فضلك اختر اسم شركه او عميل للبحث");
            }
            if (datesSearch.Name2 == 0)
            {
                ModelState.AddModelError("Name2", "من فضلك اختر اسم مورد للبحث");
            }
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
                TempData["Name2"] = datesSearch.Name2;


                //_context.Add(datesSearch);
                await _context.SaveChangesAsync();
                return RedirectToAction("CXStatement", "AllBills");
            }
            var CompanyId =(await _userManager.Users.SingleOrDefaultAsync(a => a.Id == _userManager.GetUserId(User))).CompanyId;
            var customersSuppliers =await (from user in _context.CustomersSuppliers.Where(a => a.CompanyId == CompanyId)
                                      where !_context.NonSuppliers.Any(f => f.CustomerSupplierId == user.Id)
                                      select new
                                      {
                                          user.Id,
                                          user.Name,
                                          user.CustomerOrSupplier,
                                          user.CustomerOrSupplierId
                                      }).ToListAsync();
            ViewData["CustomerOrSupplierId"] = new SelectList(_context.CustomerOrSuppliers, "Id", "Name",datesSearch.Name1);
            ViewData["CustomerSupplierId"] = new SelectList(customersSuppliers.Where(a => a.CustomerOrSupplierId == datesSearch.Name1).OrderBy(d => d.Name), "Id", "Name", datesSearch.Name2);

            return View(datesSearch);
        }
        public async Task<ActionResult> CXStatement()
        {
            var CompanyId = _userManager.Users.SingleOrDefault(a => a.Id == _userManager.GetUserId(User)).CompanyId;
            //StartTransactions
            var startBillAirLineEzn =await (from x in _context.BillAirLines.Where(A => A.CompanyID == CompanyId) select new Models.ViewModel.Transactions { EDate = x.BillDate, Debit = x.NetPrice, Name2 = x.TicketExportId }).Where(d => d.EDate < Convert.ToDateTime(TempData["StartDate"])).Where(d => d.Name2 == Convert.ToInt32(TempData["Name2"])).ToListAsync();
            var startBillAirLineEsal =await (from x in _context.BillAirLines.Where(A => A.CompanyID == CompanyId) select new Models.ViewModel.Transactions { EDate = x.BillDate, Credit = x.CustomerPrice, Name2 = x.CustomerSupplierId }).Where(d => d.EDate < Convert.ToDateTime(TempData["StartDate"])).Where(d => d.Name2 == Convert.ToInt32(TempData["Name2"])).ToListAsync();

            var startBillVisasEzn = await (from x in _context.BillVisas.Where(A => A.CompanyID == CompanyId) select new Models.ViewModel.Transactions { EDate = x.BillDate, Debit = x.NetPrice, Name2 = x.TicketExportId }).Where(d => d.EDate < Convert.ToDateTime(TempData["StartDate"])).Where(d => d.Name2 == Convert.ToInt32(TempData["Name2"])).ToListAsync();
            var startBillVisasEsal = await (from x in _context.BillVisas.Where(A => A.CompanyID == CompanyId) select new Models.ViewModel.Transactions { EDate = x.BillDate, Credit = x.CustomerPrice, Name2 = x.CustomerSupplierId }).Where(d => d.EDate < Convert.ToDateTime(TempData["StartDate"])).Where(d => d.Name2 == Convert.ToInt32(TempData["Name2"])).ToListAsync();


            var startBillDomesticEzn = await (from x in _context.BillDomestic.Where(A => A.CompanyID == CompanyId) select new Models.ViewModel.Transactions { EDate = x.BillDate, Debit = x.NetPrice, Name2 = x.TicketExportId }).Where(d => d.EDate < Convert.ToDateTime(TempData["StartDate"])).Where(d => d.Name2 == Convert.ToInt32(TempData["Name2"])).ToListAsync();
            var startBillDomesticEsal = await (from x in _context.BillDomestic.Where(A => A.CompanyID == CompanyId) select new Models.ViewModel.Transactions { EDate = x.BillDate, Credit = x.CustomerPrice, Name2 = x.CustomerSupplierId }).Where(d => d.EDate < Convert.ToDateTime(TempData["StartDate"])).Where(d => d.Name2 == Convert.ToInt32(TempData["Name2"])).ToListAsync();

            var startBillForeignEzn = await (from x in _context.BillForeigns.Where(A => A.CompanyID == CompanyId) select new Models.ViewModel.Transactions { EDate = x.BillDate, Debit = x.NetPrice, Name2 = x.TicketExportId }).Where(d => d.EDate < Convert.ToDateTime(TempData["StartDate"])).Where(d => d.Name2 == Convert.ToInt32(TempData["Name2"])).ToListAsync();
            var startBillForeignEsal = await (from x in _context.BillForeigns.Where(A => A.CompanyID == CompanyId) select new Models.ViewModel.Transactions { EDate = x.BillDate, Credit = x.CustomerPrice, Name2 = x.CustomerSupplierId }).Where(d => d.EDate < Convert.ToDateTime(TempData["StartDate"])).Where(d => d.Name2 == Convert.ToInt32(TempData["Name2"])).ToListAsync();

            var startEsal = await (from x in _context.Esals.Where(a => a.MenuLE0.M0_Name != "تمويل").Where(A => A.CompanyID == CompanyId) select new Models.ViewModel.Transactions { EDate = x.EsalDate, Debit = x.AmountPaid, Name2 = x.CustomerSupplierId }).Where(d => d.EDate < Convert.ToDateTime(TempData["StartDate"])).Where(d => d.Name2 == Convert.ToInt32(TempData["Name2"])).ToListAsync();
            var startEzn = await (from x in _context.Ezns.Where(a => a.MenuLE0Id != null).Where(A => A.CompanyID == CompanyId) select new Models.ViewModel.Transactions { EDate = x.EznDate, Credit = x.AmountWithdrawn, Name2 = x.CustomerSupplierId }).Where(d => d.EDate < Convert.ToDateTime(TempData["StartDate"])).Where(d => d.Name2 == Convert.ToInt32(TempData["Name2"])).ToListAsync();
            ViewData["startTransactions"] = (startEsal.Sum(a => a.Debit) + (startBillAirLineEzn.Sum(a => a.Debit) + startBillVisasEzn.Sum(a => a.Debit) + startBillDomesticEzn.Sum(a => a.Debit) + startBillForeignEzn.Sum(a => a.Debit))) - (startEzn.Sum(a => a.Credit) + (startBillAirLineEsal.Sum(a => a.Credit) + startBillVisasEsal.Sum(a => a.Credit) + startBillDomesticEsal.Sum(a => a.Credit) + startBillForeignEsal.Sum(a => a.Credit)));
            ViewData["OpeningBalance"] = (from x in _context.OpeningBalances.Where(A => A.CompanyID == CompanyId) select new { x.Balance, Name2 = x.CustomerSupplierId }).Where(d => d.Name2 == Convert.ToInt32(TempData["Name2"])).Sum(a => a.Balance);
            //For Head Name
            TempData["NameCS"] =(await _context.CustomersSuppliers.SingleOrDefaultAsync(a => a.Id == Convert.ToInt32(TempData["Name2"]))).Name;

            var billAirLineEzn =await (from x in _context.BillAirLines.Where(A => A.CompanyID == CompanyId) select new Models.ViewModel.Transactions { EId = x.BillId, Type = "طيران", EDate = x.BillDate, Statement = string.Concat(x.MenuLE0.M0_Name, " - ", x.MenuLE1.M1_Name, " - ", x.MenuLE2.M2_Name, " - ", x.Direction, " - ", x.PNR, " - ", x.Commnets), Debit = x.NetPrice, Name2 = x.TicketExportId }).Where(d => d.EDate >= Convert.ToDateTime(TempData["StartDate"])).Where(d => d.EDate.Date <= Convert.ToDateTime(TempData["EndDate"])).Where(d => d.Name2 == Convert.ToInt32(TempData["Name2"])).ToListAsync();
            var billAirLineEsal =await (from x in _context.BillAirLines.Where(A => A.CompanyID == CompanyId) select new Models.ViewModel.Transactions { EId = x.BillId, Type = "طيران", EDate = x.BillDate, Statement = string.Concat(x.MenuLE0.M0_Name, " - ", x.MenuLE1.M1_Name, " - ", x.MenuLE2.M2_Name, " - ", x.Direction, " - ", x.PNR, " - ", x.Commnets), Credit = x.CustomerPrice, Name2 = x.CustomerSupplierId }).Where(d => d.EDate >= Convert.ToDateTime(TempData["StartDate"])).Where(d => d.EDate.Date <= Convert.ToDateTime(TempData["EndDate"])).Where(d => d.Name2 == Convert.ToInt32(TempData["Name2"])).ToListAsync();
            var esalAirLine =await (from x in _context.Esals.Where(a => a.MenuLE0.M0_Name == "طيران").Where(A => A.CompanyID == CompanyId) select new Models.ViewModel.Transactions { EId = x.EsalId, Type = "إيصال", EDate = x.EsalDate, Statement = string.Concat(x.MenuLE0.M0_Name, " - ", x.MenuLE1.M1_Name, " - ", x.MenuLE2.M2_Name, " - ", x.DepositDesc, " - " + x.PaymentMethod.Name), Debit = x.AmountPaid, Name1 = x.CustomerOrSupplierId, Name2 = x.CustomerSupplierId }).Where(d => d.EDate >= Convert.ToDateTime(TempData["StartDate"])).Where(d => d.EDate.Date <= Convert.ToDateTime(TempData["EndDate"])).Where(d => d.Name2 == Convert.ToInt32(TempData["Name2"])).ToListAsync();
            var eznAirLine =await (from x in _context.Ezns.Where(a => a.MenuLE0.M0_Name == "طيران").Where(A => A.CompanyID == CompanyId) select new Models.ViewModel.Transactions { EId = x.EznId, Type = "إذن صرف", EDate = x.EznDate, Statement = string.Concat(x.MenuLE0.M0_Name, " - ", x.ExpenseName), Credit = x.AmountWithdrawn, Name1 = x.CustomerOrSupplierId, Name2 = x.CustomerSupplierId }).Where(d => d.EDate >= Convert.ToDateTime(TempData["StartDate"])).Where(d => d.EDate.Date <= Convert.ToDateTime(TempData["EndDate"])).Where(d => d.Name2 == Convert.ToInt32(TempData["Name2"])).ToListAsync();

            var billVisasEzn =await (from x in _context.BillVisas.Where(A => A.CompanyID == CompanyId) select new Models.ViewModel.Transactions { EId = x.BillId, Type = "تأشيره", EDate = x.BillDate, Statement = string.Concat(x.MenuLE0.M0_Name, " - ", x.MenuLE1.M1_Name, " - ", x.MenuLE2.M2_Name, " - ", x.CustomerOrSupplier.Name, " - ", x.CustomerSupplier.Name, " - ", x.Commnets, " - ", x.BillState == false ? "Pending" : "Approved"), Debit = x.NetPrice, Name2 = x.TicketExportId }).Where(d => d.EDate >= Convert.ToDateTime(TempData["StartDate"])).Where(d => d.EDate.Date <= Convert.ToDateTime(TempData["EndDate"])).Where(d => d.Name2 == Convert.ToInt32(TempData["Name2"])).ToListAsync();
            var billVisasEsal =await (from x in _context.BillVisas.Where(A => A.CompanyID == CompanyId) select new Models.ViewModel.Transactions { EId = x.BillId, Type = "تأشيره", EDate = x.BillDate, Statement = string.Concat(x.MenuLE0.M0_Name, " - ", x.MenuLE1.M1_Name, " - ", x.MenuLE2.M2_Name, " - ", x.CustomerOrSupplier.Name, " - ", x.CustomerSupplier.Name, " - ", x.Commnets, " - ", x.BillState == false ? "Pending" : "Approved"), Credit = x.CustomerPrice, Name2 = x.CustomerSupplierId }).Where(d => d.EDate >= Convert.ToDateTime(TempData["StartDate"])).Where(d => d.EDate.Date <= Convert.ToDateTime(TempData["EndDate"])).Where(d => d.Name2 == Convert.ToInt32(TempData["Name2"])).ToListAsync();
            var esalVisas =await (from x in _context.Esals.Where(a => a.MenuLE0.M0_Name == "تأشيرات").Where(A => A.CompanyID == CompanyId) select new Models.ViewModel.Transactions { EId = x.EsalId, Type = "إيصال", EDate = x.EsalDate, Statement = string.Concat(x.MenuLE0.M0_Name, " - ", x.MenuLE1.M1_Name, " - ", x.MenuLE2.M2_Name, " - ", x.DepositDesc, " - " + x.PaymentMethod.Name), Debit = x.AmountPaid, Name1 = x.CustomerOrSupplierId, Name2 = x.CustomerSupplierId }).Where(d => d.EDate >= Convert.ToDateTime(TempData["StartDate"])).Where(d => d.EDate.Date <= Convert.ToDateTime(TempData["EndDate"])).Where(d => d.Name2 == Convert.ToInt32(TempData["Name2"])).ToListAsync();
            var eznVisas =await (from x in _context.Ezns.Where(a => a.MenuLE0.M0_Name == "تأشيرات").Where(A => A.CompanyID == CompanyId) select new Models.ViewModel.Transactions { EId = x.EznId, Type = "إذن صرف", EDate = x.EznDate, Statement = string.Concat(x.MenuLE0.M0_Name, " - ", x.ExpenseName), Credit = x.AmountWithdrawn, Name1 = x.CustomerOrSupplierId, Name2 = x.CustomerSupplierId }).Where(d => d.EDate >= Convert.ToDateTime(TempData["StartDate"])).Where(d => d.EDate.Date <= Convert.ToDateTime(TempData["EndDate"])).Where(d => d.Name2 == Convert.ToInt32(TempData["Name2"])).ToListAsync();


            var billDomesticEzn =await (from x in _context.BillDomestic.Where(A => A.CompanyID == CompanyId) select new Models.ViewModel.Transactions { EId = x.BillId, Type = "داخلى", EDate = x.BillDate, Statement = string.Concat(x.MenuLE0.M0_Name, " - ", x.MenuLE1.M1_Name, " - ", x.MenuLE2.M2_Name, " - ", x.CustomerOrSupplier.Name, " - ", x.CustomerSupplier.Name, " - ", x.Commnets), Debit = x.NetPrice, Name2 = x.TicketExportId }).Where(d => d.EDate >= Convert.ToDateTime(TempData["StartDate"])).Where(d => d.EDate.Date <= Convert.ToDateTime(TempData["EndDate"])).Where(d => d.Name2 == Convert.ToInt32(TempData["Name2"])).ToListAsync();
            var billDomesticEsal =await (from x in _context.BillDomestic.Where(A => A.CompanyID == CompanyId) select new Models.ViewModel.Transactions { EId = x.BillId, Type = "داخلى", EDate = x.BillDate, Statement = string.Concat(x.MenuLE0.M0_Name, " - ", x.MenuLE1.M1_Name, " - ", x.MenuLE2.M2_Name, " - ", x.CustomerOrSupplier.Name, " - ", x.CustomerSupplier.Name, " - ", x.Commnets), Credit = x.CustomerPrice, Name2 = x.CustomerSupplierId }).Where(d => d.EDate >= Convert.ToDateTime(TempData["StartDate"])).Where(d => d.EDate.Date <= Convert.ToDateTime(TempData["EndDate"])).Where(d => d.Name2 == Convert.ToInt32(TempData["Name2"])).ToListAsync();
            var esalDomestic =await (from x in _context.Esals.Where(a => a.MenuLE0.M0_Name == "سياحة داخلية").Where(A => A.CompanyID == CompanyId) select new Models.ViewModel.Transactions { EId = x.EsalId, Type = "إيصال", EDate = x.EsalDate, Statement = string.Concat(x.MenuLE0.M0_Name, " - ", x.MenuLE1.M1_Name, " - ", x.MenuLE2.M2_Name, " - ", x.DepositDesc, " - " + x.PaymentMethod.Name), Debit = x.AmountPaid, Name1 = x.CustomerOrSupplierId, Name2 = x.CustomerSupplierId }).Where(d => d.EDate >= Convert.ToDateTime(TempData["StartDate"])).Where(d => d.EDate.Date <= Convert.ToDateTime(TempData["EndDate"])).Where(d => d.Name2 == Convert.ToInt32(TempData["Name2"])).ToListAsync();
            var eznDomestic =await (from x in _context.Ezns.Where(a => a.MenuLE0.M0_Name == "سياحة داخلية").Where(A => A.CompanyID == CompanyId) select new Models.ViewModel.Transactions { EId = x.EznId, Type = "إذن صرف", EDate = x.EznDate, Statement = string.Concat(x.MenuLE0.M0_Name, " - ", x.ExpenseName), Credit = x.AmountWithdrawn, Name1 = x.CustomerOrSupplierId, Name2 = x.CustomerSupplierId }).Where(d => d.EDate >= Convert.ToDateTime(TempData["StartDate"])).Where(d => d.EDate.Date <= Convert.ToDateTime(TempData["EndDate"])).Where(d => d.Name2 == Convert.ToInt32(TempData["Name2"])).ToListAsync();

            var billForeignEzn = await (from x in _context.BillForeigns.Where(A => A.CompanyID == CompanyId) select new Models.ViewModel.Transactions { EId = x.BillId, Type = "خارجى", EDate = x.BillDate, Statement = string.Concat(x.MenuLE0.M0_Name, " - ", x.MenuLE1.M1_Name, " - ", x.MenuLE2.M2_Name, " - ", x.CustomerOrSupplier.Name, " - ", x.CustomerSupplier.Name, " - ", x.Commnets), Debit = x.NetPrice, Name2 = x.TicketExportId }).Where(d => d.EDate >= Convert.ToDateTime(TempData["StartDate"])).Where(d => d.EDate.Date <= Convert.ToDateTime(TempData["EndDate"])).Where(d => d.Name2 == Convert.ToInt32(TempData["Name2"])).ToListAsync();
            var billForeignEsal = await (from x in _context.BillForeigns.Where(A => A.CompanyID == CompanyId) select new Models.ViewModel.Transactions { EId = x.BillId, Type = "خارجى", EDate = x.BillDate, Statement = string.Concat(x.MenuLE0.M0_Name, " - ", x.MenuLE1.M1_Name, " - ", x.MenuLE2.M2_Name, " - ", x.CustomerOrSupplier.Name, " - ", x.CustomerSupplier.Name, " - ", x.Commnets), Credit = x.CustomerPrice, Name2 = x.CustomerSupplierId }).Where(d => d.EDate >= Convert.ToDateTime(TempData["StartDate"])).Where(d => d.EDate.Date <= Convert.ToDateTime(TempData["EndDate"])).Where(d => d.Name2 == Convert.ToInt32(TempData["Name2"])).ToListAsync();
            var esalForeign = await (from x in _context.Esals.Where(a => a.MenuLE0.M0_Name == "سياحة خارجية").Where(A => A.CompanyID == CompanyId) select new Models.ViewModel.Transactions { EId = x.EsalId, Type = "إيصال", EDate = x.EsalDate, Statement = string.Concat(x.MenuLE0.M0_Name, " - ", x.MenuLE1.M1_Name, " - ", x.MenuLE2.M2_Name, " - ", x.DepositDesc, " - " + x.PaymentMethod.Name), Debit = x.AmountPaid, Name1 = x.CustomerOrSupplierId, Name2 = x.CustomerSupplierId }).Where(d => d.EDate >= Convert.ToDateTime(TempData["StartDate"])).Where(d => d.EDate.Date <= Convert.ToDateTime(TempData["EndDate"])).Where(d => d.Name2 == Convert.ToInt32(TempData["Name2"])).ToListAsync();
            var eznForeign = await (from x in _context.Ezns.Where(a => a.MenuLE0.M0_Name == "سياحة خارجية").Where(A => A.CompanyID == CompanyId) select new Models.ViewModel.Transactions { EId = x.EznId, Type = "إذن صرف", EDate = x.EznDate, Statement = string.Concat(x.MenuLE0.M0_Name, " - ", x.ExpenseName), Credit = x.AmountWithdrawn, Name1 = x.CustomerOrSupplierId, Name2 = x.CustomerSupplierId }).Where(d => d.EDate >= Convert.ToDateTime(TempData["StartDate"])).Where(d => d.EDate.Date <= Convert.ToDateTime(TempData["EndDate"])).Where(d => d.Name2 == Convert.ToInt32(TempData["Name2"])).ToListAsync();

            var Outbut =  ((billAirLineEsal.Concat(billAirLineEzn).Concat(esalAirLine).Concat(eznAirLine)
                .Concat(billVisasEsal).Concat(billVisasEzn).Concat(esalVisas).Concat(eznVisas)
                .Concat(billDomesticEsal).Concat(billDomesticEzn).Concat(esalDomestic).Concat(eznDomestic)
                .Concat(billForeignEsal).Concat(billForeignEzn).Concat(esalForeign).Concat(eznForeign)).OrderBy(a => a.EDate));

            if (Outbut == null)
            {
                return NotFound();
            }
            ViewData["CompanyLogo"] = "~/" + TravelERP.Properties.Resources.ImgFolder + "/" +(await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyId)).CompanyLogo;
            ViewData["CompanyA4EsalImage"] = "~/" + TravelERP.Properties.Resources.ImgFolder + "/" +(await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyId)).CompanyA4EsalImage;
            ViewData["CompanyNameE"] =(await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyId)).Company_NameE;
            ViewData["CompanyName"] =(await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyId)).Company_Name;
            ViewData["CompanyId"] = CompanyId;


            return View(Outbut);
        }



        //////////////////////////////////////////////////////////////////////////////////////
        ///// AllBills/CommissionDateTotal **** العمولات - تقرير مجمع للعمولات **** ////////
        public IActionResult CommissionDateTotal()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CommissionDateTotal([Bind("StartDate,EndDate")] DatesSearch datesSearch)
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
                return RedirectToAction("StatementCommissionTotal", "AllBills");
            }
            return View(datesSearch);
        }
        public async Task<ActionResult> StatementCommissionTotal()
        {
            var UserId = _userManager.GetUserId(User);
            var CompanyId =(await _userManager.Users.SingleOrDefaultAsync(a => a.Id == UserId)).CompanyId;

            var Commission = (await (from x in _context.BillAirLines.Where(d => d.BillDate.Date >= Convert.ToDateTime(TempData["StartDate"])).Where(d => d.BillDate.Date <= Convert.ToDateTime(TempData["EndDate"])).Where(A => A.CompanyID == CompanyId) select new { user = x.User.UserName, Commission = x.EMPCommission }).ToListAsync())
                 .Concat(await( from x in _context.BillVisas.Where(d => d.ApprovedDate.Date >= Convert.ToDateTime(TempData["StartDate"])).Where(d => d.ApprovedDate.Date <= Convert.ToDateTime(TempData["EndDate"])).Where(a=>a.BillState == true).Where(A => A.CompanyID == CompanyId) select new { user = x.User.UserName, Commission = x.EMPCommission }).ToListAsync())
                 .Concat(await( from x in _context.BillDomestic.Where(d => d.BillDate.Date >= Convert.ToDateTime(TempData["StartDate"])).Where(d => d.BillDate.Date <= Convert.ToDateTime(TempData["EndDate"])).Where(A => A.CompanyID == CompanyId) select new { user = x.User.UserName, Commission = x.EMPCommission }).ToListAsync())
                 .Concat(await (from x in _context.BillForeigns.Where(d => d.BillDate.Date >= Convert.ToDateTime(TempData["StartDate"])).Where(d => d.BillDate.Date <= Convert.ToDateTime(TempData["EndDate"])).Where(A => A.CompanyID == CompanyId) select new { user = x.User.UserName, Commission = x.EMPCommission }).ToListAsync())
                 .GroupBy(x => x.user)
                 .Select(a =>
                     new
                     {
                         User = a.Key,
                         Commission = a.Sum(x => x.Commission)
                     });

            var Outbut = (from x in Commission select new Models.ViewModel.Transactions { Statement = x.User, Commission = x.Commission }).OrderByDescending(a => a.Commission);

            if (Outbut == null)
            {
                return NotFound();
            }
            ViewData["CompanyLogo"] = "~/" + TravelERP.Properties.Resources.ImgFolder + "/" +(await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyId)).CompanyLogo;
            ViewData["CompanyA4EsalImage"] = "~/" + TravelERP.Properties.Resources.ImgFolder + "/" +(await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyId)).CompanyA4EsalImage;
            ViewData["CompanyNameE"] =(await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyId)).Company_NameE;
            ViewData["CompanyName"] =(await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyId)).Company_Name;
            ViewData["CompanyId"] = CompanyId;

            return View(Outbut);

        }




        ///////////////////////////////////////////////////////////////////////////////////////////
        // AllBills/CommissionDateCompanies **** الاداره - عمولات الموظفين لجميع الشركات **** //
        public IActionResult CommissionDateCompanies()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CommissionDateCompanies([Bind("StartDate,EndDate")] DatesSearch datesSearch)
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
                return RedirectToAction("StatementCommissionCompanies", "AllBills");
            }
            return View(datesSearch);
        }
        public async Task<ActionResult> StatementCommissionCompanies()
        {
            var UserId = _userManager.GetUserId(User);
            var CompanyId =(await _userManager.Users.SingleOrDefaultAsync(a => a.Id == UserId)).CompanyId;
            var Commission = (await (from x in _context.BillAirLines.Where(d => d.BillDate.Date >= Convert.ToDateTime(TempData["StartDate"])).Where(d => d.BillDate.Date <= Convert.ToDateTime(TempData["EndDate"])) select new { user = x.User.UserName, Commission = (x.CustomerPrice - x.NetPrice) * (_context.UsersDetails.Where(a => a.UserId == x.UserId).Count() == 0 ? 0 : _context.UsersDetails.SingleOrDefault(a => a.UserId == x.UserId).Commission) }).ToListAsync())
                .Concat(await (from x in _context.BillVisas.Where(d => d.ApprovedDate.Date >= Convert.ToDateTime(TempData["StartDate"])).Where(d => d.ApprovedDate.Date <= Convert.ToDateTime(TempData["EndDate"])).Where(a => a.BillState == true) select new { user = x.User.UserName, Commission = (x.CustomerPrice - x.NetPrice) * (_context.UsersDetails.Where(a => a.UserId == x.UserId).Count() == 0 ? 0 : _context.UsersDetails.SingleOrDefault(a => a.UserId == x.UserId).Commission) }).ToListAsync())
                .Concat(await (from x in _context.BillDomestic.Where(d => d.BillDate.Date >= Convert.ToDateTime(TempData["StartDate"])).Where(d => d.BillDate.Date <= Convert.ToDateTime(TempData["EndDate"])) select new { user = x.User.UserName, Commission = (x.CustomerPrice - x.NetPrice) * (_context.UsersDetails.Where(a => a.UserId == x.UserId).Count() == 0 ? 0 : _context.UsersDetails.SingleOrDefault(a => a.UserId == x.UserId).Commission) }).ToListAsync())
                .Concat(await (from x in _context.BillForeigns.Where(d => d.BillDate.Date >= Convert.ToDateTime(TempData["StartDate"])).Where(d => d.BillDate.Date <= Convert.ToDateTime(TempData["EndDate"])) select new { user = x.User.UserName, Commission = (x.CustomerPrice - x.NetPrice) * (_context.UsersDetails.Where(a => a.UserId == x.UserId).Count() == 0 ? 0 : _context.UsersDetails.SingleOrDefault(a => a.UserId == x.UserId).Commission) }).ToListAsync())
                .OrderByDescending(p => p.user)
                 .GroupBy(x => x.user)
                 .Select(a =>
                     new
                     {
                         User = a.Key,
                         Commission = a.Sum(x => x.Commission)
                     });

            var Outbut = (from x in Commission select new Models.ViewModel.Transactions { Statement = x.User, Commission = x.Commission, Name3 = _context.Companies.SingleOrDefault(a => a.Id == _context.Users.SingleOrDefault(u => u.UserName == x.User).CompanyId).Company_Name }).OrderByDescending(a => a.Commission);

            if (Outbut == null)
            {
                return NotFound();
            }
            ViewData["CompanyLogo"] = "~/" + TravelERP.Properties.Resources.ImgFolder + "/" +(await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyId)).CompanyLogo;
            ViewData["CompanyA4EsalImage"] = "~/" + TravelERP.Properties.Resources.ImgFolder + "/" +(await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyId)).CompanyA4EsalImage;
            ViewData["CompanyNameE"] =(await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyId)).Company_NameE;
            ViewData["CompanyName"] =(await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyId)).Company_Name;
            ViewData["CompanyId"] = CompanyId;


            return View(Outbut);

        }


        //////////////////////////////////////////////////////////////////////////////////////
        // AllBills/CXTStatementAllDate **** التقارير - تقرير مجمع لكل الشركات **** //
        public IActionResult CXTStatementAllDate()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CXTStatementAllDate([Bind("StartDate")] DatesSearch datesSearch)
        {
            if (datesSearch.StartDate == null)
            {
                ModelState.AddModelError("StartDate", "من فضلك اختر تاريخ للبحث");
            }
            if (ModelState.IsValid)
            {
                TempData["StartDate"] = datesSearch.StartDate.Value.Date.ToString();


                //_context.Add(datesSearch);
                await _context.SaveChangesAsync();
                return RedirectToAction("CXTStatementAll", "AllBills");
            }
            return View(datesSearch);
        }
        public async Task<ActionResult> CXTStatementAll()
        {

            var ConcatBillEsal = (await (from x in _context.BillAirLines.Where(a => a.CustomerOrSupplier.Name == "مورد او وكيل").Where(d => d.BillDate.Date <= Convert.ToDateTime(TempData["StartDate"]).Date) select new { Name2 = x.CustomerSupplier.Name + " - " + x.Company.Company_Name, Credit = -x.CustomerPrice }).ToListAsync())
                             .Concat(await(from x in _context.BillVisas.Where(a => a.CustomerOrSupplier.Name == "مورد او وكيل").Where(d => d.BillDate.Date <= Convert.ToDateTime(TempData["StartDate"]).Date) select new { Name2 = x.CustomerSupplier.Name + " - " + x.Company.Company_Name, Credit = -x.CustomerPrice }).ToListAsync())
                             .Concat(await (from x in _context.BillDomestic.Where(a => a.CustomerOrSupplier.Name == "مورد او وكيل").Where(d => d.BillDate.Date <= Convert.ToDateTime(TempData["StartDate"]).Date) select new { Name2 = x.CustomerSupplier.Name + " - " + x.Company.Company_Name, Credit = -x.CustomerPrice }).ToListAsync())
                             .Concat(await (from x in _context.BillForeigns.Where(a => a.CustomerOrSupplier.Name == "مورد او وكيل").Where(d => d.BillDate.Date <= Convert.ToDateTime(TempData["StartDate"]).Date) select new { Name2 = x.CustomerSupplier.Name + " - " + x.Company.Company_Name, Credit = -x.CustomerPrice }).ToListAsync())
                             .Concat(await (from x in _context.Esals.Where(a => a.CustomerOrSupplier.Name == "مورد او وكيل").Where(d => d.EsalDate.Date <= Convert.ToDateTime(TempData["StartDate"]).Date) select new { Name2 = x.CustomerSupplier.Name + " - " + x.Company.Company_Name, Credit = x.AmountPaid }).ToListAsync())
                             .Concat(await (from x in _context.BillAirLines.Where(d => d.BillDate.Date <= Convert.ToDateTime(TempData["StartDate"]).Date) select new { Name2 = x.TicketExport.Name + " - " + x.Company.Company_Name, Credit = x.NetPrice }).ToListAsync())
                             .Concat(await (from x in _context.BillVisas.Where(d => d.BillDate.Date <= Convert.ToDateTime(TempData["StartDate"]).Date) select new { Name2 = x.TicketExport.Name + " - " + x.Company.Company_Name, Credit = x.NetPrice }).ToListAsync())
                             .Concat(await (from x in _context.BillDomestic.Where(d => d.BillDate.Date <= Convert.ToDateTime(TempData["StartDate"]).Date) select new { Name2 = x.TicketExport.Name + " - " + x.Company.Company_Name, Credit = x.NetPrice }).ToListAsync())
                             .Concat(await (from x in _context.BillForeigns.Where(d => d.BillDate.Date <= Convert.ToDateTime(TempData["StartDate"]).Date) select new { Name2 = x.TicketExport.Name + " - " + x.Company.Company_Name, Credit = x.NetPrice }).ToListAsync())
                             .Concat(await (from x in _context.Ezns.Where(a => a.MenuLE0Id != null).Where(a => a.CustomerOrSupplier.Name == "مورد او وكيل").Where(d => d.EznDate.Date <= Convert.ToDateTime(TempData["StartDate"]).Date) select new { Name2 = x.CustomerSupplier.Name + " - " + x.Company.Company_Name, Credit = -x.AmountWithdrawn }).ToListAsync())
                 .Concat(await (from x in _context.OpeningBalances.Where(a => a.StatementType.Name != "الخزينه") select new { Name2 = x.CustomerSupplier.Name + " - " + x.Company.Company_Name, Credit = x.Balance }).ToListAsync())
                 .OrderByDescending(p => p.Name2)
                 .GroupBy(x => x.Name2)
                 .Select(a =>
                     new
                     {
                         Name2 = a.Key,
                         Credit = a.Sum(x => x.Credit)
                     });

            var Outbut = (from x in ConcatBillEsal select new Models.ViewModel.Transactions { Statement = x.Name2, Credit = x.Credit }).OrderBy(d => d.Credit);


            var UserId = _userManager.GetUserId(User);
            var CompanyID =(await _userManager.Users.SingleOrDefaultAsync(a => a.Id == _userManager.GetUserId(User))).CompanyId;
            if (Outbut == null)
            {
                return NotFound();
            }
            ViewData["CompanyLogo"] = "~/" + TravelERP.Properties.Resources.ImgFolder + "/" +(await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyID)).CompanyLogo;
            ViewData["CompanyA4EsalImage"] = "~/" + TravelERP.Properties.Resources.ImgFolder + "/" +(await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyID)).CompanyA4EsalImage;
            ViewData["CompanyNameE"] =(await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyID)).Company_NameE;
            ViewData["CompanyName"] =(await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyID)).Company_Name;


            return View(Outbut);
        }


        //////////////////////////////////////////////////////////////////////////////////////
        /// AllBills/AirLinesStatementDate **** التقارير - تقرير موردين لكل الشركات **** ///
        public IActionResult AirLinesStatementDate()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AirLinesStatementDate([Bind("StartDate,EndDate,Name2")] DatesSearch datesSearch)
        {
            if (datesSearch.Name2 == 0)
            {
                ModelState.AddModelError("Name2", "من فضلك اختر اسم للبحث");
            }
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
                TempData["Name2"] = datesSearch.Name2;

                if (datesSearch.Name2 == 1)
                {
                    TempData["Name3"] = "طيران العربيه";
                }
                else if (datesSearch.Name2 == 2)
                {
                    TempData["Name3"] = "طيران الجزيره";

                }
                else if (datesSearch.Name2 == 3)
                {
                    TempData["Name3"] = "طيران فلاى دبى";

                }
                else if (datesSearch.Name2 == 4)
                {
                    TempData["Name3"] = "طيران فلاى ايجيبت";

                }
                else if (datesSearch.Name2 == 5)
                {
                    TempData["Name3"] = "Iata";

                }
                else if (datesSearch.Name2 == 6)
                {
                    TempData["Name3"] = "فهد مسقط";

                }
                else if (datesSearch.Name2 == 7)
                {
                    TempData["Name3"] = "مسقط المدهش";

                }
                else if (datesSearch.Name2 == 8)
                {
                    TempData["Name3"] = "مسقط ربا الاندلس";

                }
                else if (datesSearch.Name2 == 9)
                {
                    TempData["Name3"] = "مسقط الموسى";

                }
                else if (datesSearch.Name2 == 10)
                {
                    TempData["Name3"] = "الامارات";

                }


                //_context.Add(datesSearch);
                await _context.SaveChangesAsync();
                return RedirectToAction("AirLinesStatement", "AllBills");
            }
            return View(datesSearch);
        }
        public async Task<ActionResult> AirLinesStatement()
        {
            //StartTransactions
            var startBillAirLineEzn =await (from x in _context.BillAirLines select new Models.ViewModel.Transactions { EDate = x.BillDate, Debit = x.NetPrice, Name3 = x.TicketExport.Name }).Where(d => d.EDate < Convert.ToDateTime(TempData["StartDate"])).Where(d => d.Name3 == TempData["Name3"].ToString()).ToListAsync();
            var startBillAirLineEsal = await (from x in _context.BillAirLines select new Models.ViewModel.Transactions { EDate = x.BillDate, Credit = x.CustomerPrice, Name3 = x.CustomerSupplier.Name }).Where(d => d.EDate < Convert.ToDateTime(TempData["StartDate"])).Where(d => d.Name3 == TempData["Name3"].ToString()).ToListAsync();

            var startEsal = await (from x in _context.Esals.Where(a => a.MenuLE0.M0_Name == "طيران" || a.MenuLE0.M0_Name == "تأشيرات") select new Models.ViewModel.Transactions { EDate = x.EsalDate, Debit = x.AmountPaid, Name3 = x.CustomerSupplier.Name }).Where(d => d.EDate < Convert.ToDateTime(TempData["StartDate"])).Where(d => d.Name3 == TempData["Name3"].ToString()).ToListAsync();
            var startEzn = await (from x in _context.Ezns.Where(a => a.MenuLE0.M0_Name == "طيران" || a.MenuLE0.M0_Name == "تأشيرات") select new Models.ViewModel.Transactions { EDate = x.EznDate, Credit = x.AmountWithdrawn, Name3 = x.CustomerSupplier.Name }).Where(d => d.EDate < Convert.ToDateTime(TempData["StartDate"])).Where(d => d.Name3 == TempData["Name3"].ToString()).ToListAsync();
            ViewData["startTransactions"] = (startEsal.Sum(a => a.Debit) + (startBillAirLineEzn.Sum(a => a.Debit))) - (startEzn.Sum(a => a.Credit) + (startBillAirLineEsal.Sum(a => a.Credit)));
            ViewData["OpeningBalance"] =  (from x in _context.OpeningBalances select new { x.Balance, Name3 = x.CustomerSupplier.Name }).Where(d => d.Name3 == TempData["Name3"].ToString()).Sum(a => a.Balance);
            //For Head Name
            var billVisasEzn = await (from x in _context.BillVisas select new Models.ViewModel.Transactions { EId = x.BillId, Type = "ف تأشيرات", EDate = x.BillDate,ADate = x.ApprovedDate, Statement = string.Concat(x.MenuLE0.M0_Name, " - ", x.MenuLE1.M1_Name, " - ", x.MenuLE2.M2_Name, " - ", x.CustomerOrSupplier.Name, " - ", x.CustomerSupplier.Name, " - ", x.Commnets, " - ", x.Company.Company_Name, " - ", x.PassportNo), Debit = x.NetPrice, Name3 = x.TicketExport.Name }).Where(d => d.EDate >= Convert.ToDateTime(TempData["StartDate"])).Where(d => d.EDate.Date <= Convert.ToDateTime(TempData["EndDate"])).Where(d => d.Name3 == TempData["Name3"].ToString()).ToListAsync();
            var billVisasEsal = await (from x in _context.BillVisas select new Models.ViewModel.Transactions { EId = x.BillId, Type = "ف تأشيرات", EDate = x.BillDate,ADate = x.ApprovedDate, Statement = string.Concat(x.MenuLE0.M0_Name, " - ", x.MenuLE1.M1_Name, " - ", x.MenuLE2.M2_Name, " - ", x.CustomerOrSupplier.Name, " - ", x.CustomerSupplier.Name, " - ", x.Commnets, " - ", x.Company.Company_Name, " - " , x.PassportNo), Credit = x.CustomerPrice, Name3 = x.CustomerSupplier.Name }).Where(d => d.EDate >= Convert.ToDateTime(TempData["StartDate"])).Where(d => d.EDate.Date <= Convert.ToDateTime(TempData["EndDate"])).Where(d => d.Name3 == TempData["Name3"].ToString()).ToListAsync();

            var billAirLineEzn = await (from x in _context.BillAirLines select new Models.ViewModel.Transactions { EId = x.BillId, Type = "ف طيران", EDate = x.BillDate, Statement = string.Concat(x.MenuLE0.M0_Name, " - ", x.MenuLE1.M1_Name, " - ", x.MenuLE2.M2_Name, " - ", x.Direction, " - ", x.PNR, " - ", x.eTicketNumber, " - ", x.CustomerOrSupplier.Name, " - ", x.CustomerSupplier.Name, " - ", x.Commnets, " - ", x.Company.Company_Name), Debit = x.NetPrice, Name3 = x.TicketExport.Name }).Where(d => d.EDate >= Convert.ToDateTime(TempData["StartDate"])).Where(d => d.EDate.Date <= Convert.ToDateTime(TempData["EndDate"])).Where(d => d.Name3 == TempData["Name3"].ToString()).ToListAsync();
            var billAirLineEsal = await (from x in _context.BillAirLines select new Models.ViewModel.Transactions { EId = x.BillId, Type = "ف طيران", EDate = x.BillDate, Statement = string.Concat(x.MenuLE0.M0_Name, " - ", x.MenuLE1.M1_Name, " - ", x.MenuLE2.M2_Name, " - ", x.Direction, " - ", x.PNR, " - ", x.CustomerOrSupplier.Name, " - ", x.CustomerSupplier.Name, " - ", x.Commnets, " - ", x.Company.Company_Name), Credit = x.CustomerPrice, Name3 = x.CustomerSupplier.Name }).Where(d => d.EDate >= Convert.ToDateTime(TempData["StartDate"])).Where(d => d.EDate.Date <= Convert.ToDateTime(TempData["EndDate"])).Where(d => d.Name3 == TempData["Name3"].ToString()).ToListAsync();
            var esalAirLine = await (from x in _context.Esals.Where(a => a.MenuLE0.M0_Name == "طيران" || a.MenuLE0.M0_Name == "تأشيرات") select new Models.ViewModel.Transactions { EId = x.EsalId, Type = "إيصال", EDate = x.EsalDate, Statement = string.Concat(x.MenuLE0.M0_Name, " - ", x.MenuLE1.M1_Name, " - ", x.MenuLE2.M2_Name, " - ", x.DepositDesc, " - ", x.Company.Company_Name), Debit = x.AmountPaid, Name1 = x.CustomerOrSupplierId, Name3 = x.CustomerSupplier.Name }).Where(d => d.EDate >= Convert.ToDateTime(TempData["StartDate"])).Where(d => d.EDate.Date <= Convert.ToDateTime(TempData["EndDate"])).Where(d => d.Name3 == TempData["Name3"].ToString()).ToListAsync();
            var eznAirLine = await (from x in _context.Ezns.Where(a => a.MenuLE0.M0_Name == "طيران" || a.MenuLE0.M0_Name == "تأشيرات") select new Models.ViewModel.Transactions { EId = x.EznId, Type = "إذن صرف", EDate = x.EznDate, Statement = string.Concat(x.MenuLE0.M0_Name, " - ", x.ExpenseName, " - ", x.Company.Company_Name), Credit = x.AmountWithdrawn, Name1 = x.CustomerOrSupplierId, Name3 = x.CustomerSupplier.Name }).Where(d => d.EDate >= Convert.ToDateTime(TempData["StartDate"])).Where(d => d.EDate.Date <= Convert.ToDateTime(TempData["EndDate"])).Where(d => d.Name3 == TempData["Name3"].ToString()).ToListAsync();

            var Outbut = (billAirLineEsal.Concat(billAirLineEzn).Concat(esalAirLine).Concat(eznAirLine).Concat(billVisasEzn).Concat(billVisasEsal).OrderBy(a => a.EDate));

            var UserId = _userManager.GetUserId(User);
            var CompanyID =(await _userManager.Users.SingleOrDefaultAsync(a => a.Id == UserId)).CompanyId;
            if (Outbut == null)
            {
                return NotFound();
            }
            ViewData["CompanyLogo"] = "~/" + TravelERP.Properties.Resources.ImgFolder + "/" +(await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyID)).CompanyLogo;
            ViewData["CompanyA4EsalImage"] = "~/" + TravelERP.Properties.Resources.ImgFolder + "/" +(await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyID)).CompanyA4EsalImage;
            ViewData["CompanyNameE"] =(await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyID)).Company_NameE;
            ViewData["CompanyName"] =(await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyID)).Company_Name;


            return View( Outbut);
        }


        //////////////////////////////////////////////////////////////////////////////////////
        // AllBills/CXTStatementAllDate **** التقارير - تأشيرات البينديج للشركات**** //

        [Authorize(Roles = CustomRoles.Admin)]
        public IActionResult VisaPendingAllDate()
        {
            return View();
        }

        [Authorize(Roles = CustomRoles.Admin)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VisaPendingAllDate([Bind("StartDate", "EndDate")] DatesSearch datesSearch)
        {
            if (datesSearch.StartDate == null)
            {
                ModelState.AddModelError("StartDate", "من فضلك اختر تاريخ للبحث");
            }
            if (datesSearch.EndDate == null)
            {
                ModelState.AddModelError("EndDate", "من فضلك اختر تاريخ للبحث");
            }

            if (ModelState.IsValid)
            {
                TempData["StartDate"] = datesSearch.StartDate.Value.Date.ToString();
                TempData["EndDate"] = datesSearch.EndDate.Value.Date.ToString();

                //_context.Add(datesSearch);
                await _context.SaveChangesAsync();
                return RedirectToAction("VisaPendingAll", "AllBills");
            }
            return View(datesSearch);
        }

        [Authorize(Roles = CustomRoles.Admin)]
        public async Task<ActionResult> VisaPendingAll()
        {
            var UserId = _userManager.GetUserId(User);
            var ConcatBillEsal = await(from x in _context.BillVisas.Where(a=>a.BillState == false).Where(a => a.CustomerOrSupplier.Name == "مورد او وكيل").Where(d => d.BillDate.Date >= Convert.ToDateTime(TempData["StartDate"]).Date).Where(d => d.BillDate.Date <= Convert.ToDateTime(TempData["EndDate"])) select new { Name1 = x.CustomerSupplierId })
                 .GroupBy(x => x.Name1)
                 .Select(a =>
                     new
                     {
                         Name1 = a.Key,
                         Name2 = a.Count(),
                     }).ToListAsync();

            var Outbut = from x in ConcatBillEsal select new Models.ViewModel.BillCountViewModel { CustomerSupplierName = _context.CustomersSuppliers.SingleOrDefault(a => a.Id == x.Name1).Name, Count = x.Name2 , CompanyName = _context.Companies.SingleOrDefault(a=>a.Id == _context.CustomersSuppliers.SingleOrDefault(L => L.Id == x.Name1).CompanyId).Company_Name };


            var CompanyID =(await _userManager.Users.SingleOrDefaultAsync(a => a.Id == _userManager.GetUserId(User))).CompanyId;
            if (Outbut == null)
            {
                return NotFound();
            }
            ViewData["CompanyLogo"] = "~/" + TravelERP.Properties.Resources.ImgFolder + "/" +(await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyID)).CompanyLogo;
            ViewData["CompanyA4EsalImage"] = "~/" + TravelERP.Properties.Resources.ImgFolder + "/" +(await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyID)).CompanyA4EsalImage;
            ViewData["CompanyNameE"] =(await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyID)).Company_NameE;
            ViewData["CompanyName"] =(await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyID)).Company_Name;


            return View(Outbut);
        }

        //////////////////////////////////////////////////////////////////////////////////////
        // AllBills/CXTStatementAllDate **** التقارير - تأشيرات الابروف للشركات**** //

        [Authorize(Roles = CustomRoles.Admin)]
        public IActionResult VisaApprovedAllDate()
        {
            return View();
        }

        [Authorize(Roles = CustomRoles.Admin)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VisaApprovedAllDate([Bind("StartDate", "EndDate")] DatesSearch datesSearch)
        {
            if (datesSearch.StartDate == null)
            {
                ModelState.AddModelError("StartDate", "من فضلك اختر تاريخ للبحث");
            }
            if (datesSearch.EndDate == null)
            {
                ModelState.AddModelError("EndDate", "من فضلك اختر تاريخ للبحث");
            }

            if (ModelState.IsValid)
            {
                TempData["StartDate"] = datesSearch.StartDate.Value.Date.ToString();
                TempData["EndDate"] = datesSearch.EndDate.Value.Date.ToString();

                //_context.Add(datesSearch);
                await _context.SaveChangesAsync();
                return RedirectToAction("VisaApprovedAll", "AllBills");
            }
            return View(datesSearch);
        }

        [Authorize(Roles = CustomRoles.Admin)]
        public async Task<ActionResult> VisaApprovedAll()
        {
            var UserId = _userManager.GetUserId(User);
            var ConcatBillEsal = await (from x in _context.BillVisas.Where(a => a.BillState == true).Where(a => a.CustomerOrSupplier.Name == "مورد او وكيل").Where(d => d.BillDate.Date >= Convert.ToDateTime(TempData["StartDate"]).Date).Where(d => d.BillDate.Date <= Convert.ToDateTime(TempData["EndDate"])) select new { Name1 = x.CustomerSupplierId })
                 .GroupBy(x => x.Name1)
                 .Select(a =>
                     new
                     {
                         Name1 = a.Key,
                         Name2 = a.Count(),
                     }).ToListAsync();

            var Outbut = from x in ConcatBillEsal select new Models.ViewModel.BillCountViewModel { CustomerSupplierName = _context.CustomersSuppliers.SingleOrDefault(a => a.Id == x.Name1).Name, Count = x.Name2, CompanyName = _context.Companies.SingleOrDefault(a => a.Id == _context.CustomersSuppliers.SingleOrDefault(L => L.Id == x.Name1).CompanyId).Company_Name };


            var CompanyID =(await _userManager.Users.SingleOrDefaultAsync(a => a.Id == _userManager.GetUserId(User))).CompanyId;
            if (Outbut == null)
            {
                return NotFound();
            }
            ViewData["CompanyLogo"] = "~/" + TravelERP.Properties.Resources.ImgFolder + "/" +(await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyID)).CompanyLogo;
            ViewData["CompanyA4EsalImage"] = "~/" + TravelERP.Properties.Resources.ImgFolder + "/" +(await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyID)).CompanyA4EsalImage;
            ViewData["CompanyNameE"] =(await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyID)).Company_NameE;
            ViewData["CompanyName"] =(await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyID)).Company_Name;


            return View(Outbut);
        }


        //////////////////////////////////////////////////////////////////////////////////////
        // AllBills/CXTStatementAllDate **** التقارير - تأشيرات البينديج للشركه **** //

        [Authorize(Roles = CustomRoles.BranchManager)]
        public IActionResult VisaPendingCompanyDate()
        {
            return View();
        }

        [Authorize(Roles = CustomRoles.BranchManager)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VisaPendingCompanyDate([Bind("StartDate", "EndDate")] DatesSearch datesSearch)
        {
            if (datesSearch.StartDate == null)
            {
                ModelState.AddModelError("StartDate", "من فضلك اختر تاريخ للبحث");
            }
            if (datesSearch.EndDate == null)
            {
                ModelState.AddModelError("EndDate", "من فضلك اختر تاريخ للبحث");
            }

            if (ModelState.IsValid)
            {
                TempData["StartDate"] = datesSearch.StartDate.Value.Date.ToString();
                TempData["EndDate"] = datesSearch.EndDate.Value.Date.ToString();

                //_context.Add(datesSearch);
                await _context.SaveChangesAsync();
                return RedirectToAction("VisaPendingCompany", "AllBills");
            }
            return View(datesSearch);
        }

        [Authorize(Roles = CustomRoles.BranchManager)]
        public async Task<ActionResult> VisaPendingCompany()
        {
            var UserId = _userManager.GetUserId(User);
            var CompanyID =(await _userManager.Users.SingleOrDefaultAsync(a => a.Id == _userManager.GetUserId(User))).CompanyId;
            var ConcatBillEsal = await (from x in _context.BillVisas.Where(a=>a.CompanyID == CompanyID).Where(a => a.BillState == false).Where(a => a.CustomerOrSupplier.Name == "مورد او وكيل").Where(d => d.BillDate.Date >= Convert.ToDateTime(TempData["StartDate"]).Date).Where(d => d.BillDate.Date <= Convert.ToDateTime(TempData["EndDate"])) select new { Name1 = x.CustomerSupplierId })
                 .GroupBy(x => x.Name1)
                 .Select(a =>
                     new
                     {
                         Name1 = a.Key,
                         Name2 = a.Count(),
                     }).ToListAsync();

            var Outbut = from x in ConcatBillEsal select new Models.ViewModel.BillCountViewModel { CustomerSupplierName = _context.CustomersSuppliers.SingleOrDefault(a => a.Id == x.Name1).Name, Count = x.Name2, CompanyName = _context.Companies.SingleOrDefault(a => a.Id == _context.CustomersSuppliers.SingleOrDefault(L => L.Id == x.Name1).CompanyId).Company_Name };
            if (Outbut == null)
            {
                return NotFound();
            }
            ViewData["CompanyLogo"] = "~/" + TravelERP.Properties.Resources.ImgFolder + "/" +(await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyID)).CompanyLogo;
            ViewData["CompanyA4EsalImage"] = "~/" + TravelERP.Properties.Resources.ImgFolder + "/" +(await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyID)).CompanyA4EsalImage;
            ViewData["CompanyNameE"] =(await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyID)).Company_NameE;
            ViewData["CompanyName"] =(await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyID)).Company_Name;


            return View(Outbut);
        }



        //////////////////////////////////////////////////////////////////////////////////////
        // AllBills/CXTStatementAllDate **** التقارير - تأشيرات البينديج للموظف **** //

        public IActionResult VisaPendingUserDate()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VisaPendingUserDate([Bind("StartDate", "EndDate")] DatesSearch datesSearch)
        {
            if (datesSearch.StartDate == null)
            {
                ModelState.AddModelError("StartDate", "من فضلك اختر تاريخ للبحث");
            }
            if (datesSearch.EndDate == null)
            {
                ModelState.AddModelError("EndDate", "من فضلك اختر تاريخ للبحث");
            }

            if (ModelState.IsValid)
            {
                TempData["StartDate"] = datesSearch.StartDate.Value.Date.ToString();
                TempData["EndDate"] = datesSearch.EndDate.Value.Date.ToString();

                //_context.Add(datesSearch);
                await _context.SaveChangesAsync();
                return RedirectToAction("VisaPendingUser", "AllBills");
            }
            return View(datesSearch);
        }

        public async Task<ActionResult> VisaPendingUser()
        {
            var UserId = _userManager.GetUserId(User);
            var CompanyID =(await _userManager.Users.SingleOrDefaultAsync(a => a.Id == _userManager.GetUserId(User))).CompanyId;
            var ConcatBillEsal = await (from x in _context.BillVisas.Where(a=>a.UserId == UserId).Where(a => a.BillState == false).Where(a => a.CustomerOrSupplier.Name == "مورد او وكيل").Where(d => d.BillDate.Date >= Convert.ToDateTime(TempData["StartDate"]).Date).Where(d => d.BillDate.Date <= Convert.ToDateTime(TempData["EndDate"])) select new { Name1 = x.CustomerSupplierId })
                 .GroupBy(x => x.Name1)
                 .Select(a =>
                     new
                     {
                         Name1 = a.Key,
                         Name2 = a.Count(),
                     }).ToListAsync();

            var Outbut = from x in ConcatBillEsal select new Models.ViewModel.BillCountViewModel { CustomerSupplierName = _context.CustomersSuppliers.SingleOrDefault(a => a.Id == x.Name1).Name, Count = x.Name2, CompanyName = _context.Companies.SingleOrDefault(a => a.Id == _context.CustomersSuppliers.SingleOrDefault(L => L.Id == x.Name1).CompanyId).Company_Name };
            if (Outbut == null)
            {
                return NotFound();
            }
            ViewData["CompanyLogo"] = "~/" + TravelERP.Properties.Resources.ImgFolder + "/" +(await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyID)).CompanyLogo;
            ViewData["CompanyA4EsalImage"] = "~/" + TravelERP.Properties.Resources.ImgFolder + "/" +(await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyID)).CompanyA4EsalImage;
            ViewData["CompanyNameE"] =(await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyID)).Company_NameE;
            ViewData["CompanyName"] =(await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyID)).Company_Name;


            return View(Outbut);
        }

        //////////////////////////////////////////////////////////////////////////////////////
        // AllBills/MenuL1Date **** التقارير - تقرير قوائم 1 **** //

        public IActionResult MenuLE1Date()
        {

            ViewData["MenuLE0Id"] = new SelectList(_context.MenuLE0.Where(a => a.M0_Name != "سياحة دينية"), "Id", "M0_Name");
            return View();

        }

        [HttpPost]
        public async Task<IActionResult> MenuLE1Date([Bind("Name1,Name2")] DatesSearch datesSearch)
        {
            if (datesSearch.Name1 == 0)
            {
                ModelState.AddModelError("Name1", "من فضلك اختر اسم قائمه رئيسيه");
            }
            if (datesSearch.Name2 == 0)
            {
                ModelState.AddModelError("Name2", "من فضلك اختر اسم قائمه منسدله 1");
            }
            //if (datesSearch.StartDate == null)
            //{
            //    ModelState.AddModelError("StartDate", "من فضلك اختر تاريخ من للبحث");
            //}
            //if (datesSearch.EndDate == null)
            //{
            //    ModelState.AddModelError("EndDate", "من فضلك اختر تاريخ الى للبحث");
            //}


            if (ModelState.IsValid)
            {
                //TempData["StartDate"] = datesSearch.StartDate.Value.Date.ToString();
                //TempData["EndDate"] = datesSearch.EndDate.Value.Date.ToString();
                TempData["Name1"] = datesSearch.Name1;
                TempData["Name2"] = datesSearch.Name2;


                //_context.Add(datesSearch);
                await _context.SaveChangesAsync();
                return RedirectToAction("MenuLE1Report", "AllBills");
            }
            ViewData["MenuLE0Id"] = new SelectList(_context.MenuLE0.Where(a => a.M0_Name != "سياحة دينية"), "Id", "M0_Name",datesSearch.Name1);
            ViewData["MenuLE1Id"] = new SelectList(_context.MenuLE1.Where(a => a.MenuLE0.M0_Name != "سياحة دينية").Where(a => a.MenuLE0Id == datesSearch.Name1), "Id", "M0_Name", datesSearch.Name2);

            return View(datesSearch);
        }


        public async Task<ActionResult> MenuLE1Report()
        {
            var CompanyId = (await _userManager.Users.SingleOrDefaultAsync(a => a.Id == _userManager.GetUserId(User))).CompanyId;

            var MenuLE0Name = (await _context.MenuLE0.SingleOrDefaultAsync(a => a.Id == Convert.ToInt32(TempData["Name1"]))).M0_Name;
            var MenuLE1Id = Convert.ToInt32(TempData["Name2"]);
            var MenuLE1Name = (await _context.MenuLE1.SingleOrDefaultAsync(a => a.Id == MenuLE1Id)).M1_Name;

            if ( MenuLE0Name == "سياحة داخلية")
            {
                var billsNetPrices =await _context.BillDomestic.Where(x => x.MenuLE1Id == MenuLE1Id).SumAsync(a=>a.NetPrice);
                var billsCustomersPrices = await _context.BillDomestic.Where(x => x.MenuLE1Id == MenuLE1Id).SumAsync(a => a.CustomerPrice);
                var ezns = await (from x in _context.Ezns.Where(A => A.CompanyID == CompanyId).Where(x => x.MenuLE1Id == MenuLE1Id) select new Models.ViewModel.Transactions { EId = x.EznId, EDate = x.EznDate, Statement = string.Concat(x.Name, x.CustomerOrSupplier.Name, " - " + x.CustomerSupplier.Name, " - " + x.ExpenseName, " - " + x.MenuLE0.M0_Name, " - " + x.MenuLE1.M1_Name, " - " + x.MenuLE2.M2_Name, " - " + x.PaymentMethod.Name, " - " + x.User.UserName), Credit = x.AmountWithdrawn, Type = x.PaymentMethod.Name }).ToListAsync();
                var esals = await (from x in _context.Esals.Where(A => A.CompanyID == CompanyId).Where(x => x.MenuLE1Id == MenuLE1Id) select new Models.ViewModel.Transactions { EId = x.EsalId, EDate = x.EsalDate, Statement = string.Concat(x.CustomerOrSupplier.Name, " - " + x.CustomerSupplier.Name, " - " + x.MenuLE0.M0_Name, " - " + x.MenuLE1.M1_Name, " - " + x.MenuLE2.M2_Name, " - " + x.DepositDesc, " - " + x.PaymentMethod.Name, " - " + x.User.UserName), Debit = x.AmountPaid, Type = x.PaymentMethod.Name }).ToListAsync();
                var All = (ezns.Concat(esals)).OrderBy(a => a.EDate);
                ViewData["HeaderName"] = MenuLE0Name + " - " + MenuLE1Name;
                ViewData["billsNetPrices"] = billsNetPrices;
                ViewData["billsCustomersPrices"] = billsCustomersPrices;
                if (All == null)
                {
                    return NotFound();
                }
                return View(All);
            }
            if (MenuLE0Name == "سياحة خارجية")
            {
                var billsNetPrices = await _context.BillForeigns.Where(x => x.MenuLE1Id == MenuLE1Id).SumAsync(a => a.NetPrice);
                var billsCustomersPrices = await _context.BillForeigns.Where(x => x.MenuLE1Id == MenuLE1Id).SumAsync(a => a.CustomerPrice);
                var ezns = await (from x in _context.Ezns.Where(A => A.CompanyID == CompanyId).Where(x => x.MenuLE1Id == MenuLE1Id) select new Models.ViewModel.Transactions { EId = x.EznId, EDate = x.EznDate, Statement = string.Concat(x.Name, x.CustomerOrSupplier.Name, " - " + x.CustomerSupplier.Name, " - " + x.ExpenseName, " - " + x.MenuLE0.M0_Name, " - " + x.MenuLE1.M1_Name, " - " + x.MenuLE2.M2_Name, " - " + x.PaymentMethod.Name, " - " + x.User.UserName), Credit = x.AmountWithdrawn, Type = x.PaymentMethod.Name }).ToListAsync();
                var esals = await (from x in _context.Esals.Where(A => A.CompanyID == CompanyId).Where(x => x.MenuLE1Id == MenuLE1Id) select new Models.ViewModel.Transactions { EId = x.EsalId, EDate = x.EsalDate, Statement = string.Concat(x.CustomerOrSupplier.Name, " - " + x.CustomerSupplier.Name, " - " + x.MenuLE0.M0_Name, " - " + x.MenuLE1.M1_Name, " - " + x.MenuLE2.M2_Name, " - " + x.DepositDesc, " - " + x.PaymentMethod.Name, " - " + x.User.UserName), Debit = x.AmountPaid, Type = x.PaymentMethod.Name }).ToListAsync();
                var All = (ezns.Concat(esals)).OrderBy(a => a.EDate);
                ViewData["HeaderName"] = MenuLE0Name + " - " + MenuLE1Name;
                ViewData["billsNetPrices"] = billsNetPrices;
                ViewData["billsCustomersPrices"] = billsCustomersPrices;
                if (All == null)
                {
                    return NotFound();
                }
                return View(All);
            }
            if (MenuLE0Name == "تأشيرات")
            {
                var billsNetPrices = await _context.BillVisas.Where(x => x.MenuLE1Id == MenuLE1Id).SumAsync(a => a.NetPrice);
                var billsCustomersPrices = await _context.BillVisas.Where(x => x.MenuLE1Id == MenuLE1Id).SumAsync(a => a.CustomerPrice);
                var ezns = await (from x in _context.Ezns.Where(A => A.CompanyID == CompanyId).Where(x => x.MenuLE1Id == MenuLE1Id) select new Models.ViewModel.Transactions { EId = x.EznId, EDate = x.EznDate, Statement = string.Concat(x.Name, x.CustomerOrSupplier.Name, " - " + x.CustomerSupplier.Name, " - " + x.ExpenseName, " - " + x.MenuLE0.M0_Name, " - " + x.MenuLE1.M1_Name, " - " + x.MenuLE2.M2_Name, " - " + x.PaymentMethod.Name, " - " + x.User.UserName), Credit = x.AmountWithdrawn, Type = x.PaymentMethod.Name }).ToListAsync();
                var esals = await (from x in _context.Esals.Where(A => A.CompanyID == CompanyId).Where(x => x.MenuLE1Id == MenuLE1Id) select new Models.ViewModel.Transactions { EId = x.EsalId, EDate = x.EsalDate, Statement = string.Concat(x.CustomerOrSupplier.Name, " - " + x.CustomerSupplier.Name, " - " + x.MenuLE0.M0_Name, " - " + x.MenuLE1.M1_Name, " - " + x.MenuLE2.M2_Name, " - " + x.DepositDesc, " - " + x.PaymentMethod.Name, " - " + x.User.UserName), Debit = x.AmountPaid, Type = x.PaymentMethod.Name }).ToListAsync();
                var All = (ezns.Concat(esals)).OrderBy(a => a.EDate);
                ViewData["HeaderName"] = MenuLE0Name + " - " + MenuLE1Name;
                ViewData["billsNetPrices"] = billsNetPrices;
                ViewData["billsCustomersPrices"] = billsCustomersPrices;
                if (All == null)
                {
                    return NotFound();
                }
                return View(All);
            }
            if (MenuLE0Name == "طيران")
            {
                var billsNetPrices = await _context.BillAirLines.Where(x => x.MenuLE1Id == MenuLE1Id).SumAsync(a => a.NetPrice);
                var billsCustomersPrices = await _context.BillAirLines.Where(x => x.MenuLE1Id == MenuLE1Id).SumAsync(a => a.CustomerPrice);
                var ezns = await (from x in _context.Ezns.Where(A => A.CompanyID == CompanyId).Where(x => x.MenuLE1Id == MenuLE1Id) select new Models.ViewModel.Transactions { EId = x.EznId, EDate = x.EznDate, Statement = string.Concat(x.Name, x.CustomerOrSupplier.Name, " - " + x.CustomerSupplier.Name, " - " + x.ExpenseName, " - " + x.MenuLE0.M0_Name, " - " + x.MenuLE1.M1_Name, " - " + x.MenuLE2.M2_Name, " - " + x.PaymentMethod.Name, " - " + x.User.UserName), Credit = x.AmountWithdrawn, Type = x.PaymentMethod.Name }).ToListAsync();
                var esals = await (from x in _context.Esals.Where(A => A.CompanyID == CompanyId).Where(x => x.MenuLE1Id == MenuLE1Id) select new Models.ViewModel.Transactions { EId = x.EsalId, EDate = x.EsalDate, Statement = string.Concat(x.CustomerOrSupplier.Name, " - " + x.CustomerSupplier.Name, " - " + x.MenuLE0.M0_Name, " - " + x.MenuLE1.M1_Name, " - " + x.MenuLE2.M2_Name, " - " + x.DepositDesc, " - " + x.PaymentMethod.Name, " - " + x.User.UserName), Debit = x.AmountPaid, Type = x.PaymentMethod.Name }).ToListAsync();
                var All = (ezns.Concat(esals)).OrderBy(a => a.EDate);
                ViewData["HeaderName"] = MenuLE0Name + " - " + MenuLE1Name;
                ViewData["billsNetPrices"] = billsNetPrices;
                ViewData["billsCustomersPrices"] = billsCustomersPrices;
                if (All == null)
                {
                    return NotFound();
                }
                return View(All);
            }
            ViewData["CompanyId"] = CompanyId;
            ViewData["CompanyLogo"] = "~/" + TravelERP.Properties.Resources.ImgFolder + "/" + _context.Companies.SingleOrDefault(a => a.Id == CompanyId).CompanyLogo;
            ViewData["CompanyA4EsalImage"] = "~/" + TravelERP.Properties.Resources.ImgFolder + "/" + _context.Companies.SingleOrDefault(a => a.Id == CompanyId).CompanyA4EsalImage;
            ViewData["CompanyNameE"] = (await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyId)).Company_NameE;
            ViewData["CompanyName"] = (await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyId)).Company_Name;
            return View();

        }

        //////////////////////////////////////////////////////////////////////////////////////
        // AllBills/MenuL1Date **** التقارير - تقرير قوائم 2 **** //

        public IActionResult MenuLE2Date()
        {

            ViewData["MenuLE0Id"] = new SelectList(_context.MenuLE0.Where(a => a.M0_Name != "سياحة دينية"), "Id", "M0_Name");
            return View();

        }

        [HttpPost]
        public async Task<IActionResult> MenuLE2Date([Bind("Name1,Name2,Name3")] DatesSearch datesSearch)
        {
            if (datesSearch.Name1 == 0)
            {
                ModelState.AddModelError("Name1", "من فضلك اختر اسم قائمه رئيسيه");
            }
            if (datesSearch.Name2 == 0)
            {
                ModelState.AddModelError("Name2", "من فضلك اختر اسم قائمه منسدله 1");
            }
            if (datesSearch.Name3 == 0)
            {
                ModelState.AddModelError("Name3", "من فضلك اختر اسم قائمه منسدله 2");
            }

            //if (datesSearch.StartDate == null)
            //{
            //    ModelState.AddModelError("StartDate", "من فضلك اختر تاريخ من للبحث");
            //}
            //if (datesSearch.EndDate == null)
            //{
            //    ModelState.AddModelError("EndDate", "من فضلك اختر تاريخ الى للبحث");
            //}


            if (ModelState.IsValid)
            {
                //TempData["StartDate"] = datesSearch.StartDate.Value.Date.ToString();
                //TempData["EndDate"] = datesSearch.EndDate.Value.Date.ToString();
                TempData["Name1"] = datesSearch.Name1;
                TempData["Name2"] = datesSearch.Name2;
                TempData["Name3"] = datesSearch.Name3;
                //_context.Add(datesSearch);
                await _context.SaveChangesAsync();
                return RedirectToAction("MenuLE2Report", "AllBills");
            }
            ViewData["MenuLE0Id"] = new SelectList(_context.MenuLE0.Where(a => a.M0_Name != "سياحة دينية"), "Id", "M0_Name", datesSearch.Name1);
            ViewData["MenuLE1Id"] = new SelectList(_context.MenuLE1.Where(a => a.MenuLE0.M0_Name != "سياحة دينية").Where(a => a.MenuLE0Id == datesSearch.Name1), "Id", "M0_Name", datesSearch.Name2);
            ViewData["MenuLE2Id"] = new SelectList(_context.MenuLE2.Where(a => a.MenuLE0.M0_Name != "سياحة دينية").Where(a => a.MenuLE1Id == datesSearch.Name2), "Id", "M0_Name", datesSearch.Name3);

            return View(datesSearch);
        }


        public async Task<ActionResult> MenuLE2Report()
        {
            var CompanyId = (await _userManager.Users.SingleOrDefaultAsync(a => a.Id == _userManager.GetUserId(User))).CompanyId;

            var MenuLE0Name = (await _context.MenuLE0.SingleOrDefaultAsync(a => a.Id == Convert.ToInt32(TempData["Name1"]))).M0_Name;
            var MenuLE1Id = Convert.ToInt32(TempData["Name2"]);
            var MenuLE2Id = Convert.ToInt32(TempData["Name3"]);
            var MenuLE1Name = (await _context.MenuLE1.SingleOrDefaultAsync(a => a.Id == MenuLE1Id)).M1_Name;
            var MenuLE2Name = (await _context.MenuLE2.SingleOrDefaultAsync(a => a.Id == MenuLE2Id)).M2_Name;

            if (MenuLE0Name == "سياحة داخلية")
            {
                var billsNetPrices = await _context.BillDomestic.Where(x => x.MenuLE2Id == MenuLE2Id).SumAsync(a => a.NetPrice);
                var billsCustomersPrices = await _context.BillDomestic.Where(x => x.MenuLE2Id == MenuLE2Id).SumAsync(a => a.CustomerPrice);
                var billsChildCount = await _context.BillDomestic.Where(x => x.MenuLE2Id == MenuLE2Id).SumAsync(a => a.ChildN);
                var billsAdultCount = await _context.BillDomestic.Where(x => x.MenuLE2Id == MenuLE2Id).SumAsync(a => a.AdultN);

                var ezns = await (from x in _context.Ezns.Where(A => A.CompanyID == CompanyId).Where(x => x.MenuLE2Id == MenuLE2Id) select new Models.ViewModel.Transactions { EId = x.EznId, EDate = x.EznDate, Statement = string.Concat(x.Name, x.CustomerOrSupplier.Name, " - " + x.CustomerSupplier.Name, " - " + x.ExpenseName, " - " + x.MenuLE0.M0_Name, " - " + x.MenuLE1.M1_Name, " - " + x.MenuLE2.M2_Name, " - " + x.PaymentMethod.Name, " - " + x.User.UserName), Credit = x.AmountWithdrawn, Type = x.PaymentMethod.Name }).ToListAsync();
                var esals = await (from x in _context.Esals.Where(A => A.CompanyID == CompanyId).Where(x => x.MenuLE2Id == MenuLE2Id) select new Models.ViewModel.Transactions { EId = x.EsalId, EDate = x.EsalDate, Statement = string.Concat(x.CustomerOrSupplier.Name, " - " + x.CustomerSupplier.Name, " - " + x.MenuLE0.M0_Name, " - " + x.MenuLE1.M1_Name, " - " + x.MenuLE2.M2_Name, " - " + x.DepositDesc, " - " + x.PaymentMethod.Name, " - " + x.User.UserName), Debit = x.AmountPaid, Type = x.PaymentMethod.Name }).ToListAsync();
                var All = (ezns.Concat(esals)).OrderBy(a => a.EDate);
                ViewData["HeaderName"] = MenuLE0Name + " - " + MenuLE1Name + " - " + MenuLE2Name;
                ViewData["billsNetPrices"] = billsNetPrices;
                ViewData["billsCustomersPrices"] = billsCustomersPrices;
                ViewData["billsChildCount"] = billsChildCount;
                ViewData["billsAdultCount"] = billsAdultCount;

                if (All == null)
                {
                    return NotFound();
                }
                return View(All);
            }
            if (MenuLE0Name == "سياحة خارجية")
            {
                var billsNetPrices = await _context.BillForeigns.Where(x => x.MenuLE2Id == MenuLE2Id).SumAsync(a => a.NetPrice);
                var billsCustomersPrices = await _context.BillForeigns.Where(x => x.MenuLE2Id == MenuLE2Id).SumAsync(a => a.CustomerPrice);
                var billsChildCount = await _context.BillForeigns.Where(x => x.MenuLE2Id == MenuLE2Id).SumAsync(a => a.ChildN);
                var billsAdultCount = await _context.BillForeigns.Where(x => x.MenuLE2Id == MenuLE2Id).SumAsync(a => a.AdultN);

                var ezns = await (from x in _context.Ezns.Where(A => A.CompanyID == CompanyId).Where(x => x.MenuLE2Id == MenuLE2Id) select new Models.ViewModel.Transactions { EId = x.EznId, EDate = x.EznDate, Statement = string.Concat(x.Name, x.CustomerOrSupplier.Name, " - " + x.CustomerSupplier.Name, " - " + x.ExpenseName, " - " + x.MenuLE0.M0_Name, " - " + x.MenuLE1.M1_Name, " - " + x.MenuLE2.M2_Name, " - " + x.PaymentMethod.Name, " - " + x.User.UserName), Credit = x.AmountWithdrawn, Type = x.PaymentMethod.Name }).ToListAsync();
                var esals = await (from x in _context.Esals.Where(A => A.CompanyID == CompanyId).Where(x => x.MenuLE2Id == MenuLE2Id) select new Models.ViewModel.Transactions { EId = x.EsalId, EDate = x.EsalDate, Statement = string.Concat(x.CustomerOrSupplier.Name, " - " + x.CustomerSupplier.Name, " - " + x.MenuLE0.M0_Name, " - " + x.MenuLE1.M1_Name, " - " + x.MenuLE2.M2_Name, " - " + x.DepositDesc, " - " + x.PaymentMethod.Name, " - " + x.User.UserName), Debit = x.AmountPaid, Type = x.PaymentMethod.Name }).ToListAsync();
                var All = (ezns.Concat(esals)).OrderBy(a => a.EDate);
                ViewData["HeaderName"] = MenuLE0Name + " - " + MenuLE1Name + " - " + MenuLE2Name;
                ViewData["billsNetPrices"] = billsNetPrices;
                ViewData["billsCustomersPrices"] = billsCustomersPrices;
                ViewData["billsChildCount"] = billsChildCount;
                ViewData["billsAdultCount"] = billsAdultCount;

                if (All == null)
                {
                    return NotFound();
                }
                return View(All);
            }
            if (MenuLE0Name == "تأشيرات")
            {
                var billsNetPrices = await _context.BillVisas.Where(x => x.MenuLE2Id == MenuLE2Id).SumAsync(a => a.NetPrice);
                var billsCustomersPrices = await _context.BillVisas.Where(x => x.MenuLE2Id == MenuLE2Id).SumAsync(a => a.CustomerPrice);
                var billsChildCount = await _context.BillVisas.Where(x => x.MenuLE2Id == MenuLE2Id).SumAsync(a => a.ChildN);
                var billsAdultCount = await _context.BillVisas.Where(x => x.MenuLE2Id == MenuLE2Id).SumAsync(a => a.AdultN);

                var ezns = await (from x in _context.Ezns.Where(A => A.CompanyID == CompanyId).Where(x => x.MenuLE2Id == MenuLE2Id) select new Models.ViewModel.Transactions { EId = x.EznId, EDate = x.EznDate, Statement = string.Concat(x.Name, x.CustomerOrSupplier.Name, " - " + x.CustomerSupplier.Name, " - " + x.ExpenseName, " - " + x.MenuLE0.M0_Name, " - " + x.MenuLE1.M1_Name, " - " + x.MenuLE2.M2_Name, " - " + x.PaymentMethod.Name, " - " + x.User.UserName), Credit = x.AmountWithdrawn, Type = x.PaymentMethod.Name }).ToListAsync();
                var esals = await (from x in _context.Esals.Where(A => A.CompanyID == CompanyId).Where(x => x.MenuLE2Id == MenuLE2Id) select new Models.ViewModel.Transactions { EId = x.EsalId, EDate = x.EsalDate, Statement = string.Concat(x.CustomerOrSupplier.Name, " - " + x.CustomerSupplier.Name, " - " + x.MenuLE0.M0_Name, " - " + x.MenuLE1.M1_Name, " - " + x.MenuLE2.M2_Name, " - " + x.DepositDesc, " - " + x.PaymentMethod.Name, " - " + x.User.UserName), Debit = x.AmountPaid, Type = x.PaymentMethod.Name }).ToListAsync();
                var All = (ezns.Concat(esals)).OrderBy(a => a.EDate);
                ViewData["HeaderName"] = MenuLE0Name + " - " + MenuLE1Name + " - " + MenuLE2Name;
                ViewData["billsNetPrices"] = billsNetPrices;
                ViewData["billsCustomersPrices"] = billsCustomersPrices;
                ViewData["billsChildCount"] = billsChildCount;
                ViewData["billsAdultCount"] = billsAdultCount;

                if (All == null)
                {
                    return NotFound();
                }
                return View(All);
            }
            if (MenuLE0Name == "طيران")
            {
                var billsNetPrices = await _context.BillAirLines.Where(x => x.MenuLE2Id == MenuLE2Id).SumAsync(a => a.NetPrice);
                var billsCustomersPrices = await _context.BillAirLines.Where(x => x.MenuLE2Id == MenuLE2Id).SumAsync(a => a.CustomerPrice);
                var billsChildCount = await _context.BillAirLines.Where(x => x.MenuLE2Id == MenuLE2Id).SumAsync(a => a.ChildN);
                var billsAdultCount = await _context.BillAirLines.Where(x => x.MenuLE2Id == MenuLE2Id).SumAsync(a => a.AdultN);

                var ezns = await (from x in _context.Ezns.Where(A => A.CompanyID == CompanyId).Where(x => x.MenuLE2Id == MenuLE2Id) select new Models.ViewModel.Transactions { EId = x.EznId, EDate = x.EznDate, Statement = string.Concat(x.Name, x.CustomerOrSupplier.Name, " - " + x.CustomerSupplier.Name, " - " + x.ExpenseName, " - " + x.MenuLE0.M0_Name, " - " + x.MenuLE1.M1_Name, " - " + x.MenuLE2.M2_Name, " - " + x.PaymentMethod.Name, " - " + x.User.UserName), Credit = x.AmountWithdrawn, Type = x.PaymentMethod.Name }).ToListAsync();
                var esals = await (from x in _context.Esals.Where(A => A.CompanyID == CompanyId).Where(x => x.MenuLE2Id == MenuLE2Id) select new Models.ViewModel.Transactions { EId = x.EsalId, EDate = x.EsalDate, Statement = string.Concat(x.CustomerOrSupplier.Name, " - " + x.CustomerSupplier.Name, " - " + x.MenuLE0.M0_Name, " - " + x.MenuLE1.M1_Name, " - " + x.MenuLE2.M2_Name, " - " + x.DepositDesc, " - " + x.PaymentMethod.Name, " - " + x.User.UserName), Debit = x.AmountPaid, Type = x.PaymentMethod.Name }).ToListAsync();
                var All = (ezns.Concat(esals)).OrderBy(a => a.EDate);
                ViewData["HeaderName"] = MenuLE0Name + " - " + MenuLE1Name + " - " + MenuLE2Name;
                ViewData["billsNetPrices"] = billsNetPrices;
                ViewData["billsCustomersPrices"] = billsCustomersPrices;
                ViewData["billsChildCount"] = billsChildCount;
                ViewData["billsAdultCount"] = billsAdultCount;

                if (All == null)
                {
                    return NotFound();
                }
                return View(All);
            }
            ViewData["CompanyId"] = CompanyId;
            ViewData["CompanyLogo"] = "~/" + TravelERP.Properties.Resources.ImgFolder + "/" + _context.Companies.SingleOrDefault(a => a.Id == CompanyId).CompanyLogo;
            ViewData["CompanyA4EsalImage"] = "~/" + TravelERP.Properties.Resources.ImgFolder + "/" + _context.Companies.SingleOrDefault(a => a.Id == CompanyId).CompanyA4EsalImage;
            ViewData["CompanyNameE"] = (await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyId)).Company_NameE;
            ViewData["CompanyName"] = (await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyId)).Company_Name;
            return View();

        }

        //////////////////////////////////////////////////////////////////////////////////////
        //////////// AllBills/CommissionByUserDate **** العمولات - عمولات الموظف **** /////////////
        public async Task<IActionResult> CommissionByUserDate()
        {
            var CompanyId = (await _userManager.Users.SingleOrDefaultAsync(a => a.Id == _userManager.GetUserId(User))).CompanyId;
            ViewData["UsersId"] = new SelectList(_context.Users.Where(a => a.UserName != "Developer").Where(x=>x.CompanyId == CompanyId), "Id", "UserName");

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CommissionByUserDate([Bind("Name7,StartDate,EndDate")] DatesSearch datesSearch)
        {
            if (datesSearch.Name7 == null)
            {
                ModelState.AddModelError("Name7", "من فضلك اختر اسم الموظف");
            }

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
                TempData["Name7"] = datesSearch.Name7.ToString();

                //_context.Add(datesSearch);
                await _context.SaveChangesAsync();
                return RedirectToAction("StatementByUserCommission", "AllBills");
            }
            var CompanyId = (await _userManager.Users.SingleOrDefaultAsync(a => a.Id == _userManager.GetUserId(User))).CompanyId;
            ViewData["UsersId"] = new SelectList(_context.Users.Where(a => a.UserName != "Developer").Where(x => x.CompanyId == CompanyId), "Id", "UserName",datesSearch.Name1);

            return View(datesSearch);
        }
        public async Task<ActionResult> StatementByUserCommission()
        {

            var UserId = _userManager.GetUserId(User);
            var CompanyId = (await _userManager.Users.SingleOrDefaultAsync(a => a.Id == UserId)).CompanyId;
            var SelectedUser = TempData["Name7"].ToString();

            var billAirLine = await (from x in _context.BillAirLines
                    .Where(A => A.CompanyID == CompanyId)
                    .Where(x => x.UserId == SelectedUser)
                    .Where(d => d.BillDate >= Convert.ToDateTime(TempData["StartDate"]))
                    .Where(d => d.BillDate.Date <= Convert.ToDateTime(TempData["EndDate"]))
                                     select new Models.ViewModel.AllBillsViewModel { BillId = x.BillId, BillType = "طيران", BillDate = x.BillDate, CustomerOrSupplierName = x.CustomerOrSupplier.Name, CustomerSupplierName = x.CustomerSupplier.Name, MenuLE0Name = x.MenuLE0.M0_Name, MenuLE1Name = x.MenuLE1.M1_Name, MenuLE2Name = x.MenuLE2.M2_Name, Direction = x.Direction, PNR = x.PNR, TicketExportName = x.TicketExport.Name, AdultN = x.AdultN, ChildN = x.ChildN, CustomerPrice = x.CustomerPrice, NetPrice = x.NetPrice, Commission = x.EMPCommission, TicketFrom = x.TicketFrom, TicketTo = x.TicketTo, Commnets = x.Commnets, UserName = x.User.UserName, CompanyID = x.CompanyID }).ToListAsync();
            ViewData["billAirLine"] = billAirLine.Sum(a => a.Commission);

            var billDomestic = await (from x in _context.BillDomestic
                    .Where(A => A.CompanyID == CompanyId)
                    .Where(x => x.UserId == SelectedUser)
                    .Where(d => d.BillDate >= Convert.ToDateTime(TempData["StartDate"]))
                    .Where(d => d.BillDate.Date <= Convert.ToDateTime(TempData["EndDate"]))
                                      select new Models.ViewModel.AllBillsViewModel { BillId = x.BillId, BillType = "داخلى", BillDate = x.BillDate, CustomerOrSupplierName = x.CustomerOrSupplier.Name, CustomerSupplierName = x.CustomerSupplier.Name, MenuLE0Name = x.MenuLE0.M0_Name, MenuLE1Name = x.MenuLE1.M1_Name, MenuLE2Name = x.MenuLE2.M2_Name, TicketExportName = x.TicketExport.Name, AdultN = x.AdultN, ChildN = x.ChildN, CustomerPrice = x.CustomerPrice, NetPrice = x.NetPrice, Commission = x.EMPCommission, TicketFrom = x.TicketFrom, TicketTo = x.TicketTo, Commnets = x.Commnets, UserName = x.User.UserName, CompanyID = x.CompanyID }).ToListAsync();
            ViewData["billDomestic"] = billDomestic.Sum(a => a.Commission);

            var billForeign = await (from x in _context.BillForeigns
                    .Where(A => A.CompanyID == CompanyId)
                    .Where(x => x.UserId == SelectedUser)
                    .Where(d => d.BillDate >= Convert.ToDateTime(TempData["StartDate"]))
                    .Where(d => d.BillDate.Date <= Convert.ToDateTime(TempData["EndDate"]))
                                     select new Models.ViewModel.AllBillsViewModel { BillId = x.BillId, BillType = "خارجى", BillDate = x.BillDate, CustomerOrSupplierName = x.CustomerOrSupplier.Name, CustomerSupplierName = x.CustomerSupplier.Name, MenuLE0Name = x.MenuLE0.M0_Name, MenuLE1Name = x.MenuLE1.M1_Name, MenuLE2Name = x.MenuLE2.M2_Name, TicketExportName = x.TicketExport.Name, AdultN = x.AdultN, ChildN = x.ChildN, CustomerPrice = x.CustomerPrice, NetPrice = x.NetPrice, Commission = x.EMPCommission, TicketFrom = x.TicketFrom, TicketTo = x.TicketTo, Commnets = x.Commnets, UserName = x.User.UserName, CompanyID = x.CompanyID }).ToListAsync();
            ViewData["billForeign"] = billForeign.Sum(a => a.Commission);

            var billVisas = await (from x in _context.BillVisas
                    .Where(A => A.CompanyID == CompanyId)
                    .Where(x => x.UserId == SelectedUser)
                    .Where(a => a.BillState == true)
                    .Where(d => d.ApprovedDate >= Convert.ToDateTime(TempData["StartDate"]))
                    .Where(d => d.ApprovedDate.Date <= Convert.ToDateTime(TempData["EndDate"]))
                                   select new Models.ViewModel.AllBillsViewModel { BillId = x.BillId, BillType = "تأشيرات", BillDate = x.ApprovedDate, CustomerOrSupplierName = x.CustomerOrSupplier.Name, CustomerSupplierName = x.CustomerSupplier.Name, MenuLE0Name = x.MenuLE0.M0_Name, MenuLE1Name = x.MenuLE1.M1_Name, MenuLE2Name = x.MenuLE2.M2_Name, TicketExportName = x.TicketExport.Name, AdultN = x.AdultN, ChildN = x.ChildN, CustomerPrice = x.CustomerPrice, NetPrice = x.NetPrice, Commission = x.EMPCommission, Commnets = x.Commnets, UserName = x.User.UserName, CompanyID = x.CompanyID }).ToListAsync();
            ViewData["billVisas"] = billVisas.Sum(a => a.Commission);


            var Outbut = (billAirLine.Concat(billVisas).Concat(billDomestic).Concat(billForeign).OrderBy(a => a.BillDate));
            ViewData["UserName"] = (await _context.Users.SingleOrDefaultAsync(x => x.Id == SelectedUser)).UserName;
            ViewData["CompanyLogo"] = "~/" + TravelERP.Properties.Resources.ImgFolder + "/" + _context.Companies.SingleOrDefault(a => a.Id == CompanyId).CompanyLogo;
            ViewData["CompanyA4EsalImage"] = "~/" + TravelERP.Properties.Resources.ImgFolder + "/" + _context.Companies.SingleOrDefault(a => a.Id == CompanyId).CompanyA4EsalImage;
            ViewData["CompanyNameE"] = (await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyId)).Company_NameE;
            ViewData["CompanyName"] = (await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyId)).Company_Name;
            ViewData["CompanyId"] = CompanyId;


            return View(Outbut);
        }




    }
}