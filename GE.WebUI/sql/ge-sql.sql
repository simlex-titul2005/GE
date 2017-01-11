/************************************************************
 * Code formatted by SoftTree SQL Assistant © v6.5.278
 * Time: 12.01.2017 1:49:42
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
	WHERE  dg.Id = @id
END
GO

/*******************************************
 * Детализированная страница по игре
 *******************************************/
IF OBJECT_ID(N'dbo.get_game_by_url', N'P') IS NOT NULL
    DROP PROCEDURE dbo.get_game_by_url;
GO
CREATE PROCEDURE dbo.get_game_by_url
	@titleUrl VARCHAR(50)
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
CREATE PROCEDURE dbo.get_game_materials
	@titleUrl VARCHAR(50),
	@amount INT
AS
BEGIN
	SELECT x.Id,
	       x.ModelCoreType,
	       x.Title,
	       x.TitleUrl,
	       x.DateCreate,
	       x.DateOfPublication,
	       x.ViewsCount,
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
	           ORDER BY
	                  dm.DateOfPublication DESC
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
	           ORDER BY
	                  dm.DateOfPublication DESC
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
* удалить автора афоризмов
*******************************************/
IF OBJECT_ID(N'dbo.del_author_aphorism', N'P') IS NOT NULL
    DROP PROCEDURE dbo.del_author_aphorism;
GO
CREATE PROCEDURE dbo.del_author_aphorism
	@authorId INT
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

/*******************************************
* получить игру для материалов
*******************************************/
IF OBJECT_ID(N'dbo.get_material_game', N'P') IS NOT NULL
    DROP PROCEDURE dbo.get_material_game;
GO
CREATE PROCEDURE dbo.get_material_game
	@id INT,
	@mct INT
AS
BEGIN
	SELECT TOP(2)
	       dg.Id,
	       dg.Title,
	       da.GameVersion       AS ArticleGameVersion,
	       dn.GameVersion       AS NewsGameVersion
	FROM   DV_MATERIAL          AS dm
	       LEFT JOIN D_ARTICLE  AS da
	            ON  da.ModelCoreType = dm.ModelCoreType
	            AND da.Id = dm.Id
	       LEFT JOIN D_NEWS     AS dn
	            ON  dn.ModelCoreType = dm.ModelCoreType
	            AND dn.Id = dm.Id
	       JOIN D_GAME          AS dg
	            ON  dg.Id = da.GameId OR dg.Id = dn.GameId
	WHERE  dm.Id = @id
	       AND dm.ModelCoreType = @mct
END
GO

/*******************************************
 * добавить тест сайта
 *******************************************/
IF OBJECT_ID(N'dbo.add_site_test', N'P') IS NOT NULL
    DROP PROCEDURE dbo.add_site_test;
GO
CREATE PROCEDURE dbo.add_site_test
	@title NVARCHAR(200),
	@desc NVARCHAR(1000),
	@rules NVARCHAR(MAX),
	@titleUrl VARCHAR(255),
	@type INT,
	@show BIT
AS
BEGIN
	IF NOT EXISTS (
	       SELECT TOP(1) *
	       FROM   D_SITE_TEST AS dst
	       WHERE  dst.TitleUrl = @titleUrl
	   )
	BEGIN
	    INSERT INTO D_SITE_TEST
	      (
	        Title,
	        [Description],
	        Rules,
	        DateUpdate,
	        DateCreate,
	        TitleUrl,
	        [Type],
	        Show,
	        ViewsCount,
	        ShowSubjectDesc
	      )
	    VALUES
	      (
	        @title,
	        @desc,
	        @rules,
	        GETDATE(),
	        GETDATE(),
	        @titleUrl,
	        @type,
	        @show,
	        0,
	        0
	      )
	    
	    DECLARE @id INT
	    SET @id = @@identity
	    
	    SELECT TOP(2) *
	    FROM   D_SITE_TEST AS dst
	    WHERE  dst.Id = @id
	END
END
GO

/*******************************************
 * Правила теста
 *******************************************/
IF OBJECT_ID(N'dbo.get_site_test_rules', N'P') IS NOT NULL
    DROP PROCEDURE dbo.get_site_test_rules;
GO
CREATE PROCEDURE dbo.get_site_test_rules
	@testId INT
AS
BEGIN
	SELECT TOP 1 dst.Title,
	       dst.Rules
	FROM   D_SITE_TEST AS dst
	WHERE  dst.Id = @testId
END
GO


 
/*******************************************
 * удалить тест сайта
 *******************************************/
IF OBJECT_ID(N'dbo.del_site_test', N'P') IS NOT NULL
    DROP PROCEDURE dbo.del_site_test;
GO
CREATE PROCEDURE dbo.del_site_test
	@testId INT
AS
	BEGIN TRANSACTION
	
	DELETE 
	FROM   D_SITE_TEST_ANSWER
	WHERE  SubjectId IN (SELECT dsts.Id
	                     FROM   D_SITE_TEST_SUBJECT AS dsts
	                     WHERE  dsts.TestId = @testId)
	
	DELETE 
	FROM   D_SITE_TEST
	WHERE  Id = @testId
	
	COMMIT TRANSACTION
GO

/*******************************************
 * добавить вопрос теста
 *******************************************/
IF OBJECT_ID(N'dbo.add_site_test_question', N'P') IS NOT NULL
    DROP PROCEDURE dbo.add_site_test_question;
GO
CREATE PROCEDURE dbo.add_site_test_question
	@testId INT,
	@text NVARCHAR(500)
