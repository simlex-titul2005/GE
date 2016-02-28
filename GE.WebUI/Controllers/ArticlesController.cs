using GE.WebCoreExtantions;
using GE.WebCoreExtantions.Repositories;
using GE.WebUI.Models;
using SX.WebCore.Abstract;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GE.WebUI.Extantions.Repositories;
using SX.WebCore;

namespace GE.WebUI.Controllers
{
    public partial class ArticlesController : MaterialsController<int, Article>
    {
        public ArticlesController() : base(Enums.ModelCoreType.Article) { }

        [ChildActionOnly]
        public virtual PartialViewResult ForGamersBlock()
        {
            var viewModel = (base.Repository as RepoArticle).ForGamersBlock();
            return PartialView(MVC.Articles.Views._ForGamersBlock, viewModel);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public virtual ActionResult Preview(int gameId, string articleType, int lettersCount)
        {
            if (!Request.IsAjaxRequest()) return null;

            var viewModel = (base.Repository as RepoArticle).PreviewMaterials(gameId, articleType, lettersCount);
            if (!viewModel.Any()) return Content("<div class=\"empty-result\">Данные отсутствуют</div>");
            return PartialView(MVC.Articles.Views._Preview, viewModel);
        }

        [ChildActionOnly]
        [OutputCache(Duration = 900, VaryByParam = "amount")]
        public virtual PartialViewResult Last(int amount=3)
        {
            var viewModel = (base.Repository as RepoArticle).Last(amount);
            return PartialView(MVC.Articles.Views._Last, viewModel);
        }
    }
}