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
            var query = @"SELECT TOP(@AMOUNT) dm.ID,
       dm.DateCreate,
       dm.Title, dm.TitleUrl,
       dm.FrontPictureId,
       dn.GameId,
       dg.TitleUrl          AS GameTitle
FROM   (
           SELECT MAX(dm.DateCreate)  AS DateCreate,
                  dn.GameId           AS GameId
           FROM   D_NEWS              AS dn
                  JOIN DV_MATERIAL    AS dm
                       ON  dm.ID = dn.ID
                       AND dm.ModelCoreType = dn.ModelCoreType
           GROUP BY
                  dn.GameId
       ) X
       JOIN D_NEWS       AS dn
            ON  dn.GameId = X.GameId
       JOIN DV_MATERIAL  AS dm
            ON  dm.ID = dn.ID
            AND dm.ModelCoreType = dn.ModelCoreType
            AND dm.DateCreate = X.DateCreate
       JOIN D_GAME       AS dg
            ON  dg.ID = dn.GameId
ORDER BY
       X.DateCreate DESC";
            var viewModel = new VMLastNewsBlock(amount);
            using (var conn = new SqlConnection(repo.ConnectionString))
            {
                var result = conn.Query<VMLastNewsBlockNews>(query, new { AMOUNT = amount });
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
       dg.TitleUrl           AS GameTitleUrl,
       CASE 
            WHEN dm.Foreword IS NOT NULL THEN dm.Foreword
            ELSE SUBSTRING(dbo.FUNC_STRIP_HTML(dm.Html), 0, 200) +
                 '...'
       END                   AS Foreword,
       (
           SELECT ISNULL(SUM(1), 0)
           FROM   D_LIKE AS dl
           WHERE  dl.Direction = 1
                  AND dl.MaterialId = dm.Id
                  AND dl.ModelCoreType = dm.ModelCoreType
       )                     AS LikeUpCount,
       (
           SELECT ISNULL(SUM(1), 0)
           FROM   D_LIKE AS dl
           WHERE  dl.Direction = 0
                  AND dl.MaterialId = dm.Id
                  AND dl.ModelCoreType = dm.ModelCoreType
       )                     AS LikeDownCount,
       anu.NikName           AS UserNikName
FROM   D_NEWS                AS dn
       JOIN DV_MATERIAL      AS dm
            ON  dm.Id = dn.Id
            AND dm.ModelCoreType = dn.ModelCoreType
       LEFT JOIN D_GAME      AS dg
            ON  dg.Id = dn.GameId
       JOIN AspNetUsers      AS anu
            ON  anu.Id = dm.UserId
WHERE  dm.TitleUrl = @title_url";

                return conn.Query<VMDetailNews>(query, new { title_url = titleUrl }).SingleOrDefault();
            }
        }
    }
}