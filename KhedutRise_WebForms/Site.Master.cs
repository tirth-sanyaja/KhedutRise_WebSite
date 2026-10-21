using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web.UI;

namespace KhedutRise
{
    // Master page logic: Manages user roles, enforces route protection, and renders clean desktop & mobile navigation
    public partial class Site : System.Web.UI.MasterPage
    {
        private class NavItem
        {
            public string Url { get; set; }
            public string Title { get; set; }
            public NavItem(string url, string title) { Url = url; Title = title; }
        }

        private class DropdownItem
        {
            public string Url { get; set; }
            public string Title { get; set; }
            public string Icon { get; set; }
            public string TextColor { get; set; }
            public bool IsDivider { get; set; }

            public DropdownItem(string url, string title, string icon, string textColor = "text-dark")
            {
                Url = url; Title = title; Icon = icon; TextColor = textColor; IsDivider = false;
            }

            public DropdownItem(bool isDivider)
            {
                IsDivider = isDivider;
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            // Handle user logout action when requested via query parameter
            if (Request.QueryString["action"] == "logout")
            {
                Session.Clear();
                Session.Abandon();
                Response.Redirect("Default.aspx");
                return;
            }

            // Identify current page
            string currentPage = Path.GetFileName(Request.FilePath);
            // Read active role from Session
            string activeRole = Session["UserRole"] != null ? Session["UserRole"].ToString() : "";

            // Route-level page definitions
            string[] farmerPages = { "FarmerDashboard.aspx", "FarmPlots.aspx", "MyCrops.aspx", "SellHarvest.aspx" };
            string[] buyerPages = { "BuyerDashboard.aspx", "MyPurchases.aspx", "OrderDetails.aspx", "Wishlist.aspx" };
            string[] operatorPages = { "MyBookings.aspx" };
            string[] adminPages = { "AdminDashboard.aspx", "AdminManagement.aspx", "AdminAnalytics.aspx", "AdminSettings.aspx" };
            string[] authenticatedActionPages = { "BookingConfirm.aspx", "ProfileSettings.aspx" };

            // 1. PostBack Protection: Actions on role-exclusive routes require registration/login
            if (Page.IsPostBack)
            {
                if (Array.IndexOf(adminPages, currentPage) >= 0 && activeRole != "Admin")
                {
                    Response.Redirect("AdminLogin.aspx?error=admin_required");
                    return;
                }
                else if (string.IsNullOrEmpty(activeRole))
                {
                    if (Array.IndexOf(farmerPages, currentPage) >= 0 ||
                        Array.IndexOf(buyerPages, currentPage) >= 0 ||
                        Array.IndexOf(operatorPages, currentPage) >= 0 ||
                        Array.IndexOf(authenticatedActionPages, currentPage) >= 0)
                    {
                        Response.Redirect("SelectRole.aspx?msg=register_required&returnUrl=" + Server.UrlEncode(currentPage));
                        return;
                    }
                }
            }

            // 2. Read-Only / Preview Mode for Guest:
            if (string.IsNullOrEmpty(activeRole))
            {
                bool isRoleExclusive = Array.IndexOf(farmerPages, currentPage) >= 0 ||
                                       Array.IndexOf(buyerPages, currentPage) >= 0 ||
                                       Array.IndexOf(operatorPages, currentPage) >= 0 ||
                                       Array.IndexOf(adminPages, currentPage) >= 0 ||
                                       Array.IndexOf(authenticatedActionPages, currentPage) >= 0;

                if (isRoleExclusive && phGuestBanner != null)
                {
                    string bannerHtml = @"<div class='alert alert-warning alert-dismissible fade show text-center border-0 shadow-sm py-2 px-3 mb-0' role='alert' style='background-color: #fffbeb; color: #92400e; border-left: 5px solid #f59e0b !important; border-radius: 0;'>
                        <i class='bi bi-eye-fill me-2 fs-6'></i>
                        <strong>Guest Preview Mode:</strong> You are exploring this page in read-only mode. To perform actions, buy, sell, or book, please <a href='SelectRole.aspx' class='text-dark fw-bold text-decoration-underline'>Register (Choose Role)</a> or <a href='Login.aspx' class='text-dark fw-bold text-decoration-underline'>Login</a>.
                        <button type='button' class='btn-close py-2' data-bs-dismiss='alert' aria-label='Close'></button>
                    </div>";
                    phGuestBanner.Controls.Add(new LiteralControl(bannerHtml));
                }
            }

            // Build dynamic navigation for the validated session
            BuildNavigation(currentPage, activeRole);
        }

