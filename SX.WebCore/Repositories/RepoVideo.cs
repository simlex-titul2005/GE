using SX.WebCore.Abstract;
using System;

namespace SX.WebCore.Repositories
{
    public sealed class RepoVideo<TDbContext> : SxDbRepository<Guid, SxVideo, TDbContext> where TDbContext : SxDbContext
    {
        public void AddView(Guid videoId)
        {
            var video = GetByKey(videoId);
            video.ViewsCount++;
            Update(video);
        }
    }
}
