USE [CodeTest]
GO
/****** Object:  Table [dbo].[Account]    Script Date: 9/21/2021 12:07:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Account](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AccountName] [varchar](250) NOT NULL,
	[StatusId] [int] NOT NULL,
	[CreatedBy] [varchar](250) NOT NULL,
	[CreatedOn] [datetimeoffset](7) NOT NULL,
	[ModifiedBy] [varchar](250) NOT NULL,
	[ModifiedOn] [datetimeoffset](7) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[Balance] [decimal](10, 2) NOT NULL,
	[OpeningBalance] [decimal](10, 2) NOT NULL,
 CONSTRAINT [PK_AccountName] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Payment]    Script Date: 9/21/2021 12:07:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Payment](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AccountId] [int] NOT NULL,
	[PaymentDate] [datetimeoffset](7) NOT NULL,
	[Amount] [decimal](10, 2) NOT NULL,
	[StatusId] [int] NOT NULL,
	[CreatedBy] [varchar](250) NOT NULL,
	[CreatedOn] [datetimeoffset](7) NOT NULL,
	[ModifiedBy] [varchar](250) NOT NULL,
	[ModifiedOn] [datetimeoffset](7) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Payment] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Status]    Script Date: 9/21/2021 12:07:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Status](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](250) NOT NULL,
	[CreatedBy] [varchar](250) NOT NULL,
	[CreatedOn] [datetimeoffset](7) NOT NULL,
	[ModifiedBy] [varchar](250) NOT NULL,
	[ModifiedOn] [datetimeoffset](7) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Status] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Account] ON 

INSERT [dbo].[Account] ([Id], [AccountName], [StatusId], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn], [IsDeleted], [Balance], [OpeningBalance]) VALUES (1, N'Paul Account', 1, N'Paul', CAST(N'2021-09-20T17:49:41.9600000+00:00' AS DateTimeOffset), N'Paul', CAST(N'2021-09-20T17:49:41.9600000+00:00' AS DateTimeOffset), 0, CAST(4250.00 AS Decimal(10, 2)), CAST(5000.00 AS Decimal(10, 2)))
SET IDENTITY_INSERT [dbo].[Account] OFF
GO
SET IDENTITY_INSERT [dbo].[Payment] ON 

INSERT [dbo].[Payment] ([Id], [AccountId], [PaymentDate], [Amount], [StatusId], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn], [IsDeleted]) VALUES (1, 1, CAST(N'2021-09-20T17:50:40.1333333+00:00' AS DateTimeOffset), CAST(500.00 AS Decimal(10, 2)), 1, N'Paul', CAST(N'2021-09-20T17:50:40.1333333+00:00' AS DateTimeOffset), N'Paul', CAST(N'2021-09-20T17:50:40.1333333+00:00' AS DateTimeOffset), 0)
INSERT [dbo].[Payment] ([Id], [AccountId], [PaymentDate], [Amount], [StatusId], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn], [IsDeleted]) VALUES (3, 1, CAST(N'2021-09-21T08:50:40.1333333+00:00' AS DateTimeOffset), CAST(250.00 AS Decimal(10, 2)), 1, N'Paul', CAST(N'2021-09-21T08:50:40.1333333+00:00' AS DateTimeOffset), N'Paul', CAST(N'2021-09-21T08:50:40.1333333+00:00' AS DateTimeOffset), 0)
SET IDENTITY_INSERT [dbo].[Payment] OFF
GO
SET IDENTITY_INSERT [dbo].[Status] ON 

INSERT [dbo].[Status] ([Id], [Name], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn], [IsDeleted]) VALUES (1, N'Open', N'Paul', CAST(N'2021-09-20T17:49:02.5066667+00:00' AS DateTimeOffset), N'Paul', CAST(N'2021-09-20T17:49:02.5066667+00:00' AS DateTimeOffset), 0)
INSERT [dbo].[Status] ([Id], [Name], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn], [IsDeleted]) VALUES (2, N'Closed', N'Paul', CAST(N'2021-09-20T17:49:06.8800000+00:00' AS DateTimeOffset), N'Paul', CAST(N'2021-09-20T17:49:06.8800000+00:00' AS DateTimeOffset), 0)
SET IDENTITY_INSERT [dbo].[Status] OFF
GO
ALTER TABLE [dbo].[Account]  WITH CHECK ADD  CONSTRAINT [FK_Account_Status] FOREIGN KEY([StatusId])
REFERENCES [dbo].[Status] ([Id])
GO
ALTER TABLE [dbo].[Account] CHECK CONSTRAINT [FK_Account_Status]
GO
ALTER TABLE [dbo].[Payment]  WITH CHECK ADD  CONSTRAINT [FK_Payment_Account] FOREIGN KEY([AccountId])
REFERENCES [dbo].[Account] ([Id])
GO
ALTER TABLE [dbo].[Payment] CHECK CONSTRAINT [FK_Payment_Account]
GO
ALTER TABLE [dbo].[Payment]  WITH CHECK ADD  CONSTRAINT [FK_Payment_Status] FOREIGN KEY([StatusId])
REFERENCES [dbo].[Status] ([Id])
GO
ALTER TABLE [dbo].[Payment] CHECK CONSTRAINT [FK_Payment_Status]
GO
