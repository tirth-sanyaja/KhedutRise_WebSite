using System;
using System.Data;

namespace KhedutRise
{
    public partial class OrderDetails : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserId"] == null)
            {
                Response.Redirect("Login.aspx?returnUrl=" + Server.UrlEncode(Request.RawUrl));
                return;
            }

            if (!IsPostBack)
            {
                int orderId = 0;
                string qId = Request.QueryString["orderId"] ?? Request.QueryString["id"];
                if (!string.IsNullOrEmpty(qId))
                {
                    int.TryParse(qId, out orderId);
                }

                if (orderId == 0)
                {
                    // Fallback to highest order ID
                    DataTable dtLatest = DbHelper.ExecuteQuery("SELECT TOP 1 OrderId FROM Orders ORDER BY OrderId DESC");
                    if (dtLatest != null && dtLatest.Rows.Count > 0)
                    {
                        orderId = Convert.ToInt32(dtLatest.Rows[0]["OrderId"]);
                    }
                }

                if (orderId > 0)
                {
                    LoadOrder(orderId);
                }
                else
                {
                    lblOrderMsg.Text = "No order record specified.";
                    lblOrderMsg.CssClass = "alert alert-warning d-block p-2 text-center small mb-3";
                }
            }
        }

        private void LoadOrder(int orderId)
        {
            try
            {
                DataTable dt = OrderRepository.GetOrderById(orderId);
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow r = dt.Rows[0];

                    litOrderIdHeader.Text = r["OrderId"].ToString();
                    litProductName.Text = r["ProductName"].ToString();
                    litFarmerInfo.Text = r["FarmerFullName"] != DBNull.Value ? r["FarmerFullName"].ToString() : (r["FarmerName"] != DBNull.Value ? r["FarmerName"].ToString() : "Verified Farmer");
                    litQuantity.Text = r["Quantity"].ToString();
                    DateTime oDate = Convert.ToDateTime(r["OrderDate"]);
                    litOrderDate.Text = oDate.ToString("dd MMM yyyy, hh:mm tt");

                    decimal total = Convert.ToDecimal(r["TotalPrice"]);
                    litSubtotal.Text = total.ToString("N0");
                    litLogisticsFee.Text = "0";
                    litTotalPaid.Text = total.ToString("N0");

                    string buyerName = r["BuyerFullName"] != DBNull.Value ? r["BuyerFullName"].ToString() : (r["BuyerName"] != DBNull.Value ? r["BuyerName"].ToString() : "Registered Buyer");
                    litBuyerDetails.Text = buyerName;

                    string deliveryAddr = r["DeliveryAddress"] != DBNull.Value ? r["DeliveryAddress"].ToString() : "Buyer Verified Address, Gujarat";
                    litDeliveryAddress.Text = deliveryAddr;

                    string img = r["ImagePath"] != DBNull.Value ? r["ImagePath"].ToString() : "Images/groundnut_harvest.jpg";
                    imgProduct.ImageUrl = ResolveUrl(img);

                    string status = r["Status"].ToString();
                    UpdateStepper(status);
                }
                else
                {
                    lblOrderMsg.Text = "Order #" + orderId + " was not found in database records.";
                    lblOrderMsg.CssClass = "alert alert-warning d-block p-2 text-center small mb-3";
                }
            }
            catch (Exception ex)
            {
                lblOrderMsg.Text = "Error loading order: " + ex.Message;
                lblOrderMsg.CssClass = "alert alert-danger d-block p-2 text-center small mb-3";
            }
        }

        private void UpdateStepper(string status)
        {
            litStatusBadge.Text = "<span class='badge bg-success-subtle text-success border border-success-subtle fw-bold px-3 py-1'>" + status + "</span>";

            string activeNode = "rounded-circle d-inline-flex align-items-center justify-content-center text-white";
            string activeStyle = "width: 22px; height: 22px; background-color: #0b6028; font-size: 0.65rem;";

            string inactiveNode = "rounded-circle d-inline-flex align-items-center justify-content-center border";
            string inactiveStyle = "width: 22px; height: 22px; background-color: #ffffff; border-color: #cbd5e1 !important; font-size: 0.65rem;";

            node1.Attributes["class"] = activeNode;
            node1.Attributes["style"] = activeStyle;
            node1.InnerText = "✓";

            if (status.Equals("Processing", StringComparison.OrdinalIgnoreCase))
            {
                node2.Attributes["class"] = activeNode;
                node2.Attributes["style"] = activeStyle;
                node2.InnerText = "✓";

                node3.Attributes["class"] = inactiveNode;
                node3.Attributes["style"] = inactiveStyle;
                node3.InnerText = "•";

                node4.Attributes["class"] = inactiveNode;
                node4.Attributes["style"] = inactiveStyle;
                node4.InnerText = "•";

                node5.Attributes["class"] = inactiveNode;
                node5.Attributes["style"] = inactiveStyle;
                node5.InnerText = "•";

                litStepperProgressLine.Text = "<div style='width: 35%; background-color: #0b6028;'></div><div style='width: 65%; background-color: #e2e8f0;'></div>";
            }
            else if (status.Equals("Shipped", StringComparison.OrdinalIgnoreCase))
            {
                node2.Attributes["class"] = activeNode;
                node2.Attributes["style"] = activeStyle;
                node2.InnerText = "✓";

                node3.Attributes["class"] = activeNode;
                node3.Attributes["style"] = activeStyle;
                node3.InnerText = "✓";

                node4.Attributes["class"] = inactiveNode;
                node4.Attributes["style"] = inactiveStyle;
                node4.InnerText = "•";

                node5.Attributes["class"] = inactiveNode;
                node5.Attributes["style"] = inactiveStyle;
                node5.InnerText = "•";

                litStepperProgressLine.Text = "<div style='width: 60%; background-color: #0b6028;'></div><div style='width: 40%; background-color: #e2e8f0;'></div>";
            }
            else if (status.Equals("In Transit", StringComparison.OrdinalIgnoreCase) || status.Equals("Out for Delivery", StringComparison.OrdinalIgnoreCase))
            {
                node2.Attributes["class"] = activeNode;
                node2.Attributes["style"] = activeStyle;
                node2.InnerText = "✓";

                node3.Attributes["class"] = activeNode;
                node3.Attributes["style"] = activeStyle;
                node3.InnerText = "✓";

                node4.Attributes["class"] = activeNode;
                node4.Attributes["style"] = activeStyle;
                node4.InnerText = "✓";

                node5.Attributes["class"] = inactiveNode;
                node5.Attributes["style"] = inactiveStyle;
                node5.InnerText = "•";

                litStepperProgressLine.Text = "<div style='width: 80%; background-color: #0b6028;'></div><div style='width: 20%; background-color: #e2e8f0;'></div>";
            }
            else if (status.Equals("Delivered", StringComparison.OrdinalIgnoreCase))
            {
                node2.Attributes["class"] = activeNode;
                node2.Attributes["style"] = activeStyle;
                node2.InnerText = "✓";

                node3.Attributes["class"] = activeNode;
                node3.Attributes["style"] = activeStyle;
                node3.InnerText = "✓";

                node4.Attributes["class"] = activeNode;
                node4.Attributes["style"] = activeStyle;
                node4.InnerText = "✓";

                node5.Attributes["class"] = activeNode;
                node5.Attributes["style"] = activeStyle;
                node5.InnerText = "✓";

                litStepperProgressLine.Text = "<div style='width: 100%; background-color: #0b6028;'></div>";
            }
            else
            {
                litStatusBadge.Text = "<span class='badge bg-danger-subtle text-danger border border-danger-subtle fw-bold px-3 py-1'>" + status + "</span>";
                litStepperProgressLine.Text = "<div style='width: 25%; background-color: #ea580c;'></div><div style='width: 75%; background-color: #e2e8f0;'></div>";
            }
        }
    }
}
