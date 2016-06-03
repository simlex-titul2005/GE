using GE.WebAdmin.Models;
using GE.WebCoreExtantions;
using SX.WebCore;
using SX.WebCore.Abstract;
using SX.WebCore.Repositories;
using SX.WebCore.Resources;
using System.Text;
using System.Web.Mvc;

namespace GE.WebAdmin.Controllers
{
    public partial class SettingsController : Controller
    {
        private const string __notSetSettingValue = "Настройка не определена";
        private SxDbRepository<string, SxSiteSetting, DbContext> _repo;
        public SettingsController()
        {
            _repo = new RepoSiteSetting<DbContext>();
        }

        #region Начальная иконка
        [Authorize(Roles = "admin")]
        [AcceptVerbs(HttpVerbs.Get)]
        public virtual ViewResult EditEmptyGame()
        {
            var settings = (_repo as RepoSiteSetting<DbContext>).GetByKeys(
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
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        public virtual ActionResult EditEmptyGame(VMEditEmptyGameSettings model)
        {
            if (ModelState.IsValid)
            {
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
                        _repo.Create(settings[i]);
                    }

                    TempData["EditEmptyGameMessage"] = "Настройки успешно сохранены";
                    return RedirectToAction(MVC.Settings.EditEmptyGame());
                }
                else if (isExists && isModified)
                {
                    _repo.Update(new SxSiteSetting { Id = Settings.emptyGameIconPath, Value = model.IconPath }, true, "Value");
                    _repo.Update(new SxSiteSetting { Id = Settings.emptyGameGoodImagePath, Value = model.GoodImagePath }, true, "Value");
                    _repo.Update(new SxSiteSetting { Id = Settings.emptyGameBadImagePath, Value = model.BadImagePath }, true, "Value");
                    TempData["EditEmptyGameMessage"] = "Настройки успешно обновлены";
                    return RedirectToAction(MVC.Settings.EditEmptyGame());
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

        #region Настройки сайта
        [Authorize(Roles = "admin")]
        [AcceptVerbs(HttpVerbs.Get)]
        public virtual ViewResult EditSite()
        {

            var settings = (_repo as RepoSiteSetting<DbContext>).GetByKeys(
                    Settings.siteDomain,
                    Settings.siteLogoPath,
                    Settings.siteName,
                    Settings.siteBgPath,
                    Settings.siteFaveiconPath
                );

            var viewModel = new VMSiteSettings
            {
                SiteDomain= settings.ContainsKey(Settings.siteDomain) ? settings[Settings.siteDomain].Value : null,
                LogoPath = settings.ContainsKey(Settings.siteLogoPath) ? settings[Settings.siteLogoPath].Value : null,
                SiteName = settings.ContainsKey(Settings.siteName) ? settings[Settings.siteName].Value : null,
                SiteBgPath = settings.ContainsKey(Settings.siteBgPath) ? settings[Settings.siteBgPath].Value : null,
                SiteFaveiconPath = settings.ContainsKey(Settings.siteFaveiconPath) ? settings[Settings.siteFaveiconPath].Value : null,
            };

            viewModel.OldSiteDomain = viewModel.SiteDomain;
            viewModel.OldLogoPath = viewModel.LogoPath;
            viewModel.OldSiteName = viewModel.SiteName;
            viewModel.OldSiteBgPath = viewModel.SiteBgPath;
            viewModel.OldSiteFaveiconPath = viewModel.SiteFaveiconPath;
            return View(viewModel);
        }

        [Authorize(Roles = "admin"), HttpPost, ValidateAntiForgeryToken]
        public virtual ActionResult EditSite(VMSiteSettings model)
        {
            if (ModelState.IsValid)
            {
                var isExists = !string.IsNullOrEmpty(model.OldSiteDomain) || !string.IsNullOrEmpty(model.OldLogoPath) || !string.IsNullOrEmpty(model.OldSiteName) || !string.IsNullOrEmpty(model.OldSiteBgPath) || !string.IsNullOrEmpty(model.OldSiteFaveiconPath);
                var isModified = !Equals(model.SiteDomain, model.OldSiteDomain) || !Equals(model.LogoPath, model.OldLogoPath) || !Equals(model.SiteName, model.OldSiteName) || !Equals(model.SiteBgPath, model.OldSiteBgPath) || !Equals(model.SiteFaveiconPath, model.OldSiteFaveiconPath);

                if (!isExists)
                {
                    var settings = new SxSiteSetting[] {
                        new SxSiteSetting { Id = Settings.siteDomain, Value = model.SiteDomain },
                        new SxSiteSetting { Id = Settings.siteLogoPath, Value = model.LogoPath },
                        new SxSiteSetting { Id = Settings.siteName, Value = model.SiteName },
                        new SxSiteSetting { Id = Settings.siteBgPath, Value = model.SiteBgPath },
                        new SxSiteSetting { Id = Settings.siteFaveiconPath, Value = model.SiteFaveiconPath }
                    };
                    for (int i = 0; i < settings.Length; i++)
                    {
                        var setting = settings[i];
                        _repo.Create(setting);
                    }

                    TempData["EditEmptyGameMessage"] = "Настройки успешно сохранены";
                    MvcApplication.SiteDomain = model.SiteDomain;
                    return RedirectToAction(MVC.Settings.EditSite());
                }
                else if (isExists && isModified)
                {
                    _repo.Update(new SxSiteSetting { Id = Settings.siteDomain, Value = model.SiteDomain }, true, "Value");

                    _repo.Update(new SxSiteSetting { Id = Settings.siteLogoPath, Value = model.LogoPath }, true, "Value");
                    _repo.Update(new SxSiteSetting { Id = Settings.siteName, Value = model.SiteName }, true, "Value");
                    _repo.Update(new SxSiteSetting { Id = Settings.siteBgPath, Value = model.SiteBgPath }, true, "Value");
                    _repo.Update(new SxSiteSetting { Id = Settings.siteFaveiconPath, Value = model.SiteFaveiconPath }, true, "Value");
                    TempData["EditEmptyGameMessage"] = "Настройки успешно обновлены";
                    MvcApplication.SiteDomain = model.SiteDomain;
                    return RedirectToAction(MVC.Settings.EditSite());
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

        #region Robots.txt
        [Authorize(Roles = "seo")]
        [AcceptVerbs(HttpVerbs.Get)]
        public virtual ViewResult EditRobotsFile()
        {
            var settings = (_repo as RepoSiteSetting<DbContext>).GetByKeys(
                    Settings.robotsFileSetting
                );

            var viewModel = new VMRobotsFile
            {
                FileContent = settings.ContainsKey(Settings.robotsFileSetting) ? settings[Settings.robotsFileSetting].Value : null,
            };
            viewModel.OldFileContent = viewModel.FileContent;
            return View(viewModel);
        }

        [Authorize(Roles = "seo")]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        public virtual ActionResult EditRobotsFile(VMRobotsFile model)
        {
            if (ModelState.IsValid)
            {
                var isExists = !string.IsNullOrEmpty(model.OldFileContent);
                var isModified = !Equals(model.FileContent, model.OldFileContent);

                if (!isExists)
                {
                    var settings = new SxSiteSetting[] {
                        new SxSiteSetting { Id = Settings.robotsFileSetting, Value = model.FileContent }
                    };
                    for (int i = 0; i < settings.Length; i++)
                    {
                        _repo.Create(settings[i]);
                    }

                    TempData["EditEmptyGameMessage"] = "Настройки успешно сохранены";
                    return RedirectToAction(MVC.Settings.EditRobotsFile());
                }
                else if (isExists && isModified)
                {
                    _repo.Update(new SxSiteSetting { Id = Settings.robotsFileSetting, Value = model.FileContent }, true, "Value");
                    TempData["EditEmptyGameMessage"] = "Настройки успешно обновлены";
                    return RedirectToAction(MVC.Settings.EditRobotsFile());
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

        #region ReleaseNotes.txt
        public virtual FileResult ReleaseNotes()
        {
            int pageCode = 1251;
            Encoding encoding = Encoding.GetEncoding(pageCode);
            var file = Files.ReleaseNotes;
            byte[] encodedBytes = encoding.GetBytes(file);
            
            return File(encodedBytes, "txt");
        }
        #endregion
    }
}