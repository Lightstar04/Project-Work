using System.Diagnostics;
using InventoryManagement.Models;
using InventoryManagement.Stores;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HomeStore _store;

        public HomeController(ILogger<HomeController> logger, HomeStore store)
        {
            _logger = logger;
            _store = store;
        }

        public async Task<IActionResult> Index(string query)
        {
            var result = await _store.Get(query);

            return View(result);
        }
    }
}
