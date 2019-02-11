USE [VoucherDemo]
GO

/****** Object:  View [dbo].[Voucher_DiscountView]    Script Date: 1/16/2019 2:58:13 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW [dbo].[Voucher_DiscountView] 
AS
SELECT a.VoucherId, MerchantId, Code, VoucherType, CreationDate, VoucherStatus, ExpiryDate, DiscountAmount, DiscountPercentage, DiscountUnit, RedemptionCount, Metadata, [Description]
        FROM Voucher as a, DiscountVoucher as b
        WHERE a.VoucherId = b.VoucherId

GO

-----------------------------------------------------------------------------------------------------------------------------

USE [VoucherDemo]
GO

/****** Object:  View [dbo].[Voucher_GiftView]    Script Date: 1/16/2019 2:58:59 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[Voucher_GiftView]
AS
SELECT v.VoucherId, MerchantId, Code, VoucherType, CreationDate, ExpiryDate, VoucherStatus, GiftAmount, GiftBalance, Metadata, [Description]
FROM Voucher v, GiftVoucher g
WHERE v.VoucherId = g.VoucherId

GO

-----------------------------------------------------------------------------------------------------------------------------

USE [VoucherDemo]
GO

/****** Object:  View [dbo].[Voucher_ValueView]    Script Date: 1/16/2019 2:59:36 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[Voucher_ValueView]

AS

SELECT V.VoucherId, MerchantId, Code, VoucherType, CreationDate, ExpiryDate, VoucherStatus, ValueAmount, Metadata, [Description]

FROM Voucher AS V , ValueVoucher AS VV
WHERE V.VoucherId = VV.VoucherId

GO


