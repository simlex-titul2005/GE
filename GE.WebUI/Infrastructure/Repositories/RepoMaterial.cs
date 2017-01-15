using Dapper;
using GE.WebUI.ViewModels.Abstracts;
using SX.WebCore;
using SX.WebCore.SxRepositories;
using System;
using System.Text;
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
        protected RepoMaterial(byte mct) : base(mct) { }

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

        protected override Action<SxFilter, StringBuilder, DynamicParameters> ChangeWhereBody => (filter, sb, param) =>
        {
            var gameTitleUtl = filter.AddintionalInfo!=null && filter.AddintionalInfo.Length>1 ? filter.AddintionalInfo[1]:null;
            if (gameTitleUtl == null) return;

            sb.Append(" AND (dg.TitleUrl=@gameTitleUrl OR @gameTitleUrl IS NULL) ");
            param.Add("gameTitleUrl", gameTitleUtl.ToString());
        };

        protected override Action<SqlConnection, TViewModel[]> ChangeMaterialsAfterSelect => (connection, data) =>
        {
            var ids = data.Select(x => x.Id.ToString()).ToDelimeterString(',');
            var materialGames = connection.Query<VMMaterial, VMGame, VMMaterial>("dbo.get_material_games @mct, @ids", (m, g) =>
            {
                m.Game = g;
                return m;
            }, new { mct = ModelCoreType, ids }).ToArray();

            if (!materialGames.Any()) return;

            VMMaterial materialGame;
            TViewModel item;
            for (var i = 0; i < materialGames.Length; i++)
            {
                materialGame = materialGames[i];
                item = data.SingleOrDefault(x => x.Id == materialGame.Id);
                if (item == null || materialGame.Game == null) continue;

                item.GameId = materialGame.Game.Id;
                item.Game = materialGame.Game;
            }
        };

        public override SxVMMaterial[] GetPopular(int? mct = null, int? mid = null, int amount = 10, DateTime? dateBegin = null, DateTime? dateEnd = null)
        {
            var data = base.GetPopular(mct, mid, amount, dateBegin, dateEnd);
            var viewData = new SxVMMaterial[data.Length];
            for (var i = 0; i < data.Length; i++)
            {
                viewData[i] = GetFromMaterial(data[i]);
            }

            return viewData;
        }

        private static VMMaterial GetFromMaterial(SxVMMaterial model)
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
            const string gamesSql = @"SELECT DISTINCT TOP(@amount) dg.*, dp.Id FROM DV_MATERIAL AS dm
LEFT JOIN D_ARTICLE AS da ON da.ModelCoreType = dm.ModelCoreType AND da.Id = dm.Id
LEFT JOIN D_NEWS AS dn ON dn.ModelCoreType = dm.ModelCoreType AND dn.Id = dm.Id
JOIN D_GAME AS dg ON dg.Id = da.GameId OR dg.Id=dn.GameId
LEFT JOIN D_PICTURE AS dp ON dp.Id=dg.FrontPictureId
WHERE dg.Show=1 AND dm.Show=1 AND dm.DateOfPublication<=GETDATE()
ORDER BY dg.Title";

            const string materialsSql = @"SELECT TOP(@amount) dm.Id, dm.ModelCoreType, dm.Title, dm.DateCreate, dm.FrontPictureId
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