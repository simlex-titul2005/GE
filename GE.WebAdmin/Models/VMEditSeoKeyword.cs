using SX.WebCore;
using SX.WebCore.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GE.WebAdmin.Models
{
    public sealed class VMEditSeoKeyword : ISxViewModel<int>
    {
        public int Id { get; set; }

        public VMSeoInfo SeoInfo { get; set; }

        [Required]
        public int SeoInfoId { get; set; }

        [MaxLength(50), Required, Display(Name="Значение ключевого слова")]
        public string Value { get; set; }
    }
}