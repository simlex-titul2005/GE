using GE.WebCoreExtantions;
using SX.WebCore;
using SX.WebCore.Abstract;
using SX.WebCore.Repositories;
using System.Web.Mvc;
using System.Linq;
using System.Collections.Generic;
using SX.WebCore.ViewModels;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace GE.WebUI.Controllers
{
    public sealed class SiteTestsController : BaseController
    {
        private static SxRepoSiteTest<DbContext> _repo;
        public SiteTestsController()
        {
            if (_repo == null)
                _repo = new SxRepoSiteTest<DbContext>();
        }

        [HttpGet]
        public ViewResult List(int page=1)
        {
            var defaultOrder = new SxOrder { FieldName = "DateCreate", Direction = SortDirection.Desc };
            var filter = new SxFilter { Order = defaultOrder, OnlyShow=true };
            filter.PagerInfo.TotalItems = _repo.Count(filter);
            ViewBag.Filter = filter;
            var data = _repo.Query(filter).Select(x => Mapper.Map<SxSiteTest, SxVMSiteTest>(x)).ToArray();

            var viewModel = new SxPagedCollection<SxVMSiteTest> {
                Collection= data,
                PagerInfo= filter.PagerInfo
            };

            return View(model: viewModel);
        }

#if !DEBUG
        [OutputCache(Duration =900)]
#endif
        [ChildActionOnly]
        public PartialViewResult RandomList()
        {
            var data = (_repo as SxRepoSiteTest<DbContext>).RandomList();
            return PartialView("_RandomList", data);
        }

        [HttpGet]
        public ActionResult Details(string titleUrl)
        {
            var data = (_repo as SxRepoSiteTest<DbContext>).GetSiteTestPage(titleUrl);
            if (data == null) return new HttpNotFoundResult();

            ViewBag.OldSteps = new SxVMSiteTestStep[] { new SxVMSiteTestStep { QuestionId = data.Id, IsCorrect = false } };
            var viewModel = Mapper.Map<SxSiteTestQuestion, SxVMSiteTestQuestion>(data);
            return View(model: viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Step(int testId, List<SxVMSiteTestStep> steps)
        {
            var data = _repo.GetNextStep(testId, steps);
            steps.Add(new SxVMSiteTestStep { QuestionId = data.Id, IsCorrect = false });
            ViewBag.OldSteps = steps.ToArray();
            var viewModel = Mapper.Map<SxSiteTestQuestion, SxVMSiteTestQuestion>(data);
            return PartialView("_Step", viewModel);
        }
    }
}