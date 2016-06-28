using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace GE.WebAdmin.Models
{
    public sealed class VMEditAuthorAphorism
    {
        public int Id { get; set; }

        [Required, MaxLength(100), Display(Name = "Имя")]
        public string Name { get; set; }

        [AllowHtml, DataType(DataType.MultilineText), Display(Name="Описание")]
        public string Description { get; set; }

        [Display(Name = "Изображение"), UIHint("PicturesLookupGrid")]
        public Guid? PictureId { get; set; }
    }
}