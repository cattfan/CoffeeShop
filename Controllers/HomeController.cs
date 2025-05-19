using System.Diagnostics;
using coffeeshop.Models.Services;
using Coffeeshop.Models.interfaces;
using CoffeeShop.Models;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IProductRepository productRepository;
        public HomeController(ILogger<HomeController> logger, IProductRepository productRepository)
        {
            _logger = logger;
            this.productRepository = productRepository;
        }

        public IActionResult Index()
        {
            return View(productRepository.GetTrendingProducts());
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
