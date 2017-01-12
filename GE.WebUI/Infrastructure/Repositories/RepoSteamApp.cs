using Dapper;
using GE.WebUI.Models;
using GE.WebUI.ViewModels;
using SX.WebCore;
using SX.WebCore.SxProviders;
using SX.WebCore.SxRepositories.Abstract;
using SX.WebCore.ViewModels;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace GE.WebUI.Infrastructure.Repositories
{
    public sealed class RepoSteamApp : SxDbRepository<int, SteamApp, VMSteamApp>
    {
        public override VMSteamApp[] Read(SxFilter filter)
        {
            var sb = new StringBuilder();
            sb.Append(SxQueryProvider.GetSelectString(new string[] { "dsa.*" }));
            sb.Append(" FROM D_STEAM_APP AS dsa ");

            object param = null;
            var gws = GetSteamAppsWhereString(filter, out param);
            sb.Append(gws);

            var defaultOrder = new SxOrderItem { FieldName = "Name", Direction = SortDirection.Asc };
            sb.Append(SxQueryProvider.GetOrderString(defaultOrder, filter.Order));

            //count
            var sbCount = new StringBuilder();
            sbCount.Append("SELECT COUNT(1) FROM D_STEAM_APP AS dsa ");
            sbCount.Append(gws);

            using (var connection = new SqlConnection(ConnectionString))
            {
                filter.PagerInfo.TotalItems = connection.Query<int>(sbCount.ToString(), param: param).SingleOrDefault();
                sb.AppendFormat(" OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", filter.PagerInfo.TotalItems <= filter.PagerInfo.PageSize ? 0 : filter.PagerInfo.SkipCount, filter.PagerInfo.PageSize);
                var data = connection.Query<VMSteamApp>(sb.ToString(), param: param).ToArray();
                return data;
            }
        }
        private static string GetSteamAppsWhereString(SxFilter filter, out object param)
        {
            param = null;
            var query = new StringBuilder();
            query.Append(" WHERE (dsa.[Name] LIKE '%'+@name+'%' OR @name IS NULL) ");
            query.Append(" AND (dsa.[Id] = @appId OR @appId IS NULL) ");

            var free = filter.AddintionalInfo?[0] == null ? (bool?)null : Convert.ToBoolean(filter.AddintionalInfo[0]);
            if (free.HasValue)
                query.Append(" AND (dsa.[Id] NOT IN (SELECT dgsa.SteamAppId FROM D_GAME_STEAM_APP AS dgsa)) ");

            var gameId = filter.AddintionalInfo?[1] == null ? (int?)null : Convert.ToInt32(filter.AddintionalInfo[1]);
            if (gameId.HasValue)
                query.Append(" AND (dsa.[Id] IN (SELECT dgsa.SteamAppId FROM D_GAME_STEAM_APP AS dgsa WHERE dgsa.GameId=@gameId)) ");

            param = new
            {
                name = (string)filter.WhereExpressionObject?.Name,
                appId = (int?)(filter.WhereExpressionObject?.Id == 0 ? null : filter.WhereExpressionObject?.Id),
                gameId = gameId
            };

            return query.ToString();
        }

        public override SteamApp Create(SteamApp model)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var data = connection.Query<SteamApp>("dbo.add_steam_app @appId, @appName", new { appId = model.Id, appName = model.Name });
                return data.SingleOrDefault();
            }
        }

        public async Task<SxVMResultMessage> LinkSteamAppsAsync(int gameId, int[] steamAppIds)
        {
            var result = new SxVMResultMessage("success", SxVMResultMessage.ResultMessageType.Ok);
            try
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    await connection.ExecuteAsync("dbo.link_steam_apps @gameId, @steamAppIds", new { gameId, steamAppIds = steamAppIds.Select(x => x.ToString()).ToDelimeterString(',') });
                }
            }
            catch (Exception ex)
            {
                result = new SxVMResultMessage(ex.Message, SxVMResultMessage.ResultMessageType.Error);
            }

            return result;
        }

        public async Task<SxVMResultMessage> UnlinkSteamAppsAsync(int gameId, int[] steamAppIds)
        {
            var result = new SxVMResultMessage("success", SxVMResultMessage.ResultMessageType.Ok);
            try
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    await connection.ExecuteAsync("dbo.unlink_steam_apps @gameId, @steamAppIds", new { gameId, steamAppIds = steamAppIds != null && steamAppIds.Any() ? steamAppIds.Select(x => x.ToString()).ToDelimeterString(',') : null });
                }
            }
            catch (Exception ex)
            {
                result = new SxVMResultMessage(ex.Message, SxVMResultMessage.ResultMessageType.Error);
            }

            return result;
        }
    }
}