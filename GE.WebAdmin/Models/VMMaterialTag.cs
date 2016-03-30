using static SX.WebCore.Enums;

namespace GE.WebAdmin.Models
{
    public sealed class VMMaterialTag
    {
        public string Id { get; set; }
        public int MaterialId { get; set; }
        public ModelCoreType ModelCoreType { get; set; }
    }
}