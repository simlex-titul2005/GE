using System;
using System.Linq;
using static SX.WebCore.Enums;

namespace GE.WebAdmin.Models
{
    public sealed class VMMaterialCategory
    {
        public VMMaterialCategory()
        {
            ChildCategories = new VMMaterialCategory[0];
        }

        public string Id { get; set; }
        public ModelCoreType ModelCoreType { get; set; }
        public string ParentCategoryId { get; set; }
        public string Title { get; set; }
        public VMMaterialCategory[] ChildCategories { get; set; }
        public bool HasChildGroups
        {
            get
            {
                return ChildCategories.Any();
            }
        }
        public int Level { get; set; }
        public Guid? FrontPictureId { get; set; }

        public VMGame Game { get; set; }
        public int? GameId { get; set; }
    }
}