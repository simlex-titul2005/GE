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
        private static SxDbRepository<int, SxSiteTest, DbContext> _repo;
        public SiteTestsController()
        {
            if (_repo == null)
                _repo = new SxRepoSiteTest<DbContext>();
        }

        [HttpGet]
        public ViewResult List(int page=1)
        {
            var defaultOrder = new SxOrder { FieldName = "DateCreate", Direction = SortDirection.Desc };
            var filter = new SxFilter { Order = defaultOrder };
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

            var viewModel = Mapper.Map<SxSiteTestQuestion, SxVMSiteTestQuestion>(data);
            return View(model: viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Step(string ttu, List<SxVMSiteTestStep> pastQ = null)
        {
            //ViewBag.PastQ = pastQ.Select(x => new { T = x.Question.Text, C = x.Question.IsCorrect, O = x.Order }).Distinct()
            //    .Select(x => new SxVMSiteTestStep {
            //        Order=x.O,
            //        Question=new SxVMSiteTestQuestion { Text=x.T, IsCorrect=x.C }
            //    }).ToList();

            var blocksCount = -1;
            SxVMSiteTestQuestion data = getGuessYesNoStep(ttu, pastQ, out blocksCount);
            ViewBag.BlocksCount = blocksCount;
            return PartialView("_Step", data);
        }
        private static SxVMSiteTestQuestion getGuessYesNoStep(string ttu, List<SxVMSiteTestStep> pastQ, out int blocksCount)
        {
            var select = pastQ.Select(x => Mapper.Map<SxVMSiteTestStep, SxSiteTestStep>(x)).ToList();
            var data = (_repo as SxRepoSiteTest<DbContext>).GetGuessYesNoStep(ttu, select, out blocksCount);
            var viewModel = Mapper.Map<SxSiteTestQuestion, SxVMSiteTestQuestion>(data);
            return viewModel;
        }
    }
}