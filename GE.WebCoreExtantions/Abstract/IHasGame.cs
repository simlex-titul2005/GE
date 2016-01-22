using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GE.WebCoreExtantions.Abstract
{
    public interface IHasGame
    {
        Game Game { get; set; }
        int? GameId { get; set; }
    }
}
