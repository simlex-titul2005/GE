using GE.WebUI.Infrastructure.Repositories;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace GE.WebUI.Controllers
{
    public sealed class InfographicsController : BaseController
    {
        public static RepoInfographic Repo { get; set; } = new RepoInfographic();

        [HttpGet]
        public async Task<ActionResult> Details(Guid id)
        {
            var viewModel = await Repo.GetByKeyAsync(new object[] { id });
            if (viewModel == null) return new HttpNotFoundResult();

            return View(model: viewModel);
        }
    }
}