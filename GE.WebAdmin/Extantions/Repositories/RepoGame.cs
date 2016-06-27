using GE.WebAdmin.Models;
using SX.WebCore.Providers;
using System.Data.SqlClient;
using System.Linq;
using static SX.WebCore.HtmlHelpers.SxExtantions;
using Dapper;
using GE.WebCoreExtantions.Repositories;
using SX.WebCore;

namespace GE.WebAdmin.Extantions.Repositories
{
    public static partial class RepositoryExtantions
    {
        public static VMGame[] QueryForAdmin(this RepoGame repo, SxFilter filter)
        {
            var query = SxQueryProvider.GetSelectString(new string[] { "dg.Id", "dg.Title", "dg.TitleAbbr", "dg.[Description]", "dg.Show" });
            query += " FROM D_GAME AS dg ";

            object param = null;
            query += getGameWhereString(filter, out param);

            var defaultOrder = new SxOrder { FieldName = "dg.Title", Direction = SortDirection.Asc };
            query += SxQueryProvider.GetOrderString(defaultOrder, filter.Order);

            query += " OFFSET " + filter.PagerInfo.SkipCount + " ROWS FETCH NEXT " + filter.PagerInfo.PageSize + " ROWS ONLY";

            using (var conn = new SqlConnection(repo.ConnectionString))
            {
                var data = conn.Query<VMGame>(query, param: param);
                return data.ToArray();
            }
        }

        public static int FilterCount(this RepoGame repo, SxFilter filter)
        {
            var query = @"SELECT COUNT(1) FROM D_GAME AS dg";

            object param = null;
            query += getGameWhereString(filter, out param);

            using (var conn = new SqlConnection(repo.ConnectionString))
            {
                var data = conn.Query<int>(query, param: param).Single();
                return data;
            }
        }

        private static string getGameWhereString(SxFilter filter, out object param)
        {
            param = null;
            string query = null;
            query += " WHERE (dg.Title LIKE '%'+@title+'%' OR @title IS NULL) ";
            query += " AND (dg.TitleAbbr LIKE '%'+@title_abbr+'%' OR @title_abbr IS NULL) ";
            query += " AND (dg.[Description] LIKE '%'+@desc+'%' OR @desc IS NULL) ";

            var title = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Title != null ? (string)filter.WhereExpressionObject.Title : null;
            var title_abbr = filter.WhereExpressionObject != null && filter.WhereExpressionObject.TitleAbbr != null ? (string)filter.WhereExpressionObject.TitleAbbr : null;
            var desc = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Description != null ? (string)filter.WhereExpressionObject.Description : null;

            param = new
            {
                title = title,
                title_abbr = title_abbr,
                desc = desc
            };

            return query;
        }
    }
}