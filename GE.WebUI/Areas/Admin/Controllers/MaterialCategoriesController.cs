using GE.WebUI.ViewModels;
using SX.WebCore.MvcControllers;
using GE.WebUI.Infrastructure.Repositories;
using GE.WebUI.Models;
using System;
using System.Web.Mvc;
using System.Threading.Tasks;

namespace GE.WebUI.Areas.Admin.Controllers
{
    public sealed class MaterialCategoriesController : SxMaterialCategoriesController<MaterialCategory, VMMaterialCategory>
    {
        public MaterialCategoriesController()
        {
            if (Repo == null || (Repo as RepoMaterialCategory)==null)
                Repo = new RepoMaterialCategory();
        }

        protected override Action<MaterialCategory> AfterSelectRedactModel
        {
            get
            {
                return (data)=> {
                    ViewBag.GameTitle = data.Game?.Title;
                };
            }
        }

        public override ActionResult Index(byte? mct, int page = 1)
        {
            switch(mct)
            {
                case 1:
                    ViewBag.PageTitle = "Категории статей";
                    break;
                case 2:
                    ViewBag.PageTitle = "Категории новостей";
                    break;
                case 6:
                    ViewBag.PageTitle = "Категории афоризмов";
                    break;
                case 7:
                    ViewBag.PageTitle = "Категории юмора";
                    break;
            }

            return base.Index(mct, page);
        }

        public override async Task<ActionResult> Edit(VMMaterialCategory model)
        {
            var isFeatured = Convert.ToBoolean(Request.Form.Get(nameof(model.IsFeatured)));
            var desc = Request.Form.Get(nameof(model.Description));
            model.IsFeatured = isFeatured;
            model.Description = desc;

            if (model.IsFeatured)
            {
                if(string.IsNullOrEmpty(model.Description))
                    ModelState.AddModelError(nameof(model.Description), "Для избранных категорий поле обязательно для заполнения");
                if(!model.FrontPictureId.HasValue)
                    ModelState.AddModelError(nameof(model.FrontPictureId), "Для избранных категорий поле обязательно для заполнения");

            }

            if (!ModelState.IsValid) return View(model);
            return await base.Edit(model);
        }
    }
}