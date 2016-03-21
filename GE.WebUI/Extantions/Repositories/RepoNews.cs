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

        public static VMDetailNews GetByTitleUrl(this GE.WebCoreExtantions.Repositories.RepoNews repo, string titleUrl)
        {
            using (var conn = new SqlConnection(repo.ConnectionString))
            {
                var query = @"SELECT dn.*,
       dm.*,
       (
           SELECT ISNULL(SUM(1), 0)
           FROM   D_VOTE AS dv1
           WHERE  dv1.IsUp = 1
                  AND dv1.MaterialId = dm.Id
                  AND dv1.ModelCoreType = dm.ModelCoreType
       )                 AS VoteUpCount,
       (
           SELECT ISNULL(SUM(1), 0)
           FROM   D_VOTE AS dv1
           WHERE  dv1.IsUp = 0
                  AND dv1.MaterialId = dm.Id
                  AND dv1.ModelCoreType = dm.ModelCoreType
       )                 AS VoteDownCount
FROM   D_NEWS            AS dn
       JOIN DV_MATERIAL  AS dm
            ON  dm.Id = dn.Id
            AND dm.ModelCoreType = dn.ModelCoreType
WHERE  dm.TitleUrl = @TITLE_URL";

                return conn.Query<VMDetailNews>(query, new { TITLE_URL = titleUrl }).FirstOrDefault();
            }
        }
    }
}