# KhedutRise - Smart Agriculture & Direct Farmer Marketplace Platform

## 1. Project Overview
KhedutRise is a web-based agricultural ecosystem built with ASP.NET Web Forms and Microsoft SQL Server. The platform bridges the gap between farmers, wholesale commodity buyers, farm equipment operators, and administrators by removing middlemen and enabling transparent, direct agricultural trade.

---

## 2. Key Modules and Functionalities

### 2.1 Farmer Portal
* **Dashboard:** Displays quick farm metrics, weather forecasts, and market trends.
* **Farm & Plots Management (`FarmPlots.aspx`):** Allows farmers to register farm plots, track area in acres, and monitor crop growth stages.
* **Crop Catalog (`MyCrops.aspx`):** Tracks cultivated crops from sowing to harvesting.
* **Sell Harvest (`SellHarvest.aspx`):** Enables farmers to list harvested crops directly on the marketplace with price, grade, and quantity.

### 2.2 Buyer Marketplace
* **Browse Marketplace (`Marketplace.aspx`):** Allows buyers to filter verified agricultural produce by category (Groundnut, Cotton, Wheat, Vegetables) and location.
* **Product Details (`ProductDetails.aspx`):** Displays product specifications, farmer background, available stock, and pricing.
* **Order Tracking (`OrderDetails.aspx`):** Features a 5-step fulfillment tracker: Order Placed -> Escrow Paid -> Shipped -> Out for Delivery -> Verified Arrival.
* **My Purchases (`MyPurchases.aspx`):** History of previous orders and purchase logs.
* **Wishlist (`Wishlist.aspx`):** Allows buyers to save crops for future purchase.

### 2.3 Farm Machinery & Rentals
* **Machinery Catalog (`TractorRentals.aspx`):** Displays available tractors, rotavators, and harvesters for hourly rental.
* **Booking Confirmation (`BookingConfirm.aspx`):** Facilitates machinery scheduling with hourly calculation, address input, and Cash on Delivery / Field payment selection.
* **Rental History (`MyBookings.aspx`):** Tracks active and completed equipment bookings.

### 2.4 Live APMC Mandi Rates
* **Market Rates (`ApmcRates.aspx`):** Real-time APMC mandi commodity rates with search filters for crops and market yards.

### 2.5 Agri Services & Soil Testing
* **Agricultural Services (`AgriServices.aspx`):** Allows farmers to book soil testing visits, drone spraying, and irrigation consultations.

### 2.6 Land Portal
* **Farmland Listings (`LandPortal.aspx`):** Portal for leasing agricultural land or partnering on crop cultivation.

### 2.7 Weather & Community
* **Weather Forecast (`WeatherForecast.aspx`):** Displays 7-day temperature, precipitation, and agricultural weather advice.
* **Farmer Community (`Community.aspx`):** Interactive forum for farmers to discuss crop diseases, irrigation methods, and farming practices.

### 2.8 Administrative Controls
* **Admin Dashboard (`AdminDashboard.aspx`):** Overview of total platform users, active listings, and pending approvals.
* **Platform Management (`AdminManagement.aspx`):** Moderation system to verify new farmers, approve crop listings, and manage land postings.
* **Platform Settings (`AdminSettings.aspx`):** Configuration for platform commissions, auto-approvals, and helpline information.
* **Platform Analytics (`AdminAnalytics.aspx`):** Visual metrics and revenue summaries.

---

## 3. User Roles and Security

* **Farmer:** Can manage farm plots, post crop listings, book equipment, and view APMC rates.
* **Buyer:** Can browse marketplace crops, place orders, track shipments, and view mandi rates.
* **Operator:** Can manage rental bookings and equipment listings.
* **Admin:** Super-user with full platform management, moderation, and approval privileges.
* **Guest:** Public visitor with browsing access to public catalogs and market rates.

---

## 4. Technology Stack

* **Framework:** ASP.NET Web Forms (.NET Framework 4.8)
* **Language:** C# (Code-Behind Architecture)
* **Database:** Microsoft SQL Server (LocalDB / Express)
* **Data Access:** ADO.NET (`SqlConnection`, `SqlCommand`, `SqlDataAdapter`, `DataTable`)
* **Styling & Layout:** Bootstrap 5, Bootstrap Icons, Custom CSS
* **Validation:** Built-in HTML5 form validation (`required`, `type="tel"`, `type="email"`, `pattern`, `minlength`, `maxlength`)
* **Web Server:** IIS Express

---

## 5. C# Collections and Generics Implementation

All collection and generic concepts are centralized in `App_Code/KhedutCollectionsService.cs`:

1. **`ArrayList` (Non-Generic):** Stores heterogeneous search criteria (string, decimal, bool) in `Marketplace.aspx`.
2. **`Queue` (Non-Generic FIFO):** Manages machinery rental requests in first-in-first-out order in `BookingConfirm.aspx`.
3. **`Hashtable` (Non-Generic):** Key-value dictionary for translating crop names between English and Gujarati.
4. **`List<T>` (Generic List):** Strongly-typed collection converting database records into `List<CropItem>`.
5. **`Dictionary<TKey, TValue>` (Generic):** Instant $O(1)$ lookup for real-time commodity rates in `ApmcRates.aspx`.
6. **`HashSet<T>` (Generic):** Extracts unique district and market names without duplicate entries in `Marketplace.aspx`.
7. **`Queue<T>` (Generic Queue):** Strongly-typed FIFO pipeline for managing soil lab test requests in `AgriServices.aspx`.
8. **`Generic Method` (`GetSessionValue<T>`):** Type-safe session value reader eliminating manual type casting.
9. **`Generic Delegates`:**
   * `Predicate<T>`: In-memory filtering of crop items by price or category.
   * `Func<T, TResult>`: Calculation of rental totals with extra fees.
   * `Action<T>`: Reusable notification and audit logging delegate.
