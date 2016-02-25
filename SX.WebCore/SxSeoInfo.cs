﻿using SX.WebCore.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SX.WebCore
{
    [Table("D_SEO_INFO")]
    public class SxSeoInfo : SxDbUpdatedModel<int>
    {
        [MaxLength(255), Required]
        public string SeoTitle { get; set; }

        [MaxLength(1000)]
        public string SeoDescription { get; set; }

        public virtual ICollection<SxSeoKeyWord> SeoKeyWords { get; set; }

        public SxMaterial Material { get; set; }
        public int MaterialId { get; set; }

        public Enums.ModelCoreType ModelCoreType { get; set; }
    }
}
