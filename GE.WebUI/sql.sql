/************************************************************
 * Code formatted by SoftTree SQL Assistant © v6.5.278
 * Time: 17.05.2016 2:33:31
 ************************************************************/

/*******************************************
 * получить страницу афоризма
 *******************************************/
IF OBJECT_ID(N'get_aphorism_page_model', N'TF') IS NOT NULL
    DROP FUNCTION get_aphorism_page_model;
GO
CREATE FUNCTION get_aphorism_page_model
(
	@title_url         VARCHAR(255),
	@author_amount     INT,
	@cat_amount        INT
)
RETURNS @result TABLE(
            Id INT,
            Title VARCHAR(255),
            TitleUrl VARCHAR(255),
            Html VARCHAR(MAX),
            CategoryId VARCHAR(100),
            CategoryTitle VARCHAR(100),
            AuthorId INT,
            AuthorName VARCHAR(100),
            AuthorPictureId UNIQUEIDENTIFIER,
            Flag INT,
            CommentsCount INT
        )
AS
BEGIN
	DECLARE @authorId     INT,
	        @catId        VARCHAR(100)
	
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
	                  AND da.AuthorId NOT IN (@authorId)
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