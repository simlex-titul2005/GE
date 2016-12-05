using SX.WebCore.DbModels.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GE.WebUI.Models
{
    [Table("D_POPULAR_YOUTUBE_VIDEO")]
    public sealed class PopularYoutubeVideo : SxDbUpdatedModel<string>
    {
        [Required, MaxLength(400)]
        public string Title { get; set; }

        [Required, MaxLength(100)]
        public string ChannelId { get; set; }

        [MaxLength(400)]
        public string ChannelTitle { get; set; }

        public int Rating { get; set; }
    }
}