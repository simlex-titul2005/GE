using GE.WebCoreExtantions;
using SX.WebCore.Repositories;

namespace GE.WebAdmin.Infrastructure.Repositories
{
    public sealed class RepoPicture : SxRepoPicture<DbContext>
    {
        public RepoPicture():base()
        {
            InsertNotFreePictures = (sb) =>
            {
                sb.AppendLine(" INSERT INTO @result SELECT dp.Id FROM D_PICTURE AS dp WHERE dp.Id IN(SELECT daa.PictureId FROM D_AUTHOR_APHORISM AS daa)");
                sb.AppendLine(" INSERT INTO @result SELECT dp.Id FROM D_PICTURE AS dp WHERE dp.Id IN(SELECT dg.FrontPictureId FROM D_GAME AS dg) OR dp.Id IN(SELECT dg.GoodPictureId FROM D_GAME AS dg) OR dp.Id IN(SELECT dg.BadPictureId FROM D_GAME AS dg)");
            };
        }
    }
}