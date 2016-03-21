using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SX.WebCore.Abstract
{
    public interface ISxVote
    {
        ICollection<SxVote> Votes { get; set; }
    }
}
