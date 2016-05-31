/************************************************************
 * Code formatted by SoftTree SQL Assistant © v6.5.278
 * Time: 31.05.2016 10:45:32
 ************************************************************/

/*******************************************
 * функция обрезки html тегов
 *******************************************/
IF OBJECT_ID(N'dbo.func_strip_html', N'FN') IS NOT NULL
    DROP FUNCTION dbo.func_strip_html;
GO 
CREATE FUNCTION dbo.func_strip_html
(
	@HTMLText NVARCHAR(MAX)
)
RETURNS NVARCHAR(MAX)
AS
BEGIN
	DECLARE @Start INT
	DECLARE @End INT
	DECLARE @Length INT
	
	-- Replace the HTML entity &amp; with the '&' character (this needs to be done first, as
	-- '&' might be double encoded as '&amp;amp;')
	SET @Start = CHARINDEX('&amp;', @HTMLText)
	SET @End = @Start + 4
	SET @Length = (@End - @Start) + 1
	
	WHILE (@Start > 0 AND @End > 0 AND @Length > 0)
	BEGIN
	    SET @HTMLText = STUFF(@HTMLText, @Start, @Length, '&')
	    SET @Start = CHARINDEX('&amp;', @HTMLText)
	    SET @End = @Start + 4
	    SET @Length = (@End - @Start) + 1
	END
	
	-- Replace the HTML entity &ndash; with the ' - ' character
	SET @Start = CHARINDEX('&ndash;', @HTMLText)
	SET @End = @Start + 6
	SET @Length = (@End - @Start) + 1
	
	WHILE (@Start > 0 AND @End > 0 AND @Length > 0)
	BEGIN
	    SET @HTMLText = STUFF(@HTMLText, @Start, @Length, ' - ')
	    SET @Start = CHARINDEX('&ndash;', @HTMLText)
	    SET @End = @Start + 6
	    SET @Length = (@End - @Start) + 1
	END
	
	-- Replace the HTML entity &laquo; with the ' " ' character
	SET @Start = CHARINDEX('&laquo;', @HTMLText)
	SET @End = @Start + 6
	SET @Length = (@End - @Start) + 1
	
	WHILE (@Start > 0 AND @End > 0 AND @Length > 0)
	BEGIN
	    SET @HTMLText = STUFF(@HTMLText, @Start, @Length, '"')
	    SET @Start = CHARINDEX('&laquo;', @HTMLText)
	    SET @End = @Start + 6
	    SET @Length = (@End - @Start) + 1
	END
	
	-- Replace the HTML entity &raquo; with the ' " ' character
	SET @Start = CHARINDEX('&raquo;', @HTMLText)
	SET @End = @Start + 6
	SET @Length = (@End - @Start) + 1
	
	WHILE (@Start > 0 AND @End > 0 AND @Length > 0)
	BEGIN
	    SET @HTMLText = STUFF(@HTMLText, @Start, @Length, '"')
	    SET @Start = CHARINDEX('&raquo;', @HTMLText)
	    SET @End = @Start + 6
	    SET @Length = (@End - @Start) + 1
	END
	
	-- Replace the HTML entity &lt; with the '<' character
	SET @Start = CHARINDEX('&lt;', @HTMLText)
	SET @End = @Start + 3
	SET @Length = (@End - @Start) + 1
	
	WHILE (@Start > 0 AND @End > 0 AND @Length > 0)
	BEGIN
	    SET @HTMLText = STUFF(@HTMLText, @Start, @Length, '<')
	    SET @Start = CHARINDEX('&lt;', @HTMLText)
	    SET @End = @Start + 3
	    SET @Length = (@End - @Start) + 1
	END
	
	-- Replace the HTML entity &gt; with the '>' character
	SET @Start = CHARINDEX('&gt;', @HTMLText)
	SET @End = @Start + 3
	SET @Length = (@End - @Start) + 1
	
	WHILE (@Start > 0 AND @End > 0 AND @Length > 0)
	BEGIN
	    SET @HTMLText = STUFF(@HTMLText, @Start, @Length, '>')
	    SET @Start = CHARINDEX('&gt;', @HTMLText)
	    SET @End = @Start + 3
	    SET @Length = (@End - @Start) + 1
	END
	
	-- Replace the HTML entity &amp; with the '&' character
	SET @Start = CHARINDEX('&amp;amp;', @HTMLText)
	SET @End = @Start + 4
	SET @Length = (@End - @Start) + 1
	
	WHILE (@Start > 0 AND @End > 0 AND @Length > 0)
	BEGIN
	    SET @HTMLText = STUFF(@HTMLText, @Start, @Length, '&')
	    SET @Start = CHARINDEX('&amp;amp;', @HTMLText)
	    SET @End = @Start + 4
	    SET @Length = (@End - @Start) + 1
	END
	
	-- Replace the HTML entity &nbsp; with the ' ' character
	SET @Start = CHARINDEX('&nbsp;', @HTMLText)
	SET @End = @Start + 5
	SET @Length = (@End - @Start) + 1
	
	WHILE (@Start > 0 AND @End > 0 AND @Length > 0)
	BEGIN
	    SET @HTMLText = STUFF(@HTMLText, @Start, @Length, ' ')
	    SET @Start = CHARINDEX('&nbsp;', @HTMLText)
	    SET @End = @Start + 5
	    SET @Length = (@End - @Start) + 1
	END
	
	-- Replace the HTML entity &quot; with the '"' character
	SET @Start = CHARINDEX('&quot;', @HTMLText)
	SET @End = @Start + 5
	SET @Length = (@End - @Start) + 1
	
	WHILE (@Start > 0 AND @End > 0 AND @Length > 0)
	BEGIN
	    SET @HTMLText = STUFF(@HTMLText, @Start, @Length, '"')
	    SET @Start = CHARINDEX('&quot;', @HTMLText)
	    SET @End = @Start + 5
	    SET @Length = (@End - @Start) + 1
	END
	
	-- Replace any <br> tags with a newline
	SET @Start = CHARINDEX('<br>', @HTMLText)
	SET @End = @Start + 3
	SET @Length = (@End - @Start) + 1
	
	WHILE (@Start > 0 AND @End > 0 AND @Length > 0)
	BEGIN
	    SET @HTMLText = STUFF(@HTMLText, @Start, @Length, CHAR(13) + CHAR(10))
	    --CHAR(13) + CHAR(10)
	    SET @Start = CHARINDEX('<br>', @HTMLText)
	    SET @End = @Start + 3
	    SET @Length = (@End - @Start) + 1
	END
	
	-- Replace any <br/> tags with a newline
	SET @Start = CHARINDEX('<br/>', @HTMLText)
	SET @End = @Start + 4
	SET @Length = (@End - @Start) + 1
	
	WHILE (@Start > 0 AND @End > 0 AND @Length > 0)
	BEGIN
	    SET @HTMLText = STUFF(@HTMLText, @Start, @Length, CHAR(13) + CHAR(10))
	    --CHAR(13) + CHAR(10)
	    SET @Start = CHARINDEX('<br/>', @HTMLText)
	    SET @End = @Start + 4
	    SET @Length = (@End - @Start) + 1
	END
	
	-- Replace any <br /> tags with a newline
	SET @Start = CHARINDEX('<br />', @HTMLText)
	SET @End = @Start + 5
	SET @Length = (@End - @Start) + 1
	
	WHILE (@Start > 0 AND @End > 0 AND @Length > 0)
	BEGIN
	    SET @HTMLText = STUFF(@HTMLText, @Start, @Length, CHAR(13) + CHAR(10))
	    --CHAR(13) + CHAR(10)
	    SET @Start = CHARINDEX('<br />', @HTMLText)
	    SET @End = @Start + 5
	    SET @Length = (@End - @Start) + 1
	END
	
	-- Remove anything between <whatever> tags
	SET @Start = CHARINDEX('<', @HTMLText)
	SET @End = CHARINDEX('>', @HTMLText, CHARINDEX('<', @HTMLText))
	SET @Length = (@End - @Start) + 1
	
	WHILE (@Start > 0 AND @End > 0 AND @Length > 0)
	BEGIN
	    SET @HTMLText = STUFF(@HTMLText, @Start, @Length, '')
	    SET @Start = CHARINDEX('<', @HTMLText)
	    SET @End = CHARINDEX('>', @HTMLText, CHARINDEX('<', @HTMLText))
	    SET @Length = (@End - @Start) + 1
	END
	
	RETURN LTRIM(RTRIM(@HTMLText))
