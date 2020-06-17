USE [OpenLDRRefs]
GO
/****** Object:  Table [dbo].[Susceptability]    Script Date: 17/06/2020 09:51:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Susceptability](
	[Organism] [nvarchar](100) NOT NULL,
	[Drug] [nvarchar](100) NOT NULL,
	[Class] [nvarchar](100) NOT NULL,
	[Date] [int] NOT NULL,
	[Month] [int] NOT NULL,
	[Year] [int] NOT NULL,
	[TotalCount] [int] NOT NULL,
	[ResultCount] [int] NOT NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Transmission]    Script Date: 17/06/2020 09:51:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Transmission](
	[Test] [nvarchar](50) NOT NULL,
	[System] [nvarchar](250) NOT NULL,
	[TestingLab] [nvarchar](250) NOT NULL,
	[Date] [int] NOT NULL,
	[Month] [int] NOT NULL,
	[Year] [int] NOT NULL,
	[Received] [int] NOT NULL,
	[Registered] [int] NOT NULL,
	[Tested] [int] NOT NULL,
	[Authorised] [int] NOT NULL,
	[Rejected] [int] NOT NULL,
	[Tested_Workload] [int] NOT NULL,
	[Authorised_Workload] [int] NOT NULL
) ON [PRIMARY]

GO