AS
BEGIN
	IF NOT EXISTS (
	       SELECT TOP(1) *
	       FROM   D_SITE_TEST_QUESTION AS dstq
	       WHERE  dstq.TestId = @testId
	              AND dstq.[Text] = @text
	   )
	BEGIN
	    INSERT INTO D_SITE_TEST_QUESTION
	      (
	        TestId,
	        [Text],
	        DateUpdate,
	        DateCreate
	      )
	    VALUES
	      (
	        @testId,
	        @text,
	        GETDATE(),
	        GETDATE()
	      )
	    
	    DECLARE @id INT
	    SET @id = @@identity
	    
	    INSERT INTO D_SITE_TEST_ANSWER
	      (
	        QuestionId,
	        SubjectId,
	        DateUpdate,
	        DateCreate,
	        IsCorrect
	      )
	    SELECT @id,
	           dsts.Id,
	           GETDATE(),
	           GETDATE(),
	           0
	    FROM   D_SITE_TEST_SUBJECT AS dsts
	    WHERE  dsts.TestId = @testId
	    
	    SELECT TOP(1) *
	    FROM   D_SITE_TEST_QUESTION AS dstq
	    WHERE  dstq.Id = @id
	END
END
GO



 /*******************************************
 * удалить вопрос теста
 *******************************************/
IF OBJECT_ID(N'dbo.del_site_test_question', N'P') IS NOT NULL
    DROP PROCEDURE dbo.del_site_test_question;
GO
CREATE PROCEDURE dbo.del_site_test_question
	@questionId INT
AS
	BEGIN TRANSACTION
	
	DELETE 
	FROM   D_SITE_TEST_ANSWER
	WHERE  QuestionId = @questionId
	
	DELETE 
	FROM   D_SITE_TEST_QUESTION
	WHERE  Id = @questionId
	
	COMMIT TRANSACTION
GO

/*******************************************
 * добавить объект теста
 *******************************************/
IF OBJECT_ID(N'dbo.add_site_test_subject', N'P') IS NOT NULL
    DROP PROCEDURE dbo.add_site_test_subject;
GO
CREATE PROCEDURE dbo.add_site_test_subject
	@testId INT,
	@title NVARCHAR(400),
	@desc NVARCHAR(MAX),
	@pictureId UNIQUEIDENTIFIER
AS
BEGIN
	IF NOT EXISTS (
	       SELECT TOP(1) *
	       FROM   D_SITE_TEST_SUBJECT AS dsts
	       WHERE  dsts.TestId = @testId
	              AND dsts.Title = @title
	   )
	BEGIN
	    INSERT INTO D_SITE_TEST_SUBJECT
	      (
	        Title,
	        [Description],
	        TestId,
	        DateUpdate,
	        DateCreate,
	        PictureId
	      )
	    VALUES
	      (
	        @title,
	        @desc,
	        @testId,
	        GETDATE(),
	        GETDATE(),
	        @pictureId
	      )
	    
	    DECLARE @id INT
	    SET @id = @@identity
	    
	    INSERT INTO D_SITE_TEST_ANSWER
	      (
	        QuestionId,
	        SubjectId,
	        DateUpdate,
	        DateCreate,
	        IsCorrect
	      )
	    SELECT dstq.Id,
	           @id,
	           GETDATE(),
	           GETDATE(),
	           0
	    FROM   D_SITE_TEST_QUESTION AS dstq
	    WHERE  dstq.TestId = @testId
	    
	    SELECT TOP(1) *
	    FROM   D_SITE_TEST_SUBJECT AS dsts
	    WHERE  dsts.Id = @id
	END
END
GO

/*******************************************
 * удалить объект теста
 *******************************************/
IF OBJECT_ID(N'dbo.del_site_test_subject', N'P') IS NOT NULL
    DROP PROCEDURE dbo.del_site_test_subject;
GO
CREATE PROCEDURE dbo.del_site_test_subject
	@subjectId INT
AS
	BEGIN TRANSACTION
	
	DELETE 
	FROM   D_SITE_TEST_ANSWER
	WHERE  SubjectId = @subjectId
	
	DELETE 
	FROM   D_SITE_TEST_SUBJECT
	WHERE  Id = @subjectId
	
	COMMIT TRANSACTION
GO

/*******************************************
 * Страницы прохождения теста
 *******************************************/
IF OBJECT_ID(N'dbo.get_site_test_page', N'P') IS NOT NULL
    DROP PROCEDURE dbo.get_site_test_page;
GO
CREATE PROCEDURE dbo.get_site_test_page
	@titleUrl VARCHAR(255)
AS
BEGIN
	SELECT TOP(1) *
	FROM   D_SITE_TEST_ANSWER             AS dsta
	       JOIN D_SITE_TEST_QUESTION      AS dstq
	            ON  dstq.Id = dsta.QuestionId
	       JOIN D_SITE_TEST_SUBJECT       AS dsts
	            ON  dsts.Id = dsta.SubjectId
	       JOIN D_SITE_TEST               AS dst
	            ON  dst.Id = dstq.TestId
	            AND dst.TitleUrl = @titleUrl
	            AND dst.Show = 1
	       LEFT JOIN D_SITE_TEST_SETTING  AS dsts2
	            ON  dsts2.TestId = dst.Id
	ORDER BY
	       NEWID()
END
GO

/*******************************************
 * Тип для предыдущих ответов теста
 *******************************************/
IF OBJECT_ID(N'dbo.get_site_test_next_guess_step', N'P') IS NOT NULL
    DROP PROCEDURE dbo.get_site_test_next_guess_step;
GO

IF TYPE_ID(N'dbo.OldSiteTestStepGuess') IS NOT NULL
    DROP TYPE dbo.OldSiteTestStepGuess
GO

CREATE TYPE dbo.OldSiteTestStepGuess AS TABLE 
(QuestionId INT, IsCorrect BIT, [Order] INT);  
GO

/*******************************************
 * Следующий вопрос теста Guess
 *******************************************/
CREATE PROCEDURE dbo.get_site_test_next_guess_step
	@oldSteps dbo.OldSiteTestStepGuess READONLY,
	@subjectsCount INT OUTPUT
