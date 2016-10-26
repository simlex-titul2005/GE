using System;
using System.Data.SqlClient;
using GE.WebUI.Models;
using GE.WebUI.ViewModels;
using SX.WebCore.Repositories;
using System.Linq;
using Dapper;

namespace GE.WebUI.Infrastructure.Repositories
{
    public sealed class RepoMaterialCategory : SxRepoMaterialCategory<MaterialCategory, VMMaterialCategory>
    {
        protected sealed override Action<SqlConnection, MaterialCategory> BeforeGetByKeyAction
        {
            get
            {
                return (connection, data) => {
                    if (!data.GameId.HasValue) return;

                    var query = "SELECT TOP(2) dg.* FROM D_GAME AS dg WHERE dg.Id=@gameId";
                    var game = connection.Query<Game>(query, new { gameId = data.GameId }).SingleOrDefault();
                    data.Game = game;
                };
            }
        }

        protected sealed override Action<SqlConnection, MaterialCategory, MaterialCategory> AfterUpdateAction
        {
            get
            {
                return (connection, model, data) => {
                    if (!model.GameId.HasValue) return;

                    var query = "UPDATE D_MATERIAL_CATEGORY SET GameId=@gameId WHERE Id=@categoryId; SELECT * FROM D_GAME AS dg WHERE dg.Id=@gameId";
                    var game = connection.Query<Game>(query, new { gameId = model.GameId, categoryId = model.Id }).SingleOrDefault();
                    data.Game = game;
                };
            }
        }
    }
}
