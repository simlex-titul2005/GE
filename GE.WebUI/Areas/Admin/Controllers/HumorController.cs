﻿using GE.WebUI.Models;
using GE.WebUI.ViewModels;
using SX.WebCore.SxRepositories;
using GE.WebUI.Infrastructure.Repositories;
using System.Web.Mvc;
using System.Linq;

namespace GE.WebUI.Areas.Admin.Controllers
{
    [Authorize(Roles ="author-article")]
    public sealed class HumorController : MaterialsController<Humor, VMHumor>
    {
        private static RepoHumor _repo = new RepoHumor();
        public HumorController() : base(MvcApplication.ModelCoreTypeProvider[nameof(Humor)]) { }
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