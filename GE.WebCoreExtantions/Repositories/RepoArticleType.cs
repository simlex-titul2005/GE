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
                    var query = @"select * from D_ARTICLE_TYPE a join D_GAME g on g.ID=a.GAME_ID order by g.TITLE, a.NAME";
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
