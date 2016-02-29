﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GE.WebAdmin.Models
{
    public sealed class VMEditSeoInfo
    {
        public int Id { get; set; }

        [MaxLength(255), Required, Display(Name="Адрес")]
        public string RawUrl { get; set; }

        [MaxLength(255), Required, Display(Name = "Заголовок страницы")]
        public string SeoTitle { get; set; }

        [MaxLength(1000), Display(Name = "Описание страницы"), DataType(DataType.MultilineText)]
        public string SeoDescription { get; set; }
    }
}