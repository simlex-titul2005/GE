using System.Web.Mvc;
using GE.WebUI.Models;
using GE.WebUI.ViewModels;
using SX.WebCore.Attributes;
using System;

namespace GE.WebUI.Controllers
{
    public sealed class AphorismController : BaseController
    {
#if !DEBUG
        [OutputCache(Duration = 3600, VaryByParam = "id")]
#endif
        [HttpGet, NotLogRequest]
        public ActionResult Random(int? id = null)
        {
            // http://www.yugzone.ru/fastread/speed.htm
            var data = AphorismsController.Repo.GetRandom(id);
            
            // минимальное количество секунд на чтение
            var minSecondsCount= Convert.ToInt32(SX.WebCore.MvcApplication.SxMvcApplication.SiteSettingsProvider["aphorism-min-read-seconds-count"]?.Value ?? "5");
            // скорость чтения (знаков в минуту)
            var speed = Convert.ToInt32(SX.WebCore.MvcApplication.SxMvcApplication.SiteSettingsProvider["aphorism-read-speed"]?.Value ?? "900");
            var seconds = Math.Floor((double)(data.Html.Length / (speed / 60)));
            ViewBag.Seconds = seconds < minSecondsCount ? minSecondsCount : seconds;
            var viewModel = Mapper.Map<Aphorism, VMAphorism>(data);
            return PartialView("_Random", viewModel);
        }
    }
}