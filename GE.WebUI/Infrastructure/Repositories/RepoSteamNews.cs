using System.Threading.Tasks;
using GE.WebUI.Models;
using GE.WebUI.ViewModels;
using SX.WebCore.SxRepositories.Abstract;
using System.Data.SqlClient;
using Dapper;
using System.Linq;
using System;
using SX.WebCore;

namespace GE.WebUI.Infrastructure.Repositories
{
    public sealed class RepoSteamNews : SxDbRepository<string, SteamNews, VMSteamNews>
    {
        public async Task<SteamNews> CreateAsync(SteamNews model, int gameId)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var @params = new
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
                    dateCreate = DateTimeOffset.FromUnixTimeSeconds(model.Date).DateTime
                };

                var data = await connection.QueryAsync<SteamNews>("dbo.add_steam_news @steamAppId, @gid, @title, @url, @is_external_url, @author, @contents, @feedlabel, @date, @feedname, @titleUrl, @mct, @discriminator, @gameId, @dateCreate", @params);

                return data.SingleOrDefault();
            }
        }

        public async Task<SteamNews[]> GetSteamAppNewsAsync(int steamAppId, string[] newsIds)
        {
            if (newsIds == null || !newsIds.Any()) return new SteamNews[0];

            using (var connection = new SqlConnection(ConnectionString))
            {
                var data = await connection.QueryAsync<SteamNews>("dbo.get_steam_app_news @steamAppId, @newsIds", new { steamAppId, newsIds =newsIds.ToDelimeterString(',') });
                return data.ToArray();
            }
        }
    }
}