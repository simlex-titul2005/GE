using GE.WebCoreExtantions.Abstract;
using SX.WebCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace GE.WebCoreExtantions
{
    [Table("D_ARTICLE")]
    public class Article : SxArticle, IHasGame
    {
        public virtual Game Game { get; set; }
        public int? GameId { get; set; }

        [MaxLength(100)]
        public string GameVersion { get; set; }

        [NotMapped]
        public int CommentsCount { get; set; }

        public string Url(UrlHelper urlHelper)
        {
            string url = "#";
            switch (ModelCoreType)
            {
                case Enums.ModelCoreType.Article:
                    url = urlHelper.Action("Details", "Articles", new {year = DateCreate.Year, month = DateCreate.Month.ToString("00"), day = DateCreate.Day.ToString("00"), titleUrl = TitleUrl });
                    break;
                case Enums.ModelCoreType.News:
                    url = urlHelper.Action("Details", "News", new {year = DateCreate.Year, month = DateCreate.Month.ToString("00"), day = DateCreate.Day.ToString("00"), titleUrl = TitleUrl });
                    break;
            }
            return url;
        }
    }
}
