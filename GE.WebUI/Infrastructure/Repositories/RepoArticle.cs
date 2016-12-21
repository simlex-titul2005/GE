using Dapper;
using GE.WebUI.Models;
using GE.WebUI.ViewModels;
using GE.WebUI.ViewModels.Abstracts;
using System.Data.SqlClient;
using System.Linq;
using System;
using SX.WebCore;
using System.Text;

namespace GE.WebUI.Infrastructure.Repositories
{
    public sealed class RepoArticle : RepoMaterial<Article, VMArticle>
    {
        public RepoArticle() : base((byte)Enums.ModelCoreType.Article) { }

        public override void Delete(Article model)
        {
            const string query = "DELETE FROM D_ARTICLE WHERE Id=@mid AND ModelCoreType=@mct";
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Execute(query, new { mid = model.Id, mct = model.ModelCoreType });
            }

            base.Delete(model);
        }

        private static readonly string _queryPreviewMaterials = @"dbo.get_preview_materials @lettersCount, @gameTitle, @categoryId";

        public VMFGBlock ForGamersBlock(string gameTitle = null)
        {
            var viewModel = new VMFGBlock() { SelectedGameTitle = gameTitle };
            dynamic[] result;
            var query = @"SELECT da.GameId,
       dg.FrontPictureId,
       dg.Title,
       dg.TitleUrl,
       dg.[Description],
       dm.CategoryId,
       dmc.Title                 AS CategoryTitle,
       dbo.get_comments_count(dm.Id, dm.ModelCoreType) as CommentsCount
FROM   DV_MATERIAL               AS dm
       JOIN D_ARTICLE            AS da
            ON  da.Id = dm.Id
            AND da.ModelCoreType = dm.ModelCoreType
       JOIN D_GAME               AS dg
            ON  dg.Id = da.GameId
            AND dg.Show = 1
            AND dg.FrontPictureId IS NOT NULL
       JOIN D_MATERIAL_CATEGORY  AS dmc
            ON  dmc.Id = dm.CategoryId
WHERE  dm.Show = 1
       AND dm.DateOfPublication <= GETDATE()
GROUP BY
       dm.Id,
       dm.ModelCoreType,
       da.GameId,
       dg.FrontPictureId,
       dg.Title,
       dg.TitleUrl,
       dg.[Description],
       dm.CategoryId,
       dmc.Title";
            using (var conn = new SqlConnection(ConnectionString))
            {
                result = conn.Query<dynamic>(query).ToArray();
            }

            var games = result
                .Select(x => new
                {
                    Id = x.GameId,
                    Description = x.Description,
                    FrontPictureId = x.FrontPictureId,
                    Title = x.Title,
                    TitleUrl = x.TitleUrl
                }).Distinct().ToArray();

            viewModel.Games = new VMFGBGame[games.Length];
            for (var i = 0; i < games.Length; i++)
            {
                var game = games[i];
                var categories = result.Where(x => x.GameId == game.Id).Select(x => new { Id = x.CategoryId, Title = x.CategoryTitle }).Distinct().OrderBy(x => x.Title);
                viewModel.Games[i] = new VMFGBGame
                {
                    Id = game.Id,
                    Description = game.Description,
                    FrontPictureId = game.FrontPictureId,
                    Title = game.Title,
                    TitleUrl = game.TitleUrl,
                    MaterialCategories = categories.Select(x => new VMMaterialCategory { Id = x.Id, Title = x.Title }).ToArray()
                };
            }

            query = _queryPreviewMaterials;
            using (var conn = new SqlConnection(ConnectionString))
            {
                var articles = conn.Query<VMMaterial>(query, new { gameTitle = gameTitle, categoryId = (string)null, lettersCount = 200 }).ToArray();
                viewModel.Articles = articles;
            }

            return viewModel;
        }

        public VMMaterial[] PreviewMaterials(string gameTitle, string categoryId, int lettersCount = 200)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<VMMaterial>(_queryPreviewMaterials, new { gameTitle = gameTitle, categoryId = categoryId, lettersCount = lettersCount }).ToArray();
                return data;
            }
        }

        public VMMaterial[] Last(int amount)
        {
            const string query = @"SELECT TOP(@amount) *
FROM   (
           SELECT TOP(@amount) dm.DateCreate,
                  dm.Title,
                  dm.TitleUrl,
                  dm.ModelCoreType
           FROM   DV_MATERIAL     AS dm
                  JOIN D_ARTICLE  AS da
                       ON  da.ModelCoreType = dm.ModelCoreType
                       AND da.Id = dm.Id
           WHERE  dm.Show = 1
                  AND dm.DateOfPublication <= GETDATE()
           ORDER BY
                  dm.DateCreate DESC
           UNION
           SELECT TOP(@amount) dm.DateCreate,
                  dm.Title,
                  dm.TitleUrl,
                  dm.ModelCoreType
           FROM   DV_MATERIAL  AS dm
                  JOIN D_NEWS  AS dn
                       ON  dn.ModelCoreType = dm.ModelCoreType
                       AND dn.Id = dm.Id
           WHERE  dm.Show = 1
                  AND dm.DateOfPublication <= GETDATE()
           ORDER BY
                  dm.DateCreate DESC
       ) x
ORDER BY
       x.DateCreate DESC";
            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<VMMaterial>(query, new { amount = amount });
                return data.ToArray();
            }
        }

        public VMMaterial[] GetPopular(byte mct, int mid, int amount)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var data = connection.Query<VMMaterial>("dbo.get_popular_materials @mid, @mct, @amount", new { mct = mct, mid = mid, amount = amount }).ToArray();
                return data;
            }
        }

        protected override Action<SqlConnection, Article> ChangeMaterialBeforeSelect => (connection, model) =>
        {
            var data = connection.Query<dynamic>("dbo.get_material_game @id, @mct", new { id = model.Id, mct = model.ModelCoreType }).SingleOrDefault();
            if (data == null) return;

            model.Game = new Game { Id = data.Id, Title = data.Title };
            model.GameId = data.Id;
            model.GameVersion = GetGameVesion(model.ModelCoreType, data);
        };

        protected override Action<SxFilter, StringBuilder> ChangeJoinBody => (filter, sb) =>
        {
            sb.Append(" JOIN D_ARTICLE AS da ON da.Id=dm.Id AND da.ModelCoreType=dm.ModelCoreType ");
            sb.Append(" LEFT JOIN D_GAME AS dg ON dg.Id=da.GameId ");
        };
    }
}