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
                return base.All;
            }
        }
    }
}
