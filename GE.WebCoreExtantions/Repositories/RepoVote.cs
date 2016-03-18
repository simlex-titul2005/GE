using SX.WebCore;
using SX.WebCore.Abstract;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace GE.WebCoreExtantions.Repositories
{
    public sealed class RepoVote : SxDbRepository<int, SxVote, DbContext>
    {
        public override SxVote Create(SxVote model)
        {
            return base.Create(model);
        }
    }
}
