using GE.WebAdmin.Models;
using GE.WebCoreExtantions;
using SX.WebCore;
using SX.WebCore.Abstract;
using SX.WebCore.HtmlHelpers;
using System.Linq;
using System.Web.Mvc;

namespace GE.WebAdmin.Controllers
{
    public partial class ManualGroupsController : BaseController
    {
        private SxDbRepository<string, SxManualGroup, DbContext> _repo;
        public ManualGroupsController()
        {
            _repo = new SX.WebCore.Repositories.RepoManualGroup<DbContext>();
        }

        [HttpGet]
        public virtual ActionResult Index()
        {
            var data = _repo.Query(null).Select(x => Mapper.Map<SxManualGroup, VMManualGroup>(x)).ToArray();
            var parents = data.Where(x => x.ParentGroupId == null).ToArray();
            for (int i = 0; i < parents.Length; i++)
            {
                var parent = parents[i];
                parent.Level = 1;
                updateTreeNodeLevel(parent.Id, data, 1);
                fillManualGroup(parent, null, data);
            }

            ViewBag.MaxTreeViewLevel = data.Max(x => x.Level);

            return View(parents);
        }

        [HttpPost]
        public virtual PartialViewResult Index(VMManualGroup filterModel)
        {
            var filter = new WebCoreExtantions.Filter { WhereExpressionObject = filterModel };
            var data = _repo.Query(filter).Select(x => Mapper.Map<SxManualGroup, VMManualGroup>(x)).ToArray();
            var parents = data.Where(x => x.ParentGroupId == null).ToArray();
            for (int i = 0; i < parents.Length; i++)
            {
                var parent = parents[i];
                parent.Level = 1;
                updateTreeNodeLevel(parent.Id, data, 1);
                fillManualGroup(parent, null, data);
            }

            ViewBag.MaxTreeViewLevel = data.Any()? data.Max(x => x.Level):1;

            return PartialView(MVC.ManualGroups.Views._TreeViewEditable, parents);
        }

        private static void fillManualGroup(VMManualGroup pg, VMManualGroup parent, VMManualGroup[] all)
        {
            var children = all.Where(x => x.ParentGroupId == pg.Id).ToArray();
            if (!children.Any()) return;

            for (int i = 0; i < children.Length; i++)
            {
                var child = children[i];
                child.Level = pg.Level + 1;
                updateTreeNodeLevel(child.Id, all, child.Level);
                fillManualGroup(child, pg, all);
            }

            pg.ChildGroups = children.OrderBy(x => x.Title).ToArray();
        }

        private static void updateTreeNodeLevel(string id, VMManualGroup[] all, int level)
        {
            all.Single(x => x.Id == id).Level = level;
        }

        [HttpGet]
        public virtual ViewResult Edit(string id)
        {
            var data = string.IsNullOrEmpty(id)?new SxManualGroup() : _repo.GetByKey(id);
            var viewModel = Mapper.Map<SxManualGroup, VMEditManualGroup>(data);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Edit(VMEditManualGroup model)
        {
            var redactModel = Mapper.Map<VMEditManualGroup, SxManualGroup>(model);
            if (ModelState.IsValid)
            {
                var existModel = _repo.GetByKey(model.Id);
                if (existModel != null)
                {
                    ModelState.AddModelError("Id", "Элемент с таким ключем уже существует");
                    model.Id = null;
                    return View(model);
                }

                SxManualGroup newModel = null;
                newModel = _repo.Update(redactModel, "Title");

                return RedirectToAction(MVC.ManualGroups.Index());
            }
            else
            {
                model.Id = null;
                return View(model);
            }
        }

        [HttpPost]
        public virtual ViewResult FindTable(int page = 1, int pageSize = 10)
        {
            var filter = new WebCoreExtantions.Filter(page, pageSize);
            var totalItems = (_repo as SX.WebCore.Repositories.RepoManualGroup<DbContext>).Count(filter);
            var data = (_repo as SX.WebCore.Repositories.RepoManualGroup<DbContext>).GetFindTable(filter);

            var viewModel = new SxExtantions.SxPagedCollection<VMManualGroup>
            {
                Collection = data
                .Select(x => Mapper.Map<SxManualGroup, VMManualGroup>(x)).ToArray(),
                PagerInfo = new SxExtantions.SxPagerInfo(page, pageSize)
                {
                    TotalItems = totalItems,
                    PagerSize = 4
                }
            };

            return View(viewModel);
        }
    }
}