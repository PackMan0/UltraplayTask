using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using AbstractionProvider;
using AbstractionProvider.Interfaces.Services;
using AbstractionProvider.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Options;
using PresentationLayer.Models;

namespace PresentationLayer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISportService _sportService;
        private readonly CacheProvider _cacheProvider;
        private readonly ViewRenderer _viewRenderer;

        public HomeController(ISportService sportService, CacheProvider cacheProvider, ViewRenderer viewRenderer)
        {
            this._sportService = sportService;
            this._cacheProvider = cacheProvider;
            this._viewRenderer = viewRenderer;
        }

        public IActionResult Index()
        {
            var sport = this._sportService.GetAllSportDataAsync();
            
            return View("Index", sport);
        }

        [HttpGet]
        public async Task<IActionResult> GetEventHtml(string cachKey)
        {
            var events = this._cacheProvider.Get<IEnumerable<Event>>(cachKey);

            return await SportDataToHtml(events, "SportExternaID");
        }

        [HttpGet]
        public async Task<IActionResult> GetMatchHtml(string cachKey)
        {
            var matches = this._cacheProvider.Get<IEnumerable<Match>>(cachKey);

            return await SportDataToHtml(matches, "EventExtarnalID");
        }

        [HttpGet]
        public async Task<IActionResult> GetBetHtml(string cachKey)
        {
            var bets = this._cacheProvider.Get<IEnumerable<Bet>>(cachKey);

            return await SportDataToHtml(bets, "MatchExternalID");
        }

        [HttpGet]
        public async Task<IActionResult> GetOddHtml(string cachKey)
        {
            var odds = this._cacheProvider.Get<IEnumerable<Odd>>(cachKey);

            return await SportDataToHtml(odds, "BetExtarnalID");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private async Task<IActionResult> SportDataToHtml(IEnumerable<Base> sporDataColletion, string parentIdPropertyName)
        {
            if (sporDataColletion != null)
            {
                var resultList = new List<UpdateDataModel>();

                foreach (var ev in sporDataColletion)
                {
                    var model = new UpdateDataModel();

                    model.ParentContainerID = (int) ev.GetType().GetProperty(parentIdPropertyName).GetValue(ev);
                    model.DataHtml = await this._viewRenderer.RenderToStringAsync("Home/_EventPatialView", ev);
                }

                return Json(resultList);
            }

            return BadRequest();
        }
    }
}
