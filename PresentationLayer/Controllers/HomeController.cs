using System.Diagnostics;
using System.Threading.Tasks;
using AbstractionProvider.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Models;

namespace PresentationLayer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISportService _sportService;

        public HomeController(ISportService sportService)
        {
            this._sportService = sportService;
        }

        public IActionResult Index()
        {
            var sport = this._sportService.GetAllSportDataAsync();
            
            return View("Index", sport);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
