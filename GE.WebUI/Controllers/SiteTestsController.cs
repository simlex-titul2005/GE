﻿using GE.WebCoreExtantions;
using SX.WebCore;
using System.Web.Mvc;
using System.Linq;
using System.Collections.Generic;
using SX.WebCore.ViewModels;
using static SX.WebCore.HtmlHelpers.SxExtantions;
using System.Threading.Tasks;
using SX.WebCore.MvcControllers;
using GE.WebUI.Infrastructure;
using SX.WebCore.Attrubutes;

namespace GE.WebUI.Controllers
{
    public sealed class SiteTestsController : SxSiteTestsController<DbContext>
    {
        public SiteTestsController()
        {
            WriteBreadcrumbs = BreadcrumbsManager.WriteBreadcrumbs;
        }

        [HttpGet, AllowAnonymous]
        public ViewResult List(int page = 1)
        {
            var defaultOrder = new SxOrder { FieldName = "DateCreate", Direction = SortDirection.Desc };
            var filter = new SxFilter { Order = defaultOrder, OnlyShow = true };

            var data = Repo.Read(filter).Select(x => Mapper.Map<SxSiteTest, SxVMSiteTest>(x)).ToArray();

            var viewModel = new SxPagedCollection<SxVMSiteTest>
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
        [ChildActionOnly, AllowAnonymous]
        public PartialViewResult RandomList()
        {
            var data = Repo.RandomList();
            return PartialView("_RandomList", data);
        }

        [HttpGet, AllowAnonymous]
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

            if (data.Question.Test.Type == SxSiteTest.SiteTestType.Guess)
            {
                var step= new SxVMSiteTestStepGuess();
                step.QuestionId = data.QuestionId;
                step.IsCorrect = false;
                ViewBag.OldSteps = new SxVMSiteTestStepGuess[] { step };
            }
            else if(data.Question.Test.Type == SxSiteTest.SiteTestType.Normal || data.Question.Test.Type == SxSiteTest.SiteTestType.NormalImage )
            {
                var step = new SxVMSiteTestStepNormal();
                step.SubjectId = data.SubjectId;
                step.QuestionId = 0;
                step.LettersCount = getStepNormalLettersCount(data);
                ViewBag.OldSteps = new SxVMSiteTestStepNormal[] { step };
            }
            
            var viewModel = Mapper.Map<SxSiteTestAnswer, SxVMSiteTestAnswer>(data);
            if(viewModel.Question!=null && viewModel.Question.Test!=null)
                viewModel.Question.Test.ViewsCount= await Repo.AddShow(viewModel.Question.Test.Id);

            return View(model: viewModel);
        }
        

        [HttpPost, ValidateAntiForgeryToken, AllowAnonymous, NotLogRequest]
        public async Task<ActionResult> StepGuess(List<SxVMSiteTestStepGuess> steps)
        {
            return await Task.Run(() =>
            {
                int subjectsCount;
                var data = Repo.GetGuessStep(steps, out subjectsCount);
                ViewBag.SubjectsCount = subjectsCount;
                steps.Add(new SxVMSiteTestStepGuess { QuestionId = data.QuestionId, IsCorrect = false });
                ViewBag.OldSteps = steps.ToArray();
                var viewModel = Mapper.Map<SxSiteTestAnswer, SxVMSiteTestAnswer>(data);
                return PartialView("_StepGuess", viewModel);
            });
        }

        [HttpPost, ValidateAntiForgeryToken, AllowAnonymous, NotLogRequest]
        public async Task<ActionResult> StepNormal(List<SxVMSiteTestStepNormal> steps)
        {
            return await Task.Run(() =>
            {
                int subjectsCount;
                int allSubjectsCount;
                var data = Repo.GetNormalStep(steps, out subjectsCount, out allSubjectsCount);
                ViewBag.SubjectsCount = subjectsCount;
                if(data!=null)
                    steps.Add(new SxVMSiteTestStepNormal {
                        SubjectId = data.SubjectId,
                        QuestionId = 0,
                        LettersCount=getStepNormalLettersCount(data)
                    });
                ViewBag.OldSteps = steps.ToArray();
                ViewBag.AllSubjectsCount = allSubjectsCount;

                var viewModel = Mapper.Map<SxSiteTestAnswer, SxVMSiteTestAnswer>(data);
                return PartialView("_StepNormal", viewModel);
            });
        }
        private static int getStepNormalLettersCount(SxSiteTestAnswer answer)
        {
            var test = answer.Question.Test;
            var questions = test.Questions;
            if (questions==null || !questions.Any()) return 0;

            var result = 0;
            foreach (var q in questions)
            {
                result += q.Text.Length;
            }

            if (test.Type == SxSiteTest.SiteTestType.NormalImage && answer.Subject != null && answer.Subject.Description != null)
                result += answer.Subject.Description.Length;

            return result;
        }
    }
}