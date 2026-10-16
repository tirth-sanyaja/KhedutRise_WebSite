-- =========================================================================================
-- KHEDUTRISE RELATIONAL DATABASE SCHEMA (KhedutRiseDB)
-- Fully Normalized Schema with Primary Keys, Foreign Keys, Constraints, and JOIN Operations
-- Platform: Microsoft SQL Server / LocalDB | Framework: ASP.NET Web Forms & ADO.NET
-- =========================================================================================

/*
   =========================================================================================
   ENTITY RELATIONSHIP (ER) OVERVIEW:
   =========================================================================================
   
      +-----------------------------------------------------------+
      |                         Users                             |
      |-----------------------------------------------------------|
      | PK: UserId                                                |
      | FullName, MobileNo (UNIQUE), Email, UserRole, Location    |
      +-----------------------------------------------------------+
            |                  |                    |       |
            | 1:N (FarmerId)   | 1:N (FarmerId)     |       | 1:N (SellerId)
            v                  v                    |       v
      +----------------+ +----------------+         |  +--------------------+
      |    Products    | |   FarmPlots    |         |  |   LandListings     |
      |----------------| |----------------|         |  |--------------------|
      | PK: ProductId  | | PK: PlotId     |         |  | PK: LandId         |
      | FK: FarmerId   | | FK: FarmerId   |         |  | FK: SellerId       |
      | Crop/Price/Qty | | Area/Crop/GPS  |         |  | Area/Price/Water   |
      +----------------+ +----------------+         |  +--------------------+
            |                  ^                    |
            | 1:N (ProductId)  |                    | 1:N (OwnerId)
            v                  |                    v
      +--------------------------------+       +--------------------+
      |             Orders             |       |     Equipments     |
      |--------------------------------|       |--------------------|
      | PK: OrderId                    |       | PK: EquipmentId    |
      | FK: BuyerId  -> Users(UserId)  |       | FK: OwnerId        |
      | FK: FarmerId -> Users(UserId)  |       | Tractor/Machinery  |
      | FK: ProductId-> Products       |       +--------------------+
      | TotalPrice, Status, OrderDate  |                    |
      +--------------------------------+                    | 1:N (UserId)
                                                            v
      +--------------------------------+       +--------------------+
      |       ContactInquiries         |       |      Bookings      |
      |--------------------------------|       |--------------------|
      | PK: InquiryId                  |       | PK: BookingId      |
      | FullName, MobileNo, Message    |       | FK: UserId         |
      | SubmittedDate                  |       | Soil/Drone Testing |
      +--------------------------------+       +--------------------+

      +--------------------------------+
      |           ApmcRates            |
      |--------------------------------|
      | PK: RateId                     |
      | Commodity, Yard, Min/Max/Avg   |
      +--------------------------------+
*/

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'KhedutRiseDB')
BEGIN
    CREATE DATABASE KhedutRiseDB;
END
GO

USE KhedutRiseDB;
GO

-- =========================================================================================
-- 1. USERS TABLE (Primary Master Entity)
-- Stores Farmers, Buyers, Agri-Equipment Operators, and System Administrators
-- =========================================================================================
IF OBJECT_ID('Users', 'U') IS NULL
BEGIN
    CREATE TABLE Users (
        UserId INT PRIMARY KEY IDENTITY(1001, 1),
        FullName NVARCHAR(100) NOT NULL,
        MobileNo NVARCHAR(20) NOT NULL CONSTRAINT UQ_Users_Mobile UNIQUE,
        Email NVARCHAR(100) NULL,
        UserRole NVARCHAR(20) NOT NULL,                 -- 'Farmer', 'Buyer', 'Operator', 'Admin'
        Location NVARCHAR(100) NULL,
        SpecificField NVARCHAR(200) NULL,               -- e.g. 'Cotton / Wheat', 'Wholesale Buyer'
        Status NVARCHAR(20) NOT NULL CONSTRAINT DF_Users_Status DEFAULT 'Verified',
        CreatedDate DATETIME NOT NULL CONSTRAINT DF_Users_CreatedDate DEFAULT GETDATE()
    );
END
GO

-- =========================================================================================
-- 2. PRODUCTS TABLE (Agricultural Produce Catalog)
-- Foreign Key: FarmerId -> References Users(UserId)
-- =========================================================================================
IF OBJECT_ID('Products', 'U') IS NULL
BEGIN
    CREATE TABLE Products (
        ProductId INT PRIMARY KEY IDENTITY(1, 1),
        FarmerId INT NULL CONSTRAINT FK_Products_Users REFERENCES Users(UserId),
        ProductName NVARCHAR(100) NOT NULL,             -- e.g. 'Premium Groundnut', 'Organic Bt Cotton'
        Category NVARCHAR(50) NOT NULL,                -- 'Groundnut', 'Cotton', 'Wheat', 'Spices'
        QuantityInQuintal DECIMAL(10, 2) NOT NULL,      -- Available harvest in quintals
        PricePerQuintal DECIMAL(10, 2) NOT NULL,        -- Price in INR per quintal
        FarmerName NVARCHAR(100) NOT NULL,             -- Fallback display name
        Location NVARCHAR(100) NOT NULL,               -- Village / APMC market location
        Grade NVARCHAR(20) CONSTRAINT DF_Products_Grade DEFAULT 'Grade A',
        IsApproved BIT CONSTRAINT DF_Products_IsApproved DEFAULT 1,
        CreatedDate DATETIME CONSTRAINT DF_Products_CreatedDate DEFAULT GETDATE()
    );
