using GE.WebUI.Infrastructure.ModelBinders;
using SX.WebCore.ViewModels;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using GE.WebUI.ViewModels.Abstracts;

namespace GE.WebUI.ViewModels
{
    [ModelBinder(typeof(VMMaterialCategoryModelBinder))]
    public sealed class VMMaterialCategory : SxVMMaterialCategory
    {
        public VMMaterialCategory()
        {
            Materials=new VMMaterial[0];
        }

        [Display(Name="Игра"), UIHint("_GameLookupGrid")]
        public int? GameId { get; set; }
        public VMGame Game { get; set; }

        [Display(Name = "Показывать в избранных")]
        public bool IsFeatured { get; set; }

        public string Url(UrlHelper url)
        {
            var result = "#";
            switch(ModelCoreType)
            {
                default: break;
                case 1: result = url.Action("List", "Articles", new { category = Id }); break;
            }
            return result;
        }

        public VMMaterial[] Materials { get; set; }
    }
}