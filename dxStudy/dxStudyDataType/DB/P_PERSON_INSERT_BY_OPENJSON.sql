USE [dxStudy]
GO

/****** Object:  StoredProcedure [dbo].[P_PERSON_INSERT_BY_OPENJSON]    Script Date: 2021/5/29 21:22:32 ******/
DROP PROCEDURE [dbo].[P_PERSON_INSERT_BY_OPENJSON]
GO

/****** Object:  StoredProcedure [dbo].[P_PERSON_INSERT_BY_OPENJSON]    Script Date: 2021/5/29 21:22:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[P_PERSON_INSERT_BY_OPENJSON]
(
	@PersonJsonData NVARCHAR(MAX)
)
AS
BEGIN
	DECLARE @CURRENT_DATETIME DATETIME = GETDATE()

	INSERT INTO T_PERSON
		(
			ID,
			[NAME],
			AGE,
			CREATED_BY,
			CREATED_TIME
		)
		SELECT ID,
			   [NAME],
			   AGE,
			   CREATED_BY,
			   @CURRENT_DATETIME
		  FROM OPENJSON(@PersonJsonData)
		           WITH(
							ID VARCHAR(36) '$.ID',
							[NAME] NVARCHAR(50) '$.Name',
							AGE INT '$.Age',
							CREATED_BY NVARCHAR(50) '$.CreatedBy'
				       )
END
GO


