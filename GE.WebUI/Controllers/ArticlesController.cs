using GE.WebCoreExtantions;
using GE.WebCoreExtantions.Repositories;
using GE.WebUI.Models;
using SX.WebCore.Abstract;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dapper;

namespace GE.WebUI.Controllers
{
    public partial class ArticlesController : BaseController
    {
        private SxDbRepository<int, Article, DbContext> _repo;
        public ArticlesController()
        {
            _repo = new RepoArticle();
        }

        [ChildActionOnly]
        [AcceptVerbs(HttpVerbs.Get)]
        public virtual ViewResult ForGamersBlock()
        {
            var viewModel = new VMFGBlock();
            dynamic[] result = null;
            using (var conn = new SqlConnection(_repo.ConnectionString))
            {
                result = conn.Query<dynamic>(Resources.Sql_Articles.FGBGameMenu).ToArray();
            }

            var games = result
                .Select(x => new 
                {
                    Id = x.GameId,
                    Description = x.Description,
                    FrontPictureId = x.FrontPictureId,
                    Title = x.Title
                }).Distinct().ToArray();

            viewModel.Games = new VMFGBGame[games.Length];
            for (int i = 0; i < games.Length; i++)
			{
                var game = games[i];
                viewModel.Games[i] = new VMFGBGame {
                    Id = game.Id,
                    Description = game.Description,
                    FrontPictureId = game.FrontPictureId,
                    Title = game.Title,
                    ArticleTypes = result.Where(t => t.GameId == game.Id).Select(t => new VMFGBArticleType
                    {
                        Name = t.ArticleTypeName,
                        Description = t.ArticleTypeDesc
                    }).ToArray()
                };
			}

            return View(viewModel);
        }

    }
}