AS
BEGIN
	DECLARE @subjects TABLE (SubjectId INT)
	DECLARE @questions TABLE (QuestionId INT)
	
	BEGIN
		DECLARE @questionId     INT,
		        @isCorrect      INT
		
		DECLARE c CURSOR FORWARD_ONLY LOCAL READ_ONLY 
		FOR
		    SELECT os.QuestionId,
		           os.IsCorrect
		    FROM   @oldSteps AS os
		    ORDER BY
		           os.[Order]
		
		OPEN c
		FETCH NEXT FROM c INTO @questionId, @isCorrect
		WHILE @@FETCH_STATUS = 0
		BEGIN
		    INSERT INTO @subjects
		    SELECT DISTINCT dsta.SubjectId
		    FROM   D_SITE_TEST_ANSWER AS dsta
		    WHERE  dsta.QuestionId = @questionId
		           AND dsta.IsCorrect = @isCorrect
		           AND (
		                   dsta.SubjectId IN (SELECT s.SubjectId
		                                      FROM   @subjects AS s)
		                   OR (
		                          SELECT COUNT(1)
		                          FROM   @subjects
		                      ) = 0
		               )
		    
		    DELETE 
		    FROM   @subjects
		    WHERE  SubjectId IN (SELECT DISTINCT dsta.SubjectId
		                         FROM   D_SITE_TEST_ANSWER AS dsta
		                         WHERE  dsta.QuestionId = @questionId
		                                AND dsta.IsCorrect <> @isCorrect)
		    
		    FETCH NEXT FROM c INTO @questionId, @isCorrect
		END
		CLOSE c
		DEALLOCATE c
	END
	
	INSERT INTO @questions
	SELECT dsta.QuestionId
	FROM   D_SITE_TEST_ANSWER  AS dsta
	       JOIN @subjects      AS s
	            ON  s.SubjectId = dsta.SubjectId
	WHERE  dsta.QuestionId NOT IN (SELECT os.QuestionId
	                               FROM   @oldSteps AS os)
	       AND dsta.IsCorrect = 1
	
	SELECT @subjectsCount = COUNT(DISTINCT s.SubjectId)
	FROM   @subjects AS s
	
	IF (@subjectsCount > 1)
	    SELECT TOP(1) *
	    FROM   D_SITE_TEST_ANSWER         AS dsta
	           JOIN @questions            AS q
	                ON  q.QuestionId = dsta.QuestionId
	           JOIN D_SITE_TEST_QUESTION  AS dstq
	                ON  dstq.Id = dsta.QuestionId
	           JOIN D_SITE_TEST_SUBJECT   AS dsts
	                ON  dsts.Id = dsta.SubjectId
	           JOIN D_SITE_TEST           AS dst
	                ON  dst.Id = dstq.TestId
	ELSE
	    SELECT TOP(1) *
	    FROM   D_SITE_TEST_ANSWER         AS dsta
	           JOIN D_SITE_TEST_QUESTION  AS dstq
	                ON  dstq.Id = dsta.QuestionId
	           JOIN D_SITE_TEST_SUBJECT   AS dsts
	                ON  dsts.Id = dsta.SubjectId
	                AND dsts.Id IN (SELECT s.SubjectId
	                                FROM   @subjects AS s)
	           JOIN D_SITE_TEST           AS dst
	                ON  dst.Id = dstq.TestId
END
GO

/*******************************************
 * Тип для предыдущих ответов теста
 *******************************************/
IF OBJECT_ID(N'dbo.get_site_test_next_normal_step', N'P') IS NOT NULL
    DROP PROCEDURE dbo.get_site_test_next_normal_step;
GO

IF OBJECT_ID(N'dbo.get_site_test_normal_results', N'P') IS NOT NULL
    DROP PROCEDURE dbo.get_site_test_normal_results;
GO

IF TYPE_ID(N'dbo.OldSiteTestStepNormal') IS NOT NULL
    DROP TYPE dbo.OldSiteTestStepNormal
GO

CREATE TYPE dbo.OldSiteTestStepNormal AS TABLE 
(QuestionId INT, SubjectId INT);  
GO 

/*******************************************
 * Следующий вопрос теста Normal
 *******************************************/
CREATE PROCEDURE dbo.get_site_test_next_normal_step
	@oldSteps dbo.OldSiteTestStepNormal READONLY,
	@subjectsCount INT OUTPUT,
	@allSubjectsCount INT OUTPUT
AS
BEGIN
	DECLARE @testId INT
	SELECT TOP(1) @testId = dsts.TestId
	FROM   D_SITE_TEST_SUBJECT  AS dsts
	       JOIN @oldSteps       AS os
	            ON  os.SubjectId = dsts.Id
	
	SELECT @allSubjectsCount = COUNT(DISTINCT dsts.Id)
	FROM   D_SITE_TEST_SUBJECT AS dsts
	WHERE  dsts.TestId = @testId
	
	SELECT @subjectsCount = COUNT(DISTINCT dsts.Id)
	FROM   D_SITE_TEST_SUBJECT AS dsts
	WHERE  dsts.Id NOT IN (SELECT os.SubjectId
	                       FROM   @oldSteps AS os)
	       AND dsts.TestId = @testId
	
	IF (@subjectsCount > 0)
	    SELECT TOP 1 *
	    FROM   D_SITE_TEST_ANSWER         AS dsta
	           JOIN D_SITE_TEST_QUESTION  AS dstq
	                ON  dstq.Id = dsta.QuestionId
	           JOIN D_SITE_TEST_SUBJECT   AS dsts
	                ON  dsts.Id = dsta.SubjectId
	           JOIN D_SITE_TEST           AS dst
	                ON  dst.Id = dstq.TestId
	                AND dst.Show = 1
	                AND dst.Id = @testId
	    WHERE  dsts.Id NOT IN (SELECT os.SubjectId
	                           FROM   @oldSteps AS os)
	ELSE
	    SELECT TOP 1 *
	    FROM   D_SITE_TEST_ANSWER         AS dsta
	           JOIN D_SITE_TEST_QUESTION  AS dstq
	                ON  dstq.Id = dsta.QuestionId
	           JOIN D_SITE_TEST_SUBJECT   AS dsts
	                ON  dsts.Id = dsta.SubjectId
	           JOIN D_SITE_TEST           AS dst
	                ON  dst.Id = dstq.TestId
	                AND dst.Show = 1
	                AND dst.Id = @testId
	    ORDER BY
	           NEWID()
END
GO

