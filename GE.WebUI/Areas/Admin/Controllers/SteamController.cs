using GE.WebUI.ViewModels;
using SX.WebCore;
using System.Net;
using System.Web.Mvc;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace GE.WebUI.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    public sealed class SteamController : BaseController
    {
        private static VMSteamApp[] _appCache;
        private static readonly int _appPageSize = 20;
        [HttpGet]
        public async Task<ActionResult> AppIndex(int page = 1)
        {
            var filter = new SxFilter(page, _appPageSize);
            var data = await getAppCache();
            var viewModel = data.Skip((page-1)*_appPageSize).Take(_appPageSize).ToArray();
            ViewBag.Filter = filter;
            return View(model: viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> AppIndex(VMSteamApp filterModel, SxOrderItem order, int page = 1)
        {
            var filter = new SxFilter(page, _appPageSize) { WhereExpressionObject= filterModel, Order=order };
            var data = await getAppCache();
            var viewModel = data.Skip((page - 1) * _appPageSize).Take(_appPageSize).ToArray();
            ViewBag.Filter = filter;
            return View(model: viewModel);
        }

        private async Task<VMSteamApp[]> getAppCache()
        {
            if (_appCache != null) return _appCache;

            using (var webClient = new WebClient())
            {
                var json = await webClient.DownloadStringTaskAsync(new System.Uri("http://api.steampowered.com/ISteamApps/GetAppList/v0002?format=json"));
                _appCache = ((JArray)JsonConvert.DeserializeObject<dynamic>(json).applist.apps).Select(x => new VMSteamApp() { AppId = (int)x["appid"], Name = (string)x["name"] }).ToArray();
                return _appCache;
            }
        }
    }
}