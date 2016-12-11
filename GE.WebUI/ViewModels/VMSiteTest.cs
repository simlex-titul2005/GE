using GE.WebUI.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace GE.WebUI.ViewModels
{
    public sealed class VMSiteTest
    {
        public VMSiteTest()
        {
            Questions = new VMSiteTestQuestion[0];
        }
        public int Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(SX.WebCore.Resources.Messages), ErrorMessageResourceName = "RequiredField")]
        [MaxLength(200, ErrorMessageResourceType = typeof(SX.WebCore.Resources.Messages), ErrorMessageResourceName = "MaxLengthField")]
        [Display(Name = "Заголовок")]
        public string Title { get; set; }

        [MaxLength(255, ErrorMessageResourceType = typeof(SX.WebCore.Resources.Messages), ErrorMessageResourceName = "MaxLengthField")]
        [RegularExpression(@"^[A-Za-z0-9]([-]*[A-Za-z0-9])*$", ErrorMessageResourceType = typeof(SX.WebCore.Resources.Messages), ErrorMessageResourceName = "IdentityStringField")]
        [Display(Name = "Строковый ключ")]
        public string TitleUrl { get; set; }

        public DateTime DateCreate { get; set; }

        [Required(ErrorMessageResourceType = typeof(SX.WebCore.Resources.Messages), ErrorMessageResourceName = "RequiredField")]
        [MaxLength(1000, ErrorMessageResourceType = typeof(SX.WebCore.Resources.Messages), ErrorMessageResourceName = "MaxLengthField")]
        [Display(Name = "Описание"), DataType(DataType.MultilineText), AllowHtml]
        public string Description { get; set; }

        [Display(Name = "Показывать")]
        public bool Show { get; set; }

        [Display(Name = "Тип теста")]
        public SiteTest.SiteTestType Type { get; set; }

        public VMSiteTestQuestion[] Questions { get; set; }

        [Display(Name = "Правила"), AllowHtml, DataType(DataType.MultilineText)]
        public string Rules { get; set; }

        public int ViewsCount { get; set; }

        [Display(Name = "Показывать описания объектов")]
        public bool ShowSubjectDesc { get; set; }

        public VMSiteTestSetting Settings { get; set; }

        [Display(Name = "Показывать на главной")]
        public bool ViewOnMainPage { get; set; }
    }
}
