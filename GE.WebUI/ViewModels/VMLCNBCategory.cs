using SX.WebCore.ViewModels;
using System;

namespace GE.WebUI.ViewModels
{
    public sealed class VMLCNBCategory
    {
        public VMLCNBCategory()
        {
            News = new VMLCNBNews[0];
            SubCategories = new VMLCNBCategory[0];
            Tags = new SxVMMaterialTag[0];
        }

        public string Id { get; set; }
        public string Title { get; set; }
        public Guid? FrontPictureId { get; set; }

        public VMLCNBNews[] News { get; set; }
        public VMLCNBCategory[] SubCategories { get; set; }
        public SxVMMaterialTag[] Tags { get; set; }
    }
}