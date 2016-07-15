using SX.WebCore.ViewModels;
using System;
using System.Web.Mvc;
using static SX.WebCore.Enums;

namespace GE.WebUI.Models.Abstract
{
    public abstract class VMLastMaterial : SxVMMaterial
    {
        public DateTime DateOfPublication { get; set; }
        public DateTime DateCreate { get; set; }
        public string Title { get; set; }
        public string TitleUrl { get; set; }
        public Guid? FrontPictureId { get; set; }
        public string Foreword { get; set; }
        public VMUser Author { get; set; }
        public string Url(UrlHelper urlHelper)
        {
            string url = "#";
            switch (ModelCoreType)
            {
                case ModelCoreType.Article:
                    url = urlHelper.Action("details", new { controller="articles", year = DateCreate.Year, month = DateCreate.Month.ToString("00"), day = DateCreate.Day.ToString("00"), titleUrl = TitleUrl });
                    break;
                case ModelCoreType.News:
                    url = urlHelper.Action("details", new { controller = "news", year = DateCreate.Year, month = DateCreate.Month.ToString("00"), day = DateCreate.Day.ToString("00"), titleUrl = TitleUrl });
                    break;
            }
            return url;
        }
        public int CommentsCount { get; set; }
        public int LikesCount { get; set; }
        public int ViewsCount { get; set; }
    }
}