using Dapper;
using GE.WebUI.Models;
using GE.WebUI.ViewModels;
using SX.WebCore;
using SX.WebCore.SxProviders;
using SX.WebCore.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace GE.WebUI.Infrastructure.Repositories
{
    public sealed class RepoInfographic
    {
        private static readonly string _connectionString = ConfigurationManager.ConnectionStrings["DbContext"].ConnectionString;

        public async Task<VMInfographic[]> ReadAsync(SxFilter filter)
        {
            var sb = new StringBuilder();
            sb.Append(SxQueryProvider.GetSelectString(new string[] {
                // infographic
                "dp.Id AS PictureId",
                "di.MaterialId",
                "di.ModelCoreType",
                // picture
                "dp.Id",
                "dp.Caption"
            }));
            sb.Append(@" FROM D_PICTURE AS dp
LEFT JOIN D_INFOGRAPHIC AS di ON di.PictureId = dp.Id
LEFT JOIN DV_MATERIAL AS dm ON dm.Id = di.MaterialId AND dm.ModelCoreType = di.ModelCoreType ");

            object param = null;
            var gws = GetInfographicsWhereString(filter, out param);
            sb.Append(gws);

            var defaultOrder = new SxOrderItem { FieldName = "Caption", Direction = SortDirection.Asc };
            sb.Append(SxQueryProvider.GetOrderString(defaultOrder, filter.Order, new Dictionary<string, string> {
                ["Caption"]= "dp.Caption",
                ["PictureId"]="dp.Id"
            }));

            sb.AppendFormat(" OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", filter.PagerInfo.SkipCount, filter.PagerInfo.PageSize);

            //count
            var sbCount = new StringBuilder();
            sbCount.Append(@"SELECT COUNT(1) FROM D_PICTURE AS dp
LEFT JOIN D_INFOGRAPHIC AS di ON di.PictureId = dp.Id
LEFT JOIN DV_MATERIAL AS dm ON dm.Id = di.MaterialId AND dm.ModelCoreType = di.ModelCoreType ");
            sbCount.Append(gws);

            using (var connection = new SqlConnection(_connectionString))
            {
                filter.PagerInfo.TotalItems = (await connection.QueryAsync<int>(sbCount.ToString(), param: param)).SingleOrDefault();
                var data = connection.Query<VMInfographic, SxVMPicture, VMInfographic>(sb.ToString(), (i, p)=> {
                    i.Picture = p;
                    return i;
                }, param: param, splitOn:"PictureId, Id");
                return data.ToArray();
            }
        }
        private static string GetInfographicsWhereString(SxFilter filter, out object param)
        {
            param = null;
            var query = new StringBuilder();
            
            query.Append(" WHERE (dp.[Caption] LIKE '%'+@caption+'%' OR @caption IS NULL) ");

            var linked = (bool)filter.AddintionalInfo?[0];
            if (linked)
                query.Append(" AND (di.[MaterialId]=@mid AND di.[ModelCoreType]=@mct) ");
            else
                query.Append(" AND (dp.[Id] NOT IN (SELECT di2.PictureId FROM D_INFOGRAPHIC AS di2))");

            param = new
            {
                mid=filter.MaterialId,
                mct=filter.ModelCoreType,
                caption=(string)filter.WhereExpressionObject?.Caption
            };

            return query.ToString();
        }

        public async Task DeleteAsync(Infographic model)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.ExecuteAsync("dbo.del_infographic @pid, @mid, @mct", new { pid = model.PictureId, mid = model.MaterialId, mct = model.ModelCoreType });
            }
        }

        public async Task<VMInfographic> GetByKeyAsync(object[] keys)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var data = await connection.QueryAsync<VMInfographic, SxVMPicture, VMInfographic>("dbo.get_infographic @pid", (i, p) =>
                {
                    i.Picture = p;
                    return i;
                }, new { pid = keys[0] });

                return data.SingleOrDefault();
            }
        }

        public async Task AddListAsync(int mid, byte mct, List<Guid> ids)
        {
            var @params = ids.Select(x => x.ToString()).ToDelimeterString(',');
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.ExecuteAsync("dbo.add_infographics @mid, @mct, @ids", new { mid=mid, mct=mct, ids = @params });
            }
        }
    }
}