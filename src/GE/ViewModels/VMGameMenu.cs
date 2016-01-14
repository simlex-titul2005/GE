using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GE.ViewModels
{
    public class VMGameMenu
    {
        public VMGameMenuGame CurrentGame { get; set; }
        public VMGameMenuImg GoodGame { get; set; }
        public VMGameMenuImg BadGame { get; set; }
        public List<VMGameMenuGame> Games { get; set; }
        public string MenuItemWidth
        {
            get
            {
                var result = Games.Any() ? Math.Round(100 / (decimal)Games.Count, 4) : 0;
                return result + "px";
            }
        }
    }

    public class VMGameMenuGame
    {
        public string PictureImgSrc { get; set; }
        public string Title { get; set; }
    }

    public class VMGameMenuImg
    {
        public string Title { get; set; }
        public string ImgSrc { get; set; }
    }
}
