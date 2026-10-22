using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace KhedutRise
{
    // =========================================================================
    // 1. USER REPOSITORY
    // =========================================================================
    public static class UserRepository
    {
        // Authenticate user via Mobile number (and password / OTP)
        public static User AuthenticateUser(string mobile, string passwordOrOtp)
        {
            string sql = @"SELECT TOP 1 UserId, FullName, MobileNo, Email, UserRole, Location, SpecificField, Status, CreatedDate, PasswordHash 
                           FROM Users 
                           WHERE MobileNo = @Mob";
            SqlParameter[] prms = new SqlParameter[] { new SqlParameter("@Mob", mobile) };
            DataTable dt = DbHelper.ExecuteQuery(sql, prms);

            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow r = dt.Rows[0];
                string storedHash = r["PasswordHash"] != DBNull.Value ? r["PasswordHash"].ToString() : "";
                
                // Validate password/OTP
                if (SecurityHelper.VerifyPassword(passwordOrOtp, storedHash))
                {
                    return MapRowToUser(r);
                }
            }
            return null;
        }

        // Retrieve user by primary key
        public static User GetUserById(int userId)
        {
            string sql = "SELECT * FROM Users WHERE UserId = @Id";
            SqlParameter[] prms = new SqlParameter[] { new SqlParameter("@Id", userId) };
            DataTable dt = DbHelper.ExecuteQuery(sql, prms);
            if (dt != null && dt.Rows.Count > 0)
            {
                return MapRowToUser(dt.Rows[0]);
            }
            return null;
        }

        // Register a new user
        public static int RegisterUser(string fullName, string mobile, string email, string role, string location, string specificField, string password)
        {
            string hash = SecurityHelper.HashPassword(password);
            string sql = @"INSERT INTO Users (FullName, MobileNo, Email, UserRole, Location, SpecificField, Status, PasswordHash, CreatedDate)
                           OUTPUT INSERTED.UserId
                           VALUES (@Name, @Mob, @Email, @Role, @Loc, @Field, 'Verified', @Hash, GETDATE())";
            SqlParameter[] prms = new SqlParameter[]
            {
                new SqlParameter("@Name", fullName),
                new SqlParameter("@Mob", mobile),
                new SqlParameter("@Email", (object)email ?? DBNull.Value),
                new SqlParameter("@Role", role),
                new SqlParameter("@Loc", (object)location ?? "Gujarat"),
                new SqlParameter("@Field", (object)specificField ?? string.Empty),
                new SqlParameter("@Hash", hash)
            };
            object res = DbHelper.ExecuteScalar(sql, prms);
            return res != null ? Convert.ToInt32(res) : 0;
        }

        // Update profile
        public static bool UpdateProfile(int userId, string fullName, string email, string location, string specificField)
        {
            string sql = @"UPDATE Users 
                           SET FullName = @Name, Email = @Email, Location = @Loc, SpecificField = @Field 
                           WHERE UserId = @Id";
            SqlParameter[] prms = new SqlParameter[]
            {
                new SqlParameter("@Name", fullName),
                new SqlParameter("@Email", (object)email ?? DBNull.Value),
                new SqlParameter("@Loc", (object)location ?? DBNull.Value),
                new SqlParameter("@Field", (object)specificField ?? DBNull.Value),
                new SqlParameter("@Id", userId)
            };
            return DbHelper.ExecuteNonQuery(sql, prms) > 0;
        }

        // Change password securely with salted hash verification
        public static bool ChangePassword(int userId, string currentPassword, string newPassword, out string errorMessage)
        {
            errorMessage = "";
            string selectSql = "SELECT PasswordHash FROM Users WHERE UserId = @Id";
            SqlParameter[] selPrms = new SqlParameter[] { new SqlParameter("@Id", userId) };
            DataTable dt = DbHelper.ExecuteQuery(selectSql, selPrms);
            if (dt == null || dt.Rows.Count == 0)
            {
                errorMessage = "User account not found.";
                return false;
            }

            string storedHash = dt.Rows[0]["PasswordHash"] != DBNull.Value ? dt.Rows[0]["PasswordHash"].ToString() : "";
            if (!string.IsNullOrEmpty(storedHash) && !SecurityHelper.VerifyPassword(currentPassword, storedHash))
            {
                errorMessage = "Current password is incorrect.";
                return false;
            }

            string newHash = SecurityHelper.HashPassword(newPassword);
            string updateSql = "UPDATE Users SET PasswordHash = @Hash WHERE UserId = @Id";
            SqlParameter[] updPrms = new SqlParameter[]
            {
                new SqlParameter("@Hash", newHash),
                new SqlParameter("@Id", userId)
            };
            return DbHelper.ExecuteNonQuery(updateSql, updPrms) > 0;
        }

        private static User MapRowToUser(DataRow r)
        {
            return new User
            {
                UserId = Convert.ToInt32(r["UserId"]),
                FullName = r["FullName"].ToString(),
                MobileNo = r["MobileNo"].ToString(),
                Email = r["Email"] != DBNull.Value ? r["Email"].ToString() : "",
                UserRole = r["UserRole"].ToString(),
                Location = r["Location"] != DBNull.Value ? r["Location"].ToString() : "",
                SpecificField = r["SpecificField"] != DBNull.Value ? r["SpecificField"].ToString() : "",
                Status = r["Status"] != DBNull.Value ? r["Status"].ToString() : "Verified",
                CreatedDate = r["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(r["CreatedDate"]) : DateTime.Now
            };
        }
    }

    // =========================================================================
    // 2. PRODUCT REPOSITORY
    // =========================================================================
    public static class ProductRepository
    {
        // Get all approved products for marketplace via SQL LEFT JOIN on Users
        public static DataTable GetMarketplaceProducts(string category = "", string search = "", string sort = "")
        {
            string sql = @"SELECT p.ProductId, p.ProductName, p.Category, p.QuantityInQuintal, p.PricePerQuintal, 
                                  p.Grade, p.IsApproved, p.CreatedDate, p.FarmerId,
                                  COALESCE(NULLIF(p.ImagePath, ''), 
                                      CASE 
                                          WHEN p.Category = 'Cotton' THEN 'Images/cotton.jpg'
                                          WHEN p.Category = 'Wheat' THEN 'Images/wheat.jpg'
                                          WHEN p.Category = 'Vegetables' THEN 'Images/tomato.jpg'
                                          WHEN p.Category = 'Spices' THEN 'Images/cumin_spices.jpg'
                                          ELSE 'Images/groundnut.jpg'
                                      END) AS ImagePath,
                                  ISNULL(u.FullName, p.FarmerName) AS FarmerName, 
                                  ISNULL(u.Location, p.Location) AS Location, 
                                  u.MobileNo AS FarmerContact 
                           FROM Products p 
                           LEFT JOIN Users u ON p.FarmerId = u.UserId 
                           WHERE p.IsApproved = 1 AND p.QuantityInQuintal > 0";

            List<SqlParameter> prms = new List<SqlParameter>();

            if (!string.IsNullOrEmpty(category))
            {
                sql += " AND p.Category = @Category";
                prms.Add(new SqlParameter("@Category", category));
            }

            if (!string.IsNullOrEmpty(search))
            {
                sql += " AND (p.ProductName LIKE @Search OR p.Category LIKE @Search OR u.Location LIKE @Search)";
                prms.Add(new SqlParameter("@Search", "%" + search + "%"));
            }

            if (sort == "price_asc") sql += " ORDER BY p.PricePerQuintal ASC";
            else if (sort == "price_desc") sql += " ORDER BY p.PricePerQuintal DESC";
            else sql += " ORDER BY p.ProductId DESC";

            return DbHelper.ExecuteQuery(sql, prms.ToArray());
        }

        // Get single product details via SQL LEFT JOIN
        public static DataTable GetProductDetailsById(int productId)
        {
            string sql = @"SELECT p.ProductId, p.ProductName, p.Category, p.QuantityInQuintal, p.PricePerQuintal, 
                                  p.Grade, p.IsApproved, p.CreatedDate, p.FarmerId,
                                  COALESCE(NULLIF(p.ImagePath, ''), 
                                      CASE 
                                          WHEN p.Category = 'Cotton' THEN 'Images/cotton.jpg'
                                          WHEN p.Category = 'Wheat' THEN 'Images/wheat.jpg'
                                          WHEN p.Category = 'Vegetables' THEN 'Images/tomato.jpg'
                                          WHEN p.Category = 'Spices' THEN 'Images/cumin_spices.jpg'
                                          ELSE 'Images/groundnut.jpg'
                                      END) AS ImagePath,
                                  ISNULL(u.FullName, p.FarmerName) AS FarmerName, 
                                  ISNULL(u.Location, p.Location) AS Location, 
                                  u.MobileNo AS FarmerContact,
                                  u.Email AS FarmerEmail
                           FROM Products p 
                           LEFT JOIN Users u ON p.FarmerId = u.UserId 
                           WHERE p.ProductId = @Id";
            SqlParameter[] prms = new SqlParameter[] { new SqlParameter("@Id", productId) };
            return DbHelper.ExecuteQuery(sql, prms);
        }

        // Insert new harvest listing by authenticated farmer
        public static int AddProduct(int farmerId, string farmerName, string name, string category, decimal qty, decimal price, string location, string grade, string imagePath)
        {
            string sql = @"INSERT INTO Products (FarmerId, ProductName, Category, QuantityInQuintal, PricePerQuintal, FarmerName, Location, Grade, IsApproved, CreatedDate, ImagePath)
                           OUTPUT INSERTED.ProductId
                           VALUES (@FarmerId, @Name, @Cat, @Qty, @Price, @FarmerName, @Loc, @Grade, 1, GETDATE(), @Img)";
            SqlParameter[] prms = new SqlParameter[]
            {
                new SqlParameter("@FarmerId", farmerId),
                new SqlParameter("@Name", name),
                new SqlParameter("@Cat", category),
                new SqlParameter("@Qty", qty),
                new SqlParameter("@Price", price),
                new SqlParameter("@FarmerName", farmerName),
                new SqlParameter("@Loc", location),
                new SqlParameter("@Grade", (object)grade ?? "Grade A"),
                new SqlParameter("@Img", (object)imagePath ?? "Images/groundnut.jpg")
            };
            object res = DbHelper.ExecuteScalar(sql, prms);
            return res != null ? Convert.ToInt32(res) : 0;
        }

        // Get farmer's own products
        public static DataTable GetFarmerProducts(int farmerId)
        {
            string sql = "SELECT * FROM Products WHERE FarmerId = @FarmerId ORDER BY ProductId DESC";
            SqlParameter[] prms = new SqlParameter[] { new SqlParameter("@FarmerId", farmerId) };
            return DbHelper.ExecuteQuery(sql, prms);
        }
    }

    // =========================================================================
    // 3. ORDER REPOSITORY (With Atomic Concurrency-Safe Transaction)
    // =========================================================================
    public static class OrderRepository
    {
        // Concurrency-safe order placement with stock deduction transaction
        public static int PlaceOrder(int buyerId, string buyerName, int productId, decimal requestedQty, string deliveryAddress, out string errorMessage)
        {
            errorMessage = "";
            int newOrderId = 0;

            try
            {
                bool success = DbHelper.ExecuteTransaction((conn, tran) =>
                {
                    // 1. Lock and check product stock
                    string checkSql = @"SELECT ProductId, FarmerId, ProductName, PricePerQuintal, QuantityInQuintal, FarmerName 
                                        FROM Products WITH (UPDLOCK, ROWLOCK) 
                                        WHERE ProductId = @PId";
                    using (SqlCommand cmdCheck = new SqlCommand(checkSql, conn, tran))
                    {
                        cmdCheck.Parameters.AddWithValue("@PId", productId);
                        using (SqlDataReader r = cmdCheck.ExecuteReader())
                        {
                            if (!r.Read())
                            {
                                throw new Exception("Product does not exist or has been removed.");
                            }

                            decimal availableStock = Convert.ToDecimal(r["QuantityInQuintal"]);
                            if (availableStock < requestedQty)
                            {
                                throw new Exception("Insufficient stock available! Only " + availableStock + " Quintals remaining.");
                            }

                            decimal unitPrice = Convert.ToDecimal(r["PricePerQuintal"]);
                            decimal total = unitPrice * requestedQty;
                            int farmerId = r["FarmerId"] != DBNull.Value ? Convert.ToInt32(r["FarmerId"]) : 1001;
                            string productName = r["ProductName"].ToString();
                            string farmerName = r["FarmerName"].ToString();

                            r.Close();

                            // 2. Deduct inventory stock
                            string deductSql = @"UPDATE Products 
                                                SET QuantityInQuintal = QuantityInQuintal - @Qty 
                                                WHERE ProductId = @PId";
                            using (SqlCommand cmdDeduct = new SqlCommand(deductSql, conn, tran))
                            {
                                cmdDeduct.Parameters.AddWithValue("@Qty", requestedQty);
                                cmdDeduct.Parameters.AddWithValue("@PId", productId);
                                cmdDeduct.ExecuteNonQuery();
                            }

                            // 3. Insert into Orders table
                            string insertSql = @"INSERT INTO Orders (BuyerId, FarmerId, ProductId, BuyerName, FarmerName, Commodity, Quantity, TotalPrice, OrderDate, Status, DeliveryAddress)
                                                 OUTPUT INSERTED.OrderId
                                                 VALUES (@BId, @FId, @PId, @BName, @FName, @Prod, @QtyStr, @Total, GETDATE(), 'Processing', @Addr)";
                            using (SqlCommand cmdInsert = new SqlCommand(insertSql, conn, tran))
                            {
                                cmdInsert.Parameters.AddWithValue("@BId", buyerId);
                                cmdInsert.Parameters.AddWithValue("@FId", farmerId);
                                cmdInsert.Parameters.AddWithValue("@PId", productId);
                                cmdInsert.Parameters.AddWithValue("@BName", buyerName);
                                cmdInsert.Parameters.AddWithValue("@FName", farmerName);
                                cmdInsert.Parameters.AddWithValue("@Prod", productName);
                                cmdInsert.Parameters.AddWithValue("@QtyStr", requestedQty.ToString() + " Quintals");
                                cmdInsert.Parameters.AddWithValue("@Total", total);
                                cmdInsert.Parameters.AddWithValue("@Addr", (object)deliveryAddress ?? "Buyer Verified Address, Gujarat");

                                object createdId = cmdInsert.ExecuteScalar();
                                newOrderId = Convert.ToInt32(createdId);
                            }
                        }
                    }
                });

                return newOrderId;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return 0;
            }
        }

        // Get buyer's orders with SQL JOIN
        public static DataTable GetBuyerOrders(int buyerId)
        {
            string sql = @"SELECT o.OrderId, o.TotalPrice, o.OrderDate, o.Status, o.Quantity, o.DeliveryAddress,
                                  ISNULL(p.ProductName, o.Commodity) AS Commodity,
                                  COALESCE(NULLIF(p.ImagePath, ''),
                                      CASE 
                                          WHEN p.Category = 'Cotton' OR o.Commodity LIKE '%Cotton%' THEN 'Images/cotton.jpg'
                                          WHEN p.Category = 'Wheat' OR o.Commodity LIKE '%Wheat%' THEN 'Images/wheat.jpg'
                                          WHEN p.Category = 'Vegetables' OR o.Commodity LIKE '%Tomato%' THEN 'Images/tomato.jpg'
                                          WHEN o.Commodity LIKE '%Onion%' THEN 'Images/onion.jpg'
                                          WHEN o.Commodity LIKE '%Chill%' OR o.Commodity LIKE '%Okra%' THEN 'Images/green_chilli.jpg'
                                          WHEN p.Category = 'Spices' OR o.Commodity LIKE '%Cumin%' THEN 'Images/cumin_spices.jpg'
                                          ELSE 'Images/groundnut.jpg'
                                      END) AS ImagePath,
                                  ISNULL(uFarmer.FullName, o.FarmerName) AS FarmerName,
                                  ISNULL(uBuyer.FullName, o.BuyerName) AS BuyerName,
                                  uFarmer.MobileNo AS FarmerContact
                           FROM Orders o
                           LEFT JOIN Users uFarmer ON o.FarmerId = uFarmer.UserId
                           LEFT JOIN Users uBuyer ON o.BuyerId = uBuyer.UserId
                           LEFT JOIN Products p ON o.ProductId = p.ProductId
                           WHERE o.BuyerId = @BId
                           ORDER BY o.OrderId DESC";
            SqlParameter[] prms = new SqlParameter[] { new SqlParameter("@BId", buyerId) };
            return DbHelper.ExecuteQuery(sql, prms);
        }

        // Get single order for tracking
        public static DataTable GetOrderById(int orderId)
        {
            string sql = @"SELECT o.*, 
                                  ISNULL(p.ProductName, o.Commodity) AS ProductName,
                                  COALESCE(NULLIF(p.ImagePath, ''),
                                      CASE 
                                          WHEN p.Category = 'Cotton' OR o.Commodity LIKE '%Cotton%' THEN 'Images/cotton.jpg'
                                          WHEN p.Category = 'Wheat' OR o.Commodity LIKE '%Wheat%' THEN 'Images/wheat.jpg'
                                          WHEN p.Category = 'Vegetables' OR o.Commodity LIKE '%Tomato%' THEN 'Images/tomato.jpg'
                                          WHEN o.Commodity LIKE '%Onion%' THEN 'Images/onion.jpg'
                                          WHEN o.Commodity LIKE '%Chill%' OR o.Commodity LIKE '%Okra%' THEN 'Images/green_chilli.jpg'
                                          WHEN p.Category = 'Spices' OR o.Commodity LIKE '%Cumin%' THEN 'Images/cumin_spices.jpg'
                                          ELSE 'Images/groundnut.jpg'
                                      END) AS ImagePath,
                                  uFarmer.FullName AS FarmerFullName, uFarmer.MobileNo AS FarmerMobile,
                                  uBuyer.FullName AS BuyerFullName, uBuyer.MobileNo AS BuyerMobile
                           FROM Orders o
                           LEFT JOIN Users uFarmer ON o.FarmerId = uFarmer.UserId
                           LEFT JOIN Users uBuyer ON o.BuyerId = uBuyer.UserId
                           LEFT JOIN Products p ON o.ProductId = p.ProductId
                           WHERE o.OrderId = @OId";
            SqlParameter[] prms = new SqlParameter[] { new SqlParameter("@OId", orderId) };
            return DbHelper.ExecuteQuery(sql, prms);
        }
    }

    // =========================================================================
    // 4. EQUIPMENT & BOOKING REPOSITORY
    // =========================================================================
    public static class EquipmentRepository
    {
        // Get all available equipments for rental
        public static DataTable GetEquipments(string search = "")
        {
            string sql = @"SELECT e.EquipmentId, e.ModelName, e.Horsepower, e.HourlyRate, 
                                  ISNULL(u.FullName, e.OwnerName) AS OwnerName, 
                                  ISNULL(u.Location, e.Location) AS Location,
                                  e.Rating, e.IsAvailable,
                                  ISNULL(e.ImagePath, 'Images/tractor_hero.jpg') AS ImagePath
                           FROM Equipments e
                           LEFT JOIN Users u ON e.OwnerId = u.UserId
                           WHERE e.IsAvailable = 1";

            List<SqlParameter> prms = new List<SqlParameter>();
            if (!string.IsNullOrEmpty(search))
            {
                sql += " AND (e.ModelName LIKE @Search OR e.Location LIKE @Search)";
                prms.Add(new SqlParameter("@Search", "%" + search + "%"));
            }
            sql += " ORDER BY e.EquipmentId ASC";
            return DbHelper.ExecuteQuery(sql, prms.ToArray());
        }

        // Create booking for farmer
        public static string CreateBooking(int userId, int equipmentId, string serviceType, string providerName, string customerName, decimal amount)
        {
            string bookingId = string.Format("#KR-BK-{0:yyMMdd}-{1:D4}", DateTime.Now, new Random().Next(1000, 9999));
            string sql = @"INSERT INTO Bookings (BookingId, UserId, EquipmentId, ServiceType, ProviderName, CustomerName, BookingDate, Amount, Status)
                           VALUES (@BId, @UId, @EqId, @Type, @Prov, @Cust, GETDATE(), @Amt, 'Confirmed')";
            SqlParameter[] prms = new SqlParameter[]
            {
                new SqlParameter("@BId", bookingId),
                new SqlParameter("@UId", userId),
                new SqlParameter("@EqId", equipmentId),
                new SqlParameter("@Type", serviceType),
                new SqlParameter("@Prov", providerName),
                new SqlParameter("@Cust", customerName),
                new SqlParameter("@Amt", amount)
            };
            DbHelper.ExecuteNonQuery(sql, prms);
            return bookingId;
        }

        // Get single equipment by ID
        public static DataTable GetEquipmentById(int eqId)
        {
            string sql = @"SELECT e.*, 
                                  ISNULL(u.FullName, e.OwnerName) AS ResolvedOwnerName,
                                  ISNULL(u.Location, e.Location) AS ResolvedLocation,
                                  u.MobileNo AS OwnerMobile
                           FROM Equipments e
                           LEFT JOIN Users u ON e.OwnerId = u.UserId
                           WHERE e.EquipmentId = @EqId";
            SqlParameter[] prms = new SqlParameter[] { new SqlParameter("@EqId", eqId) };
            return DbHelper.ExecuteQuery(sql, prms);
        }

        // Get farmer's own bookings with SQL LEFT JOIN on Equipments
        public static DataTable GetUserBookings(int userId)
        {
            string sql = @"SELECT b.BookingId, b.ServiceType, b.ProviderName, b.CustomerName, b.BookingDate, b.Amount, b.Status,
                                  ISNULL(e.ModelName, b.ServiceType) AS EquipmentModel,
                                  ISNULL(e.HourlyRate, 850) AS HourlyRate,
                                  ISNULL(e.ImagePath, 'Images/tractor_hero.jpg') AS ImagePath
                           FROM Bookings b
                           LEFT JOIN Equipments e ON b.EquipmentId = e.EquipmentId
                           WHERE b.UserId = @UId
                           ORDER BY b.BookingDate DESC";
            SqlParameter[] prms = new SqlParameter[] { new SqlParameter("@UId", userId) };
            return DbHelper.ExecuteQuery(sql, prms);
        }
    }

    // =========================================================================
    // 5. FARM PLOT REPOSITORY
    // =========================================================================
    public static class FarmPlotRepository
    {
        // Get plots belonging strictly to authenticated farmer
        public static DataTable GetFarmerPlots(int farmerId)
        {
            string sql = @"SELECT f.*, 
                                  ISNULL(u.FullName, 'Registered Farmer') AS FarmerName,
                                  DATEDIFF(day, f.SowingDate, GETDATE()) AS DaysActive,
                                  DATEDIFF(day, GETDATE(), f.ExpectedHarvestDate) AS DaysToHarvest
                           FROM FarmPlots f
                           LEFT JOIN Users u ON f.FarmerId = u.UserId
                           WHERE f.FarmerId = @FId
                           ORDER BY f.PlotId ASC";
            SqlParameter[] prms = new SqlParameter[] { new SqlParameter("@FId", farmerId) };
            return DbHelper.ExecuteQuery(sql, prms);
        }

        // Add plot for farmer
        public static int AddPlot(int farmerId, string plotName, decimal area, string crop, string stage, string soilType)
        {
            string sql = @"INSERT INTO FarmPlots (FarmerId, PlotName, AreaAcres, Crop, Stage, Status, SoilType, SowingDate, ExpectedHarvestDate)
                           OUTPUT INSERTED.PlotId
                           VALUES (@FId, @Name, @Area, @Crop, @Stage, 'Healthy', @Soil, GETDATE(), DATEADD(day, 60, GETDATE()))";
            SqlParameter[] prms = new SqlParameter[]
            {
                new SqlParameter("@FId", farmerId),
                new SqlParameter("@Name", plotName),
                new SqlParameter("@Area", area),
                new SqlParameter("@Crop", crop),
                new SqlParameter("@Stage", stage),
                new SqlParameter("@Soil", (object)soilType ?? "Clayey Loam")
            };
            object res = DbHelper.ExecuteScalar(sql, prms);
            return res != null ? Convert.ToInt32(res) : 0;
        }
    }

    // =========================================================================
    // 6. ANALYTICS REPOSITORY (Advanced SQL with CTEs, Aggregates, & Window Functions)
    // =========================================================================
    public static class AnalyticsRepository
    {
        // Advanced Analytics using Common Table Expressions (CTE) and Aggregates
        public static DataTable GetPlatformAnalytics()
        {
            string sql = @"
            WITH UserStats AS (
                SELECT 
                    COUNT(*) AS TotalUsers,
                    SUM(CASE WHEN UserRole = 'Farmer' THEN 1 ELSE 0 END) AS FarmerCount,
                    SUM(CASE WHEN UserRole = 'Buyer' THEN 1 ELSE 0 END) AS BuyerCount,
                    SUM(CASE WHEN UserRole = 'Operator' THEN 1 ELSE 0 END) AS OperatorCount
                FROM Users
            ),
            ProductStats AS (
                SELECT 
                    COUNT(*) AS TotalProducts,
                    ISNULL(SUM(QuantityInQuintal), 0) AS TotalStockQuintal,
                    ISNULL(AVG(PricePerQuintal), 0) AS AvgCropPrice
                FROM Products
                WHERE IsApproved = 1
            ),
            OrderStats AS (
                SELECT 
                    COUNT(*) AS TotalOrders,
                    ISNULL(SUM(TotalPrice), 0) AS TotalOrderValue,
                    SUM(CASE WHEN Status = 'Delivered' THEN 1 ELSE 0 END) AS DeliveredOrders,
                    SUM(CASE WHEN Status = 'Processing' OR Status = 'In Transit' THEN 1 ELSE 0 END) AS ActiveOrders
                FROM Orders
            ),
            BookingStats AS (
                SELECT 
                    COUNT(*) AS TotalBookings,
                    ISNULL(SUM(Amount), 0) AS TotalBookingRevenue
                FROM Bookings
            )
            SELECT * FROM UserStats, ProductStats, OrderStats, BookingStats;
            ";
            return DbHelper.ExecuteQuery(sql);
        }

        // Category breakdown with Window Functions
        public static DataTable GetCategoryBreakdown()
        {
            string sql = @"
            SELECT 
                Category,
                COUNT(*) AS ListingCount,
                SUM(QuantityInQuintal) AS TotalQuantity,
                AVG(PricePerQuintal) AS AvgPrice,
                ROW_NUMBER() OVER (ORDER BY COUNT(*) DESC) AS PopularityRank
            FROM Products
            GROUP BY Category;
            ";
            return DbHelper.ExecuteQuery(sql);
        }

        // Live Farmer dashboard metrics
        public static DataTable GetFarmerDashboardMetrics(int farmerId)
        {
            string sql = @"
            SELECT 
                (SELECT ISNULL(SUM(AreaAcres), 0) FROM FarmPlots WHERE FarmerId = @FId) AS TotalAcres,
                COALESCE((SELECT TOP 1 Crop FROM FarmPlots WHERE FarmerId = @FId ORDER BY PlotId DESC), 'Mixed Crops') AS CurrentCrop,
                (SELECT COUNT(*) FROM Orders WHERE FarmerId = @FId AND Status != 'Delivered') AS ActiveOrders,
                (SELECT ISNULL(SUM(TotalPrice), 0) FROM Orders WHERE FarmerId = @FId) AS TotalRevenue,
                (SELECT COUNT(*) FROM Bookings WHERE UserId = @FId) AS TotalBookings;
            ";
            SqlParameter[] prms = new SqlParameter[] { new SqlParameter("@FId", farmerId) };
            return DbHelper.ExecuteQuery(sql, prms);
        }

        // Live Recent Orders for Admin Telemetry
        public static DataTable GetRecentPlatformOrders(int limit = 5)
        {
            string sql = @"
            SELECT TOP (@Limit) 
                o.OrderId, o.Commodity, o.Quantity, o.TotalPrice, o.OrderDate, o.Status,
                ISNULL(uBuyer.FullName, o.BuyerName) AS BuyerName,
                ISNULL(uFarmer.FullName, o.FarmerName) AS FarmerName
            FROM Orders o
            LEFT JOIN Users uBuyer ON o.BuyerId = uBuyer.UserId
            LEFT JOIN Users uFarmer ON o.FarmerId = uFarmer.UserId
            ORDER BY o.OrderId DESC";
            SqlParameter[] prms = new SqlParameter[] { new SqlParameter("@Limit", limit) };
            return DbHelper.ExecuteQuery(sql, prms);
        }
    }
}
