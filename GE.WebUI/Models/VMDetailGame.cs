using SX.WebCore;
using System;
using System.Linq;
using static SX.WebCore.Enums;

namespace GE.WebUI.Models
{
    public sealed class VMDetailGame
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string TitleUrl { get; set; }

        public string FullDescription { get; set; }

        public Guid? FrontPictureId { get; set; }

        public VMDetailGameMaterial[] Materials { get; set; }
        public VMDetailGameMaterial[] GetMaterialsByCoreType(ModelCoreType mct)
        {
            var data = Materials == null || !Materials.Any()
                ? new VMDetailGameMaterial[0]
                : Materials.Where(x => x.ModelCoreType == mct).ToArray();

            return data;
        }

        public SxVideo[] Videos { get; set; }
    }
}