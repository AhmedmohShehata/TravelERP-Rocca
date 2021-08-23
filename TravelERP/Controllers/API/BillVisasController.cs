using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelERP.Data;
using TravelERP.Models;
using TravelERP.Models.ViewModel;
using static TravelERP.Controllers.AccountController;

namespace TravelERP.Controllers.API
{
    [Produces("application/json")]
    [Route("api/BillVisas")]
    public class BillVisasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _usermanager;


        public BillVisasController(ApplicationDbContext context, UserManager<ApplicationUser> usermanager)
        {
            _context = context;
            _usermanager = usermanager;
        }

        // GET: api/BillVisas
        [HttpGet]
        public async Task<IEnumerable<BillsHomePageViewModel>> GetBillVisas()
        {
            var CompanyId =(await _context.Users.SingleOrDefaultAsync(a => a.Id == _usermanager.GetUserId(User))).CompanyId;
            var UserId = (await _usermanager.Users.SingleOrDefaultAsync(a => a.Id == _usermanager.GetUserId(User))).Id;
            var CompanyCuntry = (await _context.Companies.SingleOrDefaultAsync(a => a.Id == CompanyId)).DateTimeCountry;
            var info = TimeZoneInfo.FindSystemTimeZoneById(CompanyCuntry);
            DateTimeOffset localServerTime = DateTimeOffset.Now;
            DateTimeOffset localTime = TimeZoneInfo.ConvertTime(localServerTime, info);
            DateTime datetimenow = localTime.DateTime;
            ViewData["DateTimeNow"] = datetimenow.Date;

            if (User.IsInRole(CustomRoles.Admin) || User.IsInRole(CustomRoles.BranchManager))
            {
                var Bills = await (from x in _context.BillVisas.Where(a => a.CompanyID == CompanyId) select new BillsHomePageViewModel { Id = x.Id, BillId = x.BillId, BillDate = x.ApprovedDate.Date.ToShortDateString(), CustomerPrice = x.CustomerPrice, NetPrice = x.NetPrice, EMPCommission = (float)Math.Round(x.EMPCommission, 2), CustomerSupplier = string.Concat(x.CustomerSupplier.Name, " - " + x.Commnets, " - " + x.PassportNo), TicketExport = x.TicketExport.Name, Details = string.Concat(x.MenuLE1.M1_Name, " - " + x.MenuLE2.M2_Name) }).ToListAsync();
                return Bills;
            }
            else
            {
                var Bills = await (from x in _context.BillVisas.Where(a => a.UserId == UserId) select new BillsHomePageViewModel { Id = x.Id, BillId = x.BillId, BillDate = x.ApprovedDate.Date.ToShortDateString(), CustomerPrice = x.CustomerPrice, NetPrice = x.NetPrice, EMPCommission = (float)Math.Round(x.EMPCommission, 2), CustomerSupplier = string.Concat(x.CustomerSupplier.Name, " - " + x.Commnets, " - " + x.PassportNo), TicketExport = x.TicketExport.Name, Details = string.Concat(x.MenuLE1.M1_Name, " - " + x.MenuLE2.M2_Name) }).ToListAsync();
                return Bills;
            }
        }


        // GET: api/BillVisas/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBillVisa([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var billVisa = await _context.BillVisas.SingleOrDefaultAsync(m => m.Id == id);

            if (billVisa == null)
            {
                return NotFound();
            }

            return Ok(billVisa);
        }

        // PUT: api/BillVisas/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBillVisa([FromRoute] int id, [FromBody] BillVisa billVisa)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != billVisa.Id)
            {
                return BadRequest();
            }

            _context.Entry(billVisa).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BillVisaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/BillVisas
        [HttpPost]
        public async Task<IActionResult> PostBillVisa([FromBody] BillVisa billVisa)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.BillVisas.Add(billVisa);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBillVisa", new { id = billVisa.Id }, billVisa);
        }

        // DELETE: api/BillVisas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBillVisa([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var billVisa = await _context.BillVisas.SingleOrDefaultAsync(m => m.Id == id);
            if (billVisa == null)
            {
                return NotFound();
            }

            _context.BillVisas.Remove(billVisa);
            await _context.SaveChangesAsync();

            return Ok(billVisa);
        }

        private bool BillVisaExists(int id)
        {
            return _context.BillVisas.Any(e => e.Id == id);
        }
    }
}