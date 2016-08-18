using GE.WebAdmin.Infrastructure.Repositories;
using GE.WebCoreExtantions;
using SX.WebCore;
using SX.WebCore.MvcControllers;
using SX.WebCore.ViewModels;
using System.Threading.Tasks;
using System.Web.Mvc;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace GE.WebAdmin.Controllers
{
    public sealed class PicturesController : SxPicturesController<DbContext>
    {
        static PicturesController()
        {
            if (Repo == null)
                Repo = new RepoPicture();
        }

        private static readonly int _freePageSize=10;

        [HttpGet]
        public PartialViewResult FreePictures(int page=1)
        {
            var order = new SxOrder { FieldName = "Size", Direction = SortDirection.Desc };
            var filter = new SxFilter(page, _freePageSize) { Order = order };

            var viewModel = Repo.GetFreePictures(filter);

            ViewBag.Filter = filter;

            return PartialView("_FreePictures", viewModel);
        }

        [HttpPost]
        public async Task<PartialViewResult> FreePictures(SxVMPicture filterModel, SxOrder order, int page = 1)
        {
            var filter = new SxFilter(page, _freePageSize) { Order = order != null && order.Direction != SortDirection.Unknown ? order : null, WhereExpressionObject = filterModel };

            var viewModel = await Repo.GetFreePicturesAsync(filter); ;

            filter.PagerInfo.Page = filter.PagerInfo.TotalItems <= _freePageSize ? 1 : page;

            ViewBag.Filter = filter;

            return PartialView("_FreePictures", viewModel);
        }
    }
}