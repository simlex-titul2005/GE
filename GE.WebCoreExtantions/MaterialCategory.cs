using SX.WebCore;

namespace GE.WebCoreExtantions
{
    public class MaterialCategory : SxMaterialCategory
    {
        public virtual Game Game { get; set; }
        public int? GameId { get; set; }
    }
}
