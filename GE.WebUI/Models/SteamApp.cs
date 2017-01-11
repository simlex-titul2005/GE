using SX.WebCore.DbModels.Abstract;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GE.WebUI.Models
{
    [Table("D_STEAM_APP")]
    public class SteamApp : SxDbUpdatedModel<int>
    {
        [Required, MaxLength(400), Index]
        public string Name { get; set; }
    }
}