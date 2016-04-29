using Dapper;
using SX.WebCore.Abstract;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SX.WebCore.Repositories
{
    public class RepoStatistic<TDbContext> : SxDbRepository<Guid, SxStatistic, TDbContext> where TDbContext : SxDbContext
    {
        /// <summary>
        /// Статистика входа пользователя
        /// </summary>
        /// <param name="date"></param>
        /// <param name="userId"></param>
        public void CreateStatisticUserLogin(DateTime date, string userId)
        {
            Task.Run(() =>
            {
                var stat = Create(new SxStatistic { DateCreate = date, Type = SxStatistic.SxStatisticType.UserLogin });
                var statUserLogin = new SxStatisticUserLogin
                {
                    DateCreate = date,
                    StatisticId = stat.Id,
                    UserId = userId
                };

                var query = @"INSERT INTO D_STAT_LOGIN
VALUES
  (
    @sid,
    @uid,
    @dc
  )";
                using (var conn = new SqlConnection(ConnectionString))
                {
                    conn.Execute(query, new { sid = stat.Id, uid = userId, dc = date });
                }
            });
        }
    }
}
