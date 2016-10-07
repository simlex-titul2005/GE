using GE.WebUI.ViewModels;
using SX.WebCore.MvcControllers;
using System;
using static SX.WebCore.Enums;

namespace GE.WebUI.Controllers
{
    public sealed class SeoController : SxSeoController
    {
        protected override Func<dynamic, string> SeoItemUrlFunc
        {
            get
            {
                return getSeoItemUrl;
            }
        }

        private string getSeoItemUrl(dynamic model)
        {
            var mct = (byte)model.ModelCoreType;
            switch (mct)
            {
                case (byte)ModelCoreType.Article:
                    return new VMArticle { DateCreate = model.DateCreate, TitleUrl = model.TitleUrl, ModelCoreType = mct }.GetUrl(Url);
                case (byte)ModelCoreType.News:
                    return new VMNews { DateCreate = model.DateCreate, TitleUrl = model.TitleUrl, ModelCoreType = mct }.GetUrl(Url);
                case 6://aphorism
                    return new VMAphorism { TitleUrl=model.TitleUrl, CategoryId=model.CategoryId, ModelCoreType=mct }.GetUrl(Url);
                case 7://humor
                    return new VMHumor { DateCreate = model.DateCreate, TitleUrl = model.TitleUrl, ModelCoreType = mct }.GetUrl(Url);
                default:
                    return null;
            }
        }
    }
}