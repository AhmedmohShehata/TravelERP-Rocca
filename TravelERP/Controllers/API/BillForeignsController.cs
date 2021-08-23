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
    [Route("api/BillForeigns")]
    public class BillForeignsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _usermanager;

        public BillForeignsController(ApplicationDbContext context, UserManager<ApplicationUser> usermanager)
        {
            _context = context;
            _usermanager = usermanager;
        }

        // GET: api/BillForeigns
        [HttpGet]
        public async Task<IEnumerable<BillsHomePageViewModel>> GetBillForeign()
        {
            var CompanyId =(await _context.Users.SingleOrDefaultAsync(a => a.Id == _usermanager.GetUserId(User))).CompanyId;
            var UserId = (await _usermanager.Users.SingleOrDefaultAsync(a => a.Id == _usermanager.GetUserId(User))).Id;

            if (User.IsInRole(CustomRoles.Admin) || User.IsInRole(CustomRoles.BranchManager))
            {
                var Bills = await (from x in _context.BillForeigns.Where(a => a.CompanyID == CompanyId) select new BillsHomePageViewModel { Id = x.Id, BillId = x.BillId, BillDate = x.BillDate.Date.ToShortDateString(), CustomerPrice = x.CustomerPrice, NetPrice = x.NetPrice, EMPCommission = (float)Math.Round(x.EMPCommission, 2), CustomerSupplier = string.Concat(x.CustomerSupplier.Name, " - " + x.Commnets), TicketExport = x.TicketExport.Name, Details = string.Concat(x.MenuLE0.M0_Name, " - " + x.MenuLE1.M1_Name, " - " + x.MenuLE2.M2_Name) }).ToListAsync();
                return Bills;
            }
            else
            {
                var Bills = await (from x in _context.BillForeigns.Where(a => a.UserId == UserId) select new BillsHomePageViewModel { Id = x.Id, BillId = x.BillId, BillDate = x.BillDate.Date.ToShortDateString(), CustomerPrice = x.CustomerPrice, NetPrice = x.NetPrice, EMPCommission = (float)Math.Round(x.EMPCommission, 2), CustomerSupplier = string.Concat(x.CustomerSupplier.Name, " - " + x.Commnets), TicketExport = x.TicketExport.Name, Details = string.Concat(x.MenuLE0.M0_Name, " - " + x.MenuLE1.M1_Name, " - " + x.MenuLE2.M2_Name) }).ToListAsync();
                return Bills;
            }
        }

        // GET: api/BillForeigns/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBillForeign([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var billForeign = await _context.BillForeigns.SingleOrDefaultAsync(m => m.Id == id);

            if (billForeign == null)
            {
                return NotFound();
            }

            return Ok(billForeign);
        }

        // PUT: api/BillForeigns/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBillForeign([FromRoute] int id, [FromBody] BillForeign billForeign)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != billForeign.Id)
            {
                return BadRequest();
            }

            _context.Entry(billForeign).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BillForeignExists(id))
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

        // POST: api/billForeign
        [HttpPost]
        public async Task<IActionResult> PostBillforeign([FromBody] BillForeign billForeign)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.BillForeigns.Add(billForeign);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBillForeign", new { id = billForeign.Id }, billForeign);
        }

        // DELETE: api/billForeigns/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBillbillForeign([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var billForeign = await _context.BillForeigns.SingleOrDefaultAsync(m => m.Id == id);
            if (billForeign == null)
            {
                return NotFound();
            }

            _context.BillForeigns.Remove(billForeign);
            await _context.SaveChangesAsync();

            return Ok(billForeign);
        }

        private bool BillForeignExists(int id)
        {
            return _context.BillForeigns.Any(e => e.Id == id);
        }
    }
}