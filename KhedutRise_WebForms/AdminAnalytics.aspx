<%@ Page Title="Platform Analytics" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="AdminAnalytics.aspx.cs" Inherits="KhedutRise.AdminAnalytics" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        /* Shared Enterprise Admin Panel Styles */
        .admin-sidebar {
            background: #0f172a;
            color: #ffffff;
            border-right: 1px solid #1e293b;
        }
        .admin-sidebar-brand {
            display: flex;
            align-items: center;
            padding-bottom: 1.25rem;
            margin-bottom: 1.25rem;
            border-bottom: 1px solid #1e293b;
        }
        .admin-sidebar-section-title {
            color: #475569;
            font-size: 0.68rem;
            font-weight: 700;
            text-transform: uppercase;
            letter-spacing: 1.2px;
            margin-bottom: 0.5rem;
            padding-left: 0.5rem;
        }
        .admin-nav-item,
        .admin-sidebar a.d-block {
            color: #94a3b8 !important;
            font-weight: 500;
            font-size: 0.88rem;
            padding: 0.68rem 0.85rem;
            border-radius: 8px;
            display: flex;
            align-items: center;
            text-decoration: none !important;
            transition: all 0.15s ease-in-out;
            margin-bottom: 0.25rem;
            border: 1px solid transparent;
        }
        .admin-nav-item:hover,
        .admin-sidebar a.d-block:hover {
            color: #ffffff !important;
            background: rgba(255, 255, 255, 0.06);
        }
        .admin-nav-item.active,
        .admin-sidebar a.d-block.fw-bold {
            color: #22c55e !important;
            background: rgba(34, 197, 94, 0.12) !important;
            border-color: rgba(34, 197, 94, 0.25) !important;
            font-weight: 600 !important;
        }
        .admin-nav-item i,
        .admin-sidebar a.d-block i {
            font-size: 1.15rem;
            margin-right: 0.75rem;
            display: inline-flex;
            align-items: center;
            justify-content: center;
            width: 22px;
        }
        .admin-sidebar-footer {
            margin-top: auto;
            padding-top: 1.25rem;
            border-top: 1px solid #1e293b;
        }

        /* Top Header Bar */
        .admin-header {
            display: flex;
            flex-wrap: wrap;
            justify-content: space-between;
            align-items: center;
            gap: 1rem;
            margin-bottom: 1.75rem;
            padding-bottom: 1.25rem;
            border-bottom: 1px solid #e2e8f0;
        }
        .admin-title {
            font-size: 1.75rem;
            font-weight: 800;
            color: #0f172a;
            letter-spacing: -0.025em;
            margin: 0;
        }
        .admin-subtitle {
            font-size: 0.85rem;
            color: #64748b;
            margin: 0.25rem 0 0 0;
        }

        /* Modern KPI Stat Cards */
        .analytics-kpi-card {
            background: #ffffff;
            border: 1px solid #e2e8f0;
            border-radius: 14px;
            padding: 1.25rem;
            box-shadow: 0 1px 3px rgba(0, 0, 0, 0.04);
            transition: transform 0.2s ease, box-shadow 0.2s ease, border-color 0.2s ease;
            position: relative;
            overflow: hidden;
        }
        .analytics-kpi-card:hover {
            transform: translateY(-3px);
            box-shadow: 0 12px 24px -6px rgba(15, 23, 42, 0.08);
            border-color: #cbd5e1;
        }
        .kpi-title {
            font-size: 0.72rem;
            font-weight: 700;
            text-transform: uppercase;
            letter-spacing: 0.8px;
            color: #64748b;
            margin-bottom: 0.5rem;
        }
        .kpi-value {
            font-size: 1.85rem;
            font-weight: 800;
            color: #0f172a;
            line-height: 1.2;
            margin-bottom: 0.35rem;
            letter-spacing: -0.02em;
        }

        /* Tables & Containers */
        .admin-card {
            background: #ffffff;
            border: 1px solid #e2e8f0;
            border-radius: 14px;
            box-shadow: 0 1px 3px rgba(0, 0, 0, 0.04);
            overflow: hidden;
            margin-bottom: 1.75rem;
        }
        .admin-card-header {
            padding: 1.25rem 1.5rem;
            border-bottom: 1px solid #f1f5f9;
            background: #ffffff;
            display: flex;
            flex-wrap: wrap;
            justify-content: space-between;
            align-items: center;
            gap: 1rem;
        }
        .admin-card-title {
            font-size: 1.15rem;
            font-weight: 700;
            color: #0f172a;
            margin: 0;
        }
        .admin-card-subtitle {
            font-size: 0.8rem;
            color: #64748b;
            margin: 0.2rem 0 0 0;
        }
        .admin-table thead th {
            background: #f8fafc;
            color: #475569;
            font-size: 0.72rem;
            font-weight: 700;
            text-transform: uppercase;
            letter-spacing: 0.8px;
            border-bottom: 1px solid #e2e8f0;
            padding: 0.85rem 1.25rem;
            white-space: nowrap;
        }
        .admin-table tbody td {
            padding: 0.95rem 1.25rem;
            border-bottom: 1px solid #f1f5f9;
            vertical-align: middle;
            font-size: 0.88rem;
        }
        .admin-table tbody tr:hover {
            background-color: #f8fafc;
        }
        @media (max-width: 991.98px) {
            .admin-layout-container {
                flex-direction: column !important;
            }
            .admin-sidebar {
                width: 100% !important;
                border-right: none !important;
                border-bottom: 1px solid #1e293b;
            }
            .admin-main {
                padding: 1.25rem 1rem !important;
            }
        }
    </style>

    <div class="admin-layout-container" style="display: flex; min-height: calc(100vh - 80px);">
        <!-- Sidebar: fixed 240px wide -->
        <div class="admin-sidebar" style="width: 240px; flex-shrink: 0; background: #0f172a; color: #ffffff; display: flex; flex-direction: column; padding: 1.5rem 1rem; border-right: 1px solid #1e293b;">
            <!-- Brand Section -->
            <div class="admin-sidebar-brand">
                <div class="rounded-circle d-flex align-items-center justify-content-center me-3 flex-shrink-0" style="width: 40px; height: 40px; background: rgba(34, 197, 94, 0.15); color: #22c55e; border: 1px solid rgba(34, 197, 94, 0.3);">
                    <i class="bi bi-shield-check fs-5"></i>
                </div>
                <div style="min-width: 0;">
                    <h6 class="fw-bold text-white mb-0" style="letter-spacing: 0.5px; font-size: 0.95rem;">
                        <span style="color: #4ade80;">Khedut</span><span style="color: #22c55e;">Rise</span> <span class="badge bg-success-subtle text-success ms-1 px-1 py-0" style="font-size: 0.65rem;">PRO</span>
                    </h6>
                    <small class="text-white-50 d-block text-truncate" style="font-size: 0.72rem;">Admin Console</small>
                </div>
            </div>

            <!-- Navigation Links: Main Console -->
            <div class="admin-sidebar-section-title">Core Console</div>
            <nav class="d-flex flex-column mb-3">
                <a href="AdminDashboard.aspx" class="admin-nav-item">
                    <i class="bi bi-grid-1x2-fill"></i>
                    <span>Dashboard</span>
                </a>
                <a href="AdminManagement.aspx?tab=Farmers" class="admin-nav-item">
                    <i class="bi bi-people-fill"></i>
                    <span>Farmers</span>
                </a>
                <a href="AdminManagement.aspx?tab=Buyers" class="admin-nav-item">
                    <i class="bi bi-shop"></i>
                    <span>Buyers</span>
                </a>
                <a href="AdminManagement.aspx?tab=Moderation" class="admin-nav-item">
                    <i class="bi bi-shield-lock-fill"></i>
                    <span>Moderation</span>
                </a>
            </nav>

            <!-- Navigation Links: System & Analytics -->
            <div class="admin-sidebar-section-title">Analytics & Config</div>
            <nav class="d-flex flex-column mb-auto">
                <a href="AdminAnalytics.aspx" class="admin-nav-item active">
                    <i class="bi bi-graph-up-arrow"></i>
                    <span>Analytics</span>
                </a>
                <a href="AdminSettings.aspx" class="admin-nav-item">
                    <i class="bi bi-sliders"></i>
                    <span>Settings</span>
                </a>
            </nav>

            <!-- Bottom of sidebar: Admin avatar, name, email, logout link -->
            <div class="admin-sidebar-footer">
                <div class="d-flex align-items-center justify-content-between">
                    <div class="d-flex align-items-center" style="min-width: 0;">
                        <div class="rounded-circle text-white fw-bold d-flex align-items-center justify-content-center me-2 flex-shrink-0" style="width: 34px; height: 34px; font-size: 0.8rem; background: #22c55e;">
                            SA
                        </div>
                        <div style="line-height: 1.2; min-width: 0;">
                            <span class="d-block text-white small fw-bold text-truncate">System Admin</span>
                            <span class="text-white-50 d-block text-truncate" style="font-size: 0.72rem;">admin@khedutrise.com</span>
                        </div>
                    </div>
                    <a href="Default.aspx?action=logout" class="btn btn-sm btn-outline-secondary border-0 p-1 text-danger-emphasis ms-2" title="Sign Out" onclick="window.location.href='Default.aspx?action=logout'; return false;">
                        <i class="bi bi-power fs-5"></i>
                    </a>
                </div>
            </div>
        </div>

        <!-- Main content: takes remaining space -->
        <div class="admin-main" style="flex: 1; min-width: 0; padding: 2rem 2.25rem; background-color: #f8fafc;">
            <!-- Top Header Bar inside Main Content -->
            <div class="admin-header">
                <div>
                    <small class="text-muted"><a href="Default.aspx" class="text-decoration-none text-muted">Home</a> / <a href="AdminDashboard.aspx" class="text-decoration-none text-muted">Admin Portal</a> / Platform Analytics</small>
                    <h2 class="admin-title mt-1">KhedutRise Telemetry & Business Analytics</h2>
                    <p class="admin-subtitle">Real-time database aggregated metrics, market volume, and financial indicators</p>
                </div>
                <div class="d-flex gap-2 align-items-center">
                    <span class="badge bg-success-subtle text-success border border-success-subtle px-3 py-2 fw-semibold rounded-pill">
                        <i class="bi bi-circle-fill me-1 small"></i> Live Database Sync
                    </span>
                    <span class="badge bg-white text-secondary border px-3 py-2 fw-semibold rounded-pill shadow-sm">
                        <asp:Literal ID="litGeneratedTime" runat="server"></asp:Literal>
                    </span>
                </div>
            </div>

            <!-- 4 Top Cards (Live Database Aggregates via CTE) -->
            <div class="row g-3 mb-4">
                <div class="col-6 col-md-3">
                    <div class="analytics-kpi-card h-100">
                        <div class="d-flex justify-content-between align-items-center mb-1">
                            <span class="kpi-title">TOTAL ECOSYSTEM GMV</span>
                            <div class="rounded-circle d-flex align-items-center justify-content-center" style="width: 36px; height: 36px; background: #dcfce7; color: #16a34a;">
                                <i class="bi bi-currency-rupee fs-6"></i>
                            </div>
                        </div>
                        <h4 class="kpi-value mb-0" style="color: #0b6028 !important;">
                            <asp:Literal ID="litTotalGmv" runat="server">₹0</asp:Literal>
                        </h4>
                        <small class="text-muted mt-1 d-block" style="font-size: 0.75rem;">Orders Transaction Value</small>
                    </div>
                </div>
                <div class="col-6 col-md-3">
                    <div class="analytics-kpi-card h-100">
                        <div class="d-flex justify-content-between align-items-center mb-1">
                            <span class="kpi-title">ORDERS PLACED</span>
                            <div class="rounded-circle d-flex align-items-center justify-content-center" style="width: 36px; height: 36px; background: #dbeafe; color: #2563eb;">
                                <i class="bi bi-box-seam fs-6"></i>
                            </div>
                        </div>
                        <h4 class="kpi-value mb-0">
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
                            <span class="kpi-title">TOTAL ACCOUNTS</span>
                            <div class="rounded-circle d-flex align-items-center justify-content-center" style="width: 36px; height: 36px; background: #e0f2fe; color: #0284c7;">
                                <i class="bi bi-people-fill fs-6"></i>
                            </div>
                        </div>
                        <h4 class="kpi-value mb-0" style="color: #0284c7 !important;">
                            <asp:Literal ID="litTotalUsers" runat="server">0</asp:Literal>
                        </h4>
                        <small class="text-muted mt-1 d-block" style="font-size: 0.75rem;">Farmers & Buyers</small>
                    </div>
                </div>
                <div class="col-6 col-md-3">
                    <div class="analytics-kpi-card h-100">
                        <div class="d-flex justify-content-between align-items-center mb-1">
                            <span class="kpi-title">RENTALS REVENUE</span>
                            <div class="rounded-circle d-flex align-items-center justify-content-center" style="width: 36px; height: 36px; background: #ffedd5; color: #ea580c;">
                                <i class="bi bi-truck fs-6"></i>
                            </div>
                        </div>
                        <h4 class="kpi-value mb-0" style="color: #ea580c !important;">
                            <asp:Literal ID="litTotalRentals" runat="server">₹0</asp:Literal>
                        </h4>
                        <small class="text-muted mt-1 d-block" style="font-size: 0.75rem;">
                            <asp:Literal ID="litBookingsCount" runat="server">0 Bookings</asp:Literal>
                        </small>
                    </div>
                </div>
            </div>

            <!-- User Ecosystem Distribution Progress Bar -->
            <div class="admin-card">
                <div class="admin-card-header">
                    <div>
                        <h6 class="admin-card-title fs-6"><i class="bi bi-pie-chart-fill me-2 text-primary"></i>User Ecosystem Demographics</h6>
                        <p class="admin-card-subtitle">Ecosystem distribution by registered account type</p>
                    </div>
                    <span class="badge bg-light text-dark border px-3 py-2 fw-semibold rounded-pill">Total: <asp:Literal ID="litDemographicsTotal" runat="server">0</asp:Literal> Verified Users</span>
                </div>
                <div class="p-4">
                    <div class="progress" style="height: 12px; border-radius: 6px; background-color: #f1f5f9;">
                        <asp:Literal ID="litProgressBar" runat="server"></asp:Literal>
                    </div>
                    <div class="d-flex flex-wrap gap-4 mt-3 small">
                        <div class="d-flex align-items-center">
                            <span class="d-inline-block rounded-circle me-2" style="width: 10px; height: 10px; background-color: #16a34a;"></span>
                            <span class="text-muted me-1">Farmers:</span> <strong><asp:Literal ID="litFarmerCountText" runat="server">0</asp:Literal></strong>
                        </div>
                        <div class="d-flex align-items-center">
                            <span class="d-inline-block rounded-circle me-2" style="width: 10px; height: 10px; background-color: #2563eb;"></span>
                            <span class="text-muted me-1">Buyers & Traders:</span> <strong><asp:Literal ID="litBuyerCountText" runat="server">0</asp:Literal></strong>
                        </div>
                        <div class="d-flex align-items-center">
                            <span class="d-inline-block rounded-circle me-2" style="width: 10px; height: 10px; background-color: #f59e0b;"></span>
                            <span class="text-muted me-1">Machinery Operators:</span> <strong><asp:Literal ID="litOperatorCountText" runat="server">0</asp:Literal></strong>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Crop Marketplace Categories Breakdown (SQL Window Function ROW_NUMBER) -->
            <div class="admin-card">
                <div class="admin-card-header">
                    <div>
                        <h5 class="admin-card-title">Crop Category Analytics (SQL Window Function Analysis)</h5>
                        <p class="admin-card-subtitle">Aggregated listing volume, inventory in Quintals, and average market price</p>
                    </div>
                    <span class="badge bg-light text-dark border px-2 py-1 small rounded-pill">Ranked by Volume</span>
                </div>

                <div class="table-responsive">
                    <asp:Repeater ID="rptCategoryBreakdown" runat="server">
                        <HeaderTemplate>
                            <table class="table admin-table align-middle mb-0">
                                <thead>
                                    <tr>
                                        <th class="ps-4">RANK</th>
                                        <th>COMMODITY CATEGORY</th>
                                        <th>ACTIVE LISTINGS</th>
                                        <th>TOTAL VOLUME</th>
                                        <th class="text-end pe-4">AVG PRICE / QUINTAL</th>
                                    </tr>
                                </thead>
                                <tbody>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td class="ps-4">
                                    <span class="badge bg-dark-subtle text-dark border rounded-circle" style="width: 26px; height: 26px; display: inline-flex; align-items: center; justify-content: center; font-weight: 700;">
                                        <%# Eval("PopularityRank") %>
                                    </span>
                                </td>
                                <td>
                                    <strong class="text-dark"><%# Eval("Category") %></strong>
                                </td>
                                <td>
                                    <span class="badge bg-success-subtle text-success border border-success-subtle px-2 py-1 fw-bold rounded-pill">
                                        <%# Eval("ListingCount") %> Listings
                                    </span>
                                </td>
                                <td>
                                    <strong class="text-dark"><%# Convert.ToDecimal(Eval("TotalQuantity")).ToString("N0") %></strong> <span class="text-muted">Quintals</span>
                                </td>
                                <td class="text-end pe-4">
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
            <div class="admin-card mb-0">
                <div class="admin-card-header">
                    <div>
                        <h5 class="admin-card-title">Recent Platform Orders & Escrow Pipeline</h5>
                        <p class="admin-card-subtitle">Direct transaction telemetry between Buyers and Farmers</p>
                    </div>
                    <a href="AdminManagement.aspx?tab=Buyers" class="small fw-bold text-success text-decoration-none">Manage Orders <i class="bi bi-arrow-right"></i></a>
                </div>

                <div class="table-responsive">
                    <asp:Repeater ID="rptRecentOrders" runat="server">
                        <HeaderTemplate>
                            <table class="table admin-table align-middle mb-0">
                                <thead>
                                    <tr>
                                        <th class="ps-4">ORDER ID</th>
                                        <th>COMMODITY</th>
                                        <th>BUYER</th>
                                        <th>FARMER</th>
                                        <th>TOTAL VALUE</th>
                                        <th class="text-end pe-4">STATUS</th>
                                    </tr>
                                </thead>
                                <tbody>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td class="ps-4"><strong class="text-dark">#<%# Eval("OrderId") %></strong></td>
                                <td><strong><%# Eval("Commodity") %></strong> <span class="text-muted">(<%# Eval("Quantity") %>)</span></td>
                                <td><%# Eval("BuyerName") %></td>
                                <td><%# Eval("FarmerName") %></td>
                                <td><strong style="color: #16a34a;">₹<%# Convert.ToDecimal(Eval("TotalPrice")).ToString("N0") %></strong></td>
                                <td class="text-end pe-4">
                                    <span class='badge <%# Eval("Status").ToString() == "Delivered" ? "bg-success-subtle text-success border border-success-subtle" : (Eval("Status").ToString() == "In Transit" ? "bg-primary-subtle text-primary border border-primary-subtle" : "bg-warning-subtle text-warning border border-warning-subtle") %> fw-bold rounded-pill px-2 py-1'>
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
</asp:Content>