<%@ Page Title="Platform Analytics" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="AdminAnalytics.aspx.cs" Inherits="KhedutRise.AdminAnalytics" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .admin-sidebar {
            background: linear-gradient(180deg, #0f172a 0%, #1e293b 100%);
            border-radius: 16px;
            box-shadow: 0 10px 25px -5px rgba(15, 23, 42, 0.3);
        }
        .admin-nav-item {
            color: #94a3b8;
            font-weight: 500;
            font-size: 0.92rem;
            padding: 0.75rem 1rem;
            border-radius: 10px;
            display: flex;
            align-items: center;
            text-decoration: none;
            transition: all 0.2s ease;
            position: relative;
        }
        .admin-nav-item:hover {
            color: #ffffff;
            background: rgba(255, 255, 255, 0.08);
            transform: translateX(4px);
        }
        .admin-nav-item.active {
            color: #4ade80 !important;
            background: rgba(34, 197, 94, 0.15);
            font-weight: 700;
        }
        .admin-nav-item.active::before {
            content: '';
            position: absolute;
            left: 0;
            top: 15%;
            bottom: 15%;
            width: 4px;
            background: #22c55e;
            border-radius: 0 4px 4px 0;
        }
        .analytics-kpi-card {
            background: #ffffff;
            border: 1px solid #e2e8f0;
            border-radius: 14px;
            padding: 1.35rem 1.25rem;
            box-shadow: 0 2px 4px rgba(0,0,0,0.02);
            transition: all 0.2s ease;
        }
        .analytics-kpi-card:hover {
            transform: translateY(-3px);
            box-shadow: 0 12px 20px -8px rgba(0,0,0,0.08);
            border-color: #cbd5e1;
        }
    </style>

    <!-- Admin Platform Analytics -->
    <div class="kr-page-container py-4">
        <!--Header -->
        <div class="d-flex flex-wrap justify-content-between align-items-center mb-4 pb-2 border-bottom">
            <div>
                <small class="text-muted"><a href="Default.aspx" class="text-decoration-none text-muted">Home</a> / <a href="AdminDashboard.aspx" class="text-decoration-none text-muted">Admin Portal</a> / Platform Analytics</small>
                <h2 class="fw-bold mt-1 text-dark" style="font-size: 1.85rem; letter-spacing: -0.5px;">KhedutRise Telemetry & Business Analytics</h2>
                <p class="text-muted small mb-0">Real-time database aggregated metrics, market volume, and financial indicators</p>
            </div>
            <div class="mt-2 mt-md-0 d-flex gap-2 align-items-center">
                <span class="badge bg-success-subtle text-success border border-success-subtle px-3 py-2 fw-semibold rounded-pill">
                    <i class="bi bi-circle-fill me-1 small"></i> Live Database Sync
                </span>
                <span class="badge bg-white text-secondary border px-3 py-2 fw-semibold rounded-pill shadow-sm">
                    <asp:Literal ID="litGeneratedTime" runat="server"></asp:Literal>
                </span>
            </div>
        </div>

        <div class="row g-4">
            <!-- LEFT: Modern Enterprise Sidebar -->
            <div class="col-lg-3">
                <div class="admin-sidebar p-4 h-100 d-flex flex-column">
                    <!-- Brand Section -->
                    <div class="d-flex align-items-center mb-4 pb-3 border-bottom border-secondary border-opacity-25">
                        <div class="rounded-circle d-flex align-items-center justify-content-center me-3" style="width: 44px; height: 44px; background: rgba(34, 197, 94, 0.2); color: #4ade80;">
                            <i class="bi bi-shield-check fs-4"></i>
                        </div>
                        <div>
                            <h6 class="fw-bold text-white mb-0" style="letter-spacing: 0.5px;"><span style="color: #4ade80;">Khedut</span><span style="color: #22c55e;">Rise</span> Pro</h6>
                            <small class="text-success fw-semibold" style="font-size: 0.75rem;">Super Administrator</small>
                        </div>
                    </div>

                    <!-- Navigation Links Group -->
                    <div class="mb-2">
                        <small class="text-uppercase fw-bold text-muted mb-2 d-block" style="font-size: 0.7rem; letter-spacing: 1px;">Main Console</small>
                    </div>
                    <nav class="d-flex flex-column gap-2 mb-4">
                        <a href="AdminDashboard.aspx" class="admin-nav-item">
                            <i class="bi bi-grid-1x2-fill me-3 fs-5"></i>
                            <span>Dashboard Overview</span>
                        </a>
                        <a href="AdminManagement.aspx?tab=Farmers" class="admin-nav-item">
                            <i class="bi bi-people-fill me-3 fs-5"></i>
                            <span>Farmers Directory</span>
                        </a>
                        <a href="AdminManagement.aspx?tab=Buyers" class="admin-nav-item">
                            <i class="bi bi-shop me-3 fs-5"></i>
                            <span>Buyers & Traders</span>
                        </a>
                        <a href="AdminManagement.aspx?tab=Moderation" class="admin-nav-item">
                            <i class="bi bi-shield-lock-fill me-3 fs-5"></i>
                            <span>All Moderation</span>
                        </a>
                    </nav>

                    <div class="mb-2">
                        <small class="text-uppercase fw-bold text-muted mb-2 d-block" style="font-size: 0.7rem; letter-spacing: 1px;">Analytics & System</small>
                    </div>
                    <nav class="d-flex flex-column gap-2 mb-auto">
                        <a href="AdminAnalytics.aspx" class="admin-nav-item active">
                            <i class="bi bi-graph-up-arrow me-3 fs-5"></i>
                            <span>Telemetry & Reports</span>
                        </a>
                        <a href="AdminSettings.aspx" class="admin-nav-item">
                            <i class="bi bi-sliders me-3 fs-5"></i>
                            <span>Platform Settings</span>
                        </a>
                    </nav>

                    <!-- Sidebar Footer Admin Identity -->
                    <div class="mt-4 pt-3 border-top border-secondary border-opacity-25">
                        <div class="d-flex align-items-center justify-content-between">
                            <div class="d-flex align-items-center">
                                <div class="rounded-circle bg-success text-white fw-bold d-flex align-items-center justify-content-center me-2" style="width: 32px; height: 32px; font-size: 0.8rem;">
                                    SA
                                </div>
                                <div style="line-height: 1.2;">
                                    <span class="d-block text-white small fw-bold">System Admin</span>
                                    <span class="text-muted" style="font-size: 0.72rem;">admin@khedutrise.com</span>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <!-- RIGHT: Metrics & Visualizations -->
            <div class="col-lg-9">
                <!-- 4 Top Cards (Live Database Aggregates via CTE) -->
                <div class="row g-3 mb-4">
                    <div class="col-6 col-md-3">
                        <div class="analytics-kpi-card h-100">
                            <div class="d-flex justify-content-between align-items-center mb-1">
                                <small class="text-muted fw-bold" style="font-size: 0.72rem; letter-spacing: 0.5px;">TOTAL ECOSYSTEM GMV</small>
                                <i class="bi bi-currency-rupee text-success fs-5"></i>
                            </div>
                            <h4 class="fw-bold mt-2 mb-0" style="color: #0b6028; font-size: 1.5rem;">
                                <asp:Literal ID="litTotalGmv" runat="server">₹0</asp:Literal>
                            </h4>
                            <small class="text-muted mt-1 d-block" style="font-size: 0.75rem;">Orders Transaction Value</small>
                        </div>
                    </div>
                    <div class="col-6 col-md-3">
                        <div class="analytics-kpi-card h-100">
                            <div class="d-flex justify-content-between align-items-center mb-1">
                                <small class="text-muted fw-bold" style="font-size: 0.72rem; letter-spacing: 0.5px;">ORDERS PLACED</small>
                                <i class="bi bi-box-seam text-primary fs-5"></i>
                            </div>
                            <h4 class="fw-bold text-dark mt-2 mb-0" style="font-size: 1.5rem;">
                                <asp:Literal ID="litTotalOrders" runat="server">0</asp:Literal>
                            </h4>
                            <small class="text-muted mt-1 d-block" style="font-size: 0.75rem;">
                                <asp:Literal ID="litActiveOrdersCount" runat="server">0 In-Flight</asp:Literal>
                            </small>
                        </div>
                    </div>
                    <div class="col-6 col-md-3">
                        <div class="analytics-kpi-card h-100">
                            <div class="d-flex justify-content-between align-items-center mb-1">
                                <small class="text-muted fw-bold" style="font-size: 0.72rem; letter-spacing: 0.5px;">TOTAL ACCOUNTS</small>
                                <i class="bi bi-people-fill text-info fs-5"></i>
                            </div>
                            <h4 class="fw-bold mt-2 mb-0" style="color: #0284c7; font-size: 1.5rem;">
                                <asp:Literal ID="litTotalUsers" runat="server">0</asp:Literal>
                            </h4>
                            <small class="text-muted mt-1 d-block" style="font-size: 0.75rem;">Farmers & Buyers</small>
                        </div>
                    </div>
                    <div class="col-6 col-md-3">
                        <div class="analytics-kpi-card h-100">
                            <div class="d-flex justify-content-between align-items-center mb-1">
                                <small class="text-muted fw-bold" style="font-size: 0.72rem; letter-spacing: 0.5px;">RENTALS REVENUE</small>
                                <i class="bi bi-truck text-warning fs-5"></i>
                            </div>
                            <h4 class="fw-bold mt-2 mb-0" style="color: #ea580c; font-size: 1.5rem;">
                                <asp:Literal ID="litTotalRentals" runat="server">₹0</asp:Literal>
                            </h4>
                            <small class="text-muted mt-1 d-block" style="font-size: 0.75rem;">
                                <asp:Literal ID="litBookingsCount" runat="server">0 Bookings</asp:Literal>
                            </small>
                        </div>
                    </div>
                </div>

                <!-- User Ecosystem Distribution Progress Bar -->
                <div class="card card-custom p-4 mb-4 border shadow-sm">
                    <div class="d-flex justify-content-between align-items-center mb-2">
                        <h6 class="fw-bold text-dark mb-0"><i class="bi bi-pie-chart-fill me-2 text-primary"></i>User Ecosystem Demographics</h6>
                        <small class="text-muted">Total: <asp:Literal ID="litDemographicsTotal" runat="server">0</asp:Literal> Verified Users</small>
                    </div>
                    <div class="progress" style="height: 12px; border-radius: 6px;">
                        <asp:Literal ID="litProgressBar" runat="server"></asp:Literal>
                    </div>
                    <div class="d-flex flex-wrap gap-4 mt-3 small">
                        <div>
                            <span class="d-inline-block rounded-circle me-1" style="width: 10px; height: 10px; background-color: #16a34a;"></span>
                            <strong>Farmers:</strong> <asp:Literal ID="litFarmerCountText" runat="server">0</asp:Literal>
                        </div>
                        <div>
                            <span class="d-inline-block rounded-circle me-1" style="width: 10px; height: 10px; background-color: #2563eb;"></span>
                            <strong>Buyers & Traders:</strong> <asp:Literal ID="litBuyerCountText" runat="server">0</asp:Literal>
                        </div>
                        <div>
                            <span class="d-inline-block rounded-circle me-1" style="width: 10px; height: 10px; background-color: #f59e0b;"></span>
                            <strong>Machinery Operators:</strong> <asp:Literal ID="litOperatorCountText" runat="server">0</asp:Literal>
                        </div>
                    </div>
                </div>

                <!-- Crop Marketplace Categories Breakdown (SQL Window Function ROW_NUMBER) -->
                <div class="card card-custom p-4 mb-4 border shadow-sm">
                    <div class="d-flex justify-content-between align-items-center mb-3">
                        <div>
                            <h5 class="fw-bold mb-0 text-dark">Crop Category Analytics (SQL Window Function Analysis)</h5>
                            <small class="text-muted">Aggregated listing volume, inventory in Quintals, and average market price</small>
                        </div>
                        <span class="badge bg-light text-dark border px-2 py-1 small">Ranked by Volume</span>
                    </div>

                    <div class="table-responsive">
                        <asp:Repeater ID="rptCategoryBreakdown" runat="server">
                            <HeaderTemplate>
                                <table class="table table-hover align-middle mb-0">
                                    <thead class="text-muted bg-light" style="font-size: 0.75rem; letter-spacing: 0.5px;">
                                        <tr>
                                            <th class="py-2 ps-3">RANK</th>
                                            <th class="py-2">COMMODITY CATEGORY</th>
                                            <th class="py-2">ACTIVE LISTINGS</th>
                                            <th class="py-2">TOTAL VOLUME</th>
                                            <th class="py-2 text-end pe-3">AVG PRICE / QUINTAL</th>
                                        </tr>
                                    </thead>
                                    <tbody style="font-size: 0.92rem;">
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td class="ps-3 py-3">
                                        <span class="badge bg-dark-subtle text-dark border rounded-circle" style="width: 24px; height: 24px; display: inline-flex; align-items: center; justify-content: center;">
                                            <%# Eval("PopularityRank") %>
                                        </span>
                                    </td>
                                    <td>
                                        <strong class="text-dark"><%# Eval("Category") %></strong>
                                    </td>
                                    <td>
                                        <span class="badge bg-success-subtle text-success border border-success-subtle px-2 py-1 fw-bold">
                                            <%# Eval("ListingCount") %> Listings
                                        </span>
                                    </td>
                                    <td>
                                        <strong class="text-dark"><%# Convert.ToDecimal(Eval("TotalQuantity")).ToString("N0") %></strong> Quintals
                                    </td>
                                    <td class="text-end pe-3">
                                        <strong style="color: #0b6028; font-size: 1rem;">₹<%# Convert.ToDecimal(Eval("AvgPrice")).ToString("N0") %></strong>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                    </tbody>
                                </table>
                            </FooterTemplate>
                        </asp:Repeater>
                    </div>
                </div>

                <!-- Recent Live Transactions Table -->
                <div class="card card-custom p-4 border shadow-sm">
                    <div class="d-flex justify-content-between align-items-center mb-3">
                        <div>
                            <h5 class="fw-bold mb-0 text-dark">Recent Platform Orders & Escrow Pipeline</h5>
                            <small class="text-muted">Direct transaction telemetry between Buyers and Farmers</small>
                        </div>
                        <a href="AdminManagement.aspx?tab=Buyers" class="small fw-bold text-success text-decoration-none">Manage Orders <i class="bi bi-arrow-right"></i></a>
                    </div>

                    <div class="table-responsive">
                        <asp:Repeater ID="rptRecentOrders" runat="server">
                            <HeaderTemplate>
                                <table class="table table-hover align-middle mb-0">
                                    <thead class="text-muted bg-light" style="font-size: 0.75rem; letter-spacing: 0.5px;">
                                        <tr>
                                            <th class="py-2 ps-3">ORDER ID</th>
                                            <th class="py-2">COMMODITY</th>
                                            <th class="py-2">BUYER</th>
                                            <th class="py-2">FARMER</th>
                                            <th class="py-2">TOTAL VALUE</th>
                                            <th class="py-2 text-end pe-3">STATUS</th>
                                        </tr>
                                    </thead>
                                    <tbody style="font-size: 0.92rem;">
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td class="ps-3 py-3"><strong class="text-dark">#<%# Eval("OrderId") %></strong></td>
                                    <td><%# Eval("Commodity") %> (<%# Eval("Quantity") %>)</td>
                                    <td><%# Eval("BuyerName") %></td>
                                    <td><%# Eval("FarmerName") %></td>
                                    <td><strong style="color: #16a34a;">₹<%# Convert.ToDecimal(Eval("TotalPrice")).ToString("N0") %></strong></td>
                                    <td class="text-end pe-3">
                                        <span class='badge <%# Eval("Status").ToString() == "Delivered" ? "bg-success-subtle text-success border border-success-subtle" : (Eval("Status").ToString() == "In Transit" ? "bg-primary-subtle text-primary border border-primary-subtle" : "bg-warning-subtle text-warning border border-warning-subtle") %> fw-bold'>
                                            <%# Eval("Status") %>
                                        </span>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                    </tbody>
                                </table>
                            </FooterTemplate>
                        </asp:Repeater>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
