USE [VoucherDemo]
GO

/****** Object:  StoredProcedure [dbo].[usp_CreateDiscountVoucher]    Script Date: 1/16/2019 3:01:29 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
 --##--------------User Defined Table Type-------------------------------------------------
 ----------------------Gift ------------------------
DROP TYPE [dbo].GiftVoucherType
GO

CREATE TYPE [dbo].GiftVoucherType AS TABLE (
	[VoucherId] [bigint] PRIMARY KEY,
	[Code] [nvarchar](100) NOT NULL,
	[VoucherType] [nvarchar](50) NOT NULL,
	[CreationDate] [datetime] NOT NULL,
	[ExpiryDate] [datetime] NOT NULL,
	[VoucherStatus] [nvarchar](10) NOT NULL,
	[MerchantId] [nvarchar](100) NOT NULL,
	[Metadata] [nvarchar](100) NULL,
	[Description] [nvarchar](100) NULL,
	[GiftAmount] [bigint] NOT NULL,
	[GiftBalance] [bigint] NOT NULL
)
GO

 ----------------------Value ------------------------
CREATE TYPE [dbo].ValueVoucherType AS TABLE (
	[VoucherId] [bigint] PRIMARY KEY NOT NULL,
	[Code] [nvarchar](100) NOT NULL,
	[VoucherType] [nvarchar](50) NOT NULL,
	[CreationDate] [datetime] NOT NULL,
	[ExpiryDate] [datetime] NOT NULL,
	[VoucherStatus] [nvarchar](10) NOT NULL,
	[MerchantId] [nvarchar](100) NOT NULL,
	[Metadata] [nvarchar](100) NULL,
	[Description] [nvarchar](100) NULL,
	[ValueAmount] [bigint] NOT NULL
)
GO

 ----------------------Discount ------------------------
CREATE TYPE [dbo].DiscountVoucherType AS TABLE (	
	[VoucherId] [bigint] PRIMARY KEY NOT NULL,
	[Code] [nvarchar](100) NOT NULL,
	[VoucherType] [nvarchar](50) NOT NULL,
	[CreationDate] [datetime] NOT NULL,
	[ExpiryDate] [datetime] NOT NULL,
	[VoucherStatus] [nvarchar](10) NOT NULL,
	[MerchantId] [nvarchar](100) NOT NULL,
	[Metadata] [nvarchar](100) NULL,
	[Description] [nvarchar](100) NULL,
	[DiscountAmount] [bigint] NULL,
	[DiscountUnit] [bigint] NULL,
	[DiscountPercentage] [float] NULL
)
GO
--------------------------------------------------------------------
IF EXISTS ( SELECT * FROM INFORMATION_SCHEMA.ROUTINES WHERE SPECIFIC_SCHEMA = N'dbo'
    AND SPECIFIC_NAME = N'usp_CreateDiscountVoucher')
DROP PROCEDURE dbo.usp_CreateGiftVoucher
GO
CREATE PROCEDURE [dbo].[usp_CreateDiscountVoucher]

   @tblDiscount DiscountVoucherType READONLY
AS

		DECLARE @idmap TABLE (TempId BIGINT NOT NULL PRIMARY KEY, 
							VId BIGINT UNIQUE NOT NULL)

   BEGIN TRY
    BEGIN TRANSACTION CreateDiscountVoucher

           MERGE Voucher V 
		   USING (SELECT [VoucherId], [Code], [VoucherType], [MerchantId], [ExpiryDate],
		    [Metadata], [Description] FROM @tblDiscount) TB ON 1 = 0
		   WHEN NOT MATCHED BY TARGET THEN
		   INSERT ([Code], [VoucherType], [MerchantId], [ExpiryDate], [Metadata], [Description])
		   VALUES(TB.Code, TB.VoucherType, TB.MerchantId, TB.ExpiryDate, TB.Metadata, TB.[Description])
		   OUTPUT TB.VoucherId, inserted.VoucherId INTO @idmap(TempId, VId);

           INSERT DiscountVoucher
               ( -- columns to insert data into
               DiscountAmount, DiscountPercentage, DiscountUnit, VoucherId
               )
           SELECT TB.DiscountAmount, TB.DiscountPercentage, TB.DiscountUnit, i.VId
		   FROM @tblDiscount TB
		   JOIN @idmap i ON i.TempId = TB.VoucherId

        COMMIT TRANSACTION CreateDiscountVoucher

    END TRY

    BEGIN CATCH
        ROLLBACK TRANSACTION
    END CATCH


GO
-----------------------------------------------------------------------------------------------------------------------------

SELECT * FROM Voucher
USE [VoucherDemo]
GO

/****** Object:  StoredProcedure [dbo].[usp_CreateGiftVoucher]    Script Date: 1/16/2019 3:02:11 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF EXISTS ( SELECT * FROM INFORMATION_SCHEMA.ROUTINES WHERE SPECIFIC_SCHEMA = N'dbo'
    AND SPECIFIC_NAME = N'usp_CreateGiftVoucher')
DROP PROCEDURE dbo.usp_CreateGiftVoucher
GO

CREATE PROCEDURE [dbo].[usp_CreateGiftVoucher]

@tblGift [dbo].GiftVoucherType READONLY
AS

	DECLARE @idmap TABLE (TempId BIGINT NOT NULL PRIMARY KEY, 
							VId BIGINT UNIQUE NOT NULL)

<<<<<<< HEAD
BEGIN TRY
	BEGIN TRANSACTION CreateGiftVoucher

		MERGE Voucher V 
		USING (SELECT [VoucherId], [Code], [VoucherType], [MerchantId], [ExpiryDate],
			[Metadata], [Description] FROM @tblGift) TB ON 1 = 0
		WHEN NOT MATCHED BY TARGET THEN
		INSERT ([Code], [VoucherType], [MerchantId], [ExpiryDate], [Metadata], [Description])
		VALUES(TB.Code, TB.VoucherType, TB.MerchantId, TB.ExpiryDate, TB.Metadata, TB.[Description])
		OUTPUT TB.VoucherId, inserted.VoucherId INTO @idmap(TempId, VId);

		-- Insert rows into table 'GiftVoucher'
		INSERT GiftVoucher
		(
		GiftAmount, GiftBalance, VoucherId
		)
		SELECT TB.GiftAmount, TB.GiftBalance, i.VId
		FROM @tblGift TB
		JOIN @idmap i ON i.TempId = TB.VoucherId
-- 1, 3
	COMMIT TRANSACTION CreateGiftVoucher
END TRY
BEGIN CATCH
	ROLLBACK
END CATCH
=======
   BEGIN TRY
    BEGIN TRANSACTION CreateGiftVoucher

           MERGE Voucher V 
		   USING (SELECT [VoucherId], [Code], [VoucherType], [MerchantId], [ExpiryDate],
		    [Metadata], [Description] FROM @tblGift) TB ON 1 = 0
		   WHEN NOT MATCHED BY TARGET THEN
		   INSERT ([Code], [VoucherType], [MerchantId], [ExpiryDate], [Metadata], [Description])
		   VALUES(TB.Code, TB.VoucherType, TB.MerchantId, TB.ExpiryDate, TB.Metadata, TB.[Description])
		   OUTPUT TB.VoucherId, inserted.VoucherId INTO @idmap(TempId, VId);

           -- Insert rows into table 'GiftVoucher'
           INSERT GiftVoucher
           (
           GiftAmount, GiftBalance, VoucherId
           )
           SELECT TB.GiftAmount, TB.GiftBalance, i.VId
		   FROM @tblGift TB
		   JOIN @idmap i ON i.TempId = TB.VoucherId

       COMMIT TRANSACTION CreateGiftVoucher
   END TRY
   BEGIN CATCH
       ROLLBACK
   END CATCH
>>>>>>> fffde7f6d0345f24db18956dbce53ca56b9651d6

GO
-----------------------------------------------------------------------------------------------------------------------------

USE [VoucherDemo]
GO

/****** Object:  StoredProcedure [dbo].[usp_CreateValueVoucher]    Script Date: 1/16/2019 3:02:37 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF EXISTS ( SELECT * FROM INFORMATION_SCHEMA.ROUTINES WHERE SPECIFIC_SCHEMA = N'dbo'
    AND SPECIFIC_NAME = N'usp_CreateValueVoucher')
DROP PROCEDURE dbo.usp_CreateGiftVoucher
GO

CREATE PROCEDURE [dbo].[usp_CreateValueVoucher]

   @tblValue ValueVoucherType READONLY
AS

		DECLARE @idmap TABLE (TempId BIGINT NOT NULL PRIMARY KEY, 
							VId BIGINT UNIQUE NOT NULL)

   BEGIN TRY

    BEGIN TRANSACTION CreateValueVoucher

		MERGE Voucher V 
		   USING (SELECT [VoucherId], [Code], [VoucherType], [MerchantId], [ExpiryDate],
		    [Metadata], [Description] FROM @tblValue) TB ON 1 = 0
		   WHEN NOT MATCHED BY TARGET THEN
		   INSERT ([Code], [VoucherType], [MerchantId], [ExpiryDate], [Metadata], [Description])
		   VALUES(TB.Code, TB.VoucherType, TB.MerchantId, TB.ExpiryDate, TB.Metadata, TB.[Description])
		   OUTPUT TB.VoucherId, inserted.VoucherId INTO @idmap(TempId, VId);

		-- Insert rows into table 'ValueVoucher'
		INSERT INTO ValueVoucher
		( -- columns to insert data into
	 	 ValueAmount, VoucherId
		)
		SELECT TB.ValueAmount, i.VId
		FROM @tblValue TB
		JOIN @idmap i ON i.TempId = TB.VoucherId

	COMMIT TRANSACTION
END TRY
BEGIN CATCH	
	ROLLBACK
END CATCH
GO


-----------------------------------------------------------------------------------------------------------------------------

USE [VoucherDemo]
GO

/****** Object:  StoredProcedure [dbo].[usp_DeleteVoucherByCode]    Script Date: 1/16/2019 3:03:09 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[usp_DeleteVoucherByCode]
   @Code nvarchar(100)


AS
   BEGIN TRY
		BEGIN TRANSACTION DeleteVoucherByCode

				UPDATE Voucher
				SET
					[VoucherStatus] = 'DELETED'
					-- add more columns and values here
				WHERE @Code=Code
			 
		COMMIT TRANSACTION DeleteVoucherByCode

    END TRY

    BEGIN CATCH
        ROLLBACK TRANSACTION
    END CATCH


GO

-----------------------------------------------------------------------------------------------------------------------------

USE [VoucherDemo]
GO

/****** Object:  StoredProcedure [dbo].[usp_DeleteVoucherById]    Script Date: 1/16/2019 3:04:04 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[usp_DeleteVoucherById]
   @VoucherId BIGINT


AS
   BEGIN TRY
		BEGIN TRANSACTION DeleteVoucherById

				UPDATE Voucher
				SET
					[VoucherStatus] = 'DELETED'
					-- add more columns and values here
				WHERE @VoucherId=VoucherId
			 
		COMMIT TRANSACTION DeleteVoucherById

    END TRY

    BEGIN CATCH
        ROLLBACK TRANSACTION
    END CATCH


GO

-----------------------------------------------------------------------------------------------------------------------------

USE [VoucherDemo]
GO

/****** Object:  StoredProcedure [dbo].[usp_GetAllDiscountVouchers]    Script Date: 1/16/2019 3:04:42 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[usp_GetAllDiscountVouchers] 
AS
SELECT VoucherId, MerchantId, Code, VoucherType, CreationDate, VoucherStatus, ExpiryDate, DiscountAmount, DiscountPercentage, DiscountUnit, RedemptionCount
FROM Voucher_DiscountView

GO

-----------------------------------------------------------------------------------------------------------------------------

USE [VoucherDemo]
GO

/****** Object:  StoredProcedure [dbo].[usp_GetAllDiscountVouchersFilterByMerchantId]    Script Date: 1/16/2019 3:52:42 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[usp_GetAllDiscountVouchersFilterByMerchantId] 
@MerchantId nvarchar(100)

AS
SELECT VoucherId, MerchantId, Code, VoucherType, CreationDate, VoucherStatus, ExpiryDate, DiscountAmount, DiscountPercentage, DiscountUnit, RedemptionCount
FROM Voucher_DiscountView
WHERE @MerchantId = MerchantId

GO

-----------------------------------------------------------------------------------------------------------------------------

USE [VoucherDemo]
GO

/****** Object:  StoredProcedure [dbo].[usp_GetAllGiftVouchers]    Script Date: 1/16/2019 3:05:15 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[usp_GetAllGiftVouchers]
AS
SELECT VoucherId, MerchantId, Code, VoucherType, CreationDate, ExpiryDate, VoucherStatus, GiftAmount, GiftBalance
FROM Voucher_GiftView

GO

-----------------------------------------------------------------------------------------------------------------------------

USE [VoucherDemo]
GO

/****** Object:  StoredProcedure [dbo].[usp_GetAllGiftVouchersFilterByMerchantId]    Script Date: 1/16/2019 3:54:03 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[usp_GetAllGiftVouchersFilterByMerchantId]
@MerchantId nvarchar(100)

AS
SELECT VoucherId, MerchantId, Code, VoucherType, CreationDate, ExpiryDate, VoucherStatus, GiftAmount, GiftBalance
FROM Voucher_GiftView
WHERE @MerchantId = MerchantId

GO

-----------------------------------------------------------------------------------------------------------------------------

USE [VoucherDemo]
GO

/****** Object:  StoredProcedure [dbo].[usp_GetAllValueVouchers]    Script Date: 1/16/2019 3:05:45 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[usp_GetAllValueVouchers]

AS

SELECT VoucherId, MerchantId, Code, VoucherType, CreationDate, ExpiryDate, VoucherStatus, ValueAmount

FROM Voucher_ValueView

GO

-----------------------------------------------------------------------------------------------------------------------------

USE [VoucherDemo]
GO

/****** Object:  StoredProcedure [dbo].[usp_GetAllValueVouchersFilterByMerchantId]    Script Date: 1/16/2019 3:55:02 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[usp_GetAllValueVouchersFilterByMerchantId]
@MerchantId nvarchar(100)

AS

SELECT VoucherId, MerchantId, Code, VoucherType, CreationDate, ExpiryDate, VoucherStatus, ValueAmount

FROM Voucher_ValueView
WHERE @MerchantId = MerchantId

GO

-----------------------------------------------------------------------------------------------------------------------------

USE [VoucherDemo]
GO

/****** Object:  StoredProcedure [dbo].[usp_GetAllVouchers]    Script Date: 1/16/2019 3:06:18 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[usp_GetAllVouchers] 
AS
SELECT VoucherId, MerchantId, Code, VoucherType, CreationDate, ExpiryDate, VoucherStatus
FROM Voucher

GO

-----------------------------------------------------------------------------------------------------------------------------

USE [VoucherDemo]
GO

/****** Object:  StoredProcedure [dbo].[usp_GetAllVouchersFilterByMerchantId]    Script Date: 1/16/2019 3:55:51 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[usp_GetAllVouchersFilterByMerchantId]
@MerchantId BIGINT
 
AS
SELECT VoucherId, MerchantId, Code, VoucherType, CreationDate, ExpiryDate, VoucherStatus
FROM Voucher
WHERE @MerchantId = MerchantId

GO

-----------------------------------------------------------------------------------------------------------------------------

USE [VoucherDemo]
GO

/****** Object:  StoredProcedure [dbo].[usp_GetVoucherByCode]    Script Date: 1/16/2019 3:07:06 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[usp_GetVoucherByCode]
	@Code NVARCHAR(100) = NULL,
	@VoucherType NVARCHAR(50) = NULL

	AS
	BEGIN TRY
		BEGIN TRANSACTION 

			IF @VoucherType = 'Value'

				BEGIN
					-- body of the stored procedure
					-- Select rows from a Table or View 'Voucher_ValueView' in schema 'dbo'
					SELECT VoucherId,  MerchantId, Code, VoucherType, CreationDate, ExpiryDate, VoucherStatus, ValueAmount, VoucherId
					FROM dbo.Voucher_ValueView
					WHERE Code LIKE '%'+@Code+'%'
				END

			ELSE IF @VoucherType = 'Discount'
		
				BEGIN
					-- Select rows from a Table or View 'Voucher_ValueView' in schema 'dbo'
					SELECT VoucherId, MerchantId, Code, VoucherType, CreationDate, VoucherStatus, ExpiryDate, DiscountAmount, DiscountPercentage, DiscountUnit, RedemptionCount
					FROM dbo.Voucher_DiscountView
					WHERE Code LIKE '%'+@Code+'%'
				END

			ELSE IF @VoucherType = 'Gift' 
	
				BEGIN
					SELECT VoucherId, MerchantId, Code, VoucherType, CreationDate, ExpiryDate, VoucherStatus, GiftAmount, GiftBalance 
					FROM dbo.Voucher_GiftView
					WHERE Code LIKE '%'+@Code+'%'
				END

			ELSE

				BEGIN 
					SELECT  VoucherId, Code, VoucherType, CreationDate, ExpiryDate, VoucherStatus, MerchantId
					FROM dbo.Voucher
					WHERE Code LIKE '%'+@Code+'%'
				END
		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH	
		ROLLBACK
	END CATCH
GO

-----------------------------------------------------------------------------------------------------------------------------

USE [VoucherDemo]
GO

/****** Object:  StoredProcedure [dbo].[usp_GetVoucherByCodeFilterByMerchantId]    Script Date: 1/16/2019 3:07:41 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[usp_GetVoucherByCodeFilterByMerchantId]
	@Code NVARCHAR(100) = NULL,
	@VoucherType NVARCHAR(50) = NULL,
	@MerchantId NVARCHAR(100)

	AS
	BEGIN TRY
		BEGIN TRANSACTION 

			IF @VoucherType = 'Value'

				BEGIN
					-- body of the stored procedure
					-- Select rows from a Table or View 'Voucher_ValueView' in schema 'dbo'
					SELECT VoucherId,  MerchantId, Code, VoucherType, CreationDate, ExpiryDate, VoucherStatus, ValueAmount, VoucherId
					FROM dbo.Voucher_ValueView
					WHERE Code LIKE '%'+@Code+'%' AND @MerchantId = MerchantId AND VoucherStatus <> 'DELETED'
				END

			ELSE IF @VoucherType = 'Discount'
		
				BEGIN
					-- Select rows from a Table or View 'Voucher_ValueView' in schema 'dbo'
					SELECT VoucherId, MerchantId, Code, VoucherType, CreationDate, VoucherStatus, ExpiryDate, DiscountAmount, DiscountPercentage, DiscountUnit, RedemptionCount
					FROM dbo.Voucher_DiscountView
					WHERE Code LIKE '%'+@Code+'%' AND @MerchantId = MerchantId AND VoucherStatus <> 'DELETED'
				END

			ELSE IF @VoucherType = 'Gift' 
	
				BEGIN
					SELECT VoucherId, MerchantId, Code, VoucherType, CreationDate, ExpiryDate, VoucherStatus, GiftAmount, GiftBalance 
					FROM dbo.Voucher_GiftView
					WHERE Code LIKE '%'+@Code+'%' AND @MerchantId = MerchantId AND VoucherStatus <> 'DELETED'
				END

			ELSE

				BEGIN 
					SELECT  VoucherId, Code, VoucherType, CreationDate, ExpiryDate, VoucherStatus, MerchantId
					FROM dbo.Voucher
					WHERE Code LIKE '%'+@Code+'%' AND @MerchantId = MerchantId AND VoucherStatus <> 'DELETED'
				END
		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH	
		ROLLBACK
	END CATCH
GO

-----------------------------------------------------------------------------------------------------------------------------

USE [VoucherDemo]
GO

/****** Object:  StoredProcedure [dbo].[usp_GetVoucherByCreationDate]    Script Date: 1/16/2019 3:08:07 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[usp_GetVoucherByCreationDate]
	@CreationDate DATETIME = NULL,
	@VoucherType NVARCHAR(50) = NULL

	AS
	BEGIN TRY
		BEGIN TRANSACTION 

			IF @VoucherType = 'Value'

				BEGIN
					-- body of the stored procedure
					-- Select rows from a Table or View 'Voucher_ValueView' in schema 'dbo'
					SELECT VoucherId,  MerchantId, Code, VoucherType, CreationDate, ExpiryDate, VoucherStatus, ValueAmount, VoucherId
					FROM dbo.Voucher_ValueView
					WHERE 	CONVERT(date, @CreationDate) LIKE CONVERT(date, CreationDate)
				END

			ELSE IF @VoucherType = 'Discount'
		
				BEGIN
					-- Select rows from a Table or View 'Voucher_ValueView' in schema 'dbo'
					SELECT VoucherId, MerchantId, Code, VoucherType, CreationDate, VoucherStatus, ExpiryDate, DiscountAmount, DiscountPercentage, DiscountUnit, RedemptionCount
					FROM dbo.Voucher_DiscountView
					WHERE 	CONVERT(date, @CreationDate) LIKE CONVERT(date, CreationDate)
				END

			ELSE IF @VoucherType = 'Gift' 
	
				BEGIN
					SELECT VoucherId, MerchantId, Code, VoucherType, CreationDate, ExpiryDate, VoucherStatus, GiftAmount, GiftBalance 
					FROM dbo.Voucher_GiftView
					WHERE 	CONVERT(date, @CreationDate) LIKE CONVERT(date, CreationDate)
				END

			ELSE

				BEGIN 
					SELECT  VoucherId, Code, VoucherType, CreationDate, ExpiryDate, VoucherStatus, MerchantId
					FROM dbo.Voucher
					WHERE 	CONVERT(date, @CreationDate) LIKE CONVERT(date, CreationDate)
				END
		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH	
		ROLLBACK
	END CATCH
GO

-----------------------------------------------------------------------------------------------------------------------------

USE [VoucherDemo]
GO

/****** Object:  StoredProcedure [dbo].[usp_GetVoucherByCreationDateFilterByMerchantId]    Script Date: 1/16/2019 3:08:39 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[usp_GetVoucherByCreationDateFilterByMerchantId]
	@CreationDate DATETIME = NULL,
	@VoucherType NVARCHAR(50) = NULL,
	@MerchantId NVARCHAR(100)

	AS
	BEGIN TRY
		BEGIN TRANSACTION 

			IF @VoucherType = 'Value'

				BEGIN
					-- body of the stored procedure
					-- Select rows from a Table or View 'Voucher_ValueView' in schema 'dbo'
					SELECT VoucherId,  MerchantId, Code, VoucherType, CreationDate, ExpiryDate, VoucherStatus, ValueAmount, VoucherId
					FROM dbo.Voucher_ValueView
					WHERE 	CONVERT(date, @CreationDate) LIKE CONVERT(date, CreationDate) AND @MerchantId = MerchantId AND VoucherStatus <> 'DELETED'
				END

			ELSE IF @VoucherType = 'Discount'
		
				BEGIN
					-- Select rows from a Table or View 'Voucher_ValueView' in schema 'dbo'
					SELECT VoucherId, MerchantId, Code, VoucherType, CreationDate, VoucherStatus, ExpiryDate, DiscountAmount, DiscountPercentage, DiscountUnit, RedemptionCount
					FROM dbo.Voucher_DiscountView
					WHERE 	CONVERT(date, @CreationDate) LIKE CONVERT(date, CreationDate) AND @MerchantId = MerchantId AND VoucherStatus <> 'DELETED'
				END

			ELSE IF @VoucherType = 'Gift' 
	
				BEGIN
					SELECT VoucherId, MerchantId, Code, VoucherType, CreationDate, ExpiryDate, VoucherStatus, GiftAmount, GiftBalance 
					FROM dbo.Voucher_GiftView
					WHERE 	CONVERT(date, @CreationDate) LIKE CONVERT(date, CreationDate) AND @MerchantId = MerchantId AND VoucherStatus <> 'DELETED'
				END

			ELSE

				BEGIN 
					SELECT  VoucherId, Code, VoucherType, CreationDate, ExpiryDate, VoucherStatus, MerchantId
					FROM dbo.Voucher
					WHERE 	CONVERT(date, @CreationDate) LIKE CONVERT(date, CreationDate) AND @MerchantId = MerchantId AND VoucherStatus <> 'DELETED'
				END
		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH	
		ROLLBACK
	END CATCH
GO

-----------------------------------------------------------------------------------------------------------------------------

USE [VoucherDemo]
GO

/****** Object:  StoredProcedure [dbo].[usp_GetVoucherByExpiryDate]    Script Date: 1/16/2019 3:09:07 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[usp_GetVoucherByExpiryDate]
	@ExpiryDate DATETIME,
	@VoucherType NVARCHAR(50) = NULL

	AS
	BEGIN TRY
		BEGIN TRANSACTION 

			IF @VoucherType = 'Value'

				BEGIN
					-- body of the stored procedure
					-- Select rows from a Table or View 'Voucher_ValueView' in schema 'dbo'
					SELECT VoucherId,  MerchantId, Code, VoucherType, CreationDate, ExpiryDate, VoucherStatus, ValueAmount, VoucherId
					FROM dbo.Voucher_ValueView
					WHERE 	CONVERT(date, @ExpiryDate) LIKE CONVERT(date, ExpiryDate)
				END

			ELSE IF @VoucherType = 'Discount'
		
				BEGIN
					-- Select rows from a Table or View 'Voucher_ValueView' in schema 'dbo'
					SELECT VoucherId, MerchantId, Code, VoucherType, CreationDate, VoucherStatus, ExpiryDate, DiscountAmount, DiscountPercentage, DiscountUnit, RedemptionCount
					FROM dbo.Voucher_DiscountView
					WHERE 	CONVERT(date, @ExpiryDate) LIKE CONVERT(date, ExpiryDate)
				END

			ELSE IF @VoucherType = 'Gift' 
	
				BEGIN
					SELECT VoucherId, MerchantId, Code, VoucherType, CreationDate, ExpiryDate, VoucherStatus, GiftAmount, GiftBalance 
					FROM dbo.Voucher_GiftView
					WHERE 	CONVERT(date, @ExpiryDate) LIKE CONVERT(date, ExpiryDate)
				END

			ELSE

				BEGIN 
					SELECT  VoucherId, Code, VoucherType, CreationDate, ExpiryDate, VoucherStatus, MerchantId
					FROM dbo.Voucher
					WHERE 	CONVERT(date, @ExpiryDate) LIKE CONVERT(date, ExpiryDate)
				END
		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH	
		ROLLBACK
	END CATCH
GO

-----------------------------------------------------------------------------------------------------------------------------

USE [VoucherDemo]
GO

/****** Object:  StoredProcedure [dbo].[usp_GetVoucherByExpiryDateFilterByMerchantId]    Script Date: 1/16/2019 3:09:30 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[usp_GetVoucherByExpiryDateFilterByMerchantId]
	@ExpiryDate DATETIME,
	@VoucherType NVARCHAR(50) = NULL,
	@MerchantId NVARCHAR(100)

	AS
	BEGIN TRY
		BEGIN TRANSACTION 

			IF @VoucherType = 'Value'

				BEGIN
					-- body of the stored procedure
					-- Select rows from a Table or View 'Voucher_ValueView' in schema 'dbo'
					SELECT VoucherId,  MerchantId, Code, VoucherType, CreationDate, ExpiryDate, VoucherStatus, ValueAmount, VoucherId
					FROM dbo.Voucher_ValueView
					WHERE 	CONVERT(date, @ExpiryDate) LIKE CONVERT(date, ExpiryDate) AND @MerchantId = MerchantId AND VoucherStatus <> 'DELETED'
				END

			ELSE IF @VoucherType = 'Discount'
		
				BEGIN
					-- Select rows from a Table or View 'Voucher_ValueView' in schema 'dbo'
					SELECT VoucherId, MerchantId, Code, VoucherType, CreationDate, VoucherStatus, ExpiryDate, DiscountAmount, DiscountPercentage, DiscountUnit, RedemptionCount
					FROM dbo.Voucher_DiscountView
					WHERE 	CONVERT(date, @ExpiryDate) LIKE CONVERT(date, ExpiryDate) AND @MerchantId = MerchantId AND VoucherStatus <> 'DELETED'
				END

			ELSE IF @VoucherType = 'Gift' 
	
				BEGIN
					SELECT VoucherId, MerchantId, Code, VoucherType, CreationDate, ExpiryDate, VoucherStatus, GiftAmount, GiftBalance 
					FROM dbo.Voucher_GiftView
					WHERE 	CONVERT(date, @ExpiryDate) LIKE CONVERT(date, ExpiryDate) AND @MerchantId = MerchantId AND VoucherStatus <> 'DELETED'
				END

			ELSE

				BEGIN 
					SELECT  VoucherId, Code, VoucherType, CreationDate, ExpiryDate, VoucherStatus, MerchantId
					FROM dbo.Voucher
					WHERE 	CONVERT(date, @ExpiryDate) LIKE CONVERT(date, ExpiryDate) AND @MerchantId = MerchantId AND VoucherStatus <> 'DELETED'
				END
		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH	
		ROLLBACK
	END CATCH
GO

-----------------------------------------------------------------------------------------------------------------------------

USE [VoucherDemo]
GO

/****** Object:  StoredProcedure [dbo].[usp_GetVoucherById]    Script Date: 1/16/2019 3:09:56 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[usp_GetVoucherById]
	@VoucherId BIGINT,
	@VoucherType NVARCHAR(50) = NULL

	AS
	BEGIN TRY
		BEGIN TRANSACTION 

			IF @VoucherType = 'Value'

				BEGIN
					-- body of the stored procedure
					-- Select rows from a Table or View 'Voucher_ValueView' in schema 'dbo'
					SELECT VoucherId,  MerchantId, Code, VoucherType, CreationDate, ExpiryDate, VoucherStatus, ValueAmount, VoucherId
					FROM dbo.Voucher_ValueView
					WHERE 	@VoucherId = VoucherId
				END

			ELSE IF @VoucherType = 'Discount'
		
				BEGIN
					-- Select rows from a Table or View 'Voucher_ValueView' in schema 'dbo'
					SELECT VoucherId, MerchantId, Code, VoucherType, CreationDate, VoucherStatus, ExpiryDate, DiscountAmount, DiscountPercentage, DiscountUnit, RedemptionCount
					FROM dbo.Voucher_DiscountView
					WHERE @VoucherId = VoucherId
				END

			ELSE IF @VoucherType = 'Gift' 
	
				BEGIN
					SELECT VoucherId, Code, VoucherType, CreationDate, ExpiryDate, VoucherStatus, MerchantId, GiftAmount, GiftBalance 
					FROM dbo.Voucher_GiftView
					WHERE @VoucherId = VoucherId
				END

			ELSE

				BEGIN 
					SELECT  VoucherId, Code, VoucherType, CreationDate, ExpiryDate, VoucherStatus, MerchantId
					FROM dbo.Voucher
					WHERE @VoucherId = VoucherId
				END
		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH	
		ROLLBACK
	END CATCH
GO

-----------------------------------------------------------------------------------------------------------------------------

USE [VoucherDemo]
GO

/****** Object:  StoredProcedure [dbo].[usp_GetVoucherByIdFilterByMerchantId]    Script Date: 1/16/2019 3:10:24 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[usp_GetVoucherByIdFilterByMerchantId]
	@VoucherId BIGINT,
	@VoucherType NVARCHAR(50) = NULL,
	@MerchantId NVARCHAR(100)

	AS
	BEGIN TRY
		BEGIN TRANSACTION 

			IF @VoucherType = 'Value'

				BEGIN
					-- body of the stored procedure
					-- Select rows from a Table or View 'Voucher_ValueView' in schema 'dbo'
					SELECT VoucherId,  MerchantId, Code, VoucherType, CreationDate, ExpiryDate, VoucherStatus, ValueAmount, VoucherId
					FROM dbo.Voucher_ValueView
					WHERE 	@VoucherId = VoucherId AND @MerchantId = MerchantId AND VoucherStatus <> 'DELETED'
				END

			ELSE IF @VoucherType = 'Discount'
		
				BEGIN
					-- Select rows from a Table or View 'Voucher_ValueView' in schema 'dbo'
					SELECT VoucherId, MerchantId, Code, VoucherType, CreationDate, VoucherStatus, ExpiryDate, DiscountAmount, DiscountPercentage, DiscountUnit, RedemptionCount
					FROM dbo.Voucher_DiscountView
					WHERE @VoucherId = VoucherId AND @MerchantId = MerchantId AND VoucherStatus <> 'DELETED'
				END

			ELSE IF @VoucherType = 'Gift' 
	
				BEGIN
					SELECT VoucherId, Code, VoucherType, CreationDate, ExpiryDate, VoucherStatus, MerchantId, GiftAmount, GiftBalance 
					FROM dbo.Voucher_GiftView
					WHERE @VoucherId = VoucherId AND @MerchantId = MerchantId AND VoucherStatus <> 'DELETED'
				END

			ELSE

				BEGIN 
					SELECT  VoucherId, Code, VoucherType, CreationDate, ExpiryDate, VoucherStatus, MerchantId
					FROM dbo.Voucher
					WHERE @VoucherId = VoucherId AND @MerchantId = MerchantId AND VoucherStatus <> 'DELETED'
				END
		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH	
		ROLLBACK
	END CATCH
GO

-----------------------------------------------------------------------------------------------------------------------------

USE [VoucherDemo]
GO

/****** Object:  StoredProcedure [dbo].[usp_GetVoucherByMerchantId]    Script Date: 1/16/2019 3:10:58 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[usp_GetVoucherByMerchantId]
	@MerchantId NVARCHAR(100),
	@VoucherType NVARCHAR(50) = NULL

	AS
	BEGIN TRY
		BEGIN TRANSACTION 

			IF @VoucherType = 'Value'

				BEGIN
					-- body of the stored procedure
					-- Select rows from a Table or View 'Voucher_ValueView' in schema 'dbo'
					SELECT VoucherId,  MerchantId, Code, VoucherType, CreationDate, ExpiryDate, VoucherStatus, ValueAmount, VoucherId
					FROM dbo.Voucher_ValueView
					WHERE 	@MerchantId = MerchantId
				END

			ELSE IF @VoucherType = 'Discount'
		
				BEGIN
					-- Select rows from a Table or View 'Voucher_ValueView' in schema 'dbo'
					SELECT VoucherId, MerchantId, Code, VoucherType, CreationDate, VoucherStatus, ExpiryDate, DiscountAmount, DiscountPercentage, DiscountUnit, RedemptionCount
					FROM dbo.Voucher_DiscountView
					WHERE 	@MerchantId = MerchantId
				END

			ELSE IF @VoucherType = 'Gift' 
	
				BEGIN
					SELECT VoucherId, MerchantId, Code, VoucherType, CreationDate, ExpiryDate, VoucherStatus, GiftAmount, GiftBalance 
					FROM dbo.Voucher_GiftView
					WHERE 	@MerchantId = MerchantId
				END

			ELSE

				BEGIN 
					SELECT  VoucherId, Code, VoucherType, CreationDate, ExpiryDate, VoucherStatus, MerchantId
					FROM dbo.Voucher
					WHERE 	@MerchantId = MerchantId
				END
		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH	
		ROLLBACK
	END CATCH
GO

-----------------------------------------------------------------------------------------------------------------------------

USE [VoucherDemo]
GO

/****** Object:  StoredProcedure [dbo].[usp_GetVoucherByStatus]    Script Date: 1/16/2019 3:11:27 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[usp_GetVoucherByStatus]
	@VoucherStatus NVARCHAR(10),
	@VoucherType NVARCHAR(50) = NULL

	AS
	BEGIN TRY
		BEGIN TRANSACTION 

			IF @VoucherType = 'Value'

				BEGIN
					-- body of the stored procedure
					-- Select rows from a Table or View 'Voucher_ValueView' in schema 'dbo'
					SELECT VoucherId,  MerchantId, Code, VoucherType, CreationDate, ExpiryDate, VoucherStatus, ValueAmount, VoucherId
					FROM dbo.Voucher_ValueView
					WHERE 	@VoucherStatus = VoucherStatus
				END

			ELSE IF @VoucherType = 'Discount'
		
				BEGIN
					-- Select rows from a Table or View 'Voucher_ValueView' in schema 'dbo'
					SELECT VoucherId, MerchantId, Code, VoucherType, CreationDate, VoucherStatus, ExpiryDate, DiscountAmount, DiscountPercentage, DiscountUnit, RedemptionCount
					FROM dbo.Voucher_DiscountView
					WHERE 	@VoucherStatus = VoucherStatus
				END

			ELSE IF @VoucherType = 'Gift' 
	
				BEGIN
					SELECT VoucherId, MerchantId, Code, VoucherType, CreationDate, ExpiryDate, VoucherStatus, GiftAmount, GiftBalance 
					FROM dbo.Voucher_GiftView
					WHERE 	@VoucherStatus = VoucherStatus
				END

			ELSE

				BEGIN 
					SELECT  VoucherId, Code, VoucherType, CreationDate, ExpiryDate, VoucherStatus, MerchantId
					FROM dbo.Voucher
					WHERE 	@VoucherStatus = VoucherStatus
				END
		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH	
		ROLLBACK
	END CATCH
GO

-----------------------------------------------------------------------------------------------------------------------------

USE [VoucherDemo]
GO

/****** Object:  StoredProcedure [dbo].[usp_GetVoucherByStatusFilterByMerchantId]    Script Date: 1/16/2019 3:12:59 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[usp_GetVoucherByStatusFilterByMerchantId]
	@VoucherStatus NVARCHAR(10),
	@VoucherType NVARCHAR(50) = NULL,
	@MerchantId NVARCHAR(100)

	AS
	BEGIN TRY
		BEGIN TRANSACTION 

			IF @VoucherType = 'Value'

				BEGIN
					-- body of the stored procedure
					-- Select rows from a Table or View 'Voucher_ValueView' in schema 'dbo'
					SELECT VoucherId,  MerchantId, Code, VoucherType, CreationDate, ExpiryDate, VoucherStatus, ValueAmount, VoucherId
					FROM dbo.Voucher_ValueView
					WHERE 	@VoucherStatus = VoucherStatus AND @MerchantId = MerchantId
				END

			ELSE IF @VoucherType = 'Discount'
		
				BEGIN
					-- Select rows from a Table or View 'Voucher_ValueView' in schema 'dbo'
					SELECT VoucherId, MerchantId, Code, VoucherType, CreationDate, VoucherStatus, ExpiryDate, DiscountAmount, DiscountPercentage, DiscountUnit, RedemptionCount
					FROM dbo.Voucher_DiscountView
					WHERE 	@VoucherStatus = VoucherStatus AND @MerchantId = MerchantId
				END

			ELSE IF @VoucherType = 'Gift' 
	
				BEGIN
					SELECT VoucherId, MerchantId, Code, VoucherType, CreationDate, ExpiryDate, VoucherStatus, GiftAmount, GiftBalance 
					FROM dbo.Voucher_GiftView
					WHERE 	@VoucherStatus = VoucherStatus AND @MerchantId = MerchantId
				END

			ELSE

				BEGIN 
					SELECT  VoucherId, Code, VoucherType, CreationDate, ExpiryDate, VoucherStatus, MerchantId
					FROM dbo.Voucher
					WHERE 	@VoucherStatus = VoucherStatus AND @MerchantId = MerchantId
				END
		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH	
		ROLLBACK
	END CATCH
GO

-----------------------------------------------------------------------------------------------------------------------------

USE [VoucherDemo]
GO

/****** Object:  StoredProcedure [dbo].[usp_UpdateGiftAmountByCode]    Script Date: 1/16/2019 3:13:23 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[usp_UpdateGiftAmountByCode]
   @Code nvarchar(100),
   @GiftAmount BIGINT


AS
   BEGIN TRY
		BEGIN TRANSACTION UpdateGiftAmountByCode
 
				-- Update rows in table 'TableName'
				UPDATE [dbo].[Voucher_GiftView]
				SET
					[GiftAmount] = @GiftAmount, GiftBalance = GiftBalance + @GiftAmount
					-- add more columns and values here
				WHERE @Code=Code AND @GiftAmount > GiftAmount
			 
		COMMIT TRANSACTION UpdateGiftAmountByCode

    END TRY

    BEGIN CATCH
        ROLLBACK TRANSACTION
    END CATCH


GO

-----------------------------------------------------------------------------------------------------------------------------

USE [VoucherDemo]
GO

/****** Object:  StoredProcedure [dbo].[usp_UpdateGiftAmountById]    Script Date: 1/16/2019 3:13:46 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[usp_UpdateGiftAmountById]
   @VoucherId BIGINT,
   @GiftAmount BIGINT


AS
   BEGIN TRY
		BEGIN TRANSACTION UpdateGiftAmountById
 
				-- Update rows in table 'TableName'
				UPDATE [dbo].[Voucher_GiftView]
				SET
					[GiftAmount] = @GiftAmount, GiftBalance = GiftBalance + @GiftAmount
					-- add more columns and values here
				WHERE @VoucherId=VoucherId AND @GiftAmount > GiftAmount
			 
		COMMIT TRANSACTION UpdateGiftAmountById

    END TRY

    BEGIN CATCH
        ROLLBACK TRANSACTION
    END CATCH


GO

-----------------------------------------------------------------------------------------------------------------------------

USE [VoucherDemo]
GO

/****** Object:  StoredProcedure [dbo].[usp_UpdateVoucherExpiryDateByCode]    Script Date: 1/16/2019 3:14:09 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[usp_UpdateVoucherExpiryDateByCode]
   @Code nvarchar(100),
   @ExpiryDate DATETIME


AS
   BEGIN TRY
		BEGIN TRANSACTION UpdateVoucherExpiryDateByCode
 
				-- Update rows in table 'TableName'
				UPDATE Voucher
				SET
					[ExpiryDate] =@ExpiryDate
					-- add more columns and values here
				WHERE @Code=Code AND VoucherStatus <> 'DELETED'

		COMMIT TRANSACTION UpdateVoucherExpiryDateByCode

    END TRY

    BEGIN CATCH
        ROLLBACK TRANSACTION
    END CATCH


GO

-----------------------------------------------------------------------------------------------------------------------------

USE [VoucherDemo]
GO

/****** Object:  StoredProcedure [dbo].[usp_UpdateVoucherExpiryDateById]    Script Date: 1/16/2019 3:14:35 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[usp_UpdateVoucherExpiryDateById]
   @VoucherId nvarchar(100),
   @ExpiryDate DATETIME


AS
   BEGIN TRY
		BEGIN TRANSACTION UpdateVoucherExpiryDateById
 
				-- Update rows in table 'TableName'
				UPDATE Voucher
				SET
					[ExpiryDate] =@ExpiryDate
					-- add more columns and values here
				WHERE @VoucherId=VoucherId AND VoucherStatus <> 'DELETED'

		COMMIT TRANSACTION UpdateVoucherExpiryDateById

    END TRY

    BEGIN CATCH
        ROLLBACK TRANSACTION
    END CATCH


GO

-----------------------------------------------------------------------------------------------------------------------------

USE [VoucherDemo]
GO

/****** Object:  StoredProcedure [dbo].[usp_UpdateVoucherStatusByCode]    Script Date: 1/16/2019 3:15:03 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[usp_UpdateVoucherStatusByCode]
   @Code nvarchar(100),
   @VoucherStatus VARCHAR(8)


AS
   BEGIN TRY
		BEGIN TRANSACTION UpdateVoucherStatusByCode

			IF @VoucherStatus = 'ACTIVE'  
				-- Update rows in table 'TableName'
				UPDATE Voucher
				SET
					[VoucherStatus] =@VoucherStatus
					-- add more columns and values here
				WHERE @Code=Code

			ELSE IF @VoucherStatus = 'INACTIVE'
				-- Update rows in table 'TableName'
				UPDATE Voucher
				SET
					[VoucherStatus] =@VoucherStatus
					-- add more columns and values here
				WHERE @Code=Code
			 
		COMMIT TRANSACTION UpdateVoucherStatusByCode

    END TRY

    BEGIN CATCH
        ROLLBACK TRANSACTION
    END CATCH


GO

-----------------------------------------------------------------------------------------------------------------------------

USE [VoucherDemo]
GO

/****** Object:  StoredProcedure [dbo].[usp_UpdateVoucherStatusById]    Script Date: 1/16/2019 3:15:29 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[usp_UpdateVoucherStatusById]
   @VoucherId nvarchar(100),
   @VoucherStatus VARCHAR(8)


AS
   BEGIN TRY
		BEGIN TRANSACTION UpdateVoucherStatusById

			IF @VoucherStatus = 'ACTIVE'  
				-- Update rows in table 'TableName'
				UPDATE Voucher
				SET
					[VoucherStatus] =@VoucherStatus
					-- add more columns and values here
				WHERE VoucherId=@VoucherId

			ELSE IF @VoucherStatus = 'INACTIVE'
				-- Update rows in table 'TableName'
				UPDATE Voucher
				SET
					[VoucherStatus] =@VoucherStatus
					-- add more columns and values here
				WHERE VoucherId=@VoucherId

		COMMIT TRANSACTION UpdateVoucherStatusById

    END TRY

    BEGIN CATCH
        ROLLBACK TRANSACTION
    END CATCH


GO

-----------------------------------------------------------------------------------------------------------------------------

