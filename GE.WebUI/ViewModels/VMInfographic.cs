using SX.WebCore.ViewModels;
using System;

namespace GE.WebUI.ViewModels
{
    public sealed class VMInfographic
    {
        public Guid PictureId { get; set; }
        public SxVMPicture Picture { get; set; }

        public string Caption
        {
            get
            {
                return Picture?.Caption;
            }
            set
            {
                if(!string.IsNullOrEmpty(value))
                {
                    Picture = Picture ?? new SxVMPicture();
                    Picture.Caption = value;
                }
            }
        }

        public int MaterialId { get; set; }

        public byte ModelCoreType { get; set; }
    }
}