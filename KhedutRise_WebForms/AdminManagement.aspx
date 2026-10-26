<%@ Page Title="Platform Management" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="AdminManagement.aspx.cs" Inherits="KhedutRise.AdminManagement" %>

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
        .mgmt-kpi-card {
            background: #ffffff;
            border: 1px solid #e2e8f0;
            border-radius: 14px;
            padding: 1.25rem;
            box-shadow: 0 1px 3px rgba(0, 0, 0, 0.04);
            transition: transform 0.2s ease, box-shadow 0.2s ease, border-color 0.2s ease;
            position: relative;
            overflow: hidden;
        }
        .mgmt-kpi-card:hover {
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

        /* Modern Tables & Cards */
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
                <asp:LinkButton ID="lnkNavFarmers" runat="server" OnClick="NavSidebar_Click" CommandArgument="Farmers" CssClass="admin-nav-item">
                    <i class="bi bi-people-fill"></i>
                    <span>Farmers</span>
                </asp:LinkButton>
                <asp:LinkButton ID="lnkNavBuyers" runat="server" OnClick="NavSidebar_Click" CommandArgument="Buyers" CssClass="admin-nav-item">
                    <i class="bi bi-shop"></i>
                    <span>Buyers</span>
                </asp:LinkButton>
                <asp:LinkButton ID="lnkNavModeration" runat="server" OnClick="NavSidebar_Click" CommandArgument="Moderation" CssClass="admin-nav-item active">
                    <i class="bi bi-shield-lock-fill"></i>
                    <span>Moderation</span>
                </asp:LinkButton>
            </nav>

            <!-- Navigation Links: System & Analytics -->
            <div class="admin-sidebar-section-title">Analytics & Config</div>
            <nav class="d-flex flex-column mb-auto">
                <a href="AdminAnalytics.aspx" class="admin-nav-item">
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
                    <small class="text-muted"><a href="Default.aspx" class="text-decoration-none text-muted">Home</a> / <a href="AdminDashboard.aspx" class="text-decoration-none text-muted">Admin Portal</a> / <asp:Literal ID="litBreadcrumb" runat="server">Platform Management</asp:Literal></small>
                    <h2 class="admin-title mt-1">
                        <asp:Literal ID="litPageTitle" runat="server">Platform Moderation & Management</asp:Literal>
                    </h2>
                    <p class="admin-subtitle">Live review and governance of farmers, grain traders, crops and agricultural plots</p>
                </div>
                <div class="d-flex align-items-center gap-2">
                    <a href="AdminDashboard.aspx" class="btn btn-sm btn-outline-secondary rounded-pill px-3 py-2 fw-semibold shadow-sm bg-white">
                        <i class="bi bi-arrow-left me-1"></i> Back to Dashboard
                    </a>
                </div>
            </div>

            <!-- Status KPI summary cards -->
            <div class="row g-3 mb-4">
                <div class="col-6 col-md-3">
                    <div class="mgmt-kpi-card h-100 border-start border-4 border-primary">
                        <div class="d-flex justify-content-between align-items-center mb-1">
                            <span class="kpi-title">TOTAL ITEMS</span>
                            <i class="bi bi-collection text-primary fs-5"></i>
                        </div>
                        <h4 class="kpi-value mb-0">
                            <asp:Label ID="lblTotalCount" runat="server" Text="0"></asp:Label>
                        </h4>
                    </div>
                </div>
                <div class="col-6 col-md-3">
                    <div class="mgmt-kpi-card h-100 border-start border-4 border-success">
                        <div class="d-flex justify-content-between align-items-center mb-1">
                            <span class="kpi-title">APPROVED</span>
                            <i class="bi bi-check-circle-fill text-success fs-5"></i>
                        </div>
                        <h4 class="kpi-value mb-0" style="color: #16a34a !important;">
                            <asp:Label ID="lblApprovedCount" runat="server" Text="0"></asp:Label>
                        </h4>
                    </div>
                </div>
                <div class="col-6 col-md-3">
                    <div class="mgmt-kpi-card h-100 border-start border-4 border-warning">
                        <div class="d-flex justify-content-between align-items-center mb-1">
                            <span class="kpi-title">PENDING</span>
                            <i class="bi bi-clock-history text-warning fs-5"></i>
                        </div>
                        <h4 class="kpi-value mb-0" style="color: #d97706 !important;">
                            <asp:Label ID="lblPendingCount" runat="server" Text="0"></asp:Label>
                        </h4>
                    </div>
                </div>
                <div class="col-6 col-md-3">
                    <div class="mgmt-kpi-card h-100 border-start border-4 border-danger">
                        <div class="d-flex justify-content-between align-items-center mb-1">
                            <span class="kpi-title">REJECTED</span>
                            <i class="bi bi-x-circle-fill text-danger fs-5"></i>
                        </div>
                        <h4 class="kpi-value mb-0" style="color: #dc2626 !important;">
                            <asp:Label ID="lblRejectedCount" runat="server" Text="0"></asp:Label>
                        </h4>
                    </div>
                </div>
            </div>

            <!-- Action Feedback Alert -->
            <asp:Panel ID="pnlAlert" runat="server" Visible="false" CssClass="alert alert-success alert-dismissible fade show p-3 mb-4 rounded-3 shadow-sm" role="alert">
                <div class="d-flex align-items-center">
                    <i class="bi bi-info-circle-fill fs-5 me-2"></i>
                    <div>
                        <asp:Label ID="lblAlertMsg" runat="server" CssClass="fw-bold"></asp:Label>
                    </div>
                </div>
                <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
            </asp:Panel>

            <!-- Filter Category Pills & Status Filter Bar -->
            <div class="d-flex flex-wrap align-items-center justify-content-between gap-3 mb-4">
                <div class="d-flex flex-wrap gap-2">
                    <asp:LinkButton ID="btnFarmers" runat="server" CssClass="btn btn-sm rounded-pill px-4 fw-bold text-white shadow-none" style="background-color: #0b6028; border: 1px solid #0b6028;" OnClick="TabFilter_Click" CommandArgument="Farmer">Farmers</asp:LinkButton>
                    <asp:LinkButton ID="btnBuyers" runat="server" CssClass="btn btn-sm rounded-pill px-4 fw-semibold shadow-none bg-white text-dark" style="border: 1px solid #d1d5db;" OnClick="TabFilter_Click" CommandArgument="Buyer">Buyers</asp:LinkButton>
                    <asp:LinkButton ID="btnModeration" runat="server" CssClass="btn btn-sm rounded-pill px-4 fw-semibold shadow-none bg-white text-dark" style="border: 1px solid #d1d5db;" OnClick="TabFilter_Click" CommandArgument="Moderation">All Moderation</asp:LinkButton>
                    <asp:LinkButton ID="btnProducts" runat="server" CssClass="btn btn-sm rounded-pill px-4 fw-semibold shadow-none bg-white text-dark" style="border: 1px solid #d1d5db;" OnClick="TabFilter_Click" CommandArgument="Products">Crops List</asp:LinkButton>
                    <asp:LinkButton ID="btnLandListings" runat="server" CssClass="btn btn-sm rounded-pill px-4 fw-semibold shadow-none bg-white text-dark" style="border: 1px solid #d1d5db;" OnClick="TabFilter_Click" CommandArgument="LandListings">Land Listings</asp:LinkButton>
                </div>

                <!-- Status Filter Toggle Buttons -->
                <div class="btn-group btn-group-sm" role="group">
                    <asp:LinkButton ID="btnStatusAll" runat="server" CssClass="btn btn-outline-secondary" OnClick="StatusFilter_Click" CommandArgument="ALL">All</asp:LinkButton>
                    <asp:LinkButton ID="btnStatusApproved" runat="server" CssClass="btn btn-outline-success" OnClick="StatusFilter_Click" CommandArgument="Approved">Approved</asp:LinkButton>
                    <asp:LinkButton ID="btnStatusPending" runat="server" CssClass="btn btn-outline-warning text-dark" OnClick="StatusFilter_Click" CommandArgument="Pending">Pending</asp:LinkButton>
                    <asp:LinkButton ID="btnStatusRejected" runat="server" CssClass="btn btn-outline-danger" OnClick="StatusFilter_Click" CommandArgument="Rejected">Rejected</asp:LinkButton>
                </div>
            </div>

            <!-- VIEW 1: USERS TABLE (Farmers, Buyers, All Moderation) -->
            <asp:Panel ID="pnlUsersView" runat="server" CssClass="admin-card">
                <div class="admin-card-header">
                    <div>
                        <h5 class="admin-card-title">
                            <asp:Literal ID="litTableHeading" runat="server">Accounts & Moderation List</asp:Literal>
                        </h5>
                        <p class="admin-card-subtitle">Select an account below to review KYC, verify credentials, or apply moderation actions</p>
                    </div>
                </div>

                <div class="table-responsive">
                    <asp:Repeater ID="rptModeration" runat="server" OnItemCommand="rptModeration_ItemCommand">
                        <HeaderTemplate>
                            <table class="table admin-table align-middle mb-0">
                                <thead>
                                    <tr>
                                        <th class="ps-4">USER & CONTACT</th>
                                        <th>ROLE</th>
                                        <th>LOCATION</th>
                                        <th>STATUS</th>
                                        <th>DETAILS</th>
                                        <th class="text-end pe-4">APPROVAL ACTIONS</th>
                                    </tr>
                                </thead>
                                <tbody>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td class="ps-4">
                                    <strong class="text-dark d-block"><%# Eval("FullName") %></strong>
                                    <small class="text-muted"><i class="bi bi-telephone me-1"></i><%# Eval("MobileNo") %></small>
                                </td>
                                <td>
                                    <span class="badge bg-light text-dark border px-2 py-1"><%# Eval("UserRole") %></span>
                                </td>
                                <td>
                                    <span class="text-secondary small fw-medium">
                                        <i class="bi bi-geo-alt me-1 text-danger"></i><%# Eval("Location") %>
                                    </span>
                                </td>
                                <td>
                                    <%# (Eval("Status").ToString() == "Verified" || Eval("Status").ToString() == "Approved")
                                        ? "<span class='badge rounded-pill bg-success-subtle text-success border border-success-subtle px-2 py-1'><i class='bi bi-check-circle-fill me-1'></i> Approved</span>" 
                                        : (Eval("Status").ToString() == "Rejected"
                                            ? "<span class='badge rounded-pill bg-danger-subtle text-danger border border-danger-subtle px-2 py-1'><i class='bi bi-x-circle-fill me-1'></i> Rejected</span>"
                                            : "<span class='badge rounded-pill bg-warning-subtle text-warning-emphasis border border-warning-subtle px-2 py-1'><i class='bi bi-clock-history me-1'></i> Pending</span>") %>
                                </td>
                                <td>
                                    <small class="text-muted"><%# Eval("SpecificField") ?? "Registered Account" %></small>
                                </td>
                                <td class="text-end pe-4">
                                    <div class="d-inline-flex gap-1">
                                        <!-- Approve Button -->
                                        <asp:LinkButton ID="btnApprove" runat="server" CommandName="Approve" CommandArgument='<%# Eval("UserId") %>' 
                                            CssClass="btn btn-sm btn-success text-white py-1 px-2 fw-semibold rounded-2" 
                                            ToolTip="Approve this account">
                                            <i class="bi bi-check-lg me-1"></i>Approve
                                        </asp:LinkButton>
                                        
                                        <!-- Reject Button -->
                                        <asp:LinkButton ID="btnReject" runat="server" CommandName="Reject" CommandArgument='<%# Eval("UserId") %>' 
                                            CssClass="btn btn-sm btn-danger text-white py-1 px-2 fw-semibold rounded-2" 
                                            ToolTip="Reject this account">
                                            <i class="bi bi-x-lg me-1"></i>Reject
                                        </asp:LinkButton>

                                        <!-- Delete / Remove Button -->
                                        <asp:LinkButton ID="btnDeactivate" runat="server" CommandName="DeleteUser" CommandArgument='<%# Eval("UserId") %>' 
                                            CssClass="btn btn-sm btn-outline-secondary py-1 px-2 rounded-2" 
                                            OnClientClick="return confirm('Are you sure you want to delete this record?');"
                                            ToolTip="Delete record">
                                            <i class="bi bi-trash"></i>
                                        </asp:LinkButton>
                                    </div>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                                </tbody>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </div>
            </asp:Panel>

            <!-- VIEW 2: CROPS / PRODUCTS TABLE -->
            <asp:Panel ID="pnlProductsView" runat="server" Visible="false" CssClass="admin-card">
                <div class="admin-card-header">
                    <div>
                        <h5 class="admin-card-title">Crop Listings Moderation</h5>
                        <p class="admin-card-subtitle">Approve or reject farmer harvest listings before they appear in APMC Marketplace</p>
                    </div>
                </div>

                <div class="table-responsive">
                    <asp:Repeater ID="rptProducts" runat="server" OnItemCommand="rptProducts_ItemCommand">
                        <HeaderTemplate>
                            <table class="table admin-table align-middle mb-0">
                                <thead>
                                    <tr>
                                        <th class="ps-4">CROP & CATEGORY</th>
                                        <th>FARMER NAME</th>
                                        <th>PRICE / Q</th>
                                        <th>STATUS</th>
                                        <th class="text-end pe-4">APPROVAL ACTIONS</th>
                                    </tr>
                                </thead>
                                <tbody>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td class="ps-4">
                                    <strong class="text-dark d-block"><%# Eval("ProductName") %></strong>
                                    <small class="text-muted"><%# Eval("Category") %> | <%# Eval("QuantityInQuintal") %> Quintals</small>
                                </td>
                                <td>
                                    <span class="fw-semibold text-dark"><%# Eval("FarmerName") %></span><br />
                                    <small class="text-muted"><i class="bi bi-geo-alt me-1"></i><%# Eval("Location") %></small>
                                </td>
                                <td>
                                    <strong style="color: #0b6028;">₹<%# string.Format("{0:N0}", Eval("PricePerQuintal")) %></strong>
                                </td>
                                <td>
                                    <%# Convert.ToBoolean(Eval("IsApproved"))
                                        ? "<span class='badge rounded-pill bg-success-subtle text-success border border-success-subtle px-2 py-1'><i class='bi bi-check-circle-fill me-1'></i> Approved</span>" 
                                        : "<span class='badge rounded-pill bg-danger-subtle text-danger border border-danger-subtle px-2 py-1'><i class='bi bi-x-circle-fill me-1'></i> Rejected</span>" %>
                                </td>
                                <td class="text-end pe-4">
                                    <div class="d-inline-flex gap-1">
                                        <asp:LinkButton ID="btnApproveProduct" runat="server" CommandName="ApproveProduct" CommandArgument='<%# Eval("ProductId") %>' 
                                            CssClass="btn btn-sm btn-success text-white py-1 px-2 fw-semibold rounded-2" ToolTip="Approve Crop">
                                            <i class="bi bi-check-lg me-1"></i>Approve
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="btnRejectProduct" runat="server" CommandName="RejectProduct" CommandArgument='<%# Eval("ProductId") %>' 
                                            CssClass="btn btn-sm btn-danger text-white py-1 px-2 fw-semibold rounded-2" ToolTip="Reject Crop">
                                            <i class="bi bi-x-lg me-1"></i>Reject
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="btnDeleteProduct" runat="server" CommandName="DeleteProduct" CommandArgument='<%# Eval("ProductId") %>' 
                                            CssClass="btn btn-sm btn-outline-secondary py-1 px-2 rounded-2" OnClientClick="return confirm('Are you sure?');">
                                            <i class="bi bi-trash"></i>
                                        </asp:LinkButton>
                                    </div>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                                </tbody>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </div>
            </asp:Panel>

            <!-- VIEW 3: LAND LISTINGS TABLE -->
            <asp:Panel ID="pnlLandView" runat="server" Visible="false" CssClass="admin-card">
                <div class="admin-card-header">
                    <div>
                        <h5 class="admin-card-title">Land Listings Moderation</h5>
                        <p class="admin-card-subtitle">Approve or reject agricultural plots and farmland lease listings</p>
                    </div>
                </div>

                <div class="table-responsive">
                    <asp:Repeater ID="rptLand" runat="server" OnItemCommand="rptLand_ItemCommand">
                        <HeaderTemplate>
                            <table class="table admin-table align-middle mb-0">
                                <thead>
                                    <tr>
                                        <th class="ps-4">LAND TITLE & LOCATION</th>
                                        <th>AREA (ACRES)</th>
                                        <th>PRICE</th>
                                        <th>SOIL & WATER</th>
                                        <th>STATUS</th>
                                        <th class="text-end pe-4">APPROVAL ACTIONS</th>
                                    </tr>
                                </thead>
                                <tbody>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td class="ps-4">
                                    <strong class="text-dark d-block"><%# Eval("Title") %></strong>
                                    <small class="text-muted"><i class="bi bi-geo-alt me-1 text-danger"></i><%# Eval("Location") %></small>
                                </td>
                                <td><span class="badge bg-light text-dark border px-2 py-1"><%# Eval("AreaAcres") %> Acres</span></td>
                                <td><strong class="text-dark"><%# Eval("Price") %></strong></td>
                                <td><small class="text-muted"><%# Eval("SoilType") %> | <%# Eval("WaterSource") %></small></td>
                                <td>
                                    <%# (Eval("Status").ToString() == "Approved" || Eval("Status").ToString() == "Verified")
                                        ? "<span class='badge rounded-pill bg-success-subtle text-success border border-success-subtle px-2 py-1'><i class='bi bi-check-circle-fill me-1'></i> Approved</span>" 
                                        : (Eval("Status").ToString() == "Rejected"
                                            ? "<span class='badge rounded-pill bg-danger-subtle text-danger border border-danger-subtle px-2 py-1'><i class='bi bi-x-circle-fill me-1'></i> Rejected</span>"
                                            : "<span class='badge rounded-pill bg-warning-subtle text-warning-emphasis border border-warning-subtle px-2 py-1'><i class='bi bi-clock-history me-1'></i> Pending</span>") %>
                                </td>
                                <td class="text-end pe-4">
                                    <div class="d-inline-flex gap-1">
                                        <asp:LinkButton ID="btnApproveLand" runat="server" CommandName="ApproveLand" CommandArgument='<%# Eval("LandId") %>' 
                                            CssClass="btn btn-sm btn-success text-white py-1 px-2 fw-semibold rounded-2" ToolTip="Approve Land">
                                            <i class="bi bi-check-lg me-1"></i>Approve
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="btnRejectLand" runat="server" CommandName="RejectLand" CommandArgument='<%# Eval("LandId") %>' 
                                            CssClass="btn btn-sm btn-danger text-white py-1 px-2 fw-semibold rounded-2" ToolTip="Reject Land">
                                            <i class="bi bi-x-lg me-1"></i>Reject
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="btnDeleteLand" runat="server" CommandName="DeleteLand" CommandArgument='<%# Eval("LandId") %>' 
                                            CssClass="btn btn-sm btn-outline-secondary py-1 px-2 rounded-2" OnClientClick="return confirm('Are you sure?');">
                                            <i class="bi bi-trash"></i>
                                        </asp:LinkButton>
                                    </div>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                                </tbody>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </div>
            </asp:Panel>
        </div>
    </div>
</asp:Content>