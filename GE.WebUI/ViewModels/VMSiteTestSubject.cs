using SX.WebCore.ViewModels;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace GE.WebUI.ViewModels
{
    public sealed class VMSiteTestSubject
    {
        public int Id { get; set; }

        [MaxLength(400, ErrorMessageResourceType = typeof(SX.WebCore.Resources.Messages), ErrorMessageResourceName = "MaxLengthField")]
        [Required(ErrorMessageResourceType = typeof(SX.WebCore.Resources.Messages), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "Заголовок"), DataType(DataType.MultilineText)]
        public string Title { get; set; }

        [Display(Name = "Описание"), AllowHtml, DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public VMSiteTest Test { get; set; }
        public int TestId { get; set; }

        public SxVMPicture Picture { get; set; }
        [Display(Name = "Картинка"), UIHint("PicturesLookupGrid")]
        public Guid? PictureId { get; set; }
    }
}
