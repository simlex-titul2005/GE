using SX.WebCore.DbModels;
using SX.WebCore.DbModels.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GE.WebUI.Models
{
    [Table("D_GAME")]
    public class Game : SxDbUpdatedModel<int>
    {
        [MaxLength(100), Required]
        public string Title { get; set; }

        [MaxLength(100), Required]
        public string TitleUrl { get; set; }

        [MaxLength(100),]
        public string TitleAbbr { get; set; }

        public bool Show { get; set; }

        [MaxLength(255)]
        public string Description { get; set; }

        public string FullDescription { get; set; }

        public Guid? FrontPictureId { get; set; }
        public virtual SxPicture FrontPicture { get; set; }

        public Guid? GoodPictureId { get; set; }
        public virtual SxPicture GoodPicture { get; set; }

        public Guid? BadPictureId { get; set; }
        public virtual SxPicture BadPicture { get; set; }

        public virtual ICollection<GameSteamApp> GameSteamApps { get; set; }
    }
}