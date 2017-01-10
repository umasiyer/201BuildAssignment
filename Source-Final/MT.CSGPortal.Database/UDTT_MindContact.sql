USE [CSGPortal]

GO

IF EXISTS(SELECT * 
           FROM SYS.TYPES
		   WHERE IS_TABLE_TYPE=1 AND NAME='UDTT_MindContact')


DROP TYPE [dbo].[UDTT_MindContact]

CREATE TYPE [dbo].[UDTT_MindContact] AS TABLE(
	[MID] [varchar](12),
	[ContactTypeId] [int],
	[ContactText] [varchar](50)
)

GO