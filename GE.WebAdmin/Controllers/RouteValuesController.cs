using GE.WebAdmin.Models;
using GE.WebCoreExtantions;
using GE.WebCoreExtantions.Repositories;
using SX.WebCore;
using SX.WebCore.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GE.WebAdmin.Controllers
{
    public partial class RouteValuesController : BaseController
    {
        private SxDbRepository<Guid, SxRouteValue, DbContext> _repo;
        public RouteValuesController()
        {
            _repo = new RepoRouteValue();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public virtual ViewResult Edit(Guid routeId, Guid? id=null)
        {
            var model = id.HasValue ? _repo.GetByKey(id) : new SxRouteValue { RouteId = routeId };
            var viewModel = Mapper.Map<SxRouteValue, VMEditRouteValue>(model);
            return View(viewModel);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Edit(VMEditRouteValue model)
        {
            var redactModel = Mapper.Map<VMEditRouteValue, SxRouteValue>(model);
            if (ModelState.IsValid)
            {
                SxRouteValue newModel = null;
                if (model.Id == Guid.Empty)
                    newModel = _repo.Create(redactModel);
                else
                    newModel = _repo.Update(redactModel, "Name", "Value");
                return RedirectToAction("Edit", "Routes", new { @id = newModel.RouteId });
            }
            else
                return View(model);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public virtual PartialViewResult Delete(VMRouteValue model)
        {
            _repo.Delete(model.Id);
            var viewModel = new VMRouteValueList { RouteId = model.RouteId };
            viewModel.Values = _repo.All.Where(x => x.RouteId == model.RouteId).Select(x => Mapper.Map<SxRouteValue, VMRouteValue>(x)).ToArray();
            return PartialView(MVC.RouteValues.Views._RouteValues, viewModel);
        }
    }
}