using GE.WebUI.ViewModels;
using GE.WebUI.ViewModels.Abstracts;
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
                return (model) =>
                {
                    var mct= (byte)model.ModelCoreType;

                    VMMaterial item = null;
                    switch(mct)
                    {
                        case 1:
                            item= new VMArticle { DateCreate = model.DateCreate, TitleUrl = model.TitleUrl, ModelCoreType = mct };
                            break;
                        case 2:
                            item = new VMNews { DateCreate = model.DateCreate, TitleUrl = model.TitleUrl, ModelCoreType = mct };
                            break;
                        case 6:
                            item = new VMAphorism { TitleUrl = model.TitleUrl, CategoryId = model.CategoryId, ModelCoreType = mct };
                            break;
                        case 7:
                            item = new VMHumor { DateCreate = model.DateCreate, TitleUrl = model.TitleUrl, ModelCoreType = mct };
                            break;
                    }

                    return item?.GetUrl(Url) ?? null;
                };
            }
        }

        //private string getSeoItemUrl(dynamic model)
        //{
        //    var mct = (byte)model.ModelCoreType;
        //    switch (mct)
        //    {
        //        case (byte)ModelCoreType.Article:
        //            return new VMArticle { DateCreate = model.DateCreate, TitleUrl = model.TitleUrl, ModelCoreType = mct }.GetUrl(Url);
        //        case (byte)ModelCoreType.News:
        //            return new VMNews { DateCreate = model.DateCreate, TitleUrl = model.TitleUrl, ModelCoreType = mct }.GetUrl(Url);
        //        case 6://aphorism
        //            return new VMAphorism { TitleUrl=model.TitleUrl, CategoryId=model.CategoryId, ModelCoreType=mct }.GetUrl(Url);
        //        case 7://humor
        //            return new VMHumor { DateCreate = model.DateCreate, TitleUrl = model.TitleUrl, ModelCoreType = mct }.GetUrl(Url);
        //        default:
        //            return null;
        //    }
        //}
    }
}