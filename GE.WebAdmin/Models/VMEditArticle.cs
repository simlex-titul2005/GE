﻿using GE.WebCoreExtantions;
using SX.WebCore.Abstract;
using SX.WebCore.Attrubutes;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using static SX.WebCore.Enums;


namespace GE.WebAdmin.Models
{
    [MetadataType(typeof(VMEditArticleMetadata))]
    public sealed class VMEditArticle : SxMaterial
    {
        public Game Game { get; set; }
        [UIHint("EditGame"), Display(Name = "Игра")]
        public int? GameId { get; set; }

        [Display(Name = "Тип статьи")]
        public string ArticleTypeName { get; set; }

        [MaxLength(255)]
        public string OldTitleUrl { get; set; }
    }

    sealed class VMEditArticleMetadata
    {
        [Display(Name = "Название статьи"), MaxLength(255), Required, MaxWordsCount(15), MinWordsCount(7)]
        public string Title { get; set; }

        [Display(Name = "Контент"), Required, DataType(DataType.MultilineText), AllowHtml]
        public string Html { get; set; }

        [Display(Name = "Вступление"), DataType(DataType.MultilineText)]
        public string Foreword { get; set; }

        [Display(Name = "Показывать"), Required]
        public bool Show { get; set; }

        [Display(Name = "Изображение"), UIHint("EditImage")]
        public Guid? FrontPictureId { get; set; }

        [Display(Name = "Показывать картинку на странице")]
        public bool ShowFrontPictureOnDetailPage { get; set; }

        [Display(Name = "Строковый ключ"), MaxLength(255), Required]
        public string TitleUrl { get; set; }

        [Display(Name = "Дата публикации"), UIHint("EditDate")]
        public DateTime DateOfPublication { get; set; }

        [Display(Name = "Категория"), UIHint("EditMaterialCategory"), AdditionalMetadata("mct", ModelCoreType.Article)]
        public string CategoryId { get; set; }

        [Display(Name = "Показывать в избранных")]
        public bool IsTop { get; set; }
    }
}