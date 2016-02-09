using SX.WebCore.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GE.WebAdmin.Models
{
    public sealed class VMEditMenu : ISxViewModel<int>
    {
        public VMEditMenu()
        {
            Items = new VMMenuItem[0];
        }

        public int Id { get; set; }

        [Required, MaxLength(100), Display(Name="Название меню")]
        public string Name { get; set; }

        [MaxLength(500), Display(Name="Описание"), DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public VMMenuItem[] Items { get; set; }
    }
}