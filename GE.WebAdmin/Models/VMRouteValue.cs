using System;
using System.ComponentModel.DataAnnotations;

namespace GE.WebAdmin.Models
{
    public sealed class VMRouteValue
    {
        public Guid Id { get; set; }
        public Guid RouteId { get; set; }

        [Required, MaxLength(100), Display(Name="Значение параметра")]
        public string Value { get; set; }
    }
}