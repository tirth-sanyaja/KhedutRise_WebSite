using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace KhedutRise
{
    // Helper class to handle common database operations using ADO.NET
    // Features self-healing auto-provisioning: If KhedutRiseDB or its tables do not exist
    // on a new machine / localdb instance (SqlException 4060 or 208), it creates the database
    // and tables automatically without manual setup.
    public static class DbHelper
    {
        private static readonly object _initLock = new object();
        private static bool _initialized = false;

        // Get the active connection string from Web.config
        public static string GetConnectionString()
        {
            if (ConfigurationManager.ConnectionStrings["KhedutRiseDB"] != null)
            {
                return ConfigurationManager.ConnectionStrings["KhedutRiseDB"].ConnectionString;
            }
            // Fallback connection to LocalDB
            return @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=KhedutRiseDB;Integrated Security=True;";
        }

        // Execute a SELECT query and return results as a DataTable (used for GridViews and Repeaters)
        public static DataTable ExecuteQuery(string query, SqlParameter[] parameters = null)
        {
            try
            {
                return ExecuteQueryInternal(query, parameters);
            }
            catch (SqlException ex)
            {
                if (ex.Number == 4060 || ex.Number == 208)
                {
                    // Error 4060: Cannot open database (missing on this computer)
                    // Error 208: Invalid object name (missing table)
                    EnsureDatabase(true);
                    return ExecuteQueryInternal(query, parameters);
                }
                throw;
            }
        }

        private static DataTable ExecuteQueryInternal(string query, SqlParameter[] parameters)
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(CloneParameters(parameters));
                    }
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(dt);
                    }
                }
            }
            return dt;
        }

        // Execute an INSERT, UPDATE, or DELETE query and return the number of affected rows
        public static int ExecuteNonQuery(string query, SqlParameter[] parameters = null)
        {
            try
            {
                return ExecuteNonQueryInternal(query, parameters);
            }
            catch (SqlException ex)
            {
                if (ex.Number == 4060 || ex.Number == 208)
                {
                    EnsureDatabase(true);
                    return ExecuteNonQueryInternal(query, parameters);
                }
                throw;
            }
        }

        private static int ExecuteNonQueryInternal(string query, SqlParameter[] parameters)
        {
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(CloneParameters(parameters));
                    }
                    con.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        // Execute a query and return a single scalar value
        public static object ExecuteScalar(string query, SqlParameter[] parameters = null)
        {
            try
            {
                return ExecuteScalarInternal(query, parameters);
            }
            catch (SqlException ex)
            {
                if (ex.Number == 4060 || ex.Number == 208)
                {
                    EnsureDatabase(true);
                    return ExecuteScalarInternal(query, parameters);
                }
                throw;
            }
        }

        private static object ExecuteScalarInternal(string query, SqlParameter[] parameters)
        {
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(CloneParameters(parameters));
                    }
                    con.Open();
                    return cmd.ExecuteScalar();
                }
            }
        }

        // Execute a multi-operation database transaction using C# Action delegate
        public static bool ExecuteTransaction(Action<SqlConnection, SqlTransaction> dbOperations)
        {
            try
            {
                return ExecuteTransactionInternal(dbOperations);
            }
            catch (SqlException ex)
            {
                if (ex.Number == 4060 || ex.Number == 208)
                {
                    EnsureDatabase(true);
                    return ExecuteTransactionInternal(dbOperations);
                }
                throw;
            }
        }

        private static bool ExecuteTransactionInternal(Action<SqlConnection, SqlTransaction> dbOperations)
        {
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                con.Open();
                using (SqlTransaction tran = con.BeginTransaction())
                {
                    try
                    {
                        dbOperations(con, tran);
                        tran.Commit();
                        return true;
                    }
                    catch
                    {
                        tran.Rollback();
                        throw;
                    }
                }
            }
        }

        // Self-healing database auto-provisioner
        public static void EnsureDatabase(bool force = false)
        {
            if (_initialized && !force) return;
            lock (_initLock)
            {
                if (_initialized && !force) return;
                try
                {
                    SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(GetConnectionString());
                    string targetDb = string.IsNullOrEmpty(builder.InitialCatalog) ? "KhedutRiseDB" : builder.InitialCatalog;

                    // 1. Connect to 'master' on the target SQL Server / LocalDB instance
                    SqlConnectionStringBuilder masterBuilder = new SqlConnectionStringBuilder(builder.ConnectionString);
                    masterBuilder.InitialCatalog = "master";

                    using (SqlConnection mCon = new SqlConnection(masterBuilder.ConnectionString))
                    {
                        mCon.Open();

                        // Check if target database exists
                        using (SqlCommand checkCmd = new SqlCommand("SELECT db_id(@DbName)", mCon))
                        {
                            checkCmd.Parameters.AddWithValue("@DbName", targetDb);
                            object dbId = checkCmd.ExecuteScalar();

                            if (dbId == null || dbId == DBNull.Value)
                            {
                                // Create database if missing on this computer
                                using (SqlCommand createCmd = new SqlCommand(string.Format("CREATE DATABASE [{0}]", targetDb), mCon))
                                {
                                    createCmd.ExecuteNonQuery();
                                }
                            }
                        }
                    }

                    // 2. Provision essential tables and schema if missing
                    ProvisionSchemaTables(builder.ConnectionString);

                    _initialized = true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.TraceWarning("[DbHelper.EnsureDatabase] " + ex.Message);
                }
            }
        }

        private static void ProvisionSchemaTables(string connStr)
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();

                // 1. Users Table
                string userSql = @"
