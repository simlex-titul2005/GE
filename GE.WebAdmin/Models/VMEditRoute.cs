using SX.WebCore.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GE.WebAdmin.Models
{
    public sealed class VMEditRoute : ISxViewModel<Guid>
    {
        public Guid Id { get; set; }

        [Required, MaxLength(100), Display(Name = "Имя маршрута")]
        public string Name { get; set; }

        [MaxLength(100), Display(Name = "Домен")]
        public string Domain { get; set; }

        [Required, MaxLength(100), Display(Name = "Контроллер")]
        public string Controller { get; set; }

        [Required, MaxLength(100), Display(Name = "Действие")]
        public string Action { get; set; }

        public VMRouteValue[] Values { get; set; }
    }
}