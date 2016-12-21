using GE.WebUI.Infrastructure.Repositories;
using GE.WebUI.Models;
using GE.WebUI.ViewModels;
using SX.WebCore;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace GE.WebUI.Areas.Admin.Controllers
{
    public sealed class SiteTestQuestionsController : BaseController
    {
        private static RepoSiteTestQuestion _repo = new RepoSiteTestQuestion();
        public static RepoSiteTestQuestion Repo
        {
            get { return _repo; }
            set { _repo = value; }
        }

        private static int _pageSize = 10;

        [HttpPost]
        public ActionResult Index(int testId, VMSiteTestQuestion filterModel, SxOrderItem order, int page = 1)
        {
            var defaultOrder = new SxOrderItem { FieldName = "Text", Direction = SortDirection.Asc };
            var filter = new SxFilter(page, _pageSize) { Order = order == null || order.Direction == SortDirection.Unknown ? defaultOrder : order, WhereExpressionObject = filterModel, AddintionalInfo = new object[] { testId } };

            var viewModel = _repo.Read(filter);
            if (page > 1 && !viewModel.Any())
                return new HttpNotFoundResult();

            ViewBag.Filter = filter;

            return PartialView("_GridView", viewModel);
        }

        [HttpGet]
        public ActionResult Edit(int testId, int? id)
        {
            var model = id.HasValue ? _repo.GetByKey(id) : new SiteTestQuestion() { TestId = testId };
            if (id.HasValue)
            {
                if (model == null)
                    return new HttpNotFoundResult();
            }
            var viewModel = new VMSiteTestQuestion
            {
                Id = model.Id,
                Text = model.Text,
                TestId = model.TestId,
                Test = model.Test != null ? new VMSiteTest
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
        public ActionResult Edit(VMSiteTestQuestion model)
        {
            if (!ModelState.IsValid) return View(model);

            var redactModel = Mapper.Map<VMSiteTestQuestion, SiteTestQuestion>(model);
            var isNew = model.Id == 0;
            redactModel = isNew ? Repo.Create(redactModel) : Repo.Update(redactModel, true, "TestId", "Text");
            return getResult(redactModel.TestId);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<PartialViewResult> Delete(SiteTestQuestion model)
        {
            var testId = model.TestId;
            await _repo.DeleteAsync(model);
            return getResult(testId);
        }

        private PartialViewResult getResult(int testId)
        {
            var defaultOrder = new SxOrderItem { FieldName = "Text", Direction = SortDirection.Asc };
            var filter = new SxFilter(1, _pageSize) { Order = defaultOrder, AddintionalInfo = new object[] { testId } };

            var viewModel = _repo.Read(filter);
            ViewBag.Filter = filter;
            return PartialView("_GridView", viewModel);
        }
    }
}