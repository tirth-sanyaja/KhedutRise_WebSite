-- =========================================================================
-- KHEDUTRISE RELATIONAL DATABASE SCHEMA UPDATE
-- Script: Create_OTP_Table.sql
-- Purpose: Creates the UserOTPs table for secure SMS OTP authentication
--          and seeds/verifies the System Administrator account.
-- =========================================================================

USE KhedutRiseDB;
GO

-- 1. Create UserOTPs Table for Secure SMS OTP Authentication
IF OBJECT_ID('UserOTPs', 'U') IS NULL
BEGIN
    CREATE TABLE UserOTPs (
        OTPId INT PRIMARY KEY IDENTITY(1, 1),
        UserId INT NULL CONSTRAINT FK_UserOTPs_Users REFERENCES Users(UserId),
        MobileNo NVARCHAR(20) NOT NULL,
        OTPHash NVARCHAR(256) NOT NULL,
        OTPSalt NVARCHAR(64) NOT NULL,
        OTPExpiry DATETIME NOT NULL,
        OTPAttempts INT NOT NULL CONSTRAINT DF_UserOTPs_Attempts DEFAULT 0,
        MaxAttempts INT NOT NULL CONSTRAINT DF_UserOTPs_MaxAttempts DEFAULT 5,
        OTPCreatedAt DATETIME NOT NULL CONSTRAINT DF_UserOTPs_CreatedAt DEFAULT GETDATE(),
        OTPSentAt DATETIME NULL,
        OTPPurpose NVARCHAR(50) NOT NULL CONSTRAINT DF_UserOTPs_Purpose DEFAULT 'LOGIN',
        IsUsed BIT NOT NULL CONSTRAINT DF_UserOTPs_IsUsed DEFAULT 0,
        UserRole NVARCHAR(20) NULL,
        DeliveryStatus NVARCHAR(50) NULL,
        GatewayResponse NVARCHAR(500) NULL,
        IPAddress NVARCHAR(50) NULL
    );

    CREATE INDEX IX_UserOTPs_Mobile_Purpose ON UserOTPs(MobileNo, OTPPurpose, IsUsed);
    CREATE INDEX IX_UserOTPs_CreatedAt ON UserOTPs(OTPCreatedAt);
END
GO

-- 2. Ensure System Administrator Account Exists in Users Table
IF NOT EXISTS (SELECT 1 FROM Users WHERE UserRole = 'Admin' OR Email = 'admin@khedutrise.com')
BEGIN
    INSERT INTO Users (FullName, MobileNo, Email, UserRole, Location, SpecificField, Status, CreatedDate)
    VALUES ('System Admin', '9876500001', 'admin@khedutrise.com', 'Admin', 'Gandhinagar, Gujarat', 'Platform Superuser', 'Verified', GETDATE());
END
ELSE
BEGIN
    UPDATE Users 
    SET UserRole = 'Admin', Status = 'Verified'
    WHERE Email = 'admin@khedutrise.com';
END
GO
