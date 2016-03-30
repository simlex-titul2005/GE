using GE.WebUI.Models;
using System.Data.SqlClient;
using System.Linq;
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
       dg.TitleUrl       AS GameTitleUrl,
       CASE 
            WHEN dm.Foreword IS NOT NULL THEN dm.Foreword
            ELSE SUBSTRING(dbo.FUNC_STRIP_HTML(dm.Html), 0, 200) +
                 '...'
       END                    AS Foreword,
       (
           SELECT ISNULL(SUM(1), 0)
           FROM   D_LIKE AS dl
           WHERE  dl.Direction = 1
                  AND dl.MaterialId = dm.Id
                  AND dl.ModelCoreType = dm.ModelCoreType
       )                 AS LikeUpCount,
       (
           SELECT ISNULL(SUM(1), 0)
           FROM   D_LIKE AS dl
           WHERE  dl.Direction = 0
                  AND dl.MaterialId = dm.Id
                  AND dl.ModelCoreType = dm.ModelCoreType
       )                 AS LikeDownCount,
       anu.NikName AS UserNikName
FROM   D_NEWS            AS dn
       JOIN DV_MATERIAL  AS dm
            ON  dm.Id = dn.Id
            AND dm.ModelCoreType = dn.ModelCoreType
       LEFT JOIN D_GAME  AS dg
            ON  dg.Id = dn.GameId
            JOIN  AspNetUsers AS anu ON anu.Id = dm.UserId
WHERE  dm.TitleUrl = @TITLE_URL";

                return conn.Query<VMDetailNews>(query, new { TITLE_URL = titleUrl }).FirstOrDefault();
            }
        }
    }
}