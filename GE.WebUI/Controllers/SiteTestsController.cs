using GE.WebCoreExtantions;
using SX.WebCore;
using SX.WebCore.Abstract;
using SX.WebCore.Repositories;
using System.Web.Mvc;
using System.Linq;
using GE.WebUI.Models;
using System.Collections.Generic;
using SX.WebCore.ViewModels;

namespace GE.WebUI.Controllers
{
    public partial class SiteTestsController : BaseController
    {
        private static SxDbRepository<int, SxSiteTest, DbContext> _repo;
        public SiteTestsController()
        {
            if (_repo == null)
                _repo = new SxRepoSiteTest<DbContext>();
        }

#if !DEBUG
        [OutputCache(Duration =900)]
#endif
        [ChildActionOnly]
        public virtual PartialViewResult RandomList()
        {
            var data = (_repo as SxRepoSiteTest<DbContext>).RandomList();
            return PartialView("_RandomList", data);
        }

        [HttpGet]
        public virtual ActionResult Details(string titleUrl)
        {
            var data = (_repo as SxRepoSiteTest<DbContext>).GetSiteTestPage(titleUrl);
            var viewModel = Mapper.Map<SxSiteTestQuestion, SxVMSiteTestQuestion>(data);
            return View(model: viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public virtual ActionResult Step(string ttu, List<SxVMSiteTestStep> pastQ = null)
        {
            ViewBag.PastQ = pastQ.Select(x => new { T = x.Question.Text, C = x.Question.IsCorrect, O = x.Order }).Distinct()
                .Select(x => new SxVMSiteTestStep {
                    Order=x.O,
                    Question=new SxVMSiteTestQuestion { Text=x.T, IsCorrect=x.C }
                }).ToList();

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