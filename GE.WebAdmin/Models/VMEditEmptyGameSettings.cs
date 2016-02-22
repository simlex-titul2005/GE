using SX.WebCore.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GE.WebAdmin.Models
{
    public class VMEditEmptyGameSettings
    {
        [Required, MaxLength(255), Display(Name="Путь к иконке")]
        public string IconPath { get; set; }

        [MaxLength(255)]
        public string OldIconPath { get; set; }

        [Required, MaxLength(255), Display(Name = "Путь к положительному изображению")]
        public string GoodImagePath { get; set; }
        
        [MaxLength(255)]
        public string OldGoodImagePath { get; set; }

        [Required, MaxLength(255), Display(Name = "Путь к отрицательному изображению")]
        public string BadImagePath { get; set; }

        [MaxLength(255)]
        public string OldBadImagePath { get; set; }
    }
}