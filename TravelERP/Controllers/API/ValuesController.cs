using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelERP.Data;
using TravelERP.Models;
using TravelERP.Models.ViewModel;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TravelERP.Controllers.API
{
    [Produces("application/json")]

    [Authorize]
    public class ValuesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _usermanager;


        public ValuesController(ApplicationDbContext context, UserManager<ApplicationUser> usermanager)
        {
            _context = context;
            _usermanager = usermanager;
        }

        // GET: api/<ValuesController>
        [HttpGet]
        public async Task<IEnumerable<BillsHomePageViewModel>> BillVisasUsers()
        {
            var CompanyId =(await _context.Users.SingleOrDefaultAsync(a => a.Id == _usermanager.GetUserId(User))).CompanyId;
            var UserId = _usermanager.GetUserId(User);

            var Bills =await( from x in _context.BillVisas.Where(a=>a.BillState == false ).Where(a => a.CompanyID == CompanyId).Where(a=>a.UserId== UserId) select new BillsHomePageViewModel { Id = x.Id, BillId = x.BillId, BillDate = x.BillDate.Date.ToShortDateString(), CustomerPrice = x.CustomerPrice, NetPrice = x.NetPrice, EMPCommission = (float)Math.Round(x.EMPCommission, 2), CustomerSupplier = string.Concat(x.CustomerSupplier.Name, " - " + x.Commnets, " - " + x.PassportNo), TicketExport = x.TicketExport.Name, Details = string.Concat(x.MenuLE1.M1_Name, " - " + x.MenuLE2.M2_Name) }).ToListAsync();

            return Bills;
        }

    }
}
