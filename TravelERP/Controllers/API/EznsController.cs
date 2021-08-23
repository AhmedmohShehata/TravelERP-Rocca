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
    [Route("api/Ezns")]
    public class EznsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _usermanager;

        public EznsController(ApplicationDbContext context, UserManager<ApplicationUser> usermanager)
        {
            _context = context;
            _usermanager = usermanager;
        }

        // GET: api/Ezns
        [HttpGet]
        public async Task<IEnumerable<EznsHomePageViewModel>> GetEzns()
        {
            var CompanyId =(await _usermanager.Users.SingleOrDefaultAsync(a => a.Id == _usermanager.GetUserId(User))).CompanyId;
            var UserId = (await _usermanager.Users.SingleOrDefaultAsync(a => a.Id == _usermanager.GetUserId(User))).Id;

            if (User.IsInRole(CustomRoles.Admin) || User.IsInRole(CustomRoles.BranchManager))
            {
                var Ezns = await (from x in _context.Ezns.Where(a => a.CompanyID == CompanyId) select new EznsHomePageViewModel { Id = x.Id, EznId = x.EznId, EznDate = x.EznDate.Date.ToShortDateString(), Name = string.Concat(x.Name, " - " + x.CustomerOrSupplier.Name, " - " + x.CustomerSupplier.Name), ExpenseName = string.Concat(x.MenuLE0.M0_Name, " - " + x.MenuLE1.M1_Name, " - " + x.MenuLE2.M2_Name, " - " + x.MenuLZ0.M0_Name, " - " + x.MenuLZ1.M1_Name, " - " + x.MenuLZ2.M2_Name, " - " + x.ExpenseName), AmountWithdrawn = x.AmountWithdrawn, PaymentMethod = x.PaymentMethod.Name }).ToListAsync();
                return Ezns;
            }
            else
            {
                var Ezns = await (from x in _context.Ezns.Where(a => a.UserId == UserId) select new EznsHomePageViewModel { Id = x.Id, EznId = x.EznId, EznDate = x.EznDate.Date.ToShortDateString(), Name = string.Concat(x.Name, " - " + x.CustomerOrSupplier.Name, " - " + x.CustomerSupplier.Name), ExpenseName = string.Concat(x.MenuLE0.M0_Name, " - " + x.MenuLE1.M1_Name, " - " + x.MenuLE2.M2_Name, " - " + x.MenuLZ0.M0_Name, " - " + x.MenuLZ1.M1_Name, " - " + x.MenuLZ2.M2_Name, " - " + x.ExpenseName), AmountWithdrawn = x.AmountWithdrawn, PaymentMethod = x.PaymentMethod.Name }).ToListAsync();
                return Ezns;
            }

        }

        // GET: api/Ezns/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEzn([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ezn = await _context.Ezns.SingleOrDefaultAsync(m => m.Id == id);

            if (ezn == null)
            {
                return NotFound();
            }

            return Ok(ezn);
        }

        // PUT: api/Ezns/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEzn([FromRoute] int id, [FromBody] Ezn ezn)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != ezn.Id)
            {
                return BadRequest();
            }

            _context.Entry(ezn).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EznExists(id))
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

        // POST: api/Ezns
        [HttpPost]
        public async Task<IActionResult> PostEzn([FromBody] Ezn ezn)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Ezns.Add(ezn);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEzn", new { id = ezn.Id }, ezn);
        }

        // DELETE: api/Ezns/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEzn([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ezn = await _context.Ezns.SingleOrDefaultAsync(m => m.Id == id);
            if (ezn == null)
            {
                return NotFound();
            }

            _context.Ezns.Remove(ezn);
            await _context.SaveChangesAsync();

            return Ok(ezn);
        }

        private bool EznExists(int id)
        {
            return _context.Ezns.Any(e => e.Id == id);
        }
    }
}