using SX.WebCore.DbModels.Abstract;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GE.WebUI.Models
{
    [Table("D_STEAM_APP")]
    public class SteamApp : SxDbUpdatedModel<Guid>
    {
        public int AppId { get; set; }

        [Required, MaxLength(400), Index]
        public string Name { get; set; }
    }
}