using SX.WebCore.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GE.WebAdmin.Models
{
    public sealed class VMPicture : ISxViewModel<Guid>
    {
        public Guid Id { get; set; }
        public int Width { get; set; }
    }
}