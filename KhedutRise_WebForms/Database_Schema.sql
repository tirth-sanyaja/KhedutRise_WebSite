IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'KhedutRiseDB')
BEGIN
    CREATE DATABASE KhedutRiseDB;
END
GO

USE KhedutRiseDB;
GO

--1. Users Table: Stores registered Farmers, Buyers, Operators, and Admins
IF OBJECT_ID('Users', 'U') IS NULL
BEGIN
    CREATE TABLE Users (
        UserId INT PRIMARY KEY IDENTITY(1,1),
        FullName NVARCHAR(100) NOT NULL,
        MobileNo NVARCHAR(20) NOT NULL UNIQUE,
        Email NVARCHAR(100) NULL,
        UserRole NVARCHAR(20) NOT NULL, -- Roles: 'Farmer', 'Buyer', 'Operator', 'Admin'
        Location NVARCHAR(100) NULL,
        SpecificField NVARCHAR(200) NULL, -- Sowing Crop / Business Type / Tractor Model
        Status NVARCHAR(20) DEFAULT 'Verified', -- 'Verified', 'Pending'
        CreatedDate DATETIME DEFAULT GETDATE()
    );
END
GO

-- 2. Products Table: Core table for agricultural produce CRUD (Page 19 & 14)
IF OBJECT_ID('Products', 'U') IS NULL
BEGIN
    CREATE TABLE Products (
        ProductId INT PRIMARY KEY IDENTITY(1,1),
        ProductName NVARCHAR(100) NOT NULL,       -- e.g. 'Premium Groundnut', 'Cotton G-10'
        Category NVARCHAR(50) NOT NULL,          -- 'Cotton', 'Groundnut', 'Wheat', 'Spices'
        QuantityInQuintal DECIMAL(10,2) NOT NULL,-- Available quantity
        PricePerQuintal DECIMAL(10,2) NOT NULL,  -- Price per quintal in INR
        FarmerName NVARCHAR(100) NOT NULL,       -- Seller / Farmer name
        Location NVARCHAR(100) NOT NULL,         -- Location of the farm / market
        Grade NVARCHAR(20) DEFAULT 'Grade A',
        IsApproved BIT DEFAULT 1,
        CreatedDate DATETIME DEFAULT GETDATE()
    );
END
GO

-- 3. FarmPlots Table: Stores farmer land plot registry and GPS data (Page 9)
IF OBJECT_ID('FarmPlots', 'U') IS NULL
BEGIN
    CREATE TABLE FarmPlots (
        PlotId INT PRIMARY KEY IDENTITY(1,1),
        PlotName NVARCHAR(50) NOT NULL,  -- e.g. 'Plot A', 'Plot B'
        AreaAcres DECIMAL(6,2) NOT NULL, -- Land area in acres
        Crop NVARCHAR(50) NOT NULL,      -- Crop currently sown
        Stage NVARCHAR(50) NOT NULL,     -- Growth stage (e.g. 'Flowering', 'Boll Dev')
        Status NVARCHAR(30) DEFAULT 'Healthy',
        SoilType NVARCHAR(50) DEFAULT 'Clayey Loam'
    );
END
GO

-- 4. ApmcRates Table: Real-time agricultural prices across Gujarat yards (Page 12 & 13)
IF OBJECT_ID('ApmcRates', 'U') IS NULL
BEGIN
    CREATE TABLE ApmcRates (
        RateId INT PRIMARY KEY IDENTITY(1,1),
        Commodity NVARCHAR(50) NOT NULL,  -- e.g. 'Groundnut', 'Cotton', 'Wheat'
        MarketYard NVARCHAR(100) NOT NULL,-- e.g. 'Rajkot APMC', 'Gondal APMC'
        MinPrice DECIMAL(10,2) NOT NULL,  -- Minimum rate per quintal
        MaxPrice DECIMAL(10,2) NOT NULL,  -- Maximum rate per quintal
        AvgPrice DECIMAL(10,2) NOT NULL,  -- Average market price
        ArrivalTons DECIMAL(10,2) DEFAULT 100,
        UpdatedDate DATETIME DEFAULT GETDATE()
    );
