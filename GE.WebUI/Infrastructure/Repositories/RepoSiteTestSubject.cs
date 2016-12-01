﻿using Dapper;
using GE.WebUI.Models;
using GE.WebUI.ViewModels;
using SX.WebCore;
using SX.WebCore.SxProviders;
using SX.WebCore.SxRepositories.Abstract;
using SX.WebCore.ViewModels;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace GE.WebUI.Infrastructure.Repositories
{
    public sealed class RepoSiteTestSubject : SxDbRepository<int, SiteTestSubject, VMSiteTestSubject>
    {
        public override VMSiteTestSubject[] Read(SxFilter filter)
        {
            var sb = new StringBuilder();
            sb.Append(SxQueryProvider.GetSelectString(new string[] {
                "dstq.Id",
                "dstq.Title",
                "dstq.Description",
                "dstq.TestId",
                "dstq.PictureId",
                "dst.Id",
                "dp.Id"
            }));
            sb.Append(" FROM D_SITE_TEST_SUBJECT AS dstq ");
            var joinString = @"JOIN D_SITE_TEST AS dst ON  dst.Id = dstq.TestId
       LEFT JOIN D_PICTURE AS dp on dp.Id=dstq.PictureId";
            sb.Append(joinString);

            object param = null;
            var gws = getSiteTestSubjectsWhereString(filter, out param);
            sb.Append(gws);

            var defaultOrder = new SxOrderItem { FieldName = "dstq.DateCreate", Direction = SortDirection.Desc };
            sb.Append(SxQueryProvider.GetOrderString(defaultOrder, filter.Order));

            sb.AppendFormat(" OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", filter.PagerInfo.SkipCount, filter.PagerInfo.PageSize);

            //count
            var sbCount = new StringBuilder();
            sbCount.Append("SELECT COUNT(1) FROM D_SITE_TEST_SUBJECT AS dstq ");
            sbCount.Append(joinString);
            sbCount.Append(gws);

            using (var connection = new SqlConnection(ConnectionString))
            {
                var data = connection.Query<VMSiteTestSubject, VMSiteTest, SxVMPicture, VMSiteTestSubject>(sb.ToString(), (q, t, p) =>
                {
                    q.Picture = p;
                    q.Test = t;
                    return q;
                }, param: param, splitOn: "Id");
                filter.PagerInfo.TotalItems = connection.Query<int>(sbCount.ToString(), param: param).SingleOrDefault();
                return data.ToArray();
            }
        }

        private static string getSiteTestSubjectsWhereString(SxFilter filter, out object param)
        {
            param = null;
            var query = new StringBuilder();
            query.Append(" WHERE (dstq.[Title] LIKE '%'+@title+'%' OR @title IS NULL) ");
            query.Append(" AND (dstq.[Description] LIKE '%'+@desc+'%' OR @desc IS NULL) ");
            query.Append(" AND (dst.Id=@testId) ");

            var title = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Title != null ? (string)filter.WhereExpressionObject.Title : null;
            var desc = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Description != null ? (string)filter.WhereExpressionObject.Description : null;
            var testId = filter.AddintionalInfo != null && filter.AddintionalInfo[0] != null ? (int)filter.AddintionalInfo[0] : -1;

            param = new
            {
                title = title,
                desc=desc,
                testId= testId
            };

            return query.ToString();
        }

        public override void Delete(SiteTestSubject model)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Execute("dbo.del_site_test_subject @subjectId", new { subjectId = model.Id });
            }
        }

        public override SiteTestSubject Create(SiteTestSubject model)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<SiteTestSubject>("dbo.add_site_test_subject @testId, @title, @desc, @pictureId", new
                {
                    testId = model.TestId,
                    title = model.Title,
                    desc=model.Description,
                    pictureId=model.PictureId
                }).SingleOrDefault();
                return data;
            }
        }
    }
}
