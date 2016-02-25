using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

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
    }
}
