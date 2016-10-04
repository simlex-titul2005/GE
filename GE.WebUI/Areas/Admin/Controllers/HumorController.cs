using GE.WebUI.Models;
using GE.WebUI.ViewModels;
using SX.WebCore;
using SX.WebCore.Repositories;
using GE.WebUI.Infrastructure.Repositories;
using System.Web.Mvc;
using System.Linq;

namespace GE.WebUI.Areas.Admin.Controllers
{
    [Authorize(Roles ="author-article")]
    public sealed class HumorController : MaterialsController<Humor, VMHumor>
    {
        private static RepoHumor _repo = new RepoHumor();
        public HumorController() : base(Enums.ModelCoreType.Humor) { }
        public override SxRepoMaterial<Humor, VMHumor> Repo
        {
            get
            {
                return _repo;
            }
        }

        protected override string[] PropsForUpdate
        {
            get
            {
                return base.PropsForUpdate.Union(new string[] {
                    "UserName"
                }).ToArray();
            }
        }
    }
}