END
GO







/*******************************************
 * Превью материалов
 *******************************************/
IF OBJECT_ID(N'dbo.get_preview_materials', N'P') IS NOT NULL
    DROP PROCEDURE dbo.get_preview_materials;
GO
CREATE PROCEDURE dbo.get_preview_materials(
    @lettersCount     INT,
    @gameTitle        VARCHAR(100),
    @categoryId       VARCHAR(100)
)
AS
BEGIN
	SELECT TOP(8) da.Id,
	       dm.Title,
	       dm.TitleUrl,
	       dm.DateCreate,
	       dm.DateOfPublication,
	       dm.ViewsCount,
	       dbo.get_comments_count(dm.Id, dm.ModelCoreType) AS CommentsCount,
	       CASE 
	            WHEN dm.Foreword IS NOT NULL THEN dm.Foreword
	            ELSE SUBSTRING(dbo.func_strip_html(dm.Html), 0, @lettersCount) +
	                 '...'
	       END                    AS Foreword,
	       anu.NikName            AS UserName,
	       dg.Title               AS GameTitle
	FROM   D_ARTICLE              AS da
	       JOIN DV_MATERIAL       AS dm
	            ON  dm.Id = da.Id
	            AND dm.ModelCoreType = da.ModelCoreType
	            AND (dm.Show = 1 AND dm.DateOfPublication <= GETDATE())
	       LEFT JOIN AspNetUsers  AS anu
	            ON  anu.Id = dm.UserId
	       LEFT JOIN D_GAME       AS dg
	            ON  dg.Id = da.GameId
	WHERE  (@gameTitle IS NULL)
	       OR  (
	               @gameTitle IS NOT NULL
	               AND @categoryId IS NULL
	               AND dg.TitleUrl = @gameTitle
	           )
	       OR  (
	               @gameTitle IS NOT NULL
	               AND @categoryId IS NOT NULL
	               AND dg.TitleUrl = @gameTitle
	               AND dm.CategoryId = @categoryId
	           )
	ORDER BY
	       dm.DateCreate DESC
