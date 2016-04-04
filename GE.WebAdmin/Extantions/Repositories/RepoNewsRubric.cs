using GE.WebCoreExtantions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Dapper;
using SX.WebCore.Abstract;
using GE.WebAdmin.Models;
using GE.WebCoreExtantions.Repositories;
using SX.WebCore.Providers;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace GE.WebAdmin.Extantions.Repositories
{
    public static partial class RepositoryExtantions
    {
        public static VMNewsRubric[] QueryForAdmin(this RepoNewsRubric repo, Filter filter)
        {
            var query = QueryProvider.GetSelectString();
            query += @" FROM D_NEWS_RUBRIC AS dnr ";

            object param = null;
            query += getNewsRubricWhereString(filter, out param);

            query += QueryProvider.GetOrderString("dnr.Id", SortDirection.Asc, filter.Orders);

            query += " OFFSET " + filter.PagerInfo.SkipCount + " ROWS FETCH NEXT " + filter.PagerInfo.PageSize + " ROWS ONLY";

            using (var conn = new SqlConnection(repo.ConnectionString))
            {
                var data = conn.Query<VMNewsRubric>(query, param: param);
                return data.ToArray();
            }
        }

        public static int FilterCount(this RepoNewsRubric repo, Filter filter)
        {
            var query = @"SELECT COUNT(1) FROM D_NEWS_RUBRIC AS dnr ";

            object param = null;
            query += getNewsRubricWhereString(filter, out param);

            using (var conn = new SqlConnection(repo.ConnectionString))
            {
                var data = conn.Query<int>(query, param: param).Single();
                return data;
            }
        }

        private static string getNewsRubricWhereString(Filter filter, out object param)
        {
            param = null;
            string query = null;
            query += " WHERE (dnr.Description LIKE '%'+@desc+'%' OR @desc IS NULL)";

            var desc = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Description != null ? (string)filter.WhereExpressionObject.Description : null;

            param = new
            {
                desc = desc
            };

            return query;
        }
    }
}