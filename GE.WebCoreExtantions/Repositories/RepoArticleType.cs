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
    public sealed class RepoArticleType : SX.WebCore.Abstract.SxDbRepository<int, ArticleType, DbContext>
    {
        public override int Count(SxFilter filter)
        {
            var f = (Filter)filter;
            using (var conn = new SqlConnection(base.ConnectionString))
            {
                var query = @"SELECT COUNT(1) FROM D_ARTICLE_TYPE AS dat";
                var data = conn.Query<int>(query).Single();

                return (int)data;
            }
        }

        public override void Delete(params object[] id)
        {
            using (var conn = new SqlConnection(base.ConnectionString))
            {
                var query = @"UPDATE D_ARTICLE
SET
	ArticleTypeName = NULL,
	ArticleTypeGameId = NULL
WHERE ArticleTypeName=@AT_NAME AND ArticleTypeGameId=@AT_GAME_ID ";
                conn.Execute(query, new { @AT_NAME = id[0], @AT_GAME_ID=id[1]});
            }
            base.Delete(id);
        }
    }
}
