using GE.WebUI.Models;
using GE.WebUI.ViewModels;
using SX.WebCore.SxRepositories;
using GE.WebUI.Infrastructure.Repositories;
using System.Web.Mvc;
using SX.WebCore;
using System.Threading.Tasks;

namespace GE.WebUI.Areas.Admin.Controllers
{
    public sealed class NewsController : MaterialsController<News, VMNews>
    {
        private static RepoNews _repo = new RepoNews();
        public NewsController() : base((byte)Enums.ModelCoreType.News) { }
        public override SxRepoMaterial<News, VMNews> Repo
        {
            get
            {
                return _repo;
            }
        }

        private static readonly string _title = "Новости";

        public override async Task<ActionResult> Index(int page = 1)
        {
            ViewBag.Title = _title;
            return await base.Index(page);
        }

        public override async Task<ActionResult> Edit(int? id = default(int?))
        {
            ViewBag.Scripts = "var gvlGames = new SxGridLookup('#GameId');";
            ViewBag.Title = _title;
            return await base.Edit(id);
        }
    }
}