using GE.WebCoreExtantions;
using GE.WebCoreExtantions.Repositories;
using SX.WebCore.Abstract;
using System.Web.Mvc;
using System.Linq;
using GE.WebAdmin.Models;
using Microsoft.AspNet.Identity;
using static SX.WebCore.UrlHelperExtensions;
using GE.WebAdmin.Extantions.Repositories;
using static SX.WebCore.Enums;
using SX.WebCore.Repositories;
using GE.WebAdmin.Infrastructure.ReportAdapters;
using SX.WebCore;

namespace GE.WebAdmin.Controllers
{
    [Authorize(Roles = "admin")]
    public partial class AphorismsController : BaseController
    {
        private static int _pageSize = 10;
        private static SxDbRepository<int, Aphorism, DbContext> _repo;
        public AphorismsController()
        {
            if(_repo==null)
                _repo = new RepoAphorism();
        }

        [HttpGet]
        public virtual ViewResult Index(int page = 1, string curCat=null)
        {
            if(curCat != null)
            {
                var category = new SxRepoMaterialCategory<DbContext>().GetByKey(curCat);
                ViewBag.Category = category;
            }

            ViewBag.CategoryId = curCat;
            var filter = new SxFilter(page, _pageSize) { WhereExpressionObject=new VMAphorism { CategoryId= curCat } };
            var totalItems = (_repo as RepoAphorism).Count(filter);
            filter.PagerInfo.TotalItems = totalItems;
            ViewBag.Filter = filter;

            var data = (_repo as RepoAphorism).Query(filter).ToArray();
            var viewModel = data.Select(x=>Mapper.Map<Aphorism, VMAphorism>(x)).ToArray();

            return View(viewModel);
        }

        [HttpPost]
        public virtual PartialViewResult Index(string curCat, VMAphorism filterModel, SxOrder order, int page = 1)
        {
            filterModel.CategoryId = curCat;
            ViewBag.CategoryId = curCat;

            var filter = new SxFilter(page, _pageSize) { Order = order, WhereExpressionObject = filterModel };
            var totalItems = (_repo as RepoAphorism).Count(filter);
            filter.PagerInfo.TotalItems = totalItems;
            ViewBag.Filter = filter;

            var viewModel = (_repo as RepoAphorism).Query(filter).ToArray().Select(x => Mapper.Map<Aphorism, VMAphorism>(x)).ToArray();

            return PartialView("~/views/Aphorisms/_GridView.cshtml", viewModel);
        }

        [HttpGet]
        public virtual ViewResult Edit(string cat, ModelCoreType mct, int? id = null)
        {
            var data = id.HasValue ? _repo.GetByKey(id, mct) : new Aphorism { CategoryId = cat, Show=true };
            var viewModel = Mapper.Map<Aphorism, VMEditAphorism>(data);
            return View(viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public virtual ActionResult Edit(VMEditAphorism model)
        {
            if (string.IsNullOrEmpty(model.TitleUrl))
            {
                var titleUrl = Url.SeoFriendlyUrl(model.Title);
                var existModel = (_repo as RepoAphorism).GetByTitleUrl(titleUrl);
                if (existModel != null && existModel.Id != model.Id)
                    ModelState.AddModelError("Title", "Строковый ключ должен быть уникальным");
                else
                {
                    model.TitleUrl = titleUrl;
                    ModelState.Remove("TitleUrl");
                }
            }

            var isNew = model.Id == 0;
            var redactModel = Mapper.Map<VMEditAphorism, Aphorism>(model);
            redactModel.ModelCoreType = ModelCoreType.Aphorism;
            redactModel.UserId = User.Identity.GetUserId();
            if (ModelState.IsValid)
            {
                if (isNew)
                    _repo.Create(redactModel);
                else
                    _repo.Update(redactModel, true, "Title", "Html", "AuthorId", "Show", "SourceUrl");
                return RedirectToAction("index", new { curCat= model.CategoryId });
            }
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public virtual RedirectToRouteResult Delete(string cat, int id, ModelCoreType mct)
        {
            _repo.Delete(id, mct);
            return RedirectToAction("index", new { curCat= cat });
        }

        [HttpPost]
        public virtual void Report()
        {
            using (var ra = new ReportAphorismsAdapter())
            {
                ra.SendReport(Response, "aphorisms");
            }
        }
    }
}