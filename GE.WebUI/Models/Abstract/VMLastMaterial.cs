using System;
using System.Web.Mvc;
using static SX.WebCore.Enums;

namespace GE.WebUI.Models.Abstract
{
    public abstract class VMLastMaterial
    {
        public DateTime DateOfPublication { get; set; }
        public ModelCoreType ModelCoreType { get; set; }
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
                    url = urlHelper.Action(MVC.Articles.Details(DateCreate.Year, DateCreate.Month.ToString("00"), DateCreate.Day.ToString("00"), TitleUrl));
                    break;
                case ModelCoreType.News:
                    url = urlHelper.Action(MVC.News.Details(DateCreate.Year, DateCreate.Month.ToString("00"), DateCreate.Day.ToString("00"), TitleUrl));
                    break;
            }
            return url;
        }
    }
}