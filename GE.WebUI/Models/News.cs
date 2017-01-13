using SX.WebCore.DbModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GE.WebUI.Models
{
    [Table("D_NEWS")]
    public class News : SxNews
    {
        public virtual Game Game { get; set; }
        public int? GameId { get; set; }

        [MaxLength(100)]
        public string GameVersion { get; set; }

        public string SteamNewsGid { get; set; }
        public virtual SteamNews SteamNews { get; set; }
    }
}