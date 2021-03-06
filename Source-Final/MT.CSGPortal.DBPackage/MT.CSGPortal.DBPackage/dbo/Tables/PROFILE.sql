﻿CREATE TABLE [dbo].[PROFILE](
	[PROFILE_ID] [int] IDENTITY(1,1) NOT NULL,
	[MID] [varchar](12) NOT NULL,
	[PROFILE_TYPE_ID] [int] NOT NULL,
	[NAME] [varchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[PROFILE_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[PROFILE]  WITH CHECK ADD  CONSTRAINT [PROFILE_MIND_FK] FOREIGN KEY([MID])
REFERENCES [dbo].[MIND] ([MID])
GO

ALTER TABLE [dbo].[PROFILE] CHECK CONSTRAINT [PROFILE_MIND_FK]
GO
ALTER TABLE [dbo].[PROFILE]  WITH CHECK ADD  CONSTRAINT [PROFILE_PROFILE_TYPE_FK] FOREIGN KEY([PROFILE_TYPE_ID])
REFERENCES [dbo].[PROFILE_TYPE] ([PROFILE_TYPE_ID])
GO

ALTER TABLE [dbo].[PROFILE] CHECK CONSTRAINT [PROFILE_PROFILE_TYPE_FK]