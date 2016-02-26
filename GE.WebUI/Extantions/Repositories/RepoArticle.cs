﻿using GE.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Dapper;

namespace GE.WebUI.Extantions.Repositories
{
    public static partial class RepositoryExtantions
    {
        public static VMFGBlock ForGamersBlock(this GE.WebCoreExtantions.Repositories.RepoArticle repo)
        {
            var viewModel = new VMFGBlock();
            dynamic[] result = null;
            using (var conn = new SqlConnection(repo.ConnectionString))
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
                viewModel.Games[i] = new VMFGBGame
                {
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

            return viewModel;
        }

        public static VMPreviewArticle[] PreviewMaterials(this GE.WebCoreExtantions.Repositories.RepoArticle repo, int gameId, string articleType, int lettersCount=200)
        {
            using (var conn = new SqlConnection(repo.ConnectionString))
            {
                return conn.Query<VMPreviewArticle>(Resources.Sql_Articles.PreviewMaterials, new { GAME_ID = gameId, ARTICLE_TYPE_NAME = articleType, LETTERS_COUNT=lettersCount }).ToArray();
            }
        }

        public static Dictionary<int, string> Last(this GE.WebCoreExtantions.Repositories.RepoArticle repo, int amount)
        {
            var result = new Dictionary<int, string>();
            using (var conn = new SqlConnection(repo.ConnectionString))
            {
                var results = conn.Query<dynamic>(Resources.Sql_Articles.LastArticles, new { AMOUNT = amount }).ToArray();
                for (int i = 0; i < results.Length; i++)
                {
                    var a=results[i];
                    result.Add(a.Id, a.Title);
                }
            }
            return result;
        }
    }
}