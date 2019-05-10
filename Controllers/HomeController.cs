using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FacturaOnline.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace FacturaOnline.Controllers
{
    public class HomeController : Controller
    {

        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        //GET: Partners
        [Authorize(Roles = "Admin")]
        public IActionResult Partners()
        {
            return View(model: _context.Partners.ToList());
        }


        // GET: /<controller>/
        public IActionResult StartPage()
        {
            return View();
        }

        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
