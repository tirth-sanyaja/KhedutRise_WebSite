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
                    EnsureDatabase();
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
                    EnsureDatabase();
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
                    EnsureDatabase();
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
                    EnsureDatabase();
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
        public static void EnsureDatabase()
        {
            if (_initialized) return;
            lock (_initLock)
            {
                if (_initialized) return;
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

                // 4. Default System Administrator seed
                string adminSql = @"
IF NOT EXISTS (SELECT 1 FROM Users WHERE UserRole = 'Admin' OR Email = 'admin@khedutrise.com')
BEGIN
    INSERT INTO Users (FullName, MobileNo, Email, UserRole, Location, SpecificField, Status, CreatedDate)
    VALUES ('System Admin', '9876500001', 'admin@khedutrise.com', 'Admin', 'Gandhinagar, Gujarat', 'Platform Superuser', 'Verified', GETDATE());
END";
                ExecuteSimpleCommand(con, adminSql);
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
