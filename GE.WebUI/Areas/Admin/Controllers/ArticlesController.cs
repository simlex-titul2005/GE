using GE.WebUI.Models;
using GE.WebUI.ViewModels;
using SX.WebCore.SxRepositories;
using GE.WebUI.Infrastructure.Repositories;
using System.Web.Mvc;
using SX.WebCore;
using System.Threading.Tasks;

namespace GE.WebUI.Areas.Admin.Controllers
{
    public sealed class ArticlesController : MaterialsController<Article, VMArticle>
    {
        private static RepoArticle _repo = new RepoArticle();
        public ArticlesController() : base((byte)Enums.ModelCoreType.Article) { }
        public override SxRepoMaterial<Article, VMArticle> Repo
        {
            get
            {
                return _repo;
            }
        }

        private static readonly string _title = "Статьи";

        public override async Task<ActionResult> Index(int page = 1)
        {
            ViewBag.Title = _title;
            return await base.Index(page);
        }

        public override async Task<ActionResult> Edit(int? id = null)
        {
            ViewBag.ScriptsFiles = "<script src=\"/Areas/Admin/Scripts/editMaterialPage.js\"></script>";
            ViewBag.Scripts = $"var gvlGames=new SxGridLookup('#GameId'); var editMaterialPage=new EditMaterialPage({id}, {ModelCoreType});";
            ViewBag.Title = _title;
            ViewBag.Tabs = "<li role=\"presentation\"><a href=\"#mm-infographics\" aria-controls=\"mm-infographics\" role=\"tab\" data-toggle=\"tab\"><i class=\"fa fa-retweet\" aria-hidden=\"true\"></i> Инфографики</a></li>";
            ViewBag.TabsContent = "<div role=\"tabpanel\" class=\"tab-pane\" id=\"mm-infographics\"><h4>Инфографики</h4><div id=\"infographics\"></div></div>";
            ViewBag.RenderPartial = "~/Areas/Admin/Views/Infographics/_ModalNotLinked.cshtml";
            ViewBag.RenderRecommendations = "_Recommendations";
            return await base.Edit(id);
        }
    }
}