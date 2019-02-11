
USE master
GO
IF NOT EXISTS (
    SELECT name
        FROM sys.databases
        WHERE name = N'Demo'
)
CREATE DATABASE Demo
GO
-----------------------------

IF OBJECT_ID('[dbo].People', 'U') IS NOT NULL
DROP TABLE [dbo].People
GO

CREATE TABLE [dbo].People
(
    PeopleId INT NOT NULL PRIMARY KEY IDENTITY(1, 1), -- primary key column
    [FName] [NVARCHAR](50) NOT NULL,
    [LName] [NVARCHAR](50) NOT NULL,
    -- specify more columns here
);
GO

CREATE TYPE [dbo].PeopleType AS TABLE 
(
    PeopleId INT NOT NULL PRIMARY KEY IDENTITY(1, 1), -- primary key column
    [FName] [NVARCHAR](50) NOT NULL,
    [LName] [NVARCHAR](50) NOT NULL
    -- specify more columns here
);
GO


IF EXISTS (SELECT *FROM INFORMATION_SCHEMA.ROUTINES 
    WHERE SPECIFIC_SCHEMA = N'dbo' AND SPECIFIC_NAME = N'usp_InsertPple')
DROP PROCEDURE dbo.usp_InsertPple
GO

CREATE PROCEDURE dbo.usp_InsertPple  
    @ppt AS PeopleType READONLY
AS
    DECLARE @ppl as PeopleType 

    INSERT INTO @ppl ( FName, LName)
    VALUES (N'JOpe', N'OlaYemi')

    INSERT INTO [dbo].People (FName, LName)
    SELECT FName, LName 
    FROM @ppt


EXECUTE dbo.usp_InsertPple @ppl
GO

SELECT * FROM People




