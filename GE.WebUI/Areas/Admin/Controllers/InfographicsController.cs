using GE.WebUI.Infrastructure.Repositories;
using GE.WebUI.Models;
using GE.WebUI.ViewModels;
using SX.WebCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace GE.WebUI.Areas.Admin.Controllers
{
    public sealed class InfographicsController : BaseController
    {
        public static RepoInfographic Repo { get; set; } = new RepoInfographic();

        private static readonly int _pageSize = 20;
        public async Task<ActionResult> Index(int mid, byte mct, int page = 1, bool linked = true)
        {
            if (!Request.IsAjaxRequest()) return new HttpNotFoundResult();

            var defaultOrder = new SxOrderItem { FieldName = "Caption", Direction = SortDirection.Asc };
            var filter = new SxFilter(page, linked ? _pageSize : 10) { MaterialId = mid, ModelCoreType = mct, AddintionalInfo = new object[] { linked }, Order = defaultOrder };
            var viewModel = await Repo.ReadAsync(filter);
            ViewBag.Filter = filter;

            return PartialView("_GridView", viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> Index(VMInfographic filterModel, SxOrderItem order, int mid, byte mct, int page = 1, bool linked = true)
        {
            if (!Request.IsAjaxRequest()) return new HttpNotFoundResult();

            var filter = new SxFilter(page, linked ? _pageSize : 10) { MaterialId = mid, ModelCoreType = mct, AddintionalInfo = new object[] { linked }, Order = order != null && order.Direction != SortDirection.Unknown ? order : null, WhereExpressionObject = filterModel };
            var viewModel = await Repo.ReadAsync(filter);
            ViewBag.Filter = filter;

            return PartialView("_GridView", viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(Infographic model)
        {
            var data = await Repo.GetByKeyAsync(new object[] { model.PictureId });
            if (data != null)
                await Repo.DeleteAsync(model);

            return RedirectToAction("Index", new { mid = model.MaterialId, mct = model.ModelCoreType, linked = true });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> AddList(int mid, byte mct, List<Guid> ids)
        {
            await Repo.AddListAsync(mid, mct, ids);
            return RedirectToAction("Index", new { mid = mid, mct = mct, linked = true });
        }
    }
}