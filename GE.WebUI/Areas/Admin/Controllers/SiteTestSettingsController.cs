using GE.WebUI.Infrastructure.Repositories;
using GE.WebUI.Models;
using GE.WebUI.ViewModels;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace GE.WebUI.Areas.Admin.Controllers
{
    [Authorize(Roles="admin")]
    public sealed class SiteTestSettingsController : BaseController
    {
        private static RepoSiteTestSetting _repo = new RepoSiteTestSetting();
        public static RepoSiteTestSetting Repo { get { return _repo; } }

        [HttpGet]
        public ActionResult Edit(int testId)
        {
            var data = Repo.GetByKey(testId);
            var viewModel = data==null? new VMSiteTestSetting(): Mapper.Map<SiteTestSetting, VMSiteTestSetting>(data);

            return PartialView("_Edit", viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(VMSiteTestSetting model)
        {
            if(ModelState.IsValid)
            {
                var data = Mapper.Map<VMSiteTestSetting, SiteTestSetting>(model);

                await Repo.UpdateAsync(data);

                return JavaScript("alert('Настройки успешно сохранены')");
            }

            return JavaScript("alert('Ошибка сохранения настроек!')");
        }
    }
}