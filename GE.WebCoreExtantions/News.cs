using GE.WebCoreExtantions.Abstract;
using SX.WebCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace GE.WebCoreExtantions
{
    [Table("D_NEWS")]
    public class News : SxNews, IHasGame
    {
        public virtual Game Game { get; set; }
        public int? GameId { get; set; }

        [NotMapped]
        public int CommentsCount { get; set; }

        public string Url(UrlHelper urlHelper)
        {
            string url = "#";
            switch (ModelCoreType)
            {
                case Enums.ModelCoreType.Article:
                    url = urlHelper.Action("details", new { controller = "articles", year = DateCreate.Year, month = DateCreate.Month.ToString("00"), day = DateCreate.Day.ToString("00"), titleUrl = TitleUrl });
                    break;
                case Enums.ModelCoreType.News:
                    url = urlHelper.Action("details", new { controller = "news", year = DateCreate.Year, month = DateCreate.Month.ToString("00"), day = DateCreate.Day.ToString("00"), titleUrl = TitleUrl });
                    break;
            }
            return url;
        }
    }
}
