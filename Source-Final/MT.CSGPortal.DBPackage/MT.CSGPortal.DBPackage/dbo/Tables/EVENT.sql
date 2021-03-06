﻿CREATE TABLE [dbo].[EVENT](
	[EVENT_TYPE_ID] [int] NOT NULL,
	[WORK_UNIT_ID] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[WORK_UNIT_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[EVENT]  WITH CHECK ADD  CONSTRAINT [EVENT_EVENT_TYP_FK] FOREIGN KEY([EVENT_TYPE_ID])
REFERENCES [dbo].[EVENT_TYPE] ([EVENT_TYPE_ID])
GO

ALTER TABLE [dbo].[EVENT] CHECK CONSTRAINT [EVENT_EVENT_TYP_FK]
GO
ALTER TABLE [dbo].[EVENT]  WITH CHECK ADD  CONSTRAINT [WRK_UNT_EVENT_FK] FOREIGN KEY([WORK_UNIT_ID])
REFERENCES [dbo].[WORK_UNIT] ([WORK_UNIT_ID])
GO

ALTER TABLE [dbo].[EVENT] CHECK CONSTRAINT [WRK_UNT_EVENT_FK]