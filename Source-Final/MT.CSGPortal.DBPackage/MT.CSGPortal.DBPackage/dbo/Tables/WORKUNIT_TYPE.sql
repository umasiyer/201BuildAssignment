﻿CREATE TABLE [dbo].[WORKUNIT_TYPE](
	[WORK_UNIT_TYPE] [int] NOT NULL,
	[TYPE_TEXT] [varchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[WORK_UNIT_TYPE] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]