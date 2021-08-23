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
    [Route("api/BillAirLines")]
    public class BillAirLinesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _usermanager;

        public BillAirLinesController(ApplicationDbContext context, UserManager<ApplicationUser> usermanager)
        {
            _context = context;
            _usermanager = usermanager;
        }

        // GET: api/BillAirLines
        [HttpGet]
        public async Task<IEnumerable<BillsHomePageViewModel>> GetBillAirLines()
        {
            var CompanyId =(await _context.Users.SingleOrDefaultAsync(a => a.Id == _usermanager.GetUserId(User))).CompanyId;
            var UserId = (await _usermanager.Users.SingleOrDefaultAsync(a => a.Id == _usermanager.GetUserId(User))).Id;

            if (User.IsInRole(CustomRoles.Admin) || User.IsInRole(CustomRoles.BranchManager))
            {
                var Bills = await (from x in _context.BillAirLines.Where(a => a.CompanyID == CompanyId) select new BillsHomePageViewModel { Id = x.Id, BillId = x.BillId, BillDate = x.BillDate.Date.ToShortDateString(), CustomerPrice = x.CustomerPrice, NetPrice = x.NetPrice, EMPCommission = (float)Math.Round(x.EMPCommission, 2), CustomerSupplier = string.Concat(x.CustomerSupplier.Name, " - " + x.Commnets), TicketExport = x.TicketExport.Name, Details = string.Concat(x.MenuLE0.M0_Name, " - " + x.MenuLE1.M1_Name, " - " + x.MenuLE2.M2_Name, " - " + x.Direction), TicketNo = string.Concat(x.PNR, " - " + x.eTicketNumber) }).ToListAsync();
                return Bills;

            }
            else
            {
                var Bills = await (from x in _context.BillAirLines.Where(a => a.UserId == UserId) select new BillsHomePageViewModel { Id = x.Id, BillId = x.BillId, BillDate = x.BillDate.Date.ToShortDateString(), CustomerPrice = x.CustomerPrice, NetPrice = x.NetPrice, EMPCommission = (float)Math.Round(x.EMPCommission, 2), CustomerSupplier = string.Concat(x.CustomerSupplier.Name, " - " + x.Commnets), TicketExport = x.TicketExport.Name, Details = string.Concat(x.MenuLE0.M0_Name, " - " + x.MenuLE1.M1_Name, " - " + x.MenuLE2.M2_Name, " - " + x.Direction), TicketNo = string.Concat(x.PNR, " - " + x.eTicketNumber) }).ToListAsync();
                return Bills;

            }
        }

        // GET: api/BillAirLines/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBillAirLine([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var billAirLine = await _context.BillAirLines.SingleOrDefaultAsync(m => m.Id == id);

            if (billAirLine == null)
            {
                return NotFound();
            }

            return Ok(billAirLine);
        }

        // PUT: api/BillAirLines/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBillAirLine([FromRoute] int id, [FromBody] BillAirLine billAirLine)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != billAirLine.Id)
            {
                return BadRequest();
            }

            _context.Entry(billAirLine).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BillAirLineExists(id))
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

        // POST: api/BillAirLines
        [HttpPost]
        public async Task<IActionResult> PostBillAirLine([FromBody] BillAirLine billAirLine)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.BillAirLines.Add(billAirLine);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBillAirLine", new { id = billAirLine.Id }, billAirLine);
        }

        // DELETE: api/BillAirLines/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBillAirLine([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var billAirLine = await _context.BillAirLines.SingleOrDefaultAsync(m => m.Id == id);
            if (billAirLine == null)
            {
                return NotFound();
            }

            _context.BillAirLines.Remove(billAirLine);
            await _context.SaveChangesAsync();

            return Ok(billAirLine);
        }

        private bool BillAirLineExists(int id)
        {
            return _context.BillAirLines.Any(e => e.Id == id);
        }
    }
}