IF OBJECT_ID('Users', 'U') IS NULL
BEGIN
    CREATE TABLE Users (
        UserId INT PRIMARY KEY IDENTITY(1001, 1),
        FullName NVARCHAR(100) NOT NULL,
        MobileNo NVARCHAR(20) NOT NULL CONSTRAINT UQ_Users_Mobile UNIQUE,
        Email NVARCHAR(100) NULL,
        UserRole NVARCHAR(20) NOT NULL,
        Location NVARCHAR(100) NULL,
        SpecificField NVARCHAR(200) NULL,
        Status NVARCHAR(20) NOT NULL CONSTRAINT DF_Users_Status DEFAULT 'Verified',
        CreatedDate DATETIME NOT NULL CONSTRAINT DF_Users_CreatedDate DEFAULT GETDATE(),
        PasswordHash NVARCHAR(256) NULL
    );
END
ELSE
BEGIN
    IF COL_LENGTH('Users', 'PasswordHash') IS NULL
    BEGIN
        ALTER TABLE Users ADD PasswordHash NVARCHAR(256) NULL;
    END
END";
                ExecuteSimpleCommand(con, userSql);

                // 2. UserOTPs Table
                string otpSql = @"
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
        IPAddress NVARCHAR(50) NULL,
        Email NVARCHAR(255) NULL
    );
    CREATE INDEX IX_UserOTPs_Mobile_Purpose ON UserOTPs(MobileNo, OTPPurpose, IsUsed);
    CREATE INDEX IX_UserOTPs_CreatedAt ON UserOTPs(OTPCreatedAt);
END
ELSE
BEGIN
    IF COL_LENGTH('UserOTPs', 'Email') IS NULL
    BEGIN
        ALTER TABLE UserOTPs ADD Email NVARCHAR(255) NULL;
    END
    IF COL_LENGTH('UserOTPs', 'GatewayResponse') IS NULL
    BEGIN
        ALTER TABLE UserOTPs ADD GatewayResponse NVARCHAR(500) NULL;
    END
END";
                ExecuteSimpleCommand(con, otpSql);

                // 3. Products Table
                string prodSql = @"
IF OBJECT_ID('Products', 'U') IS NULL
BEGIN
    CREATE TABLE Products (
        ProductId INT PRIMARY KEY IDENTITY(1, 1),
        FarmerId INT NULL CONSTRAINT FK_Products_Users REFERENCES Users(UserId),
        ProductName NVARCHAR(100) NOT NULL,
        Category NVARCHAR(50) NOT NULL,
        QuantityInQuintal DECIMAL(10, 2) NOT NULL,
        PricePerQuintal DECIMAL(10, 2) NOT NULL,
        FarmerName NVARCHAR(100) NOT NULL,
        Location NVARCHAR(100) NOT NULL,
        Grade NVARCHAR(20) CONSTRAINT DF_Products_Grade DEFAULT 'Grade A',
        IsApproved BIT CONSTRAINT DF_Products_IsApproved DEFAULT 1,
        CreatedDate DATETIME CONSTRAINT DF_Products_CreatedDate DEFAULT GETDATE(),
        ImagePath NVARCHAR(300) NULL CONSTRAINT DF_Products_ImagePath DEFAULT 'Images/groundnut.jpg'
    );
END";
                ExecuteSimpleCommand(con, prodSql);

                // 4. FarmPlots Table
                string plotsSql = @"
