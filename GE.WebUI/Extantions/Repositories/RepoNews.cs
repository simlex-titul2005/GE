using GE.WebUI.Models;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using SX.WebCore.ViewModels;
using static SX.WebCore.Enums;
using System;

namespace GE.WebUI.Extantions.Repositories
{
    public static partial class RepositoryExtantions
    {
        /// <summary>
        /// Последние новости игр
        /// </summary>
        /// <param name="repo">Репозиотрий</param>
        /// <param name="lnc">Кол-во последних новостей в левом блоке</param>
        /// <param name="gc">Кол-во отображаемых игр в правом блоке</param>
        /// <param name="glnc">Кол-во последних новостей для игр в правом блоке</param>
        /// <param name="gtc">Кол-во тегов для игры</param>
        /// <returns></returns>
        public static VMLGNB LastGameNewsBlock(this WebCoreExtantions.Repositories.RepoNews repo, int lnc, int gc, int glnc, int gtc)
        {
            var model = new VMLGNB(lnc, gc, glnc);

            var queryLastNews = @"SELECT TOP(@lnc) dm.DateOfPublication,
       dm.Title,
       dm.TitleUrl,
       dm.DateCreate,
       dn.GameId
FROM   DV_MATERIAL  AS dm
       JOIN D_NEWS  AS dn
            ON  dn.Id = dm.Id
            AND dn.ModelCoreType = dm.ModelCoreType
       JOIN D_GAME  AS dg
            ON  dg.Id = dn.GameId
            AND dg.Show = 1
WHERE  dm.Show = 1
       AND dm.DateOfPublication <= GETDATE()
ORDER BY
       dm.DateCreate DESC";

            var queryGames = @"SELECT TOP(@gc) x.TitleUrl,
       x.Id,
       x.Title,
       x.FrontPictureId
FROM   (
           SELECT dg.TitleUrl,
                  dg.Id,
                  dg.Title,
                  dg.FrontPictureId,
                  COUNT(dc.Id)           AS CommentsCount,
                  COUNT(dm.Id)           AS NewsCount
           FROM   D_GAME                 AS dg
                  LEFT JOIN D_NEWS       AS dn
                       ON  dn.GameId = dg.Id
                  LEFT JOIN DV_MATERIAL  AS dm
                       ON  dm.Id = dn.Id
                       AND dm.ModelCoreType = dn.ModelCoreType
                  LEFT JOIN D_COMMENT    AS dc
                       ON  dc.ModelCoreType = dm.ModelCoreType
                       AND dc.MaterialId = dm.Id
           WHERE  dg.Show = 1
           GROUP BY
                  dg.TitleUrl,
                  dg.Id,
                  dg.Title,
                  dg.FrontPictureId
       ) AS x
ORDER BY
       x.CommentsCount DESC";

            var queryGameNews = @"SELECT TOP(@glnc) dm.DateOfPublication,
       dm.Title,
       dm.TitleUrl,
       dm.FrontPictureId,
       dm.DateCreate
FROM   DV_MATERIAL  AS dm
       JOIN D_NEWS  AS dn
            ON  dn.Id = dm.Id
            AND dn.ModelCoreType = dm.ModelCoreType
       JOIN D_GAME  AS dg
            ON  dg.Id = dn.GameId
            AND dg.TitleUrl = @gturl
WHERE  dm.Show = 1
       AND dm.DateOfPublication <= GETDATE()
ORDER BY
       dm.DateOfPublication DESC";

            var queryGameTags= @"SELECT TOP(@amount)
       dmt.Id            AS Title,
       COUNT(dmt.Id)     AS [Count],
       1                 AS IsCurrent
FROM   D_MATERIAL_TAG    AS dmt
       JOIN D_NEWS       AS dn
            ON  dn.Id = dmt.MaterialId
            AND dn.ModelCoreType = dmt.ModelCoreType
       JOIN DV_MATERIAL  AS dm
            ON  dm.Id = dmt.MaterialId
            AND dm.ModelCoreType = dmt.ModelCoreType
            AND dm.Show = 1
            AND dm.DateOfPublication <= GETDATE()
       JOIN D_GAME       AS dg
            ON  dg.Id = dn.GameId
            AND dg.TitleUrl = @gturl
GROUP BY
       dmt.Id";

            using (var connection = new SqlConnection(repo.ConnectionString))
            {
                model.News = connection.Query<VMLGNBNews>(queryLastNews, new { lnc = lnc }).ToArray();
                model.Games = connection.Query<VMLGNBGame>(queryGames, new { gc = gc }).ToArray();
                if (model.Games.Any())
                {
                    for (int i = 0; i < model.Games.Length; i++)
                    {
                        var game = model.Games[i];
                        game.News = connection.Query<VMLGNBNews>(queryGameNews, new { glnc = glnc, gturl = game.TitleUrl }).ToArray();
                        game.Tags = connection.Query<SxVMMaterialTag>(queryGameTags, new { amount = gtc, gturl = game.TitleUrl }).ToArray();
                    }
                }
            }


            return model;
        }

