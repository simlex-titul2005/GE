/************************************************************
 * Code formatted by SoftTree SQL Assistant © v6.5.278
 * Time: 19.05.2016 10:47:08
 ************************************************************/

/*******************************************
 * получить материал
 *******************************************/
IF OBJECT_ID(N'get_material_by_url', N'P') IS NOT NULL
    DROP PROCEDURE get_material_by_url;
GO
CREATE PROCEDURE get_material_by_url(
    @year          INT,
    @month         INT,
    @day           INT,
    @title_url     NVARCHAR(255),
    @mct           INT
)
AS
BEGIN
	SELECT dm.*,
	       dg.TitleUrl          AS GameTitleUrl,
	       CASE 
	            WHEN dm.Foreword IS NOT NULL THEN dm.Foreword
	            ELSE SUBSTRING(dbo.FUNC_STRIP_HTML(dm.Html), 0, 200) +
	                 '...'
	       END                  AS Foreword,
	       (
	           SELECT ISNULL(SUM(1), 0)
	           FROM   D_USER_CLICK  AS duc
	                  JOIN D_LIKE   AS dl
	                       ON  dl.UserClickId = duc.Id
	           WHERE  duc.MaterialId = dm.Id
	                  AND duc.ModelCoreType = dm.ModelCoreType
	                  AND dl.Direction = 1
	       )                    AS LikeUpCount,
	       (
	           SELECT ISNULL(SUM(1), 0)
	           FROM   D_USER_CLICK  AS duc
	                  JOIN D_LIKE   AS dl
	                       ON  dl.UserClickId = duc.Id
	           WHERE  duc.MaterialId = dm.Id
	                  AND duc.ModelCoreType = dm.ModelCoreType
	                  AND dl.Direction = 2
	       )                    AS LikeDownCount,
	       (
	           SELECT COUNT(1)
	           FROM   D_COMMENT AS dc
	           WHERE  dc.MaterialId = dm.Id
	                  AND dc.ModelCoreType = dm.ModelCoreType
	       )                    AS CommentsCount,
	       anu.NikName          AS UserNikName
	FROM   DV_MATERIAL          AS dm
	       LEFT JOIN D_ARTICLE  AS da
	            ON  da.ModelCoreType = dm.ModelCoreType
	            AND da.Id = dm.Id
	       LEFT JOIN D_NEWS     AS dn
	            ON  dn.Id = dm.Id
	            AND dn.ModelCoreType = dm.ModelCoreType
	       LEFT JOIN D_GAME     AS dg
	            ON  (dg.Id = da.GameId OR dg.Id = dn.GameId)
	       JOIN AspNetUsers     AS anu
	            ON  anu.Id = dm.UserId
	WHERE  dm.TitleUrl = @title_url
	       AND dm.Show = 1
	       AND dm.DateOfPublication <= GETDATE()
	       AND dm.ModelCoreType = @mct
	       AND (
	               YEAR(dm.DateCreate) = @year
	               AND MONTH(dm.DateCreate) = @month
	               AND DAY(dm.DateCreate) = @day
	           )
END
GO

/*******************************************
 * получить видео материал
 *******************************************/
IF OBJECT_ID(N'get_material_videos', N'P') IS NOT NULL
    DROP PROCEDURE get_material_videos;
GO
CREATE PROCEDURE get_material_videos(@mid INT)
AS
BEGIN
	SELECT dv.*
	FROM   D_VIDEO_LINK    AS dvl
	       JOIN D_VIDEO    AS dv
	            ON  dv.Id = dvl.VideoId
	       JOIN D_ARTICLE  AS da
	            ON  da.ModelCoreType = dvl.ModelCoreType
	            AND da.Id = @mid
END
GO

/*******************************************
 * получить комментарии материала
 *******************************************/
IF OBJECT_ID(N'get_material_comments', N'P') IS NOT NULL
    DROP PROCEDURE get_material_comments;
GO
CREATE PROCEDURE get_material_comments(@mid INT, @mct INT)
AS
BEGIN
	SELECT dc.Id,
	       dc.MaterialId,
	       dc.ModelCoreType,
	       dc.UserId,
	       dc.Html,
	       dc.DateCreate,
	       dc.UserName,
	       anu.Id,
	       anu.AvatarId,
	       anu.NikName
	FROM   D_COMMENT              AS dc
	       LEFT JOIN AspNetUsers  AS anu
	            ON  anu.Id = dc.UserId
	WHERE  dc.MaterialId = @mid
	       AND dc.ModelCoreType = @mct
	ORDER BY
	       dc.DateCreate DESC
END
GO

/*******************************************
 * добавить комментарии материала
 *******************************************/
IF OBJECT_ID(N'add_material_comment', N'P') IS NOT NULL
    DROP PROCEDURE add_material_comment;
