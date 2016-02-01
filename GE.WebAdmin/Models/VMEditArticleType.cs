using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GE.WebAdmin.Models
{
    public sealed class VMEditArticleType : SX.WebCore.Abstract.ISxViewModel
    {
        public int Id { get; set; }
        public DateTime DateCreate { get; set; }
        public string Name { get; set; }
        public int GameId { get; set; }
    }
}