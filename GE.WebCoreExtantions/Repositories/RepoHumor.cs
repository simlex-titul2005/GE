using Dapper;
using SX.WebCore;
using SX.WebCore.Providers;
using SX.WebCore.Repositories;
using System.Data.SqlClient;
using System.Linq;
using static SX.WebCore.Enums;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace GE.WebCoreExtantions.Repositories
{
    public sealed class RepoHumor: SxRepoMaterial<SxHumor, DbContext>
    {
        public RepoHumor() : base(ModelCoreType.Humor) { }

        public override IQueryable<SxHumor> All
        {
            get
            {
                using (var conn = new SqlConnection(base.ConnectionString))
                {
                    var query = @"SELECT * FROM D_HUMOR AS dh
JOIN DV_MATERIAL AS dm ON dm.ID = dh.ID AND dm.ModelCoreType = dh.ModelCoreType
ORDER BY dm.DateCreate DESC";

                    return conn.Query<SxHumor>(query).AsQueryable();
                }
            }
        }

        public override SxHumor[] Query(SxFilter filter)
        {
            var query = SxQueryProvider.GetSelectString(new string[] {
                "da.Id", "dm.TitleUrl", "dm.FrontPictureId", "dm.ShowFrontPictureOnDetailPage", "dm.Title", "dm.ModelCoreType",
                @"(SELECT
       SUBSTRING(
           CASE 
                WHEN dm.Foreword IS NOT NULL THEN dm.Foreword
                ELSE dbo.func_strip_html(dm.HTML)
           END,
           0,
           200
       ))                 AS Foreword",
                "dm.Html",
                "dm.DateCreate",
                "dm.DateOfPublication",
                "dm.ViewsCount",
                "(SELECT COUNT(1) FROM D_COMMENT dc WHERE dc.MaterialId=dm.Id AND dc.ModelCoreType=dm.ModelCoreType ) AS CommentsCount",
                "dm.UserId",
                "anu.NikName",
            });
            query += @" FROM D_HUMOR AS da
       JOIN DV_MATERIAL  AS dm
            ON  dm.Id = da.ID
            AND dm.ModelCoreType = da.ModelCoreType
       LEFT JOIN AspNetUsers AS anu ON anu.Id=dm.UserId";

            var queryForVideos = @"SELECT TOP(1) * 
FROM   D_VIDEO_LINK  AS dvl
       JOIN D_VIDEO  AS dv
            ON  dv.Id = dvl.VideoId
WHERE  dvl.MaterialId = @mid
       AND dvl.ModelCoreType = @mct
ORDER BY dv.DateCreate DESC";

            object param = null;
            query += getHumorWhereString(filter, out param);

            var defaultOrder = new SxOrder { FieldName = "dm.DateCreate", Direction = SortDirection.Desc };
            query += SxQueryProvider.GetOrderString(defaultOrder, filter.Order);

            query += " OFFSET " + filter.PagerInfo.SkipCount + " ROWS FETCH NEXT " + filter.PagerInfo.PageSize + " ROWS ONLY";

            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<SxHumor, SxAppUser, SxHumor>(query, (da, anu) => {
                    da.User = anu;
                    return da;
                }, param: param, splitOn: "UserId");

                if (data.Any())
                {
                    foreach (var item in data)
                    {
                        item.VideoLinks = conn.Query<SxVideoLink, SxVideo, SxVideoLink>(queryForVideos, (vl, v) => {
                            vl.Video = v;
                            return vl;
                        }, new { mid = item.Id, mct = ModelCoreType.Humor }).ToArray();
                    }
                }


                return data.ToArray();
            }
        }

        public override int Count(SxFilter filter)
        {
            var query = @"SELECT COUNT(1) FROM D_HUMOR AS da
       JOIN DV_MATERIAL  AS dm
            ON  dm.Id = da.ID
            AND dm.ModelCoreType = da.ModelCoreType
       LEFT JOIN AspNetUsers AS anu ON anu.Id=dm.UserId";

            object param = null;
            query += getHumorWhereString(filter, out param);

            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<int>(query, param: param).SingleOrDefault();
                return data;
            }
        }

        private static string getHumorWhereString(SxFilter filter, out object param)
        {
            param = null;
            string query = null;
            query += " WHERE dm.DateOfPublication <= GETDATE() AND dm.Show=1 ";
            if (!string.IsNullOrEmpty(filter.Tag))
            {
                query += @" AND (dm.Id IN (SELECT dmt.MaterialId
                  FROM D_MATERIAL_TAG AS dmt WHERE dmt.MaterialId = dm.Id AND dmt.ModelCoreType = dm.ModelCoreType AND dmt.Id=N''+@tag+'')) ";

                param = new
                {
                    tag = filter.Tag
                };
            }
            

            return query;
        }
    }
}
