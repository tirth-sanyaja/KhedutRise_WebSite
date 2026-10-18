using System;
using System.IO;
using System.Web.UI;

namespace KhedutRise
{
    // Master page logic: Manages user roles, enforces route protection, and renders clean desktop & mobile navigation
    public partial class Site : System.Web.UI.MasterPage
    {
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

            // =========================================================================
            // ROUTE-LEVEL ACCESS CONTROL
            // =========================================================================

            // List of pages strictly exclusive to the Farmer role
            string[] farmerPages = {
                "FarmerDashboard.aspx", "FarmPlots.aspx", "MyCrops.aspx", "SellHarvest.aspx"
            };

            // List of pages strictly exclusive to the Buyer role
            string[] buyerPages = {
                "BuyerDashboard.aspx", "MyPurchases.aspx", "OrderDetails.aspx", "Wishlist.aspx"
            };

            // List of pages strictly exclusive to the Operator role
            string[] operatorPages = {
                "MyBookings.aspx"
            };

            // List of pages strictly exclusive to the Admin role
            string[] adminPages = {
                "AdminDashboard.aspx", "AdminManagement.aspx", "AdminAnalytics.aspx", "AdminSettings.aspx"
            };

            // Pages that require ANY authenticated role (Guest cannot proceed with actions)
            string[] authenticatedActionPages = {
                "BookingConfirm.aspx", "ProfileSettings.aspx"
            };

            // 1. PostBack Protection: Actions/mutations on role-exclusive routes require registration/login
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
            // Guest users are allowed to VIEW all 32 pages, but are notified of read-only mode on role-specific pages
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

