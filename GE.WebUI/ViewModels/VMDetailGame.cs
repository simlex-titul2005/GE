using GE.WebUI.ViewModels.Abstracts;
using SX.WebCore.ViewModels;
using System;
using System.Linq;
using static SX.WebCore.Enums;

namespace GE.WebUI.ViewModels
{
    public sealed class VMDetailGame
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string TitleUrl { get; set; }

        public string FullDescription { get; set; }

        public Guid? FrontPictureId { get; set; }

        public VMMaterial[] Materials { get; set; }
        public VMMaterial[] GetMaterialsByCoreType(ModelCoreType mct)
        {
            var data = Materials == null || !Materials.Any()
                ? new VMMaterial[0]
                : Materials.Where(x => x.ModelCoreType == mct).ToArray();

            return data;
        }

        public SxVMVideo[] Videos { get; set; }
    }
}