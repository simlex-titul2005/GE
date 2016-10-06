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
            var mct = (ModelCoreType)model.ModelCoreType;
            switch (mct)
            {
                case ModelCoreType.Article:
                    return new VMArticle { DateCreate = model.DateCreate, TitleUrl = model.TitleUrl, ModelCoreType = mct }.GetUrl(Url);
                case ModelCoreType.News:
                    return new VMNews { DateCreate = model.DateCreate, TitleUrl = model.TitleUrl, ModelCoreType = mct }.GetUrl(Url);
                case ModelCoreType.Aphorism:
                    return new VMAphorism { TitleUrl=model.TitleUrl, CategoryId=model.CategoryId, ModelCoreType=mct }.GetUrl(Url);
                case ModelCoreType.Humor:
                    return new VMHumor { DateCreate = model.DateCreate, TitleUrl = model.TitleUrl, ModelCoreType = mct }.GetUrl(Url);
                default:
                    return null;
            }
        }
    }
}