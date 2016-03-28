using GE.WebCoreExtantions.Repositories;
using System.Data.SqlClient;
using System.Linq;
using Dapper;

namespace GE.WebUI.Extantions.Repositories
{
    public static partial class RepositoryExtantions
    {
        public static bool ExistGame(this RepoGame repo, string titleUrl)
        {
            var query = @"SELECT COUNT(1) FROM D_GAME AS dg WHERE dg.TitleUrl=@title_url";
            using (var connection = new SqlConnection(repo.ConnectionString))
            {
                var data = connection.Query<int>(query, new { title_url=titleUrl }).SingleOrDefault();
                return data == 1;
            }
        }
    }
}