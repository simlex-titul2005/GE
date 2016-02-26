using SX.WebCore.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GE.WebAdmin.Models
{
    public sealed class VMEditMenuItem : ISxViewModel<int>
    {
        public int Id { get; set; }

        public int MenuId { get; set; }

        [Display(Name = "Маршрут")]
        public Guid? RouteId { get; set; }

        [Required, MaxLength(100), Display(Name="Название пункта меню")]
        public string Caption { get; set; }

        [MaxLength(150), Display(Name = "Подсказка пункта")]
        public string Title { get; set; }
    }
}