        // Dynamically build single-line navigation matching active role
        private void BuildNavigation(string currentPage, string activeRole)
        {
            string navHtml = "";
            string authHtml = "";
            string userIconsHtml = "";

            string userName = Session["UserName"] != null ? Session["UserName"].ToString() : "User";

            if (activeRole == "Farmer")
            {
                // Active Role: Farmer Navigation
                navHtml = "<ul class='kr-nav-list'>" +
                    "<li><a href='FarmerDashboard.aspx' class='kr-nav-link " + (currentPage == "FarmerDashboard.aspx" ? "active" : "") + "'>Dashboard</a></li>" +
                    "<li><a href='MyCrops.aspx' class='kr-nav-link " + (currentPage == "MyCrops.aspx" ? "active" : "") + "'>My Crops</a></li>" +
                    "<li><a href='SellHarvest.aspx' class='kr-nav-link " + (currentPage == "SellHarvest.aspx" ? "active" : "") + "'>Sell Harvest</a></li>" +
                    "<li><a href='TractorRentals.aspx' class='kr-nav-link " + (currentPage == "TractorRentals.aspx" ? "active" : "") + "'>Tractor Rentals</a></li>" +
                    "<li><a href='MyBookings.aspx' class='kr-nav-link " + (currentPage == "MyBookings.aspx" ? "active" : "") + "'>My Bookings</a></li>" +
                    "<li><a href='ProfileSettings.aspx' class='kr-nav-link " + (currentPage == "ProfileSettings.aspx" ? "active" : "") + "'>Profile Settings</a></li>" +
                    "</ul>";

                authHtml = "<span class='d-none d-lg-inline-block text-dark fw-bold me-2' style='font-size: 0.9rem;'>Welcome, " + Server.HtmlEncode(userName) + " (Farmer)</span>" +
                           "<a href='Default.aspx?action=logout' class='btn-kr-logout' title='Log out of farmer account'>LOGOUT</a>";
                userIconsHtml = GetNotificationBellHtml() + GetProfileDropdownHtml(activeRole);
            }
            else if (activeRole == "Buyer")
            {
                // Active Role: Buyer Navigation
                navHtml = "<ul class='kr-nav-list'>" +
                    "<li><a href='BuyerDashboard.aspx' class='kr-nav-link " + (currentPage == "BuyerDashboard.aspx" ? "active" : "") + "'>Dashboard</a></li>" +
                    "<li><a href='Marketplace.aspx' class='kr-nav-link " + (currentPage == "Marketplace.aspx" ? "active" : "") + "'>Marketplace</a></li>" +
                    "<li><a href='MyPurchases.aspx' class='kr-nav-link " + (currentPage == "MyPurchases.aspx" ? "active" : "") + "'>My Purchases</a></li>" +
                    "<li><a href='OrderDetails.aspx' class='kr-nav-link " + (currentPage == "OrderDetails.aspx" ? "active" : "") + "'>Order Tracking</a></li>" +
                    "<li><a href='ProfileSettings.aspx' class='kr-nav-link " + (currentPage == "ProfileSettings.aspx" ? "active" : "") + "'>Profile Settings</a></li>" +
                    "</ul>";

                authHtml = "<span class='d-none d-lg-inline-block text-dark fw-bold me-2' style='font-size: 0.9rem;'>Welcome, " + Server.HtmlEncode(userName) + " (Buyer)</span>" +
                           "<a href='Default.aspx?action=logout' class='btn-kr-logout' title='Log out of buyer account'>LOGOUT</a>";
                userIconsHtml = GetNotificationBellHtml() + GetProfileDropdownHtml(activeRole);
            }
            else if (activeRole == "Operator")
            {
                // Active Role: Equipment Operator Navigation
                navHtml = "<ul class='kr-nav-list'>" +
                    "<li><a href='TractorRentals.aspx' class='kr-nav-link " + (currentPage == "TractorRentals.aspx" ? "active" : "") + "'>Tractors & Machinery</a></li>" +
                    "<li><a href='MyBookings.aspx' class='kr-nav-link " + (currentPage == "MyBookings.aspx" ? "active" : "") + "'>My Bookings</a></li>" +
                    "<li><a href='AgriServices.aspx' class='kr-nav-link " + (currentPage == "AgriServices.aspx" ? "active" : "") + "'>Agri Services</a></li>" +
                    "<li><a href='ProfileSettings.aspx' class='kr-nav-link " + (currentPage == "ProfileSettings.aspx" ? "active" : "") + "'>Profile</a></li>" +
                    "</ul>";

                authHtml = "<span class='d-none d-lg-inline-block text-dark fw-bold me-2' style='font-size: 0.9rem;'>Welcome, " + Server.HtmlEncode(userName) + " (Operator)</span>" +
                           "<a href='Default.aspx?action=logout' class='btn-kr-logout' title='Log out of operator account'>LOGOUT</a>";
                userIconsHtml = GetNotificationBellHtml() + GetProfileDropdownHtml(activeRole);
            }
            else if (activeRole == "Admin")
            {
                // Active Role: Admin Navigation
                navHtml = "<ul class='kr-nav-list'>" +
                    "<li><a href='AdminDashboard.aspx' class='kr-nav-link " + (currentPage == "AdminDashboard.aspx" ? "active" : "") + "'>Dashboard</a></li>" +
                    "<li><a href='AdminManagement.aspx' class='kr-nav-link " + (currentPage == "AdminManagement.aspx" ? "active" : "") + "'>Platform Management</a></li>" +
                    "<li><a href='AdminAnalytics.aspx' class='kr-nav-link " + (currentPage == "AdminAnalytics.aspx" ? "active" : "") + "'>Analytics</a></li>" +
                    "<li><a href='AdminSettings.aspx' class='kr-nav-link " + (currentPage == "AdminSettings.aspx" ? "active" : "") + "'>Settings</a></li>" +
                    "</ul>";

                authHtml = "<span class='d-none d-lg-inline-block text-dark fw-bold me-2' style='font-size: 0.9rem;'>Welcome, " + Server.HtmlEncode(userName) + " (Admin)</span>" +
                           "<a href='Default.aspx?action=logout' class='btn-kr-logout' title='Log out of admin session'>LOGOUT</a>";
                userIconsHtml = GetNotificationBellHtml() + GetProfileDropdownHtml(activeRole);
            }
            else
            {
                // Guest / Public Desktop Navigation
                navHtml = "<ul class='kr-nav-list'>" +
                    "<li><a href='Default.aspx' class='kr-nav-link " + (currentPage == "Default.aspx" ? "active" : "") + "'>Home</a></li>" +
                    "<li><a href='Marketplace.aspx' class='kr-nav-link " + (currentPage == "Marketplace.aspx" ? "active" : "") + "'>Marketplace</a></li>" +
                    "<li><a href='ApmcRates.aspx' class='kr-nav-link " + (currentPage == "ApmcRates.aspx" ? "active" : "") + "'>APMC Rates</a></li>" +
                    "<li><a href='LandPortal.aspx' class='kr-nav-link " + (currentPage == "LandPortal.aspx" ? "active" : "") + "'>Land Portal</a></li>" +
                    "<li><a href='About.aspx' class='kr-nav-link " + (currentPage == "About.aspx" ? "active" : "") + "'>About Us</a></li>" +
                    "<li><a href='Contact.aspx' class='kr-nav-link " + (currentPage == "Contact.aspx" ? "active" : "") + "'>Contact</a></li>" +
                    "</ul>";

                authHtml = "<a href='Login.aspx' class='btn-kr-login " + (currentPage == "Login.aspx" ? "active" : "") + "'>Login</a>" +
                           "<a href='SelectRole.aspx' class='btn-kr-register " + (currentPage.Contains("Register") || currentPage == "SelectRole.aspx" ? "active" : "") + "'>Register</a>";

                userIconsHtml = GetNotificationBellHtml() + GetProfileDropdownHtml(activeRole);
            }

            // Render controls into Master page placeholders
            phNavLinks.Controls.Add(new LiteralControl(navHtml));
            phNavAuth.Controls.Add(new LiteralControl(authHtml));
            if (!string.IsNullOrEmpty(userIconsHtml))
            {
                phUserIcons.Controls.Add(new LiteralControl(userIconsHtml));
            }
        }

