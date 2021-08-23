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
    [Route("api/BillDomestics")]
    public class BillDomesticsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _usermanager;

        public BillDomesticsController(ApplicationDbContext context, UserManager<ApplicationUser> usermanager)
        {
            _context = context;
            _usermanager = usermanager;
        }

        // GET: api/BillDomestics
        [HttpGet]
        public async Task<IEnumerable<BillsHomePageViewModel>> GetBillDomestic()
        {
            var CompanyId =(await _context.Users.SingleOrDefaultAsync(a => a.Id == _usermanager.GetUserId(User))).CompanyId;
            var UserId = (await _usermanager.Users.SingleOrDefaultAsync(a => a.Id == _usermanager.GetUserId(User))).Id;

            if (User.IsInRole(CustomRoles.Admin) || User.IsInRole(CustomRoles.BranchManager))
            {
                var Bills = await (from x in _context.BillDomestic.Where(a => a.CompanyID == CompanyId) select new BillsHomePageViewModel { Id = x.Id, BillId = x.BillId, BillDate = x.BillDate.Date.ToShortDateString(), CustomerPrice = x.CustomerPrice, NetPrice = x.NetPrice, EMPCommission = (float)Math.Round(x.EMPCommission, 2), CustomerSupplier = string.Concat(x.CustomerSupplier.Name, " - " + x.Commnets), TicketExport = x.TicketExport.Name, Details = string.Concat(x.MenuLE0.M0_Name, " - " + x.MenuLE1.M1_Name, " - " + x.MenuLE2.M2_Name) }).ToListAsync();
                return Bills;
            }
            else
            {
            var Bills =await( from x in _context.BillDomestic.Where(a => a.UserId == UserId) select new BillsHomePageViewModel { Id = x.Id, BillId = x.BillId, BillDate = x.BillDate.Date.ToShortDateString(), CustomerPrice = x.CustomerPrice, NetPrice = x.NetPrice, EMPCommission = (float)Math.Round(x.EMPCommission, 2), CustomerSupplier = string.Concat(x.CustomerSupplier.Name, " - " + x.Commnets), TicketExport = x.TicketExport.Name, Details = string.Concat(x.MenuLE0.M0_Name, " - " + x.MenuLE1.M1_Name, " - " + x.MenuLE2.M2_Name) }).ToListAsync();
            return Bills;
            }

        }

        // GET: api/BillDomestics/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBillDomestic([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var billDomestic = await _context.BillDomestic.SingleOrDefaultAsync(m => m.Id == id);

            if (billDomestic == null)
            {
                return NotFound();
            }

            return Ok(billDomestic);
        }

        // PUT: api/BillDomestics/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBillDomestic([FromRoute] int id, [FromBody] BillDomestic billDomestic)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != billDomestic.Id)
            {
                return BadRequest();
            }

            _context.Entry(billDomestic).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BillDomesticExists(id))
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

        // POST: api/BillDomestics
        [HttpPost]
        public async Task<IActionResult> PostBillDomestic([FromBody] BillDomestic billDomestic)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.BillDomestic.Add(billDomestic);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBillDomestic", new { id = billDomestic.Id }, billDomestic);
        }

        // DELETE: api/BillDomestics/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBillDomestic([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var billDomestic = await _context.BillDomestic.SingleOrDefaultAsync(m => m.Id == id);
            if (billDomestic == null)
            {
                return NotFound();
            }

            _context.BillDomestic.Remove(billDomestic);
            await _context.SaveChangesAsync();

            return Ok(billDomestic);
        }

        private bool BillDomesticExists(int id)
        {
            return _context.BillDomestic.Any(e => e.Id == id);
        }
    }
}