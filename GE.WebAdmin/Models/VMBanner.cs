using SX.WebCore;
using System;
using static SX.WebCore.Enums;

namespace GE.WebAdmin.Models
{
    public sealed class VMBanner
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Url { get; set; }

        public Guid PictureId { get; set; }

        public Guid? BannerGroupId { get; set; }

        public int? MaterialId { get; set; }
        public ModelCoreType? ModelCoreType { get; set; }

        public SxBanner.BannerPlace Place { get; set; }
    }
}