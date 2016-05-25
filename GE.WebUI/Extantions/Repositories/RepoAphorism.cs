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
                }, new { title_url = titleUrl, author_amount = tfaAmount, cat_amount = tcAmount }, splitOn:"CategoryId,AuthorId").ToArray();
                var model = data.SingleOrDefault(x => x.Flag == VMAphorism.AphorismFlag.ForThis);
                viewModel.Aphorism = model;
                viewModel.TopForAuthor = data.Where(x => x.Flag == VMAphorism.AphorismFlag.ForAuthor).ToArray();
                viewModel.TopForCategory = data.Where(x => x.Flag == VMAphorism.AphorismFlag.ForCategory).ToArray();
            }

            return viewModel;
        }
        //private static VMAphorism getAphorism(dynamic model)
        //{
        //    var data=new VMAphorism
        //    {
        //        AuthorId = model.AuthorId != null ? (int)model.AuthorId : 0,
        //        Author = model.AuthorId != null ? new VMAuthorAphorism { Id = model.AuthorId, Name = model.AuthorName, PictureId = model.PictureId } : null,
        //        Category = new VMMaterialCategory { Title = model.CategoryTitle },
        //        CategoryId = model.CategoryId,
        //        Flag = 0,
        //        Html = model.Html,
        //        Id = model.Id,
        //        Title = model.Title,
        //        TitleUrl = model.TitleUrl
        //    };
        //    return data;
        //}

        public static VMAphorismCategory[] GetAphorismCategories(this RepoAphorism repo, string curCategoryId=null)
        {
            using (var conn = new SqlConnection(repo.ConnectionString))
            {
                var data = conn.Query<VMAphorismCategory>("get_aphorism_categories @curCat", new { curCat = curCategoryId }).ToArray();
                return data;
            }
        }
    }
}