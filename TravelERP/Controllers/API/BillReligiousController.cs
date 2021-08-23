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

namespace TravelERP.Controllers.API
{
    [Produces("application/json")]
    [Route("api/BillReligious")]
    public class BillReligiousController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _usermanager;

        public BillReligiousController(ApplicationDbContext context, UserManager<ApplicationUser> usermanager)
        {
            _context = context;
            _usermanager = usermanager;
        }

        // GET: api/BillReligious
        [HttpGet]
        public async Task<IEnumerable<BillsHomePageViewModel>> GetBillReligious()
        {
            var CompanyId =(await _context.Users.SingleOrDefaultAsync(a => a.Id == _usermanager.GetUserId(User))).CompanyId;
            var Bills =await( from x in _context.BillReligious.Where(a => a.CompanyID == CompanyId) select new BillsHomePageViewModel { Id = x.Id, BillId = x.BillId, BillDate = x.BillDate.Date.ToShortDateString(), CustomerPrice = x.CustomerPrice, CustomerSupplier = string.Concat(x.CustomerSupplier.Name, " - " + x.Commnets), Details = string.Concat(x.MenuLE1.M1_Name, " - " + x.MenuLE2.M2_Name) }).ToListAsync();

            return  Bills;
        }

        // GET: api/BillReligious/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBillReligious([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var billReligious = await _context.BillReligious.SingleOrDefaultAsync(m => m.Id == id);

            if (billReligious == null)
            {
                return NotFound();
            }

            return Ok(billReligious);
        }

        // PUT: api/BillReligious/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBillReligious([FromRoute] int id, [FromBody] BillReligious billReligious)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != billReligious.Id)
            {
                return BadRequest();
            }

            _context.Entry(billReligious).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BillReligiousExists(id))
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

        // POST: api/BillReligious
        [HttpPost]
        public async Task<IActionResult> PostBillReligious([FromBody] BillReligious billReligious)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.BillReligious.Add(billReligious);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBillReligious", new { id = billReligious.Id }, billReligious);
        }

        // DELETE: api/BillReligious/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBillReligious([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var billReligious = await _context.BillReligious.SingleOrDefaultAsync(m => m.Id == id);
            if (billReligious == null)
            {
                return NotFound();
            }

            _context.BillReligious.Remove(billReligious);
            await _context.SaveChangesAsync();

            return Ok(billReligious);
        }

        private bool BillReligiousExists(int id)
        {
            return _context.BillReligious.Any(e => e.Id == id);
        }
    }
}