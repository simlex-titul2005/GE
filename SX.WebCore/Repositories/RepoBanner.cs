using Dapper;
using SX.WebCore.Abstract;
using System;
using System.Data.SqlClient;
using System.Linq;

namespace SX.WebCore.Repositories
{
    public sealed class RepoBanner<TDbContext> : SxDbRepository<Guid, SxBanner, TDbContext> where TDbContext : SxDbContext
    {
        public override IQueryable<SxBanner> All
        {
            get
            {
                using (var conn = new SqlConnection(ConnectionString))
                {
                    var data = conn.Query<SxBanner, SxPicture, SxBanner>("get_banners @id, @place", (b, p) =>
                    {
                        b.PictureId = p.Id;
                        b.Picture = p;
                        return b;
                    }, new {
                        id = (Guid?)null,
                        place = (SxBanner.BannerPlace?)null
                    }, splitOn: "Id");

                    return data.AsQueryable();
                }
            }
        }

        public override SxBanner GetByKey(params object[] id)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<SxBanner, SxPicture, SxBanner>("get_banners @id, @place", (b, p) =>
                {
                    b.PictureId = p.Id;
                    b.Picture = p;
                    return b;
                }, new {
                    id = id[0],
                    place = (SxBanner.BannerPlace?)null
                }, splitOn: "Id").SingleOrDefault();

                return data;
            }
        }

        public override SxBanner Update(SxBanner model, bool changeDateUpdate = true, params string[] propertiesForChange)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Execute("update_banner @id, @url, @pid, @title, @place, @controller, @action", new
                {
                    id = model.Id,
                    url = model.Url,
                    pid = model.PictureId,
                    title = model.Title,
                    place = model.Place,
                    controller = model.ControllerName,
                    action = model.ActionName
                });
            }
            return GetByKey(model.Id);
        }
    }
}
