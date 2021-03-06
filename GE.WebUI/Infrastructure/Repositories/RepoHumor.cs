﻿using Dapper;
using GE.WebUI.Models;
using GE.WebUI.ViewModels;
using System.Data.SqlClient;
using System;
using System.Linq;
using SX.WebCore;
using System.Text;
using SX.WebCore.MvcApplication;
using SX.WebCore.SxProviders;
using static SX.WebCore.HtmlHelpers.SxExtantions;
using SX.WebCore.ViewModels;

namespace GE.WebUI.Infrastructure.Repositories
{
    public sealed class RepoHumor : RepoMaterial<Humor, VMHumor>
    {
        public RepoHumor() : base(SxMvcApplication.ModelCoreTypeProvider[nameof(Humor)]) { }

        public override VMHumor[] Read(SxFilter filter)
        {
            var sb = new StringBuilder();
            sb.Append(SxQueryProvider.GetSelectString(new string[] {
                "dm.*",
                filter.WithComments.HasValue && filter.WithComments==true?"(SELECT COUNT(1) FROM D_COMMENT AS dc WHERE dc.MaterialId=dm.Id AND dc.ModelCoreType=dm.ModelCoreType) AS CommentsCount":null,
                "dmc.*",
                "anu.*",
                "dp.Id", "dp.Width", "dp.Height",
                "dst.*"
            }));
            sb.Append(" FROM DV_MATERIAL AS dm ");
            sb.Append(" LEFT JOIN D_MATERIAL_CATEGORY AS dmc ON dmc.Id = dm.CategoryId ");
            sb.Append(" LEFT JOIN D_USER AS anu ON anu.Id = dm.UserId ");
            sb.Append(" LEFT JOIN D_PICTURE AS dp ON dp.Id = dm.FrontPictureId ");
            sb.Append(" LEFT JOIN D_SEO_TAGS AS dst ON (dst.ModelCoreType=dm.ModelCoreType AND dst.MaterialId=dm.Id AND dst.MaterialId IS NOT NULL) ");

            object param = null;
            var gws = GetMaterialsWhereString(filter, out param);
            sb.Append(gws);

            var defaultOrder = new SxOrderItem { FieldName = "DateCreate", Direction = SortDirection.Desc };
            sb.Append(SxQueryProvider.GetOrderString(defaultOrder, filter.Order, new System.Collections.Generic.Dictionary<string, string> {
                { "DateCreate", "dm.[DateCreate]"},
                { "Title","dm.[Title]"}
            }));

            sb.AppendFormat(" OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", filter.PagerInfo.SkipCount, filter.PagerInfo.PageSize);

            //count
            var sbCount = new StringBuilder();
            sbCount.Append("SELECT COUNT(1) FROM DV_MATERIAL AS dm ");
            sbCount.Append(gws);

            using (var connection = new SqlConnection(ConnectionString))
            {
                filter.PagerInfo.TotalItems = connection.Query<int>(sbCount.ToString(), param: param).SingleOrDefault();
                var data = connection.Query<VMHumor, SxVMMaterialCategory, SxVMAppUser, SxVMPicture, SxVMSeoTags, VMHumor>(sb.ToString(), (m, c, u, p, st) =>
                {
                    m.Category = c;
                    m.User = u;
                    m.FrontPicture = p;
                    m.SeoTags = st;
                    return m;
                }, param: param, splitOn: "Id").ToArray();

                if (filter.WidthVideos == true && data.Any())
                    FillMaterialsVideo(connection, ref data);

                return data;
            }
        }
        private static string GetMaterialsWhereString(SxFilter filter, out object param)
        {
            param = null;
            var query = new StringBuilder();
            query.Append(" WHERE (dm.Title LIKE '%'+@title+'%' OR @title IS NULL) ");
            query.Append(" AND (dm.Foreword LIKE '%'+@fwd+'%' OR @fwd IS NULL) ");
            query.Append(" AND (dm.CategoryId LIKE '%'+@cat+'%' OR @cat IS NULL) ");
            if (filter.OnlyShow.HasValue && filter.OnlyShow == true)
                query.Append(" AND (dm.Show=@show AND dm.DateOfPublication<=GETDATE()) ");
            query.Append(" AND (dm.ModelCoreType=@mct) ");

            string title = filter.WhereExpressionObject?.Title;
            string fwd = filter.WhereExpressionObject?.Foreword;
            var cat = filter?.CategoryId;

            param = new
            {
                title = title,
                fwd = fwd,
                cat = cat,
                mct = SxMvcApplication.ModelCoreTypeProvider[nameof(Humor)],
                show = filter.OnlyShow == true ? true : (bool?)null
            };

            return query.ToString();
        }

        public override void Delete(Humor model)
        {
            const string query = "DELETE FROM D_HUMOR WHERE Id=@mid AND ModelCoreType=@mct";
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Execute(query, new { mid = model.Id, mct = model.ModelCoreType });
            }

            base.Delete(model);
        }

        protected override Action<SqlConnection, Humor> ChangeMaterialBeforeSelect => (connection, model) =>
        {
            const string query = "SELECT TOP(1) dh.UserName FROM D_HUMOR AS dh WHERE dh.Id=@id";
            model.UserName = connection.Query<string>(query, new { id = model.Id }).SingleOrDefault();
        };
    }
}
