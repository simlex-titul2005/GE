using GE.WebUI.Models;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using SX.WebCore;
using SX.WebCore.ViewModels;

namespace GE.WebUI.Extantions.Repositories
{
    public static partial class RepositoryExtantions
    {
        public static VMLGNB LastGameNewsBlock(this WebCoreExtantions.Repositories.RepoNews repo, int lnc, int gc, int glnc, int gtc)
        {
            var model = new VMLGNB(lnc, gc, glnc);

            var queryLastNews = @"SELECT TOP(@lnc) dm.DateOfPublication,
       dm.Title,
       dm.TitleUrl,
       dm.DateCreate,
       dn.GameId
FROM   DV_MATERIAL  AS dm
       JOIN D_NEWS  AS dn
            ON  dn.Id = dm.Id
            AND dn.ModelCoreType = dm.ModelCoreType
       JOIN D_GAME  AS dg
            ON  dg.Id = dn.GameId
            AND dg.Show = 1
WHERE  dm.Show = 1
       AND dm.DateOfPublication <= GETDATE()
ORDER BY
       dm.DateCreate DESC";

            var queryGames = @"SELECT TOP(@gc) x.TitleUrl,
       x.Id,
       x.Title,
       x.FrontPictureId
FROM   (
           SELECT dg.TitleUrl,
                  dg.Id,
                  dg.Title,
                  dg.FrontPictureId,
                  COUNT(dc.Id)           AS CommentsCount,
                  COUNT(dm.Id)           AS NewsCount
           FROM   D_GAME                 AS dg
                  LEFT JOIN D_NEWS       AS dn
                       ON  dn.GameId = dg.Id
                  LEFT JOIN DV_MATERIAL  AS dm
                       ON  dm.Id = dn.Id
                       AND dm.ModelCoreType = dn.ModelCoreType
                  LEFT JOIN D_COMMENT    AS dc
                       ON  dc.ModelCoreType = dm.ModelCoreType
                       AND dc.MaterialId = dm.Id
           WHERE  dg.Show = 1
           GROUP BY
                  dg.TitleUrl,
                  dg.Id,
                  dg.Title,
                  dg.FrontPictureId
       ) AS x
ORDER BY
       x.CommentsCount DESC";

            var queryGameNews = @"SELECT TOP(@glnc) dm.DateOfPublication,
       dm.Title,
       dm.TitleUrl,
       dm.FrontPictureId,
       dm.DateCreate
FROM   DV_MATERIAL  AS dm
       JOIN D_NEWS  AS dn
            ON  dn.Id = dm.Id
            AND dn.ModelCoreType = dm.ModelCoreType
       JOIN D_GAME  AS dg
            ON  dg.Id = dn.GameId
            AND dg.TitleUrl = @gturl
WHERE  dm.Show = 1
       AND dm.DateOfPublication <= GETDATE()
ORDER BY
       dm.DateOfPublication DESC";

            var queryGameTags= @"SELECT TOP(@amount)
       dmt.Id            AS Title,
       COUNT(dmt.Id)     AS [Count],
       1                 AS IsCurrent
FROM   D_MATERIAL_TAG    AS dmt
       JOIN D_NEWS       AS dn
            ON  dn.Id = dmt.MaterialId
            AND dn.ModelCoreType = dmt.ModelCoreType
       JOIN DV_MATERIAL  AS dm
            ON  dm.Id = dmt.MaterialId
            AND dm.ModelCoreType = dmt.ModelCoreType
            AND dm.Show = 1
            AND dm.DateOfPublication <= GETDATE()
       JOIN D_GAME       AS dg
            ON  dg.Id = dn.GameId
            AND dg.TitleUrl = @gturl
GROUP BY
       dmt.Id";

            using (var connection = new SqlConnection(repo.ConnectionString))
            {
                model.News = connection.Query<VMLGNBNews>(queryLastNews, new { lnc = lnc }).ToArray();
                model.Games = connection.Query<VMLGNBGame>(queryGames, new { gc = gc }).ToArray();
                if (model.Games.Any())
                {
                    for (int i = 0; i < model.Games.Length; i++)
                    {
                        var game = model.Games[i];
                        game.News = connection.Query<VMLGNBNews>(queryGameNews, new { glnc = glnc, gturl = game.TitleUrl }).ToArray();
                        game.Tags = connection.Query<SxVMMaterialTag>(queryGameTags, new { amount = gtc, gturl = game.TitleUrl }).ToArray();
                    }
                }
            }


            return model;
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
       END               AS Foreword,
       (
           SELECT ISNULL(SUM(1), 0)
           FROM   D_USER_CLICK  AS duc
                  JOIN D_LIKE   AS dl
                       ON  dl.UserClickId = duc.Id
           WHERE  duc.MaterialId = dm.Id
                  AND duc.ModelCoreType = dm.ModelCoreType
                  AND dl.Direction = 1
       )                 AS LikeUpCount,
       (
           SELECT ISNULL(SUM(1), 0)
           FROM   D_USER_CLICK  AS duc
                  JOIN D_LIKE   AS dl
                       ON  dl.UserClickId = duc.Id
           WHERE  duc.MaterialId = dm.Id
                  AND duc.ModelCoreType = dm.ModelCoreType
                  AND dl.Direction = 2
       )                 AS LikeDownCount,
       (
           SELECT COUNT(1)
           FROM   D_COMMENT AS dc
           WHERE  dc.MaterialId = dm.Id
                  AND dc.ModelCoreType = dm.ModelCoreType
       )                 AS CommentsCount,
       anu.NikName       AS UserNikName
FROM   D_NEWS            AS dn
       JOIN DV_MATERIAL  AS dm
            ON  dm.Id = dn.Id
            AND dm.ModelCoreType = dn.ModelCoreType
       LEFT JOIN D_GAME  AS dg
            ON  dg.Id = dn.GameId
       JOIN AspNetUsers  AS anu
            ON  anu.Id = dm.UserId
WHERE  dm.TitleUrl = @title_url";

                return conn.Query<VMDetailNews>(query, new { title_url = titleUrl }).SingleOrDefault();
            }
        }
    }
}