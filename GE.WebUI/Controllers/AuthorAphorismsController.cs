using GE.WebUI.Infrastructure.Repositories;
using GE.WebUI.Models;
using GE.WebUI.ViewModels;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace GE.WebUI.Controllers
{
    public sealed class AuthorAphorismsController : BaseController
    {
        private static RepoAuthorAphorism _repo=new RepoAuthorAphorism();
        public static RepoAuthorAphorism Repo
        {
            get { return _repo; }
            set { _repo = value; }
        }

        [HttpGet]
        public async Task<ActionResult> Details(string titleUrl)
        {
            var data = await Repo.GetByTitleUrlAsync(titleUrl);
            if (data == null) return new HttpNotFoundResult();

            var viewModel = Mapper.Map<AuthorAphorism, VMAuthorAphorism>(data);
            return View(viewModel);
        }
    }
}