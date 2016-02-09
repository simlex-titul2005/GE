using SX.WebCore.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GE.WebAdmin.Models
{
    public sealed class VMRouteValue : ISxViewModel<Guid>
    {
        public Guid Id { get; set; }
        public Guid RouteId { get; set; }

        [Required, MaxLength(100), Display(Name="Значение параметра")]
        public string Value { get; set; }
    }
}