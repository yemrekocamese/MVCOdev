using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using MVCHomework.Data;

namespace dummyWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly Context _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, Context context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.Products.ToList());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Details(int id)
        {
            return View(_context.Products.FirstOrDefault(p => p.Id == id));
        }
    }
}