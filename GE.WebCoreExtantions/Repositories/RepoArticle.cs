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
    public sealed class RepoArticle : SX.WebCore.Abstract.SxDbRepository<int, Article, DbContext>
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

        public override IQueryable<Article> Query(SxFilter filter)
        {
            var f = (Filter)filter;
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
           400
       )                 AS Foreword,
       dm.DateCreate,
       dm.ViewsCount,
       dm.CommentsCount
FROM   D_ARTICLE         AS da
       JOIN DV_MATERIAL  AS dm
            ON  dm.Id = da.ID
            AND dm.ModelCoreType = da.ModelCoreType
       LEFT JOIN D_GAME  AS dg
            ON  dg.Id = da.GameId";
                if (f != null && !string.IsNullOrEmpty(f.GameTitle))
                    query += @" WHERE  dg.TitleUrl = @GAME_TITLE_URL
       OR  @GAME_TITLE_URL IS NULL";
                query += @" ORDER BY
       dm.DateCreate DESC";
                if (f != null && f.SkipCount.HasValue && f.PageSize.HasValue)
                    query += " OFFSET " + filter.SkipCount + " ROWS FETCH NEXT " + filter.PageSize + " ROWS ONLY";

                var data = f != null && !string.IsNullOrEmpty(f.GameTitle) ? conn.Query<Article>(query, new { GAME_TITLE_URL = f.GameTitle }) : conn.Query<Article>(query);

                return data.AsQueryable();
            }
        }

        public IQueryable<Article> QueryForAdmin(SxFilter filter)
        {
            var f = (Filter)filter;
            using (var conn = new SqlConnection(base.ConnectionString))
            {
                var query = @"SELECT da.Id,
       dm.DateCreate,
       dm.Title
FROM   D_ARTICLE         AS da
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
                    query += " OFFSET " + filter.SkipCount + " ROWS FETCH NEXT " + filter.PageSize + " ROWS ONLY";

                var data = f != null && !string.IsNullOrEmpty(f.GameTitle) ? conn.Query<Article>(query, new { GAME_TITLE = f.GameTitle }) : conn.Query<Article>(query);

                return data.AsQueryable();
            }
        }

        public override int Count(SxFilter filter)
        {
            var f = (Filter)filter;
            using (var conn = new SqlConnection(base.ConnectionString))
            {
                var query = @"SELECT COUNT(1) FROM D_ARTICLE AS da
LEFT JOIN D_GAME dg ON dg.Id=da.GameId
WHERE @TITLE_URL IS NULL OR dg.TitleUrl=@TITLE_URL";
                var data = f != null && !string.IsNullOrEmpty(f.GameTitle)
                    ? conn.Query<int>(query, new { TITLE_URL = f.GameTitle }).Single()
                    : conn.Query<int>(query, new { TITLE_URL = (string)null }).Single();

                return (int)data;
            }
        }
    }
}
