using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using SX.WebCore.Abstract;
using SX.WebCore;

namespace GE.WebCoreExtantions.Repositories
{
    public sealed class RepoNews : SX.WebCore.Abstract.SxDbRepository<int, News, DbContext>
    {
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

        public override IQueryable<News> Query(SxFilter filter)
        {
            var f = (Filter)filter;
            f.GameTitle = !string.IsNullOrEmpty(f.GameTitle) ? f.GameTitle : null;
            using (var conn = new SqlConnection(base.ConnectionString))
            {
                var query = @"SELECT da.Id,
       dm.TitleUrl,
       dm.FrontPictureId,
       dm.ShowFrontPictureOnDetailPage,
       dm.Title,
       SUBSTRING(
           CASE 
                WHEN dm.Foreword IS NOT NULL THEN dm.Foreword
                ELSE dbo.FUNC_STRIP_HTML(dm.HTML)
           END,
           0,
           200
       )+'...'                AS Foreword,
       dm.DateCreate,
       dm.DateOfPublication,
       dm.ViewsCount,
       dm.CommentsCount,
       dm.UserId,
       anu.NikName,
       da.GameId,
       dg.Title,
       dg.TitleUrl,
       dg.FrontPictureId,
       dg.GoodPictureId
FROM   D_NEWS         AS da
       JOIN DV_MATERIAL  AS dm
            ON  dm.Id = da.ID
            AND dm.ModelCoreType = da.ModelCoreType
       LEFT JOIN AspNetUsers AS anu ON anu.Id=dm.UserId
       LEFT JOIN D_GAME  AS dg
            ON  dg.Id = da.GameId WHERE  (dg.TitleUrl = @GAME_TITLE_URL
       OR  @GAME_TITLE_URL IS NULL) AND dm.DateOfPublication <= GETDATE() AND dm.Show=1";
                if (!string.IsNullOrEmpty(f.Tag))
                    query += string.Format(@" AND (dm.Id IN (SELECT dmt.MaterialId
                  FROM D_MATERIAL_TAG AS dmt WHERE dmt.MaterialId = dm.Id AND dmt.ModelCoreType = dm.ModelCoreType AND dmt.Id=N'{0}'))", f.Tag);
                query += @" ORDER BY
       dm.DateCreate DESC";
                if (f != null && f.SkipCount.HasValue && f.PageSize.HasValue)
                    query += " OFFSET " + filter.SkipCount + " ROWS FETCH NEXT " + filter.PageSize + " ROWS ONLY";

                var data = conn.Query<News, SxAppUser, Game, News>(query, (da, anu, dg) =>
                {
                    da.Game = dg;
                    da.User = anu;
                    return da;
                }, new { GAME_TITLE_URL = f.GameTitle }, splitOn: "UserId, GameId");

                return data.AsQueryable();
            }
        }

        public override int Count(SxFilter filter)
        {
            var f = (Filter)filter;
            using (var conn = new SqlConnection(base.ConnectionString))
            {
                var query = @"SELECT COUNT(1) FROM D_NEWS AS da
JOIN DV_MATERIAL  AS dm
            ON  dm.Id = da.ID
            AND dm.ModelCoreType = da.ModelCoreType
LEFT JOIN D_GAME dg ON dg.Id=da.GameId
WHERE (@GAME_TITLE IS NULL OR dg.Title=@GAME_TITLE)";
                if (!string.IsNullOrEmpty(f.Tag))
                    query += string.Format(@" AND (dm.Id IN (SELECT dmt.MaterialId
                  FROM D_MATERIAL_TAG AS dmt WHERE dmt.MaterialId = dm.Id AND dmt.ModelCoreType = dm.ModelCoreType AND dmt.Id='{0}'))", f.Tag);
                var data = f != null && !string.IsNullOrEmpty(f.GameTitle)
                    ? conn.Query<int>(query, new { GAME_TITLE = f.GameTitle }).Single()
                    : conn.Query<int>(query, new { GAME_TITLE = (string)null }).Single();

                return (int)data;
            }
        }

        public News[] GetLikeMaterial(Filter filter)
        {
            var query = @"SELECT DISTINCT
       dm.DateCreate,
       dm.TitleUrl,
       dm.Title,
       dm.ModelCoreType
FROM   D_MATERIAL_TAG    AS dmt
       JOIN DV_MATERIAL  AS dm
            ON  dm.Id = dmt.MaterialId
            AND dm.ModelCoreType = dmt.ModelCoreType
            AND dm.Id NOT IN (@mid)
WHERE  dmt.Id IN (SELECT dmt2.Id
                  FROM   D_MATERIAL_TAG AS dmt2
                  WHERE  dmt2.MaterialId = @mid
                         AND dmt2.ModelCoreType = @mct)
ORDER BY
       dm.DateCreate DESC";

            using (var conn = new SqlConnection(this.ConnectionString))
            {
                var data = conn.Query<News>(query, new { mid = filter.MaterialId, mct = filter.ModelCoreType });
                return data.ToArray();
            }
        }
    }
}
