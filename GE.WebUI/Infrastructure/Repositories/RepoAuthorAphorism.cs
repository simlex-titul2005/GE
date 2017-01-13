using Dapper;
using GE.WebUI.Models;
using GE.WebUI.ViewModels;
using SX.WebCore;
using SX.WebCore.DbModels;
using SX.WebCore.SxProviders;
using SX.WebCore.SxRepositories.Abstract;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            var gws = GetAuthorAphorismsWhereString(filter, out param);
            sb.Append(gws);

            var defaultOrder = new SxOrderItem { FieldName = "daa.Name", Direction = SortDirection.Asc };
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
        private static string GetAuthorAphorismsWhereString(SxFilter filter, out object param)
        {
            param = null;
            var query = new StringBuilder();
            query.Append(" WHERE (daa.Name LIKE '%'+@name+'%' OR @name IS NULL) ");
            query.Append(" AND (daa.Description LIKE '%'+@desc+'%' OR @desc IS NULL) ");

            string name = filter.WhereExpressionObject?.Name;
            string desc = filter.WhereExpressionObject?.Description;

            param = new
            {
                name = name,
                desc = desc
            };

            return query.ToString();
        }

        public override void Delete(AuthorAphorism model)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Execute("dbo.del_author_aphorism @authorId", new { authorId = model.Id });
            }
        }

        public async Task<AuthorAphorism> GetByTitleUrlAsync(string titleUrl)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var data = await connection.QueryAsync<AuthorAphorism, SxPicture, AuthorAphorism>("dbo.get_author_aphorisms_by_title_url @titleUrl", (a, p) => {
                    a.Picture = p;
                    return a;
                }, new { titleUrl = titleUrl }, splitOn: "Id");

                return data.SingleOrDefault();
            }
        }

        public async Task<AuthorAphorism> GetByNameAsync(string name)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var query = "SELECT TOP(2) * FROM D_AUTHOR_APHORISM AS daa WHERE daa.Name=@name";
                var data = await connection.QueryAsync<AuthorAphorism>(query, new { name });
                return data.SingleOrDefault();
            }
        }
    }
}
