using System;
using System.Data.SqlClient;
using GE.WebUI.Models;
using GE.WebUI.ViewModels;
using SX.WebCore.SxRepositories;
using System.Linq;
using Dapper;
using SX.WebCore.ViewModels;

namespace GE.WebUI.Infrastructure.Repositories
{
    public sealed class RepoMaterialCategory : SxRepoMaterialCategory<MaterialCategory, VMMaterialCategory>
    {
        protected sealed override Action<SqlConnection, MaterialCategory> BeforeGetByKeyAction
        {
            get
            {
                return (connection, data) =>
                {
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
                return (connection, model, data) =>
                {
                    var query = "UPDATE D_MATERIAL_CATEGORY SET IsFeatured=@feauture, Description=@desc WHERE Id=@id";
                    connection.Execute(query, new { feauture = model.IsFeatured, id = model.Id, desc=model.Description });

                    if (model.GameId.HasValue)
                    {
                        query = "UPDATE D_MATERIAL_CATEGORY SET GameId=@gameId WHERE Id=@categoryId; SELECT * FROM D_GAME AS dg WHERE dg.Id=@gameId";
                        var game = connection.Query<Game>(query, new { gameId = model.GameId, categoryId = model.Id }).SingleOrDefault();
                        data.Game = game;
                    }
                };
            }
        }

        public VMMaterialCategory[] GetFeatured(int amount)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var data = connection.Query<VMMaterialCategory, SxVMPicture, VMMaterialCategory>("dbo.get_feautured_material_categories @amount", (c, p) =>
                {
                    c.FrontPicture = p;
                    return c;
                }, new { amount = amount }, splitOn: "Id");
                return data.ToArray();
            }
        }
    }
}