        private void BuildNavigation(string currentPage, string activeRole)
        {
            string userName = Session["UserName"] != null ? Session["UserName"].ToString() : "User";
            List<NavItem> navItems = GetNavItemsForRole(activeRole);

            // Render navigation links
            StringBuilder sbNav = new StringBuilder();
            sbNav.Append("<ul class='kr-nav-list'>");
            foreach (var item in navItems)
            {
                string activeClass = (currentPage == item.Url) ? "active" : "";
                sbNav.AppendFormat("<li><a href='{0}' class='kr-nav-link {1}'>{2}</a></li>", item.Url, activeClass, item.Title);
            }
            sbNav.Append("</ul>");

            // Render auth buttons
            string authHtml = "";
            if (!string.IsNullOrEmpty(activeRole))
            {
                authHtml = string.Format(
                    "<span class='d-none d-lg-inline-block text-dark fw-bold me-2' style='font-size: 0.9rem;'>Welcome, {0} ({1})</span>" +
                    "<a href='Default.aspx?action=logout' class='btn-kr-logout' title='Log out of account'>LOGOUT</a>",
                    Server.HtmlEncode(userName),
                    activeRole);
            }
            else
            {
                authHtml = string.Format(
                    "<a href='Login.aspx' class='btn-kr-login {0}'>Login</a>" +
                    "<a href='SelectRole.aspx' class='btn-kr-register {1}'>Register</a>",
                    currentPage == "Login.aspx" ? "active" : "",
                    (currentPage.Contains("Register") || currentPage == "SelectRole.aspx") ? "active" : "");
            }

            phNavLinks.Controls.Add(new LiteralControl(sbNav.ToString()));
            phNavAuth.Controls.Add(new LiteralControl(authHtml));

            string userIconsHtml = GetNotificationBellHtml() + GetProfileDropdownHtml(activeRole, userName);
            phUserIcons.Controls.Add(new LiteralControl(userIconsHtml));
        }

        private List<NavItem> GetNavItemsForRole(string role)
        {
            List<NavItem> items = new List<NavItem>();
            switch (role)
            {
                case "Farmer":
                    items.Add(new NavItem("FarmerDashboard.aspx", "Dashboard"));
                    items.Add(new NavItem("MyCrops.aspx", "My Crops"));
                    items.Add(new NavItem("SellHarvest.aspx", "Sell Harvest"));
                    items.Add(new NavItem("TractorRentals.aspx", "Tractor Rentals"));
                    items.Add(new NavItem("MyBookings.aspx", "My Bookings"));
                    items.Add(new NavItem("ProfileSettings.aspx", "Profile Settings"));
                    break;

                case "Buyer":
                    items.Add(new NavItem("BuyerDashboard.aspx", "Dashboard"));
                    items.Add(new NavItem("Marketplace.aspx", "Marketplace"));
                    items.Add(new NavItem("MyPurchases.aspx", "My Purchases"));
                    items.Add(new NavItem("OrderDetails.aspx", "Order Tracking"));
                    items.Add(new NavItem("ProfileSettings.aspx", "Profile Settings"));
                    break;

                case "Operator":
                    items.Add(new NavItem("TractorRentals.aspx", "Tractors & Machinery"));
                    items.Add(new NavItem("MyBookings.aspx", "My Bookings"));
                    items.Add(new NavItem("AgriServices.aspx", "Agri Services"));
                    items.Add(new NavItem("ProfileSettings.aspx", "Profile"));
                    break;

                case "Admin":
                    items.Add(new NavItem("AdminDashboard.aspx", "Dashboard"));
                    items.Add(new NavItem("AdminManagement.aspx", "Platform Management"));
                    items.Add(new NavItem("AdminAnalytics.aspx", "Analytics"));
                    items.Add(new NavItem("AdminSettings.aspx", "Settings"));
                    break;

                default:
                    items.Add(new NavItem("Default.aspx", "Home"));
                    items.Add(new NavItem("Marketplace.aspx", "Marketplace"));
                    items.Add(new NavItem("ApmcRates.aspx", "APMC Rates"));
                    items.Add(new NavItem("LandPortal.aspx", "Land Portal"));
                    items.Add(new NavItem("About.aspx", "About Us"));
                    items.Add(new NavItem("Contact.aspx", "Contact"));
                    break;
            }
            return items;
        }

