using GE.WebCoreExtantions;
using GE.WebCoreExtantions.Repositories;
using GE.WebUI.Models;
using SX.WebCore;
using SX.WebCore.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GE.WebUI.Controllers
{
    public partial class GamesController : BaseController
    {
        private SxDbRepository<int, Game, DbContext> _repo;
        private SxDbRepository<string, SxSiteSetting, DbContext> _repoSetting;
        public GamesController()
        {
            _repo = new RepoGame();
            _repoSetting = new RepoSiteSetting();
        }

        private const string __emptyGameIconPath = "emptyGameIconPath";
        private const string __emptyGameGoodImagePath = "emptyGameGoodImagePath";
        private const string __emptyGameBadImagePath = "emptyGameBadImagePath";
        [ChildActionOnly]
        public virtual PartialViewResult GameList()
        {
            var viewModel = new VMGameMenu();
            viewModel.Games = _repo.All.Where(x => x.Show && x.FrontPictureId.HasValue).OrderBy(x => x.Title).Select(x => Mapper.Map<Game, VMGame>(x)).ToArray();

            var emptyGameIconPath = _repoSetting.GetByKey(__emptyGameIconPath);
            var emptyGameGoodImagePath = _repoSetting.GetByKey(__emptyGameGoodImagePath);
            var emptyGameBadImagePath = _repoSetting.GetByKey(__emptyGameBadImagePath);
            viewModel.EmptyGame = new VMEmptyGame
            {
                IconPath = emptyGameIconPath != null ? emptyGameIconPath.Value : null,
                GoodImagePath = emptyGameGoodImagePath != null ? emptyGameGoodImagePath.Value : null,
                BadImagePath = emptyGameBadImagePath != null ? emptyGameBadImagePath.Value : null,
            };

            return PartialView(MVC.Games.Views._GameList, viewModel);
        }
    }
}