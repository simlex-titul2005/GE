using AutoMapper;
using GE.WebAdmin.Models;
using GE.WebCoreExtantions;
using SX.WebCore;
using SX.WebCore.Abstract;
using SX.WebCore.Repositories;
using System.Linq;
using System.Web.Mvc;

namespace GE.WebAdmin.Controllers
{
    public partial class ProjectStepsController : BaseController
    {
        private SxDbRepository<int, SxProjectStep, DbContext> _repo;
        public ProjectStepsController()
        {
            _repo = new RepoProjectStep<DbContext>();
        }

        [HttpGet]
        public virtual ActionResult Index()
        {
            var filter = new WebCoreExtantions.Filter();
            var data = _repo.Query(filter).ToArray();
            var parents = data.Where(x => !x.ParentStepId.HasValue)
                .Select(x => Mapper.Map<SxProjectStep, VMProjectStep>(x)).ToArray();
            if (parents.Any())
            {
                for (int i = 0; i < parents.Length; i++)
                {
                    fillSteps(parents[i], data, Mapper);
                }
            }

            return View(parents);
        }

        private static void fillSteps(VMProjectStep step, SxProjectStep[] steps, IMapper mapper)
        {
            step.Steps = steps.Where(x => x.ParentStepId == step.Id)
                .Select(x => mapper.Map<SxProjectStep, VMProjectStep>(x)).ToArray();
            if (step.Steps.Any())
            {
                for (int i = 0; i < step.Steps.Length; i++)
                {
                    fillSteps(step.Steps[i], steps, mapper);
                }
            }
        }

        [HttpGet]
        public virtual ViewResult Edit(int? id = null, int? pid = null)
        {
            var data = id.HasValue ? _repo.GetByKey(id) : new SxProjectStep { ParentStepId = pid };
            var viewModel = Mapper.Map<SxProjectStep, VMEditProjectStep>(data);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Edit(VMEditProjectStep model)
        {
            var redactModel = Mapper.Map<VMEditProjectStep, SxProjectStep>(model);
            if (ModelState.IsValid)
            {
                if (model.Id == 0)
                    _repo.Create(redactModel);
                else
                    _repo.Update(redactModel, true, "Title", "Foreword", "Html", "ParentStepId");
                return Redirect("/projectsteps/index#pstep-" + model.Id);
            }
            else
                return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual RedirectToRouteResult Delete(VMEditProjectStep model)
        {
            _repo.Delete(model.Id);
            return RedirectToAction(MVC.ProjectSteps.Index());
        }

        [HttpPost]
        public virtual EmptyResult ReplaceOrder(int id, bool dir, int? osid=null)
        {
            (_repo as RepoProjectStep<DbContext>).ReplaceOrder(id, dir, osid);
            return null;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual RedirectResult ReplaceDone(int id, bool done)
        {
            (_repo as RepoProjectStep<DbContext>).ReplaceDone(id, done);
            return Redirect("/projectsteps/index#pstep-" + id);
        }
    }
}