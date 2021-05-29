USE [dxStudy]
GO

/****** Object:  UserDefinedTableType [dbo].[DT_T_PERSON]    Script Date: 2021/5/29 21:21:11 ******/
DROP TYPE [dbo].[DT_T_PERSON]
GO

/****** Object:  UserDefinedTableType [dbo].[DT_T_PERSON]    Script Date: 2021/5/29 21:21:11 ******/
CREATE TYPE [dbo].[DT_T_PERSON] AS TABLE(
	[ID] [varchar](36) NOT NULL,
	[NAME] [nvarchar](50) NOT NULL,
	[AGE] [int] NOT NULL,
	[CREATED_BY] [nvarchar](50) NOT NULL,
	[CREATED_TIME] [datetime] NOT NULL,
	PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (IGNORE_DUP_KEY = OFF)
)
GO


