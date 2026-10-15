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
    </style>

    <!-- Admin Platform Analytics  -->
    <div class="kr-page-container py-4">
        <!--Header -->
        <div class="d-flex flex-wrap justify-content-between align-items-center mb-4">
            <div>
                <small class="text-muted"><a href="Default.aspx" class="text-decoration-none text-muted">Home</a> / <a href="AdminDashboard.aspx" class="text-decoration-none text-muted">Admin Portal</a> / Platform Analytics</small>
                <h2 class="fw-bold mt-1 text-dark" style="font-size: 2rem;">KhedutRise Platform Analytics</h2>
                <p class="text-muted small mb-0">Monitor the complete agricultural ecosystem from one place.</p>
            </div>
            <div class="mt-2 mt-md-0">
                <span class="badge bg-white text-dark border px-3 py-2 fw-semibold" style="font-size: 0.85rem;">Last 30 Days</span>
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
                            <h6 class="fw-bold text-white mb-0" style="letter-spacing: 0.5px;">KhedutRise Pro</h6>
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
                <!-- 4 Top Cards -->
                <div class="row g-3 mb-4">
                    <div class="col-6 col-md-3">
                        <div class="card card-custom p-3 text-start h-100">
                            <small class="text-muted fw-bold" style="font-size: 0.72rem; letter-spacing: 0.5px;">TOTAL ECOSYSTEM GMV</small>
                            <h4 class="fw-bold mt-2 mb-0" style="color: #0b6028; font-size: 1.4rem;">₹14.2 Lakhs</h4>
                        </div>
                    </div>
                    <div class="col-6 col-md-3">
                        <div class="card card-custom p-3 text-start h-100">
                            <small class="text-muted fw-bold" style="font-size: 0.72rem; letter-spacing: 0.5px;">TOTAL ORDERS COMPLETED</small>
                            <h4 class="fw-bold text-dark mt-2 mb-0" style="font-size: 1.4rem;">1,840 Crop Bags</h4>
                        </div>
                    </div>
                    <div class="col-6 col-md-3">
                        <div class="card card-custom p-3 text-start h-100">
                            <small class="text-muted fw-bold" style="font-size: 0.72rem; letter-spacing: 0.5px;">ACTIVE USER SESSIONS</small>
                            <h4 class="fw-bold mt-2 mb-0" style="color: #0284c7; font-size: 1.4rem;">4,821 Farmers/Buyers</h4>
                        </div>
                    </div>
                    <div class="col-6 col-md-3">
                        <div class="card card-custom p-3 text-start h-100">
                            <small class="text-muted fw-bold" style="font-size: 0.72rem; letter-spacing: 0.5px;">LAND LISTINGS CONCLUDED</small>
                            <h4 class="fw-bold mt-2 mb-0" style="color: #ea580c; font-size: 1.4rem;">18 Properties</h4>
                        </div>
                    </div>
                </div>

                <div class="row g-4">
                    <!-- LEFT: Geographical Activity Map -->
                    <div class="col-lg-6">
                        <div class="card card-custom p-4 h-100 d-flex flex-column">
                            <h5 class="fw-bold mb-3 text-dark">Geographical Activity Map (Gujarat Area)</h5>
                            <div class="position-relative rounded-3 overflow-hidden border flex-grow-1" style="min-height: 280px;">
                                <img src="Images/aerial_plots.jpg" class="img-fluid object-fit-cover w-100 h-100" style="min-height: 280px;" alt="Gujarat Map" />
                                
                                <!-- SVG Polygon Geofence Overlay -->
                                <svg class="position-absolute top-0 start-0 w-100 h-100" style="pointer-events: none;" viewBox="0 0 100 100" preserveAspectRatio="none">
                                    <polygon points="12,18 82,10 90,65 52,85 24,68" style="fill:rgba(234, 88, 12, 0.12);stroke:#ea580c;stroke-width:2;" />
                                </svg>

                                <!-- Activity1 -->
                                <div class="position-absolute translate-middle" style="top: 35%; left: 25%;">
                                    <div class="rounded-circle shadow" style="width: 16px; height: 16px; background-color: #ea580c; border: 2px solid #ffffff;"></div>
                                </div>

                                <!-- Activity 2 -->
                                <div class="position-absolute translate-middle" style="top: 55%; left: 60%;">
                                    <div class="rounded-circle shadow" style="width: 16px; height: 16px; background-color: #ea580c; border: 2px solid #ffffff;"></div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <!-- RIGHT: Marketplace Revenue Performance Index -->
                    <div class="col-lg-6">
                        <div class="card card-custom p-4 h-100 d-flex flex-column">
                            <h5 class="fw-bold mb-3 text-dark">Marketplace Revenue Performance Index</h5>
                            <div class="rounded-3 overflow-hidden border flex-grow-1">
                                <img src="Images/agri_finance.jpg" class="img-fluid object-fit-cover w-100 h-100" style="min-height: 280px;" alt="Revenue Performance" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
