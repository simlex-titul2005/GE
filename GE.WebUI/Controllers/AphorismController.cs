using System.Web.Mvc;
using GE.WebUI.Models;
using GE.WebUI.ViewModels;
using SX.WebCore.Attrubutes;

namespace GE.WebUI.Controllers
{
    public sealed class AphorismController : BaseController
    {
        [HttpGet, NotLogRequest]
        public ActionResult Random(int? id = null)
        {
            var data = AphorismsController.Repo.GetRandom(id);
            ViewBag.AphorismLettersCount = data.Html.Length;
            var viewModel = Mapper.Map<Aphorism, VMAphorism>(data);
            return PartialView("_Random", viewModel);
        }
    }
}