using GE.WebAdmin.Models;
using GE.WebCoreExtantions;
using GE.WebCoreExtantions.Repositories;
using SX.WebCore;
using SX.WebCore.Abstract;
using System;
using System.Collections.Generic;
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

        private const string __emptyGameIconPath = "emptyGameIconPath";
        private const string __emptyGameGoodImagePath = "emptyGameGoodImagePath";
        private const string __emptyGameBadImagePath = "emptyGameBadImagePath";
        [AcceptVerbs(HttpVerbs.Get)]
        public virtual ViewResult EditEmptyGame()
        {
            var emptyGameIconPath = _repo.GetByKey(__emptyGameIconPath);
            var emptyGameGoodImagePath = _repo.GetByKey(__emptyGameGoodImagePath);
            var emptyGameBadImagePath = _repo.GetByKey(__emptyGameBadImagePath);

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
            if(ModelState.IsValid)
            {
                var isExists=!string.IsNullOrEmpty(model.OldIconPath) || !string.IsNullOrEmpty(model.OldGoodImagePath) || !string.IsNullOrEmpty(model.OldBadImagePath);
                var isModified = !Equals(model.IconPath, model.OldIconPath) || !Equals(model.GoodImagePath, model.OldGoodImagePath) || !Equals(model.BadImagePath, model.OldBadImagePath);
                
                if(!isExists)
                {
                    var settings = new SxSiteSetting[] {
                        new SxSiteSetting { Id = __emptyGameIconPath, Value = model.IconPath },
                        new SxSiteSetting { Id = __emptyGameGoodImagePath, Value = model.GoodImagePath },
                        new SxSiteSetting { Id = __emptyGameBadImagePath, Value = model.BadImagePath }
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
                    _repo.Update(new SxSiteSetting { Id = __emptyGameIconPath, Value = model.IconPath }, "Value");
                    _repo.Update(new SxSiteSetting { Id = __emptyGameGoodImagePath, Value = model.GoodImagePath }, "Value");
                    _repo.Update(new SxSiteSetting { Id = __emptyGameBadImagePath, Value = model.BadImagePath }, "Value");
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
    }
}