using GE.WebCoreExtantions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Dapper;
using SX.WebCore.Abstract;
using GE.WebAdmin.Models;

namespace GE.WebAdmin.Extantions.Repositories
{
    public static partial class RepositoryExtantions
    {
        public static IEnumerable<ArticleType> GetArticleTypesByGameId(this GE.WebCoreExtantions.Repositories.RepoArticleType repo, int gameId)
        {
            using (var conn = new SqlConnection(repo.ConnectionString))
            {
                var result = conn.Query<ArticleType>(Resources.Sql_ArticleTypes.GetArticleTypesByGameId, new { GAME_ID = gameId });
                return result;
            }
        }

        public static IQueryable<VMArticleType> QueryForAdmin(this GE.WebCoreExtantions.Repositories.RepoArticleType repo, SxFilter filter)
        {
            var f = (Filter)filter;
            using (var conn = new SqlConnection(repo.ConnectionString))
            {
                var query = @"SELECT
	dat.Id,
	dat.Name,
	dat.[Description],
	dg.Title AS GameTitle
FROM D_ARTICLE_TYPE AS dat
JOIN D_GAME AS dg ON dg.Id = dat.GameId
ORDER BY dg.Title, dat.Name";
                if (f != null && f.SkipCount.HasValue && f.PageSize.HasValue)
                    query += " OFFSET " + filter.SkipCount + " ROWS FETCH NEXT " + filter.PageSize + " ROWS ONLY";

                var data = conn.Query<VMArticleType>(query);

                return data.AsQueryable();
            }
        }
    }
}