using GE.WebCoreExtantions;
using SX.WebCore;
using SX.WebCore.Abstract;
using SX.WebCore.Repositories;
using System;
using System.Web.Mvc;

namespace GE.WebUI.Controllers
{
    public class VideosController : BaseController
    {
        private SxDbRepository<Guid, SxVideo, DbContext> _repo;

        public VideosController()
        {
            _repo = new RepoVideo<DbContext>();
        }

        [HttpPost]
        public EmptyResult AddView(Guid videoId)
        {
            (_repo as RepoVideo<DbContext>).AddView(videoId);
            return new EmptyResult();
        }
    }
}