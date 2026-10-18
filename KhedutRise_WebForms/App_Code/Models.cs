using System;

namespace KhedutRise
{
    // 1. User Domain Model
    public class User
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public string UserRole { get; set; }
        public string Location { get; set; }
        public string SpecificField { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    // 2. Product (Crop Produce) Domain Model
    public class Product
    {
        public int ProductId { get; set; }
        public int? FarmerId { get; set; }
        public string ProductName { get; set; }
        public string Category { get; set; }
        public decimal QuantityInQuintal { get; set; }
        public decimal PricePerQuintal { get; set; }
        public string FarmerName { get; set; }
        public string Location { get; set; }
        public string Grade { get; set; }
        public bool IsApproved { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ImagePath { get; set; }
        public string FarmerMobile { get; set; }
    }

    // 3. Order Domain Model
    public class Order
    {
        public int OrderId { get; set; }
        public int? BuyerId { get; set; }
        public int? FarmerId { get; set; }
        public int? ProductId { get; set; }
        public string BuyerName { get; set; }
        public string FarmerName { get; set; }
        public string Commodity { get; set; }
        public string Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public string DeliveryAddress { get; set; }
        public string BuyerMobile { get; set; }
        public string FarmerMobile { get; set; }
    }

    // 4. Equipment Domain Model
    public class Equipment
    {
        public int EquipmentId { get; set; }
        public int? OwnerId { get; set; }
        public string ModelName { get; set; }
        public string Horsepower { get; set; }
        public decimal HourlyRate { get; set; }
        public string OwnerName { get; set; }
        public string Location { get; set; }
        public decimal Rating { get; set; }
        public bool IsAvailable { get; set; }
        public string ImagePath { get; set; }
    }

    // 5. Booking Domain Model
    public class Booking
    {
        public string BookingId { get; set; }
        public int? UserId { get; set; }
        public int? EquipmentId { get; set; }
        public string ServiceType { get; set; }
        public string ProviderName { get; set; }
        public string CustomerName { get; set; }
        public DateTime BookingDate { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
        public string EquipmentModel { get; set; }
    }

    // 6. FarmPlot Domain Model
    public class FarmPlot
    {
        public int PlotId { get; set; }
        public int? FarmerId { get; set; }
        public string PlotName { get; set; }
        public decimal AreaAcres { get; set; }
        public string Crop { get; set; }
        public string Stage { get; set; }
        public string Status { get; set; }
        public string SoilType { get; set; }
        public DateTime? SowingDate { get; set; }
        public DateTime? ExpectedHarvestDate { get; set; }
        public string FarmerName { get; set; }
    }
}
