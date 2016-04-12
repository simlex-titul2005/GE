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

        public VMManualGroup Group { get; set; }
        [Display(Name = "Группа"), MaxLength(128), Required, UIHint("EditManualGroup")]
        public string GroupId { get; set; }

        [Display(Name = "Название материала"), MaxLength(255), Required]
        public string Title { get; set; }

        [Display(Name = "Контент"), Required, DataType(DataType.MultilineText), AllowHtml]
        public string Html { get; set; }

        [Display(Name = "Вступление"), DataType(DataType.MultilineText)]
        public string Foreword { get; set; }
    }
}