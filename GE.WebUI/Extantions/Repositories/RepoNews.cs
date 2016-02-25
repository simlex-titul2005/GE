using GE.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Dapper;

namespace GE.WebUI.Extantions.Repositories
{
    public static partial class RepositoryExtantions
    {
        public static VMLastNewsBlock LastNewsBlock(this GE.WebCoreExtantions.Repositories.RepoNews repo, int amount=5)
        {
            var viewModel = new VMLastNewsBlock(amount);
            using (var conn = new SqlConnection(repo.ConnectionString))
            {
                var result = conn.Query<VMLastNewsBlockNews>(Resources.Sql_News.LastNews, new { AMOUNT = amount });
                viewModel.News = result.ToArray();
            }

            return viewModel;
        }
    }
}