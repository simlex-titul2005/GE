using SX.WebCore.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GE.WebAdmin.Models
{
	public sealed class VMRoute : ISxViewModel<Guid>
	{
        public Guid Id { get; set; }

        [Required, MaxLength(100), Display(Name="Имя маршрута")]
        public string Name { get; set; }

        [MaxLength(100), Display(Name = "Домен")]
        public string Domain { get; set; }

        [Required, MaxLength(100), Display(Name = "Контролле")]
        public string Controller { get; set; }

        [Required, MaxLength(100), Display(Name = "Действие")]
        public string Action { get; set; }
	}
}