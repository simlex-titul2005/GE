using SX.WebCore.DbModels;
using SX.WebCore.DbModels.Abstract;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GE.WebUI.Models
{
    [Table("D_INFOGRAPHIC")]
    public class Infographic
    {
        [Key, Column(Order = 1)]
        public Guid PictureId { get; set; }
        public virtual SxPicture Picture { get; set; }

        [Key, Column(Order = 2)]
        public int MaterialId { get; set; }
        [Key, Column(Order = 3)]
        public byte ModelCoreType { get; set; }
        public virtual SxMaterial Material { get; set; }
    }
}