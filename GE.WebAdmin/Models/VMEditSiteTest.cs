﻿using System.ComponentModel.DataAnnotations;

namespace GE.WebAdmin.Models
{
    public sealed class VMEditSiteTest
    {
        public int Id { get; set; }

        [Required, MaxLength(200), Display(Name ="Заголовок")]
        public string Title { get; set; }

        [Required, MaxLength(255), Display(Name = "Строковый ключ"), RegularExpression(@"^[A-Za-z0-9]([-]*[A-Za-z0-9])*$", ErrorMessage = "Поле должно содержать только буквы латинского алфавита и тире")]
        public string TitleUrl { get; set; }

        [Required, MaxLength(1000), Display(Name = "Описание"), DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required, Display(Name ="Тип теста")]
        public SX.WebCore.SxSiteTest.SiteTestType TestType { get; set; }
    }
}