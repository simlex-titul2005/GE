using GE.WebCoreExtantions.Repositories;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using GE.WebUI.Models;
using static SX.WebCore.Enums;

namespace GE.WebUI.Extantions.Repositories
{
    public static partial class RepositoryExtantions
    {
        public static bool ExistGame(this RepoGame repo, string titleUrl)
        {
            var query = @"SELECT COUNT(1) FROM D_GAME AS dg WHERE dg.TitleUrl=@title_url";
            using (var connection = new SqlConnection(repo.ConnectionString))
            {
                var data = connection.Query<int>(query, new { title_url = titleUrl }).SingleOrDefault();
                return data == 1;
            }
        }

        public static VMGameMenu GetGameMenu(this RepoGame repo, int iw, int ih, int gnc, string gturl = null)
        {
            var queryForGames = @"SELECT *
FROM   D_GAME AS dg
WHERE  dg.Show = 1
       AND dg.FrontPictureId IS NOT NULL
ORDER BY dg.Title";

            var queryForNews = @"SELECT dm.ModelCoreType,
       dm.DateCreate,
       dm.DateOfPublication,
       dm.Title,
       dm.TitleUrl
FROM   DV_MATERIAL  AS dm
       JOIN (
                SELECT TOP(@amount) dn.Id,
                       dn.ModelCoreType
                FROM   D_NEWS            AS dn
                       JOIN DV_MATERIAL  AS dm2
                            ON  dm2.Id = dn.Id
                            AND dm2.ModelCoreType = dn.ModelCoreType
                       JOIN D_GAME  AS dg1
                            ON  dg1.Id = dn.GameId
                            AND (dg1.TitleUrl = @gturl OR @gturl IS NULL)
                   WHERE dm2.Show = 1
                AND dm2.DateOfPublication <= GETDATE()
                    ORDER BY
                    dm2.DateCreate DESC
                    UNION
                    SELECT TOP(@amount) da.Id,
                           da.ModelCoreType
                    FROM   D_ARTICLE AS da
                           JOIN DV_MATERIAL AS dm3
                                ON  dm3.Id = da.Id
                                AND dm3.ModelCoreType = da.ModelCoreType
                           JOIN D_GAME AS dg2
                                ON  dg2.Id = da.GameId
                                AND (dg2.TitleUrl = @gturl OR @gturl IS NULL)
                   WHERE dm3.Show = 1
                AND dm3.DateOfPublication <= GETDATE()
                    ORDER BY
                    dm3.DateCreate DESC
            )       AS x
            ON  x.Id = dm.Id
            AND x.ModelCoreType = dm.ModelCoreType";

            var model = new VMGameMenu(iw, ih);

            using (var connection = new SqlConnection(repo.ConnectionString))
            {
                model.Games = connection.Query<VMGame>(queryForGames).ToArray();
                model.Materials = connection.Query<VMImgGameMaterial>(queryForNews, new { amount= gnc, gturl = gturl }).ToArray();
            }

            return model;
        }
    }
}