using GE.WebUI.Infrastructure.ModelBinders;
using SX.WebCore.ViewModels;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace GE.WebUI.ViewModels
{
    [ModelBinder(typeof(VMMaterialCategoryModelBinder))]
    public sealed class VMMaterialCategory : SxVMMaterialCategory
    {
        [Display(Name="Игра"), UIHint("_GameLookupGrid")]
        public int? GameId { get; set; }
        public VMGame Game { get; set; }

        [Display(Name = "Показывать в избранных")]
        public bool IsFeatured { get; set; }
    }
}