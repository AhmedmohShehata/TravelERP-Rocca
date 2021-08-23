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
    [Route("api/CustomerSuppliers")]
    public class CustomerSuppliersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _usermanager;

        public CustomerSuppliersController(ApplicationDbContext context, UserManager<ApplicationUser> usermanager)
        {
            _context = context;
            _usermanager = usermanager;

        }

        // GET: api/CustomerSuppliers
        [HttpGet]
        public async Task<IEnumerable<CustomerSupplierViewModel>> GetCustomersSuppliers()
        {
            var CompanyId =(await _context.Users.SingleOrDefaultAsync(a => a.Id == _usermanager.GetUserId(User))).CompanyId;
            var CustomersSuppliers =await( from x in _context.CustomersSuppliers.Where(a => a.CompanyId == CompanyId) select new CustomerSupplierViewModel {Id = x.Id , Name = x.Name , PhoneNumber1 = x .PhoneNumber1 , PhoneNumber2 = x.PhoneNumber2 , PassportNo = x.PassportNo , CustomerOrSupplier = x.CustomerOrSupplier.Name  }).ToListAsync();
            return CustomersSuppliers;
        }

        // GET: api/CustomerSuppliers/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomerSupplier([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customerSupplier = await _context.CustomersSuppliers.SingleOrDefaultAsync(m => m.Id == id);

            if (customerSupplier == null)
            {
                return NotFound();
            }

            return Ok(customerSupplier);
        }

        // PUT: api/CustomerSuppliers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomerSupplier([FromRoute] int id, [FromBody] CustomerSupplier customerSupplier)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != customerSupplier.Id)
            {
                return BadRequest();
            }

            _context.Entry(customerSupplier).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerSupplierExists(id))
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

        // POST: api/CustomerSuppliers
        [HttpPost]
        public async Task<IActionResult> PostCustomerSupplier([FromBody] CustomerSupplier customerSupplier)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.CustomersSuppliers.Add(customerSupplier);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCustomerSupplier", new { id = customerSupplier.Id }, customerSupplier);
        }

        // DELETE: api/CustomerSuppliers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomerSupplier([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customerSupplier = await _context.CustomersSuppliers.SingleOrDefaultAsync(m => m.Id == id);
            if (customerSupplier == null)
            {
                return NotFound();
            }

            _context.CustomersSuppliers.Remove(customerSupplier);
            await _context.SaveChangesAsync();

            return Ok(customerSupplier);
        }

        private bool CustomerSupplierExists(int id)
        {
            return _context.CustomersSuppliers.Any(e => e.Id == id);
        }
    }
}