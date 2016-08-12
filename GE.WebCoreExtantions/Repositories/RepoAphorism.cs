using Dapper;
using SX.WebCore;
using SX.WebCore.Providers;
using SX.WebCore.Repositories;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using static SX.WebCore.Enums;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace GE.WebCoreExtantions.Repositories
{
    public sealed class RepoAphorism : SxRepoMaterial<Aphorism, DbContext>
    {
        public RepoAphorism() : base(ModelCoreType.Aphorism) { }

        public override Aphorism[] Read(SxFilter filter)
        {
            var sb = new StringBuilder();
            sb.Append(SxQueryProvider.GetSelectString(new string[] {
                "dbo.get_comments_count(dm.Id, dm.ModelCoreType) AS CommentsCount",
                "dm.*",
                "dmc.Id",
                "dmc.Title",
                "da.AuthorId",
                "daa.Id",
                "daa.Name"
            }));
            sb.Append(@" FROM D_APHORISM AS da ");
            var joinString = @" JOIN DV_MATERIAL AS dm ON dm.Id = da.Id AND dm.ModelCoreType = da.ModelCoreType
JOIN D_MATERIAL_CATEGORY AS dmc ON dmc.Id = dm.CategoryId
LEFT JOIN D_AUTHOR_APHORISM AS daa ON daa.Id = da.AuthorId ";
            sb.Append(joinString);

            object param = null;
            var gws = getAphorismsWhereString(filter, out param);
            sb.Append(gws);

            var defaultOrder = new SxOrder { FieldName = "dm.DateCreate", Direction = SortDirection.Desc };
            sb.Append(SxQueryProvider.GetOrderString(defaultOrder, filter.Order));

            sb.AppendFormat(" OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", filter.PagerInfo.SkipCount, filter.PagerInfo.PageSize);

            //count
            var sbCount = new StringBuilder();
            sbCount.Append(@"SELECT COUNT(1) FROM D_APHORISM AS da ");
            sbCount.Append(joinString);
            sbCount.Append(gws);

            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<Aphorism, SxMaterialCategory, AuthorAphorism, Aphorism>(sb.ToString(), (a, c, aa) =>
                {
                    a.CategoryId = c.Id;
                    a.AuthorId = aa.Id;
                    a.Category = c;
                    a.Author = aa;
                    return a;
                }, param: param, splitOn: "CategoryId, AuthorId");
                filter.PagerInfo.TotalItems = conn.Query<int>(sbCount.ToString(), param: param).SingleOrDefault();
                return data.ToArray();
            }
        }
        private static string getAphorismsWhereString(SxFilter filter, out object param)
        {
            param = null;
            var query = new StringBuilder();
            query.Append(" WHERE (dm.CategoryId=@cid OR @cid IS NULL) ");
            query.Append(" AND (dm.Title LIKE '%'+@title+'%' OR @title IS NULL) ");
            query.Append(" AND (daa.TitleUrl=@titleUrl OR @titleUrl IS NULL) ");
            query.Append(" AND (dm.Html LIKE '%'+@html+'%' OR @html IS NULL) ");
            if (filter.OnlyShow==true)
                query.Append(" AND (dm.Show=1) ");

            var cid = filter.WhereExpressionObject != null && filter.WhereExpressionObject.CategoryId != null ? (string)filter.WhereExpressionObject.CategoryId : null;
            var title = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Title != null ? (string)filter.WhereExpressionObject.Title : null;
            var titleUrl = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Author != null ? (string)filter.WhereExpressionObject.Author.TitleUrl : null;
            var html = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Html != null ? (string)filter.WhereExpressionObject.Html : null;

            param = new
            {
                cid = cid,
                title = title,
                titleUrl = titleUrl,
                html = html
            };

            return query.ToString();
        }

        public Aphorism GetRandom(int? id = null)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<Aphorism, SxMaterialCategory, AuthorAphorism, Aphorism>("dbo.get_random_aphorism @mid", (a, c, aa) =>
                {
                    a.Author = aa;
                    a.Category = c;
                    return a;
                }, new { mid = id }).SingleOrDefault();
                return data;
            }
        }

        public override void Delete(Aphorism model)
        {
            var query = "DELETE FROM D_APHORISM WHERE Id=@mid AND ModelCoreType=@mct";
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Execute(query, new { mid = model.Id, mct = model.ModelCoreType });
            }

            base.Delete(model);
        }
    }
}
