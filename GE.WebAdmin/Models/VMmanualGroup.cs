using System.Linq;

namespace GE.WebAdmin.Models
{
    public sealed class VMManualGroup
    {
        public VMManualGroup()
        {
            ChildGroups = new VMManualGroup[0];
        }

        public string Id { get; set; }
        public string ParentGroupId { get; set; }
        public string Title { get; set; }
        public VMManualGroup[] ChildGroups { get; set; }
        public bool HasChildGroups
        {
            get
            {
                return ChildGroups.Any();
            }
        }
        public int Level { get; set; }
    }
}