/*******************************************
 * Вопросы к объекту теста normal
 *******************************************/
IF OBJECT_ID(N'dbo.get_site_test_normal_questions', N'P') IS NOT NULL
    DROP PROCEDURE dbo.get_site_test_normal_questions;
GO
CREATE PROCEDURE dbo.get_site_test_normal_questions
	@testId INT,
	@subjectId INT,
	@amount INT = 3
AS
BEGIN
	DECLARE @trueQuestionId INT
	SELECT @trueQuestionId = dsta.QuestionId
	FROM   D_SITE_TEST_ANSWER AS dsta
	WHERE  dsta.SubjectId = @subjectId
	       AND dsta.IsCorrect = 1
	
	SELECT NEWID(),
	       x.*
	FROM   (
	           SELECT TOP(@amount) dstq.*
	           FROM   D_SITE_TEST_QUESTION AS dstq
	                  JOIN D_SITE_TEST AS dst
	                       ON  dst.Id = dstq.TestId
	                       AND dst.Id = @testId
	                       AND dst.Show = 1
	           WHERE  dstq.Id NOT IN (SELECT TOP 1 dstq.Id
	                                  FROM   D_SITE_TEST_QUESTION AS dstq
	                                  WHERE  dstq.Id = @trueQuestionId)
	           ORDER BY
	                  NEWID()
	           UNION ALL
	           SELECT TOP 1 *
	           FROM   D_SITE_TEST_QUESTION AS dstq
	           WHERE  dstq.Id = @trueQuestionId
	       ) x
	ORDER BY
	       1
END
GO

/*******************************************
 * Результаты теста Normal
 *******************************************/
CREATE PROCEDURE dbo.get_site_test_normal_results
	@subjectId INT
AS
BEGIN
	DECLARE @testId INT
	SELECT TOP 1 @testId = dsts.TestId
	FROM   D_SITE_TEST_SUBJECT AS dsts
	WHERE  dsts.Id = @subjectId
	
	SELECT *
	FROM   D_SITE_TEST_ANSWER             AS dsta
	       JOIN D_SITE_TEST_QUESTION      AS dstq
	            ON  dstq.Id = dsta.QuestionId
	       JOIN D_SITE_TEST_SUBJECT       AS dsts
	            ON  dsts.Id = dsta.SubjectId
	       JOIN D_SITE_TEST               AS dst
	            ON  dst.Id = dstq.TestId
	            AND dst.Id = @testId
	       LEFT JOIN D_SITE_TEST_SETTING  AS dsts2
	            ON  dsts2.TestId = dst.Id
	WHERE  dsta.IsCorrect = 1
END
GO

/*******************************************
 * Превью материалов
 *******************************************/
IF OBJECT_ID(N'dbo.get_preview_materials', N'P') IS NOT NULL
    DROP PROCEDURE dbo.get_preview_materials;
GO
CREATE PROCEDURE dbo.get_preview_materials
	@lettersCount INT,
	@gameTitle VARCHAR(100),
	@categoryId VARCHAR(100)
AS
BEGIN
	SELECT TOP(8) da.Id,
	       dm.Title,
	       dm.TitleUrl,
	       dm.DateCreate,
	       dm.ModelCoreType,
	       dm.DateOfPublication,
	       dm.ViewsCount,
	       dm.FrontPictureId,
	       dbo.get_comments_count(dm.Id, dm.ModelCoreType) AS CommentsCount,
	       CASE 
	            WHEN dm.Foreword IS NOT NULL THEN dm.Foreword
	            ELSE SUBSTRING(dbo.func_strip_html(dm.Html), 0, @lettersCount) +
	                 '...'
	       END               AS Foreword,
	       anu.NikName       AS UserName,
	       dg.Title          AS GameTitle
	FROM   D_ARTICLE         AS da
	       JOIN DV_MATERIAL  AS dm
	            ON  dm.Id = da.Id
	            AND dm.ModelCoreType = da.ModelCoreType
	            AND (dm.Show = 1 AND dm.DateOfPublication <= GETDATE())
	       LEFT JOIN D_USER  AS anu
	            ON  anu.Id = dm.UserId
	       LEFT JOIN D_GAME  AS dg
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
 * Обогатить материалы играми
 *******************************************/
IF OBJECT_ID(N'dbo.get_material_games', N'P') IS NOT NULL
    DROP PROCEDURE dbo.get_material_games;
GO
CREATE PROCEDURE dbo.get_material_games
	@mct INT,
	@ids NVARCHAR(MAX)
AS
BEGIN
	SELECT dm.Id,
	       dm.ModelCoreType,
	       dg.*
	FROM   DV_MATERIAL          AS dm
	       LEFT JOIN D_ARTICLE  AS da
	            ON  da.ModelCoreType = dm.ModelCoreType
	            AND da.Id = dm.Id
	       LEFT JOIN D_NEWS     AS dn
	            ON  dn.ModelCoreType = dm.ModelCoreType
	            AND dn.Id = dm.Id
	       JOIN D_GAME          AS dg
	            ON  dg.Id = da.GameId OR dg.Id = dn.GameId
	WHERE  dm.ModelCoreType = @mct
	       AND dm.Id IN (SELECT fsi.[Value]
	                     FROM   dbo.func_split_int(@ids) AS fsi)
END
GO

/*******************************************
 * Обогатить афоризмы авторами
 *******************************************/
IF OBJECT_ID(N'dbo.get_aphorisms_authors', N'P') IS NOT NULL
    DROP PROCEDURE dbo.get_aphorisms_authors;
GO
CREATE PROCEDURE dbo.get_aphorisms_authors
	@ids NVARCHAR(MAX)
AS
BEGIN
	SELECT dm.Id,
	       daa.*
	FROM   DV_MATERIAL             AS dm
	       JOIN D_APHORISM         AS da
	            ON  da.Id = dm.Id
	            AND da.ModelCoreType = dm.ModelCoreType
	       JOIN D_AUTHOR_APHORISM  AS daa
	            ON  daa.Id = da.AuthorId
	WHERE  dm.Id IN (SELECT fsi.[Value]
	                 FROM   dbo.func_split_int(@ids) AS fsi)