END
GO

-- =========================================================================================
-- 3. FARMPLOTS TABLE (Farmer Farmland GPS Registry)
-- Foreign Key: FarmerId -> References Users(UserId)
-- =========================================================================================
IF OBJECT_ID('FarmPlots', 'U') IS NULL
BEGIN
    CREATE TABLE FarmPlots (
        PlotId INT PRIMARY KEY IDENTITY(1, 1),
        FarmerId INT NULL CONSTRAINT FK_FarmPlots_Users REFERENCES Users(UserId),
        PlotName NVARCHAR(50) NOT NULL,                -- e.g. 'Plot A - North Field'
        AreaAcres DECIMAL(6, 2) NOT NULL,              -- Size in acres
        Crop NVARCHAR(50) NOT NULL,                    -- Currently sown crop
        Stage NVARCHAR(50) NOT NULL,                   -- 'Flowering', 'Boll Dev', 'Harvesting'
        Status NVARCHAR(30) CONSTRAINT DF_FarmPlots_Status DEFAULT 'Healthy',
        SoilType NVARCHAR(50) CONSTRAINT DF_FarmPlots_SoilType DEFAULT 'Clayey Loam'
    );
END
GO

-- =========================================================================================
-- 4. APMCRATES TABLE (Real-time Mandi Yard Telemetry)
-- =========================================================================================
IF OBJECT_ID('ApmcRates', 'U') IS NULL
BEGIN
    CREATE TABLE ApmcRates (
        RateId INT PRIMARY KEY IDENTITY(1, 1),
        Commodity NVARCHAR(50) NOT NULL,                -- 'Groundnut', 'Cotton', 'Wheat', 'Cumin'
        MarketYard NVARCHAR(100) NOT NULL,              -- 'Rajkot APMC', 'Gondal APMC'
        MinPrice DECIMAL(10, 2) NOT NULL,
        MaxPrice DECIMAL(10, 2) NOT NULL,
        AvgPrice DECIMAL(10, 2) NOT NULL,
        ArrivalTons DECIMAL(10, 2) CONSTRAINT DF_ApmcRates_ArrivalTons DEFAULT 100,
        UpdatedDate DATETIME CONSTRAINT DF_ApmcRates_UpdatedDate DEFAULT GETDATE()
    );
END
GO

-- =========================================================================================
-- 5. ORDERS TABLE (Buyer Purchase Contracts & Escrow Tracking)
-- Foreign Keys: 
--   - BuyerId  -> References Users(UserId)
--   - FarmerId -> References Users(UserId)
--   - ProductId-> References Products(ProductId)
-- =========================================================================================
IF OBJECT_ID('Orders', 'U') IS NULL
BEGIN
    CREATE TABLE Orders (
        OrderId INT PRIMARY KEY IDENTITY(10400, 1),
        BuyerId INT NULL CONSTRAINT FK_Orders_Users_Buyer REFERENCES Users(UserId),
        FarmerId INT NULL CONSTRAINT FK_Orders_Users_Farmer REFERENCES Users(UserId),
        ProductId INT NULL CONSTRAINT FK_Orders_Products REFERENCES Products(ProductId),
        BuyerName NVARCHAR(100) NOT NULL,
        FarmerName NVARCHAR(100) NOT NULL,
        Commodity NVARCHAR(100) NOT NULL,
        Quantity NVARCHAR(50) NOT NULL,
        TotalPrice DECIMAL(12, 2) NOT NULL,
        OrderDate DATETIME CONSTRAINT DF_Orders_OrderDate DEFAULT GETDATE(),
        Status NVARCHAR(50) CONSTRAINT DF_Orders_Status DEFAULT 'Processing', -- 'Delivered', 'In Transit', 'Processing', 'Cancelled'
        DeliveryAddress NVARCHAR(200) NULL
    );
END
GO

-- =========================================================================================
-- 6. EQUIPMENTS TABLE (Tractor & Agricultural Machinery Hire)
-- Foreign Key: OwnerId -> References Users(UserId)
-- =========================================================================================
IF OBJECT_ID('Equipments', 'U') IS NULL
BEGIN
    CREATE TABLE Equipments (
        EquipmentId INT PRIMARY KEY IDENTITY(1, 1),
        OwnerId INT NULL CONSTRAINT FK_Equipments_Users REFERENCES Users(UserId),
        ModelName NVARCHAR(100) NOT NULL,               -- e.g. 'Massey Ferguson 241DI'
        Horsepower NVARCHAR(20) NOT NULL,               -- e.g. '41 HP'
        HourlyRate DECIMAL(10, 2) NOT NULL,             -- Rental rate per hour
        OwnerName NVARCHAR(100) NOT NULL,
        Location NVARCHAR(100) NOT NULL,
        Rating DECIMAL(3, 1) CONSTRAINT DF_Equipments_Rating DEFAULT 4.8
    );
