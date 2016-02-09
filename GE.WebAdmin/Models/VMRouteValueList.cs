using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GE.WebAdmin.Models
{
    public sealed class VMRouteValueList
    {
        public Guid RouteId { get; set; }
        public VMRouteValue[] Values { get; set; }
    }
}