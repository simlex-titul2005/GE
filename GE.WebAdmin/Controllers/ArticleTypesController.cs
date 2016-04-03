using GE.WebAdmin.Models;
using GE.WebCoreExtantions;
using GE.WebCoreExtantions.Repositories;
using SX.WebCore.Abstract;
using SX.WebCore.HtmlHelpers;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using GE.WebAdmin.Extantions.Repositories;

namespace GE.WebAdmin.Controllers
{
    public partial class ArticleTypesController : BaseController
    {
        SxDbRepository<int, ArticleType, DbContext> _repo;
        public ArticleTypesController()
        {
            _repo = new RepoArticleType();
        }

        private static int _pageSize = 20;

        [AcceptVerbs(HttpVerbs.Get)]
        public virtual ViewResult Index(int page = 1)
        {
            var filter = new WebCoreExtantions.Filter(page, _pageSize);
            var totalItems = (_repo as RepoArticleType).FilterCount(filter);
            filter.PagerInfo.TotalItems = totalItems;
            ViewBag.PagerInfo = filter.PagerInfo;

            var viewModel = (_repo as RepoArticleType).QueryForAdmin(filter);
            return View(viewModel);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public virtual PartialViewResult Index(VMArticleType filterModel, IDictionary<string, SxExtantions.SortDirection> order, int page = 1)
        {
            ViewBag.Filter = filterModel;
            ViewBag.Order = order;

            var filter = new WebCoreExtantions.Filter(page, _pageSize) { Orders = order, WhereExpressionObject = filterModel };
            var totalItems = (_repo as RepoArticleType).FilterCount(filter);
            filter.PagerInfo.TotalItems = totalItems;
            ViewBag.PagerInfo = filter.PagerInfo;

            var viewModel = (_repo as RepoArticleType).QueryForAdmin(filter);

            return PartialView("_GridView", viewModel);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public virtual ViewResult Edit(string name=null, int? gameId=null )
        {
            var model = gameId.HasValue ? _repo.GetByKey(name, gameId) : new ArticleType();
            return View(Mapper.Map<ArticleType, VMEditArticleType>(model));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Edit(VMEditArticleType model)
        {
            var redactModel = Mapper.Map<VMEditArticleType, ArticleType>(model);
            if (ModelState.IsValid)
            {
                ArticleType newModel = null;
                if (model.Id == 0)
                    newModel = _repo.Create(redactModel);
                else
                    newModel = _repo.Update(redactModel, "Description", "Color");
                return RedirectToAction(MVC.ArticleTypes.Index());
            }
            else
                return View(model);
        }

        public virtual ActionResult ArticleTypesByGameId(int? gameId, string curName)
        {
            if (gameId.HasValue)
            {
                var viewModel = (_repo as RepoArticleType).GetArticleTypesByGameId((int)gameId)
                    .Select(x => new SelectListItem
                    {
                        Text = x.Description,
                        Value = x.Name,
                        Selected = x.Name == curName
                    });
                return PartialView(MVC.ArticleTypes.Views._ArticleTypesByGameId, viewModel);
            }
            else return Content("<span class=\"form-control text-danger\">Не поддерживается без выбора игры</span>");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Delete(VMEditArticleType model)
        {
            _repo.Delete(model.Name, model.GameId);
            return RedirectToAction("index");
        }
    }
}