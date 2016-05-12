using static SX.WebCore.Enums;

namespace GE.WebAdmin.Models
{
    public sealed class VMAphorism
    {
        public int Id { get; set; }
        public ModelCoreType ModelCoreType { get; set; }
        public string Title { get; set; }
    }
}