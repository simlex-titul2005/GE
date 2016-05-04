using GE.WebAdmin.Models;
using GE.WebCoreExtantions;
using SX.WebCore;
using SX.WebCore.Abstract;
using SX.WebCore.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace GE.WebAdmin.Controllers
{
    public partial class StatisticsController : BaseController
    {
        private SxDbRepository<Guid, SxStatistic, DbContext> _repo;
        public StatisticsController()
        {
            _repo = new RepoStatistic<DbContext>();
        }

        private static int _pageUserLoginsSize = 20;
        [HttpGet]
        public virtual ActionResult StatUserLogins(int page = 1)
        {
            var filter = new WebCoreExtantions.Filter(page, _pageUserLoginsSize);
            var totalItems = (_repo as RepoStatistic<DbContext>).UserLoginsCount(filter);
            filter.PagerInfo.TotalItems = totalItems;
            ViewBag.PagerInfo = filter.PagerInfo;

            var viewModel = (_repo as RepoStatistic<DbContext>).UserLogins(filter).Select(x=>Mapper.Map<SxStatisticUserLogin, VMStatisticUserLogin>(x)).ToArray();
            return View(MVC.Statistics.Views.UserLogins, viewModel);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public virtual PartialViewResult StatUserLogins(VMStatisticUserLogin filterModel, IDictionary<string, SortDirection> order, int page = 1)
        {
            ViewBag.Filter = filterModel;
            ViewBag.Order = order;

            var filter = new WebCoreExtantions.Filter(page, _pageUserLoginsSize) { Orders = order, WhereExpressionObject = filterModel };
            var totalItems = (_repo as RepoStatistic<DbContext>).UserLoginsCount(filter);
            filter.PagerInfo.TotalItems = totalItems;
            ViewBag.PagerInfo = filter.PagerInfo;

            var viewModel = (_repo as RepoStatistic<DbContext>).UserLogins(filter).Select(x => Mapper.Map<SxStatisticUserLogin, VMStatisticUserLogin>(x)).ToArray();

            return PartialView(MVC.Statistics.Views._UserLoginsGridView, viewModel);
        }
    }
}