END
GO

/*******************************************
* Получить случайный афоризм
*******************************************/
IF OBJECT_ID(N'dbo.get_random_aphorism', N'P') IS NOT NULL
    DROP PROCEDURE dbo.get_random_aphorism;
GO
CREATE PROCEDURE dbo.get_random_aphorism
	@id INT
AS
BEGIN
	SELECT TOP(1) dm.*,
	       da.*,
	       daa.*
	FROM   D_APHORISM              AS da
	       JOIN DV_MATERIAL        AS dm
	            ON  dm.Id = da.Id
	            AND dm.ModelCoreType = da.ModelCoreType
	            AND dm.Show = 1
	       JOIN D_AUTHOR_APHORISM  AS daa
	            ON  daa.Id = da.AuthorId
	            AND daa.PictureId IS NOT NULL
	WHERE  @id IS NULL
	       OR  da.Id NOT IN (@id)
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
CREATE PROCEDURE dbo.get_aphorism_categories
	@cur NVARCHAR(100)
AS
BEGIN
	WITH c(Id, Title, [Ref]) AS (
	         SELECT dmc.Id               AS Id,
	                dmc.Title,
	                NULL                 AS [Ref]
	         FROM   D_MATERIAL_CATEGORY  AS dmc
	                JOIN DV_MATERIAL     AS dm
	                     ON  dm.CategoryId = dmc.Id
	                JOIN D_APHORISM      AS da
	                     ON  da.ModelCoreType = dm.ModelCoreType
	                     AND da.Id = dm.Id
	         GROUP BY
	                dmc.Id,
	                dmc.Title
	         UNION ALL
	         SELECT daa.TitleUrl       AS Id,
	                daa.Name,
	                CategoryId         AS [Ref]
	         FROM   D_AUTHOR_APHORISM  AS daa
	                JOIN D_APHORISM    AS da
	                     ON  da.AuthorId = daa.Id
	                JOIN DV_MATERIAL   AS dm
	                     ON  dm.Id = da.Id
	                     AND dm.ModelCoreType = da.ModelCoreType
	                     AND dm.CategoryId = CategoryId
	         GROUP BY
	                daa.TitleUrl,
	                daa.Name,
	                CategoryId
	     )
	
	SELECT *
	FROM   c AS c
END
GO

/*******************************************
* получиь тест
*******************************************/
IF OBJECT_ID(N'dbo.get_site_test', N'P') IS NOT NULL
    DROP PROCEDURE dbo.get_site_test;
GO
CREATE PROCEDURE dbo.get_site_test
	@testId INT
AS
BEGIN
	SELECT TOP(2) *
	FROM   D_SITE_TEST AS dst
	WHERE  dst.Id = @testId
END
GO

/*******************************************
* получиь настройку теста
*******************************************/
IF OBJECT_ID(N'dbo.get_site_test_setting', N'P') IS NOT NULL
    DROP PROCEDURE dbo.get_site_test_setting;
GO
CREATE PROCEDURE dbo.get_site_test_setting
	@testId INT
AS
BEGIN
	SELECT TOP(2) *
	FROM   D_SITE_TEST_SETTING AS dsts
	WHERE  dsts.TestId = @testId
END
GO

/*******************************************
* сохранить настройку теста
*******************************************/
IF OBJECT_ID(N'dbo.save_site_test_setting', N'P') IS NOT NULL
    DROP PROCEDURE dbo.save_site_test_setting;
GO
CREATE PROCEDURE dbo.save_site_test_setting
	@testId INT,
	@lc INT,
	@oetbc INT,
	@bfos INT,
	@dqs INT,
	@dsab INT
AS
BEGIN
	DECLARE @date DATETIME = GETDATE();
	
	IF NOT EXISTS (
	       SELECT TOP(1) dsts.TestId
	       FROM   D_SITE_TEST_SETTING AS dsts
	       WHERE  dsts.TestId = @testId
	   )
	BEGIN
	    INSERT INTO D_SITE_TEST_SETTING
	      (
	        TestId,
	        DateCreate,
	        DateUpdate,
	        LettersInSecond,
	        OnEndTimeBalsCount,
	        BalsForOneSecond,
	        DefQuestionSeconds,
	        DefCorrectAnswerBals
	      )
	    VALUES
	      (
	        @testId,
	        @date,
	        @date,
	        @lc,
	        @oetbc,
	        @bfos,
	        @dqs,
	        @dsab
	      )
	END
	ELSE
	BEGIN
	    UPDATE D_SITE_TEST_SETTING
	    SET    DateUpdate               = @date,
	           LettersInSecond          = @lc,
	           OnEndTimeBalsCount       = @oetbc,
	           BalsForOneSecond         = @bfos,
	           DefQuestionSeconds       = @dqs,
	           DefCorrectAnswerBals     = @dsab
	    WHERE  TestId                   = @testId
	END
	
	EXEC dbo.get_site_test_setting @testId
END
GO

/*******************************************
 * Матрица ответов для тестов
 *******************************************/
IF OBJECT_ID(N'dbo.get_site_test_matrix', N'P') IS NOT NULL
    DROP PROCEDURE dbo.get_site_test_matrix;
GO
CREATE PROCEDURE dbo.get_site_test_matrix
	@testId INT,
	@page INT = 1,
	@pageSize INT = 10,
	@count INT OUTPUT