GO
CREATE PROCEDURE add_material_comment(
    @mid      INT,
    @mct      INT,
    @uid      NVARCHAR(128),
    @html     NVARCHAR(MAX),
    @un       NVARCHAR(50)
)
AS
BEGIN
	INSERT INTO D_COMMENT
	  (
	    MaterialId,
	    ModelCoreType,
	    UserId,
	    Html,
	    DateUpdate,
	    DateCreate,
	    UserName
	  )
	VALUES
	  (
	    @mid,
	    @mct,
	    @uid,
	    @html,
	    GETDATE(),
	    GETDATE(),
	    @un
	  )
	
	DECLARE @id INT
	SELECT @id = @@identity
	SELECT *
	FROM   D_COMMENT AS dc
	WHERE  dc.Id = @id
END
GO
   
/*******************************************
* получить страницу афоризма
*******************************************/
IF OBJECT_ID(N'get_aphorism_page_model', N'TF') IS NOT NULL
    DROP FUNCTION get_aphorism_page_model;
GO
CREATE FUNCTION get_aphorism_page_model
(
	@title_url         NVARCHAR(255),
	@author_amount     INT,
	@cat_amount        INT
)
RETURNS @result TABLE(
            Id INT,
            Title NVARCHAR(255),
            TitleUrl NVARCHAR(255),
            Html NVARCHAR(MAX),
            CategoryId NVARCHAR(100),
            CategoryTitle NVARCHAR(100),
            AuthorId INT,
            AuthorName NVARCHAR(100),
            AuthorPictureId UNIQUEIDENTIFIER,
            Flag INT,
            CommentsCount INT
        )
AS
BEGIN
	DECLARE @authorId     INT,
	        @catId        NVARCHAR(100)
	
	SELECT @authorId = da.AuthorId,
	       @catId = dm.CategoryId
	FROM   D_APHORISM        AS da
	       JOIN DV_MATERIAL  AS dm
	            ON  dm.Id = da.Id
	            AND dm.ModelCoreType = da.ModelCoreType
	WHERE  dm.TitleUrl = @title_url
	
	INSERT INTO @result
	SELECT x.Id,
	       x.Title,
	       x.TitleUrl,
	       x.Html,
	       x.CategoryId,
	       dmc.Title,
	       x.AuthorId,
	       daa.Name,
	       daa.PictureId,
	       x.Flag,
	       COUNT(dc.Id)                 AS CommentsCount
	FROM   (
	           SELECT dm.*,
	                  da.AuthorId,
	                  0                 AS Flag
	           FROM   D_APHORISM        AS da
	                  JOIN DV_MATERIAL  AS dm
	                       ON  dm.Id = da.Id
	                       AND dm.ModelCoreType = da.ModelCoreType
	           WHERE  dm.TitleUrl = @title_url
	           UNION ALL
	           SELECT TOP(@author_amount) dm.*,
	                  da.AuthorId,
	                  1                 AS Flag
	           FROM   D_APHORISM        AS da
	                  JOIN DV_MATERIAL  AS dm
	                       ON  dm.Id = da.Id
	                       AND dm.ModelCoreType = da.ModelCoreType
	           WHERE  (
	                      (@authorId IS NULL AND da.AuthorId IS NULL)
	                      OR (@authorId IS NOT NULL AND da.AuthorId IN (@authorId))
	                  )
	                  AND dm.TitleUrl NOT IN (@title_url)
	           UNION ALL
	           SELECT TOP(@cat_amount) dm.*,
	                  da.AuthorId,
	                  2                 AS Flag
	           FROM   D_APHORISM        AS da
	                  JOIN DV_MATERIAL  AS dm
	                       ON  dm.Id = da.Id
	                       AND dm.ModelCoreType = da.ModelCoreType
	           WHERE  dm.CategoryId IN (@catId)
	                  AND dm.TitleUrl NOT IN (@title_url)
	       ) x
	       LEFT JOIN D_COMMENT          AS dc
	            ON  dc.MaterialId = x.Id
	            AND dc.ModelCoreType = x.ModelCoreType
	       JOIN D_MATERIAL_CATEGORY     AS dmc
	            ON  dmc.Id = x.CategoryId
	       LEFT JOIN D_AUTHOR_APHORISM  AS daa
	            ON  daa.Id = x.AuthorId
	GROUP BY
	       x.Id,
	       x.Title,
	       x.TitleUrl,
	       x.Html,
	       x.CategoryId,
	       x.AuthorId,
	       x.Flag,
	       dmc.Title,
	       daa.Name,
	       daa.PictureId
	
	RETURN
END
GO


























/*******************************************
* получить категории афоризмов
*******************************************/


IF OBJECT_ID(N'get_aphorism_categories', N'TF') IS NOT NULL
    DROP FUNCTION get_aphorism_categories;
GO
CREATE FUNCTION get_aphorism_categories
(
	@curCat NVARCHAR(255)
)
RETURNS @result TABLE (Id NVARCHAR(255), Title NVARCHAR(255), IsCurrent BIT)
AS
BEGIN
	INSERT INTO @result
	SELECT dmc.Id,
	       dmc.Title,
	       CASE 
	            WHEN dmc.Id = @curCat THEN 1
	            ELSE 0
	       END                       AS IsCurrent
	FROM   D_APHORISM                AS da
	       JOIN DV_MATERIAL          AS dm
	            ON  dm.Id = da.Id
	            AND dm.ModelCoreType = da.ModelCoreType
	       JOIN D_MATERIAL_CATEGORY  AS dmc
	            ON  dmc.Id = dm.CategoryId
	GROUP BY
	       dmc.Id,
	       dmc.Title
	
	RETURN
