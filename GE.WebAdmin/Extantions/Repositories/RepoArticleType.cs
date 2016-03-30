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
        public static IEnumerable<ArticleType> GetArticleTypesByGameId(this RepoArticleType repo, int gameId)
        {
            var query = @"SELECT*FROM D_ARTICLE_TYPE AS dat WHERE dat.GameId=@game_id
ORDER BY dat.[DESCRIPTION]";
            using (var conn = new SqlConnection(repo.ConnectionString))
            {
                var result = conn.Query<ArticleType>(query, new { game_id = gameId });
                return result;
            }
        }

        public static VMArticleType[] QueryForAdmin(this RepoArticleType repo, Filter filter)
        {
            var query = QueryProvider.GetSelectString(new string[] {
                    "dat.Id", "dat.Name", "dat.[Description]", "dat.GameId", "dg.Title AS GameTitle"
                });
            query += @" FROM D_ARTICLE_TYPE AS dat JOIN D_GAME AS dg ON dg.Id = dat.GameId";

            object param = null;
            query += getArticleTypeWhereString(filter, out param);

            query += QueryProvider.GetOrderString("dat.Name", SortDirection.Desc, filter.Orders);

            if(filter != null && filter.SkipCount.HasValue && filter.PageSize.HasValue)
                query += " OFFSET " + filter.SkipCount + " ROWS FETCH NEXT " + filter.PageSize + " ROWS ONLY";

            using (var conn = new SqlConnection(repo.ConnectionString))
            {
                var data = conn.Query<VMArticleType>(query, param: param);
                return data.ToArray();
            }
        }

        public static int FilterCount(this RepoArticleType repo, Filter filter)
        {
            var query = @"SELECT COUNT(1) FROM D_ARTICLE_TYPE AS dat JOIN D_GAME AS dg ON dg.Id = dat.GameId";

            object param = null;
            query += getArticleTypeWhereString(filter, out param);

            using (var conn = new SqlConnection(repo.ConnectionString))
            {
                var data = conn.Query<int>(query, param: param).Single();
                return data;
            }
        }

        private static string getArticleTypeWhereString(Filter filter, out object param)
        {
            param = null;
            string query = null;
            query += " WHERE (dg.Title LIKE '%'+@game_title+'%' OR @game_title IS NULL)";
            query += " AND (dat.Name LIKE '%'+@name+'%' OR @name IS NULL)";
            query += " AND (dat.[Description] LIKE '%'+@desc+'%' OR @desc IS NULL) ";

            var gameTitle = filter.WhereExpressionObject != null && filter.WhereExpressionObject.GameTitle != null ? (string)filter.WhereExpressionObject.GameTitle : null;
            var name = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Name != null ? (string)filter.WhereExpressionObject.Name : null;
            var desc = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Description != null ? (string)filter.WhereExpressionObject.Description : null;

            param = new
            {
                game_title = gameTitle,
                name = name,
                desc = desc
            };

            return query;
        }
    }
}