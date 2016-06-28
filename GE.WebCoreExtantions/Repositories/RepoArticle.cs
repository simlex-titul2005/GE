using System.Data.SqlClient;
using System.Linq;
using Dapper;
using SX.WebCore.Abstract;
using SX.WebCore;
using SX.WebCore.Providers;
using static SX.WebCore.HtmlHelpers.SxExtantions;
using static SX.WebCore.Enums;

namespace GE.WebCoreExtantions.Repositories
{
    public sealed class RepoArticle : SxDbRepository<int, Article, DbContext>
    {
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

        public override Article[] Query(SxFilter filter)
        {
            var query = SxQueryProvider.GetSelectString(new string[] {
                "da.Id", "dm.TitleUrl", "dm.FrontPictureId", "dm.ShowFrontPictureOnDetailPage", "dm.Title",
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
                "dm.UserId",
                "anu.NikName",
                "da.GameId",
                "dg.Title",
                "dg.TitleUrl",
                "dg.BadPictureId",
            });
            query +=  @" FROM D_ARTICLE AS da
       JOIN DV_MATERIAL  AS dm
            ON  dm.Id = da.ID
            AND dm.ModelCoreType = da.ModelCoreType
       LEFT JOIN AspNetUsers AS anu ON anu.Id=dm.UserId
       LEFT JOIN D_GAME  AS dg
            ON  dg.Id = da.GameId";

            var queryForVideos = @"SELECT TOP(1) * 
FROM   D_VIDEO_LINK  AS dvl
       JOIN D_VIDEO  AS dv
            ON  dv.Id = dvl.VideoId
WHERE  dvl.MaterialId = @mid
       AND dvl.ModelCoreType = @mct
ORDER BY dv.DateCreate DESC";

            object param = null;
            query += getArticleWhereString(filter, out param);

            var defaultOrder = new SxOrder { FieldName = "dm.DateCreate", Direction = SortDirection.Desc };
            query += SxQueryProvider.GetOrderString(defaultOrder, filter.Order);

            query += " OFFSET " + filter.PagerInfo.SkipCount + " ROWS FETCH NEXT " + filter.PagerInfo.PageSize + " ROWS ONLY";

            using (var conn = new SqlConnection(base.ConnectionString))
            {
                var data = conn.Query<Article, SxAppUser, Game, Article>(query, (da, anu, dg)=> {
                    da.Game = dg;
                    da.User = anu;
                    return da;
                }, param: param, splitOn:"UserId, GameId");

                if (data.Any())
                {
                    foreach (var item in data)
                    {
                        item.VideoLinks = conn.Query<SxVideoLink, SxVideo, SxVideoLink>(queryForVideos, (vl, v)=> {
                            vl.Video = v;
                            return vl;
                        }, new { mid = item.Id, mct = SX.WebCore.Enums.ModelCoreType.Article }).ToArray();
                    }
                }


                return data.ToArray();
            }
        }

        public override int Count(SxFilter filter)
        {
            var query = @"SELECT COUNT(1) FROM D_ARTICLE AS da
       JOIN DV_MATERIAL  AS dm
            ON  dm.Id = da.ID
            AND dm.ModelCoreType = da.ModelCoreType
       LEFT JOIN AspNetUsers AS anu ON anu.Id=dm.UserId
       LEFT JOIN D_GAME  AS dg
            ON  dg.Id = da.GameId";

            object param = null;
            query += getArticleWhereString(filter, out param);

            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<int>(query, param: param).SingleOrDefault();
                return data;
            }
        }

        private static string getArticleWhereString(SxFilter filter, out object param)
        {
            param = null;
            string query = null;
            query += " WHERE (dg.TitleUrl LIKE '%'+@gtu+'%' OR @gtu IS NULL) AND dm.DateOfPublication <= GETDATE() AND dm.Show=1 ";
            if (!string.IsNullOrEmpty(filter.Tag))
            {
                query += @" AND (dm.Id IN (SELECT dmt.MaterialId
                  FROM D_MATERIAL_TAG AS dmt WHERE dmt.MaterialId = dm.Id AND dmt.ModelCoreType = dm.ModelCoreType AND dmt.Id=N''+@tag+'')) ";

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

            return query;
        }

        public Article[] GetLikeMaterial(SxFilter filter, int amount)
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
                var data = conn.Query<Article, SxAppUser, Article>(query, (m, u) => {
                    m.User = u;
                    return m;
                }, new { mid = filter.MaterialId, mct = filter.ModelCoreType, amount= amount }, splitOn: "UserId");
                return data.ToArray();
            }
        }

        public void AddUserView(int mid, ModelCoreType mct)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Execute("add_material_view @mid, @mct", new { mid = mid, mct = mct });
            }
        }
    }
}
