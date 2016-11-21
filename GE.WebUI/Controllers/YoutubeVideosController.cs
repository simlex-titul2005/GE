using System.Threading.Tasks;
using System.Web.Mvc;
using GE.WebUI.Infrastructure;
using SX.WebCore.MvcControllers.Youtube;
using SX.WebCore;
using SX.WebCore.ViewModels.Youtube;
using System;

namespace GE.WebUI.Controllers
{
    public sealed class YoutubeVideosController : SxYoutubeVideosController
    {
        public YoutubeVideosController()
        {
            FillBreadcrumbs = BreadcrumbsManager.WriteBreadcrumbs;
        }

#if !DEBUG
        [OutputCache(Duration =7200)]
#endif
        [HttpGet]
        public override Task<ActionResult> List(int cat=20, int amount=9)
        {
            return base.List(cat, amount);
        }

#if !DEBUG
        [OutputCache(Duration =7200, VaryByParam ="amount")]
#endif
        public async Task<ActionResult> PopularForHumor(int amount=20)
        {
            var filter = new SxFilter(1, amount)
            {
                AddintionalInfo = new object[] {
                new SxVMYotubeVideoFilter { CategoryId=23, RegionCode="ru" }
            }
            };

            var viewModel = await Repo.GetPopularAsync(filter);
            ViewBag.Date = DateTime.UtcNow.AddHours(2).ToString("yyyy-MM-dd hh:mm:ss");

            return PartialView("_PopularForHumor", viewModel);
        }
    }
}