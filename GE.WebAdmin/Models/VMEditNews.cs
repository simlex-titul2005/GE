using GE.WebCoreExtantions;
using SX.WebCore.Abstract;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace GE.WebAdmin.Models
{
    [MetadataType(typeof(VMEditNewsMetadata))]
    public sealed class VMEditNews : SxMaterial
    {
        public Game Game { get; set; }
        [UIHint("EditGame"), Display(Name = "Игра")]
        public int? GameId { get; set; }

        [MaxLength(255)]
        public string OldTitleUrl { get; set; }
    }

    sealed class VMEditNewsMetadata
    {
        [Display(Name="Название новости"), MaxLength(255), Required]
        public string Title { get; set; }

        [Display(Name = "Контент"), Required, DataType(DataType.MultilineText), AllowHtml]
        public string Html { get; set; }

        [Display(Name = "Вступление"), DataType(DataType.MultilineText)]
        public string Foreword { get; set; }

        [Display(Name = "Показывать"), Required]
        public bool Show { get; set; }

        [Display(Name = "Изображение"), UIHint("EditImage")]
        public Guid? FrontPictureId { get; set; }

        [Display(Name = "Строковый ключ"), MaxLength(255), Required]
        public string TitleUrl { get; set; }

        [Display(Name = "Дата публикации")]
        public DateTime DateOfPublication { get; set; }
    }
}