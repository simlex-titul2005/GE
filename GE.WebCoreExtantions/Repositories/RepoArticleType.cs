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
    }
}
