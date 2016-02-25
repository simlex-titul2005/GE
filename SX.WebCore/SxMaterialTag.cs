using SX.WebCore.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SX.WebCore
{
    [Table("D_MATERIAL_TAG")]
    public class SxMaterialTag : SxDbUpdatedModel<int>
    {
        [MaxLength(100), Required]
        public string Value { get; set; }

        public SxMaterial Material { get; set; }
        
        public int? MaterialId { get; set; }

        public Enums.ModelCoreType ModelCoreType { get; set; }
    }
}
