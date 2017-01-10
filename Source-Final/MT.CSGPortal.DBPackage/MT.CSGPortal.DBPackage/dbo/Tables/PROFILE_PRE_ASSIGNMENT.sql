﻿CREATE TABLE [dbo].[PROFILE_PRE_ASSIGNMENT](
	[PROFILE_ID] [int] NOT NULL,
	[PRE_ASSIGNMENT_ID] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[PROFILE_ID] ASC,
	[PRE_ASSIGNMENT_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[PROFILE_PRE_ASSIGNMENT]  WITH CHECK ADD  CONSTRAINT [PRFL_PREASSGMNT_PREASSGNNT_FK] FOREIGN KEY([PRE_ASSIGNMENT_ID])
REFERENCES [dbo].[PRE_ASSIGNMENT] ([PRE_ASSIGNMENT_ID])
GO

ALTER TABLE [dbo].[PROFILE_PRE_ASSIGNMENT] CHECK CONSTRAINT [PRFL_PREASSGMNT_PREASSGNNT_FK]
GO
ALTER TABLE [dbo].[PROFILE_PRE_ASSIGNMENT]  WITH CHECK ADD  CONSTRAINT [PRFLE_ASSGNMENT_PRFLE_FK] FOREIGN KEY([PROFILE_ID])
REFERENCES [dbo].[PROFILE] ([PROFILE_ID])
GO

ALTER TABLE [dbo].[PROFILE_PRE_ASSIGNMENT] CHECK CONSTRAINT [PRFLE_ASSGNMENT_PRFLE_FK]