namespace GE.WebUI.ViewModels
{
    public sealed class VMGameMenu
    {
        public VMGameMenu(int imgWidth, int iconHeight)
        {
            Games = new VMGameMenuGame[0];
            ImgWidth = imgWidth;
            IconHeight = iconHeight;
            Materials = new VMGameMenuImgGameMaterial[0];
        }

        public VMGameMenuEmptyGame EmptyGame { get; set; }
        public VMGameMenuGame[] Games { get; set; }
        public int Length 
        { 
            get
            {
                return Games.Length;
            }
        }

        public int ImgWidth { get; set; }
        public int IconHeight { get; set; }

        public VMGameMenuImgGame this[string gameName]
        {
            get
            {
                var model = new VMGameMenuImgGame
                {
                    Title="Все записи",
                    GoodPictureUrl=EmptyGame.GoodImagePath,
                    BadPictureUrl=EmptyGame.BadImagePath,
                    IconUrl=EmptyGame.IconPath,
                    ImgWidth=this.ImgWidth,
                    IconHeight=this.IconHeight
                };

                if (string.IsNullOrEmpty(gameName)) return model;

                for (int i = 0; i < Length; i++)
                {
                    var g = Games[i];
                    if (g.TitleUrl.ToLower() == gameName.ToLower())
                    {
                        model = new VMGameMenuImgGame
                        {
                            Title = g.Title,
                            GoodPictureUrl = "/pictures/picture/"+g.GoodPictureId,
                            BadPictureUrl = "/pictures/picture/" + g.BadPictureId,
                            IconUrl = "/pictures/picture/" + g.FrontPictureId
                        };
                        break;
                    }
                }

                model.ImgWidth = this.ImgWidth;
                model.IconHeight = this.IconHeight;
                return model;
            }
        }
        public VMGameMenuImgGameMaterial[] Materials { get; set; }
    }
}