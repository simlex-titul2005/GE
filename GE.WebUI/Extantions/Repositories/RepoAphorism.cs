using Dapper;
using GE.WebCoreExtantions.Repositories;
using GE.WebUI.Models;
using System.Data.SqlClient;
using System.Linq;

namespace GE.WebUI.Extantions.Repositories
{
    public static partial class RepositoryExtantions
    {
        public static VMDetailAphorism GetByTitleUrl(this RepoAphorism repo,  string titleUrl, int tfaAmount=10)
        {
            var query = @"SELECT*FROM D_APHORISM AS da
JOIN DV_MATERIAL AS dm ON dm.Id = da.Id AND dm.ModelCoreType = da.ModelCoreType
JOIN D_MATERIAL_CATEGORY AS dmc ON dmc.Id = dm.CategoryId
WHERE dm.TitleUrl=@title_url";

            var queryTopForAuthor = @"SELECT TOP(@amount) 
       da.Id,
       da.Author,
       dm.Title,
       dm.TitleUrl,
       dm.Html,
       dm.CategoryId,
       COUNT(dc.Id)         AS CommentsCount
FROM   D_APHORISM           AS da
       JOIN DV_MATERIAL     AS dm
            ON  dm.Id = da.Id
            AND dm.ModelCoreType = da.ModelCoreType
       LEFT JOIN D_COMMENT  AS dc
            ON  dc.MaterialId = dm.Id
            AND dc.ModelCoreType = dm.ModelCoreType
WHERE  (
           (@author IS NULL AND da.Author IS NULL)
           OR (@author IS NOT NULL AND da.Author = @author)
       )
       AND dm.Id NOT IN (@mid)
GROUP BY
       da.Id,
       da.Author,
       dm.Title,
       dm.TitleUrl,
       dm.Html,
       dm.CategoryId
ORDER BY
       CommentsCount";

            var queryTopForCategory = @"SELECT TOP(@amount) 
       da.Id,
       da.Author,
       dm.Title,
       dm.TitleUrl,
       dm.Html,
       dm.CategoryId,
       COUNT(dc.Id)         AS CommentsCount
FROM   D_APHORISM           AS da
       JOIN DV_MATERIAL     AS dm
            ON  dm.Id = da.Id
            AND dm.ModelCoreType = da.ModelCoreType
       LEFT JOIN D_COMMENT  AS dc
            ON  dc.MaterialId = dm.Id
            AND dc.ModelCoreType = dm.ModelCoreType
WHERE  dm.CategoryId=@cat
       AND dm.Id NOT IN (@mid)
GROUP BY
       da.Id,
       da.Author,
       dm.Title,
       dm.TitleUrl,
       dm.Html,
       dm.CategoryId
ORDER BY
       CommentsCount";

            var viewModel = new VMDetailAphorism();
            using (var conn = new SqlConnection(repo.ConnectionString))
            {
                viewModel.Aphorism = conn.Query<VMAphorism, VMMaterialCategory, VMAphorism>(query, (a, c)=> {
                    a.CategoryId = c.Id;
                    a.Category = c;
                    return a;
                }, new { title_url = titleUrl }).SingleOrDefault();
                viewModel.TopForAuthor= conn.Query<VMAphorism>(queryTopForAuthor, new { author = viewModel.Aphorism.Author, mid=viewModel.Aphorism.Id, amount= tfaAmount }).ToArray();
                viewModel.TopForCategory= conn.Query<VMAphorism>(queryTopForCategory, new { cat = viewModel.Aphorism.CategoryId, mid=viewModel.Aphorism.Id, amount= tfaAmount }).ToArray();
            }

            return viewModel;
        }
    }
}