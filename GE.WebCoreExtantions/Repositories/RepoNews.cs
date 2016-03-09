using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using SX.WebCore.Abstract;

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
            var f = filter as Filter;
            f.GameTitle = !string.IsNullOrEmpty(f.GameTitle) ? f.GameTitle : null;
            using (var conn = new SqlConnection(base.ConnectionString))
            {
                var query = @"SELECT da.Id,
       dm.TitleUrl,
       dm.FrontPictureId,
       dm.Title,
       SUBSTRING(
           CASE 
                WHEN dm.Foreword IS NOT NULL THEN dm.Foreword
                ELSE dbo.FUNC_STRIP_HTML(dm.HTML)
           END,
           0,
           200
       )                 AS Foreword,
       dm.DateCreate,
       dm.ViewsCount,
       dm.CommentsCount
FROM   D_NEWS         AS da
       LEFT JOIN D_GAME  AS dg
            ON  dg.ID = da.GameId
       JOIN DV_MATERIAL  AS dm
            ON  dm.ID = da.ID
            AND dm.ModelCoreType = da.ModelCoreType
       WHERE  dg.Title = @GAME_TITLE
       OR  @GAME_TITLE IS NULL
       ORDER BY dm.DateCreate DESC";
                if (f != null && f.SkipCount.HasValue && f.PageSize.HasValue)
                    query += " OFFSET " + f.SkipCount + " ROWS FETCH NEXT " + f.PageSize + " ROWS ONLY";

                var data = conn.Query<News, Game, News>(query, (n, g)=>{
                    n.Game = new Game();
                    return n;
                }, param: new { GAME_TITLE = f.GameTitle }, splitOn: "Game");

                return data.AsQueryable();
            }
        }

        public IQueryable<News> QueryForAdmin(SxFilter filter)
        {
            var f = filter as Filter;
            using (var conn = new SqlConnection(base.ConnectionString))
            {
                var query = @"SELECT da.Id,
       dm.DateCreate,
       dm.Title
FROM   D_NEWS         AS da
       JOIN DV_MATERIAL  AS dm
            ON  dm.ID = da.ID
            AND dm.ModelCoreType = da.ModelCoreType
       LEFT JOIN D_GAME  AS dg
            ON  dg.ID = da.GameId";
                if (f != null && !string.IsNullOrEmpty(f.GameTitle))
                    query += @" WHERE  dg.Title = @GAME_TITLE
       OR  @GAME_TITLE IS NULL";
                query += @" ORDER BY
       dm.DateCreate DESC";
                if (f != null && f.SkipCount.HasValue && f.PageSize.HasValue)
                    query += " OFFSET " + f.SkipCount + " ROWS FETCH NEXT " + f.PageSize + " ROWS ONLY";

                var data = f != null && !string.IsNullOrEmpty(f.GameTitle) ? conn.Query<News>(query, new { GAME_TITLE = f.GameTitle }) : conn.Query<News>(query);

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

        public News GetByTitleUrl(string titleUrl)
        {
            using (var conn = new SqlConnection(base.ConnectionString))
            {
                var query = @"SELECT*FROM D_NEWS AS dn
JOIN DV_MATERIAL AS dm ON dm.ID = dn.ID AND dm.ModelCoreType = dn.ModelCoreType
WHERE dm.TitleUrl=@TITLE_URL";

                return conn.Query<News>(query, new { TITLE_URL = titleUrl }).FirstOrDefault();
            }
        }
    }
}
