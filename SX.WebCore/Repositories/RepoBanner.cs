using SX.WebCore.Abstract;
using System;

namespace SX.WebCore.Repositories
{
    public sealed class RepoBanner<TDbContext> : SxDbRepository<Guid, SxBanner, TDbContext> where TDbContext : SxDbContext
    {
        
    }
}
