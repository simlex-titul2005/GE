using GE.WebUI.Models;
using GE.WebUI.ViewModels;
using System.Data.SqlClient;
using Dapper;
using System.Linq;
using GE.WebUI.ViewModels.Abstracts;
using SX.WebCore.ViewModels;
using SX.WebCore;
using SX.WebCore.Repositories.Abstract;

namespace GE.WebUI.Infrastructure.Repositories
{
    public sealed class RepoGame : SxDbRepository<int, Game, VMGame>
    {
        //        public override IQueryable<Game> All
        //        {
        //            get
        //            {
        //                using (var conn = new SqlConnection(base.ConnectionString))
        //                {
        //                    var result = conn.Query<Game>(@"SELECT *
        //FROM   D_GAME                  dg
        //ORDER BY
        //       dg.[TITLE]");
        //                    return result.AsQueryable();
        //                }
        //            }
        //        }

        public bool ExistGame(string titleUrl)
        {
            var query = @"SELECT COUNT(1) FROM D_GAME AS dg WHERE dg.TitleUrl=@title_url";
            using (var connection = new SqlConnection(ConnectionString))
            {
                var data = connection.Query<int>(query, new { title_url = titleUrl }).SingleOrDefault();
                return data == 1;
            }
        }

        public VMGameMenu GetGameMenu(int iw, int ih, int gnc, string gturl = null)
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
                       JOIN D_GAME       AS dg1
                            ON  dg1.Id = dn.GameId
                            AND (dg1.TitleUrl = @gturl OR @gturl IS NULL)
                WHERE  dm2.Show = 1
                       AND dm2.DateOfPublication <= GETDATE()
                ORDER BY
                       dm2.DateCreate DESC
                UNION
                SELECT TOP(@amount) da.Id,
                       da.ModelCoreType
                FROM   D_ARTICLE         AS da
                       JOIN DV_MATERIAL  AS dm3
                            ON  dm3.Id = da.Id
                            AND dm3.ModelCoreType = da.ModelCoreType
                       JOIN D_GAME       AS dg2
                            ON  dg2.Id = da.GameId
                            AND (dg2.TitleUrl = @gturl OR @gturl IS NULL)
                WHERE  dm3.Show = 1
                       AND dm3.DateOfPublication <= GETDATE()
                ORDER BY
                       dm3.DateCreate DESC
            )       AS x
            ON  x.Id = dm.Id
            AND x.ModelCoreType = dm.ModelCoreType
ORDER BY
       dm.DateCreate DESC";

            var model = new VMGameMenu(iw, ih);

            using (var connection = new SqlConnection(ConnectionString))
            {
                model.Games = connection.Query<VMGameMenuGame>(queryForGames).ToArray();
                model.Materials = connection.Query<VMGameMenuImgGameMaterial>(queryForNews, new { amount = gnc, gturl = gturl }).ToArray();
            }

            return model;
        }

        public VMDetailGame GetGameDetails(string titleUrl, int amount = 10)
        {
            var viewModel = new VMDetailGame();
            using (var conn = new SqlConnection(ConnectionString))
            {
                viewModel = conn.Query<VMDetailGame>("get_game_by_url @titleUrl", new { titleUrl = titleUrl }).SingleOrDefault();
                if (viewModel == null) return null;

                viewModel.Materials = conn.Query<VMMaterial>("get_game_materials @titleUrl, @amount", new { titleUrl = titleUrl, amount = amount }).ToArray();
                viewModel.Videos = conn.Query<SxVMVideo>("get_game_videos @titleUrl", new { titleUrl = titleUrl }).ToArray();
                if (viewModel.Videos.Any())
                {
                    var html = viewModel.FullDescription;
                    SxBBCodeParser.ReplaceVideo(ref html, viewModel.Videos);
                    viewModel.FullDescription = html;
                }
                if (viewModel.Materials.Any())
                {
                    for (int i = 0; i < viewModel.Materials.Length; i++)
                    {
                        var material = viewModel.Materials[i];
                        material.Videos = conn.Query<SxVMVideo>("get_material_videos @mid, @mct", new { mid = material.Id, mct = material.ModelCoreType }).ToArray();
                    }
                }

                return viewModel;
            }
        }
    }
}
