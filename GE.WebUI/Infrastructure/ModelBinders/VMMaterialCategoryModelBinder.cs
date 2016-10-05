using GE.WebUI.ViewModels;
using System;
using System.Web.Mvc;

namespace GE.WebUI.Infrastructure.ModelBinders
{
    public class VMMaterialCategoryModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var model = (VMMaterialCategory)bindingContext.Model ?? (VMMaterialCategory)DependencyResolver.Current.GetService(typeof(VMMaterialCategory));
            var hasPrefix = bindingContext.ValueProvider.ContainsPrefix(bindingContext.ModelName);
            var searchPrefix = hasPrefix ? bindingContext.ModelName + "." : "";

            string value = string.Empty;

            value = GetValue(bindingContext, searchPrefix, "FrontPictureId");
            model.FrontPictureId = string.IsNullOrEmpty(value) ? (Guid?)null : Guid.Parse(value);

            value=GetValue(bindingContext, searchPrefix, "Id");
            model.Id = value;

            value = GetValue(bindingContext, searchPrefix, "GameId");
            model.GameId = string.IsNullOrEmpty(value) ? (int?)null : int.Parse(value);

            value = GetValue(bindingContext, searchPrefix, "ModelCoreType");
            model.ModelCoreType = (SX.WebCore.Enums.ModelCoreType)Enum.Parse(typeof(SX.WebCore.Enums.ModelCoreType), value);

            value = GetValue(bindingContext, searchPrefix, "OldId");
            model.OldId = value;

            value = GetValue(bindingContext, searchPrefix, "ParentId");
            model.ParentId = string.IsNullOrEmpty(value) ? null : value;

            value = GetValue(bindingContext, searchPrefix, "Title");
            model.Title = value;

            return model;
        }

        private string GetValue(ModelBindingContext bindingContext, string perfix, string key)
        {
            var vpr = bindingContext.ValueProvider.GetValue(perfix + key);
            return vpr?.AttemptedValue;
        }
    }
}
