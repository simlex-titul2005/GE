using GE.WebUI.Models;
using GE.WebUI.ViewModels;
using System.Data.SqlClient;
using Dapper;
using System.Linq;
using GE.WebUI.ViewModels.Abstracts;
using SX.WebCore.ViewModels;
using SX.WebCore;
using SX.WebCore.Repositories.Abstract;
using System.Text;
using SX.WebCore.Providers;
using static SX.WebCore.HtmlHelpers.SxExtantions;
using SX.WebCore.Repositories;
using System.Data;

namespace GE.WebUI.Infrastructure.Repositories
{
    public sealed class RepoGame : SxDbRepository<int, Game, VMGame>
    {
        public override VMGame[] Read(SxFilter filter)
        {
            var sb = new StringBuilder();
            sb.Append(SxQueryProvider.GetSelectString());
            sb.Append(" FROM D_GAME AS dg ");

            object param = null;
            var gws = getGamesWhereString(filter, out param);
            sb.Append(gws);

            var defaultOrder = new SxOrder { FieldName = "dg.Title", Direction = SortDirection.Asc };
            sb.Append(SxQueryProvider.GetOrderString(defaultOrder, filter.Order));

            sb.AppendFormat(" OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", filter.PagerInfo.SkipCount, filter.PagerInfo.PageSize);

            //count
            var sbCount = new StringBuilder();
            sbCount.Append("SELECT COUNT(1) FROM D_GAME AS dg ");
            sbCount.Append(gws);

            using (var connection = new SqlConnection(ConnectionString))
            {

                filter.PagerInfo.TotalItems = connection.Query<int>(sbCount.ToString(), param: param).SingleOrDefault();
                var data= connection.Query<VMGame>(sb.ToString(), param: param);
                return data.ToArray();
            }
        }
        private static string getGamesWhereString(SxFilter filter, out object param)
        {
            param = null;
            var query = new StringBuilder();
            query.Append(" WHERE (dg.Title LIKE '%'+@title+'%' OR @title IS NULL) ");
            query.Append(" AND (dg.TitleAbbr LIKE '%'+@titleAbbr+'%' OR @titleAbbr IS NULL) ");
            query.Append(" AND (dg.Description LIKE '%'+@desc+'%' OR @desc IS NULL) ");

            string title = filter.WhereExpressionObject?.Title;
            string titleAbbr = filter.WhereExpressionObject?.TitleAbbr;
            string desc = filter.WhereExpressionObject?.Description;

            param = new
            {
                title = title,
                titleAbbr = titleAbbr,
                desc = desc
            };

            return query.ToString();
        }

        public sealed override Game GetByKey(params object[] id)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var data = connection.Query<Game, SxPicture, SxPicture, SxPicture, Game>("dbo.get_game @id", (g, p1, p2, p3)=> {
                    g.FrontPicture = p1;
                    g.GoodPicture = p2;
                    g.BadPicture = p3;
                    return g;
                }, new { id = id[0] }, splitOn:"Id");
                return data.SingleOrDefault();
            }
        }

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
            using (var connection = new SqlConnection(ConnectionString))
            {
                viewModel = connection.Query<VMDetailGame>("get_game_by_url @titleUrl", new { titleUrl = titleUrl }).SingleOrDefault();
                if (viewModel == null) return null;

                viewModel.Materials = connection.Query<VMMaterial>("get_game_materials @titleUrl, @amount", new { titleUrl = titleUrl, amount = amount }).ToArray();
                viewModel.Videos = connection.Query<SxVMVideo>("get_game_videos @titleUrl", new { titleUrl = titleUrl }).ToArray();
                if (viewModel.Videos.Any())
                {
                    var html = viewModel.FullDescription;
                    SxBBCodeParser.ReplaceVideo(ref html, viewModel.Videos);
                    viewModel.FullDescription = html;
                }

                if (viewModel.Materials.Any())
                    fillMaterialsVideo(connection, viewModel.Materials);

                return viewModel;
            }
        }

        private static void fillMaterialsVideo(SqlConnection connection, VMMaterial[] materials)
        {
            VMMaterial item = null;

            var table = new DataTable();
            table.Columns.Add("Materialid", typeof(int));
            table.Columns.Add("ModelCoreType", typeof(int));

            for (int i = 0; i < materials.Length; i++)
            {
                item = materials[i];
                table.Rows.Add(item.Id, (int)item.ModelCoreType);
            }

            var videoLinks = connection.Query<SxVideoLink, SxVideo, SxVideoLink>("dbo.get_materials_videos", (vl, v) =>
            {
                vl.Video = v;
                return vl;
            }, new { materials = table.AsTableValuedParameter("dbo.ForSelectMaterialVideos") }, commandType: CommandType.StoredProcedure, splitOn: "Id").ToArray();

            for (int i = 0; i < materials.Length; i++)
            {
                item = materials[i];
                item.Videos = videoLinks.Where(x => Equals(x.MaterialId, item.Id) && x.ModelCoreType == item.ModelCoreType).Select(x => Mapper.Map<SxVideo, SxVMVideo>(x.Video)).ToArray();
            }
        }
    }
}
