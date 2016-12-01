using System;
using System.Data.SqlClient;
using System.Text;
using SX.WebCore.SxRepositories;
using Dapper;
using SX.WebCore.DbModels;

namespace GE.WebUI.Infrastructure.Repositories
{
    public sealed class RepoPicture : SxRepoPicture
    {
        protected override Action<StringBuilder> InsertNotFreePictures
        {
            get
            {
                return sb =>
                {
                    sb.AppendLine("INSERT INTO @result SELECT aa.PictureId FROM D_AUTHOR_APHORISM AS aa WHERE aa.PictureId IS NOT NULL");
                    sb.AppendLine("INSERT INTO @result SELECT dsts.PictureId FROM D_SITE_TEST_SUBJECT AS dsts WHERE dsts.PictureId IS NOT NULL");
                    sb.AppendLine(@"INSERT INTO @result
SELECT x.Id FROM(
SELECT dg.FrontPictureId AS Id FROM D_GAME AS dg WHERE dg.FrontPictureId IS NOT NULL
UNION ALL
SELECT dg.GoodPictureId  AS Id FROM D_GAME AS dg WHERE dg.GoodPictureId IS NOT NULL
UNION ALL
SELECT dg.BadPictureId  AS Id FROM D_GAME AS dg WHERE dg.BadPictureId IS NOT NULL) AS x
GROUP BY x.Id");
                };
            }
        }

        protected override Action<SqlConnection, SxPicture> BeforeDeleteAction
        {
            get
            {
                return (connection, model) =>
                {
                    var query = new StringBuilder();
                    query.AppendLine("UPDATE D_AUTHOR_APHORISM SET PictureId = NULL WHERE PictureId = @pictureId");
                    query.AppendLine("UPDATE D_SITE_TEST_SUBJECT SET PictureId = NULL WHERE PictureId = @pictureId");
                    connection.Execute(query.ToString(), new { pictureId = model.Id });
                };
            }
        }
    }
}