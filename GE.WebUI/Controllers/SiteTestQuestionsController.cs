﻿using GE.WebUI.Infrastructure.Repositories;
using GE.WebUI.Models;
using GE.WebUI.ViewModels;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace SX.WebCore.MvcControllers
{
    [Authorize(Roles = "admin")]
    public abstract class SiteTestQuestionsController<TDbContext> : SxBaseController<TDbContext> where TDbContext : SxDbContext
    {
        private static RepoSiteTestQuestion _repo=new RepoSiteTestQuestion();
        public static RepoSiteTestQuestion Repo
        {
            get { return _repo; }
            set { _repo = value; }
        }

        private static int _pageSize = 10;

        [HttpPost]
        public virtual ActionResult Index(int testId, VMSiteTestQuestion filterModel, SxOrder order, int page = 1)
        {
            var defaultOrder = new SxOrder { FieldName = "Text", Direction = SortDirection.Asc };
            var filter = new SxFilter(page, _pageSize) { Order = order == null || order.Direction == SortDirection.Unknown ? defaultOrder : order, WhereExpressionObject = filterModel, AddintionalInfo = new object[] { testId } };

            var viewModel = _repo.Read(filter);
            if (page > 1 && !viewModel.Any())
                return new HttpNotFoundResult();

            ViewBag.Filter = filter;

            return PartialView("_GridView", viewModel);
        }

        [HttpGet]
        public virtual ActionResult Edit(int testId, int? id)
        {
            var model = id.HasValue ? _repo.GetByKey(id) : new SiteTestQuestion() { TestId = testId };
            if (id.HasValue)
            {
                if (model == null)
                    return new HttpNotFoundResult();
            }
            var viewModel = new VMSiteTestQuestion {
                Id=model.Id,
                Text=model.Text,
                TestId=model.TestId,
                Test= model.Test != null ? new VMSiteTest
                {
                    Id = model.Test.Id,
                    Description = model.Test.Description,
                    Rules = model.Test.Rules,
                    Show = model.Test.Show,
                    Title = model.Test.Title,
                    TitleUrl = model.Test.TitleUrl,
                    Type = model.Test.Type,
                    DateCreate = model.Test.DateCreate
                } : null
            };
            return PartialView("_Edit", viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public virtual ActionResult Edit(VMSiteTestQuestion model)
        {
            if (ModelState.IsValid)
            {
                var redactModel = Mapper.Map<VMSiteTestQuestion, SiteTestQuestion>(model);
                SiteTestQuestion newModel = null;
                if (model.Id == 0)
                    newModel = _repo.Create(redactModel);
                else
                    newModel = _repo.Update(redactModel, true, "TestId", "Text");

                return getResult(newModel.TestId);
            }
            else
            {
                return View(model);
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public virtual async Task<PartialViewResult> Delete(SiteTestQuestion model)
        {
            var testId = model.TestId;
            await _repo.DeleteAsync(model);
            return getResult(testId);
        }

        private PartialViewResult getResult(int testId)
        {
            var defaultOrder = new SxOrder { FieldName = "Text", Direction = SortDirection.Asc };
            var filter = new SxFilter(1, _pageSize) { Order = defaultOrder, AddintionalInfo = new object[] { testId } };

            var viewModel = _repo.Read(filter);
            ViewBag.Filter = filter;
            return PartialView("_GridView", viewModel);
        }
    }
}
