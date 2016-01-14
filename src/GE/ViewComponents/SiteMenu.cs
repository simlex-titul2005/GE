using GE.ViewModels;
using Microsoft.AspNet.Mvc;
using System.Collections.Generic;

namespace GE.ViewComponents
{
    public class SiteMenu : ViewComponent
    {
        private readonly VMSiteMenuItem __brandItem = new VMSiteMenuItem { Url = "/", Title = "Главная" };
        private VMSiteMenu _menu;
        public SiteMenu()
        {
            _menu = new VMSiteMenu() { Items = new List<VMSiteMenuItem>(), AuthBlock = new VMAuthBlock { MenuItems=new List<VMSiteMenuItem>()} };
            _menu.BrandItem = __brandItem;
            _menu.Items.Add(new VMSiteMenuItem { Url = "/news/list", Title = "Новости", OrderIndex = 0 });
            _menu.Items.Add(new VMSiteMenuItem { Url = "/articles/list", Title = "Статьи", OrderIndex = 1 });
            _menu.Items.Add(new VMSiteMenuItem { Url = "/contests/list", Title = "Конкурсы", OrderIndex = 2 });
            _menu.Items.Add(new VMSiteMenuItem { Url = "/forum/list", Title = "Форум", OrderIndex = 3 });

            _menu.AuthBlock.MenuItems.Add(new VMSiteMenuItem { Url = "/account/login", Title = "Вход", OrderIndex = 0 });
            _menu.AuthBlock.MenuItems.Add(new VMSiteMenuItem { Url = "/account/register", Title = "Регистрация", OrderIndex = 1 });
        }

        public IViewComponentResult Invoke()
        {
            return View(_menu);
        }
    }
}
