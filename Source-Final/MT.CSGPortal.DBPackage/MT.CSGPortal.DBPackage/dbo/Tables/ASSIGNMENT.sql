﻿CREATE TABLE [dbo].[ASSIGNMENT](
	[ORGANIZATION_GROUP_ID] [int] NOT NULL,
	[MPOWER_ID] [varchar](50) NULL,
	[CUSTOMER_ID] [int] NOT NULL,
	[DESCRIPTION] [varchar](4000) NULL,
	[TEAM_SIZE] [int] NULL,
	[WORK_UNIT_ID] [int] NOT NULL,
	[ASSIGNMENT_TYPE_ID] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[WORK_UNIT_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ASSIGNMENT]  WITH CHECK ADD  CONSTRAINT [ASSGMNT_ASSGMNT_TYP_FK] FOREIGN KEY([ASSIGNMENT_TYPE_ID])
REFERENCES [dbo].[ASSIGNMENT_TYPE] ([ASSIGNMENT_TYPE_ID])
GO

ALTER TABLE [dbo].[ASSIGNMENT] CHECK CONSTRAINT [ASSGMNT_ASSGMNT_TYP_FK]
GO
ALTER TABLE [dbo].[ASSIGNMENT]  WITH CHECK ADD  CONSTRAINT [ASSGNMNT_CSTMR_FK] FOREIGN KEY([CUSTOMER_ID])
REFERENCES [dbo].[CUSTOMER] ([CUSTOMER_ID])
GO

ALTER TABLE [dbo].[ASSIGNMENT] CHECK CONSTRAINT [ASSGNMNT_CSTMR_FK]
GO
ALTER TABLE [dbo].[ASSIGNMENT]  WITH CHECK ADD  CONSTRAINT [ORG_GRP_PROFILE_FK] FOREIGN KEY([ORGANIZATION_GROUP_ID])
REFERENCES [dbo].[ORGANIZATION_GROUP] ([ORGANIZATION_GROUP_ID])
GO

ALTER TABLE [dbo].[ASSIGNMENT] CHECK CONSTRAINT [ORG_GRP_PROFILE_FK]
GO
ALTER TABLE [dbo].[ASSIGNMENT]  WITH CHECK ADD  CONSTRAINT [WRK_UNT_ASSGNMNT_FK] FOREIGN KEY([WORK_UNIT_ID])
REFERENCES [dbo].[WORK_UNIT] ([WORK_UNIT_ID])
GO

ALTER TABLE [dbo].[ASSIGNMENT] CHECK CONSTRAINT [WRK_UNT_ASSGNMNT_FK]