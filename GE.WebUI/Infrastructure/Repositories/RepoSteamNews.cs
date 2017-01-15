using System.Threading.Tasks;
using GE.WebUI.Models;
using GE.WebUI.ViewModels;
using SX.WebCore.SxRepositories.Abstract;
using System.Data.SqlClient;
using Dapper;
using System.Linq;
using System;

namespace GE.WebUI.Infrastructure.Repositories
{
    public sealed class RepoSteamNews : SxDbRepository<string, SteamNews, VMSteamNews>
    {
        public async Task<SteamNews> CreateAsync(SteamNews model, int gameId)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var data = await connection.QueryAsync<SteamNews>("dbo.add_steam_news @steamAppId, @gid, @title, @url, @is_external_url, @author, @contents, @feedlabel, @date, @feedname, @titleUrl, @mct, @discriminator, @gameId, @dateCreate", new
                {
                    steamAppId = model.SteamAppId,
                    gid = model.Gid,
                    title = model.Title,
                    titleUrl = SX.WebCore.UrlHelperExtensions.SeoFriendlyUrl(model.Title),
                    url = model.Url,
                    is_external_url = model.IsExternalUrl,
                    author = model.Author,
                    contents = model.Contents,
                    feedlabel = model.FeedLabel,
                    date = model.Date,
                    feedname = model.FeedName,
                    mct = SX.WebCore.Enums.ModelCoreType.News,
                    discriminator = nameof(News),
                    gameId = gameId,
                    dateCreate=DateTimeOffset.FromUnixTimeSeconds(model.Date).DateTime
                });

                return data.SingleOrDefault();
            }
        }

        public async Task<SteamNews[]> GetSteamAppNews(int steamAppId)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var data = await connection.QueryAsync<SteamNews>("dbo.get_steam_app_news @steamAppId", new { steamAppId });
                return data.ToArray();
            }
        }
    }
}