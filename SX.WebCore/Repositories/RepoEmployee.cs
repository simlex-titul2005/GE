using Dapper;
using SX.WebCore.Abstract;
using System.Data.SqlClient;

namespace SX.WebCore.Repositories
{
    public sealed class RepoEmployee<TDbContext> : SxDbRepository<string, SxEmployee, TDbContext> where TDbContext : SxDbContext
    {
        public override SxEmployee Create(SxEmployee model)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Execute("add_employee @uid", new
                {
                    uid = model.Id
                });
            }
            return null;
        }

        public override void Delete(params object[] id)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Execute("del_employee @uid", new
                {
                    uid = id[0]
                });
            }
        }
    }
}