        /// <summary>
        /// Последние новости категорий
        /// </summary>
        /// <param name="repo">Репозиотрий</param>
        /// <param name="lnc">Кол-во последних новостей в левом блоке</param>
        /// <param name="clnc">Кол-во последних новостей подкатегорий</param>
        /// <param name="ctc">Кол-во тегов пордкатегории</param>
        /// <returns></returns>
        public static VMLCNB LastCategoryBlock(this WebCoreExtantions.Repositories.RepoNews repo, int lnc, int clnc, int ctc)
        {
            var queryForCategory = @"SELECT dmc.Title, dmc.Id
FROM   D_MATERIAL_CATEGORY  AS dmc
       JOIN D_NEWS          AS dn
            ON  dn.ModelCoreType = dmc.ModelCoreType
WHERE  dmc.ParentCategoryId IS NULL
GROUP BY
       dmc.Title, dmc.Id";

            var queryForSubCategories = @"SELECT dmc.Id,
       dmc.Title,
       dmc.FrontPictureId
FROM   D_MATERIAL_CATEGORY AS dmc
WHERE  dmc.ParentCategoryId = @cat_id
ORDER BY dmc.Title";

            var queryForSubCategoryNews = @"SELECT TOP(@clnc)
       dm.DateOfPublication,
       dm.DateCreate,
       dm.Title,
       dm.TitleUrl,
       dm.FrontPictureId
FROM   DV_MATERIAL AS dm
WHERE  dm.CategoryId = @cat_id
       AND dm.Show = 1
       AND dm.DateOfPublication <= GETDATE()
ORDER BY
       dm.DateOfPublication DESC";

            var queryForLastNews = @"WITH tree(ModelCoreType, Id, ParentCategoryId, Title, [Level]) AS
     (
         SELECT dmc1.ModelCoreType,
                dmc1.Id,
                dmc1.ParentCategoryId,
                dmc1.Title,
                CASE 
                     WHEN dmc1.Id = @cat_id
         OR @cat_id IS NULL THEN 1 ELSE 2 END 
            FROM D_MATERIAL_CATEGORY AS dmc1
            WHERE (
                (dmc1.Id = @cat_id OR dmc1.ParentCategoryId = @cat_id)
                OR @cat_id IS NULL
            )
            UNION ALL
            SELECT dmc2.ModelCoreType,
                   dmc2.Id,
                   dmc2.ParentCategoryId,
                   dmc2.Title,
                   t.[Level] + 1
            FROM   D_MATERIAL_CATEGORY  AS dmc2
                   JOIN tree            AS t
                        ON  t.Id = dmc2.ParentCategoryId
     )

SELECT TOP(@lnc) dm.DateOfPublication,
       dm.DateCreate,
       dm.Title,
       dm.TitleUrl,
       dm.CategoryId
FROM   DV_MATERIAL           AS dm
       JOIN (
                SELECT t.Id,
                       t.ModelCoreType
                FROM   tree AS t
                WHERE  t.ModelCoreType = @mct
                GROUP BY
                       t.Id,
                       t.ModelCoreType
            ) x
            ON  x.Id = dm.CategoryId
            AND dm.ModelCoreType = x.ModelCoreType
WHERE  dm.Show = 1
       AND dm.DateOfPublication <= GETDATE()
ORDER BY
       dm.DateOfPublication     DESC";

            var queryForTags = @"SELECT TOP(@amount)
       dmt.Id            AS Title,
       COUNT(dmt.Id)     AS [Count],
       1                 AS IsCurrent
FROM   D_MATERIAL_TAG    AS dmt
       JOIN D_NEWS       AS dn
            ON  dn.Id = dmt.MaterialId
            AND dn.ModelCoreType = dmt.ModelCoreType
       JOIN DV_MATERIAL  AS dm
            ON  dm.Id = dmt.MaterialId
            AND dm.ModelCoreType = dmt.ModelCoreType
            AND dm.Show = 1
            AND dm.DateOfPublication <= GETDATE()
WHERE  dm.CategoryId = @cat_id
GROUP BY
       dmt.Id";

            var data = new VMLCNB();

            using (var connection = new SqlConnection(repo.ConnectionString))
            {
                data.Categories = connection.Query<VMLCNBCategory>(queryForCategory).ToArray();
                if(data.Categories.Any())
                {
                    for (int i = 0; i < data.Categories.Length; i++)
                    {
                        var category = data.Categories[i];

                        category.SubCategories = connection.Query<VMLCNBCategory>(queryForSubCategories, new { cat_id = category.Id }).ToArray();

                        if(category.SubCategories.Any())
                        {
                            for (int y = 0; y < category.SubCategories.Length; y++)
                            {
                                var subCategory = category.SubCategories[y];
                                subCategory.News = connection.Query<VMLCNBNews>(queryForSubCategoryNews, new { cat_id = subCategory.Id, clnc = clnc }).ToArray();
                                subCategory.Tags = connection.Query<SxVMMaterialTag>(queryForTags, new { amount = ctc, cat_id = subCategory.Id }).ToArray();
                            }
                        }

                        category.News = connection.Query<VMLCNBNews>(queryForLastNews, new { lnc=lnc, mct=ModelCoreType.News, cat_id=category.Id }).ToArray();
                    }
                }
            }

            return data;
        }

