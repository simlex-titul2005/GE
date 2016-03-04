using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace GE.WebCoreExtantions.Repositories
{
    public sealed class RepoArticleType : SX.WebCore.Abstract.SxDbRepository<int, ArticleType, DbContext>
    {
        public override IQueryable<ArticleType> All
        {
            get
            {
                using (var conn = new SqlConnection(base.ConnectionString))
                {
                    var query = @"SELECT*FROM D_ARTICLE_TYPE AS dat
JOIN D_GAME AS dg ON dg.Id = dat.GameId
ORDER BY dg.Title, dat.Name";
                    var data = conn.Query<ArticleType, Game, ArticleType>(query, (a, g) => {
                        a.Game = new Game { Title = g.Title, Id=g.Id };
                        return a;
                    });

                    return data.AsQueryable();
                }
            }
        }
    }
}
