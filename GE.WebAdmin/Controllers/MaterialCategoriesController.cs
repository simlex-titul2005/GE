using GE.WebAdmin.Models;
using GE.WebCoreExtantions;
using SX.WebCore;
using SX.WebCore.Abstract;
using System;
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
                case ModelCoreType.Aphorism:
                    return "Категории афоризмов";
                default:
                    return "Категоря материалов не определена";
            }
        }

        public virtual ActionResult Edit(ModelCoreType mct, string pcid = null, string id = null)
        {
            var data = string.IsNullOrEmpty(id) ? new SxMaterialCategory { ModelCoreType = mct, ParentCategoryId = pcid } : _repo.GetByKey(id);
            var viewModel = Mapper.Map<SxMaterialCategory, VMEditMaterialCategory>(data);
            ViewBag.ModelCoreType = mct;
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Edit(VMEditMaterialCategory model)
        {
            ViewBag.ModelCoreType = model.ModelCoreType;

            var oldId = Request.Form["OldId"];
            var isNew = model.Id == null && oldId == null;
            var id = Url.SeoFriendlyUrl(model.Title);

            if (isNew || oldId != model.Id)
            {
                model.Id = id;
                ModelState["Id"].Errors.Clear();

                if (isNew)
                {
                    var exist = _repo.GetByKey(id);
                    if (exist != null)
                        ModelState.AddModelError("Id", "Категория с таким ключем уже существует");
                }
            }

            var redactModel = Mapper.Map<VMEditMaterialCategory, SxMaterialCategory>(model);

            if (ModelState.IsValid)
            {
                SxMaterialCategory newModel = null;
                if (isNew)
                    newModel = _repo.Create(redactModel);
                else
                {
                    if (User.IsInRole("architect"))
                        newModel = _repo.Update(redactModel, new object[] { oldId }, true, "Id", "Title", "FrontPictureId", "ModelCoreType");
                    else
                        newModel = _repo.Update(redactModel, true, "Title", "FrontPictureId", "ModelCoreType");
                }
                return RedirectToAction(MVC.MaterialCategories.Index(mct: model.ModelCoreType));
            }
            else
            {
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

        [HttpGet]
        public virtual PartialViewResult TreeViewMenu(ModelCoreType mct, string cur = null)
        {
            ViewBag.CurrentCategory = cur;

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
            ViewBag.TreeViewMenuFuncContent = TreeViewMenuFuncContent(mct);

            return PartialView(MVC.MaterialCategories.Views._TreeViewMenu, parents);
        }

        private Func<VMMaterialCategory, string> TreeViewMenuFuncContent(ModelCoreType mct)
        {
            switch (mct)
            {
                case ModelCoreType.Aphorism:
                    return (x) => string.Format("<a href=\"{0}\">{1}</a>", Url.Action(MVC.Aphorisms.Index(curCat: x.Id)), x.Title);
                case ModelCoreType.Manual:
                    return (x) => string.Format("<a href=\"{0}\">{1}</a>", Url.Action(MVC.FAQ.Index(curCat: x.Id)), x.Title);
                default:
                    return null;
            }
        }
    }
}