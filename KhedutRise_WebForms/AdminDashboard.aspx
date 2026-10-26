<%@ Page Title="Admin Dashboard" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="AdminDashboard.aspx.cs" Inherits="KhedutRise.AdminDashboard" %>

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
        .kpi-stat-card {
            background: #ffffff;
            border: 1px solid #e2e8f0;
            border-radius: 14px;
            padding: 1.25rem;
            box-shadow: 0 1px 3px rgba(0, 0, 0, 0.04);
            transition: transform 0.2s ease, box-shadow 0.2s ease, border-color 0.2s ease;
            position: relative;
            overflow: hidden;
        }
        .kpi-stat-card:hover {
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
        .icon-box-emerald {
            width: 44px;
            height: 44px;
            border-radius: 10px;
            background: #dcfce7;
            color: #16a34a;
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: 1.25rem;
        }
        .icon-box-blue {
            width: 44px;
            height: 44px;
            border-radius: 10px;
            background: #dbeafe;
            color: #2563eb;
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: 1.25rem;
        }
        .icon-box-amber {
            width: 44px;
            height: 44px;
            border-radius: 10px;
            background: #fef3c7;
            color: #d97706;
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: 1.25rem;
        }
        .icon-box-purple {
            width: 44px;
            height: 44px;
            border-radius: 10px;
            background: #f3e8ff;
            color: #9333ea;
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: 1.25rem;
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
        .user-initials-avatar {
            width: 36px;
            height: 36px;
            border-radius: 50%;
            background: #f1f5f9;
            color: #334155;
            font-weight: 700;
            font-size: 0.82rem;
            display: flex;
            align-items: center;
            justify-content: center;
            border: 1px solid #cbd5e1;
            flex-shrink: 0;
        }
        .live-pulse {
            width: 8px;
            height: 8px;
            border-radius: 50%;
            background-color: #22c55e;
            box-shadow: 0 0 0 0 rgba(34, 197, 94, 0.7);
            animation: pulse-green 2s infinite;
            display: inline-block;
        }
        @keyframes pulse-green {
            0% { transform: scale(0.95); box-shadow: 0 0 0 0 rgba(34, 197, 94, 0.7); }
            70% { transform: scale(1); box-shadow: 0 0 0 6px rgba(34, 197, 94, 0); }
            100% { transform: scale(0.95); box-shadow: 0 0 0 0 rgba(34, 197, 94, 0); }
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
                <a href="AdminDashboard.aspx" class="admin-nav-item active">
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
                    <div class="d-flex align-items-center gap-2 mb-1">
                        <span class="badge bg-dark text-white px-2 py-1 small rounded-1 fw-bold" style="letter-spacing: 0.5px; font-size: 0.7rem;">ADMIN CONSOLE</span>
                        <span class="text-muted small">•</span>
                        <span class="small text-muted fw-semibold d-inline-flex align-items-center">
                            <span class="live-pulse me-2"></span> System Live & Operational
                        </span>
                    </div>
                    <h2 class="admin-title">Executive Admin Dashboard</h2>
                    <p class="admin-subtitle">Agricultural platform oversight, user moderation and real-time operations</p>
                </div>
                <div class="d-flex align-items-center gap-2">
                    <span class="badge bg-white text-secondary border px-3 py-2 fw-semibold shadow-sm rounded-pill" style="font-size: 0.85rem;">
                        <i class="bi bi-calendar3 me-1 text-success"></i> 
                        <asp:Literal ID="litCurrentDate" runat="server"></asp:Literal>
                    </span>
                    <a href="Default.aspx" target="_blank" class="btn btn-sm btn-outline-secondary rounded-pill px-3 py-2 fw-semibold shadow-sm bg-white">
                        <i class="bi bi-box-arrow-up-right me-1"></i> Live Site
                    </a>
                </div>
            </div>

            <!-- 4 Top Executive KPI Cards -->
            <div class="row g-3 mb-4">
                <!-- KPI 1: Total Users -->
                <div class="col-6 col-md-3">
                    <div class="kpi-stat-card h-100">
                        <div class="d-flex align-items-center justify-content-between mb-2">
                            <span class="kpi-title">TOTAL ACCOUNTS</span>
                            <div class="icon-box-emerald">
                                <i class="bi bi-people-fill"></i>
                            </div>
                        </div>
                        <h3 class="kpi-value">
                            <asp:Label ID="lblTotalUsers" runat="server" Text="0"></asp:Label>
                        </h3>
                        <span class="badge bg-success-subtle text-success small fw-semibold px-2 py-1 rounded-pill">
                            Farmers, Buyers & Operators
                        </span>
                    </div>
                </div>

                <!-- KPI 2: Approved / Verified Users -->
                <div class="col-6 col-md-3">
                    <div class="kpi-stat-card h-100">
                        <div class="d-flex align-items-center justify-content-between mb-2">
                            <span class="kpi-title">VERIFIED USERS</span>
                            <div class="icon-box-blue">
                                <i class="bi bi-patch-check-fill"></i>
                            </div>
                        </div>
                        <h3 class="kpi-value" style="color: #16a34a !important;">
                            <asp:Label ID="lblApprovedCount" runat="server" Text="0"></asp:Label>
                        </h3>
                        <span class="badge bg-primary-subtle text-primary small fw-semibold px-2 py-1 rounded-pill">
                            <asp:Label ID="lblVerificationRate" runat="server" Text="100% Verified"></asp:Label>
                        </span>
                    </div>
                </div>

                <!-- KPI 3: Pending Review -->
                <div class="col-6 col-md-3">
                    <div class="kpi-stat-card h-100">
                        <div class="d-flex align-items-center justify-content-between mb-2">
                            <span class="kpi-title">PENDING KYC</span>
                            <div class="icon-box-amber">
                                <i class="bi bi-clock-history"></i>
                            </div>
                        </div>
                        <h3 class="kpi-value" style="color: #d97706 !important;">
                            <asp:Label ID="lblPendingCount" runat="server" Text="0"></asp:Label>
                        </h3>
                        <span class="badge bg-warning-subtle text-warning-emphasis small fw-semibold px-2 py-1 rounded-pill">
                            Requires Review
                        </span>
                    </div>
                </div>

                <!-- KPI 4: Support Inquiries -->
                <div class="col-6 col-md-3">
                    <div class="kpi-stat-card h-100">
                        <div class="d-flex align-items-center justify-content-between mb-2">
                            <span class="kpi-title">INQUIRIES</span>
                            <div class="icon-box-purple">
                                <i class="bi bi-chat-left-dots-fill"></i>
                            </div>
                        </div>
                        <h3 class="kpi-value" style="color: #9333ea !important;">
                            <asp:Label ID="lblInquiriesCount" runat="server" Text="0"></asp:Label>
                        </h3>
                        <span class="badge small fw-semibold px-2 py-1 rounded-pill" style="background-color: #faf5ff; color: #9333ea; border: 1px solid #f3e8ff;">
                            Direct User Messages
                        </span>
                    </div>
                </div>
            </div>

            <!-- Live Platform Telemetry Strip -->
            <div class="card p-3 mb-4 rounded-3 border bg-white shadow-sm">
                <div class="d-flex flex-wrap justify-content-between align-items-center gap-3">
                    <div class="d-flex align-items-center gap-3">
                        <div class="rounded-circle d-flex align-items-center justify-content-center" style="width: 42px; height: 42px; background-color: #f0fdf4; color: #16a34a;">
                            <i class="bi bi-graph-up-arrow fs-5"></i>
                        </div>
                        <div>
                            <small class="text-muted fw-bold d-block" style="font-size: 0.72rem; letter-spacing: 0.5px;">LIVE ECOSYSTEM GMV</small>
                            <strong class="text-dark fs-6">₹<asp:Literal ID="litDashboardGmv" runat="server">0</asp:Literal></strong>
                            <small class="text-muted"> across <asp:Literal ID="litDashboardOrders" runat="server">0</asp:Literal> harvest orders</small>
                        </div>
                    </div>
                    <div class="d-flex align-items-center gap-3">
                        <div class="rounded-circle d-flex align-items-center justify-content-center" style="width: 42px; height: 42px; background-color: #fffbeb; color: #d97706;">
                            <i class="bi bi-truck fs-5"></i>
                        </div>
                        <div>
                            <small class="text-muted fw-bold d-block" style="font-size: 0.72rem; letter-spacing: 0.5px;">MACHINERY FLEET</small>
                            <strong class="text-dark fs-6"><asp:Literal ID="litDashboardMachinery" runat="server">3 Units</asp:Literal></strong>
                            <small class="text-muted"> • <asp:Literal ID="litDashboardBookings" runat="server">0</asp:Literal> bookings</small>
                        </div>
                    </div>
                    <div>
                        <a href="AdminAnalytics.aspx" class="btn btn-sm btn-outline-success fw-bold px-3 py-2 rounded-pill">
                            View Full Analytics <i class="bi bi-arrow-right ms-1"></i>
                        </a>
                    </div>
                </div>
            </div>

            <!-- Action Notification Banner -->
            <asp:Panel ID="pnlActionAlert" runat="server" Visible="false" CssClass="alert alert-success alert-dismissible fade show p-3 mb-4 rounded-3 shadow-sm border-0" role="alert">
                <div class="d-flex align-items-center">
                    <i class="bi bi-check-circle-fill fs-5 me-2 text-success"></i>
                    <div>
                        <asp:Label ID="lblActionMsg" runat="server" CssClass="fw-semibold text-dark"></asp:Label>
                    </div>
                </div>
                <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
            </asp:Panel>

            <!-- Registrations Approval & Moderation Table Card -->
            <div class="admin-card">
                <div class="admin-card-header">
                    <div>
                        <h5 class="admin-card-title">Registrations Approval & Moderation</h5>
                        <p class="admin-card-subtitle">Live user accounts across Gujarat APMCs, Farmland Portal & Rentals</p>
                    </div>

                    <!-- Search Box & Filter Controls -->
                    <div class="d-flex flex-wrap align-items-center gap-2">
                        <div class="input-group input-group-sm" style="max-width: 260px;">
                            <span class="input-group-text bg-light border-end-0 text-muted"><i class="bi bi-search"></i></span>
                            <asp:TextBox ID="txtSearchUser" runat="server" CssClass="form-control border-start-0 ps-0" placeholder="Search user or city..." AutoPostBack="true" OnTextChanged="txtSearchUser_TextChanged"></asp:TextBox>
                            <asp:LinkButton ID="btnSearchUser" runat="server" CssClass="btn btn-outline-secondary" OnClick="btnSearchUser_Click">
                                <i class="bi bi-arrow-right-short"></i>
                            </asp:LinkButton>
                        </div>

                        <div class="btn-group btn-group-sm" role="group">
                            <asp:LinkButton ID="btnFilterAll" runat="server" CssClass="btn btn-sm btn-dark fw-bold px-3 py-1 shadow-sm rounded-pill" OnClick="FilterStatus_Click" CommandArgument="ALL">
                                All (<asp:Literal ID="litAllCount" runat="server">0</asp:Literal>)
                            </asp:LinkButton>
                            <asp:LinkButton ID="btnFilterPending" runat="server" CssClass="btn btn-sm btn-outline-warning text-dark px-3 py-1 rounded-pill" OnClick="FilterStatus_Click" CommandArgument="Pending">
                                Pending (<asp:Literal ID="litPendingCount" runat="server">0</asp:Literal>)
                            </asp:LinkButton>
                            <asp:LinkButton ID="btnFilterApproved" runat="server" CssClass="btn btn-sm btn-outline-success px-3 py-1 rounded-pill" OnClick="FilterStatus_Click" CommandArgument="Verified">
                                Verified (<asp:Literal ID="litVerifiedCount" runat="server">0</asp:Literal>)
                            </asp:LinkButton>
                            <asp:LinkButton ID="btnFilterRejected" runat="server" CssClass="btn btn-sm btn-outline-danger px-3 py-1 rounded-pill" OnClick="FilterStatus_Click" CommandArgument="Rejected">
                                Rejected (<asp:Literal ID="litRejectedCount" runat="server">0</asp:Literal>)
                            </asp:LinkButton>
                        </div>
                    </div>
                </div>

                <!-- Main Table -->
                <div class="table-responsive">
                    <asp:Repeater ID="rptPending" runat="server" OnItemCommand="rptPending_ItemCommand">
                        <HeaderTemplate>
                            <table class="table admin-table align-middle mb-0">
                                <thead>
                                    <tr>
                                        <th class="ps-4">USER ACCOUNT</th>
                                        <th>ROLE</th>
                                        <th>LOCATION</th>
                                        <th>KYC STATUS</th>
                                        <th>DOCUMENT</th>
                                        <th class="text-end pe-4">MODERATION ACTIONS</th>
                                    </tr>
                                </thead>
                                <tbody>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td class="ps-4">
                                    <div class="d-flex align-items-center">
                                        <div class="user-initials-avatar me-3">
                                            <%# GetInitials(Eval("FullName")) %>
                                        </div>
                                        <div>
                                            <strong class="text-dark d-block"><%# Eval("FullName") %></strong>
                                            <small class="text-muted"><i class="bi bi-telephone me-1"></i><%# Eval("MobileNo") %></small>
                                        </div>
                                    </div>
                                </td>
                                <td>
                                    <%# GetRoleBadge(Eval("UserRole")) %>
                                </td>
                                <td>
                                    <span class="text-secondary small fw-medium">
                                        <i class="bi bi-geo-alt me-1 text-danger"></i><%# Eval("Location") %>
                                    </span>
                                </td>
                                <td>
                                    <%# GetStatusBadge(Eval("Status")) %>
                                </td>
                                <td>
                                    <a href="#" class="btn btn-sm btn-light border py-1 px-2 text-dark small text-decoration-none fw-semibold rounded-2" onclick="alert('Document Verified: <%# GetDocName(Eval("UserRole")) %> - Validated under State Agri Standards.'); return false;">
                                        <i class="bi bi-file-earmark-pdf-fill me-1 text-danger"></i><%# GetDocName(Eval("UserRole")) %>
                                    </a>
                                </td>
                                <td class="text-end pe-4">
                                    <div class="d-inline-flex gap-1">
                                        <asp:LinkButton ID="btnApprove" runat="server" CommandName="Approve" CommandArgument='<%# Eval("UserId") %>' 
                                            CssClass='<%# (Eval("Status").ToString() == "Verified" || Eval("Status").ToString() == "Approved") ? "btn btn-sm btn-light border text-success py-1 px-2 fw-semibold disabled rounded-2" : "btn btn-sm btn-success text-white py-1 px-2 fw-semibold rounded-2" %>' 
                                            ToolTip="Approve this account">
                                            <i class="bi bi-check-lg me-1"></i><%# (Eval("Status").ToString() == "Verified" || Eval("Status").ToString() == "Approved") ? "Approved" : "Approve" %>
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="btnReject" runat="server" CommandName="Reject" CommandArgument='<%# Eval("UserId") %>' 
                                            CssClass='<%# Eval("Status").ToString() == "Rejected" ? "btn btn-sm btn-light border text-danger py-1 px-2 fw-semibold disabled rounded-2" : "btn btn-sm btn-outline-danger py-1 px-2 fw-semibold rounded-2" %>' 
                                            ToolTip="Reject this account">
                                            <i class="bi bi-x-lg me-1"></i>Reject
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

                <!-- Clean Empty State Display -->
                <asp:Panel ID="pnlEmptyState" runat="server" Visible="false" CssClass="text-center py-5">
                    <div class="rounded-circle d-inline-flex align-items-center justify-content-center mb-3" style="width: 64px; height: 64px; background-color: #f8fafc; color: #94a3b8; border: 1px solid #e2e8f0;">
                        <i class="bi bi-inbox fs-2"></i>
                    </div>
                    <h6 class="fw-bold text-dark mb-1">No Accounts Found</h6>
                    <p class="text-muted small mb-3">No user records currently match the selected moderation filter.</p>
                    <asp:LinkButton runat="server" CssClass="btn btn-sm btn-outline-secondary rounded-pill px-3 py-1 fw-semibold" OnClick="FilterStatus_Click" CommandArgument="ALL">
                        View All Registered Users
                    </asp:LinkButton>
                </asp:Panel>
            </div>

            <!-- TWO BOTTOM EXECUTIVE PANELS: Recent Inquiries & System Health -->
            <div class="row g-4">
                <!-- Left: Recent Support & Inquiries (Live from ContactInquiries table) -->
                <div class="col-lg-7">
                    <div class="admin-card h-100 mb-0">
                        <div class="admin-card-header">
                            <div>
                                <h6 class="admin-card-title fs-6"><i class="bi bi-envelope-paper-fill me-2 text-success"></i>Recent User Inquiries</h6>
                                <p class="admin-card-subtitle">Direct communications submitted via Contact Portal</p>
                            </div>
                            <a href="Contact.aspx" target="_blank" class="small fw-bold text-success text-decoration-none">Open Form <i class="bi bi-arrow-right"></i></a>
                        </div>

                        <div class="p-3">
                            <asp:Repeater ID="rptInquiries" runat="server">
                                <HeaderTemplate>
                                    <ul class="list-group list-group-flush border-0">
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <li class="list-group-item px-2 py-3 border-bottom d-flex align-items-start justify-content-between">
                                        <div class="me-3">
                                            <div class="d-flex align-items-center gap-2 mb-1">
                                                <strong class="text-dark small"><%# Eval("FullName") %></strong>
                                                <span class="badge bg-light text-secondary border small" style="font-size: 0.72rem;"><i class="bi bi-telephone me-1"></i><%# Eval("MobileNo") %></span>
                                            </div>
                                            <p class="text-muted small mb-0" style="font-size: 0.83rem;">
                                                <%# Eval("Message") %>
                                            </p>
                                        </div>
                                        <small class="text-muted text-nowrap" style="font-size: 0.75rem;">
                                            <%# Eval("SubmittedDate", "{0:dd MMM HH:mm}") %>
                                        </small>
                                    </li>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </ul>
                                </FooterTemplate>
                            </asp:Repeater>

                            <asp:Panel ID="pnlEmptyInquiries" runat="server" Visible="false" CssClass="text-center py-4 text-muted small">
                                <i class="bi bi-chat-square-text fs-3 d-block mb-1 text-secondary"></i>
                                No new support inquiries submitted yet.
                            </asp:Panel>
                        </div>
                    </div>
                </div>

                <!-- Right: Platform Diagnostics & Ecosystem Quick Launch -->
                <div class="col-lg-5">
                    <div class="admin-card h-100 mb-0">
                        <div class="admin-card-header">
                            <div>
                                <h6 class="admin-card-title fs-6"><i class="bi bi-cpu-fill me-2 text-primary"></i>Platform Diagnostics</h6>
                                <p class="admin-card-subtitle">Environment health and management shortcuts</p>
                            </div>
                        </div>

                        <div class="p-3">
                            <div class="d-flex flex-column gap-2 mb-4">
                                <div class="d-flex justify-content-between align-items-center small p-2 rounded bg-light border">
                                    <span class="text-muted fw-semibold"><i class="bi bi-database me-2 text-success"></i>Database Engine</span>
                                    <span class="badge bg-success-subtle text-success border border-success-subtle">MSSQL LocalDB</span>
                                </div>
                                <div class="d-flex justify-content-between align-items-center small p-2 rounded bg-light border">
                                    <span class="text-muted fw-semibold"><i class="bi bi-shield-check me-2 text-primary"></i>Auth Security</span>
                                    <span class="badge bg-primary-subtle text-primary border border-primary-subtle">Session Protected</span>
                                </div>
                                <div class="d-flex justify-content-between align-items-center small p-2 rounded bg-light border">
                                    <span class="text-muted fw-semibold"><i class="bi bi-code-slash me-2 text-dark"></i>Platform Stack</span>
                                    <span class="badge bg-dark text-white">ASP.NET WebForms 4.8</span>
                                </div>
                            </div>

                            <small class="text-uppercase fw-bold text-muted mb-2 d-block" style="font-size: 0.7rem; letter-spacing: 0.8px;">Quick Launch Actions</small>
                            <div class="row g-2">
                                <div class="col-6">
                                    <a href="ApmcRates.aspx" class="btn btn-sm btn-outline-secondary w-100 py-2 small fw-semibold text-start rounded-2 bg-white">
                                        <i class="bi bi-currency-rupee me-1 text-success"></i> Mandi Rates
                                    </a>
                                </div>
                                <div class="col-6">
                                    <a href="AdminAnalytics.aspx" class="btn btn-sm btn-outline-secondary w-100 py-2 small fw-semibold text-start rounded-2 bg-white">
                                        <i class="bi bi-pie-chart me-1 text-primary"></i> Analytics
                                    </a>
                                </div>
                                <div class="col-6">
                                    <a href="AdminManagement.aspx?tab=Products" class="btn btn-sm btn-outline-secondary w-100 py-2 small fw-semibold text-start rounded-2 bg-white">
                                        <i class="bi bi-box-seam me-1 text-warning"></i> Crop Catalog
                                    </a>
                                </div>
                                <div class="col-6">
                                    <a href="AdminSettings.aspx" class="btn btn-sm btn-outline-secondary w-100 py-2 small fw-semibold text-start rounded-2 bg-white">
                                        <i class="bi bi-gear me-1 text-secondary"></i> System Settings
                                    </a>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>