        public static VMDetailNews GetByTitleUrl(this WebCoreExtantions.Repositories.RepoNews repo, string titleUrl)
        {
            var query = @"SELECT dn.*,
       dm.*,
       dg.TitleUrl       AS GameTitleUrl,
       CASE 
            WHEN dm.Foreword IS NOT NULL THEN dm.Foreword
            ELSE SUBSTRING(dbo.FUNC_STRIP_HTML(dm.Html), 0, 200) +
                 '...'
       END               AS Foreword,
       (
           SELECT ISNULL(SUM(1), 0)
           FROM   D_USER_CLICK  AS duc
                  JOIN D_LIKE   AS dl
                       ON  dl.UserClickId = duc.Id
           WHERE  duc.MaterialId = dm.Id
                  AND duc.ModelCoreType = dm.ModelCoreType
                  AND dl.Direction = 1
       )                 AS LikeUpCount,
       (
           SELECT ISNULL(SUM(1), 0)
           FROM   D_USER_CLICK  AS duc
                  JOIN D_LIKE   AS dl
                       ON  dl.UserClickId = duc.Id
           WHERE  duc.MaterialId = dm.Id
                  AND duc.ModelCoreType = dm.ModelCoreType
                  AND dl.Direction = 2
       )                 AS LikeDownCount,
       (
           SELECT COUNT(1)
           FROM   D_COMMENT AS dc
           WHERE  dc.MaterialId = dm.Id
                  AND dc.ModelCoreType = dm.ModelCoreType
       )                 AS CommentsCount,
       anu.NikName       AS UserNikName
FROM   D_NEWS            AS dn
       JOIN DV_MATERIAL  AS dm
            ON  dm.Id = dn.Id
            AND dm.ModelCoreType = dn.ModelCoreType
       LEFT JOIN D_GAME  AS dg
            ON  dg.Id = dn.GameId
       JOIN AspNetUsers  AS anu
            ON  anu.Id = dm.UserId
WHERE  dm.TitleUrl = @title_url";

            using (var connection = new SqlConnection(repo.ConnectionString))
            {
                var data=connection.Query<VMDetailNews>(query, new { title_url = titleUrl }).SingleOrDefault();
                return data;
            }
        }