END
GO

/*******************************************
 * получить материал
 *******************************************/
IF OBJECT_ID(N'dbo.get_material_by_url', N'P') IS NOT NULL
    DROP PROCEDURE dbo.get_material_by_url;
GO
CREATE PROCEDURE dbo.get_material_by_url(
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
 * получить видео материала
 *******************************************/
IF OBJECT_ID(N'dbo.get_material_videos', N'P') IS NOT NULL
    DROP PROCEDURE dbo.get_material_videos;
GO
CREATE PROCEDURE dbo.get_material_videos(@mid INT, @mct INT)
AS
BEGIN
	SELECT dv.*
	FROM   D_VIDEO_LINK         AS dvl
	       JOIN D_VIDEO         AS dv
	            ON  dv.Id = dvl.VideoId
	       LEFT JOIN D_ARTICLE  AS da
	            ON  da.ModelCoreType = dvl.ModelCoreType
	            AND da.Id = @mid
	       LEFT JOIN D_NEWS     AS dn
	            ON  dn.ModelCoreType = dvl.ModelCoreType
	            AND dn.Id = @mid
	WHERE  dvl.ModelCoreType = @mct
	       AND dvl.MaterialId = @mid
END
GO

/*******************************************
 * получить комментарии материала
 *******************************************/
IF OBJECT_ID(N'dbo.get_material_comments', N'P') IS NOT NULL
    DROP PROCEDURE dbo.get_material_comments;
GO
CREATE PROCEDURE dbo.get_material_comments(@mid INT, @mct INT)
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
 * Функция получения кол-ва комментариев материала
 *******************************************/
IF OBJECT_ID(N'dbo.get_comments_count', N'FN') IS NOT NULL
    DROP FUNCTION dbo.get_comments_count;
GO
CREATE FUNCTION dbo.get_comments_count
(
	@mid     INT,
	@mct     INT
)
RETURNS INT
AS
BEGIN
	DECLARE @res INT
	SELECT @res = COUNT(1)
	FROM   D_COMMENT AS dc
	WHERE  dc.MaterialId = @mid
	       AND dc.ModelCoreType = @mct
	
	RETURN @res
END
GO







/*******************************************
 * добавить комментарии материала
 *******************************************/
IF OBJECT_ID(N'dbo.add_material_comment', N'P') IS NOT NULL
    DROP PROCEDURE dbo.add_material_comment;
GO
CREATE PROCEDURE dbo.add_material_comment(
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
 * Удалить категорию материалов
 *******************************************/
IF OBJECT_ID(N'dbo.del_material_category', N'P') IS NOT NULL
    DROP PROCEDURE dbo.del_material_category;
GO
CREATE PROCEDURE dbo.del_material_category(@catId VARCHAR(100))
AS
BEGIN
	BEGIN TRANSACTION
	
	DECLARE @idForDel TABLE (Id VARCHAR(100));
	WITH j(Id, ParentCategoryId) AS
	     (
	         SELECT dmc1.Id,
	                dmc1.ParentCategoryId
	         FROM   D_MATERIAL_CATEGORY AS dmc1
	         WHERE  dmc1.Id = @catId
	         UNION ALL
	         SELECT dmc2.Id,
	                dmc2.ParentCategoryId
	         FROM   D_MATERIAL_CATEGORY AS dmc2
	                JOIN j
	                     ON  dmc2.ParentCategoryId = j.Id
	     )
	
	INSERT INTO @idForDel
	SELECT j.Id
	FROM   j AS j
	
	--Удалить афоризмы категории
	DELETE 
	FROM   D_APHORISM
	WHERE  Id IN (SELECT dm.Id
	              FROM   DV_MATERIAL      AS dm
	                     JOIN D_APHORISM  AS da
	                          ON  da.Id = dm.Id
	                          AND da.ModelCoreType = dm.ModelCoreType
	              WHERE  dm.CategoryId IN (SELECT fd.Id
	                                       FROM   @idForDel fd))
	
	DELETE 
	FROM   DV_MATERIAL
	WHERE  TitleUrl IN (SELECT dm.TitleUrl
	                    FROM   DV_MATERIAL AS dm
	                           JOIN D_APHORISM AS da
	                                ON  da.Id = dm.Id
	                                AND da.ModelCoreType = dm.ModelCoreType
	                    WHERE  dm.CategoryId IN (SELECT fd.Id
	                                             FROM   @idForDel fd))
	
	
	--Обновить статьи и новости
	UPDATE DV_MATERIAL
	SET    CategoryId = NULL
	WHERE  CategoryId IN (SELECT fd.Id
	                      FROM   @idForDel fd) 
	
	--Удалить категорию	         
	DELETE 
	FROM   D_MATERIAL_CATEGORY
	WHERE  Id IN (SELECT fd.Id
	              FROM   @idForDel fd) 
	
	COMMIT TRANSACTION
END
GO
   
/*******************************************
* получить страницу афоризма
*******************************************/
IF OBJECT_ID(N'dbo.get_aphorism_page_model', N'P') IS NOT NULL
    DROP PROCEDURE dbo.get_aphorism_page_model;
GO
CREATE PROCEDURE dbo.get_aphorism_page_model
(
    @title_url         NVARCHAR(255),
    @author_amount     INT,
    @cat_amount        INT
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
	
	SELECT x.Id,
	       x.DateOfPublication,
	       x.ViewsCount,
	       x.Flag,
	       x.Title,
	       x.TitleUrl,
	       x.Html,
	       x.CategoryId,
	       dmc.Title,
	       dmc.Id,
	       x.AuthorId,
	       daa.Name,
	       daa.PictureId,
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
	                  AND dm.Show = 1
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
	                  AND (dm.TitleUrl NOT IN (@title_url))
	                  AND dm.Show = 1
	           UNION ALL
	           SELECT TOP(@cat_amount) dm.*,
	                  da.AuthorId,
	                  2                 AS Flag
	           FROM   D_APHORISM        AS da
	                  JOIN DV_MATERIAL  AS dm
	                       ON  dm.Id = da.Id
	                       AND dm.ModelCoreType = da.ModelCoreType
	           WHERE  dm.CategoryId IN (@catId)
	                  AND (dm.TitleUrl NOT IN (@title_url))
	                  AND dm.Show = 1
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
	       x.DateOfPublication,
	       x.ViewsCount,
	       x.Title,
	       x.TitleUrl,
	       x.Html,
	       x.CategoryId,
	       x.AuthorId,
	       x.Flag,
	       dmc.Title,
	       daa.Name,
	       daa.PictureId,
	       dmc.Id
END
GO

/*******************************************
* Получить случайный афоризм
*******************************************/
IF OBJECT_ID(N'dbo.get_random_aphorism', N'P') IS NOT NULL
    DROP PROCEDURE dbo.get_random_aphorism;
GO
CREATE PROCEDURE dbo.get_random_aphorism(@mid INT)
AS
BEGIN
	SELECT TOP(1) *
	FROM   D_APHORISM                   AS da
	       JOIN DV_MATERIAL             AS dm
	            ON  dm.Id = da.Id
	            AND dm.ModelCoreType = da.ModelCoreType
	       JOIN D_MATERIAL_CATEGORY     AS dmc
	            ON  dmc.Id = dm.CategoryId
	       LEFT JOIN D_AUTHOR_APHORISM  AS daa
	            ON  daa.Id = da.AuthorId
	WHERE  (@mid IS NULL)
	       OR  (@mid IS NOT NULL AND da.Id NOT IN (@mid))
	       AND dm.CategoryId IS NOT NULL
	ORDER BY
	       NEWID()
END
GO


/*******************************************
* получить категории афоризмов
*******************************************/
IF OBJECT_ID(N'dbo.get_aphorism_categories', N'P') IS NOT NULL
    DROP PROCEDURE dbo.get_aphorism_categories;
GO
CREATE PROCEDURE dbo.get_aphorism_categories(@curCat VARCHAR(100))
AS
BEGIN
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
END
GO

/*******************************************
 * Популярные материалы
 *******************************************/
IF OBJECT_ID(N'dbo.get_popular_materials', N'P') IS NOT NULL
    DROP PROCEDURE dbo.get_popular_materials;
GO
CREATE PROCEDURE dbo.get_popular_materials(@mid INT, @mct INT, @amount INT)
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
IF OBJECT_ID(N'dbo.get_other_materials', N'P') IS NOT NULL
    DROP PROCEDURE dbo.get_other_materials;
GO
CREATE PROCEDURE dbo.get_other_materials
(@mid INT, @mct INT, @dir BIT, @amount INT = 3)
AS
BEGIN
	DECLARE @date DATETIME
	SELECT @date = dm.DateOfPublication
	FROM   DV_MATERIAL AS dm
	WHERE  dm.Id = @mid
	       AND dm.ModelCoreType = @mct
	
	IF (@dir = 1)
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
IF OBJECT_ID(N'dbo.get_banned_urls', N'P') IS NOT NULL
    DROP PROCEDURE dbo.get_banned_urls;
GO
CREATE PROCEDURE dbo.get_banned_urls
AS
BEGIN
	SELECT dbu.[Url]
	FROM   D_BANNED_URL AS dbu
END
GO

/*******************************************
 * Детализированная страница по игре
 *******************************************/
IF OBJECT_ID(N'dbo.get_game_by_url', N'P') IS NOT NULL
    DROP PROCEDURE dbo.get_game_by_url;
GO
CREATE PROCEDURE dbo.get_game_by_url(@titleUrl VARCHAR(50))
AS
BEGIN
	SELECT *
	FROM   D_GAME AS dg
	WHERE  dg.TitleUrl = @titleUrl
	       AND dg.Show = 1
END
GO

/*******************************************
 * Последние материалы по игре
 *******************************************/
IF OBJECT_ID(N'dbo.get_game_materials', N'P') IS NOT NULL
    DROP PROCEDURE dbo.get_game_materials;
GO
CREATE PROCEDURE dbo.get_game_materials(@titleUrl VARCHAR(50), @amount INT)
AS
BEGIN
	SELECT x.Id,
	       x.ModelCoreType,
	       x.Title,
	       x.TitleUrl,
	       x.DateCreate,
	       x.DateOfPublication,
	       CASE 
	            WHEN x.ModelCoreType = 6 THEN x.Html
	            ELSE x.Foreword
	       END  AS Foreword,
	       CASE 
	            WHEN x.ModelCoreType = 6 THEN x.PictureId
	            ELSE x.FrontPictureId
	       END  AS FrontPictureId,
	       x.CategoryId,
	       (
	           SELECT TOP(1) dvl.VideoId
	           FROM   D_VIDEO_LINK AS dvl
	           WHERE  dvl.MaterialId = x.Id
	                  AND dvl.ModelCoreType = x.ModelCoreType
	       )    AS TopVideoId,
	       dbo.get_comments_count(x.Id, x.ModelCoreType) AS CommentsCount
	FROM   (
	           SELECT TOP(@amount) dm.*,
	                  NULL              AS PictureId,
	                  dn.GameId
	           FROM   D_NEWS            AS dn
	                  JOIN DV_MATERIAL  AS dm
	                       ON  dm.Id = dn.Id
	                       AND dm.ModelCoreType = dn.ModelCoreType
	                  JOIN D_GAME       AS dg
	                       ON  dg.Id = dn.GameId
	                       AND dg.TitleUrl = @titleUrl
	                       AND dg.Show = 1
	           WHERE  dm.Show = 1
	                  AND dm.DateOfPublication <= GETDATE()
	           UNION ALL
	           SELECT TOP(@amount) dm.*,
	                  NULL              AS PictureId,
	                  da.GameId
	           FROM   D_ARTICLE         AS da
	                  JOIN DV_MATERIAL  AS dm
	                       ON  dm.Id = da.Id
	                       AND dm.ModelCoreType = da.ModelCoreType
	                  JOIN D_GAME       AS dg
	                       ON  dg.Id = da.GameId
	                       AND dg.TitleUrl = @titleUrl
	                       AND dg.Show = 1
	           WHERE  dm.Show = 1
	                  AND dm.DateOfPublication <= GETDATE()
	           UNION ALL
	           SELECT TOP(@amount) dm.*,
	                  daa.PictureId,
	                  dmc.GameId
	           FROM   D_APHORISM        AS da2
	                  LEFT JOIN D_AUTHOR_APHORISM AS daa
	                       ON  daa.Id = da2.AuthorId
	                  JOIN DV_MATERIAL  AS dm
	                       ON  dm.Id = da2.Id
	                       AND dm.ModelCoreType = da2.ModelCoreType
	                  JOIN D_MATERIAL_CATEGORY AS dmc
	                       ON  dmc.Id = dm.CategoryId
	                  JOIN D_GAME       AS dg
	                       ON  dg.Id = dmc.GameId
	                       AND dg.Show = 1
	                       AND dg.TitleUrl = @titleUrl
	           WHERE  dm.Show = 1
	                  AND dm.DateOfPublication <= GETDATE()
	           ORDER BY
	                  dm.DateOfPublication DESC
	       )    AS x
END
GO

/*******************************************
 * Получить видео для игры
 *******************************************/
IF OBJECT_ID(N'dbo.get_game_videos', N'P') IS NOT NULL
    DROP PROCEDURE dbo.get_game_videos;
GO
CREATE PROCEDURE dbo.get_game_videos(@titleUrl VARCHAR(50))
AS
BEGIN
	SELECT dv.*
	FROM   D_VIDEO                AS dv
	       JOIN D_VIDEO_LINK      AS dvl
	            ON  dvl.VideoId = dv.Id
	       LEFT JOIN DV_MATERIAL  AS dm
	            ON  dm.Id = dvl.MaterialId
	            AND dm.ModelCoreType = dvl.ModelCoreType
	            AND dm.Show = 1
	            AND dm.DateOfPublication <= GETDATE()
	       LEFT JOIN D_NEWS       AS dn
	            ON  dn.Id = dm.Id
	            AND dn.ModelCoreType = dm.ModelCoreType
	       LEFT JOIN D_ARTICLE    AS da
	            ON  da.Id = dm.Id
	            AND da.ModelCoreType = dm.ModelCoreType
	       LEFT JOIN D_GAME       AS dg
	            ON  (dg.Id = dn.GameId OR dg.Id = da.GameId)
	            AND dg.Show = 1
	WHERE  (dn.GameId IS NOT NULL OR da.GameId IS NOT NULL)
	       AND dg.TitleUrl = @titleUrl
END
GO

/*******************************************
 * Добавить сотрудника
 *******************************************/
IF OBJECT_ID(N'dbo.add_employee', N'P') IS NOT NULL
    DROP PROCEDURE dbo.add_employee;
GO
CREATE PROCEDURE dbo.add_employee(@uid VARCHAR(128))
AS
BEGIN
	INSERT INTO D_EMPLOYEE
	  (
	    Id,
	    DateCreate
	  )
	VALUES
	  (
	    @uid,
	    GETDATE()
	  )
END
GO

/*******************************************
 * Удалить сотрудника
 *******************************************/
IF OBJECT_ID(N'dbo.del_employee', N'P') IS NOT NULL
    DROP PROCEDURE dbo.del_employee;
GO
CREATE PROCEDURE dbo.del_employee(@uid VARCHAR(128))
AS
BEGIN
	DELETE 
	FROM   D_EMPLOYEE
	WHERE  Id = @uid
END
GO

/*******************************************
 * Сотрудники
 *******************************************/
IF OBJECT_ID(N'dbo.get_employees', N'P') IS NOT NULL
    DROP PROCEDURE dbo.get_employees;
GO
CREATE PROCEDURE dbo.get_employees(@id VARCHAR(128))
AS
BEGIN
	SELECT *
	FROM   D_EMPLOYEE        AS de
	       JOIN AspNetUsers  AS anu
	            ON  anu.Id = de.Id
	WHERE  (@id IS NULL OR de.Id = @id)
END
GO

/*******************************************
 * Увеличить количество просмотров
 *******************************************/
IF OBJECT_ID(N'dbo.add_material_view', N'P') IS NOT NULL
    DROP PROCEDURE dbo.add_material_view;
GO
CREATE PROCEDURE dbo.add_material_view(@mid INT, @mct INT)
AS
BEGIN
	DECLARE @oldCount INT
	SELECT @oldCount = dm.ViewsCount
	FROM   DV_MATERIAL AS dm
	WHERE  dm.Id = @mid
	       AND dm.ModelCoreType = @mct
	
	UPDATE DV_MATERIAL
	SET    ViewsCount            = @oldCount + 1
	WHERE  Id                    = @mid
	       AND ModelCoreType     = @mct
END
GO

/*******************************************
 * Баннеры
 *******************************************/
IF OBJECT_ID(N'dbo.get_banners', N'P') IS NOT NULL
    DROP PROCEDURE dbo.get_banners;
GO
CREATE PROCEDURE dbo.get_banners(@id UNIQUEIDENTIFIER, @place INT)
AS
BEGIN
	SELECT db.*,
	       dp.Id,
	       dp.Caption
	FROM   D_BANNER             AS db
	       LEFT JOIN D_PICTURE  AS dp
	            ON  dp.Id = db.PictureId
	WHERE  (@id IS NULL OR db.Id = @id)
	       AND (@place IS NULL OR db.Place = @place)
END
GO

/*******************************************
 * Обновить баннер
 *******************************************/
IF OBJECT_ID(N'dbo.update_banner', N'P') IS NOT NULL
    DROP PROCEDURE dbo.update_banner;
GO
CREATE PROCEDURE dbo.update_banner(
    @id             UNIQUEIDENTIFIER,
    @url            VARCHAR(255),
    @pid            UNIQUEIDENTIFIER,
    @title          NVARCHAR(100),
    @place          INT,
    @controller     NVARCHAR(50),
    @action         NVARCHAR(50)
)
AS
BEGIN
	UPDATE D_BANNER
	SET    Title              = @title,
	       PictureId          = @pid,
	       [Url]              = @url,
	       DateUpdate         = GETDATE(),
	       ControllerName     = @controller,
	       ActionName         = @action,
	       Place              = @place
	WHERE  Id                 = @id
END
GO

/*******************************************
 * Обновить количество кликов баннера
 *******************************************/
IF OBJECT_ID(N'dbo.add_banner_clicks_count', N'P') IS NOT NULL
    DROP PROCEDURE dbo.add_banner_clicks_count;
GO
CREATE PROCEDURE dbo.add_banner_clicks_count(@id UNIQUEIDENTIFIER)
AS
BEGIN
	DECLARE @count INT
	SELECT TOP(1) @count = db.ClicksCount
	FROM   D_BANNER AS db
	WHERE  db.Id = @id
	
	UPDATE D_BANNER
	SET    ClicksCount     = @count + 1
	WHERE  Id              = @id
END
GO