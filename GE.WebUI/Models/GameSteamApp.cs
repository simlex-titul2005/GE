using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GE.WebUI.Models
{
    [Table("D_GAME_STEAM_APP")]
    public class GameSteamApp
    {
        [Key, Column(Order = 1)]
        public int GameId { get; set; }
        public virtual Game Game { get; set; }

        [Key, Column(Order = 2)]
        public int SteamAppId { get; set; }
        public virtual SteamApp SteamApp { get; set; }
    }
}