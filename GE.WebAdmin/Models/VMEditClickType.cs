using SX.WebCore.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GE.WebAdmin.Models
{
    public sealed class VMEditClickType : ISxViewModel<int>
    {
        public int Id { get; set; }

        [MaxLength(255), Required, Display(Name = "Наименование типа статистики кликов")]
        public string Name { get; set; }

        [MaxLength(255), Display(Name = "Описание типа статистики кликов")]
        public string Description { get; set; }
    }
}