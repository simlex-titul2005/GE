using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GE.WebUI.Models
{
    /// <summary>
    /// <see cref="https://wiki.teamfortress.com/wiki/WebAPI/GetNewsForApp"/>
    /// </summary>
    [Table("D_STEAM_NEWS")]
    public class SteamNews
    {
        public int SteamAppId { get; set; }
        public virtual SteamApp SteamApp { get; set; }

        public int TheNewsId { get; set; }
        public byte ModelCoreType { get; set; }
        public virtual News TheNews { get; set; }

        /// <summary>
        /// The unique identifier of the news item
        /// </summary>
        [Key, MaxLength(100)]
        public string Gid { get; set; }

        /// <summary>
        /// Title of the news item
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Permanent link to the item
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// true if the url given links to an external website. false if it links to the Steam store
        /// </summary>
        public bool IsExternalUrl { get; set; }

        /// <summary>
        /// The author of the news item
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// The article body with a length equal to the given length with an appended ellipsis if it is exceeded
        /// </summary>
        public string Contents { get; set; }

        /// <summary>
        /// The category label of the news item
        /// </summary>
        public string FeedLabel { get; set; }

        /// <summary>
        /// A unix timestamp of the date the item was posted
        /// </summary>
        public long Date { get; set; }

        /// <summary>
        /// An internal tag that describes the source of the news item
        /// </summary>
        public string FeedName { get; set; }
    }
}