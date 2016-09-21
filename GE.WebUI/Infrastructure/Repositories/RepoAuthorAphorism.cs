using Dapper;
using GE.WebUI.Models;
using GE.WebUI.ViewModels;
using SX.WebCore;
using SX.WebCore.Providers;
using SX.WebCore.Repositories.Abstract;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace GE.WebUI.Infrastructure.Repositories
{
    public sealed class RepoAuthorAphorism : SxDbRepository<int, AuthorAphorism, VMAuthorAphorism>
    {
        public override VMAuthorAphorism[] Read(SxFilter filter)
        {
            var sb = new StringBuilder();
            sb.Append(SxQueryProvider.GetSelectString());
            sb.Append(@" FROM D_AUTHOR_APHORISM AS daa ");

            object param = null;
            var gws = getAuthorAphorismsWhereString(filter, out param);
            sb.Append(gws);

            var defaultOrder = new SxOrder { FieldName = "daa.Name", Direction = SortDirection.Asc };
            sb.Append(SxQueryProvider.GetOrderString(defaultOrder, filter.Order));

            sb.AppendFormat(" OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", filter.PagerInfo.SkipCount, filter.PagerInfo.PageSize);

            //count
            var sbCount = new StringBuilder();
            sbCount.Append(@"SELECT COUNT(1) FROM D_AUTHOR_APHORISM AS daa ");
            sbCount.Append(gws);

            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<VMAuthorAphorism>(sb.ToString(), param: param);
                filter.PagerInfo.TotalItems = conn.Query<int>(sbCount.ToString(), param: param).SingleOrDefault();
                return data.ToArray();
            }
        }

        public override void Delete(AuthorAphorism model)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Execute("dbo.del_author_aphorism @authorId", new { authorId = model.Id });
            }
        }

        private static string getAuthorAphorismsWhereString(SxFilter filter, out object param)
        {
            param = null;
            string query = null;
            query += " WHERE (daa.Name LIKE '%'+@name+'%' OR @name IS NULL) ";
            query += " AND (daa.Description LIKE '%'+@desc+'%' OR @desc IS NULL) ";

            var name = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Name != null ? (string)filter.WhereExpressionObject.Name : null;
            var desc = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Description != null ? (string)filter.WhereExpressionObject.Description : null;

            param = new
            {
                name = name,
                desc = desc
            };

            return query;
        }

        public AuthorAphorism GetByTitleUrl(string titleUrl)
        {
            var query = @"SELECT*FROM D_AUTHOR_APHORISM AS daa
LEFT JOIN D_PICTURE AS dp ON dp.Id = daa.PictureId
WHERE daa.TitleUrl=@titleUrl";

            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<AuthorAphorism, SxPicture, AuthorAphorism>(query, (a, p) => {
                    a.Picture = p;
                    return a;
                }, new { titleUrl = titleUrl }, splitOn: "Id").SingleOrDefault();

                return data;
            }
        }
    }
}
