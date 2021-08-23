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
    [Route("api/Esals")]
    public class EsalsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _usermanager;

        public EsalsController(ApplicationDbContext context, UserManager<ApplicationUser> usermanager)
        {
            _context = context;
            _usermanager = usermanager;
        }

        // GET: api/Esals
        [HttpGet]
        public async Task<IEnumerable<EsalsHomePageViewModel>> GetEsals()
        {
            var CompanyId =(await _usermanager.Users.SingleOrDefaultAsync(a => a.Id == _usermanager.GetUserId(User))).CompanyId;
            var UserId = (await _usermanager.Users.SingleOrDefaultAsync(a => a.Id == _usermanager.GetUserId(User))).Id;

            if (User.IsInRole(CustomRoles.Admin) || User.IsInRole(CustomRoles.BranchManager))
            {
                var Esals = await (from x in _context.Esals.Where(a => a.CompanyID == CompanyId) select new EsalsHomePageViewModel { Id = x.Id, EsalId = x.EsalId, EsalDate = x.EsalDate.Date.ToShortDateString(), CustomerSupplier = x.CustomerSupplier.Name, BillIdId = x.BillIdId, AmountPaid = x.AmountPaid, MenuLE0 = string.Concat(x.MenuLE0.M0_Name, " - " + x.MenuLE1.M1_Name), PaymentMethod = x.PaymentMethod.Name }).ToListAsync();
                return Esals;

            }
            else
            {
                var Esals = await (from x in _context.Esals.Where(a => a.UserId == UserId) select new EsalsHomePageViewModel { Id = x.Id, EsalId = x.EsalId, EsalDate = x.EsalDate.Date.ToShortDateString(), CustomerSupplier = x.CustomerSupplier.Name, BillIdId = x.BillIdId, AmountPaid = x.AmountPaid, MenuLE0 = string.Concat(x.MenuLE0.M0_Name, " - " + x.MenuLE1.M1_Name), PaymentMethod = x.PaymentMethod.Name }).ToListAsync();
                return Esals;
            }
        }

        // GET: api/Esals/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEsal([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var esal = await _context.Esals.SingleOrDefaultAsync(m => m.Id == id);

            if (esal == null)
            {
                return NotFound();
            }

            return Ok(esal);
        }

        // PUT: api/Esals/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEsal([FromRoute] int id, [FromBody] Esal esal)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != esal.Id)
            {
                return BadRequest();
            }

            _context.Entry(esal).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EsalExists(id))
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

        // POST: api/Esals
        [HttpPost]
        public async Task<IActionResult> PostEsal([FromBody] Esal esal)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Esals.Add(esal);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEsal", new { id = esal.Id }, esal);
        }

        // DELETE: api/Esals/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEsal([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var esal = await _context.Esals.SingleOrDefaultAsync(m => m.Id == id);
            if (esal == null)
            {
                return NotFound();
            }

            _context.Esals.Remove(esal);
            await _context.SaveChangesAsync();

            return Ok(esal);
        }

        private bool EsalExists(int id)
        {
            return _context.Esals.Any(e => e.Id == id);
        }
    }
}