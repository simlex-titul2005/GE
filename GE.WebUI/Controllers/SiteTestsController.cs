using GE.WebUI.Infrastructure.Repositories;
using GE.WebUI.Models;
using GE.WebUI.ViewModels;
using SX.WebCore;
using SX.WebCore.Attrubutes;
using SX.WebCore.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace GE.WebUI.Controllers
{
    [AllowAnonymous]
    public sealed class SiteTestsController : BaseController
    {
        private static RepoSiteTest _repo = new RepoSiteTest();
        public static RepoSiteTest Repo
        {
            get { return _repo; }
            set { _repo = value; }
        }

        public SiteTestsController()
        {
            //WriteBreadcrumbs = BreadcrumbsManager.WriteBreadcrumbs;
        }

        [HttpGet]
        public ViewResult List(int page = 1)
        {
            var defaultOrder = new SxOrder { FieldName = "DateCreate", Direction = SortDirection.Desc };
            var filter = new SxFilter { Order = defaultOrder, OnlyShow = true };

            var data = Repo.Read(filter);

            var viewModel = new SxPagedCollection<VMSiteTest>
            {
                Collection = data,
                PagerInfo = filter.PagerInfo
            };

            ViewBag.Filter = filter;

            return View(model: viewModel);
        }

#if !DEBUG
        [OutputCache(Duration =900)]
#endif
        [ChildActionOnly]
        public PartialViewResult RandomList()
        {
            var data = Repo.RandomList();
            return PartialView("_RandomList", data);
        }

        [HttpGet]
        public async Task<ActionResult> Details(string titleUrl)
        {
            var data = Repo.GetSiteTestPage(titleUrl);
            if (data == null) return new HttpNotFoundResult();

            var breadcrumbs = (SxVMBreadcrumb[])ViewBag.Breadcrumbs;
            if (breadcrumbs != null)
            {
                var bc = breadcrumbs.ToList();
                bc.Add(new SxVMBreadcrumb { Title = data.Question.Test.Title });
                ViewBag.Breadcrumbs = bc.ToArray();
            }

            if (data.Question.Test.Type == SiteTest.SiteTestType.Guess)
            {
                var step = new VMSiteTestStepGuess();
                step.QuestionId = data.QuestionId;
                step.IsCorrect = false;
                ViewBag.OldSteps = new VMSiteTestStepGuess[] { step };
            }
            else if (data.Question.Test.Type == SiteTest.SiteTestType.Normal || data.Question.Test.Type == SiteTest.SiteTestType.NormalImage)
            {
                var step = new VMSiteTestStepNormal();
                step.SubjectId = data.SubjectId;
                step.QuestionId = 0;
                step.LettersCount = getStepNormalLettersCount(data);
                ViewBag.OldSteps = new VMSiteTestStepNormal[] { step };
            }

            var viewModel = Mapper.Map<SiteTestAnswer, VMSiteTestAnswer>(data);
            if (viewModel.Question != null && viewModel.Question.Test != null)
                viewModel.Question.Test.ViewsCount = await Repo.AddShow(viewModel.Question.Test.Id);

            return View(model: viewModel);
        }


        [HttpPost, ValidateAntiForgeryToken, NotLogRequest]
        public async Task<ActionResult> StepGuess(List<VMSiteTestStepGuess> steps)
        {
            return await Task.Run(() =>
            {
                int subjectsCount;
                var data = Repo.GetGuessStep(steps, out subjectsCount);
                ViewBag.SubjectsCount = subjectsCount;
                steps.Add(new VMSiteTestStepGuess { QuestionId = data.QuestionId, IsCorrect = false });
                ViewBag.OldSteps = steps.ToArray();
                var viewModel = Mapper.Map<SiteTestAnswer, VMSiteTestAnswer>(data);
                return PartialView("_StepGuess", viewModel);
            });
        }

        [HttpPost, ValidateAntiForgeryToken, NotLogRequest]
        public async Task<ActionResult> StepNormal(List<VMSiteTestStepNormal> steps)
        {
            return await Task.Run(() =>
            {
                int subjectsCount;
                int allSubjectsCount;
                var data = Repo.GetNormalStep(steps, out subjectsCount, out allSubjectsCount);
                ViewBag.SubjectsCount = subjectsCount;
                if (data != null)
                    steps.Add(new VMSiteTestStepNormal
                    {
                        SubjectId = data.SubjectId,
                        QuestionId = 0,
                        LettersCount = getStepNormalLettersCount(data)
                    });
                ViewBag.OldSteps = steps.ToArray();
                ViewBag.AllSubjectsCount = allSubjectsCount;

                var viewModel = Mapper.Map<SiteTestAnswer, VMSiteTestAnswer>(data);
                return PartialView("_StepNormal", viewModel);
            });
        }
        private static int getStepNormalLettersCount(SiteTestAnswer answer)
        {
            var test = answer.Question.Test;
            var questions = test.Questions;
            if (questions == null || !questions.Any()) return 0;

            var result = 0;
            foreach (var q in questions)
            {
                result += q.Text.Length;
            }

            if (test.Type == SiteTest.SiteTestType.NormalImage && answer.Subject != null && answer.Subject.Description != null)
                result += answer.Subject.Description.Length;

            return result;
        }

        [HttpPost, AllowAnonymous]
        public async Task<PartialViewResult> ResultNormal(List<VMSiteTestStepNormal> steps)
        {
            return await Task.Run(() =>
            {
                var validSteps = steps.Where(x => x.QuestionId != 0).ToArray();
                SiteTestAnswer answer;
                VMSiteTestStepNormal userAnswer;
                var result = new VMSiteTestResult<VMSiteTestResultNormal>();
                var data = _repo.GetNormalResults(steps.First().SubjectId);
                var test = data.First().Question.Test;
                result.SiteTestTitle = test.Title;
                result.SiteTestUrl = Url.Action("Details", "SiteTests", new { titleUrl = test.TitleUrl });
                result.Results = new VMSiteTestResultNormal[validSteps.Length];

                for (int i = 0; i < validSteps.Length; i++)
                {
                    userAnswer = validSteps[i];
                    answer = data.First(x => x.SubjectId == userAnswer.SubjectId);

                    result.Results[i] = new VMSiteTestResultNormal
                    {
                        SubjectTitle = answer.Subject.Title,
                        QuestionText = data.First(x => x.QuestionId == userAnswer.QuestionId).Question.Text,
                        IsCorrect = answer.QuestionId == userAnswer.QuestionId,
                        Step = userAnswer
                    };

                    var isCorrect = answer.QuestionId == userAnswer.QuestionId;
                    result.BallsCount += userAnswer.BallsSubjectShow + (isCorrect ? userAnswer.BallsGoodRead : 0) + userAnswer.BallsBadRead + (isCorrect ? 15 : 0);
                }


                return PartialView("_ResultNormal", result);
            });
        }
    }
}
