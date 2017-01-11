using GE.WebUI.Infrastructure.Repositories;
using GE.WebUI.Models;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SX.WebCore.SxRepositories;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace GE.WebUI.Hubs
{
    public sealed class HubSteamApps : Hub
    {
        private static SxRepoApiParameter _repoApiParameter { get; set; } = new SxRepoApiParameter();
        private static RepoSteamApp _repoSteamApp { get; set; } = new RepoSteamApp();

        private const string _getAppListUrl = "http://api.steampowered.com/ISteamApps/GetAppList/v2";
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
            using (var webClient = new WebClient())
            {
                var json = await webClient.DownloadStringTaskAsync(new System.Uri($"{_getAppListUrl}?key={apiKey}"));
                data = ((JArray)JsonConvert.DeserializeObject<dynamic>(json).applist.apps).Select(x => new SteamApp() { AppId = (int)x["appid"], Name = (string)x["name"] }).ToArray();
                Clients.All.addStatusAppListMessage("Завершено получение списка игр...");
            }

            if (!data.Any())
            {
                Clients.All.addStatusAppListMessage("Запрос вернул пустой набор данных!");
                _locker = false;
                return;
            }

            var length = data.Length;

            SteamApp item;
            for (int i = 0; i < length; i++)
            {
                if (_cancelationToken) break;

                var progressBarValue = System.Math.Round((double)(i + 1) * 100 / length, 2);
                item = data[i];
                Clients.All.addStatusAppListMessage($"%[{progressBarValue}] appid: {item.AppId}, name: {item.Name}", progressBarValue);
                _repoSteamApp.Create(item);
            }

            _locker = false;

            Clients.All.endProcessingAppList();
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