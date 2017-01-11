using Dapper;
using GE.WebUI.Models;
using GE.WebUI.ViewModels;
using SX.WebCore;
using SX.WebCore.SxProviders;
using SX.WebCore.SxRepositories.Abstract;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace GE.WebUI.Infrastructure.Repositories
{
    public sealed class RepoSteamApp : SxDbRepository<int, SteamApp, VMSteamApp>
    {
        public override VMSteamApp[] Read(SxFilter filter)
        {
            var sb = new StringBuilder();
            sb.Append(SxQueryProvider.GetSelectString());
            sb.Append(" FROM D_STEAM_APP AS dsa ");

            object param = null;
            var gws = GetSteamAppsWhereString(filter, out param);
            sb.Append(gws);

            var defaultOrder = new SxOrderItem { FieldName = "Name", Direction = SortDirection.Asc };
            sb.Append(SxQueryProvider.GetOrderString(defaultOrder, filter.Order));

            sb.AppendFormat(" OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", filter.PagerInfo.SkipCount, filter.PagerInfo.PageSize);

            //count
            var sbCount = new StringBuilder();
            sbCount.Append("SELECT COUNT(1) FROM D_STEAM_APP AS dsa ");
            sbCount.Append(gws);

            using (var connection = new SqlConnection(ConnectionString))
            {
                filter.PagerInfo.TotalItems = connection.Query<int>(sbCount.ToString(), param: param).SingleOrDefault();
                var data = connection.Query<VMSteamApp>(sb.ToString(), param: param);
                return data.ToArray();
            }
        }
        private static string GetSteamAppsWhereString(SxFilter filter, out object param)
        {
            param = null;
            var query = new StringBuilder();
            query.Append(" WHERE (dsa.[Name] LIKE '%'+@name+'%' OR @name IS NULL) ");
            query.Append(" AND (dsa.[AppId] = @appId OR @appId IS NULL) ");

            param = new
            {
                name = (string)filter.WhereExpressionObject?.Name,
                appId = filter.WhereExpressionObject?.AppId
            };

            return query.ToString();
        }

        public override SteamApp Create(SteamApp model)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var data = connection.Query<SteamApp>("dbo.add_steam_app @appId, @appName", new { appId=model.AppId, appName=model.Name });
                return data.SingleOrDefault();
            }
        }
    }
}