10. **`Generic Class` (`ApiResponse<T>`):** Standardized response wrapper for operational results.

---

## 6. Database Setup

1. Open **SQL Server Management Studio (SSMS)** or Visual Studio **SQL Server Object Explorer**.
2. Connect to your local SQL Server instance: `(localdb)\MSSQLLocalDB`.
3. Open and execute the script located at:
   ```
   KhedutRise_WebForms/Database_Schema.sql
   ```
4. The script creates the `KhedutRiseDB` database along with the following 9 tables and sample seed records:
   * `Users`
   * `Products`
   * `FarmPlots`
   * `ApmcRates`
   * `Orders`
   * `Equipments`
   * `Bookings`
   * `LandListings`
   * `ContactInquiries`

---

## 7. How to Run the Project

1. Open Visual Studio.
2. Click **File -> Open -> Project/Solution** and select:
   ```
   KhedutRise.sln
   ```
3. In Solution Explorer, ensure `Web.config` connection string points to your local database:
   ```xml
   <connectionStrings>
     <add name="KhedutRiseDB" 
          connectionString="Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=KhedutRiseDB;Integrated Security=True;Connect Timeout=30" 
          providerName="System.Data.SqlClient" />
   </connectionStrings>
   ```
4. Right-click on `Default.aspx` in Solution Explorer and select **Set As Start Page**.
5. Press **F5** or **Ctrl + F5** to run the website in IIS Express.

---

## 8. Demo Credentials

### 8.1 User Authentication (Mobile OTP)
* Navigate to `Login.aspx`.
* Select your role (**Farmer**, **Buyer**, or **Operator**).
* Enter any 10-digit mobile number (e.g. `9876543210`).
* Click **Send OTP**.
* Enter verification OTP: `482190`.
* Click **Verify & Sign In**.

### 8.2 Administrator Portal
* Navigate to `AdminLogin.aspx`.
* **Admin Email:** `admin@khedutrise.com`
* **Admin Password:** `admin123`
* Click **Secure Admin Login**.

---

## 9. Project Directory Structure

```
KhedutRise/
│
├── KhedutRise.sln                       # Visual Studio Solution File
├── README.md                            # Project Documentation
│
└── KhedutRise_WebForms/                 # Web Application Directory
    ├── App_Code/
    │   ├── DbHelper.cs                  # ADO.NET Database Operations Helper
    │   └── KhedutCollectionsService.cs  # Collections & Generics Master Service
    │
    ├── Images/                          # Platform Media & Photos
    ├── Site.Master                      # Master Page Template
    ├── Site.Master.cs                   # Master Page Code-Behind & Role Navigation
    ├── Web.config                       # Application & Connection String Configuration
    ├── Database_Schema.sql              # Database Creation & Seeding Script
    │
    ├── Default.aspx                     # Home Landing Page
    ├── Login.aspx                       # Mobile OTP Authentication Page
    ├── SelectRole.aspx                  # Role Selection Gateway
    ├── RegisterFarmer.aspx              # Farmer Registration Form
    ├── RegisterBuyer.aspx               # Buyer Registration Form
    ├── RegisterOperator.aspx            # Equipment Operator Registration Form
    │
    ├── Marketplace.aspx                 # Agricultural Produce Catalog
    ├── ProductDetails.aspx              # Detailed Product View
    ├── SellHarvest.aspx                 # Crop Listing & Sales Management
    ├── OrderDetails.aspx                # Order Tracking & Escrow Milestone View
    ├── MyPurchases.aspx                 # Buyer Orders History
    ├── Wishlist.aspx                    # Buyer Saved Items
    │
    ├── TractorRentals.aspx              # Farm Machinery Catalog
    ├── BookingConfirm.aspx              # Equipment Booking Confirmation Page
    ├── MyBookings.aspx                  # Equipment Rental Booking History
    │
    ├── ApmcRates.aspx                   # Live Mandi Rates Page
    ├── AgriServices.aspx                # Soil Testing & Farm Services Page
    ├── LandPortal.aspx                  # Agricultural Land Portal
    │
    ├── FarmerDashboard.aspx             # Farmer Summary Dashboard
    ├── FarmPlots.aspx                   # Farm Land GPS & Plots Registry
    ├── MyCrops.aspx                     # Active Crops Registry
    │
    ├── BuyerDashboard.aspx              # Buyer Summary Dashboard
    ├── WeatherForecast.aspx             # 7-Day Weather & Advisory Page
    ├── Community.aspx                   # Farmer Discussion Forum
    ├── Contact.aspx                     # Support & Inquiries Form
    ├── About.aspx                       # About KhedutRise Information
    │
    ├── AdminLogin.aspx                  # Protected Admin Login
    ├── AdminDashboard.aspx              # Super Admin Analytics & Approvals
    ├── AdminManagement.aspx             # User Moderation & Directory
    ├── AdminAnalytics.aspx              # Revenue & Platform Metrics
    └── AdminSettings.aspx               # Platform Policies & Preferences
```
