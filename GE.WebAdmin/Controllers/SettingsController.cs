using GE.WebAdmin.Models;
using GE.WebCoreExtantions;
using GE.WebCoreExtantions.Repositories;
using SX.WebCore;
using SX.WebCore.Abstract;
using SX.WebCore.Resources;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GE.WebAdmin.Controllers
{
    public partial class SettingsController : Controller
    {
        private const string __notSetSettingValue = "Настройка не определена";
        private SxDbRepository<string, SxSiteSetting, DbContext> _repo;
        public SettingsController()
        {
            _repo = new RepoSiteSetting();
        }

        #region Начальная иконка
        [AcceptVerbs(HttpVerbs.Get)]
        public virtual ViewResult EditEmptyGame()
        {
            var emptyGameIconPath = _repo.GetByKey(Settings.emptyGameIconPath);
            var emptyGameGoodImagePath = _repo.GetByKey(Settings.emptyGameGoodImagePath);
            var emptyGameBadImagePath = _repo.GetByKey(Settings.emptyGameBadImagePath);

            var viewModel = new VMEditEmptyGameSettings
            {
                IconPath = emptyGameIconPath != null ? emptyGameIconPath.Value : null,
                GoodImagePath = emptyGameGoodImagePath != null ? emptyGameGoodImagePath.Value : null,
                BadImagePath = emptyGameBadImagePath != null ? emptyGameBadImagePath.Value : null,
            };
            viewModel.OldIconPath = viewModel.IconPath;
            viewModel.OldGoodImagePath = viewModel.GoodImagePath;
            viewModel.OldBadImagePath = viewModel.BadImagePath;

            return View(viewModel);
        }

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
                    _repo.Update(new SxSiteSetting { Id = Settings.emptyGameIconPath, Value = model.IconPath }, "Value");
                    _repo.Update(new SxSiteSetting { Id = Settings.emptyGameGoodImagePath, Value = model.GoodImagePath }, "Value");
                    _repo.Update(new SxSiteSetting { Id = Settings.emptyGameBadImagePath, Value = model.BadImagePath }, "Value");
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
        [AcceptVerbs(HttpVerbs.Get)]
        public virtual ViewResult EditSite()
        {
            var siteLogoPath = _repo.GetByKey(Settings.siteLogoPath);
            var siteName = _repo.GetByKey(Settings.siteName);
            var siteBgPath = _repo.GetByKey(Settings.siteBgPath);
            var viewModel = new VMSiteSettings
            {
                LogoPath = siteLogoPath != null ? siteLogoPath.Value : null,
                SiteName = siteName != null ? siteName.Value : null,
                SiteBgPath = siteBgPath != null ? siteBgPath.Value : null
            };
            viewModel.OldLogoPath = viewModel.LogoPath;
            viewModel.OldSiteName = viewModel.SiteName;
            viewModel.OldSiteBgPath = viewModel.SiteBgPath;
            return View(viewModel);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        public virtual ActionResult EditSite(VMSiteSettings model)
        {
            if (ModelState.IsValid)
            {
                var isExists = !string.IsNullOrEmpty(model.OldLogoPath) || !string.IsNullOrEmpty(model.OldSiteName) || !string.IsNullOrEmpty(model.OldSiteBgPath);
                var isModified = !Equals(model.LogoPath, model.OldLogoPath) || !Equals(model.SiteName, model.OldSiteName) || !Equals(model.SiteBgPath, model.OldSiteBgPath);

                if (!isExists)
                {
                    var settings = new SxSiteSetting[] {
                        new SxSiteSetting { Id = Settings.siteLogoPath, Value = model.LogoPath },
                        new SxSiteSetting { Id = Settings.siteName, Value = model.SiteName },
                        new SxSiteSetting { Id = Settings.siteBgPath, Value = model.SiteBgPath }
                    };
                    for (int i = 0; i < settings.Length; i++)
                    {
                        _repo.Create(settings[i]);
                    }

                    TempData["EditEmptyGameMessage"] = "Настройки успешно сохранены";
                    return RedirectToAction(MVC.Settings.EditSite());
                }
                else if (isExists && isModified)
                {
                    _repo.Update(new SxSiteSetting { Id = Settings.siteLogoPath, Value = model.LogoPath }, "Value");
                    _repo.Update(new SxSiteSetting { Id = Settings.siteName, Value = model.SiteName }, "Value");
                    _repo.Update(new SxSiteSetting { Id = Settings.siteBgPath, Value = model.SiteBgPath }, "Value");
                    TempData["EditEmptyGameMessage"] = "Настройки успешно обновлены";
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

    }
}