using SX.WebCore;
using System.Web.Mvc;
using SX.WebCore.MvcControllers;
using GE.WebUI.ViewModels.Abstracts;
using SX.WebCore.ViewModels;
using System.Globalization;
using System.Linq;
using SX.WebCore.MvcApplication;
using System.Threading.Tasks;
using SX.WebCore.DbModels.Abstract;
using SX.WebCore.DbModels;

namespace GE.WebUI.Controllers
{
    public abstract class MaterialsController<TModel, TViewModel> : SxMaterialsController<TModel, TViewModel>
        where TModel : SxMaterial
        where TViewModel : VMMaterial, new()
    {

        protected MaterialsController(byte mct) : base(mct)
        {
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
                var existGame = GamesController.Repo.ExistGame(gameTitle);
                if (!existGame)
                {
                    return false;
                }
            }


            switch (ModelCoreType)
            {
                case 1://article
                    filter.PagerInfo.PageSize = 9;
                    break;
                case 2://news
                    filter.PagerInfo.PageSize = 10;
                    break;
                //TODO: убрать 7
                case 7://humor
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
            VMMaterial model = null;
            switch (ModelCoreType)
            {
                case 1://article
                    model = Repo.GetByTitleUrl(year, month, day, titleUrl);
                    break;
                case 2://news
                    model = Repo.GetByTitleUrl(year, month, day, titleUrl);
                    break;
                //TODO: убрать 7
                case 7://humor
                    model = Repo.GetByTitleUrl(year, month, day, titleUrl);
                    break;
            }

            if (model == null) return new HttpNotFoundResult();

            var html = model.Html;
            SxBBCodeParser.ReplaceValutes(ref html);
            SxBBCodeParser.ReplaceBanners(ref html, SxMvcApplication.BannerProvider.BannerCollection, (b) => Url.Action("Picture", "Pictures", new { id = b.PictureId }));
            SxBBCodeParser.ReplaceVideo(ref html, model.Videos);
            SxBBCodeParser.ReplaceImages(ref html, model.Pictures);
            if(!Request.IsLocal)
                SxBBCodeParser.ReplaceGoogleBanners(ref html, SxMvcApplication.BannerProvider.BannerCollection);
            model.Html = html;

            var matSeoInfo = Mapper.Map<SxSeoTags, SxVMSeoTags>(SxSeoTagsController.Repo.GetSeoTags(model.Id, model.ModelCoreType));

            ViewBag.Title = ViewBag.Title ?? (matSeoInfo != null ? matSeoInfo.SeoTitle : null) ?? model.Title;
            ViewBag.Description = ViewBag.Description ?? (matSeoInfo != null ? matSeoInfo.SeoDescription : null) ?? model.Foreword;
            ViewBag.Keywords = ViewBag.Keywords ?? (matSeoInfo != null ? matSeoInfo.KeywordsString : null);
            ViewBag.H1 = ViewBag.H1 ?? (matSeoInfo != null ? matSeoInfo.H1 : null) ?? model.Title;

            CultureInfo ci = new CultureInfo("en-US");
            ViewBag.LastModified = model.DateUpdate.ToString("ddd, dd MMM yyyy HH:mm:ss 'GMT'", ci);

            if (model.Game != null)
                ViewBag.GameName = model.Game.TitleUrl.ToLower();

            var breadcrumbs = (SxVMBreadcrumb[])ViewBag.Breadcrumbs;
            if (breadcrumbs != null)
            {
                var bc = breadcrumbs.ToList();
                if(model.Category!=null)
                    bc.Add(new SxVMBreadcrumb { Title = model.Category.Title, Url=Url.Action("Index", new { cat=model.Category.Id }) });

                bc.Add(new SxVMBreadcrumb { Title = ViewBag.Title });
                ViewBag.Breadcrumbs = bc.ToArray();
            }

            if (!Request.IsLocal)
            {
                //update views count
                Task.Run(() =>
                {
                    Repo.AddUserView(model.Id, model.ModelCoreType);
                });

                model.ViewsCount = model.ViewsCount + 1;
            }

            return View(model);
        }
    }
}