END
GO


























/*******************************************
* получить афоризмы
*******************************************/
IF OBJECT_ID(N'get_aphorisms', N'TF') IS NOT NULL
    DROP FUNCTION get_aphorisms;
GO
CREATE FUNCTION get_aphorisms
(
	@curCat NVARCHAR(255)
)
RETURNS @result TABLE
        (Id INT, Title NVARCHAR(255), TitleUtl NVARCHAR(255))
AS
BEGIN
	INSERT INTO @result
	SELECT da.Id,
	       dm.Title,
	       dm.TitleUrl
	FROM   D_APHORISM                   AS da
	       JOIN DV_MATERIAL             AS dm
	            ON  dm.Id = da.Id
	            AND dm.ModelCoreType = da.ModelCoreType
	       JOIN D_MATERIAL_CATEGORY     AS dmc
	            ON  dmc.Id = dm.CategoryId
	       LEFT JOIN D_AUTHOR_APHORISM  AS daa
	            ON  daa.Id = da.AuthorId
	
	RETURN
END
GO


/*******************************************
 * Популярные материалы
 *******************************************/
IF OBJECT_ID(N'get_popular_materials', N'P') IS NOT NULL
    DROP PROCEDURE get_popular_materials;
GO
CREATE PROCEDURE get_popular_materials(@mid INT, @mct INT, @amount INT)
AS
BEGIN
	SELECT TOP(@amount)
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
	       dm.IsTop,
	       dm.DateCreate,
	       dm.DateOfPublication,
	       dm.Title,
	       dm.TitleUrl,
	       dm.ModelCoreType
	HAVING COUNT(dc.Id) > 0 OR COUNT(dl.Id) > 0 OR COUNT(dm.ViewsCount) > 0
	ORDER BY
	       dm.IsTop DESC,
	       CommentsCount DESC,
	       LikesCount DESC,
	       ViewsCount DESC
END
GO

/*******************************************
* другие материалы
*******************************************/
IF OBJECT_ID(N'get_other_materials', N'TF') IS NOT NULL
    DROP FUNCTION get_other_materials;
GO
CREATE FUNCTION get_other_materials
(
	@mid        INT,
	@mct        INT,
	@dir        BIT,
	@amount     INT = 3
)
RETURNS @result TABLE(
            Id INT,
            DateCreate DATETIME,
            DateOfPublication DATETIME,
            Title NVARCHAR(255),
            TitleUrl NVARCHAR(255),
            Foreword NVARCHAR(400),
            ModelCoreType INT,
            UserId NVARCHAR(128),
            AvatarId UNIQUEIDENTIFIER,
            NikName NVARCHAR(100)
        )
AS
BEGIN
	DECLARE @date DATETIME
	SELECT @date = dm.DateOfPublication
	FROM   DV_MATERIAL AS dm
	WHERE  dm.Id = @mid
	       AND dm.ModelCoreType = @mct
	
	IF (@dir = 1)
	    INSERT INTO @result
	    SELECT TOP(@amount) dm.Id,
	           dm.DateCreate,
	           dm.DateOfPublication,
	           dm.Title,
	           dm.TitleUrl,
	           dm.Foreword,
	           dm.ModelCoreType,
	           dm.UserId,
	           anu.AvatarId,
	           anu.NikName
	    FROM   DV_MATERIAL       AS dm
	           JOIN AspNetUsers  AS anu
	                ON  anu.Id = dm.UserId
	    WHERE  dm.ModelCoreType = @mct
	           AND (
	                   dm.Id IN (@mid)
	                   OR (dm.Id NOT IN (@mid) AND dm.DateOfPublication > @date)
	               )
	    ORDER BY
	           dm.DateOfPublication DESC
	ELSE 
	IF (@dir = 0)
	    INSERT INTO @result
	    SELECT TOP(@amount) dm.Id,
	           dm.DateCreate,
	           dm.DateOfPublication,
	           dm.Title,
	           dm.TitleUrl,
	           dm.Foreword,
	           dm.ModelCoreType,
	           dm.UserId,
	           anu.AvatarId,
	           anu.NikName
	    FROM   DV_MATERIAL       AS dm
	           JOIN AspNetUsers  AS anu
	                ON  anu.Id = dm.UserId
	    WHERE  dm.ModelCoreType = @mct
	           AND (
	                   dm.Id IN (@mid)
	                   OR (dm.Id NOT IN (@mid) AND dm.DateOfPublication < @date)
	               )
	    ORDER BY
	           dm.DateOfPublication DESC
	
	RETURN
END
GO



/*******************************************
 * Получить список забаненных адресов
 *******************************************/
IF OBJECT_ID(N'get_banned_urls', N'P') IS NOT NULL
    DROP PROCEDURE get_banned_urls;
GO
CREATE PROCEDURE get_banned_urls
AS
BEGIN
	SELECT dbu.[Url]
	FROM   D_BANNED_URL AS dbu
END