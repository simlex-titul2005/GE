using GE.WebAdmin.Models;
using GE.WebCoreExtantions;
using GE.WebCoreExtantions.Repositories;
using SX.WebCore.Abstract;
using System.Linq;
using System.Web.Mvc;

namespace GE.WebAdmin.Controllers
{
    [Authorize(Roles = "admin")]
    public partial class AphorismsController : BaseController
    {
        private SxDbRepository<int, Aphorism, DbContext> _repo;
        public AphorismsController()
        {
            _repo = new RepoAphorism();
        }

        [HttpGet, ChildActionOnly]
        public virtual PartialViewResult Categories()
        {
            var viewModel = (_repo as RepoAphorism).Categories;
            return PartialView(MVC.Aphorisms.Views._Categories, viewModel);
        }

        [HttpGet]
        public virtual ViewResult Index()
        {
            return View();
        }

        [HttpGet]
        public virtual ViewResult Edit(int? id = null)
        {
            var model = id.HasValue ? _repo.GetByKey(id) : new Aphorism();
            var viewModel = Mapper.Map<Aphorism, VMEditAphorism>(model);
            return View(viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public virtual ActionResult Edit(VMEditAphorism model)
        {
            if(ModelState.IsValid)
            {
                if (model.Id == 0)
                    _repo.Create(Mapper.Map<VMEditAphorism, Aphorism>(model));
                else
                    _repo.Update(Mapper.Map<VMEditAphorism, Aphorism>(model), true, "Category", "Author", "Html");
                return RedirectToAction(MVC.Aphorisms.Index());
            }
            return View(model);
        }
    }
}