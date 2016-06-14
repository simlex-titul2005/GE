using GE.WebCoreExtantions;
using SX.WebCore;
using SX.WebCore.Abstract;
using SX.WebCore.Repositories;
using System.Web.Mvc;
using System.Linq;
using GE.WebUI.Models;
using System.Collections.Generic;

namespace GE.WebUI.Controllers
{
    public partial class SiteTestsController : BaseController
    {
        private static SxDbRepository<int, SxSiteTest, DbContext> _repo;
        public SiteTestsController()
        {
            if (_repo == null)
                _repo = new RepoSiteTest<DbContext>();
        }

#if !DEBUG
        [OutputCache(Duration =900)]
#endif
        [ChildActionOnly]
        public virtual PartialViewResult RandomList()
        {
            var data = (_repo as RepoSiteTest<DbContext>).RandomList();
            return PartialView("_RandomList", data);
        }

        [HttpGet]
        public virtual ActionResult Details(string titleUrl)
        {
            var viewModel = getTest(titleUrl);
            if (viewModel == null)
                return new HttpNotFoundResult();

            return View(model: viewModel);
        }

        [HttpPost]
        public virtual ActionResult Details(string titleUrl, string questionText, bool isTrue, List<VMSiteTestQuestion> pastQuestions)
        {
            //pastQuestionTexts
            var pqt = pastQuestions == null? new VMSiteTestQuestion[1] : new VMSiteTestQuestion[pastQuestions.Count+1];
            var q=new VMSiteTestQuestion { Text=questionText, IsCorrect= isTrue };
            if (pastQuestions != null)
            {
                pastQuestions.CopyTo(pqt);
                pqt[pastQuestions.Count] = q;
            }
            else
            {
                pqt[0] = q;
            }
            ViewBag.PastQuestionTexts = pqt.ToArray();

            var viewModel = getTest(titleUrl);
            var blocks = viewModel.Blocks.ToList();
            VMSiteTestQuestion question = null;
            foreach (var block in blocks)
            {
                if (block.Questions == null) continue;
                for (int i = 0; i < pqt.Length; i++)
                {
                    if (block.Questions == null) continue;

                    question = pqt[i];
                    if (block.Questions.SingleOrDefault(x => x.Text == question.Text && x.IsCorrect == question.IsCorrect) == null)
                        block.Questions = null;
                }
            }

            viewModel.Blocks = blocks.Where(x => x.Questions != null).ToArray();

            return PartialView(MVC.SiteTests.Views._Details, viewModel);
        }

        private static VMSiteTest getTest(string titleUrl)
        {
            var data = (_repo as RepoSiteTest<DbContext>).GetSiteTestPage(titleUrl);
            if (!data.Any()) return null;

            var dataTest = data.GroupBy(x => x.Block.Test).FirstOrDefault();
            var viewModel = new VMSiteTest();
            viewModel.Title = dataTest.Key.Title;
            viewModel.Description = dataTest.Key.Description;
            viewModel.TitleUrl = dataTest.Key.TitleUrl;

            viewModel.Blocks = data.GroupBy(x => x.Block).Select(x => new
            {
                Id = x.Key.Id,
                Title = x.Key.Title,
                Description=x.Key.Description
            }).Distinct().Select(x => new VMSiteTestBlock
            {
                Id = x.Id,
                Title = x.Title,
                Description=x.Description,
                Questions = data.Where(q => q.Block.Id == x.Id).Select(q => new VMSiteTestQuestion
                {
                    Id = q.Id,
                    IsCorrect = q.IsCorrect,
                    Text = q.Text
                }).ToArray()
            }).ToArray();

            return viewModel;
        }
    }
}