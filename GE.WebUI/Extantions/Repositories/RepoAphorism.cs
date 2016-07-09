using Dapper;
using GE.WebCoreExtantions.Repositories;
using GE.WebUI.Models;
using System.Data.SqlClient;
using System.Linq;

namespace GE.WebUI.Extantions.Repositories
{
    public static partial class RepositoryExtantions
    {
        public static VMDetailAphorism GetByTitleUrl(this RepoAphorism repo, string categoryId, string titleUrl, int tfaAmount = 10, int tcAmount = 10)
        {
            var viewModel = new VMDetailAphorism();
            using (var conn = new SqlConnection(repo.ConnectionString))
            {
                var data = conn.Query<VMAphorism, VMMaterialCategory, VMAuthorAphorism, VMAphorism>("get_aphorism_page_model @title_url, @author_amount, @cat_amount", (a, c, au)=> {
                    a.CategoryId = c.Id;
                    a.Category = c;
                    a.AuthorId = au.Id;
                    a.Author = au;
                    return a;
                }, new { title_url = titleUrl, author_amount = tfaAmount, cat_amount = tcAmount }, splitOn:"Id").ToArray();
                var model = data.SingleOrDefault(x => x.Flag == VMAphorism.AphorismFlag.ForThis);
                viewModel.Aphorism = model;
                viewModel.TopForAuthor = data.Where(x => x.Flag == VMAphorism.AphorismFlag.ForAuthor).ToArray();
                viewModel.TopForCategory = data.Where(x => x.Flag == VMAphorism.AphorismFlag.ForCategory).ToArray();
            }

            return viewModel;
        }

        public static VMAphorismCategory[] GetAphorismCategories(this RepoAphorism repo, string cur=null)
        {
            dynamic[] data;
            using (var conn = new SqlConnection(repo.ConnectionString))
            {
                data = conn.Query("get_aphorism_categories @cur", new { cur = cur }).ToArray();
            }

            var result = data.Where(x => x.Ref == null).OrderBy(x=>x.Title).Select(x => new VMAphorismCategory { Id=x.Id, Title=x.Title }).ToArray();

            VMAphorismCategory category;
            for (int i = 0; i < result.Length; i++)
            {
                category = result[i];
                category.Authors = data.Where(x => Equals(x.Ref, category.Id)).OrderBy(x => x.Title).Select(x => new VMAphorismCategory { Id = x.Id, Title = x.Title }).ToArray();
            }

            return result;
        }
    }
}