IF OBJECT_ID('FarmPlots', 'U') IS NULL
BEGIN
    CREATE TABLE FarmPlots (
        PlotId INT PRIMARY KEY IDENTITY(1, 1),
        FarmerId INT NULL CONSTRAINT FK_FarmPlots_Users REFERENCES Users(UserId),
        PlotName NVARCHAR(50) NOT NULL,
        AreaAcres DECIMAL(6, 2) NOT NULL,
        Crop NVARCHAR(50) NOT NULL,
        Stage NVARCHAR(50) NOT NULL,
        Status NVARCHAR(30) CONSTRAINT DF_FarmPlots_Status DEFAULT 'Healthy',
        SoilType NVARCHAR(50) CONSTRAINT DF_FarmPlots_SoilType DEFAULT 'Clayey Loam'
    );
END";
                ExecuteSimpleCommand(con, plotsSql);

                // 5. ApmcRates Table
                string ratesSql = @"
IF OBJECT_ID('ApmcRates', 'U') IS NULL
BEGIN
    CREATE TABLE ApmcRates (
        RateId INT PRIMARY KEY IDENTITY(1, 1),
        Commodity NVARCHAR(50) NOT NULL,
        MarketYard NVARCHAR(100) NOT NULL,
        MinPrice DECIMAL(10, 2) NOT NULL,
        MaxPrice DECIMAL(10, 2) NOT NULL,
        AvgPrice DECIMAL(10, 2) NOT NULL,
        ArrivalTons DECIMAL(10, 2) CONSTRAINT DF_ApmcRates_ArrivalTons DEFAULT 100,
        UpdatedDate DATETIME CONSTRAINT DF_ApmcRates_UpdatedDate DEFAULT GETDATE()
    );
END";
                ExecuteSimpleCommand(con, ratesSql);

                // 6. Orders Table
                string ordersSql = @"
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
        Status NVARCHAR(50) CONSTRAINT DF_Orders_Status DEFAULT 'Processing',
        DeliveryAddress NVARCHAR(200) NULL
    );
END";
                ExecuteSimpleCommand(con, ordersSql);

                // 7. Equipments Table
                string equipSql = @"
IF OBJECT_ID('Equipments', 'U') IS NULL
BEGIN
    CREATE TABLE Equipments (
        EquipmentId INT PRIMARY KEY IDENTITY(1, 1),
        OwnerId INT NULL CONSTRAINT FK_Equipments_Users REFERENCES Users(UserId),
        ModelName NVARCHAR(100) NOT NULL,
        Horsepower NVARCHAR(20) NOT NULL,
        HourlyRate DECIMAL(10, 2) NOT NULL,
        OwnerName NVARCHAR(100) NOT NULL,
        Location NVARCHAR(100) NOT NULL,
        Rating DECIMAL(3, 1) CONSTRAINT DF_Equipments_Rating DEFAULT 4.8,
        IsAvailable BIT CONSTRAINT DF_Equipments_IsAvailable DEFAULT 1,
        ImagePath NVARCHAR(300) NULL,
        Attachments NVARCHAR(500) NULL,
        Description NVARCHAR(300) NULL
    );
END";
                ExecuteSimpleCommand(con, equipSql);

                // 8. Bookings Table
                string bookingsSql = @"
IF OBJECT_ID('Bookings', 'U') IS NULL
BEGIN
    CREATE TABLE Bookings (
        BookingId NVARCHAR(30) PRIMARY KEY,
        UserId INT NULL CONSTRAINT FK_Bookings_Users REFERENCES Users(UserId),
        ServiceType NVARCHAR(100) NOT NULL,
        ProviderName NVARCHAR(100) NOT NULL,
        CustomerName NVARCHAR(100) NOT NULL,
        BookingDate DATETIME CONSTRAINT DF_Bookings_BookingDate DEFAULT GETDATE(),
        Amount DECIMAL(10, 2) NOT NULL,
        Status NVARCHAR(50) CONSTRAINT DF_Bookings_Status DEFAULT 'Confirmed'
    );
END";
                ExecuteSimpleCommand(con, bookingsSql);

                // 9. LandListings Table
                string landSql = @"
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
END";
                ExecuteSimpleCommand(con, landSql);

                // 10. ContactInquiries Table
                string inquirySql = @"
IF OBJECT_ID('ContactInquiries', 'U') IS NULL
BEGIN
    CREATE TABLE ContactInquiries (
        InquiryId INT PRIMARY KEY IDENTITY(1, 1),
        FullName NVARCHAR(100) NOT NULL,
        MobileNo NVARCHAR(20) NOT NULL,
        Message NVARCHAR(MAX) NOT NULL,
        SubmittedDate DATETIME CONSTRAINT DF_ContactInquiries_SubmittedDate DEFAULT GETDATE()
    );
