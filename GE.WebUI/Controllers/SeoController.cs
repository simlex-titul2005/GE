using System;
using GE.WebUI.ViewModels;
using GE.WebUI.ViewModels.Abstracts;
using SX.WebCore.MvcControllers;

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
    }
}