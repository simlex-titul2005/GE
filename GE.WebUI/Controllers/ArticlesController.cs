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

namespace GE.WebUI.Controllers
{
    public partial class ArticlesController : BaseController
    {
        private SxDbRepository<int, Article, DbContext> _repo;
        public ArticlesController()
        {
            _repo = new RepoArticle();
        }

        [ChildActionOnly]
        [AcceptVerbs(HttpVerbs.Get)]
        public virtual ViewResult ForGamersBlock()
        {
            var viewModel = (_repo as RepoArticle).ForGamersBlock();
            return View(viewModel);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public virtual ActionResult Preview(int gameId, string articleType, int lettersCount)
        {
            if (!Request.IsAjaxRequest()) return null;

            var viewModel = (_repo as RepoArticle).PreviewMaterials(gameId, articleType, lettersCount);
            if (!viewModel.Any()) return Content("<div class=\"empty-result\">Данные отсутствуют</div>");
            return PartialView(MVC.Articles.Views._Preview, viewModel);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public virtual ViewResult Details(int id)
        {
            return View();
        }
    }
}