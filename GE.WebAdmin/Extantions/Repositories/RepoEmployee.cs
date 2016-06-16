using GE.WebAdmin.Models;
using GE.WebCoreExtantions;
using SX.WebCore.Providers;
using System.Data.SqlClient;
using System.Linq;
using static SX.WebCore.HtmlHelpers.SxExtantions;
using Dapper;
using SX.WebCore.Repositories;

namespace GE.WebAdmin.Extantions.Repositories
{
    public static partial class RepositoryExtantions
    {
        public static VMEmployee[] QueryForAdmin(this SxRepoEmployee<DbContext> repo, Filter filter)
        {
            var query = SxQueryProvider.GetSelectString();
            query += @" FROM D_EMPLOYEE AS de
JOIN AspNetUsers AS anu ON anu.Id = de.Id ";

            object param = null;
            query += getEmployeeWhereString(filter, out param);

            query += SxQueryProvider.GetOrderString("de.DateCreate", SortDirection.Asc, filter.Orders);

            query += " OFFSET " + filter.PagerInfo.SkipCount + " ROWS FETCH NEXT " + filter.PagerInfo.PageSize + " ROWS ONLY";

            using (var conn = new SqlConnection(repo.ConnectionString))
            {
                var data = conn.Query<VMEmployee, VMUser, VMEmployee>(query, (e, u)=> {
                    e.User = u;
                    return e;
                }, param: param, splitOn:"Id");
                return data.ToArray();
            }
        }

        public static int FilterCount(this SxRepoEmployee<DbContext> repo, Filter filter)
        {
            var query = @"SELECT COUNT(1) FROM D_EMPLOYEE AS de";

            object param = null;
            query += getEmployeeWhereString(filter, out param);

            using (var conn = new SqlConnection(repo.ConnectionString))
            {
                var data = conn.Query<int>(query, param: param).Single();
                return data;
            }
        }

        private static string getEmployeeWhereString(Filter filter, out object param)
        {
            param = null;
            string query = null;
            //query += " WHERE (dg.Title LIKE '%'+@title+'%' OR @title IS NULL) ";
            //query += " AND (dg.TitleAbbr LIKE '%'+@title_abbr+'%' OR @title_abbr IS NULL) ";
            //query += " AND (dg.[Description] LIKE '%'+@desc+'%' OR @desc IS NULL) ";

            //var title = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Title != null ? (string)filter.WhereExpressionObject.Title : null;
            //var title_abbr = filter.WhereExpressionObject != null && filter.WhereExpressionObject.TitleAbbr != null ? (string)filter.WhereExpressionObject.TitleAbbr : null;
            //var desc = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Description != null ? (string)filter.WhereExpressionObject.Description : null;

            //param = new
            //{
            //    title = title,
            //    title_abbr = title_abbr,
            //    desc = desc
            //};

            return query;
        }
    }
}