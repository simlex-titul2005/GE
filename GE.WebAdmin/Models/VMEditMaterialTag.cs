using System.ComponentModel.DataAnnotations;
using static SX.WebCore.Enums;

namespace GE.WebAdmin.Models
{
    public sealed class VMEditMaterialTag
    {
        [MaxLength(128), Display(Name="Значение"), Required]
        public string Id { get; set; }

        public int MaterialId { get; set; }

        public ModelCoreType ModelCoreType { get; set; }
    }
}