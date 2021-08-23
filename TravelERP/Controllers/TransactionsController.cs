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
using TravelERP.Models.ViewModel;
using static TravelERP.Controllers.AccountController;

namespace TravelERP.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;


        public TransactionsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;

        }
        [Authorize(Roles = CustomRoles.StatementDate)]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = CustomRoles.StatementDate)]
        public async Task<ActionResult> DailyStatement()
        {
            var CompanyId =(await _userManager.Users.SingleOrDefaultAsync(a => a.Id == _userManager.GetUserId(User))).CompanyId;
            //startTransactions
            var startEsal =await (from x in _context.Esals.Where(A => A.CompanyID == CompanyId).Where(a => a.PaymentMethod.Name == "نقدى") select new Models.ViewModel.Transactions { Debit = x.AmountPaid, EDate=x.EsalDate }).Where(a => a.EDate.Date < Convert.ToDateTime(TempData["StartDate"])).ToListAsync();
            var startEzns =await (from x in _context.Ezns.Where(A => A.CompanyID == CompanyId).Where(a => a.PaymentMethod.Name == "نقدى") select new Models.ViewModel.Transactions { Credit = x.AmountWithdrawn, EDate=x.EznDate }).Where(a => a.EDate.Date < Convert.ToDateTime(TempData["StartDate"])).ToListAsync();

            var OpenBalance=await _context.OpeningBalances.Where(A => A.CompanyID == CompanyId).SingleOrDefaultAsync(a => a.StatementType.NameId == 3);
            ViewData["startTransactions"] = (startEsal.Sum(a => a.Debit) - startEzns.Sum(a => a.Credit)) + OpenBalance.Balance;

            var ezns =await (from x in _context.Ezns.Where(A => A.CompanyID == CompanyId) select new Models.ViewModel.Transactions { EId = x.EznId, EDate = x.EznDate, Statement =string.Concat(x.Name, x.CustomerOrSupplier.Name, " - " + x.CustomerSupplier.Name, " - " + x.ExpenseName , " - " + x.MenuLE0.M0_Name, " - " + x.MenuLZ0.M0_Name , " - " + x.MenuLZ1.M1_Name , " - " + x.MenuLZ2.M2_Name, " - " + x.PaymentMethod.Name , " - " + x.User.UserName), Credit = x.AmountWithdrawn , Type= x.PaymentMethod.Name }).Where(a => a.EDate.Date == Convert.ToDateTime(TempData["StartDate"])).ToListAsync();
            var esals =await (from x in _context.Esals.Where(A => A.CompanyID == CompanyId) select new Models.ViewModel.Transactions { EId = x.EsalId, EDate = x.EsalDate, Statement =string.Concat(x.CustomerOrSupplier.Name , " - " + x.CustomerSupplier.Name , " - " + x.MenuLE0.M0_Name , " - " + x.MenuLE1.M1_Name , " - " + x.MenuLE2.M2_Name, " - " + x.DepositDesc, " - " + x.PaymentMethod.Name , " - " + x.User.UserName), Debit = x.AmountPaid, Type = x.PaymentMethod.Name }).Where(a => a.EDate.Date == Convert.ToDateTime(TempData["StartDate"])).ToListAsync();

            var All =(ezns.Concat(esals)).OrderBy(a=>a.EDate);

            if (All == null)
            {
                return NotFound();
            }
            ViewData["CompanyId"] = CompanyId;
            ViewData["CompanyLogo"] = "~/" + TravelERP.Properties.Resources.ImgFolder + "/" + _context.Companies.SingleOrDefault(a => a.Id == CompanyId).CompanyLogo;
            ViewData["CompanyA4EsalImage"] = "~/" + TravelERP.Properties.Resources.ImgFolder + "/" + _context.Companies.SingleOrDefault(a => a.Id == CompanyId).CompanyA4EsalImage;
            ViewData["CompanyNameE"] =(await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyId)).Company_NameE;
            ViewData["CompanyName"] =(await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyId)).Company_Name;


            return View(All);
        }
        [Authorize(Roles = CustomRoles.StatementDate)]

        public IActionResult StatementDate()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = CustomRoles.StatementDate)]
        public async Task<IActionResult> StatementDate([Bind("StartDate")] DatesSearch datesSearch)
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
                return RedirectToAction("DailyStatement", "Transactions");
            }
            return View(datesSearch);
        }

        [Authorize(Roles = CustomRoles.Admin)]
        public async Task<IActionResult> AllStatementStart()
        {
            var info = TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time");
            DateTimeOffset localServerTime = DateTimeOffset.Now;
            DateTimeOffset localTime = TimeZoneInfo.ConvertTime(localServerTime, info);
            DateTime datetimenow = localTime.DateTime;

            var OpenBalance = await (from x in _context.OpeningBalances.Where(a => a.StatementType.NameId == 3) select new Models.ViewModel.AllCompanyProfitViewModel { StatementDate = x.Balance, CompanyName = x.Company.Company_Name }).ToListAsync();
            var ezns = await (from x in _context.Ezns.Where(f => f.PaymentMethod.Name == "نقدى") select new Models.ViewModel.AllCompanyProfitViewModel { StatementDate = x.AmountWithdrawn * -1, CompanyName = x.Company.Company_Name }).ToListAsync();
            var esals =await  (from x in _context.Esals.Where(f => f.PaymentMethod.Name == "نقدى") select new Models.ViewModel.AllCompanyProfitViewModel { StatementDate = x.AmountPaid, CompanyName = x.Company.Company_Name }).ToListAsync();

            var billAirLines =await (from x in _context.BillAirLines.Where(a => a.BillDate.Date == datetimenow.Date) select new Models.ViewModel.AllCompanyProfitViewModel { BillToday = (x.CustomerPrice - x.NetPrice - x.EMPCommission), CompanyName = x.Company.Company_Name }).ToListAsync();
            var billDomestic = await (from x in _context.BillDomestic.Where(a => a.BillDate.Date == datetimenow.Date) select new Models.ViewModel.AllCompanyProfitViewModel { BillToday = (x.CustomerPrice - x.NetPrice - x.EMPCommission), CompanyName = x.Company.Company_Name }).ToListAsync();
            var billForeign = await (from x in _context.BillForeigns.Where(a => a.BillDate.Date == datetimenow.Date) select new Models.ViewModel.AllCompanyProfitViewModel { BillToday = (x.CustomerPrice - x.NetPrice - x.EMPCommission), CompanyName = x.Company.Company_Name }).ToListAsync();
            var billVisas = await (from x in _context.BillVisas.Where(a => a.BillDate.Date == datetimenow.Date) select new Models.ViewModel.AllCompanyProfitViewModel { BillToday = (x.CustomerPrice - x.NetPrice - x.EMPCommission), CompanyName = x.Company.Company_Name }).ToListAsync();


            var billAirLinesAll = await (from x in _context.BillAirLines.Where(a => a.BillDate.Date >= new DateTime(datetimenow.Year, datetimenow.Month, 1)) select new Models.ViewModel.AllCompanyProfitViewModel { BillProfitThisMonth = (x.CustomerPrice - x.NetPrice - x.EMPCommission), CompanyName = x.Company.Company_Name }).ToListAsync();
            var billDomesticAll = await (from x in _context.BillDomestic.Where(a => a.BillDate.Date >= new DateTime(datetimenow.Year, datetimenow.Month, 1)) select new Models.ViewModel.AllCompanyProfitViewModel { BillProfitThisMonth = (x.CustomerPrice - x.NetPrice - x.EMPCommission), CompanyName = x.Company.Company_Name }).ToListAsync();
            var billForeignAll = await (from x in _context.BillForeigns.Where(a => a.BillDate.Date >= new DateTime(datetimenow.Year, datetimenow.Month, 1)) select new Models.ViewModel.AllCompanyProfitViewModel { BillProfitThisMonth = (x.CustomerPrice - x.NetPrice - x.EMPCommission), CompanyName = x.Company.Company_Name }).ToListAsync();
            var billVisasAll = await (from x in _context.BillVisas.Where(a => a.BillDate.Date >= new DateTime(datetimenow.Year, datetimenow.Month, 1)) select new Models.ViewModel.AllCompanyProfitViewModel { BillProfitThisMonth = (x.CustomerPrice - x.NetPrice - x.EMPCommission), CompanyName = x.Company.Company_Name }).ToListAsync();

            var All = ((OpenBalance.Concat(ezns).Concat(esals)
                .Concat(billAirLines).Concat(billDomestic).Concat(billForeign).Concat(billVisas)
                .Concat(billAirLinesAll).Concat(billDomesticAll).Concat(billForeignAll).Concat(billVisasAll)));

            var AllGrouped = (All)
     .OrderByDescending(p => p.CompanyName).ToList()
     .GroupBy(x => x.CompanyName)
     .Select(a =>
         new
         {
             CompanyName = a.Key,
             StatementDate = a.Sum(x => x.StatementDate),
             BillProfitThisMonth = a.Sum(x => x.BillProfitThisMonth),
             BillToday = a.Sum(x => x.BillToday)

         });


            var Outbut = (from x in AllGrouped select new Models.ViewModel.AllCompanyProfitViewModel { StatementDate = x.StatementDate, BillToday = x.BillToday, BillProfitThisMonth = x.BillProfitThisMonth, CompanyName = x.CompanyName });



            TempData["AtharBillToday"] = Outbut.SingleOrDefault(a => a.CompanyName == "آثار للسياحه").BillToday;
            TempData["AtharStatementDate"] = Outbut.SingleOrDefault(a => a.CompanyName == "آثار للسياحه").StatementDate;

            TempData["AtharManshiaBillToday"] = Outbut.SingleOrDefault(a => a.CompanyName == "آثار للسياحة - فرع الغرفه").BillToday;
            TempData["AtharManshiaStatementDate"] = Outbut.SingleOrDefault(a => a.CompanyName == "آثار للسياحة - فرع الغرفه").StatementDate;

            TempData["NilesatBillToday"] = Outbut.SingleOrDefault(a => a.CompanyName == "نايل سات للسياحه").BillToday;
            TempData["NilesatStatementDate"] = Outbut.SingleOrDefault(a => a.CompanyName == "نايل سات للسياحه").StatementDate;

            TempData["NiceBillToday"] = Outbut.SingleOrDefault(a => a.CompanyName == "نيس للسياحه").BillToday;
            TempData["NiceStatementDate"] = Outbut.SingleOrDefault(a => a.CompanyName == "نيس للسياحه").StatementDate;

            TempData["EITBillToday"] = Outbut.SingleOrDefault(a => a.CompanyName == "المصريه العالميه للسياحه").BillToday;
            TempData["EITStatementDate"] = Outbut.SingleOrDefault(a => a.CompanyName == "المصريه العالميه للسياحه").StatementDate;

            TempData["AtharAgmanBillToday"] = Outbut.SingleOrDefault(a => a.CompanyName == "آثار للسياحة والسفريات ذ.م.م").BillToday;
            TempData["AtharAgmanStatementDate"] = Outbut.SingleOrDefault(a => a.CompanyName == "آثار للسياحة والسفريات ذ.م.م").StatementDate;



            return RedirectToAction("AllStatement", "Transactions");

        }

        [Authorize(Roles = CustomRoles.Admin)]
        public async Task<IActionResult> AllStatement()
        {
            var info = TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time");
            DateTimeOffset localServerTime = DateTimeOffset.Now;
            DateTimeOffset localTime = TimeZoneInfo.ConvertTime(localServerTime, info);
            DateTime datetimenow = localTime.DateTime;

            var OpenBalance = await(from x in _context.OpeningBalances.Where(a=>a.StatementType.NameId==3) select new Models.ViewModel.AllCompanyProfitViewModel { StatementDate = x.Balance, CompanyName=x.Company.Company_Name }).ToListAsync();
            var ezns =await (from x in _context.Ezns.Where(f => f.PaymentMethod.Name == "نقدى" ) select new Models.ViewModel.AllCompanyProfitViewModel { StatementDate = x.AmountWithdrawn * -1, CompanyName = x.Company.Company_Name }).ToListAsync();
            var esals =await (from x in _context.Esals.Where(f => f.PaymentMethod.Name == "نقدى") select new Models.ViewModel.AllCompanyProfitViewModel { StatementDate = x.AmountPaid , CompanyName = x.Company.Company_Name }).ToListAsync();

            var billAirLines =await (from x in _context.BillAirLines.Where(a => a.BillDate.Date == datetimenow.Date) select new Models.ViewModel.AllCompanyProfitViewModel { BillToday = (x.CustomerPrice - x.NetPrice - x.EMPCommission), CompanyName = x.Company.Company_Name }).ToListAsync();
            var billDomestic =await (from x in _context.BillDomestic.Where(a => a.BillDate.Date == datetimenow.Date) select new Models.ViewModel.AllCompanyProfitViewModel { BillToday = (x.CustomerPrice - x.NetPrice - x.EMPCommission), CompanyName = x.Company.Company_Name }).ToListAsync();
            var billForeign = await (from x in _context.BillForeigns.Where(a => a.BillDate.Date == datetimenow.Date) select new Models.ViewModel.AllCompanyProfitViewModel { BillToday = (x.CustomerPrice - x.NetPrice - x.EMPCommission), CompanyName = x.Company.Company_Name }).ToListAsync();
            var billVisas =await (from x in _context.BillVisas.Where(a => a.ApprovedDate.Date == datetimenow.Date).Where(a=>a.BillState == true) select new Models.ViewModel.AllCompanyProfitViewModel { BillToday = (x.CustomerPrice - x.NetPrice -x.EMPCommission), CompanyName = x.Company.Company_Name }).ToListAsync();


            var billAirLinesAll = await(from x in _context.BillAirLines.Where(a=>a.BillDate.Date >= new DateTime(datetimenow.Year,datetimenow.Month ,1)) select new Models.ViewModel.AllCompanyProfitViewModel { BillProfitThisMonth = (x.CustomerPrice - x.NetPrice - x.EMPCommission), CompanyName = x.Company.Company_Name}).ToListAsync();
            var billDomesticAll =await (from x in _context.BillDomestic.Where(a => a.BillDate.Date >= new DateTime(datetimenow.Year, datetimenow.Month, 1)) select new Models.ViewModel.AllCompanyProfitViewModel { BillProfitThisMonth = (x.CustomerPrice - x.NetPrice - x.EMPCommission), CompanyName = x.Company.Company_Name}).ToListAsync();
            var billForeignAll = await (from x in _context.BillForeigns.Where(a => a.BillDate.Date >= new DateTime(datetimenow.Year, datetimenow.Month, 1)) select new Models.ViewModel.AllCompanyProfitViewModel { BillProfitThisMonth = (x.CustomerPrice - x.NetPrice - x.EMPCommission), CompanyName = x.Company.Company_Name }).ToListAsync();
            var billVisasAll =await (from x in _context.BillVisas.Where(a => a.ApprovedDate.Date >= new DateTime(datetimenow.Year, datetimenow.Month, 1)).Where(a => a.BillState == true) select new Models.ViewModel.AllCompanyProfitViewModel { BillProfitThisMonth = (x.CustomerPrice - x.NetPrice - x.EMPCommission), CompanyName = x.Company.Company_Name}).ToListAsync();

            var All =  ((OpenBalance.Concat(ezns).Concat(esals)
                .Concat(billAirLines).Concat(billDomestic).Concat(billForeign).Concat(billVisas)
                .Concat(billAirLinesAll).Concat(billDomesticAll).Concat(billForeignAll).Concat(billVisasAll)));

            var AllGrouped = ( All )
     .OrderByDescending(p => p.CompanyName).ToList()
     .GroupBy(x => x.CompanyName)
     .Select(a =>
         new
         {
             CompanyName = a.Key,
             StatementDate = a.Sum(x => x.StatementDate),
             BillProfitThisMonth = a.Sum(x => x.BillProfitThisMonth),
             BillToday = a.Sum(x => x.BillToday)

         });


            var Outbut = (from x in AllGrouped select new Models.ViewModel.AllCompanyProfitViewModel { StatementDate = x.StatementDate, BillToday = x.BillToday, BillProfitThisMonth = x.BillProfitThisMonth, CompanyName = x.CompanyName });


            ViewData["StatementDate"] = Outbut.Where(a => a.CompanyName != "آثار للسياحة والسفريات ذ.م.م").Sum(a => a.StatementDate);
            ViewData["BillToday"] = Outbut.Where(a => a.CompanyName != "آثار للسياحة والسفريات ذ.م.م").Sum(a => a.BillToday);
            ViewData["BillProfitThisMonth"] = Outbut.Where(a => a.CompanyName != "آثار للسياحة والسفريات ذ.م.م").Sum(a => a.BillProfitThisMonth);

            
            return View(Outbut);
        }

        [Authorize(Roles = CustomRoles.Admin)]
        public ActionResult AllStatementUpdate()
        {

            TempData["AtharBillTodayUpdate"] = TempData["AtharBillToday"];
            TempData["AtharStatementDateUpdate"] = TempData["AtharStatementDate"];

            TempData["AtharManshiaBillTodayUpdate"] = TempData["AtharManshiaBillToday"];
            TempData["AtharManshiaStatementDateUpdate"] = TempData["AtharManshiaStatementDate"];

            TempData["NilesatBillTodayUpdate"] = TempData["NilesatBillToday"];
            TempData["NilesatStatementDateUpdate"] = TempData["NilesatStatementDate"];

            TempData["NiceBillTodayUpdate"] = TempData["NiceBillToday"];
            TempData["NiceStatementDateUpdate"] = TempData["NiceStatementDate"];

            TempData["EITBillTodayUpdate"] = TempData["EITBillToday"];
            TempData["EITStatementDateUpdate"] = TempData["EITStatementDate"];

            TempData["AtharAgmanBillTodayUpdate"] = TempData["AtharAgmanBillToday"];
            TempData["AtharAgmanStatementDateUpdate"] = TempData["AtharAgmanStatementDate"];

            return RedirectToAction("AllStatement", "Transactions");


        }




        [Authorize(Roles = CustomRoles.StatementDate)]

        public IActionResult StatementBankDate()
        {
            ViewData["BanksId"] = new SelectList(_context.paymentMethods.Where(a=>a.Name !="نقدى"), "Id", "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = CustomRoles.StatementDate)]
        public async Task<IActionResult> StatementBankDate([Bind("Name1,StartDate")] DatesSearch datesSearch)
        {
            if (datesSearch.Name1 == 0)
            {
                ModelState.AddModelError("Name1", "من فضلك اختر رقم الحساب");
            }

            if (datesSearch.StartDate == null)
            {
                ModelState.AddModelError("StartDate", "من فضلك اختر تاريخ للبحث");
            }
            if (ModelState.IsValid)
            {
                TempData["StartDate"] = datesSearch.StartDate.Value.Date.ToString();
                TempData["Name1"] = datesSearch.Name1;

                //_context.Add(datesSearch);
                await _context.SaveChangesAsync();
                return RedirectToAction("BankStatement", "Transactions");
            }
            ViewData["BanksId"] = new SelectList(_context.paymentMethods.Where(n => n.Name != "نقدى"), "Id", "Name", datesSearch.Name1);

            return View(datesSearch);
        }

        [Authorize(Roles = CustomRoles.StatementDate)]
        public async Task<ActionResult> BankStatement()
        {
            var CompanyId = (await _userManager.Users.SingleOrDefaultAsync(a => a.Id == _userManager.GetUserId(User))).CompanyId;
            var BankId = (Int32) TempData["Name1"];
            var BankName = (await _context.paymentMethods.SingleOrDefaultAsync(a => a.Id == BankId)).Name;
            //startTransactions
            var startEsal = await (from x in _context.Esals.Where(A => A.CompanyID == CompanyId).Where(a => a.PaymentMethodId == BankId) select new Models.ViewModel.Transactions { Debit = x.AmountPaid, EDate = x.EsalDate }).Where(a => a.EDate.Date < Convert.ToDateTime(TempData["StartDate"])).ToListAsync();
            var startEzns = await (from x in _context.Ezns.Where(A => A.CompanyID == CompanyId).Where(a => a.PaymentMethodId == BankId) select new Models.ViewModel.Transactions { Credit = x.AmountWithdrawn, EDate = x.EznDate }).Where(a => a.EDate.Date < Convert.ToDateTime(TempData["StartDate"])).ToListAsync();

            var OpenBalance = await _context.OpeningBalances.Where(A => A.CompanyID == CompanyId).SingleOrDefaultAsync(a => a.StatementType.Name == BankName);
            ViewData["startTransactions"] = (startEsal.Sum(a => a.Debit) - startEzns.Sum(a => a.Credit)) + OpenBalance.Balance;

            var ezns = await (from x in _context.Ezns.Where(A => A.CompanyID == CompanyId).Where(a=>a.PaymentMethodId == BankId) select new Models.ViewModel.Transactions { EId = x.EznId, EDate = x.EznDate, Statement = string.Concat(x.Name, x.CustomerOrSupplier.Name, " - " + x.CustomerSupplier.Name, " - " + x.ExpenseName, " - " + x.MenuLE0.M0_Name, " - " + x.MenuLZ0.M0_Name, " - " + x.MenuLZ1.M1_Name, " - " + x.MenuLZ2.M2_Name, " - " + x.PaymentMethod.Name, " - " + x.User.UserName), Credit = x.AmountWithdrawn, Type = x.PaymentMethod.Name }).Where(a => a.EDate.Date == Convert.ToDateTime(TempData["StartDate"])).ToListAsync();
            var esals = await (from x in _context.Esals.Where(A => A.CompanyID == CompanyId).Where(a => a.PaymentMethodId == BankId) select new Models.ViewModel.Transactions { EId = x.EsalId, EDate = x.EsalDate, Statement = string.Concat(x.CustomerOrSupplier.Name, " - " + x.CustomerSupplier.Name, " - " + x.MenuLE0.M0_Name, " - " + x.MenuLE1.M1_Name, " - " + x.MenuLE2.M2_Name, " - " + x.DepositDesc, " - " + x.PaymentMethod.Name, " - " + x.User.UserName), Debit = x.AmountPaid, Type = x.PaymentMethod.Name }).Where(a => a.EDate.Date == Convert.ToDateTime(TempData["StartDate"])).ToListAsync();

            var All = (ezns.Concat(esals)).OrderBy(a => a.EDate);

            if (All == null)
            {
                return NotFound();
            }
            ViewData["CompanyId"] = CompanyId;
            ViewData["BankName"] = BankName;
            ViewData["CompanyLogo"] = "~/" + TravelERP.Properties.Resources.ImgFolder + "/" + _context.Companies.SingleOrDefault(a => a.Id == CompanyId).CompanyLogo;
            ViewData["CompanyA4EsalImage"] = "~/" + TravelERP.Properties.Resources.ImgFolder + "/" + _context.Companies.SingleOrDefault(a => a.Id == CompanyId).CompanyA4EsalImage;
            ViewData["CompanyNameE"] = (await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyId)).Company_NameE;
            ViewData["CompanyName"] = (await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyId)).Company_Name;


            return View(All);
        }

    }
}