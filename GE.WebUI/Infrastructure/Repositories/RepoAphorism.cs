using Dapper;
using GE.WebUI.Models;
using GE.WebUI.ViewModels;
using SX.WebCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace GE.WebUI.Infrastructure.Repositories
{
    public sealed class RepoAphorism : RepoMaterial<Aphorism, VMAphorism>
    {
        //TODO: изменить 6
        public RepoAphorism() : base(6/*ModelCoreType.Aphorism*/, new Dictionary<string, object> { ["OnlyShow"] = false, ["WithComments"] = false }) { }

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

        protected override Action<SxFilter, StringBuilder> ChangeJoinBody
        {
            get
            {
                return (filter, sb) =>
                {
                    sb.Append(" JOIN D_APHORISM AS da ON da.Id=dm.Id AND da.ModelCoreType=dm.ModelCoreType ");
                    sb.Append(" LEFT JOIN D_AUTHOR_APHORISM AS daa ON daa.Id=da.AuthorId ");
                };
            }
        }

        protected override Action<SxFilter, StringBuilder, DynamicParameters> ChangeWhereBody
        {
            get
            {
                return (filter, sb, param) =>
                {
                    var currentContext = HttpContext.Current;
                    if(currentContext!=null)
                    {
                        var author = currentContext.Request.QueryString.Get("author");
                        if (author != null)
                        {
                            param.Add("author", author);
                            sb.Append(" AND (daa.TitleUrl=@author) ");
                        }
                    }
                };
            }
        }

        protected override Action<SqlConnection, VMAphorism[]> ChangeMaterialsAfterSelect
        {
            get
            {
                return (connection, data) => {
                    var sb = new StringBuilder();
                    VMAphorism item = null;
                    for (int i = 0; i < data.Length; i++)
                    {
                        item = data[i];
                        sb.AppendFormat(",{0}", item.Id);
                    }
                    sb.Remove(0, 1);

                    var materialAphorisms = connection.Query<VMAphorism, VMAuthorAphorism, VMAphorism>("dbo.get_aphorisms_authors @ids", (a, aa)=> {
                        a.Author = aa;
                        return a;
                    }, new { ids = sb.ToString() }).ToArray();
                    if(materialAphorisms.Any())
                    {
                        VMAphorism materialAphirism = null;
                        for (int i = 0; i < materialAphorisms.Length; i++)
                        {
                            materialAphirism = materialAphorisms[i];
                            item = data.SingleOrDefault(x => x.Id == materialAphirism.Id);
                            if (item != null)
                            {
                                item.Author = materialAphirism.Author;
                                item.AuthorId = materialAphirism.Author.Id;
                            }
                        }    
                    }
                };
            }
        }
    }
}
