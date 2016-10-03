/************************************************************
 * Code formatted by SoftTree SQL Assistant © v6.5.278
 * Time: 30.09.2016 11:43:25
 ************************************************************/

/*******************************************
 * Получить игру
 *******************************************/
IF OBJECT_ID(N'dbo.get_game', N'P') IS NOT NULL
    DROP PROCEDURE dbo.get_game;
GO
CREATE PROCEDURE dbo.get_game
	@id INT
AS
BEGIN
	SELECT TOP(2) dg.*,
	       dp.Id,
	       dp.Caption,
	       dp2.Id,
	       dp2.Caption,
	       dp3.Id,
	       dp3.Caption
	FROM   D_GAME               AS dg
	       LEFT JOIN D_PICTURE  AS dp
	            ON  dp.Id = dg.FrontPictureId
	       LEFT JOIN D_PICTURE  AS dp2
	            ON  dp2.Id = dg.GoodPictureId
	       LEFT JOIN D_PICTURE  AS dp3
	            ON  dp3.Id = dg.BadPictureId
	WHERE  dg.Id = 1
END
GO

/*******************************************
 * Добавить категорию материалов (переопределено)
 *******************************************/
IF OBJECT_ID(N'dbo.add_material_category', N'P') IS NOT NULL
    DROP PROCEDURE dbo.add_material_category;
GO
CREATE PROCEDURE dbo.add_material_category
	@categoryId NVARCHAR(128),
	@title NVARCHAR(100),
	@mct INT,
	@pcid NVARCHAR(128),
	@pictureId UNIQUEIDENTIFIER,
	@gameId INT
AS
BEGIN
	IF NOT EXISTS (
	       SELECT TOP 1 dmc.Id
	       FROM   D_MATERIAL_CATEGORY AS dmc
	       WHERE  dmc.Id = @categoryId
	   )
	BEGIN
	    DECLARE @date DATETIME = GETDATE()
	    INSERT INTO D_MATERIAL_CATEGORY
	      (
	        Id,
	        Title,
	        ModelCoreType,
	        ParentId,
	        FrontPictureId,
	        DateCreate,
	        GameId,
	        Discriminator
	      )
	    VALUES
	      (
	        @categoryId,
	        @title,
	        @mct,
	        @pcid,
	        @pictureId,
	        @date,
	        @gameId,
	        'SxMaterialCategory'
	      )
	    
	    EXEC dbo.get_material_category @categoryId
	END
END
GO

/*******************************************
 * Обновить категорию материалов
 *******************************************/
IF OBJECT_ID(N'dbo.update_material_category', N'P') IS NOT NULL
    DROP PROCEDURE dbo.update_material_category;
GO
CREATE PROCEDURE dbo.update_material_category
	@oldCategoryId NVARCHAR(128),
	@categoryId NVARCHAR(128),
	@title NVARCHAR(100),
	@mct INT,
	@pcid NVARCHAR(128),
	@pictureId UNIQUEIDENTIFIER,
	@gameId INT
AS
BEGIN
	IF (@oldCategoryId = @categoryId)
	BEGIN
	    UPDATE D_MATERIAL_CATEGORY
	    SET    Title                = @title,
	           ModelCoreType        = @mct,
	           ParentId     = @pcid,
	           FrontPictureId       = @pictureId
	    WHERE  Id                   = @categoryId
	END
	ELSE
	BEGIN
	    IF NOT EXISTS (
	           SELECT TOP 1 dmc.Id
	           FROM   D_MATERIAL_CATEGORY AS dmc
	           WHERE  dmc.Id = @categoryId
	       )
	    BEGIN
	        BEGIN TRANSACTION
	        
	        ALTER TABLE [dbo].[DV_MATERIAL] DROP CONSTRAINT 
	        [FK_dbo.DV_MATERIAL_dbo.D_MATERIAL_CATEGORY_CategoryId];
	        
	        UPDATE DV_MATERIAL
	        SET    CategoryId = @categoryId
	        WHERE  CategoryId = @oldCategoryId
	        
	        --PRINT '1'
	        
	        UPDATE D_MATERIAL_CATEGORY
	        SET    ParentId = @categoryId
	        WHERE  ParentId = @oldCategoryId
	        
	        --PRINT '2'
	        
	        UPDATE D_MATERIAL_CATEGORY
	        SET    Id = @categoryId,
	               Title = @title,
	               ModelCoreType = @mct,
	               ParentId = @pcid,
	               FrontPictureId = @pictureId,
	               GameId = @gameId
	        WHERE  Id = @oldCategoryId
	        
	        --PRINT '3'
	        
	        ALTER TABLE [dbo].[DV_MATERIAL]  
	        WITH CHECK ADD CONSTRAINT 
	             [FK_dbo.DV_MATERIAL_dbo.D_MATERIAL_CATEGORY_CategoryId] FOREIGN 
	             KEY([CategoryId])
	             REFERENCES [dbo].[D_MATERIAL_CATEGORY] ([Id]);
	        
	        --PRINT '4'
	        
	        ALTER TABLE [dbo].[DV_MATERIAL] CHECK CONSTRAINT 
	        [FK_dbo.DV_MATERIAL_dbo.D_MATERIAL_CATEGORY_CategoryId];
	        
	        --PRINT '5'
	        
	        COMMIT TRANSACTION
	    END
	END
	
	EXEC dbo.get_material_category @categoryId
END
GO

/*******************************************
* удалить автора афоризмов
*******************************************/
IF OBJECT_ID(N'dbo.del_author_aphorism', N'P') IS NOT NULL
    DROP PROCEDURE dbo.del_author_aphorism;
GO
CREATE PROCEDURE dbo.del_author_aphorism(@authorId INT)
AS
BEGIN
	UPDATE D_APHORISM
	SET    AuthorId = NULL
	WHERE  AuthorId = @authorId
	
	DELETE 
	FROM   D_AUTHOR_APHORISM
	WHERE  Id = @authorId
END
GO