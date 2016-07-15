using GE.WebCoreExtantions;
using SX.WebCore;
using SX.WebCore.Abstract;
using SX.WebCore.Repositories;
using System.Web.Mvc;
using System.Linq;
using System.Collections.Generic;
using SX.WebCore.ViewModels;
using static SX.WebCore.HtmlHelpers.SxExtantions;
using System.Threading.Tasks;
using GE.WebUI.Models;

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
        public ViewResult List(int page = 1)
        {
            var defaultOrder = new SxOrder { FieldName = "DateCreate", Direction = SortDirection.Desc };
            var filter = new SxFilter { Order = defaultOrder, OnlyShow = true };
            filter.PagerInfo.TotalItems = _repo.Count(filter);
            ViewBag.Filter = filter;
            var data = _repo.Query(filter).Select(x => Mapper.Map<SxSiteTest, SxVMSiteTest>(x)).ToArray();

            var viewModel = new SxPagedCollection<SxVMSiteTest>
            {
                Collection = data,
                PagerInfo = filter.PagerInfo
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

            var breadcrumbs = (VMBreadcrumb[])ViewBag.Breadcrumbs;
            if (breadcrumbs != null)
            {
                var bc = breadcrumbs.ToList();
                bc.Add(new VMBreadcrumb { Title = data.Question.Test.Title });
                ViewBag.Breadcrumbs = bc.ToArray();
            }

            var step = new SxVMSiteTestStep();
            if (data.Question.Test.Type == SxSiteTest.SiteTestType.Guess)
            {
                step.QuestionId = data.QuestionId;
                step.IsCorrect = false;
            }
            else if(data.Question.Test.Type == SxSiteTest.SiteTestType.Normal)
            {
                step.SubjectId = data.SubjectId;
                step.QuestionId = 0;
                ViewBag.LettersCount = getSiteTestQuestionsLettersCount(data.Question.Test.Questions);
            }

            ViewBag.OldSteps = new SxVMSiteTestStep[] { step };
            var viewModel = Mapper.Map<SxSiteTestAnswer, SxVMSiteTestAnswer>(data);
            return View(model: viewModel);
        }
        private static int getSiteTestQuestionsLettersCount(ICollection<SxSiteTestQuestion> questions)
        {
            if (!questions.Any()) return 0;

            var result = 0;
            foreach (var q in questions)
            {
                result += q.Text.Length;
            }
            return result;
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> StepGuess(List<SxVMSiteTestStep> steps)
        {
            return await Task.Run(() =>
            {
                int subjectsCount;
                var data = _repo.GetGuessStep(steps, out subjectsCount);
                ViewBag.SubjectsCount = subjectsCount;
                steps.Add(new SxVMSiteTestStep { QuestionId = data.QuestionId, IsCorrect = false });
                ViewBag.OldSteps = steps.ToArray();
                var viewModel = Mapper.Map<SxSiteTestAnswer, SxVMSiteTestAnswer>(data);
                return PartialView("_StepGuess", viewModel);
            });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> StepNormal(List<SxVMSiteTestStep> steps)
        {
            return await Task.Run(() =>
            {
                int subjectsCount;
                int allSubjectsCount;
                var data = _repo.GetNormalStep(steps, out subjectsCount, out allSubjectsCount);
                ViewBag.SubjectsCount = subjectsCount;
                if(data!=null)
                    steps.Add(new SxVMSiteTestStep { QuestionId = data.QuestionId, SubjectId = data.SubjectId });
                ViewBag.OldSteps = steps.ToArray();
                ViewBag.AllSubjectsCount = allSubjectsCount;
                ViewBag.LettersCount = getSiteTestQuestionsLettersCount(data.Question.Test.Questions);
                var viewModel = Mapper.Map<SxSiteTestAnswer, SxVMSiteTestAnswer>(data);
                return PartialView("_StepNormal", viewModel);
            });
        }
    }
}