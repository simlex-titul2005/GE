using GE.WebUI.ViewModels.Abstracts;
using SX.WebCore.MvcControllers;
using System.Linq;
using System;
using SX.WebCore.DbModels.Abstract;

namespace GE.WebUI.Areas.Admin.Controllers
{
    public abstract class MaterialsController<TModel, TViewModel> : SxMaterialsController<TModel, TViewModel>
        where TModel : SxMaterial
        where TViewModel : VMMaterial, new()
    {
        public MaterialsController(byte mct) : base(mct) { }

        protected override string[] PropsForUpdate
        {
            get
            {
                return base.PropsForUpdate.Union(new string[] {
                    "GameId",
                    "GameVersion"
                }).ToArray();
            }
        }

        protected override Action<TViewModel> BeforeEditModel
        {
            get
            {
                return (model) =>
                {
                    ViewBag.GameTitle = model.Game?.Title;
                };
            }
        }
    }
}