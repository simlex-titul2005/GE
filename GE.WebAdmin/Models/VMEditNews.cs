using GE.WebCoreExtantions;
using SX.WebCore.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GE.WebAdmin.Models
{
    [MetadataType(typeof(VMEditNewsMetadata))]
    public sealed class VMEditNews : SxMaterial, ISxViewModel<int>
    {
        [UIHint("EditGame"), Display(Name = "Игра")]
        public int? GameId { get; set; }
    }

    sealed class VMEditNewsMetadata
    {
        [Display(Name="Название новости"), MaxLength(400), Required]
        public string Title { get; set; }

        [Display(Name = "Контент"), Required, DataType(DataType.MultilineText), AllowHtml]
        public string Html { get; set; }

        [Display(Name = "Показывать"), Required]
        public bool Show { get; set; }

        [Display(Name = "Изображение"), UIHint("EditImage")]
        public Guid? FrontPictureId { get; set; }
    }
}