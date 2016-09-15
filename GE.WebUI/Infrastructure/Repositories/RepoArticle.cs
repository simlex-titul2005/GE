using Dapper;
using GE.WebUI.Models;
using GE.WebUI.ViewModels;
using GE.WebUI.ViewModels.Abstracts;
using SX.WebCore;
using SX.WebCore.Providers;
using SX.WebCore.Repositories;
using SX.WebCore.ViewModels;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using static SX.WebCore.Enums;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace GE.WebUI.Infrastructure.Repositories
{
    public sealed class RepoArticle : SxRepoMaterial<Article, VMArticle, DbContext>
    {
        public RepoArticle() : base(ModelCoreType.Article) { }

        public override VMArticle[] Read(SxFilter filter)
        {
            var sb = new StringBuilder();
            sb.Append(SxQueryProvider.GetSelectString(new string[] {
                "da.Id",
                "dm.TitleUrl",
                "dm.FrontPictureId",
                "dm.ShowFrontPictureOnDetailPage",
                "dm.Title",
                "dm.ModelCoreType",
                @"(SELECT
       SUBSTRING(
           CASE 
                WHEN dm.Foreword IS NOT NULL THEN dm.Foreword
                ELSE dbo.func_strip_html(dm.HTML)
           END,
           0,
           400
       ))                 AS Foreword",
                "dm.DateCreate",
                "dm.DateOfPublication",
                "dm.ViewsCount",
                "(SELECT COUNT(1) FROM D_COMMENT dc WHERE dc.MaterialId=dm.Id AND dc.ModelCoreType=dm.ModelCoreType ) AS CommentsCount",

                "dst.Id",
                "dst.H1",

                "anu.Id",
                "anu.NikName",

                "dg.Id",
                "dg.Title",
                "dg.TitleUrl",
                "dg.BadPictureId",
            }));
            sb.Append(@" FROM D_ARTICLE AS da ");
            var joinString = @" JOIN DV_MATERIAL  AS dm
            ON  dm.Id = da.ID
            AND dm.ModelCoreType = da.ModelCoreType
       LEFT JOIN D_SEO_TAGS AS dst
            ON  dst.MaterialId = da.ID
            AND dst.ModelCoreType = da.ModelCoreType
       LEFT JOIN AspNetUsers AS anu ON anu.Id=dm.UserId
       LEFT JOIN D_GAME  AS dg
            ON  dg.Id = da.GameId ";
            sb.Append(joinString);

            object param = null;
            var gws = getArticleWhereString(filter, out param);
            sb.Append(gws);

            var defaultOrder = new SxOrder { FieldName = "dm.DateCreate", Direction = SortDirection.Desc };
            sb.Append(SxQueryProvider.GetOrderString(defaultOrder, filter.Order));

            sb.AppendFormat(" OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", filter.PagerInfo.SkipCount, filter.PagerInfo.PageSize);

            //count
            var sbCount = new StringBuilder();
            sbCount.Append(@"SELECT COUNT(1) FROM D_ARTICLE AS da ");
            sbCount.Append(joinString);
            sbCount.Append(gws);

            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<VMArticle, SxVMSeoTags, SxVMAppUser, VMGame, VMArticle>(sb.ToString(), (da, st, anu, dg) => {
                    da.SeoTags = st;
                    da.Game = dg;
                    da.User = anu;
                    return da;
                }, param: param, splitOn: "Id");

                //video
                if (data.Any())
                {
                    foreach (var item in data)
                    {
                        item.Videos = conn.Query<SxVMVideo>("dbo.get_first_material_video @mid, @mct", new { mid = item.Id, mct = ModelCoreType.Article }).ToArray();
                    }
                }
                filter.PagerInfo.TotalItems = conn.Query<int>(sbCount.ToString(), param: param).SingleOrDefault();
                return data.ToArray();
            }
        }
        private static string getArticleWhereString(SxFilter filter, out object param)
        {
            param = null;
            var query = new StringBuilder();
            query.Append(" WHERE (dg.TitleUrl LIKE '%'+@gtu+'%' OR @gtu IS NULL) AND dm.DateOfPublication <= GETDATE() AND dm.Show=1 ");
            if (filter.Tag!=null && !string.IsNullOrEmpty(filter.Tag.Id))
            {
                query.Append(" AND (dm.Id IN (SELECT dmt.MaterialId FROM D_MATERIAL_TAG AS dmt WHERE dmt.MaterialId = dm.Id AND dmt.ModelCoreType = dm.ModelCoreType AND dmt.Id=N''+@tag+'')) ");

                param = new
                {
                    gtu = (string)(filter.AddintionalInfo == null || filter.AddintionalInfo[0] == null ? null : filter.AddintionalInfo[0]),
                    tag = filter.Tag
                };
            }
            else
            {
                param = new
                {
                    gtu = (string)(filter.AddintionalInfo == null || filter.AddintionalInfo[0] == null ? null : filter.AddintionalInfo[0])
                };
            }

            return query.ToString();
        }

        public override void Delete(Article model)
        {
            var query = "DELETE FROM D_ARTICLE WHERE Id=@mid AND ModelCoreType=@mct";
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Execute(query, new { mid = model.Id, mct = model.ModelCoreType });
            }

            base.Delete(model);
        }

        private static readonly string _queryPreviewMaterials = @"get_preview_materials @lettersCount, @gameTitle, @categoryId";

        public VMFGBlock ForGamersBlock(string gameTitle = null)
        {
            var viewModel = new VMFGBlock() { SelectedGameTitle = gameTitle };
            dynamic[] result = null;
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
            for (int i = 0; i < games.Length; i++)
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
            var query = @"SELECT TOP(@amount) *
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

        public VMMaterial[] GetPopular(ModelCoreType mct, int mid, int amount)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var data = connection.Query<VMMaterial>("dbo.get_popular_materials @mid, @mct, @amount", new { mct = mct, mid = mid, amount = amount }).ToArray();
                return data;
            }
        }
    }
}