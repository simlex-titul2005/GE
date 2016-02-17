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
        public string Caption { get; set; }
        public string Description { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string ImgFormat { get; set; }
    }
}