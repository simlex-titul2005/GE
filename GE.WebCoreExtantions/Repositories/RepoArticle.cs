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
                    var query = @"SELECT*FROM D_ARTICLE AS da
JOIN DV_MATERIAL AS dm ON dm.ID = da.ID AND dm.ModelCoreType = da.ModelCoreType
LEFT JOIN D_GAME AS dg ON dg.ID = da.GameId
LEFT JOIN D_ARTICLE_TYPE AS dat ON dat.NAME = da.ArticleTypeName AND dat.GameId = da.ArticleTypeGameId";
                    var data = conn.Query<Article, Game, ArticleType, Article>(query, (da, dg, dat) =>
                    {
                        da.Game = dg != null ? new Game { Title = dg.Title, Id = dg.Id } : null;

                        da.ArticleType = dat != null ? new ArticleType { Id = dat.Id, Name = dat.Name, Description = dat.Description } : null;

                        return da;
                    });

                    return data.AsQueryable();
                }
            }
        }

        public override IQueryable<Article> Query(SxFilter filter)
        {
            var f = filter as Filter;
            using (var conn = new SqlConnection(base.ConnectionString))
            {
                var query = @"SELECT da.Id,
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
            ON  dm.ID = da.ID
            AND dm.ModelCoreType = da.ModelCoreType
ORDER BY
       dm.DateCreate DESC";
                
                var data = conn.Query<Article>(query);

                return data.AsQueryable();
            }
        }

        public override int Count
        {
            get
            {
                using (var conn = new SqlConnection(base.ConnectionString))
                {
                    var query = @"SELECT COUNT(1) FROM D_ARTICLE";
                    var data = conn.Query<int>(query).Single();

                    return (int)data;
                }
            }
        }
    }
}
