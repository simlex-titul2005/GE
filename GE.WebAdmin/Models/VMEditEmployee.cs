using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace GE.WebAdmin.Models
{
    public sealed class VMEditEmployee
    {
        [MaxLength(128)]
        public string Id { get; set; }

        [Display(Name="Фамилия"), MaxLength(50)]
        public string Surname { get; set; }

        [Display(Name = "Имя"), MaxLength(50)]
        public string Name { get; set; }

        [Display(Name = "Отчество"), MaxLength(50)]
        public string Patronymic { get; set; }

        [Display(Name = "Описание"), AllowHtml, DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public VMUser User { get; set; }
    }
}