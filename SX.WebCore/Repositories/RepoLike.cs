using SX.WebCore.Abstract;
using System.Data.SqlClient;
using Dapper;
using System.Linq;

namespace SX.WebCore.Repositories
{
    public sealed class RepoLike<TDbContext> : SxDbRepository<int, SxLike, TDbContext> where TDbContext : SxDbContext
    {
        /// <summary>
        /// Лайкнуть материал
        /// </summary>
        /// <param name="like">Лайк</param>
        /// <returns>Если лайк уже существует (по mid, mct, sid, uid, dir), то вернет -1, в остальных случаях - кол-во лайков дпо направлению (положительный, отрицательный)</returns>
        public int CreateLike(SxLike like)
        {
            var existLike = GetByKey(like);
            if (existLike != null) return -1;

            return Create(like).Count;
        }

        public sealed override SxLike GetByKey(params object[] id)
        {
            var like = (SxLike)id[0];

            var query = @"SELECT dl.Id
FROM   D_LIKE AS dl
WHERE  dl.MaterialId = @mid
       AND dl.ModelCoreType = @mct
       AND (
               dl.SessionId = @sid
               OR (dl.SessionId <> @sid AND dl.UserId = @uid)
           )
       AND (dl.UserId = @uid OR @uid IS NULL)
       AND dl.Direction = @dir";

            using (var connection = new SqlConnection(ConnectionString))
            {
                var data = connection.Query<SxLike>(query, new { mid = like.MaterialId, mct = like.ModelCoreType, sid = like.SessionId, uid = like.UserId, dir = like.Direction }).SingleOrDefault();
                return data;
            }
        }

        public sealed override SxLike Create(SxLike model)
        {
            return base.Create(model);
        }
    }
}