END
GO

-- =========================================================================================
-- 7. BOOKINGS TABLE (Farm Soil Testing & Drone Spraying Service Requests)
-- Foreign Key: UserId -> References Users(UserId)
-- =========================================================================================
IF OBJECT_ID('Bookings', 'U') IS NULL
BEGIN
    CREATE TABLE Bookings (
        BookingId NVARCHAR(30) PRIMARY KEY,             -- e.g. '#KR-BK-408'
        UserId INT NULL CONSTRAINT FK_Bookings_Users REFERENCES Users(UserId),
        ServiceType NVARCHAR(100) NOT NULL,             -- 'Soil Lab Test Visit', 'Drone Nano Urea Spray'
        ProviderName NVARCHAR(100) NOT NULL,
        CustomerName NVARCHAR(100) NOT NULL,
        BookingDate DATETIME CONSTRAINT DF_Bookings_BookingDate DEFAULT GETDATE(),
        Amount DECIMAL(10, 2) NOT NULL,
        Status NVARCHAR(50) CONSTRAINT DF_Bookings_Status DEFAULT 'Confirmed'
    );
END
GO

-- =========================================================================================
-- 8. LANDLISTINGS TABLE (Farmland Property Market)
-- Foreign Key: SellerId -> References Users(UserId)
-- =========================================================================================
IF OBJECT_ID('LandListings', 'U') IS NULL
BEGIN
    CREATE TABLE LandListings (
        LandId INT PRIMARY KEY IDENTITY(1, 1),
        SellerId INT NULL CONSTRAINT FK_LandListings_Users REFERENCES Users(UserId),
        Title NVARCHAR(150) NOT NULL,
        AreaAcres DECIMAL(6, 2) NOT NULL,
        Location NVARCHAR(100) NOT NULL,
        WaterSource NVARCHAR(50) NOT NULL,
        Price NVARCHAR(50) NOT NULL,
        SoilType NVARCHAR(50) NOT NULL,
        Status NVARCHAR(20) CONSTRAINT DF_LandListings_Status DEFAULT 'Approved'
    );
END
GO

-- =========================================================================================
-- 9. CONTACTINQUIRIES TABLE (Customer Support & Farmer Helpdesk)
-- =========================================================================================
IF OBJECT_ID('ContactInquiries', 'U') IS NULL
BEGIN
    CREATE TABLE ContactInquiries (
        InquiryId INT PRIMARY KEY IDENTITY(1, 1),
        FullName NVARCHAR(100) NOT NULL,
        MobileNo NVARCHAR(20) NOT NULL,
        Message NVARCHAR(MAX) NOT NULL,
        SubmittedDate DATETIME CONSTRAINT DF_ContactInquiries_SubmittedDate DEFAULT GETDATE()
    );
END
GO

-- =========================================================================================
-- SAMPLE RELATIONAL SQL JOIN QUERIES (Used in ASP.NET Code & Viva Demonstrations)
-- =========================================================================================

-- Example 1: Multi-Table JOIN across Orders, Buyer (Users), Farmer (Users), and Products
-- Demonstrates: Foreign Key navigation, Aliasing, ISNULL null-coalescing
/*
SELECT 
    o.OrderId,
    uBuyer.FullName AS BuyerName,
    uBuyer.MobileNo AS BuyerMobile,
    uFarmer.FullName AS FarmerName,
    uFarmer.Location AS FarmerLocation,
    p.ProductName AS CropName,
    p.Category,
    o.Quantity,
    o.TotalPrice,
    o.Status,
    o.OrderDate
FROM Orders o
INNER JOIN Users uBuyer ON o.BuyerId = uBuyer.UserId
INNER JOIN Users uFarmer ON o.FarmerId = uFarmer.UserId
INNER JOIN Products p ON o.ProductId = p.ProductId
ORDER BY o.OrderId DESC;
*/

-- Example 2: LEFT JOIN on Products and Users (Farmer Profile)
-- Used in: Marketplace.aspx & AdminManagement.aspx
/*
SELECT 
    p.ProductId,
    p.ProductName,
    p.Category,
    p.PricePerQuintal,
    p.QuantityInQuintal,
    p.Grade,
    ISNULL(u.FullName, p.FarmerName) AS FarmerName,
    ISNULL(u.Location, p.Location) AS Location,
    u.MobileNo AS FarmerContact
FROM Products p
LEFT JOIN Users u ON p.FarmerId = u.UserId
WHERE p.IsApproved = 1;
*/

-- Example 3: LEFT JOIN on FarmPlots and Users (Land Ownership)
-- Used in: FarmPlots.aspx
/*
SELECT 
    f.PlotId,
    f.PlotName,
    f.AreaAcres,
    f.Crop,
    f.Stage,
    f.Status,
    f.SoilType,
    ISNULL(u.FullName, 'Registered Farmer') AS FarmerName,
    u.Location AS Village
FROM FarmPlots f
LEFT JOIN Users u ON f.FarmerId = u.UserId
ORDER BY f.PlotId ASC;
*/
