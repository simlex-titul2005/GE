using AutoMapper;
using GE.WebUI.Infrastructure.Repositories;
using GE.WebUI.Models;
using GE.WebUI.ViewModels;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SX.WebCore.MvcApplication;
using SX.WebCore.SxRepositories;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using SX.WebCore;
using System;

namespace GE.WebUI.Hubs
{
    public sealed class HubSteamApps : Hub
    {
        private static SxRepoApiParameter _repoApiParameter { get; set; } = new SxRepoApiParameter();
        private static RepoSteamApp _repoSteamApp { get; set; } = new RepoSteamApp();
        private static RepoGame _repoGame { get; set; } = new RepoGame();
        private static RepoSteamNews _repoSteamNews { get; set; } = new RepoSteamNews();

        private static IMapper _mapper { get; set; }
        static HubSteamApps()
        {
            _mapper = SxMvcApplication.MapperConfiguration.CreateMapper();
        }

        private const string _getAppListUrl = "https://api.steampowered.com/ISteamApps/GetAppList/v2";
        private const string _getAppNewsUrl = "https://api.steampowered.com/ISteamNews/GetNewsForApp/v2";
        private static bool _locker = false;
        private static volatile bool _cancelationToken = false;

        public async Task GetAppList()
        {
            if (_locker) return;

            _cancelationToken = false;
            _locker = true;

            var apiKey = (await _repoApiParameter.GetApiParameterAsync("Steam", "steam-api-key"))?.Value;
            if (string.IsNullOrEmpty(apiKey))
            {
                Clients.All.addStatusAppListMessage("Необходимо задать натройку steam-api-key для API Steam");
                _locker = false;
                return;
            }

            var data = new SteamApp[0];
            using (var httpClient = new HttpClient())
            {
                var json = await httpClient.GetStringAsync($"{_getAppListUrl}?key={apiKey}");
                data = ((JArray)JsonConvert.DeserializeObject<dynamic>(json).applist.apps).Select(x => new SteamApp() { Id = (int)x["appid"], Name = (string)x["name"] }).ToArray();
            }

            if (!data.Any())
            {
                Clients.All.addStatusAppListMessage("Запрос вернул пустой набор данных!");
                _locker = false;
                return;
            }

            Clients.All.addStatusAppListMessage("Получение существующих записей...");
            var existData = (await _repoSteamApp.AllAsync()).Select(x => _mapper.Map<VMSteamApp, SteamApp>(x)).ToArray();

            var comparer = new SteamAppComparer();
            var list = data.Except(existData, comparer).ToArray();

            if (!list.Any())
            {
                Clients.All.endProcessingAppList();
                _locker = false;
                return;
            }


            var length = list.Length;

            SteamApp item;
            for (int i = 0; i < length; i++)
            {
                if (_cancelationToken) break;

                var progressBarValue = System.Math.Round((double)(i + 1) * 100 / length, 2);
                item = list[i];
                Clients.All.addStatusAppListMessage($"%[{progressBarValue}] appid: {item.Id}, name: {item.Name}", progressBarValue);
                _repoSteamApp.Create(item);
            }

            _locker = false;

            Clients.All.endProcessingAppList();
        }
        private class SteamAppComparer : IEqualityComparer<SteamApp>
        {
            public bool Equals(SteamApp obj1, SteamApp obj2)
            {
                return obj1.Id == obj2.Id && obj1.Name == obj2.Name;
            }

            public int GetHashCode(SteamApp model)
            {
                return model.Id;
            }
        }

