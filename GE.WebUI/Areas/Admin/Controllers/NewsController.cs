using GE.WebUI.Models;
using GE.WebUI.ViewModels;
using SX.WebCore.SxRepositories;
using GE.WebUI.Infrastructure.Repositories;
using System.Web.Mvc;
using SX.WebCore;
using System.Threading.Tasks;
using static SX.WebCore.HtmlHelpers.SxExtantions;
using SX.WebCore.ViewModels;
using System;

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

        private static string getSteamColumnTemplate(SxVMMaterial model)
        {
            var template=((VMNews)model).SteamNewsGid == null ? null : "<button class=\"btn btn-xs btn-primary\"><i class=\"fa fa-steam-square\" area-hidden=\"true\"></i></button>";
            return template;
        }
        private static SxGridViewColumn<SxVMMaterial>[] addColums = new SxGridViewColumn<SxVMMaterial>[] {
                new SxGridViewColumn<SxVMMaterial>() { Caption="Steam", FieldName="Id", ColumnCssClass=x=>"col-steam col-cm", EnableFiltering=false, EnableSorting=false, Template=getSteamColumnTemplate }
                };

        public override async Task<ActionResult> Index(int page = 1)
        {
            ViewBag.Title = _title;
            ViewBag.Columns = addColums;
            ViewBag.ScriptFiles = new string[] { "<script src=\"/Areas/Admin/Scripts/news.js\"></script>" };
            return await base.Index(page);
        }

        public override async Task<ActionResult> Index(VMNews filterModel, SxOrderItem order, int page = 1)
        {
            ViewBag.Columns = addColums;
            return await base.Index(filterModel, order, page);
        }

        public override async Task<ActionResult> Edit(int? id = default(int?))
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