﻿using GE.WebUI.Infrastructure;
using GE.WebUI.Infrastructure.Repositories;
using GE.WebUI.Models;
using GE.WebUI.ViewModels;
using SX.WebCore;
using SX.WebCore.Attributes;
using SX.WebCore.MvcControllers.Abstract;
using SX.WebCore.ViewModels;
using System;
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
        public static RepoSiteTest Repo { get; set; } = new RepoSiteTest();
        protected override Action<SxBaseController, HashSet<SxVMBreadcrumb>> FillBreadcrumbs => BreadcrumbsManager.WriteBreadcrumbs;

        private static readonly int _pageSize = 12;
        [HttpGet]
        public async Task<ViewResult> List(int page = 1)
        {
            var defaultOrder = new SxOrderItem { FieldName = "DateCreate", Direction = SortDirection.Desc };
            var filter = new SxFilter(page, _pageSize) { Order = defaultOrder, OnlyShow = true };

            var data = await Repo.ReadAsync(filter);

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

            if (data.Question.Test.Type == SiteTest.SiteTestType.Guess)
            {
                var step = new VMSiteTestStepGuess();
                step.QuestionId = data.QuestionId;
                step.IsCorrect = false;
                ViewBag.OldSteps = new VMSiteTestStepGuess[] { step };
            }
            else if (data.Question.Test.Type == SiteTest.SiteTestType.Normal || data.Question.Test.Type == SiteTest.SiteTestType.NormalImage)
            {
                var step = new VMSiteTestStepNormal
                {
                    SubjectId = data.SubjectId,
                    QuestionId = 0,
                    LettersCount = GetStepNormalLettersCount(data)
                };
                ViewBag.OldSteps = new[] { step };
            }

            var viewModel = Mapper.Map<SiteTestAnswer, VMSiteTestAnswer>(data);
            if (viewModel.Question?.Test != null)
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
                        LettersCount = GetStepNormalLettersCount(data)
                    });
                ViewBag.OldSteps = steps.ToArray();
                ViewBag.AllSubjectsCount = allSubjectsCount;

                var viewModel = Mapper.Map<SiteTestAnswer, VMSiteTestAnswer>(data);
                return PartialView("_StepNormal", viewModel);
            });
        }
        private static int GetStepNormalLettersCount(SiteTestAnswer answer)
        {
            var test = answer.Question.Test;
            var questions = test.Questions;
            if (questions == null || !questions.Any()) return 0;

            var result = questions.Sum(q => q.Text.Length);

            if (test.Type == SiteTest.SiteTestType.NormalImage && answer.Subject?.Description != null)
                result += answer.Subject.Description.Length;

            return result;
        }

        [HttpPost, AllowAnonymous]
        public async Task<PartialViewResult> ResultNormal(List<VMSiteTestStepNormal> steps)
        {
            return await Task.Run(() =>
            {
                var validSteps = steps.Where(x => x.QuestionId != 0).ToArray();
                VMSiteTestStepNormal userAnswer;
                var result = new VMSiteTestResult<VMSiteTestResultNormal>();
                var data = Repo.GetNormalResults(steps.First().SubjectId);
                var test = data.First().Question.Test;
                result.SiteTestTitle = test.Title;
                result.SiteTestUrl = Url.Action("Details", "SiteTests", new { titleUrl = test.TitleUrl });
                result.Results = new VMSiteTestResultNormal[validSteps.Length];

                for (int i = 0; i < validSteps.Length; i++)
                {
                    userAnswer = validSteps[i];
                    var answer = data.First(x => x.SubjectId == userAnswer.SubjectId);

                    result.Results[i] = new VMSiteTestResultNormal
                    {
                        SubjectTitle = answer.Subject.Title,
                        QuestionText = data.First(x => x.QuestionId == userAnswer.QuestionId).Question.Text,
                        IsCorrect = answer.QuestionId == userAnswer.QuestionId,
                        Step = userAnswer
                    };

                    var isCorrect = answer.QuestionId == userAnswer.QuestionId;
                    result.BallsCount += userAnswer.BallsSubjectShow + (isCorrect ? userAnswer.BallsGoodRead : 0) + userAnswer.BallsBadRead + (isCorrect ? (test?.Settings?.DefCorrectAnswerBals ?? 15) : 0);
                }


                return PartialView("_ResultNormal", result);
            });
        }

        [HttpPost, NotLogRequest, AllowAnonymous]
        public async Task<JsonResult> Rules(int testId)
        {
            var data = await Repo.GetSiteTestRulesAsync(testId);
            return Json(new
            {
                Title = data.Title,
                Rules = data.Rules
            });
        }
    }
}
