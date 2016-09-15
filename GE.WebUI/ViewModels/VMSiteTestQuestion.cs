using System;
using System.ComponentModel.DataAnnotations;

namespace GE.WebUI.ViewModels
{
    public sealed class VMSiteTestQuestion
    {
        public int Id { get; set; }

        public VMSiteTest Test { get; set; }

        [Required(ErrorMessageResourceType = typeof(SX.WebCore.Resources.Messages), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "Тест"), UIHint("SiteTestsLookupGrid")]
        public int TestId { get; set; }

        [Required(ErrorMessageResourceType = typeof(SX.WebCore.Resources.Messages), ErrorMessageResourceName = "RequiredField")]
        [MaxLength(500, ErrorMessageResourceType = typeof(SX.WebCore.Resources.Messages), ErrorMessageResourceName = "MaxLengthField")]
        [Display(Name = "Вопрос"), DataType(DataType.MultilineText)]
        public string Text { get; set; }

        public DateTime DateCreate { get; set; }
    }
}
