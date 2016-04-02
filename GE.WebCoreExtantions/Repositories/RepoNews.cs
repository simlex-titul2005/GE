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
                query += @" ORDER BY
       dm.DateCreate DESC";
                if (f != null && f.SkipCount.HasValue && f.PageSize.HasValue)
                    query += " OFFSET " + filter.SkipCount + " ROWS FETCH NEXT " + filter.PageSize + " ROWS ONLY";

                var data = conn.Query<News, SxAppUser, Game, News>(query, (da, anu, dg) => {
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
LEFT JOIN D_GAME dg ON dg.Id=da.GameId
WHERE @GAME_TITLE IS NULL OR dg.Title=@GAME_TITLE";
                var data = f != null && !string.IsNullOrEmpty(f.GameTitle)
                    ? conn.Query<int>(query, new { GAME_TITLE = f.GameTitle }).Single()
                    : conn.Query<int>(query, new { GAME_TITLE = (string)null }).Single();

                return (int)data;
            }
        }
    }
}