AS
BEGIN
	DECLARE @x         NVARCHAR(MAX) = '',
	        @title     NVARCHAR(400)
	
	SET @page = (@page -1) * @pageSize
	
	DECLARE c CURSOR  
	FOR
	    SELECT dsts.Title
	    FROM   D_SITE_TEST_SUBJECT AS dsts
	    WHERE  dsts.TestId = @testId
	    GROUP BY
	           dsts.Title
	
	OPEN c
	FETCH NEXT FROM c INTO @title
	WHILE @@FETCH_STATUS = 0
	BEGIN
	    SET @x = @x + ',[' + @title + ']'
	    FETCH NEXT FROM c INTO @title
	END
	CLOSE c
	DEALLOCATE c
	
	IF (@x <> '')
	BEGIN
	    SET @x = RIGHT(@x, LEN(@x) -1)
	    
	    EXEC (
	             'SELECT *
FROM   (
           SELECT dsts.Title,
                  dstq.[Text],
                  dsta.IsCorrect
           FROM   D_SITE_TEST_SUBJECT AS dsts
                  JOIN D_SITE_TEST_QUESTION AS dstq
                       ON  dstq.TestId = ' + @testId +
	             '
                  LEFT JOIN D_SITE_TEST_ANSWER AS dsta
                       ON  dsta.SubjectId = dsts.Id
                       AND dsta.QuestionId = dstq.Id
           WHERE  dsts.TestId = ' + @testId +
	             '
       ) t
       PIVOT(
           SUM(IsCorrect) FOR t.Title IN (' + @x +
	             ')
       ) p ORDER BY p.[Text] OFFSET ' + @page + ' ROWS FETCH NEXT ' + @pageSize 
	             +
	             ' ROWS ONLY'
	         )
	END
	
	SELECT @count = COUNT(1)
	FROM   D_SITE_TEST_QUESTION AS dstq
	WHERE  dstq.TestId = @testId
	
	RETURN
END
GO

/*******************************************
 * Реверт значения матрицы теста 
 *******************************************/
IF OBJECT_ID(N'dbo.revert_site_test_matrix_value', N'P') IS NOT NULL
    DROP PROCEDURE dbo.revert_site_test_matrix_value;
GO
CREATE PROCEDURE dbo.revert_site_test_matrix_value
	@subjectTitle NVARCHAR(400),
	@questionText NVARCHAR(500),
	@value INT
AS
BEGIN
	UPDATE D_SITE_TEST_ANSWER
	SET    IsCorrect = @value
	WHERE  Id IN (SELECT dsta.Id
	              FROM   D_SITE_TEST_ANSWER AS dsta
	                     JOIN D_SITE_TEST_QUESTION AS dstq
	                          ON  dstq.Id = dsta.QuestionId
	                          AND dstq.[Text] = @questionText
	                     JOIN D_SITE_TEST_SUBJECT AS dsts
	                          ON  dsts.Id = dsta.SubjectId
	                          AND dsts.Title = @subjectTitle)
END
GO

/*******************************************
 * Добавить просмотр теста
 *******************************************/
IF OBJECT_ID(N'dbo.add_site_test_show', N'P') IS NOT NULL
    DROP PROCEDURE dbo.add_site_test_show;
GO
CREATE PROCEDURE dbo.add_site_test_show
	@testId INT
AS
BEGIN
	DECLARE @oldViewsCount INT
	SELECT TOP 1 @oldViewsCount = dst.ViewsCount
	FROM   D_SITE_TEST AS dst
	WHERE  dst.Id = @testId
	
	UPDATE D_SITE_TEST
	SET    ViewsCount = @oldViewsCount + 1
	WHERE  Id = @testId
	
	SELECT @oldViewsCount + 1
END
GO

/*******************************************
 * Блок случайных тестов
 *******************************************/
IF OBJECT_ID(N'dbo.get_random_site_tests', N'P') IS NOT NULL
    DROP PROCEDURE dbo.get_random_site_tests;
GO
CREATE PROCEDURE dbo.get_random_site_tests
	@amount INT
AS
BEGIN
	SELECT TOP(@amount)
	       dst.Title,
	       dst.[Description],
	       dst.TitleUrl,
	       dst.Rules
	FROM   D_SITE_TEST AS dst
	WHERE  dst.Show = 1
	       AND dst.ViewOnMainPage = 1
	ORDER BY
	       NEWID()
END
GO

/*******************************************
 * добавить популярные видео с youtube
 *******************************************/
IF OBJECT_ID(N'dbo.save_popular_youtube_videos', N'P') IS NOT NULL
    DROP PROCEDURE dbo.save_popular_youtube_videos;
GO
 
IF TYPE_ID(N'dbo.PopularYoutubeVideos') IS NOT NULL
    DROP TYPE dbo.PopularYoutubeVideos
GO

CREATE TYPE dbo.PopularYoutubeVideos AS TABLE(
                                                 Id NVARCHAR(128),
                                                 Title NVARCHAR(400),
                                                 ChannelId NVARCHAR(100),
                                                 ChannelTitle NVARCHAR(400)
                                             )
GO 

CREATE PROCEDURE dbo.save_popular_youtube_videos
	@videos PopularYoutubeVideos READONLY
AS
BEGIN
	DECLARE @date DATETIME = GETDATE();
	
	BEGIN TRANSACTION
	INSERT INTO D_POPULAR_YOUTUBE_VIDEO
	  (
	    Id,
	    Title,
	    ChannelId,
	    ChannelTitle,
	    Rating,
	    DateUpdate,
	    DateCreate
	  )
	SELECT v.Id,
	       v.Title,
	       v.ChannelId,
	       v.ChannelTitle,
	       0,
	       @date,
	       @date
	FROM   @videos AS v
	WHERE  v.Id NOT IN (SELECT dpv.Id
	                    FROM   D_POPULAR_YOUTUBE_VIDEO AS dpv)
	
	UPDATE D_POPULAR_YOUTUBE_VIDEO
	SET    Rating = dpv.Rating + 1,
	       DateUpdate = @date
	FROM   D_POPULAR_YOUTUBE_VIDEO AS dpv
	WHERE  dpv.Id IN (SELECT v.Id
	                  FROM   @videos AS v)
	
	COMMIT TRANSACTION
END
GO

/*******************************************
 * архив популярных видео youtube
 *******************************************/
IF OBJECT_ID(N'dbo.get_popular_youtube_videos_archive', N'P') IS NOT NULL
    DROP PROCEDURE dbo.get_popular_youtube_videos_archive;
GO
CREATE PROCEDURE dbo.get_popular_youtube_videos_archive
AS
BEGIN
	SELECT YEAR(x.[Date])   AS [Year],
	       MONTH(x.[Date])  AS [Month],
	       DAY(x.[Date])    AS [Day]
	FROM   (
	           SELECT CONVERT(DATE, dpv.DateUpdate) AS [Date]
	           FROM   D_POPULAR_YOUTUBE_VIDEO AS dpv
	           GROUP BY
	                  CONVERT(DATE, dpv.DateUpdate)
	       )                AS x
	ORDER BY
	       1,
	       2,
	       3
END
GO

/*******************************************
 * список архивных видео с youtube за период
 *******************************************/
IF OBJECT_ID(N'dbo.get_popular_youtube_video_archive_list', N'P') IS NOT NULL
    DROP PROCEDURE dbo.get_popular_youtube_video_archive_list;
GO
CREATE PROCEDURE dbo.get_popular_youtube_video_archive_list
	@year INT,
	@month INT,
	@day INT,
	@amount INT
AS
BEGIN
	IF (@year IS NULL AND @month IS NULL AND @day IS NULL)
	    SELECT TOP(@amount) *
	    FROM   D_POPULAR_YOUTUBE_VIDEO AS dpv
	    ORDER BY
	           dpv.Rating
	ELSE 
	IF (@year IS NOT NULL AND @month IS NULL AND @day IS NULL)
	    SELECT TOP(@amount) *
	    FROM   D_POPULAR_YOUTUBE_VIDEO AS dpv
	    WHERE  YEAR(dpv.DateCreate) = @year
	           OR  YEAR(dpv.DateUpdate) = @year
	    ORDER BY
	           dpv.Rating
	ELSE 
	IF (@year IS NOT NULL AND @month IS NOT NULL AND @day IS NULL)
	    SELECT TOP(@amount) *
	    FROM   D_POPULAR_YOUTUBE_VIDEO AS dpv
	    WHERE  (
	               YEAR(dpv.DateCreate) = @year
	               AND MONTH(dpv.DateCreate) = @month
	           )
	           OR  (
	                   YEAR(dpv.DateUpdate) = @year
	                   AND MONTH(dpv.DateUpdate) = @month
	               )
	    ORDER BY
	           dpv.Rating
	ELSE 
	IF (
	       @year IS NOT NULL
	       AND @month IS NOT NULL
	       AND @day IS NOT NULL
	   )
	    SELECT TOP(@amount) *
	    FROM   D_POPULAR_YOUTUBE_VIDEO AS dpv
	    WHERE  (
	               YEAR(dpv.DateCreate) = @year
	               AND MONTH(dpv.DateCreate) = @month
	               AND DAY(dpv.DateCreate) = @day
	           )
	           OR  (
	                   YEAR(dpv.DateUpdate) = @year
	                   AND MONTH(dpv.DateUpdate) = @month
	                   AND DAY(dpv.DateUpdate) = @day
	               )
	    ORDER BY
	           dpv.Rating
END
GO

/*******************************************
 * Получить избранные категории материалов
 *******************************************/
IF OBJECT_ID(N'dbo.get_feautured_material_categories', N'P') IS NOT NULL
    DROP PROCEDURE dbo.get_feautured_material_categories;
GO
CREATE PROCEDURE dbo.get_feautured_material_categories
	@amount INT
AS
BEGIN
	SELECT TOP(@amount) dmc.*,
	       dp.Id,
	       dp.Caption
	FROM   D_MATERIAL_CATEGORY  AS dmc
	       JOIN D_PICTURE       AS dp
	            ON  dp.Id = dmc.FrontPictureId
	WHERE  dmc.IsFeatured = 1
END
GO

/*******************************************
 * Получить материалы избранной категории материалов
 *******************************************/
IF OBJECT_ID(N'dbo.get_feautured_material_categories_materials', N'P') IS NOT 
   NULL
    DROP PROCEDURE dbo.get_feautured_material_categories_materials;
GO
CREATE PROCEDURE dbo.get_feautured_material_categories_materials
	@categoryId NVARCHAR(255),
	@amount INT
AS
BEGIN
	DECLARE @date DATETIME = GETDATE();
	
	SELECT TOP(@amount)
	       dm.Title,
	       dm.TitleUrl,
	       dm.ModelCoreType,
	       dm.DateOfPublication,
	       dm.DateCreate
	FROM   DV_MATERIAL AS dm
	WHERE  dm.CategoryId = @categoryId
	       AND dm.Show = 1
	       AND dm.DateOfPublication <= @date
	ORDER BY
	       dm.DateOfPublication DESC
END
GO

/*******************************************
 * Получить автора авфоризма по TitleUrl
 *******************************************/
IF OBJECT_ID(N'dbo.get_author_aphorisms_by_title_url', N'P') IS NOT NULL
    DROP PROCEDURE dbo.get_author_aphorisms_by_title_url;
GO
CREATE PROCEDURE dbo.get_author_aphorisms_by_title_url
	@titleUrl NVARCHAR(255)
AS
BEGIN
	SELECT TOP(2) *
	FROM   D_AUTHOR_APHORISM    AS daa
	       LEFT JOIN D_PICTURE  AS dp
	            ON  dp.Id = daa.PictureId
	WHERE  daa.TitleUrl = @titleUrl
END
GO

/*******************************************
* Получить страницу афоризма
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
	       x.Foreword,
	       x.CategoryId,
	       x.ModelCoreType,
	       dmc.Id,
	       dmc.Title,
	       x.AuthorId                   AS Id,
	       daa.Name,
	       daa.PictureId,
	       daa.TitleUrl,
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
	       x.Foreword,
	       x.CategoryId,
	       x.ModelCoreType,
	       x.AuthorId,
	       x.Flag,
	       dmc.Title,
	       daa.Name,
	       daa.PictureId,
	       daa.TitleUrl,
	       dmc.Id
END
GO

/*******************************************
 * Получить инфографик
 *******************************************/
IF OBJECT_ID(N'dbo.get_infographic', N'P') IS NOT NULL
    DROP PROCEDURE dbo.get_infographic;
GO
CREATE PROCEDURE dbo.get_infographic
	@pid UNIQUEIDENTIFIER
AS
BEGIN
	SELECT TOP(2)
	       di.*,
	       dp.Id,
	       dp.Caption
	FROM   D_PICTURE           AS dp
	       JOIN D_INFOGRAPHIC  AS di
	            ON  di.PictureId = dp.Id
	WHERE  di.PictureId = @pid
END
GO

/*******************************************
 * Добавить список инфографиков
 *******************************************/
IF OBJECT_ID(N'dbo.add_infographics', N'P') IS NOT NULL
    DROP PROCEDURE dbo.add_infographics;
GO
CREATE PROCEDURE dbo.add_infographics
	@mid INT,
	@mct INT,
	@ids NVARCHAR(MAX)
AS
BEGIN
	INSERT INTO D_INFOGRAPHIC
	  (
	    PictureId,
	    MaterialId,
	    ModelCoreType
	  )
	SELECT CAST(fss.[Value] AS UNIQUEIDENTIFIER),
	       @mid,
	       @mct
	FROM   dbo.func_split_string(@ids) AS fss
END
GO

/*******************************************
 * Удалить инфографик
 *******************************************/
IF OBJECT_ID(N'dbo.del_infographic', N'P') IS NOT NULL
    DROP PROCEDURE dbo.del_infographic;
GO
CREATE PROCEDURE dbo.del_infographic
	@pid UNIQUEIDENTIFIER,
	@mid INT,
	@mct INT
AS
BEGIN
	DELETE 
	FROM   D_INFOGRAPHIC
	WHERE  PictureId = @pid
	       AND MaterialId = @mid
	       AND ModelCoreType = @mct
END
GO

/*******************************************
 * Все инфографики материала
 *******************************************/
IF OBJECT_ID(N'dbo.get_material_infographics', N'P') IS NOT NULL
    DROP PROCEDURE dbo.get_material_infographics;
GO
CREATE PROCEDURE dbo.get_material_infographics
	@mid INT,
	@mct INT
AS
BEGIN
	SELECT di.*,
	       dp.Id,
	       dp.Caption
	FROM   D_INFOGRAPHIC   AS di
	       JOIN D_PICTURE  AS dp
	            ON  dp.Id = di.PictureId
	WHERE  di.MaterialId = @mid
	       AND di.ModelCoreType = @mct
END
GO

/*******************************************
 * Блок "Геймеру почитать"
 *******************************************/
IF OBJECT_ID(N'dbo.get_games_for_gamers_block', N'P') IS NOT NULL
    DROP PROCEDURE dbo.get_games_for_gamers_block;
GO
CREATE PROCEDURE dbo.get_games_for_gamers_block
AS
BEGIN
	SELECT da.GameId,
	       dg.FrontPictureId,
	       dg.Title,
	       dg.TitleUrl,
	       dg.[Description],
	       dm.CategoryId,
	       dmc.Title                 AS CategoryTitle,
	       dbo.get_comments_count(dm.Id, dm.ModelCoreType) AS CommentsCount
	FROM   DV_MATERIAL               AS dm
	       JOIN D_ARTICLE            AS da
	            ON  da.Id = dm.Id
	            AND da.ModelCoreType = dm.ModelCoreType
	       JOIN D_GAME               AS dg
	            ON  dg.Id = da.GameId
	            AND dg.Show = 1
	            AND dg.FrontPictureId IS NOT NULL
	       JOIN D_MATERIAL_CATEGORY  AS dmc
	            ON  dmc.Id = dm.CategoryId
	WHERE  dm.Show = 1
	       AND dm.DateOfPublication <= GETDATE()
	GROUP BY
	       dm.Id,
	       dm.ModelCoreType,
	       da.GameId,
	       dg.FrontPictureId,
	       dg.Title,
	       dg.TitleUrl,
	       dg.[Description],
	       dm.CategoryId,
	       dmc.Title
END
GO

/*******************************************
 * добавить игру со Steam API
 *******************************************/
IF OBJECT_ID(N'dbo.add_steam_app', N'P') IS NOT NULL
    DROP PROCEDURE dbo.add_steam_app;
GO
CREATE PROCEDURE dbo.add_steam_app
	@appId INT,
	@appName NVARCHAR(400)
AS
BEGIN
	DECLARE @date DATETIME = GETDATE();
	
	MERGE D_STEAM_APP 
	WITH (HOLDLOCK) AS t
	     USING (
	         SELECT @appId    AS Id,
	                @appName  AS [Name]
	     ) AS s ON t.Id = s.Id
	     WHEN MATCHED THEN
	
	UPDATE 
	SET    t.[Name] = @appName,
	       t.DateUpdate = @date
	       
	       WHEN NOT MATCHED BY TARGET THEN
	
	INSERT 
	  (
	    Id,
	    [Name],
	    DateCreate,
	    DateUpdate
	  )
	VALUES
	  (
	    @appId,
	    @appName,
	    @date,
	    @date
	  );
END
GO

/*******************************************
 * прикрепить steamApp к игре
 *******************************************/
IF OBJECT_ID(N'dbo.add_steam_app_to_game', N'P') IS NOT NULL
    DROP PROCEDURE dbo.add_steam_app_to_game;
GO
CREATE PROCEDURE dbo.add_steam_app_to_game
	@gameId INT,
	@steamAppId INT
AS
	BEGIN TRANSACTION
	
	UPDATE D_GAME
	SET    SteamAppId = NULL
	WHERE  SteamAppId = @steamAppId
	
	UPDATE D_GAME
	SET    SteamAppId = @steamAppId
	WHERE  Id = @gameId
	
	COMMIT TRANSACTION
GO

/*******************************************
 * открепить steamApp от игры
 *******************************************/
IF OBJECT_ID(N'dbo.del_steam_app_from_game', N'P') IS NOT NULL
    DROP PROCEDURE dbo.del_steam_app_from_game;
GO
CREATE PROCEDURE dbo.del_steam_app_from_game
	@gameId INT
AS
	UPDATE D_GAME
	SET    SteamAppId     = NULL
	WHERE  Id             = @gameId
GO