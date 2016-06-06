using GE.WebCoreExtantions;
using SX.WebCore;
using SX.WebCore.Abstract;
using SX.WebCore.Repositories;
using System.Web.Mvc;
using System.Linq;
using GE.WebUI.Models;

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

        [ChildActionOnly]
        public virtual PartialViewResult RandomList()
        {
            var data = (_repo as RepoSiteTest<DbContext>).RandomList();
            return PartialView("_RandomList", data);
        }

        [HttpGet]
        public virtual ViewResult Details(int id)
        {
            var data = (_repo as RepoSiteTest<DbContext>).GetSiteTestPage(id);

            var test = data.GroupBy(x => x.Block.Test).Select(x=> new {
                Id =x.Key.Id,
                Title=x.Key.Title,
                Description=x.Key.Description
            }).Distinct().SingleOrDefault();

            var viewModel = new VMSiteTest() {
                Id= test.Id,
                Title=test.Title,
                Description=test.Description
            };

            var blocks = data.GroupBy(x => x.Block).Select(x=> new {
                Id=x.Key.Id,
                Title=x.Key.Title,
                Description=x.Key.Description
            }).Distinct();

            viewModel.Blocks = blocks.Select(x => new VMSiteTestBlock {
                Id=x.Id,
                Title=x.Title,
                Description=x.Description
            }).ToArray();

            if(viewModel.Blocks.Any())
            {
                for (int i = 0; i < viewModel.Blocks.Length; i++)
                {
                    var block = viewModel.Blocks[i];
                    block.Questions = data.Where(x => x.BlockId == block.Id).Select(x=>Mapper.Map<SxSiteTestQuestion, VMSiteTestQuestion>(x)).ToArray();
                }
            }

            return View(viewModel);
        }

        [HttpPost]
        public virtual ActionResult Result(int testId, int[] blockId, int[] questionId)
        {
            var test = new VMSiteTest { Id = testId };
            test.Blocks = new VMSiteTestBlock[blockId.Length];
            for (int i = 0; i < blockId.Length; i++)
            {
                var block = new VMSiteTestBlock
                {
                    Id = blockId[i],
                    Questions = new VMSiteTestQuestion[1] {
                        new VMSiteTestQuestion { Id=questionId[i]}
                    }
                };
                test.Blocks[i] = block;
            }

            return null;
        }
    }
}