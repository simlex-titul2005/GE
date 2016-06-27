using GE.WebAdmin.Models;
using GE.WebCoreExtantions;
using SX.WebCore;
using SX.WebCore.Repositories;
using SX.WebCore.Resources;
using System.Web.Mvc;

namespace GE.WebAdmin.Controllers
{
    public partial class SiteSettingsController : SX.WebCore.MvcControllers.SxSiteSettingsController<WebCoreExtantions.DbContext>
    {
        #region Начальная иконка
        [Authorize(Roles = "admin")]
        [HttpGet]
        public ViewResult EditEmptyGame()
        {
            var settings = new SxRepoSiteSetting<DbContext>().GetByKeys(
                    Settings.emptyGameIconPath,
                    Settings.emptyGameGoodImagePath,
                    Settings.emptyGameBadImagePath
                );

            var viewModel = new VMEditEmptyGameSettings
            {
                IconPath = settings.ContainsKey(Settings.emptyGameIconPath) ? settings[Settings.emptyGameIconPath].Value : null,
                GoodImagePath = settings.ContainsKey(Settings.emptyGameGoodImagePath) ? settings[Settings.emptyGameGoodImagePath].Value : null,
                BadImagePath = settings.ContainsKey(Settings.emptyGameBadImagePath) ? settings[Settings.emptyGameBadImagePath].Value : null,
            };

            viewModel.OldIconPath = viewModel.IconPath;
            viewModel.OldGoodImagePath = viewModel.GoodImagePath;
            viewModel.OldBadImagePath = viewModel.BadImagePath;

            return View(viewModel);
        }

        [Authorize(Roles = "admin")]
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult EditEmptyGame(VMEditEmptyGameSettings model)
        {
            if (ModelState.IsValid)
            {
                var repo = new SxRepoSiteSetting<DbContext>();
                var isExists = !string.IsNullOrEmpty(model.OldIconPath) || !string.IsNullOrEmpty(model.OldGoodImagePath) || !string.IsNullOrEmpty(model.OldBadImagePath);
                var isModified = !Equals(model.IconPath, model.OldIconPath) || !Equals(model.GoodImagePath, model.OldGoodImagePath) || !Equals(model.BadImagePath, model.OldBadImagePath);

                if (!isExists)
                {
                    var settings = new SxSiteSetting[] {
                        new SxSiteSetting { Id = Settings.emptyGameIconPath, Value = model.IconPath },
                        new SxSiteSetting { Id = Settings.emptyGameGoodImagePath, Value = model.GoodImagePath },
                        new SxSiteSetting { Id = Settings.emptyGameBadImagePath, Value = model.BadImagePath }
                    };
                    for (int i = 0; i < settings.Length; i++)
                    {
                        repo.Create(settings[i]);
                    }

                    TempData["EditEmptyGameMessage"] = "Настройки успешно сохранены";
                    return RedirectToAction("EditEmptyGame", new { controller = "sitesettings" });
                }
                else if (isExists && isModified)
                {
                    repo.Update(new SxSiteSetting { Id = Settings.emptyGameIconPath, Value = model.IconPath }, true, "Value");
                    repo.Update(new SxSiteSetting { Id = Settings.emptyGameGoodImagePath, Value = model.GoodImagePath }, true, "Value");
                    repo.Update(new SxSiteSetting { Id = Settings.emptyGameBadImagePath, Value = model.BadImagePath }, true, "Value");
                    TempData["EditEmptyGameMessage"] = "Настройки успешно обновлены";
                    return RedirectToAction("EditEmptyGame", new { controller = "sitesettings" });
                }
                else
                {
                    TempData["EditEmptyGameMessage"] = "В настройках нет изменений";
                    return View(model);
                }
            }

            return View(model);
        }
        #endregion
    }
}