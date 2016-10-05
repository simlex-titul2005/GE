using GE.WebUI.Models;
using GE.WebUI.ViewModels;
using SX.WebCore.Repositories;
using GE.WebUI.Infrastructure.Repositories;
using System.Web.Mvc;

namespace GE.WebUI.Areas.Admin.Controllers
{
    public sealed class ArticlesController : MaterialsController<Article, VMArticle>
    {
        private static RepoArticle _repo = new RepoArticle();
        public ArticlesController() : base(SX.WebCore.Enums.ModelCoreType.Article) { }
        public override SxRepoMaterial<Article, VMArticle> Repo
        {
            get
            {
                return _repo;
            }
        }

        private static readonly string _title = "Статьи";

        public override ActionResult Index(int page = 1)
        {
            ViewBag.Title = _title;
            return base.Index(page);
        }

        public override ActionResult Edit(int? id = null)
        {
            ViewBag.Scripts = "$('#GameId').sx_gvl()";
            ViewBag.Title = _title;
            return base.Edit(id);
        }
    }
}