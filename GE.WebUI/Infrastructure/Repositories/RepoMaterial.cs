using Dapper;
using GE.WebUI.ViewModels.Abstracts;
using SX.WebCore;
using SX.WebCore.SxRepositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Data.SqlClient;
using GE.WebUI.ViewModels;
using System.Linq;
using SX.WebCore.ViewModels;
using SX.WebCore.DbModels.Abstract;

namespace GE.WebUI.Infrastructure.Repositories
{
    public abstract class RepoMaterial<TModel, TViewModel> : SxRepoMaterial<TModel, TViewModel>
        where TModel : SxMaterial
        where TViewModel : VMMaterial
    {
        private Dictionary<string, object> _filterSettings;
        public RepoMaterial(byte mct, Dictionary<string, object> filterSettings) : base(mct)
        {
            _filterSettings = filterSettings;
        }

        public override TViewModel[] Read(SxFilter filter)
        {
            filter.OnlyShow = (bool)_filterSettings["OnlyShow"];
            filter.WithComments = (bool)_filterSettings["WithComments"];
            return base.Read(filter);
        }

        protected static string GetGameVesion(byte mct, dynamic data)
        {
            switch (mct)
            {
                case (byte)Enums.ModelCoreType.Article:
                    return data.ArticleGameVersion;
                case (byte)Enums.ModelCoreType.News:
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

        public override SxVMMaterial[] GetPopular(int? mct = default(int?), int? mid = default(int?), int amount = 10, DateTime? dateBegin = default(DateTime?), DateTime? dateEnd = default(DateTime?))
        {
            var data = base.GetPopular(mct, mid, amount, dateBegin, dateEnd);
            var viewData = new VMMaterial[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                viewData[i] = getFromMaterial(data[i]);
            }

            return viewData;
        }

        private static VMMaterial getFromMaterial(SxVMMaterial model)
        {
            return new VMMaterial
            {
                Id = model.Id,
                DateCreate = model.DateCreate,
                DateOfPublication = model.DateOfPublication,
                ModelCoreType = model.ModelCoreType,
                CategoryId = model.CategoryId,
                Title = model.Title,
                TitleUrl = model.TitleUrl,
                CommentsCount=model.CommentsCount,
                LikeUpCount=model.LikeUpCount,
                LikeDownCount=model.LikeDownCount,
                ViewsCount=model.ViewsCount
            };
        }
    }
}