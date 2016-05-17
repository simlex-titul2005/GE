using Dapper;
using GE.WebCoreExtantions.Repositories;
using GE.WebUI.Models;
using System.Data.SqlClient;
using System.Linq;

namespace GE.WebUI.Extantions.Repositories
{
    public static partial class RepositoryExtantions
    {
        public static VMDetailAphorism GetByTitleUrl(this RepoAphorism repo, string categoryId,  string titleUrl, int tfaAmount=10, int tcAmount=10)
        {
            var viewModel = new VMDetailAphorism();
            using (var conn = new SqlConnection(repo.ConnectionString))
            {
                var data=conn.Query<dynamic>("SELECT*FROM get_aphorism_page_model(@title_url, @author_amount, @cat_amount)", new { title_url = titleUrl, author_amount= tfaAmount, cat_amount= tcAmount }).ToArray();
                var model= data.SingleOrDefault(x => (int)x.Flag == (int)VMAphorism.AphorismFlag.ForThis);
                viewModel.Aphorism = getAphorism(model);
                viewModel.TopForAuthor = data.Where(x => (int)x.Flag == (int)VMAphorism.AphorismFlag.ForAuthor).Select(x => (VMAphorism)getAphorism(x)).ToArray();
                viewModel.TopForCategory = data.Where(x => (int)x.Flag == (int)VMAphorism.AphorismFlag.ForCategory).Select(x => (VMAphorism)getAphorism(x)).ToArray();
            }

            return viewModel;
        }

        private static VMAphorism getAphorism(dynamic model)
        {
            return new VMAphorism
            {
                AuthorId = model.AuthorId,
                Author= model.AuthorId!=0?new VMAuthorAphorism { Id= model.AuthorId, Name=model.AuthorName, PictureId=model.AuthorPictureId }:null,
                Category = new VMMaterialCategory { Title = model.CategoryTitle },
                CategoryId = model.CategoryId,
                Flag = 0,
                Html = model.Html,
                Id = model.Id,
                Title = model.Title,
                TitleUrl = model.TitleUrl
            };
        }
    }
}