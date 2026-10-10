using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;

namespace KhedutRise
{
    

    // Model class for crop items
    public class CropItem
    {
        public int ProductId { get; set; }
        public string CropName { get; set; }
        public string Category { get; set; }
        public decimal PricePerQuintal { get; set; }
        public decimal Quantity { get; set; }
        public string Location { get; set; }
    }

    // Model class for soil test bookings
    public class SoilTestRequest
    {
        public int RequestId { get; set; }
        public string FarmerName { get; set; }
        public string ServiceName { get; set; }
        public DateTime RequestDate { get; set; }
    }

    public static class KhedutCollectionsService
    {
        // 1. Non-Generic Collection: ArrayList
        // Purpose: Stores search filter values (text, price number, boolean flag)
        // Used in: Marketplace.aspx
        public static ArrayList GetSearchFilters(string cropName, decimal minPrice, bool isVerified)
        {
            ArrayList filterList = new ArrayList();
            filterList.Add(cropName);       // String value
            filterList.Add(minPrice);       // Decimal number value
            filterList.Add(isVerified);     // Boolean true/false value
            return filterList;
        }

        // 2. Non-Generic Collection: Queue (FIFO - First In First Out)
        // Purpose: Stores tractor rental booking requests in order of booking
        // Used in: BookingConfirm.aspx
        private static Queue bookingQueue = new Queue();

        public static void EnqueueRentalBooking(string bookingDetails)
        {
            bookingQueue.Enqueue(bookingDetails);
        }

        public static string DequeueRentalBooking()
        {
            if (bookingQueue.Count > 0)
            {
                return bookingQueue.Dequeue().ToString();
            }
            return "No bookings in queue";
        }

        public static int GetBookingQueueCount()
        {
            return bookingQueue.Count;
        }

        // 3. Non-Generic Collection: Hashtable
        // Purpose: Key-Value dictionary to translate crop names to Gujarati
        // Used in: ApmcRates.aspx and Site.Master
        private static Hashtable cropTranslationTable;

        static KhedutCollectionsService()
        {
            cropTranslationTable = new Hashtable();
            cropTranslationTable.Add("Cotton", "કપાસ");
            cropTranslationTable.Add("Groundnut", "મગફળી");
            cropTranslationTable.Add("Wheat", "ઘઉં");
            cropTranslationTable.Add("Cumin", "જીરું");
            cropTranslationTable.Add("Mustard", "રાયડો");
            cropTranslationTable.Add("Sesame", "તલ");
        }

        public static string TranslateCropName(string englishName)
        {
            if (cropTranslationTable.ContainsKey(englishName))
            {
                return cropTranslationTable[englishName].ToString();
            }
            return englishName;
        }

        // 4. Generic Collection: List<T>
        // Purpose: Strongly-typed list of CropItem objects fetched from database
        // Used in: Marketplace.aspx
        public static List<CropItem> ConvertDataTableToCropList(DataTable dt)
        {
            List<CropItem> list = new List<CropItem>();
            if (dt == null) return list;

            foreach (DataRow row in dt.Rows)
            {
                CropItem item = new CropItem();
                if (dt.Columns.Contains("ProductId")) item.ProductId = Convert.ToInt32(row["ProductId"]);
                if (dt.Columns.Contains("ProductName")) item.CropName = row["ProductName"].ToString();
                if (dt.Columns.Contains("Category")) item.Category = row["Category"].ToString();
                if (dt.Columns.Contains("PricePerQuintal")) item.PricePerQuintal = Convert.ToDecimal(row["PricePerQuintal"]);
                if (dt.Columns.Contains("QuantityInQuintal")) item.Quantity = Convert.ToDecimal(row["QuantityInQuintal"]);
                if (dt.Columns.Contains("Location")) item.Location = row["Location"].ToString();
                list.Add(item);
            }
            return list;
        }

        // 5. Generic Collection: Dictionary<TKey, TValue>
        // Purpose: Instant lookup for APMC market yard commodity prices
        // Used in: ApmcRates.aspx
        public static Dictionary<string, decimal> GetLiveMandiRates()
        {
            Dictionary<string, decimal> mandiRates = new Dictionary<string, decimal>();
            mandiRates["Cotton"] = 7250;
            mandiRates["Groundnut"] = 6800;
            mandiRates["Wheat"] = 2850;
            mandiRates["Cumin"] = 28500;
            mandiRates["Castor"] = 6120;
            return mandiRates;
        }

        // 6. Generic Collection: HashSet<T>
        // Purpose: Stores unique district names without duplicates
        // Used in: Marketplace.aspx
        public static HashSet<string> GetUniqueDistricts(List<CropItem> crops)
        {
            HashSet<string> districtSet = new HashSet<string>();
            if (crops != null)
            {
                foreach (CropItem item in crops)
                {
                    if (!string.IsNullOrEmpty(item.Location))
                    {
                        districtSet.Add(item.Location.Trim());
                    }
                }
            }
            districtSet.Add("Rajkot");
            districtSet.Add("Gondal");
            districtSet.Add("Amreli");
            return districtSet;
        }

        // 7. Generic Collection: Queue<T>
        // Purpose: Strongly-typed FIFO queue for soil test requests
        // Used in: AgriServices.aspx
        private static Queue<SoilTestRequest> soilTestQueue = new Queue<SoilTestRequest>();

        public static void EnqueueSoilTest(SoilTestRequest request)
        {
            soilTestQueue.Enqueue(request);
        }

        public static SoilTestRequest DequeueSoilTest()
        {
            if (soilTestQueue.Count > 0)
            {
                return soilTestQueue.Dequeue();
            }
            return null;
        }

        // 8. Generic Concept: Generic Method
        // Purpose: Simple method to read any Session variable safely
        // Used in: Site.Master.cs and BookingConfirm.aspx
        public static T GetSessionValue<T>(string key, T defaultValue)
        {
            if (HttpContext.Current != null && HttpContext.Current.Session != null)
            {
                object val = HttpContext.Current.Session[key];
                if (val != null)
                {
                    try
                    {
                        return (T)val;
                    }
                    catch
                    {
                        return defaultValue;
                    }
                }
            }
            return defaultValue;
        }

        // 9. Generic Concept: Generic Delegates
        // Purpose: Shows Predicate<T>, Func<T, TResult>, and Action<T>
        // Used in: Marketplace.aspx and BookingConfirm.aspx

        // Predicate<T>: Returns true or false to filter crops
        public static List<CropItem> FilterCropsByPrice(List<CropItem> crops, Predicate<CropItem> condition)
        {
            if (crops == null || condition == null) return crops;
            return crops.FindAll(condition);
        }

        // Func<T, TResult>: Takes rental hours and rate, returns total cost
        public static decimal CalculateRent(int hours, decimal ratePerHour, Func<decimal, decimal> addExtraFee)
        {
            decimal basicRent = hours * ratePerHour;
            if (addExtraFee != null)
            {
                return addExtraFee(basicRent);
            }
            return basicRent;
        }

        // Action<T>: Takes a message and performs an action (printing/logging)
        public static void LogMessage(string message, Action<string> action)
        {
            if (action != null)
            {
                action(message);
            }
        }
    }

    // 10. Generic Concept: Generic Class
    // Purpose: Standard wrapper to return status and data for any entity
    // Used in: AgriServices.aspx
    public class ApiResponse<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public T ResultData { get; set; }

        public ApiResponse(bool success, string msg, T data)
        {
            IsSuccess = success;
            Message = msg;
            ResultData = data;
        }
    }
}
