/************************************************************
 * Code formatted by SoftTree SQL Assistant © v6.5.278
 * Time: 17.05.2016 17:01:58
 ************************************************************/

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
	           AND (dm.Id IN (@mid)
	           OR  (dm.Id NOT IN (@mid) AND dm.DateOfPublication > @date))
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
	           AND (dm.Id IN (@mid)
	           OR  (dm.Id NOT IN (@mid) AND dm.DateOfPublication < @date))
	    ORDER BY
	           dm.DateOfPublication DESC
	
	RETURN
END