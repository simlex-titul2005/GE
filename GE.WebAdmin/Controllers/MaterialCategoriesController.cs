using GE.WebAdmin.Models;
using GE.WebCoreExtantions;
using SX.WebCore;
using SX.WebCore.Abstract;
using System.Linq;
using System.Web.Mvc;
using static SX.WebCore.Enums;

namespace GE.WebAdmin.Controllers
{
    public partial class MaterialCategoriesController : BaseController
    {
        private SxDbRepository<string, SxMaterialCategory, DbContext> _repo;
        public MaterialCategoriesController()
        {
            _repo = new SX.WebCore.Repositories.RepoMaterialCategory<DbContext>();
        }

        [HttpGet]
        public virtual ActionResult Index(ModelCoreType mct)
        {
            var filter = new WebCoreExtantions.Filter { ModelCoreType = mct };
            var data = _repo.Query(filter).Select(x => Mapper.Map<SxMaterialCategory, VMMaterialCategory>(x)).ToArray();
            var parents = data.Where(x => x.ParentCategoryId == null).ToArray();
            for (int i = 0; i < parents.Length; i++)
            {
                var parent = parents[i];
                parent.Level = 1;
                updateTreeNodeLevel(parent.Id, data, 1);
                fillMaterialCategory(parent, null, data);
            }

            ViewBag.MaxTreeViewLevel = data.Any() ? data.Max(x => x.Level) : 1;
            ViewBag.ModelCoreType = mct;
            ViewBag.PageTitle = getPageTitle(mct);

            return View(parents);
        }

        private static void fillMaterialCategory(VMMaterialCategory pg, VMMaterialCategory parent, VMMaterialCategory[] all)
        {
            var children = all.Where(x => x.ParentCategoryId == pg.Id).ToArray();
            if (!children.Any()) return;

            for (int i = 0; i < children.Length; i++)
            {
                var child = children[i];
                child.Level = pg.Level + 1;
                updateTreeNodeLevel(child.Id, all, child.Level);
                fillMaterialCategory(child, pg, all);
            }

            pg.ChildCategories = children.OrderBy(x => x.Title).ToArray();
        }
        private static void updateTreeNodeLevel(string id, VMMaterialCategory[] all, int level)
        {
            all.Single(x => x.Id == id).Level = level;
        }
        private static string getPageTitle(ModelCoreType mct)
        {
            switch (mct)
            {
                case ModelCoreType.Article:
                    return "Категории статей";
                case ModelCoreType.News:
                    return "Категории новостей";
                case ModelCoreType.Manual:
                    return "Справочные категории";
                default:
                    return "Категоря материалов не определена";
            }
        }

        public virtual ActionResult Edit(ModelCoreType mct, string pcid = null, string id = null)
        {
            var data = string.IsNullOrEmpty(id) ? new SxMaterialCategory { ModelCoreType= mct, ParentCategoryId= pcid } : _repo.GetByKey(id);
            var viewModel = Mapper.Map<SxMaterialCategory, VMEditMaterialCategory>(data);
            ViewBag.ModelCoreType = mct;
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Edit(VMEditMaterialCategory model)
        {
            ViewBag.ModelCoreType = model.ModelCoreType;
            var redactModel = Mapper.Map<VMEditMaterialCategory, SxMaterialCategory>(model);

            if (ModelState.IsValid)
            {
                //var existModel = _repo.GetByKey(model.Id);
                //if (existModel != null)
                //{
                //    ModelState.AddModelError("Id", "Элемент с таким ключем уже существует");
                //    model.Id = null;
                //    return View(model);
                //}

                SxMaterialCategory newModel = null;
                newModel = _repo.Update(redactModel, true, "Title", "FrontPictureId");

                return RedirectToAction(MVC.MaterialCategories.Index(mct: model.ModelCoreType));
            }
            else
            {
                model.Id = null;
                return View(model);
            }
        }

        public virtual RedirectToRouteResult Delete(VMEditMaterialCategory model)
        {
            var mct = model.ModelCoreType;
            _repo.Delete(model.Id);
            return RedirectToAction(MVC.MaterialCategories.Index(mct: mct));
        }

        [HttpPost]
        public virtual PartialViewResult FindTable(ModelCoreType mct)
        {
            var filter = new WebCoreExtantions.Filter { ModelCoreType = mct };
            var data = _repo.Query(filter).Select(x => Mapper.Map<SxMaterialCategory, VMMaterialCategory>(x)).ToArray();
            var parents = data.Where(x => x.ParentCategoryId == null).ToArray();
            for (int i = 0; i < parents.Length; i++)
            {
                var parent = parents[i];
                parent.Level = 1;
                updateTreeNodeLevel(parent.Id, data, 1);
                fillMaterialCategory(parent, null, data);
            }

            ViewBag.MaxTreeViewLevel = data.Any() ? data.Max(x => x.Level) : 1;
            ViewBag.ModelCoreType = mct;
            ViewBag.PageTitle = getPageTitle(mct);

            return PartialView(MVC.MaterialCategories.Views._TreeView, parents);
        }
    }
}