using System.ComponentModel.DataAnnotations;

namespace GE.WebAdmin.Models
{
    public sealed class VMEditSiteTest
    {
        public int Id { get; set; }

        [Required, MaxLength(200), Display(Name ="Заголовок")]
        public string Title { get; set; }

        [Required, MaxLength(1000), Display(Name = "Описание"), DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required, Display(Name ="Тип теста")]
        public SX.WebCore.SxSiteTest.SiteTestType TestType { get; set; }
    }
}