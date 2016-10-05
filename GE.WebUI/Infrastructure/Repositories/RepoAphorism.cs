using Dapper;
using GE.WebUI.Models;
using GE.WebUI.ViewModels;
using SX.WebCore;
using SX.WebCore.Providers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using static SX.WebCore.Enums;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace GE.WebUI.Infrastructure.Repositories
{
    public sealed class RepoAphorism : RepoMaterial<Aphorism, VMAphorism>
    {
        public RepoAphorism() : base(ModelCoreType.Aphorism, new Dictionary<string, object> { ["OnlyShow"] = false, ["WithComments"] = false }) { }

        public override VMAphorism[] Read(SxFilter filter)
        {
            var sb = new StringBuilder();
            sb.Append(SxQueryProvider.GetSelectString(new string[] {
                filter.WithComments.HasValue && filter.WithComments==true?"dbo.get_comments_count(dm.Id, dm.ModelCoreType) AS CommentsCount":null,
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
            sb.Append(SxQueryProvider.GetOrderString(defaultOrder, filter.Order, new Dictionary<string, string> {
                ["Title"]="dm.Title",
                ["AuthorName"]= "daa.Name"
            }));

            sb.AppendFormat(" OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", filter.PagerInfo.SkipCount, filter.PagerInfo.PageSize);

            //count
            var sbCount = new StringBuilder();
            sbCount.Append(@"SELECT COUNT(1) FROM D_APHORISM AS da ");
            sbCount.Append(joinString);
            sbCount.Append(gws);

            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<VMAphorism,VMMaterialCategory, VMAuthorAphorism, VMAphorism>(sb.ToString(), (a, c, aa) =>
                {
                    a.Category = c;
                    a.Author = aa;
                    a.AuthorName = aa?.Name;
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
            query.Append(" AND (dm.Show=@show OR @show IS NULL) ");

            string cid = filter.WhereExpressionObject?.CategoryId;
            string title = filter.WhereExpressionObject?.Title;
            string titleUrl = filter.WhereExpressionObject?.TitleUrl;
            string html = filter.WhereExpressionObject?.Html;

            param = new
            {
                cid = cid,
                title = title,
                titleUrl = titleUrl,
                html = html,
                show = filter.OnlyShow.HasValue ? filter.OnlyShow : (bool?)null
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

        public VMDetailAphorism GetByTitleUrl(string categoryId, string titleUrl, int tfaAmount = 10, int tcAmount = 10)
        {
            var viewModel = new VMDetailAphorism();
            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<VMAphorism, VMMaterialCategory, VMAuthorAphorism, VMAphorism>("dbo.get_aphorism_page_model @title_url, @author_amount, @cat_amount", (a, c, au) => {
                    a.CategoryId = c.Id.ToString();
                    a.Category = c;
                    a.Author = au;
                    return a;
                }, new { title_url = titleUrl, author_amount = tfaAmount, cat_amount = tcAmount }, splitOn: "Id").ToArray();

                if (data == null)
                    viewModel = null;
                else
                {

                    var model = data.SingleOrDefault(x => x.Flag == VMAphorism.AphorismFlag.ForThis);
                    viewModel.Aphorism = model;
                    viewModel.TopForAuthor = data.Where(x => x.Flag == VMAphorism.AphorismFlag.ForAuthor && x.Author != null).ToArray();
                    viewModel.TopForCategory = data.Where(x => x.Flag == VMAphorism.AphorismFlag.ForCategory).ToArray();
                }

                return viewModel;
            }


        }

        public VMAphorism GetExistsModel(string titleUrl)
        {
            var query = "SELECT TOP(1) * FROM D_APHORISM AS da WHERE da.TitleUrl=@titleUrl";
            using (var connection = new SqlConnection(ConnectionString))
            {
                var data = connection.Query<VMAphorism>(query, new { titleUrl = titleUrl });
                return data.SingleOrDefault();
            }
        }

        public VMAphorismCategory[] GetAphorismCategories(string cur = null)
        {
            dynamic[] data;
            using (var conn = new SqlConnection(ConnectionString))
            {
                data = conn.Query("dbo.get_aphorism_categories @cur", new { cur = cur }).ToArray();
            }

            var result = data.Where(x => x.Ref == null).OrderBy(x => x.Title).Select(x => new VMAphorismCategory { Id = x.Id, Title = x.Title }).ToArray();

            VMAphorismCategory category;
            for (int i = 0; i < result.Length; i++)
            {
                category = result[i];
                category.Authors = data.Where(x => Equals(x.Ref, category.Id)).OrderBy(x => x.Title).Select(x => new VMAphorismCategory { Id = x.Id, Title = x.Title }).ToArray();
            }

            return result;
        }

        protected override Action<SqlConnection, Aphorism> ChangeMaterialBeforeSelect
        {
            get
            {
                return (connection, model) => {
                    var query = "SELECT TOP(1)  daa.* FROM D_AUTHOR_APHORISM AS daa JOIN D_APHORISM AS da ON da.AuthorId = daa.Id WHERE da.Id = @id";
                    var data = connection.Query<AuthorAphorism>(query, new { id=model.Id }).SingleOrDefault();
                    model.Author = data;
                    model.AuthorId = data?.Id;
                };
            }
        }
    }
}
