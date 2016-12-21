using SX.WebCore.MvcControllers;
using System;
using System.Runtime.Caching;

namespace GE.WebUI.Controllers
{
    public sealed class ValutesController : SxValutesController
    {
        private static CacheItemPolicy _defaultPolicy => new CacheItemPolicy
        {
            AbsoluteExpiration = DateTimeOffset.Now.AddHours(2)
        };
    }
}