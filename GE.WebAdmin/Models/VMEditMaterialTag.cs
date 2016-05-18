using SX.WebCore.Attrubutes;
using System.ComponentModel.DataAnnotations;
using static SX.WebCore.Enums;

namespace GE.WebAdmin.Models
{
    public sealed class VMEditMaterialTag
    {
        [MaxLength(128), Display(Name="Значение"), Required, MaxWordsCount(4)]
        public string Id { get; set; }

        public int MaterialId { get; set; }

        public ModelCoreType ModelCoreType { get; set; }
    }
}