        private string GetNotificationBellHtml()
        {
            return @"<div class='dropdown d-inline-block'>
                <a href='#' class='icon-circle-btn position-relative' data-bs-toggle='dropdown' aria-expanded='false' title='Notifications' style='background: #e2e8f0; color: #475569;'>
                    <i class='bi bi-bell-fill' style='font-size: 0.95rem;'></i>
                    <span class='position-absolute top-0 start-100 translate-middle p-1 bg-danger border border-light rounded-circle'></span>
                </a>
                <div class='dropdown-menu dropdown-menu-end shadow border-0 p-0' style='width: 320px; border-radius: 12px; overflow: hidden; z-index: 1050;'>
                    <div class='p-3 bg-light border-bottom d-flex justify-content-between align-items-center'>
                        <h6 class='mb-0 fw-bold text-dark' style='font-size: 0.95rem;'>Live Notifications</h6>
                        <span class='badge bg-success-subtle text-success'>3 New</span>
                    </div>
                    <div class='list-group list-group-flush' style='font-size: 0.85rem;'>
                        <a href='ApmcRates.aspx' class='list-group-item list-group-item-action p-3'>
                            <div class='d-flex align-items-center mb-1'>
                                <span class='badge bg-success-subtle text-success me-2'>APMC Rates</span>
                                <small class='text-muted'>12:45 PM</small>
                            </div>
                            <p class='mb-0 text-dark fw-semibold'>Highest Cotton rate ₹7,250/Q recorded at Gondal APMC yard.</p>
                        </a>
                        <a href='WeatherForecast.aspx' class='list-group-item list-group-item-action p-3'>
                            <div class='d-flex align-items-center mb-1'>
                                <span class='badge bg-primary-subtle text-primary me-2'>Weather</span>
                                <small class='text-muted'>Today</small>
                            </div>
                            <p class='mb-0 text-dark fw-semibold'>Rain probability 78% expected tomorrow. Complete groundnut irrigation.</p>
                        </a>
                        <a href='Marketplace.aspx' class='list-group-item list-group-item-action p-3'>
                            <div class='d-flex align-items-center mb-1'>
                                <span class='badge bg-warning-subtle text-warning me-2'>Market</span>
                                <small class='text-muted'>New Listing</small>
                            </div>
                            <p class='mb-0 text-dark fw-semibold'>Premium Groundnut (Grade A) listed by verified farmers in Rajkot.</p>
                        </a>
                    </div>
                </div>
            </div>";
        }