        public static VMLastNews[] GetByDateMaterial(this WebCoreExtantions.Repositories.RepoNews repo, ModelCoreType mct, DateTime date)
        {
            var queryForByDateMaterials = @"SELECT x.DateCreate,
       x.DateOfPublication,
       x.ModelCoreType,
       x.Title,
       x.TitleUrl,
       x.Foreword,
       x.UserId,
       anu.AvatarId,
       CASE 
            WHEN anu.NikName IS NULL THEN anu.UserName
            ELSE anu.NikName
       END               AS NikName
FROM   (
           SELECT TOP(1) dm.*,
                  IsCurrent = 1
           FROM   DV_MATERIAL AS dm
           WHERE  dm.ModelCoreType = @mct
                  AND dm.DateOfPublication > @date
                  AND dm.Show = 1
                  AND dm.DateOfPublication <= GETDATE()
           ORDER BY
                  dm.DateOfPublication DESC
           UNION ALL
           SELECT TOP(2) dm.*,
                  IsCurrent = -1
           FROM   DV_MATERIAL AS dm
           WHERE  dm.ModelCoreType = @mct
                  AND dm.DateOfPublication < @date
                  AND dm.Show = 1
                  AND dm.DateOfPublication <= GETDATE()
           ORDER BY
                  dm.DateOfPublication DESC
       ) x
       JOIN AspNetUsers  AS anu
            ON  anu.Id = x.UserId
ORDER BY
       x.IsCurrent";

            using (var connection = new SqlConnection(repo.ConnectionString))
            {
                var data = connection.Query<VMLastNews, VMUser, VMLastNews>(queryForByDateMaterials, (m, u) => {
                    m.Author = u;
                    return m;
                }, new { date = date, mct = mct }, splitOn: "UserId").ToArray();
                return data;
            }
        }

        public static VMLastNews[] GetPopular(this WebCoreExtantions.Repositories.RepoNews repo, ModelCoreType mct, int mid, int amount)
        {
            var queryForByDateMaterials = @"SELECT TOP(@amount)
       dm.DateCreate,
       dm.DateOfPublication,
       dm.Title,
       dm.TitleUrl,
       dm.ModelCoreType,
       COUNT(dc.Id)            AS CommentsCount,
       COUNT(dl.Id)            AS LikesCount,
       SUM(dm.ViewsCount)         ViewsCount
FROM   DV_MATERIAL             AS dm
       LEFT JOIN D_COMMENT     AS dc
            ON  dc.ModelCoreType = dm.ModelCoreType
            AND dc.MaterialId = dm.Id
       LEFT JOIN D_USER_CLICK  AS duc
            ON  duc.MaterialId = dm.Id
            AND duc.ModelCoreType = dm.ModelCoreType
       LEFT JOIN D_LIKE        AS dl
            ON  dl.UserClickId = duc.Id
WHERE  dm.ModelCoreType = @mct
       AND dm.Show = 1
       AND dm.DateOfPublication <= GETDATE()
       AND dm.Id NOT IN (@mid)
GROUP BY
       dm.DateCreate,
       dm.DateOfPublication,
       dm.Title,
       dm.TitleUrl,
       dm.ModelCoreType
HAVING COUNT(dc.Id) > 0 OR COUNT(dl.Id) > 0 OR COUNT(dm.ViewsCount) > 0
ORDER BY
       CommentsCount DESC,
       LikesCount DESC,
       ViewsCount DESC";

            using (var connection = new SqlConnection(repo.ConnectionString))
            {
                var data = connection.Query<VMLastNews>(queryForByDateMaterials, new { mct = mct, mid=mid, amount =amount }).ToArray();
                return data;
            }
        }
    }
}