END";
                ExecuteSimpleCommand(con, inquirySql);

                // 11. CommunityPosts Table
                string commSql = @"
IF OBJECT_ID('CommunityPosts', 'U') IS NULL
BEGIN
    CREATE TABLE CommunityPosts (
        PostId INT PRIMARY KEY IDENTITY(1, 1),
        AuthorName NVARCHAR(150) NOT NULL,
        AuthorRole NVARCHAR(50) NULL,
        Category NVARCHAR(100) NOT NULL,
        Title NVARCHAR(300) NOT NULL,
        Details NVARCHAR(MAX) NOT NULL,
        RepliesCount INT NULL CONSTRAINT DF_CommunityPosts_RepliesCount DEFAULT 0,
        CreatedDate DATETIME NULL CONSTRAINT DF_CommunityPosts_CreatedDate DEFAULT GETDATE()
    );
END";
                ExecuteSimpleCommand(con, commSql);

                // 12. Wishlists Table
                string wishSql = @"
IF OBJECT_ID('Wishlists', 'U') IS NULL
BEGIN
    CREATE TABLE Wishlists (
        WishlistId INT PRIMARY KEY IDENTITY(1, 1),
        UserId INT NULL CONSTRAINT FK_Wishlists_Users REFERENCES Users(UserId),
        ProductId INT NOT NULL CONSTRAINT FK_Wishlists_Products REFERENCES Products(ProductId),
        AddedDate DATETIME NULL CONSTRAINT DF_Wishlists_AddedDate DEFAULT GETDATE()
    );
END";
                ExecuteSimpleCommand(con, wishSql);

                // Seed Data: Admin User
                string adminSql = @"
IF NOT EXISTS (SELECT 1 FROM Users WHERE UserRole = 'Admin' OR Email = 'admin@khedutrise.com')
BEGIN
    INSERT INTO Users (FullName, MobileNo, Email, UserRole, Location, SpecificField, Status, CreatedDate, PasswordHash)
    VALUES ('System Admin', '9876500001', 'admin@khedutrise.com', 'Admin', 'Gandhinagar, Gujarat', 'Platform Superuser', 'Verified', GETDATE(), 'admin123');
END";
                ExecuteSimpleCommand(con, adminSql);

                // Seed Data: Products
                string seedProducts = @"
IF NOT EXISTS (SELECT 1 FROM Products)
BEGIN
    INSERT INTO Products (ProductName, Category, QuantityInQuintal, PricePerQuintal, FarmerName, Location, Grade, IsApproved, CreatedDate, ImagePath)
    VALUES 
    ('Premium Groundnut (GG-20)', 'Groundnut', 45.00, 6800.00, 'Mansukhbhai Patel', 'Rajkot Rural', 'Grade A', 1, GETDATE(), 'Images/groundnut.jpg'),
    ('Organic Sharbati Wheat', 'Wheat', 60.00, 2900.00, 'Hareshbhai Mer', 'Junagadh', 'Grade A+', 1, GETDATE(), 'Images/wheat.jpg'),
    ('Bt Cotton (Shankar-6)', 'Cotton', 30.00, 7200.00, 'Dineshbhai Solanki', 'Amreli', 'Grade A', 1, GETDATE(), 'Images/cotton.jpg'),
    ('Fresh Desi Tomatoes (Grade A)', 'Vegetables', 35.00, 1850.00, 'Ramesh Patel', 'Rajkot Rural', 'Grade A', 1, GETDATE(), 'Images/tomato.jpg'),
    ('Mahuva Red Onions (Premium)', 'Vegetables', 50.00, 2150.00, 'Savjibhai Patel', 'Gondal Sub-district', 'Grade A+', 1, GETDATE(), 'Images/onion.jpg');
END";
                ExecuteSimpleCommand(con, seedProducts);

                // Seed Data: Equipments
                string seedEquip = @"
IF NOT EXISTS (SELECT 1 FROM Equipments)
BEGIN
    INSERT INTO Equipments (ModelName, Horsepower, HourlyRate, OwnerName, Location, Rating, IsAvailable, ImagePath, Attachments, Description)
    VALUES
    ('Massey Ferguson 241DI', '41 HP', 800.00, 'Mansukhbhai Patel', 'Rajkot, Gujarat', 4.8, 1, 'Images/massey_ferguson_241di.jpg', 'Rotavator, Disc Harrow, Cultivator', 'Ideal for cotton and groundnut farms. Reliable 2WD tractor with powerful PTO.'),
    ('Mahindra Yuvo 575', '45 HP', 750.00, 'Hareshbhai Mer', 'Junagadh, Gujarat', 4.7, 1, 'Images/mahindra_yuvo_575.jpg', 'Seed Drill, MB Plough, Boom Sprayer', 'Modern 2WD tractor suited for wheat and vegetable farming. Excellent fuel efficiency.'),
    ('Swaraj 744 FE', '48 HP', 780.00, 'Dineshbhai Solanki', 'Anand, Gujarat', 4.9, 1, 'Images/swaraj_744fe.jpg', 'Rotavator, Laser Leveler, Seed Drill', 'Heavy duty 4WD tractor with higher ground clearance. Best for large farms and paddy fields.');
