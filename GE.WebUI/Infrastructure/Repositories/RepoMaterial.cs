using GE.WebUI.ViewModels.Abstracts;
using SX.WebCore;
using SX.WebCore.Abstract;
using SX.WebCore.Repositories;
using System.Collections.Generic;
using static SX.WebCore.Enums;

namespace GE.WebUI.Infrastructure.Repositories
{
    public abstract class RepoMaterial<TModel, TViewModel>: SxRepoMaterial<TModel, TViewModel>
        where TModel : SxMaterial
        where TViewModel :VMMaterial
    {
        private Dictionary<string, object> _filterSettings;
        public RepoMaterial(ModelCoreType mct, Dictionary<string, object> filterSettings) : base(mct) {
            _filterSettings = filterSettings;
        }

        public override TViewModel[] Read(SxFilter filter)
        {
            filter.OnlyShow = (bool)_filterSettings["OnlyShow"];
            filter.WithComments = (bool)_filterSettings["WithComments"];
            return base.Read(filter);
        }

        protected static string GetGameVesion(ModelCoreType mct, dynamic data)
        {
            switch (mct)
            {
                case ModelCoreType.Article:
                    return data.ArticleGameVersion;
                case ModelCoreType.News:
                    return data.NewsGameVersion;
                default:
                    return null;
            }
        }
    }
}