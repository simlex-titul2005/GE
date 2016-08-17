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
    public sealed class AphorismsController : BaseController
    {
        private static int _pageSize = 10;
        private static RepoAphorism _repo;
        public AphorismsController()
        {
            if(_repo==null)
                _repo = new RepoAphorism();
        }

        [HttpGet]
        public ViewResult Index(int page = 1, string curCat=null)
        {
            if(curCat != null)
            {
                var category = new SxRepoMaterialCategory<DbContext>().GetByKey(curCat);
                ViewBag.Category = category;
            }

            ViewBag.CategoryId = curCat;
            var filter = new SxFilter(page, _pageSize) { WhereExpressionObject=new VMAphorism { CategoryId= curCat } };
            var data = _repo.Read(filter);
            ViewBag.Filter = filter;
            var viewModel = data.Select(x=>Mapper.Map<Aphorism, VMAphorism>(x)).ToArray();

            return View(viewModel);
        }

        [HttpPost]
        public PartialViewResult Index(string curCat, VMAphorism filterModel, SxOrder order, int page = 1)
        {
            filterModel.CategoryId = curCat;
            ViewBag.CategoryId = curCat;

            var filter = new SxFilter(page, _pageSize) { Order = order, WhereExpressionObject = filterModel };
            var viewModel = _repo.Read(filter).Select(x => Mapper.Map<Aphorism, VMAphorism>(x)).ToArray();
            ViewBag.Filter = filter;

            return PartialView("~/views/Aphorisms/_GridView.cshtml", viewModel);
        }

        [HttpGet]
        public ActionResult Edit(string cat, ModelCoreType mct, int? id = null)
        {
            if (cat == null) return new HttpNotFoundResult();

            var data = id.HasValue ? _repo.GetByKey(id, mct) : new Aphorism { CategoryId = cat, Show=true };
            if (data.Author != null)
                ViewBag.AuthorName = data.Author.Name;
            var viewModel = Mapper.Map<Aphorism, VMEditAphorism>(data);
            return View(viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(VMEditAphorism model)
        {
            if (string.IsNullOrEmpty(model.TitleUrl))
            {
                var titleUrl = Url.SeoFriendlyUrl(model.Title);
                var existModel = _repo.GetByTitleUrl(titleUrl);
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
                    _repo.Update(redactModel, true, "Title", "Html", "Foreword", "AuthorId", "Show", "SourceUrl");
                return RedirectToAction("index", new { curCat= model.CategoryId });
            }
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Delete(string cat, int id, ModelCoreType mct)
        {
            var data = _repo.GetByKey(id, mct);
            if (data == null)
                return new HttpNotFoundResult();

            _repo.Delete(new Aphorism
            {
                Id=id,
                ModelCoreType=mct
            });
            return RedirectToAction("Index", new { curCat= cat });
        }

        [HttpPost]
        public ActionResult Report()
        {
            using (var ra = new ReportAphorismsAdapter())
            {
                ra.SendReport(Response, "aphorisms");
                return new EmptyResult();
            }
        }
    }
}