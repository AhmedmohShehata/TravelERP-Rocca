using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TravelERP.Data;
using TravelERP.Models;

namespace TravelERP.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;

        }

        public async Task<IActionResult> Index()
        {
            ViewData["M1"] =(await _context.Developer.SingleOrDefaultAsync(a => a.Title == "M1")).Message;
            ViewData["M2"] = (await _context.Developer.SingleOrDefaultAsync(a => a.Title == "M2")).Message;
            ViewData["M3"] = (await _context.Developer.SingleOrDefaultAsync(a => a.Title == "M3")).Message;
            ViewData["M4"] = (await _context.Developer.SingleOrDefaultAsync(a => a.Title == "M4")).Message;
            ViewData["HomePhoto"] = "~/" + TravelERP.Properties.Resources.ImgFolder + "/Home.jpg";
            ViewData["HomePhoto2"] = "~/" + TravelERP.Properties.Resources.ImgFolder + "/Home2.jpg";

            ViewData["AhmedPhoto"] = "~/" + TravelERP.Properties.Resources.ImgFolder + "/Ahmed.jpg";
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Company_Name");

            return View();

        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }
        [AllowAnonymous]
        public IActionResult CheckOut()
        {
            return View();
        }
        [AllowAnonymous]

        public IActionResult Payment()
        {
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
