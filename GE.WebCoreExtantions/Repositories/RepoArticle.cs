using System.Data.SqlClient;
using System.Linq;
using Dapper;
using SX.WebCore;
using SX.WebCore.Providers;
using static SX.WebCore.HtmlHelpers.SxExtantions;
using static SX.WebCore.Enums;
using SX.WebCore.Repositories;
using System.Text;
using SX.WebCore.ViewModels;

namespace GE.WebCoreExtantions.Repositories
{
    public sealed class RepoArticle<TViewModel> : SxRepoMaterial<Article, TViewModel, DbContext>
        where TViewModel: SxVMMaterial
    {
        public RepoArticle() : base(ModelCoreType.Article) { }

        public override IQueryable<Article> All
        {
            get
            {
                using (var conn = new SqlConnection(base.ConnectionString))
                {
                    var query = @"SELECT * FROM D_ARTICLE AS da
JOIN DV_MATERIAL AS dm ON dm.ID = da.ID AND dm.ModelCoreType = da.ModelCoreType
ORDER BY dm.DateCreate DESC";

                    return conn.Query<Article>(query).AsQueryable();
                }
            }
        }

        public override Article[] Read(SxFilter filter)
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

            var queryForVideos = @"SELECT TOP(1) * 
FROM   D_VIDEO_LINK  AS dvl
       JOIN D_VIDEO  AS dv
            ON  dv.Id = dvl.VideoId
WHERE  dvl.MaterialId = @mid
       AND dvl.ModelCoreType = @mct
ORDER BY dv.DateCreate DESC";

            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<Article, SxSeoTags, SxAppUser, Game, Article>(sb.ToString(), (da, st, anu, dg) => {
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
                        item.VideoLinks = conn.Query<SxVideoLink, SxVideo, SxVideoLink>(queryForVideos, (vl, v) => {
                            vl.Video = v;
                            return vl;
                        }, new { mid = item.Id, mct = ModelCoreType.Article }).ToArray();
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
            if (!string.IsNullOrEmpty(filter.Tag))
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
    }
}