        private string GetProfileDropdownHtml(string activeRole, string userName)
        {
            string roleBadgeText = "Guest User";
            string roleBadgeClass = "text-muted";
            string displayName = "Guest User";
            string titleAttr = "Account";

            List<DropdownItem> menuItems = new List<DropdownItem>();

            switch (activeRole)
            {
                case "Farmer":
                    titleAttr = "Farmer Profile";
                    displayName = userName;
                    roleBadgeText = "Verified Farmer";
                    roleBadgeClass = "badge bg-success-subtle text-success";
                    menuItems.Add(new DropdownItem("FarmerDashboard.aspx", "Farmer Dashboard", "bi bi-speedometer2 text-success"));
                    menuItems.Add(new DropdownItem("FarmPlots.aspx", "My Farm & Plots", "bi bi-grid-fill text-success"));
                    menuItems.Add(new DropdownItem("MyCrops.aspx", "My Crops", "bi bi-flower1 text-success"));
                    menuItems.Add(new DropdownItem("ProfileSettings.aspx", "Profile & Settings", "bi bi-person-gear text-success"));
                    menuItems.Add(new DropdownItem("AdminLogin.aspx", "Admin Portal Login", "bi bi-shield-lock-fill text-danger"));
                    menuItems.Add(new DropdownItem(true));
                    menuItems.Add(new DropdownItem("Default.aspx?action=logout", "Logout", "bi bi-box-arrow-right", "text-danger"));
                    break;

                case "Buyer":
                    titleAttr = "Buyer Profile";
                    displayName = userName;
                    roleBadgeText = "Verified Buyer";
                    roleBadgeClass = "badge bg-primary-subtle text-primary";
                    menuItems.Add(new DropdownItem("BuyerDashboard.aspx", "Buyer Dashboard", "bi bi-speedometer2 text-success"));
                    menuItems.Add(new DropdownItem("MyPurchases.aspx", "My Purchases", "bi bi-bag-check text-success"));
                    menuItems.Add(new DropdownItem("Wishlist.aspx", "My Wishlist", "bi bi-heart text-danger"));
                    menuItems.Add(new DropdownItem("ProfileSettings.aspx", "Profile & Settings", "bi bi-person-gear text-success"));
                    menuItems.Add(new DropdownItem("AdminLogin.aspx", "Admin Portal Login", "bi bi-shield-lock-fill text-danger"));
                    menuItems.Add(new DropdownItem(true));
                    menuItems.Add(new DropdownItem("Default.aspx?action=logout", "Logout", "bi bi-box-arrow-right", "text-danger"));
                    break;

                case "Operator":
                    titleAttr = "Operator Profile";
                    displayName = userName;
                    roleBadgeText = "Machinery Operator";
                    roleBadgeClass = "badge bg-warning-subtle text-warning";
                    menuItems.Add(new DropdownItem("TractorRentals.aspx", "Tractors & Machinery", "bi bi-truck text-success"));
                    menuItems.Add(new DropdownItem("MyBookings.aspx", "My Bookings", "bi bi-calendar-check text-success"));
                    menuItems.Add(new DropdownItem("ProfileSettings.aspx", "Profile & Settings", "bi bi-person-gear text-success"));
                    menuItems.Add(new DropdownItem("AdminLogin.aspx", "Admin Portal Login", "bi bi-shield-lock-fill text-danger"));
                    menuItems.Add(new DropdownItem(true));
                    menuItems.Add(new DropdownItem("Default.aspx?action=logout", "Logout", "bi bi-box-arrow-right", "text-danger"));
                    break;

                case "Admin":
                    titleAttr = "Admin Profile";
                    displayName = !string.IsNullOrEmpty(userName) ? userName : "System Admin";
                    roleBadgeText = "Super Administrator";
                    roleBadgeClass = "badge bg-danger-subtle text-danger";
                    menuItems.Add(new DropdownItem("AdminDashboard.aspx", "Admin Dashboard", "bi bi-speedometer2 text-success"));
                    menuItems.Add(new DropdownItem("AdminManagement.aspx", "Platform Management", "bi bi-sliders text-success"));
                    menuItems.Add(new DropdownItem("AdminAnalytics.aspx", "Platform Analytics", "bi bi-bar-chart-line text-success"));
                    menuItems.Add(new DropdownItem(true));
                    menuItems.Add(new DropdownItem("Default.aspx?action=logout", "Logout", "bi bi-box-arrow-right", "text-danger"));
                    break;

                default:
                    titleAttr = "Account";
                    displayName = "Guest User";
                    roleBadgeText = "Browsing Public Mode";
                    roleBadgeClass = "small text-muted";
                    menuItems.Add(new DropdownItem("Login.aspx", "Login", "bi bi-box-arrow-in-right text-success"));
                    menuItems.Add(new DropdownItem("SelectRole.aspx", "Register (Choose Role)", "bi bi-person-plus-fill text-success"));
                    menuItems.Add(new DropdownItem("AdminLogin.aspx", "Admin Login", "bi bi-shield-lock-fill text-danger"));
                    menuItems.Add(new DropdownItem(true));
                    menuItems.Add(new DropdownItem("About.aspx", "About KhedutRise", "bi bi-info-circle text-secondary", "text-secondary"));
                    menuItems.Add(new DropdownItem("Contact.aspx", "Help & Support", "bi bi-headset text-secondary", "text-secondary"));
                    break;
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"<div class='dropdown d-inline-block'>
                <a href='#' class='icon-circle-btn' data-bs-toggle='dropdown' aria-expanded='false' title='{0}' style='background: #0b6028; color: #ffffff;'>
                    <i class='bi bi-person-fill' style='font-size: 1.1rem;'></i>
                </a>
                <ul class='dropdown-menu dropdown-menu-end shadow border-0 p-2' style='width: 230px; border-radius: 10px; z-index: 1050;'>
                    <li class='px-3 py-2 border-bottom mb-1'>
                        <strong class='d-block text-dark' style='font-size: 0.95rem;'>{1}</strong>
                        <span class='{2}'>{3}</span>
                    </li>",
                titleAttr,
                Server.HtmlEncode(displayName),
                roleBadgeClass,
                roleBadgeText);

            foreach (var item in menuItems)
            {
                if (item.IsDivider)
                {
                    sb.Append("<li><hr class='dropdown-divider'></li>");
                }
                else
                {
                    sb.AppendFormat("<li><a class='dropdown-item py-2 fw-semibold {0}' href='{1}'><i class='{2} me-2'></i> {3}</a></li>",
                        item.TextColor, item.Url, item.Icon, item.Title);
                }
            }

            sb.Append("</ul></div>");
            return sb.ToString();
        }
    }
}
