using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GE.WebAdmin.Models
{
    public sealed class VMSiteSettings
    {
        [Display(Name = "Наименование сайта"), Required, MaxLength(50)]
        public string SiteName { get; set; }
        [MaxLength(50)]
        public string OldSiteName { get; set; }

        [Display(Name="Путь к изображению"), Required, MaxLength(255)]
        public string LogoPath { get; set; }
        [MaxLength(255)]
        public string OldLogoPath { get; set; }

        [Display(Name = "Путь к файлу фона сайта"), MaxLength(255)]
        public string SiteBgPath { get; set; }
        [MaxLength(255)]
        public string OldSiteBgPath { get; set; }
    }
}