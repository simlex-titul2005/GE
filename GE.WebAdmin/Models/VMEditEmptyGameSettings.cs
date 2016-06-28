using System.ComponentModel.DataAnnotations;

namespace GE.WebAdmin.Models
{
    public class VMEditEmptyGameSettings
    {
        [Required, MaxLength(255), Display(Name="Иконка"), UIHint("PicturesLookupGrid")]
        public string IconPath { get; set; }

        [MaxLength(255)]
        public string OldIconPath { get; set; }

        [Required, MaxLength(255), Display(Name = "Положительное изображение"), UIHint("PicturesLookupGrid")]
        public string GoodImagePath { get; set; }
        
        [MaxLength(255)]
        public string OldGoodImagePath { get; set; }

        [Required, MaxLength(255), Display(Name = "Отрицательное изображение"), UIHint("PicturesLookupGrid")]
        public string BadImagePath { get; set; }

        [MaxLength(255)]
        public string OldBadImagePath { get; set; }
    }
}