        private static HashSet<SteamNews> _gamesNews = new HashSet<SteamNews>();
        public async Task GetNewsForApp(int gameId)
        {
            var apiKey = (await _repoApiParameter.GetApiParameterAsync("Steam", "steam-api-key"))?.Value;
            if (string.IsNullOrEmpty(apiKey))
            {
                Clients.All.insertModalAppNewsHtml($"<strong class=\"text-danger\">Необходимо задать натройку steam-api-key для API Steam</strong>");
                return;
            }

            var game = await _repoGame.GetByKeyAsync(gameId);
            if (game == null)
            {
                Clients.All.insertModalAppNewsHtml($"<strong class=\"text-danger\">Отсутсвует запрашиваемая игра</strong>");
                return;
            }

            Clients.All.insertModalAppNewsHtml($"<strong>{game.Title}</strong>");

            var gameSteamApps = await _repoSteamApp.ReadAsync(new SxFilter(1, int.MaxValue) { AddintionalInfo = new object[] { null, gameId } });
            if (!gameSteamApps.Any())
            {
                Clients.All.insertModalAppNewsHtml($"<strong class=\"text-danger\">У данной игры нет привязанных приложений Steam</strong>");
                return;
            }

            // формирование списка приложений Steam
            var sb = new System.Text.StringBuilder();
            sb.Append("<table class=\"table table-striped table-condensed game-modal-steam-apps-table\">");
            VMSteamApp item = null;
            for (int i = 0; i < gameSteamApps.Length; i++)
            {
                item = gameSteamApps[i];
                sb.Append("<tr>");
                sb.AppendFormat("<td><input type=\"checkbox\" name=\"steamAppArray[]\" checked=\"checked\" value=\"{0}\"/></td>", item.Id);
                sb.AppendFormat("<td style=\"width:50%;\">{0}</td>", item.Name);
                sb.Append("<td style=\"width:50%;\"><div class=\"text-right\"><div class=\"game-modal-steam-apps-table__progress\"></div></td>");
                sb.Append("</tr>");
            }
            sb.Append("</table>");
            Clients.All.insertModalAppNewsHtml(sb.ToString());

            // запрос новостей по каждому steam приложению
            _gamesNews.Clear();
            SteamNews[] news;
            for (int i = 0; i < gameSteamApps.Length; i++)
            {
                item = gameSteamApps[i];
                using (var httpClient = new HttpClient())
                {
                    try
                    {
                        var url = $"{_getAppNewsUrl}?appid={item.Id}&key={apiKey}";
                        var json = await httpClient.GetStringAsync(url);

                        news = ((JArray)JsonConvert.DeserializeObject<dynamic>(json).appnews.newsitems).Select(x => new SteamNews()
                        {
                            Author = (string)x["author"],
                            Contents = (string)x["contents"],
                            Date = (int)x["date"],
                            FeedLabel = (string)x["feedlabel"],
                            FeedName = (string)x["feedname"],
                            Gid = (string)x["gid"],
                            IsExternalUrl = Convert.ToBoolean(x["is_external_url"]),
                            Title = (string)x["title"],
                            Url = (string)x["url"],
                            SteamAppId = item.Id
                        }).ToArray();
                    }
                    catch (Exception ex)
                    {
                        Clients.All.addModalAppNewsProcessedCount(item.Id, new { Status = "error", Count = 0, Message = ex.Message });
                        continue;
                    }
                }

                if (news.Any())
                {
                    for (int y = 0; y < news.Length; y++)
                    {
                        _gamesNews.Add(news[y]);
                    }
                    Clients.All.addModalAppNewsProcessedCount(item.Id, new { Status = "ok", Count = news.Length });
                }
                else
                {
                    Clients.All.addModalAppNewsProcessedCount(item.Id, new { Status = "ok", Count = 0 });
                }
            }

            Clients.All.showAddNewsButton(gameId);
        }
        public async Task ClearNewsList()
        {
            await Task.Run(() => {
                _gamesNews.Clear();
            });
        }
        public async Task AddNews(int gameId, int[] steamAppArray)
        {
            if(steamAppArray==null || !steamAppArray.Any())
            {
                return;
            }

            var errorsCount=0;
            var steamApps = _gamesNews.Where(x=> steamAppArray.Contains(x.SteamAppId)).GroupBy(x => x.SteamAppId);
            foreach (var steamApp in steamApps)
            {
                var newsList = steamApp.Select(x => x);
                var length = newsList.Count();
                var counter = 0;
                foreach (var item in newsList)
                {
                    try
                    {
                        await _repoSteamNews.CreateAsync(item, gameId);
                        counter++;
                        var percent = Math.Round((double)counter * 100 / length, 2);
                        Clients.All.fillNewsProgress(steamApp.Key, percent);
                    }
                    catch(Exception ex)
                    {
                        errorsCount++;
                        Clients.All.addModalAppNewsProcessedCount(steamApp.Key, new { Status = "error", Count = 0, Message = ex.Message });
                        continue;
                    }
                }
            }

            _gamesNews.Clear();
            if(errorsCount==0)
                Clients.All.showAddNewsSuccess();
        }

        public async Task CancelProcessing()
        {
            await Task.Run(() =>
            {
                _cancelationToken = true;
            });
        }
    }
}