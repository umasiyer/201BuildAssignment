/****** Object:  UserDefinedTableType [dbo].[UDTT_MindContact]    Script Date: 3/20/2014 3:16:49 PM ******/
CREATE TYPE [dbo].[UDTT_MindContact] AS TABLE(
	[MID] [varchar](12) NULL,
	[ContactTypeId] [int] NULL,
	[ContactText] [varchar](50) NULL
)