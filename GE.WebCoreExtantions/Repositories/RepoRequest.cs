using SX.WebCore;
using SX.WebCore.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GE.WebCoreExtantions.Repositories
{
    public sealed class RepoRequest : SxDbRepository<Guid, SxRequest, DbContext>
    {
        public bool Exists(SxRequest request)
        {
            var result = base.All.FirstOrDefault(x => 
                x.SessionId==request.SessionId
                && x.Controller==request.Controller
                && x.Action==request.Action
                && x.RequestType==request.RequestType
                && x.QueryString==request.QueryString
                );
            return result != null;
        }
    }
}