END";
                ExecuteSimpleCommand(con, seedEquip);

                // Seed Data: APMC Rates
                string seedRates = @"
IF NOT EXISTS (SELECT 1 FROM ApmcRates)
BEGIN
    INSERT INTO ApmcRates (Commodity, MarketYard, MinPrice, MaxPrice, AvgPrice, ArrivalTons, UpdatedDate)
    VALUES
    ('Groundnut', 'Rajkot APMC', 6200.00, 7150.00, 6750.00, 320.00, GETDATE()),
    ('Cotton', 'Rajkot APMC', 6800.00, 7450.00, 7150.00, 480.00, GETDATE()),
    ('Wheat', 'Gondal APMC', 2600.00, 3100.00, 2850.00, 210.00, GETDATE()),
    ('Cumin (Jeera)', 'Unjha APMC', 24000.00, 29500.00, 27000.00, 150.00, GETDATE());
END";
                ExecuteSimpleCommand(con, seedRates);

                // Seed Data: FarmPlots
                string seedPlots = @"
IF NOT EXISTS (SELECT 1 FROM FarmPlots)
BEGIN
    INSERT INTO FarmPlots (PlotName, AreaAcres, Crop, Stage, Status, SoilType)
    VALUES
    ('Plot A - North Field', 4.5, 'Groundnut GG-20', 'Pod Formation', 'Healthy', 'Medium Black'),
    ('Plot B - River Bank', 6.0, 'Bt Cotton Shankar-6', 'Boll Development', 'Healthy', 'Clayey Loam'),
    ('Plot C - Well Side', 3.2, 'Sharbati Wheat', 'Tillering', 'Healthy', 'Alluvial Loam');
END";
                ExecuteSimpleCommand(con, seedPlots);

                // Seed Data: LandListings
                string seedLand = @"
IF NOT EXISTS (SELECT 1 FROM LandListings)
BEGIN
    INSERT INTO LandListings (Title, AreaAcres, Location, WaterSource, Price, SoilType, Status)
    VALUES
    ('Fertile Black Soil Agricultural Land', 8.5, 'Gondal, Rajkot', 'Borewell + Canal', '65 Lakh / Acre', 'Deep Black Soil', 'Approved'),
    ('Irrigated Farmland near Highway', 5.0, 'Jasdan, Rajkot', '2 Open Wells', '48 Lakh / Acre', 'Medium Black', 'Approved');
END";
                ExecuteSimpleCommand(con, seedLand);

                // Seed Data: CommunityPosts
                string seedComm = @"
IF NOT EXISTS (SELECT 1 FROM CommunityPosts)
BEGIN
    INSERT INTO CommunityPosts (AuthorName, AuthorRole, Category, Title, Details, RepliesCount, CreatedDate)
    VALUES
    ('Rameshbhai Patel', 'Farmer', 'Pest Control', 'How to prevent white grub in groundnut crops?', 'Observed yellowing of leaves in plot A. Looking for organic bio-pesticide recommendations before harvest.', 4, GETDATE()),
    ('Dr. Kishorbhai Joshi', 'Agronomist', 'Crop Advisory', 'Optimal irrigation schedule during winter wheat tillering', 'First irrigation should be 20-25 days after sowing at CRI (Crown Root Initiation) stage for maximum yield.', 7, GETDATE());
END";
                ExecuteSimpleCommand(con, seedComm);
            }
        }

        private static void ExecuteSimpleCommand(SqlConnection con, string sql)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceWarning("[DbHelper.ExecuteSimpleCommand] " + ex.Message);
            }
        }

        private static SqlParameter[] CloneParameters(SqlParameter[] source)
        {
            if (source == null) return null;
            SqlParameter[] clones = new SqlParameter[source.Length];
            for (int i = 0; i < source.Length; i++)
            {
                clones[i] = (SqlParameter)((ICloneable)source[i]).Clone();
            }
            return clones;
        }
    }
}
