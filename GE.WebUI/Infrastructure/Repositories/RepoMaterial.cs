using Dapper;
using GE.WebUI.ViewModels.Abstracts;
using SX.WebCore;
using SX.WebCore.Abstract;
using SX.WebCore.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using static SX.WebCore.Enums;
using System.Data.SqlClient;
using GE.WebUI.ViewModels;
using System.Linq;

namespace GE.WebUI.Infrastructure.Repositories
{
    public abstract class RepoMaterial<TModel, TViewModel> : SxRepoMaterial<TModel, TViewModel>
        where TModel : SxMaterial
        where TViewModel : VMMaterial
    {
        private Dictionary<string, object> _filterSettings;
        public RepoMaterial(ModelCoreType mct, Dictionary<string, object> filterSettings) : base(mct)
        {
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

        protected override Action<SxFilter, StringBuilder, DynamicParameters> ChangeWhereBody
        {
            get
            {
                return (filter, sb, param) =>
                {
                    var currentContext = HttpContext.Current;
                    if (currentContext != null)
                    {
                        var gameTitleUtl = currentContext.Request.RequestContext.RouteData.Values["gameTitle"];
                        if (gameTitleUtl != null)
                        {
                            sb.Append(" AND (dg.TitleUrl=@gameTitleUrl OR @gameTitleUrl IS NULL) ");
                            param.Add("gameTitleUrl", gameTitleUtl.ToString());
                        }
                    }
                };
            }
        }

        protected override Action<SqlConnection, TViewModel[]> ChangeMaterialsAfterSelect
        {
            get
            {
                return (connection, data) =>
                {
                    var sb = new StringBuilder();
                    for (int i = 0; i < data.Length; i++)
                    {
                        sb.AppendFormat(",{0}", data[i].Id);
                    }
                    sb.Remove(0, 1);

                    var materialGames = connection.Query<VMMaterial, VMGame, VMMaterial>("dbo.get_material_games @mct, @ids", (m, g) =>
                    {
                        m.Game = g;
                        return m;
                    }, new { mct = ModelCoreType, ids = sb.ToString() }).ToArray();

                    if (materialGames.Any())
                    {
                        VMMaterial materialGame = null;
                        TViewModel item = null;
                        for (int i = 0; i < materialGames.Length; i++)
                        {
                            materialGame = materialGames[i];
                            item = data.SingleOrDefault(x => x.Id == materialGame.Id);
                            if (item != null && materialGame.Game != null)
                            {
                                item.GameId = materialGame.Game.Id;
                                item.Game = materialGame.Game;
                            }
                        }
                    }
                };
            }
        }
    }
}