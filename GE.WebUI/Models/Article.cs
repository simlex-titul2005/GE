using SX.WebCore.DbModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GE.WebUI.Models
{
    [Table("D_ARTICLE")]
    public class Article : SxArticle
    {
        public virtual Game Game { get; set; }
        public int? GameId { get; set; }

        [MaxLength(100)]
        public string GameVersion { get; set; }
    }
}