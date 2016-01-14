using GE.ViewModels;
using Microsoft.AspNet.Mvc;
using System.Collections.Generic;

namespace GE.ViewComponents
{
    public class GameMenu : ViewComponent
    {
        private VMGameMenu _gameMenu;
        public GameMenu()
        {
            _gameMenu = new VMGameMenu {
                BadGame = new VMGameMenuImg(),
                GoodGame=new VMGameMenuImg(),
                CurrentGame=new VMGameMenuGame(),
                Games=new List<VMGameMenuGame>()
            };
        }

        public IViewComponentResult Invoke()
        {
            return View(_gameMenu);
        }
    }
}
