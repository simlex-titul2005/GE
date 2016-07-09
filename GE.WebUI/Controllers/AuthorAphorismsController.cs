using GE.WebCoreExtantions;
using GE.WebCoreExtantions.Repositories;
using GE.WebUI.Models;
using System.Web.Mvc;

namespace GE.WebUI.Controllers
{
    public sealed class AuthorAphorismsController : BaseController
    {
        private static RepoAuthorAphorism _repo;
        public AuthorAphorismsController()
        {
            if (_repo == null)
                _repo = new RepoAuthorAphorism();
        }

        [HttpGet]
        public ActionResult Details(string titleUrl)
        {
            var data = _repo.GetByTitleUrl(titleUrl);
            if (data == null) return new HttpNotFoundResult();

            var viewModel = Mapper.Map<AuthorAphorism, VMAuthorAphorism>(data);
            return View(viewModel);
        }
    }
}