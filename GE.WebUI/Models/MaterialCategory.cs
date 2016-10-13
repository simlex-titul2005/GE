using SX.WebCore.DbModels;

namespace GE.WebUI.Models
{
    public class MaterialCategory : SxMaterialCategory
    {
        public virtual Game Game { get; set; }
        public int? GameId { get; set; }
    }
}