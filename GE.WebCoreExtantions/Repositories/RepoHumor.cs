using Dapper;
using SX.WebCore;
using SX.WebCore.Providers;
using SX.WebCore.Repositories;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using static SX.WebCore.Enums;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace GE.WebCoreExtantions.Repositories
{
    public sealed class RepoHumor: SxRepoMaterial<SxHumor, DbContext>
    {
        public RepoHumor() : base(ModelCoreType.Humor) { }

        public override IQueryable<SxHumor> All
        {
            get
            {
                using (var conn = new SqlConnection(base.ConnectionString))
                {
                    var query = @"SELECT * FROM D_HUMOR AS dh
JOIN DV_MATERIAL AS dm ON dm.ID = dh.ID AND dm.ModelCoreType = dh.ModelCoreType
ORDER BY dm.DateCreate DESC";

                    return conn.Query<SxHumor>(query).AsQueryable();
                }
            }
        }

        public override SxHumor[] Read(SxFilter filter)
        {
            var sb = new StringBuilder();
            sb.Append(SxQueryProvider.GetSelectString(new string[] {
                "da.Id", "dm.TitleUrl", "dm.FrontPictureId", "dm.ShowFrontPictureOnDetailPage", "dm.Title", "dm.ModelCoreType", "da.UserName", "dm.SourceUrl",
                @"(SELECT
       SUBSTRING(
           CASE 
                WHEN dm.Foreword IS NOT NULL THEN dm.Foreword
                ELSE dbo.func_strip_html(dm.HTML)
           END,
           0,
           200
       ))                 AS Foreword",
                "dm.Html",
                "dm.DateCreate",
                "dm.DateOfPublication",
                "dm.ViewsCount",
                "(SELECT COUNT(1) FROM D_COMMENT dc WHERE dc.MaterialId=dm.Id AND dc.ModelCoreType=dm.ModelCoreType ) AS CommentsCount",
                "anu.Id",
                "anu.NikName",
            }));
            sb.Append(@" FROM D_HUMOR AS da ");
            var joinString = @" JOIN DV_MATERIAL  AS dm
            ON  dm.Id = da.ID
            AND dm.ModelCoreType = da.ModelCoreType
       LEFT JOIN AspNetUsers AS anu ON anu.Id=dm.UserId ";
            sb.Append(joinString);

            object param = null;
            var gws = getHumorWhereString(filter, out param);
            sb.Append(gws);

            var defaultOrder = new SxOrder { FieldName = "dm.DateCreate", Direction = SortDirection.Desc };
            sb.Append(SxQueryProvider.GetOrderString(defaultOrder, filter.Order));

            sb.AppendFormat(" OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", filter.PagerInfo.SkipCount, filter.PagerInfo.PageSize);

            //count
            var sbCount = new StringBuilder();
            sbCount.Append(@"SELECT COUNT(1) FROM D_HUMOR AS da ");
            sbCount.Append(joinString);
            sbCount.Append(gws);

            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<SxHumor, SxAppUser, SxHumor>(sb.ToString(), (da, anu) => {
                    da.User = anu;
                    return da;
                }, param: param);

                //video
                if (data.Any())
                {
                    foreach (var item in data)
                    {
                        item.VideoLinks = conn.Query<SxVideoLink, SxVideo, SxVideoLink>("dbo.get_first_material_video @mid, @mct", (vl, v) => {
                            vl.Video = v;
                            return vl;
                        }, new { mid = item.Id, mct = ModelCoreType.Humor }).ToArray();
                    }
                }

                filter.PagerInfo.TotalItems = conn.Query<int>(sbCount.ToString(), param: param).SingleOrDefault();
                return data.ToArray();
            }
        }
        private static string getHumorWhereString(SxFilter filter, out object param)
        {
            param = null;
            var query = new StringBuilder();
            query.Append(" WHERE dm.DateOfPublication <= GETDATE() AND dm.Show=1 ");
            if (!string.IsNullOrEmpty(filter.Tag))
            {
                query.Append(" AND (dm.Id IN (SELECT dmt.MaterialId FROM D_MATERIAL_TAG AS dmt WHERE dmt.MaterialId = dm.Id AND dmt.ModelCoreType = dm.ModelCoreType AND dmt.Id=N''+@tag+'')) ");

                param = new
                {
                    tag = filter.Tag
                };
            }

            return query.ToString();
        }

        public override void Delete(SxHumor model)
        {
            var query = "DELETE FROM D_HUMOR WHERE Id=@mid AND ModelCoreType=@mct";
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Execute(query, new { mid = model.Id, mct = model.ModelCoreType });
            }

            base.Delete(model);
        }
    }
}
