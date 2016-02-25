using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GE.WebAdmin.Models
{
    public sealed class VMSiteLogoSettings
    {
        [Display(Name="Путь к изображению"), Required, MaxLength(255)]
        public string Path { get; set; }

        [MaxLength(255)]
        public string OldPath { get; set; }
    }
}