﻿CREATE TABLE [dbo].[TAG_GROUP](
	[TAG_GROUP_ID] [int] IDENTITY(1,1) NOT NULL,
	[NAME] [varchar](50) NOT NULL,
	[PARENT_TAG_GROUP_ID] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[TAG_GROUP_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[TAG_GROUP]  WITH CHECK ADD  CONSTRAINT [TAG_TYPE_PRNT_TAG_FK] FOREIGN KEY([TAG_GROUP_ID])
REFERENCES [dbo].[TAG_GROUP] ([TAG_GROUP_ID])
GO

ALTER TABLE [dbo].[TAG_GROUP] CHECK CONSTRAINT [TAG_TYPE_PRNT_TAG_FK]