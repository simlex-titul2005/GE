using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using static SX.WebCore.Enums;

namespace GE.WebAdmin.Models
{
    public sealed class VMEditManual
    {
        public int Id { get; set; }
        public ModelCoreType ModelCoreType { get; set; }
        public string UserId { get; set; }

        public VMEditMaterialCategory Category { get; set; }
        [Display(Name = "Категория"), UIHint("MaterialCategoryLookupGrid"), AdditionalMetadata("mct", ModelCoreType.Manual)]
        public string CategoryId { get; set; }

        [Display(Name = "Название материала"), MaxLength(255), Required]
        public string Title { get; set; }

        [Display(Name = "Контент"), Required, DataType(DataType.MultilineText), AllowHtml]
        public string Html { get; set; }
    }
}