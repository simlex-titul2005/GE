using GE.WebCoreExtantions;
using GE.WebCoreExtantions.Repositories;
using GE.WebUI.Models;
using SX.WebCore;
using SX.WebCore.Abstract;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using GE.WebUI.Extantions.Repositories;
using static SX.WebCore.Enums;
using System.Globalization;
using SX.WebCore.Repositories;
using SX.WebCore.MvcApplication;
using SX.WebCore.ViewModels;
using SX.WebCore.MvcControllers;
using GE.WebUI.Infrastructure;

namespace GE.WebUI.Controllers
{
    public class MaterialsController<TModel> : SxMaterialsController<TModel, VMMaterial, DbContext>
        where TModel : SxMaterial
    {
        private SxDbRepository<int, Game, DbContext> _repoGame;
        protected MaterialsController(ModelCoreType mct) : base(mct)
        {
            if (_repoGame == null)
                _repoGame = new RepoGame();
            WriteBreadcrumbs = BreadcrumbsManager.WriteBreadcrumbs;
            BeforeSelectListAction = beforeSelectListAction;
        }

        private bool beforeSelectListAction(SxFilter filter)
        {
            var routeDataValues = Request.RequestContext.RouteData.Values;
            var gameTitle = (string)routeDataValues["gameTitle"];
            ViewBag.GameTitle = gameTitle;
            filter.AddintionalInfo = new object[] { gameTitle };
            if (gameTitle != null)
            {
                var existGame = (_repoGame as RepoGame).ExistGame(gameTitle);
                if (!existGame)
                {
                    return false;
                }
            }


            switch (ModelCoreType)
            {
                case ModelCoreType.Article:
                    filter.PagerInfo.PageSize = 9;
                    break;
                case ModelCoreType.News:
                    filter.PagerInfo.PageSize = 10;
                    break;
                case ModelCoreType.Humor:
                    filter.PagerInfo.PageSize = 10;
                    break;
            }

            return true;
        }

#if !DEBUG
        [OutputCache(Duration =900, VaryByParam ="MaterialId;ModelCoreType")]
#endif
        [HttpGet]
        public virtual ActionResult Details(int year, string month, string day, string titleUrl)
        {
            VMMaterial model =null;
            switch (ModelCoreType)
            {
                case ModelCoreType.Article:
                    model = Repo.GetByTitleUrl(year, month, day, titleUrl);
                    break;
                case ModelCoreType.News:
                    model = Repo.GetByTitleUrl(year, month, day, titleUrl);
                    break;
                case ModelCoreType.Humor:
                    model = Repo.GetByTitleUrl(year, month, day, titleUrl);
                    break;
            }

            if (model == null) return new HttpNotFoundResult();

            var html = model.Html;
            SxBBCodeParser.ReplaceValutes(ref html);
            SxBBCodeParser.ReplaceBanners(ref html, SxApplication<DbContext>.BannerProvider.BannerCollection, (b) => Url.Action("Picture", "Pictures", new { id = b.PictureId }));
            SxBBCodeParser.ReplaceVideo(ref html, model.Videos);
            model.Html = html;

            var seoInfoRepo = new SxRepoSeoTags<DbContext>();
            var matSeoInfo = Mapper.Map<SxSeoTags, SxVMSeoTags>(seoInfoRepo.GetSeoTags(model.Id, model.ModelCoreType));

            ViewBag.Title = ViewBag.Title ?? (matSeoInfo != null ? matSeoInfo.SeoTitle : null) ?? model.Title;
            ViewBag.Description = ViewBag.Description ?? (matSeoInfo != null ? matSeoInfo.SeoDescription : null) ?? model.Foreword;
            ViewBag.Keywords = ViewBag.Keywords ?? (matSeoInfo != null ? matSeoInfo.KeywordsString : null);
            ViewBag.H1 = ViewBag.H1 ?? (matSeoInfo != null ? matSeoInfo.H1 : null) ?? model.Title;

            CultureInfo ci = new CultureInfo("en-US");
            ViewBag.LastModified = model.DateUpdate.ToString("ddd, dd MMM yyyy HH:mm:ss 'GMT'", ci);

            if(model.Game != null)
                ViewBag.GameName = model.Game.TitleUrl.ToLower();

            var breadcrumbs = (SxVMBreadcrumb[])ViewBag.Breadcrumbs;
            if (breadcrumbs != null)
            {
                var bc = breadcrumbs.ToList();
                bc.Add(new SxVMBreadcrumb { Title = ViewBag.Title });
                ViewBag.Breadcrumbs = bc.ToArray();
            }

            //update views count
            Task.Run(() =>
            {
                Repo.AddUserView(model.Id, model.ModelCoreType);
            });

            model.ViewsCount = model.ViewsCount + 1;

            return View(model);
        }
    }
}