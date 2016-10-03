using GE.WebUI.Models;
using GE.WebUI.ViewModels;
using SX.WebCore.MvcControllers;
using SX.WebCore.Repositories;
using GE.WebUI.Infrastructure.Repositories;
using System.Web.Mvc;

namespace GE.WebUI.Areas.Admin.Controllers
{
    public sealed class NewsController : SxMaterialsController<News, VMNews>
    {
        public NewsController() : base(SX.WebCore.Enums.ModelCoreType.News) { }
        public override SxRepoMaterial<News, VMNews> Repo
        {
            get
            {
                return new RepoNews();
            }
        }

        private static readonly string _title = "Новости";

        public override ActionResult Index(int page = 1)
        {
            ViewBag.Title = _title;
            return base.Index(page);
        }

        public override ActionResult Edit(int? id = default(int?))
        {
            ViewBag.Scripts = "$('#GameId').sx_gvl()";
            ViewBag.Title = _title;
            return base.Edit(id);
        }
    }
}