using System.Data.SqlClient;
using System.Linq;
using Dapper;
using SX.WebCore;
using SX.WebCore.Providers;
using static SX.WebCore.HtmlHelpers.SxExtantions;
using static SX.WebCore.Enums;
using SX.WebCore.Repositories;
using System.Text;

namespace GE.WebCoreExtantions.Repositories
{
    public sealed class RepoNews : SxRepoMaterial<News, DbContext>
    {
        public RepoNews() : base(ModelCoreType.News) { }

        public override IQueryable<News> All
        {
            get
            {
                using (var conn = new SqlConnection(base.ConnectionString))
                {
                    var query = @"SELECT*FROM D_NEWS AS dn
JOIN DV_MATERIAL AS dm ON dm.ID = dn.ID AND dm.ModelCoreType = dn.ModelCoreType
ORDER BY dm.DateCreate DESC";

                    return conn.Query<News>(query).AsQueryable();
                }
            }
        }

        public override News[] Read(SxFilter filter)
        {
            var sb = new StringBuilder();
            sb.Append(SxQueryProvider.GetSelectString(new string[] {
                "da.Id",
                "dm.TitleUrl",
                "dm.FrontPictureId",
                "dm.ShowFrontPictureOnDetailPage",
                "dm.Title",
                "dm.ModelCoreType",
                "dbo.get_comments_count(dm.Id, dm.ModelCoreType) AS CommentsCount",
                @"(SELECT
       SUBSTRING(
           CASE 
                WHEN dm.Foreword IS NOT NULL THEN dm.Foreword
                ELSE dbo.func_strip_html(dm.HTML)
           END,
           0,
           200
       ))                 AS Foreword",
                "dm.DateCreate",
                "dm.DateOfPublication",
                "dm.ViewsCount",

                "dst.Id",
                "dst.H1",

                "anu.Id",
                "anu.NikName",

                "dg.Id",
                "dg.Title",
                "dg.TitleUrl",
                "dg.BadPictureId"
            }));
            sb.Append(" FROM D_NEWS AS da ");
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
            var gws = getNewsWhereString(filter, out param);
            sb.Append(gws);

            var defaultOrder = new SxOrder { FieldName = "dm.DateCreate", Direction = SortDirection.Desc };
            sb.Append(SxQueryProvider.GetOrderString(defaultOrder, filter.Order));

            sb.AppendFormat(" OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", filter.PagerInfo.SkipCount, filter.PagerInfo.PageSize);

            //count
            var sbCount = new StringBuilder();
            sbCount.Append(@"SELECT COUNT(1) FROM D_NEWS AS da ");
            sbCount.Append(joinString);
            sbCount.Append(gws);

            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<News, SxSeoTags, SxAppUser, Game, News>(sb.ToString(), (da, st, anu, dg) =>
                {
                    da.SeoTags = st;
                    da.Game = dg;
                    da.User = anu;
                    return da;
                }, param: param, splitOn: "Id");

                filter.PagerInfo.TotalItems = conn.Query<int>(sbCount.ToString(), param: param).SingleOrDefault();
                return data.ToArray();
            }
        }

        private static string getNewsWhereString(SxFilter filter, out object param)
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

        public News[] GetLikeMaterial(SxFilter filter, int amount)
        {
            var query = @"SELECT DISTINCT TOP(@amount)
       dm.DateCreate,
       dm.TitleUrl,
       dm.Title,
       dm.ModelCoreType,
       SUBSTRING(dm.Foreword, 0, 200)  AS Foreword,
       dm.UserId,
       anu.NikName
FROM   D_MATERIAL_TAG    AS dmt
       JOIN DV_MATERIAL  AS dm
            ON  dm.Id = dmt.MaterialId
            AND dm.ModelCoreType = dmt.ModelCoreType
            AND dm.Id NOT IN (@mid)
       JOIN AspNetUsers  AS anu
            ON  anu.Id = dm.UserId
WHERE  dmt.Id IN (SELECT dmt2.Id
                  FROM   D_MATERIAL_TAG AS dmt2
                  WHERE  dmt2.MaterialId = @mid
                         AND dmt2.ModelCoreType = @mct)
ORDER BY
       dm.DateCreate DESC";

            using (var conn = new SqlConnection(this.ConnectionString))
            {
                var data = conn.Query<News, SxAppUser, News>(query, (m, u) =>
                {
                    m.User = u;
                    return m;
                }, new { mid = filter.MaterialId, mct = filter.ModelCoreType, amount = amount }, splitOn: "UserId");
                return data.ToArray();
            }
        }
    }
}
