using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Ortopediya.Models;
using Ortopediya.Models.Entitys;
using Microsoft.EntityFrameworkCore;


namespace Ortopediya.Controllers
{
    public class HomeController : Controller
    {
        private FrontContext db { get; set; }
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            db = new FrontContext(new DbContextOptions<FrontContext>());
            _logger = logger;
        }

        public IActionResult Index()
        {
            db.Database.EnsureCreated();
            return View();
        }

        public IActionResult Contact()
        {
            return View("Contact");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
