using System;
using System.ComponentModel.DataAnnotations;

namespace GE.WebAdmin.Models
{
    public sealed class VMEditRouteValue
    {
        public Guid Id { get; set; }
        public Guid RouteId { get; set; }

        [Required, MaxLength(100), Display(Name = "Наименование параметра")]
        public string Name { get; set; }

        [Required, MaxLength(100), Display(Name = "Значение параметра")]
        public string Value { get; set; }
    }
}