END
GO

-- 5. Orders Table: Buyer direct purchase orders and escrow payment status (Page 16, 17, 18)
IF OBJECT_ID('Orders', 'U') IS NULL
BEGIN
    CREATE TABLE Orders (
        OrderId INT PRIMARY KEY IDENTITY(10400, 1),
        BuyerName NVARCHAR(100) NOT NULL,
        FarmerName NVARCHAR(100) NOT NULL,
        Commodity NVARCHAR(100) NOT NULL,
        Quantity NVARCHAR(50) NOT NULL,
        TotalPrice DECIMAL(12,2) NOT NULL,
        OrderDate DATETIME DEFAULT GETDATE(),
        Status NVARCHAR(50) DEFAULT 'Processing', -- 'Delivered', 'In Transit', 'Processing', 'Cancelled'
        DeliveryAddress NVARCHAR(200) NULL
    );
END
GO

-- 6. Equipments Table: Tractor and farm machinery rental listings (Page 21, 23, 24)
IF OBJECT_ID('Equipments', 'U') IS NULL
BEGIN
    CREATE TABLE Equipments (
        EquipmentId INT PRIMARY KEY IDENTITY(1,1),
        ModelName NVARCHAR(100) NOT NULL, -- e.g. 'Massey Ferguson 241DI'
        Horsepower NVARCHAR(20) NOT NULL, -- e.g. '41 HP'
        HourlyRate DECIMAL(10,2) NOT NULL,-- Hourly rental charge in INR
        OwnerName NVARCHAR(100) NOT NULL,  -- Machinery owner name
        Location NVARCHAR(100) NOT NULL,
        Rating DECIMAL(3,1) DEFAULT 4.8
    );
END
GO

-- 7. Bookings Table: Farm service requests and booking logs (Page 22, 23, 24)
IF OBJECT_ID('Bookings', 'U') IS NULL
BEGIN
    CREATE TABLE Bookings (
        BookingId NVARCHAR(30) PRIMARY KEY, -- e.g. '#KR-BK-408'
        ServiceType NVARCHAR(100) NOT NULL, -- e.g. 'Soil Lab Test Visit'
        ProviderName NVARCHAR(100) NOT NULL,
        CustomerName NVARCHAR(100) NOT NULL,
        BookingDate DATETIME DEFAULT GETDATE(),
        Amount DECIMAL(10,2) NOT NULL,
        Status NVARCHAR(50) DEFAULT 'Confirmed' -- 'Confirmed', 'Completed', 'Cancelled'
    );
END
GO

-- 8. LandListings Table: Farmland buy & sell property listings (Page 20)
IF OBJECT_ID('LandListings', 'U') IS NULL
BEGIN
    CREATE TABLE LandListings (
        LandId INT PRIMARY KEY IDENTITY(1,1),
        Title NVARCHAR(150) NOT NULL,
        AreaAcres DECIMAL(6,2) NOT NULL,
        Location NVARCHAR(100) NOT NULL,
        WaterSource NVARCHAR(50) NOT NULL,
        Price NVARCHAR(50) NOT NULL,
        SoilType NVARCHAR(50) NOT NULL
    );
END
GO

-- 9. ContactInquiries Table: Customer support inquiries and messages (Page 32)
IF OBJECT_ID('ContactInquiries', 'U') IS NULL
BEGIN
    CREATE TABLE ContactInquiries (
        InquiryId INT PRIMARY KEY IDENTITY(1,1),
        FullName NVARCHAR(100) NOT NULL,
        MobileNo NVARCHAR(20) NOT NULL,
        Message NVARCHAR(MAX) NOT NULL,
        SubmittedDate DATETIME DEFAULT GETDATE()
    );
END
GO
