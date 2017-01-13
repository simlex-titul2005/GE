﻿using System.Threading.Tasks;
using GE.WebUI.Models;
using GE.WebUI.ViewModels;
using SX.WebCore.SxRepositories.Abstract;
using System.Data.SqlClient;
using Dapper;
using System.Linq;

namespace GE.WebUI.Infrastructure.Repositories
{
    public sealed class RepoSteamNews : SxDbRepository<string, SteamNews, VMSteamNews>
    {
        public override async Task<SteamNews> CreateAsync(SteamNews model)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var data = await connection.QueryAsync<SteamNews>("dbo.add_steam_news @steamAppId, @gid, @title, @url, @is_external_url, @author, @contents, @feedlabel, @date, @feedname", new
                {
                    steamAppId = model.SteamAppId,
                    gid = model.Gid,
                    title = model.Title,
                    url = model.Url,
                    is_external_url = model.IsExternalUrl,
                    author = model.Author,
                    contents = model.Contents,
                    feedlabel = model.FeedLabel,
                    date = model.Date,
                    feedname = model.FeedName
                });

                return data.SingleOrDefault();
            }
        }
    }
}