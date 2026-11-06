using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
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
                authHtml = "<a href='Default.aspx?action=logout' class='btn-kr-logout' title='Log out of account'>LOGOUT</a>";
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
            // Server-side check for previously read notifications via cookie
            HashSet<string> readIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            try
            {
                HttpCookie notifCookie = Request.Cookies["kr_read_notifs"];
                if (notifCookie != null && !string.IsNullOrEmpty(notifCookie.Value))
                {
                    string decoded = Server.UrlDecode(notifCookie.Value);
                    if (decoded.StartsWith("[") && decoded.EndsWith("]"))
                    {
                        decoded = decoded.Trim('[', ']');
                        string[] parts = decoded.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string p in parts)
                        {
                            string id = p.Trim('"', ' ', '\'');
                            if (!string.IsNullOrEmpty(id)) readIds.Add(id);
                        }
                    }
                    else
                    {
                        string[] parts = decoded.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string p in parts)
                        {
                            string id = p.Trim('"', ' ', '\'');
                            if (!string.IsNullOrEmpty(id)) readIds.Add(id);
                        }
                    }
                }
            }
            catch { }

            // Define all available platform notifications
            var allNotifs = new[]
            {
                new {
                    Id = "kr-notif-1",
                    Tag = "APMC Rates",
                    TagBg = "#dcfce7",
                    TagColor = "#15803d",
                    Time = "12:45 PM",
                    Icon = "bi-graph-up-arrow",
                    IconBg = "#dcfce7",
                    IconColor = "#16a34a",
                    Title = "Cotton hits ₹7,250/Q",
                    Body = "Highest rate recorded at Gondal APMC yard today.",
                    LinkUrl = "ApmcRates.aspx",
                    LinkText = "View APMC Rates"
                },
                new {
                    Id = "kr-notif-2",
                    Tag = "Weather",
                    TagBg = "#dbeafe",
                    TagColor = "#1d4ed8",
                    Time = "Today",
                    Icon = "bi-cloud-rain-fill",
                    IconBg = "#dbeafe",
                    IconColor = "#2563eb",
                    Title = "Rain probability 78%",
                    Body = "Rain expected tomorrow — complete groundnut irrigation early.",
                    LinkUrl = "WeatherForecast.aspx",
                    LinkText = "Check Forecast"
                },
                new {
                    Id = "kr-notif-3",
                    Tag = "New Listing",
                    TagBg = "#fef9c3",
                    TagColor = "#92400e",
                    Time = "2 hrs ago",
                    Icon = "bi-shop-window",
                    IconBg = "#fef9c3",
                    IconColor = "#ca8a04",
                    Title = "Premium Groundnut Grade A",
                    Body = "Newly listed harvest available by verified farmers in Rajkot.",
                    LinkUrl = "Marketplace.aspx",
                    LinkText = "Browse Market"
                }
            };

            // Filter out any notification that has already been seen / read!
            var unreadNotifs = allNotifs.Where(n => !readIds.Contains(n.Id)).ToList();
            int unreadCount = unreadNotifs.Count;

            StringBuilder itemsHtml = new StringBuilder();
            foreach (var n in unreadNotifs)
            {
                itemsHtml.AppendFormat(@"
                    <div class='notif-item unread d-flex gap-3 align-items-start px-3 py-3'
                         data-notif-id='{0}'
                         onclick='krReadNotif(event, this)'
                         title='Click to dismiss notification'>
                        <div class='notif-icon-box' style='background:{1};color:{2};flex-shrink:0;'>
                            <i class='bi {3}'></i>
                        </div>
                        <div style='min-width:0;flex:1;'>
                            <div class='d-flex justify-content-between align-items-center mb-1'>
                                <span class='notif-tag' style='background:{4};color:{5};'>{6}</span>
                                <small class='text-muted notif-time'>{7}</small>
                            </div>
                            <p class='notif-text mb-1'><strong>{8}</strong> — {9}</p>
                            <a href='{10}' class='notif-action-link' onclick='krNavigate(event, ""{0}"", ""{10}"")'>
                                {11} <i class='bi bi-arrow-right-short'></i>
                            </a>
                        </div>
                        <div class='d-flex flex-column align-items-end gap-2' style='flex-shrink:0;'>
                            <button type='button' class='notif-dismiss-btn' title='Remove notification'
                                    onclick='krReadNotif(event, this.closest("".notif-item""))'>
                                <i class='bi bi-x'></i>
                            </button>
                            <span class='notif-unread-dot'></span>
                        </div>
                    </div>",
                    n.Id, n.IconBg, n.IconColor, n.Icon,
                    n.TagBg, n.TagColor, n.Tag, n.Time,
                    n.Title, n.Body, n.LinkUrl, n.LinkText);
            }

            string badgeStyle = unreadCount > 0 ? "" : "display:none;";
            string emptyStyle = unreadCount == 0 ? "" : "display:none;";
            string bellAnimation = unreadCount > 0 ? "running" : "paused";
            string subtitleText = unreadCount > 0 
                ? (unreadCount + " unread update" + (unreadCount > 1 ? "s" : ""))
                : "No new notifications";

            StringBuilder sb = new StringBuilder();
            sb.Append(@"<div class='dropdown d-inline-block notif-wrapper'>
                <a href='#' class='notif-bell-btn position-relative' data-bs-toggle='dropdown' data-bs-auto-close='outside' aria-expanded='false' title='Notifications' id='notifBellBtn'>
                    <i class='bi bi-bell-fill notif-bell-icon' style='animation-play-state: ").Append(bellAnimation).Append(@";'></i>
                    <span class='notif-count-badge' id='notifCountBadge' style='").Append(badgeStyle).Append(@"'>").Append(unreadCount).Append(@"</span>
                </a>
                <div class='dropdown-menu dropdown-menu-end notif-panel p-0 shadow-lg border-0' id='notifDropdown'>
                    <!-- Panel Header -->
                    <div class='notif-panel-header d-flex justify-content-between align-items-center px-3 py-3'>
                        <div class='d-flex align-items-center gap-2'>
                            <div class='notif-header-icon'>
                                <i class='bi bi-bell-fill'></i>
                            </div>
                            <div>
                                <h6 class='mb-0 fw-bold text-dark' style='font-size:0.92rem;'>Notifications</h6>
                                <small class='text-muted' id='notifSubtitle' style='font-size:0.72rem;'>").Append(subtitleText).Append(@"</small>
                            </div>
                        </div>
                        <div class='d-flex align-items-center gap-2'>
                            <span class='badge rounded-pill px-2 py-1 fw-bold' id='notifHeaderBadge' style='background:#dcfce7;color:#15803d;font-size:0.72rem;").Append(badgeStyle).Append(@"'>").Append(unreadCount).Append(@" New</span>
                            <button type='button' class='btn btn-sm btn-link p-0 text-decoration-none text-muted' onclick='krMarkAllRead(event)' title='Clear all notifications' style='font-size:0.8rem; font-weight:600;'>
                                Clear All
                            </button>
                        </div>
                    </div>
                    <!-- Notification Items -->
                    <div class='notif-list' id='notifList'>
                        ").Append(itemsHtml.ToString()).Append(@"
                        <!-- Empty state -->
                        <div id='notifEmptyState' style='").Append(emptyStyle).Append(@"' class='text-center py-4 px-3'>
                            <div class='rounded-circle d-inline-flex align-items-center justify-content-center mb-2' style='width: 44px; height: 44px; background: #f0fdf4; color: #16a34a;'>
                                <i class='bi bi-check2-circle fs-4'></i>
                            </div>
                            <h6 class='fw-bold text-dark mb-1' style='font-size:0.88rem;'>All Caught Up!</h6>
                            <p class='text-muted small mb-0'>No new notifications at this time.</p>
                        </div>
                    </div>
                    <!-- Panel Footer -->
                    <div class='notif-panel-footer text-center py-2 px-3'>
                        <a href='Default.aspx' class='text-decoration-none fw-bold' style='color:#15803d;font-size:0.82rem;'>
                            <i class='bi bi-shield-check me-1'></i>KhedutRise Live Updates
                        </a>
                    </div>
                </div>
            </div>

            <script>
            (function() {
                var STORAGE_KEY = 'kr_read_notifs';

                function getRead() {
                    try {
                        var val = localStorage.getItem(STORAGE_KEY);
                        return val ? JSON.parse(val) : [];
                    } catch(e) { return []; }
                }

                function saveRead(arr) {
                    try {
                        localStorage.setItem(STORAGE_KEY, JSON.stringify(arr));
                    } catch(e) {}
                    try {
                        document.cookie = 'kr_read_notifs=' + encodeURIComponent(JSON.stringify(arr)) + '; path=/; max-age=31536000';
                    } catch(e) {}
                }

                function syncUI() {
                    var read = getRead();
                    var items = document.querySelectorAll('#notifList .notif-item[data-notif-id]');
                    var unread = 0;

                    items.forEach(function(el) {
                        var id = el.getAttribute('data-notif-id');
                        if (read.indexOf(id) !== -1) {
                            el.remove();
                        } else {
                            unread++;
                        }
                    });

                    var badge = document.getElementById('notifCountBadge');
                    var hBadge = document.getElementById('notifHeaderBadge');
                    var subtitle = document.getElementById('notifSubtitle');
                    var empty = document.getElementById('notifEmptyState');
                    var bell = document.getElementById('notifBellBtn');

                    if (badge) {
                        badge.textContent = unread;
                        badge.style.display = unread > 0 ? '' : 'none';
                    }
                    if (hBadge) {
                        hBadge.textContent = unread + ' New';
                        hBadge.style.display = unread > 0 ? '' : 'none';
                    }
                    if (subtitle) {
                        subtitle.textContent = unread > 0 
                            ? (unread + ' unread update' + (unread > 1 ? 's' : ''))
                            : 'No new notifications';
                    }
                    if (bell) {
                        var icon = bell.querySelector('.notif-bell-icon');
                        if (icon) icon.style.animationPlayState = unread > 0 ? 'running' : 'paused';
                    }
                    if (empty) {
                        empty.style.display = unread === 0 ? '' : 'none';
                    }
                }

                // Click handler for dismissing/reading an individual notification
                window.krReadNotif = function(e, el) {
                    if (e) {
                        e.preventDefault();
                        e.stopPropagation();
                    }
                    if (!el) return;
                    var id = el.getAttribute('data-notif-id');
                    var read = getRead();
                    if (read.indexOf(id) === -1) {
                        read.push(id);
                        saveRead(read);
                    }

                    // Smooth fade out and shrink
                    el.style.transition = 'opacity 0.2s ease, transform 0.2s ease, max-height 0.25s ease, padding 0.25s ease';
                    el.style.overflow = 'hidden';
                    el.style.opacity = '0';
                    el.style.transform = 'translateX(20px)';
                    el.style.maxHeight = el.scrollHeight + 'px';
                    setTimeout(function() {
                        el.style.maxHeight = '0';
                        el.style.paddingTop = '0';
                        el.style.paddingBottom = '0';
                        el.style.marginTop = '0';
                        el.style.marginBottom = '0';
                        el.style.border = 'none';
                    }, 20);
                    setTimeout(function() {
                        el.remove();
                        syncUI();
                    }, 260);
                };

                // Link click navigation with auto mark as read
                window.krNavigate = function(e, id, url) {
                    if (e) {
                        e.stopPropagation();
                    }
                    var read = getRead();
                    if (read.indexOf(id) === -1) {
                        read.push(id);
                        saveRead(read);
                    }
                    window.location.href = url;
                };

                // Mark all notifications as read
                window.krMarkAllRead = function(e) {
                    if (e) {
                        e.preventDefault();
                        e.stopPropagation();
                    }
                    var items = document.querySelectorAll('#notifList .notif-item[data-notif-id]');
                    var read = getRead();
                    items.forEach(function(el) {
                        var id = el.getAttribute('data-notif-id');
                        if (read.indexOf(id) === -1) read.push(id);
                        el.style.transition = 'opacity 0.2s ease, transform 0.2s ease, max-height 0.2s ease';
                        el.style.opacity = '0';
                        el.style.transform = 'scale(0.95)';
                    });
                    saveRead(read);
                    setTimeout(function() {
                        items.forEach(function(el) { el.remove(); });
                        syncUI();
                    }, 220);
                };

                if (document.readyState === 'loading') {
                    document.addEventListener('DOMContentLoaded', syncUI);
                } else {
                    syncUI();
                }
            })();
            </script>");

            return sb.ToString();
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
