-- Create a new database called 'VoucherDemo'
-- Connect to the 'master' database to run this snippet
USE master
GO
-- Create the new database if it does not exist already
IF NOT EXISTS (
	SELECT name
		FROM sys.databases
		WHERE name = N'VoucherDemo'
)
CREATE DATABASE VoucherDemo
GO

USE [VoucherDemo]
GO

-- 1, fgfh
-- 2, gtkr
-- 3, gfd
-- 
-- 
-- 
USE [VoucherDemo]
GO

/****** Object:  Table [dbo].[Voucher]    Script Date: 1/16/2019 2:54:36 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Voucher](
	[VoucherId] [bigint] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](100) NOT NULL,
	[VoucherType] [nvarchar](50) NOT NULL,
	[CreationDate] [datetime] NOT NULL,
	[ExpiryDate] [datetime] NOT NULL,
	[VoucherStatus] [nvarchar](10) NOT NULL,
	[MerchantId] [nvarchar](100) NOT NULL,
	[Metadata] [nvarchar](100),
	[Description] [nvarchar](100)
 CONSTRAINT [PK__Voucher__3AEE7921E146F7BD] PRIMARY KEY CLUSTERED 
(
	[VoucherId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Voucher] ADD  CONSTRAINT [DF__Voucher__Creatio__4BAC3F29]  DEFAULT (getutcdate()) FOR [CreationDate]
GO

ALTER TABLE [dbo].[Voucher] ADD CONSTRAINT [DF__Voucher__Voucher__4CA06362]  DEFAULT ('ACTIVE') FOR [VoucherStatus]
GO



/****** Object:  Table [dbo].[DiscountVoucher]    Script Date: 1/16/2019 2:46:43 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DiscountVoucher](
	[DiscontVoucherId] [int] IDENTITY(1,1) NOT NULL,
	[DiscountAmount] [nvarchar](50) NULL,
	[DiscountUnit] [nvarchar](50) NULL,
	[DiscountPercentage] [float] NULL,
	[RedemptionCount] [bigint] NULL,
	[VoucherId] [bigint] NOT NULL,
 CONSTRAINT [PK__DiscontV__42C60243A7EEEE97] PRIMARY KEY CLUSTERED 
(
	[DiscontVoucherId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[DiscountVoucher] ADD  CONSTRAINT [DF_DiscountVoucher_RedemptionCount]  DEFAULT ((0)) FOR [RedemptionCount]
GO

ALTER TABLE [dbo].[DiscountVoucher]  WITH CHECK ADD  CONSTRAINT [FK_DiscontVoucher_Voucher] FOREIGN KEY([VoucherId])
REFERENCES [dbo].[Voucher] ([VoucherId])
GO

ALTER TABLE [dbo].[DiscountVoucher] CHECK CONSTRAINT [FK_DiscontVoucher_Voucher]

/****** Object:  Table [dbo].[GiftVoucher]    Script Date: 1/16/2019 2:52:22 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[GiftVoucher](
	[GiftVoucherId] [int] IDENTITY(1,1) NOT NULL,
	[GiftAmount] [bigint] NOT NULL,
	[GiftBalance] [bigint] NOT NULL,
	[VoucherId] [bigint] NOT NULL,
 CONSTRAINT [PK__GiftVouc__FE80A55B6C817359] PRIMARY KEY CLUSTERED 
(
	[GiftVoucherId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[GiftVoucher]  WITH CHECK ADD  CONSTRAINT [FK_GiftVoucher_Voucher] FOREIGN KEY([VoucherId])
REFERENCES [dbo].[Voucher] ([VoucherId])
GO

ALTER TABLE [dbo].[GiftVoucher] CHECK CONSTRAINT [FK_GiftVoucher_Voucher]
GO

-----------------------------------------------------------------------------------------------------------------------------

USE [VoucherDemo]
GO

/****** Object:  Table [dbo].[ValueVoucher]    Script Date: 1/16/2019 2:53:20 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ValueVoucher](
	[ValueVoucherId] [bigint] IDENTITY(1,1) NOT NULL,
	[ValueAmount] [bigint] NOT NULL,
	[VoucherId] [bigint] NOT NULL,
 CONSTRAINT [PK__ValueVou__0720F430EE44B836] PRIMARY KEY CLUSTERED 
(
	[ValueVoucherId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ValueVoucher]  WITH CHECK ADD  CONSTRAINT [FK_ValueVoucher_Voucher] FOREIGN KEY([VoucherId])
REFERENCES [dbo].[Voucher] ([VoucherId])
GO

ALTER TABLE [dbo].[ValueVoucher] CHECK CONSTRAINT [FK_ValueVoucher_Voucher]
GO


-----------------------------------------------------------------------------------------------------------------------------
