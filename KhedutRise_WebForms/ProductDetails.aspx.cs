using System;
using System.Data;

namespace KhedutRise
{
    // Product Details & Direct Purchase Page - 100% Dynamic with Concurrency-Safe Order Execution
    public partial class ProductDetails : System.Web.UI.Page
    {
        private int ProductId
        {
            get
            {
                int id;
                if (int.TryParse(Request.QueryString["id"], out id)) return id;
                return 0;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (ProductId <= 0)
                {
                    ShowNotFound();
                    return;
                }
                LoadProductDetails(ProductId);
            }
        }

        // Retrieve product from database via ProductRepository
        private void LoadProductDetails(int id)
        {
            try
            {
                DataTable dt = ProductRepository.GetProductDetailsById(id);
                if (dt == null || dt.Rows.Count == 0)
                {
                    ShowNotFound();
                    return;
                }

                DataRow r = dt.Rows[0];
                pnlNotFound.Visible = false;
                pnlProductContent.Visible = true;

                string prodName = r["ProductName"].ToString();
                string category = r["Category"].ToString();
                decimal price = Convert.ToDecimal(r["PricePerQuintal"]);
                decimal stock = Convert.ToDecimal(r["QuantityInQuintal"]);
                string farmer = r["FarmerName"].ToString();
                string location = r["Location"].ToString();
                string grade = r["Grade"] != DBNull.Value ? r["Grade"].ToString() : "Grade A";
                string contact = r["FarmerContact"] != DBNull.Value ? r["FarmerContact"].ToString() : "+91 98250 12345";
                string img = r["ImagePath"] != DBNull.Value ? r["ImagePath"].ToString() : "Images/groundnut_harvest.jpg";

                lblCropTitle.Text = prodName;
                lblCategory.Text = category;
                lblGrade.Text = grade;
                lblPrice.Text = price.ToString("N0");
                lblAvailableStock.Text = stock.ToString("N1");
                lblFarmerName.Text = farmer;
                lblLocation.Text = location;
                lblFarmerContact.Text = contact;
                lblInitials.Text = GetInitials(farmer);
                imgMainProduct.ImageUrl = ResolveUrl(img);
                imgThumb1.ImageUrl = ResolveUrl(img);
                imgThumb2.ImageUrl = ResolveUrl(img);
                imgThumb3.ImageUrl = ResolveUrl("Images/apmc_yard.jpg");

                if (stock <= 0)
                {
                    btnConfirmOrder.Enabled = false;
                    btnConfirmOrder.Text = "Out of Stock / Sold Out";
                    btnConfirmOrder.CssClass = "btn w-100 py-3 fw-bold text-white bg-secondary border-0 shadow-sm";
                }
            }
            catch (Exception ex)
            {
                ShowNotFound();
            }
        }

        // Place direct purchase order into database with transaction and stock update
        protected void btnConfirmOrder_Click(object sender, EventArgs e)
        {
            // 1. Enforce Authentication
            if (Session["UserId"] == null)
            {
                Response.Redirect("Login.aspx?error=auth_required&returnUrl=" + Server.UrlEncode("ProductDetails.aspx?id=" + ProductId));
                return;
            }

            int buyerId = Convert.ToInt32(Session["UserId"]);
            string buyerName = Session["UserName"] != null ? Session["UserName"].ToString() : "Buyer";
            string role = Session["UserRole"] != null ? Session["UserRole"].ToString() : "";

            // 2. Validate Role (must be Buyer)
            if (role != "Buyer" && role != "Admin")
            {
                lblOrderMsg.Text = "⚠️ Only registered Buyers can place crop purchase orders. Your current account role is: " + role;
                lblOrderMsg.CssClass = "alert alert-warning d-block p-3 text-center mb-4";
                return;
            }

            // 3. Validate Quantity
            decimal requestedQty;
            if (!decimal.TryParse(txtOrderQty.Text.Trim(), out requestedQty) || requestedQty <= 0)
            {
                lblOrderMsg.Text = "❌ Please enter a valid positive quantity in Quintals.";
                lblOrderMsg.CssClass = "alert alert-danger d-block p-3 text-center mb-4";
                return;
            }

            string address = txtDeliveryAddress.Text.Trim();
            if (string.IsNullOrWhiteSpace(address))
            {
                address = "Standard Gujarat Delivery Destination";
            }

            // 4. Execute atomic transaction in OrderRepository
            string errorMsg;
            int orderId = OrderRepository.PlaceOrder(buyerId, buyerName, ProductId, requestedQty, address, out errorMsg);

            if (orderId > 0)
            {
                lblOrderMsg.Text = "🎉 Order Placed Successfully! Your Order ID is <b>#" + orderId + "</b>. You can track this purchase in <a href='MyPurchases.aspx' class='alert-link fw-bold text-decoration-underline'>My Purchases</a>.";
                lblOrderMsg.CssClass = "alert alert-success d-block p-3 text-center mb-4 fs-6";

                // Refresh product details to show updated remaining stock
                LoadProductDetails(ProductId);
            }
            else
            {
                lblOrderMsg.Text = "❌ Order Failed: " + errorMsg;
                lblOrderMsg.CssClass = "alert alert-danger d-block p-3 text-center mb-4";
            }
        }

        private string GetInitials(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return "KR";
            string[] parts = name.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length >= 2) return (parts[0][0].ToString() + parts[1][0].ToString()).ToUpper();
            return parts[0].Substring(0, Math.Min(2, parts[0].Length)).ToUpper();
        }

        private void ShowNotFound()
        {
            pnlNotFound.Visible = true;
            pnlProductContent.Visible = false;
        }
    }
}
