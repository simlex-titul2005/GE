using GE.WebCoreExtantions;
using GE.WebCoreExtantions.Repositories;
using GE.WebUI.Models;
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
        public GamesController()
        {
            _repo = new RepoGame();
        }

        [ChildActionOnly]
        public virtual PartialViewResult GameList()
        {
            var viewModel = _repo.All.Where(x => x.Show).Select(x=>Mapper.Map<Game, VMGame>(x)).ToArray();
            return PartialView(MVC.Games.Views._GameList, viewModel);
        }

    }
}