        // Generate interactive Notifications Dropdown
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

        // Generate interactive User Profile Dropdown
        private string GetProfileDropdownHtml(string activeRole)
        {
            string userName = Session["UserName"] != null ? Session["UserName"].ToString() : "User";

            if (activeRole == "Farmer")
            {
                return @"<div class='dropdown d-inline-block'>
                    <a href='#' class='icon-circle-btn' data-bs-toggle='dropdown' aria-expanded='false' title='Farmer Profile' style='background: #0b6028; color: #ffffff;'>
                        <i class='bi bi-person-fill' style='font-size: 1.1rem;'></i>
                    </a>
                    <ul class='dropdown-menu dropdown-menu-end shadow border-0 p-2' style='width: 230px; border-radius: 10px; z-index: 1050;'>
                        <li class='px-3 py-2 border-bottom mb-1'>
                            <strong class='d-block text-dark' style='font-size: 0.95rem;'>" + System.Web.HttpUtility.HtmlEncode(userName) + @"</strong>
                            <span class='badge bg-success-subtle text-success'>Verified Farmer</span>
                        </li>
                        <li><a class='dropdown-item py-2 fw-semibold text-dark' href='FarmerDashboard.aspx'><i class='bi bi-speedometer2 me-2 text-success'></i> Farmer Dashboard</a></li>
                        <li><a class='dropdown-item py-2 fw-semibold text-dark' href='FarmPlots.aspx'><i class='bi bi-grid-fill me-2 text-success'></i> My Farm & Plots</a></li>
                        <li><a class='dropdown-item py-2 fw-semibold text-dark' href='MyCrops.aspx'><i class='bi bi-flower1 me-2 text-success'></i> My Crops</a></li>
                        <li><a class='dropdown-item py-2 fw-semibold text-dark' href='ProfileSettings.aspx'><i class='bi bi-person-gear me-2 text-success'></i> Profile & Settings</a></li>
                        <li><a class='dropdown-item py-2 fw-semibold text-dark' href='AdminLogin.aspx'><i class='bi bi-shield-lock-fill me-2 text-danger'></i> Admin Portal Login</a></li>
                        <li><hr class='dropdown-divider'></li>
                        <li><a class='dropdown-item py-2 fw-semibold text-danger' href='Default.aspx?action=logout'><i class='bi bi-box-arrow-right me-2'></i> Logout</a></li>
                    </ul>
                </div>";
            }
            else if (activeRole == "Buyer")
            {
                return @"<div class='dropdown d-inline-block'>
                    <a href='#' class='icon-circle-btn' data-bs-toggle='dropdown' aria-expanded='false' title='Buyer Profile' style='background: #0b6028; color: #ffffff;'>
                        <i class='bi bi-person-fill' style='font-size: 1.1rem;'></i>
                    </a>
                    <ul class='dropdown-menu dropdown-menu-end shadow border-0 p-2' style='width: 230px; border-radius: 10px; z-index: 1050;'>
                        <li class='px-3 py-2 border-bottom mb-1'>
                            <strong class='d-block text-dark' style='font-size: 0.95rem;'>" + System.Web.HttpUtility.HtmlEncode(userName) + @"</strong>
                            <span class='badge bg-primary-subtle text-primary'>Verified Buyer</span>
                        </li>
                        <li><a class='dropdown-item py-2 fw-semibold text-dark' href='BuyerDashboard.aspx'><i class='bi bi-speedometer2 me-2 text-success'></i> Buyer Dashboard</a></li>
                        <li><a class='dropdown-item py-2 fw-semibold text-dark' href='MyPurchases.aspx'><i class='bi bi-bag-check me-2 text-success'></i> My Purchases</a></li>
                        <li><a class='dropdown-item py-2 fw-semibold text-dark' href='Wishlist.aspx'><i class='bi bi-heart me-2 text-danger'></i> My Wishlist</a></li>
                        <li><a class='dropdown-item py-2 fw-semibold text-dark' href='ProfileSettings.aspx'><i class='bi bi-person-gear me-2 text-success'></i> Profile & Settings</a></li>
                        <li><a class='dropdown-item py-2 fw-semibold text-dark' href='AdminLogin.aspx'><i class='bi bi-shield-lock-fill me-2 text-danger'></i> Admin Portal Login</a></li>
                        <li><hr class='dropdown-divider'></li>
                        <li><a class='dropdown-item py-2 fw-semibold text-danger' href='Default.aspx?action=logout'><i class='bi bi-box-arrow-right me-2'></i> Logout</a></li>
                    </ul>
                </div>";
            }
            else if (activeRole == "Operator")
            {
                return @"<div class='dropdown d-inline-block'>
                    <a href='#' class='icon-circle-btn' data-bs-toggle='dropdown' aria-expanded='false' title='Operator Profile' style='background: #0b6028; color: #ffffff;'>
                        <i class='bi bi-person-fill' style='font-size: 1.1rem;'></i>
                    </a>
                    <ul class='dropdown-menu dropdown-menu-end shadow border-0 p-2' style='width: 220px; border-radius: 10px; z-index: 1050;'>
                        <li class='px-3 py-2 border-bottom mb-1'>
                            <strong class='d-block text-dark' style='font-size: 0.95rem;'>Dilip Parmar</strong>
                            <span class='badge bg-warning-subtle text-warning'>Machinery Operator</span>
                        </li>
                        <li><a class='dropdown-item py-2 fw-semibold text-dark' href='TractorRentals.aspx'><i class='bi bi-truck me-2 text-success'></i> Tractors & Machinery</a></li>
                        <li><a class='dropdown-item py-2 fw-semibold text-dark' href='MyBookings.aspx'><i class='bi bi-calendar-check me-2 text-success'></i> My Bookings</a></li>
                        <li><a class='dropdown-item py-2 fw-semibold text-dark' href='ProfileSettings.aspx'><i class='bi bi-person-gear me-2 text-success'></i> Profile & Settings</a></li>
                        <li><a class='dropdown-item py-2 fw-semibold text-dark' href='AdminLogin.aspx'><i class='bi bi-shield-lock-fill me-2 text-danger'></i> Admin Portal Login</a></li>
                        <li><hr class='dropdown-divider'></li>
                        <li><a class='dropdown-item py-2 fw-semibold text-danger' href='Default.aspx?action=logout'><i class='bi bi-box-arrow-right me-2'></i> Logout</a></li>
                    </ul>
                </div>";
            }
            else if (activeRole == "Admin")
            {
                return @"<div class='dropdown d-inline-block'>
                    <a href='#' class='icon-circle-btn' data-bs-toggle='dropdown' aria-expanded='false' title='Admin Profile' style='background: #0b6028; color: #ffffff;'>
                        <i class='bi bi-person-fill' style='font-size: 1.1rem;'></i>
                    </a>
                    <ul class='dropdown-menu dropdown-menu-end shadow border-0 p-2' style='width: 220px; border-radius: 10px; z-index: 1050;'>
                        <li class='px-3 py-2 border-bottom mb-1'>
                            <strong class='d-block text-dark' style='font-size: 0.95rem;'>System Admin</strong>
                            <span class='badge bg-danger-subtle text-danger'>Super Administrator</span>
                        </li>
                        <li><a class='dropdown-item py-2 fw-semibold text-dark' href='AdminDashboard.aspx'><i class='bi bi-speedometer2 me-2 text-success'></i> Admin Dashboard</a></li>
                        <li><a class='dropdown-item py-2 fw-semibold text-dark' href='AdminManagement.aspx'><i class='bi bi-sliders me-2 text-success'></i> Platform Management</a></li>
                        <li><a class='dropdown-item py-2 fw-semibold text-dark' href='AdminAnalytics.aspx'><i class='bi bi-bar-chart-line me-2 text-success'></i> Platform Analytics</a></li>
                        <li><hr class='dropdown-divider'></li>
                        <li><a class='dropdown-item py-2 fw-semibold text-danger' href='Default.aspx?action=logout'><i class='bi bi-box-arrow-right me-2'></i> Logout</a></li>
                    </ul>
                </div>";
            }
            else
            {
                // Guest User Dropdown: No blind registration redirect
                return @"<div class='dropdown d-inline-block'>
                    <a href='#' class='icon-circle-btn' data-bs-toggle='dropdown' aria-expanded='false' title='Account' style='background: #0b6028; color: #ffffff;'>
                        <i class='bi bi-person-fill' style='font-size: 1.1rem;'></i>
                    </a>
                    <ul class='dropdown-menu dropdown-menu-end shadow border-0 p-2' style='width: 220px; border-radius: 10px; z-index: 1050;'>
                        <li class='px-3 py-2 border-bottom mb-1'>
                            <strong class='d-block text-dark' style='font-size: 0.95rem;'>Guest User</strong>
                            <small class='text-muted'>Browsing Public Mode</small>
                        </li>
                        <li><a class='dropdown-item py-2 fw-semibold text-dark' href='Login.aspx'><i class='bi bi-box-arrow-in-right me-2 text-success'></i> Login</a></li>
                        <li><a class='dropdown-item py-2 fw-semibold text-dark' href='SelectRole.aspx'><i class='bi bi-person-plus-fill me-2 text-success'></i> Register (Choose Role)</a></li>
                        <li><a class='dropdown-item py-2 fw-semibold text-dark' href='AdminLogin.aspx'><i class='bi bi-shield-lock-fill me-2 text-danger'></i> Admin Login</a></li>
                        <li><hr class='dropdown-divider'></li>
                        <li><a class='dropdown-item py-2 text-secondary' href='About.aspx'><i class='bi bi-info-circle me-2'></i> About KhedutRise</a></li>
                        <li><a class='dropdown-item py-2 text-secondary' href='Contact.aspx'><i class='bi bi-headset me-2'></i> Help & Support</a></li>
                    </ul>
                </div>";
            }
        }
    }
}
