using System.Data.SqlClient;
using System.Linq;
using Dapper;
using GE.WebAdmin.Models;
using static SX.WebCore.HtmlHelpers.SxExtantions;
using SX.WebCore.Providers;
using GE.WebCoreExtantions;
using GE.WebCoreExtantions.Repositories;
using System;
using static SX.WebCore.Enums;

namespace GE.WebAdmin.Extantions.Repositories
{
    public static partial class RepositoryExtantions
    {
        public static VMVideo[] QueryForAdmin(this RepoVideo repo, Filter filter)
        {
            var query = SxQueryProvider.GetSelectString();
            query += @" FROM D_VIDEO dv ";

            object param = null;
            query += getVideoWhereString(filter, out param);

            query += SxQueryProvider.GetOrderString("dv.DateCreate", SortDirection.Desc, filter.Orders);

            query += " OFFSET " + filter.PagerInfo.SkipCount + " ROWS FETCH NEXT " + filter.PagerInfo.PageSize + " ROWS ONLY";

            using (var conn = new SqlConnection(repo.ConnectionString))
            {
                var data = conn.Query<VMVideo>(query, param: param).ToArray();
                return data.ToArray();
            }
        }

        public static int FilterCount(this RepoVideo repo, Filter filter)
        {
            var query = @"SELECT COUNT(1) FROM D_VIDEO dv ";

            object param = null;
            query += getVideoWhereString(filter, out param);

            using (var conn = new SqlConnection(repo.ConnectionString))
            {
                var data = conn.Query<int>(query, param: param).Single();

                return data;
            }
        }

        private static string getVideoWhereString(Filter filter, out object param)
        {
            param = null;
            string query = null;
            query += " WHERE (dv.Title LIKE '%'+@title+'%' OR @title IS NULL) ";
            query += " AND (dv.VideoId LIKE '%'+@vid+'%' OR @vid IS NULL) ";

            var title = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Title != null ? (string)filter.WhereExpressionObject.Title : null;
            var vid = filter.WhereExpressionObject != null && filter.WhereExpressionObject.VideoId != null ? (string)filter.WhereExpressionObject.VideoId : null;

            param = new
            {
                title = title,
                vid = vid
            };

            return query;
        }

        public static VMVideo[] LinkedVideos(this RepoVideo repo, Filter filter, bool forMaterial)
        {
            var query = SxQueryProvider.GetSelectString(new string[] { "dv.*"});
            query += @" FROM D_VIDEO AS dv " + (forMaterial?"":@"LEFT")+ @" JOIN D_VIDEO_LINK AS dvl ON dvl.VideoId = dv.Id ";

            object param = null;
            query += getLinkedVideoWhereString(filter, forMaterial, out param);

            query += SxQueryProvider.GetOrderString("dv.DateCreate", SortDirection.Desc, filter.Orders);

            query += " OFFSET " + filter.PagerInfo.SkipCount + " ROWS FETCH NEXT " + filter.PagerInfo.PageSize + " ROWS ONLY";

            using (var conn = new SqlConnection(repo.ConnectionString))
            {
                var data = conn.Query<VMVideo>(sql: query, param: param).ToArray();
                return data.ToArray();
            }
        }

        public static int LinkedVideosCount(this RepoVideo repo, Filter filter, bool forMaterial)
        {
            var query = @"SELECT COUNT(1) FROM D_VIDEO AS dv " + (forMaterial ? "" : @"LEFT") + @" JOIN D_VIDEO_LINK AS dvl ON dvl.VideoId = dv.Id ";

            object param = null;
            query += getLinkedVideoWhereString(filter, forMaterial, out param);

            using (var conn = new SqlConnection(repo.ConnectionString))
            {
                var data = conn.Query<int>(query, param: param).Single();

                return data;
            }
        }

        private static void checkLinkedVideosFilter(Filter filter)
        {
            if (!filter.MaterialId.HasValue)
                throw new ArgumentNullException("Фильт должен сожержать идентификатор материала");
            if (filter.ModelCoreType == ModelCoreType.Unknown)
                throw new ArgumentNullException("Фильт должен сожержать тип материала");
        }

        private static string getLinkedVideoWhereString(Filter filter, bool forMaterial, out object param)
        {
            checkLinkedVideosFilter(filter);

            param = null;
            string query = null;
            query += " WHERE (dv.Title LIKE '%'+@title+'%' OR @title IS NULL) ";
            query += " AND (dv.VideoId LIKE '%'+@vid+'%' OR @vid IS NULL) ";
            if (forMaterial)
            {
                query += " AND (dvl.ModelCoreType = @mct) ";
                query += " AND (dvl.Materialid = @mid) ";
            }
            else
                query += " AND (dv.Id NOT IN (SELECT dvl2.VideoId FROM D_VIDEO_LINK AS dvl2 WHERE dvl2.MaterialId=@mid AND dvl2.ModelCoreType=@mct)) ";


            var title = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Title != null ? (string)filter.WhereExpressionObject.Title : null;
            var vid = filter.WhereExpressionObject != null && filter.WhereExpressionObject.VideoId != null ? (string)filter.WhereExpressionObject.VideoId : null;

            param = new
            {
                title = title,
                vid = vid,
                mid=filter.MaterialId,
                mct=(byte)filter.ModelCoreType
            };

            return query;
        }

        public static void AddMaterialVideo(this RepoVideo repo, int mid, ModelCoreType mct, Guid vid)
        {
            var query = @"INSERT INTO D_VIDEO_LINK
VALUES
(
	@mid,
	@mct,
	@vid
)";
            using (var conn = new SqlConnection(repo.ConnectionString))
            {
                conn.Execute(query, param: new { mid=mid, mct=mct, vid=vid});
            }
        }

        public static void DeleteMaterialVideo(this RepoVideo repo, int mid, ModelCoreType mct, Guid vid)
        {
            var query = @"DELETE FROM D_VIDEO_LINK
WHERE MaterialId=@mid AND ModelCoreType=@mct AND VideoId=@vid";
            using (var conn = new SqlConnection(repo.ConnectionString))
            {
                conn.Execute(query, param: new { mid = mid, mct = mct, vid = vid });
            }
        }
    }
}