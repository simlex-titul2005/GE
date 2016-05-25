using Dapper;
using SX.WebCore;
using SX.WebCore.Abstract;
using SX.WebCore.Providers;
using System.Data.SqlClient;
using System.Linq;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace GE.WebCoreExtantions.Repositories
{
    public sealed class RepoAphorism : SxDbRepository<int, Aphorism, DbContext>
    {

        public override IQueryable<Aphorism> Query(SxFilter filter)
        {
            var f = (Filter)filter;
            var query = QueryProvider.GetSelectString(new string[] { "dm.*", "dmc.Id", "dmc.Title", "da.AuthorId", "daa.Id", "daa.Name" });
            query += @" FROM D_APHORISM AS da
JOIN DV_MATERIAL AS dm ON dm.Id = da.Id AND dm.ModelCoreType = da.ModelCoreType
JOIN D_MATERIAL_CATEGORY AS dmc ON dmc.Id = dm.CategoryId
LEFT JOIN D_AUTHOR_APHORISM AS daa ON daa.Id = da.AuthorId";

            object param = null;
            query += getAphorismsWhereString(f, out param);

            query += QueryProvider.GetOrderString("dm.DateCreate", SortDirection.Desc, filter.Orders);

            query += " OFFSET " + filter.PagerInfo.SkipCount + " ROWS FETCH NEXT " + filter.PagerInfo.PageSize + " ROWS ONLY";

            using (var conn = new SqlConnection(base.ConnectionString))
            {
                var data = conn.Query<Aphorism, SxMaterialCategory, AuthorAphorism, Aphorism>(query, (a, c, aa) =>
                {
                    a.CategoryId = c.Id;
                    a.AuthorId = aa.Id;
                    a.Category = c;
                    a.Author = aa;
                    return a;
                }, param: param, splitOn: "CategoryId, AuthorId");

                return data.AsQueryable();
            }
        }

        public override int Count(SxFilter filter)
        {
            var f = (Filter)filter;
            var query = @"SELECT COUNT(1) FROM D_APHORISM AS da
JOIN DV_MATERIAL AS dm ON dm.Id = da.Id AND dm.ModelCoreType = da.ModelCoreType
JOIN D_MATERIAL_CATEGORY AS dmc ON dmc.Id = dm.CategoryId
LEFT JOIN D_AUTHOR_APHORISM AS daa ON daa.Id = da.AuthorId";

            object param = null;
            query += getAphorismsWhereString(f, out param);

            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<int>(query, param: param).Single();
                return data;
            }
        }

        private static string getAphorismsWhereString(Filter filter, out object param)
        {
            param = null;
            string query = null;
            query += " WHERE (dm.CategoryId=@cid OR @cid IS NULL) ";
            query += " AND (dm.Title LIKE '%'+@title+'%' OR @title IS NULL) ";
            query += " AND ((@author IS NOT NULL AND @author NOT IN('no') AND daa.Name LIKE '%'+@author+'%') OR (@author IS NOT NULL AND @author IN('no') AND daa.Name IS NULL) OR (@author IS NULL)) ";
            query += " AND (dm.Html LIKE '%'+@html+'%' OR @html IS NULL) ";
            if(filter.OnlyShow)
                query += " AND (dm.Show=1) ";

            var cid = filter.WhereExpressionObject != null && filter.WhereExpressionObject.CategoryId != null ? (string)filter.WhereExpressionObject.CategoryId : null;
            var title = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Title != null ? (string)filter.WhereExpressionObject.Title : null;
            var author = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Author != null ? (string)filter.WhereExpressionObject.Author.Name : null;
            var html = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Html != null ? (string)filter.WhereExpressionObject.Html : null;

            param = new
            {
                cid = cid,
                title = title,
                author = author,
                html = html
            };

            return query;
        }

        public Aphorism GetRandom(int? id=null)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<Aphorism, SxMaterialCategory, AuthorAphorism, Aphorism>("get_random_aphorism @mid", (a, c, aa)=> {
                    a.Author = aa;
                    a.Category = c;
                    return a;
                }, new { mid=id}).SingleOrDefault();
                return data;
            }
        }
    }
}
