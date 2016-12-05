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

        public VMLastMaterialsBlock GetLastMaterialBlock(int lmc = 5, int gc = 4, int lgmc = 3, int gtc = 20)
        {
            var gamesSql = @"SELECT DISTINCT TOP(@amount) dg.*, dp.Id FROM DV_MATERIAL AS dm
LEFT JOIN D_ARTICLE AS da ON da.ModelCoreType = dm.ModelCoreType AND da.Id = dm.Id
LEFT JOIN D_NEWS AS dn ON dn.ModelCoreType = dm.ModelCoreType AND dn.Id = dm.Id
JOIN D_GAME AS dg ON dg.Id = da.GameId OR dg.Id=dn.GameId
LEFT JOIN D_PICTURE AS dp ON dp.Id=dg.FrontPictureId
WHERE dg.Show=1 AND dm.Show=1 AND dm.DateOfPublication<=GETDATE()
ORDER BY dg.Title";

            var materialsSql = @"SELECT TOP(@amount) dm.Id, dm.ModelCoreType, dm.Title, dm.DateCreate, dm.FrontPictureId
  FROM DV_MATERIAL AS dm
LEFT JOIN D_ARTICLE AS da ON da.ModelCoreType = dm.ModelCoreType AND da.Id = dm.Id
LEFT JOIN D_NEWS AS dn ON dn.ModelCoreType = dm.ModelCoreType AND dn.Id = dm.Id
LEFT JOIN D_PICTURE AS dp ON dp.Id = dm.FrontPictureId
JOIN D_GAME AS dg ON dg.Id = da.GameId OR dg.Id=dn.GameId AND dg.Show=1
WHERE dm.Show=1 AND dm.DateOfPublication<=GETDATE() AND dn.GameId IN (SELECT fsi.[Value]
                      FROM dbo.func_split_int(@gameIds) AS fsi)
ORDER BY dm.DateOfPublication DESC";

            var model = new VMLastMaterialsBlock();
            using (var connection = new SqlConnection(ConnectionString))
            {
                model.Materials = Read(new SxFilter(1, lmc) {  OnlyShow=true });
                model.Games = connection.Query<VMGame, SxVMPicture, VMGame>(gamesSql, (g,p)=> {
                    g.FrontPicture = p;
                    return g;
                }, new { amount = gc }, splitOn:"Id").ToArray();

                var gameIds = model.Games.Select(x => x.Id.ToString()).Aggregate((s, b) => s + "," + b);
                model.Materials = connection.Query<VMMaterial>(materialsSql, new { amount = lgmc, gameIds = gameIds }).ToArray();
            }
            return model;
        }
    }
}