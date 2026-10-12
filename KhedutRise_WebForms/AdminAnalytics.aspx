<%@ Page Title="Platform Analytics" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="AdminAnalytics.aspx.cs" Inherits="KhedutRise.AdminAnalytics" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Admin Platform Analytics  -->
    <div class="kr-page-container">
        <!--Header -->
        <div class="d-flex flex-wrap justify-content-between align-items-center mb-4">
            <div>
                <small class="text-muted"><a href="Default.aspx" class="text-decoration-none text-muted">Home</a> / <a href="AdminDashboard.aspx" class="text-decoration-none text-muted">Admin Portal</a> / Platform Analytics</small>
                <h2 class="fw-bold mt-1 text-dark" style="font-size: 2rem;">KhedutRise Platform Analytics</h2>
                <p class="text-muted small mb-0">Monitor the complete agricultural ecosystem from one place.</p>
            </div>
            <div class="mt-2 mt-md-0">
                <span class="badge bg-white text-dark border px-3 py-2 fw-semibold" style="font-size: 0.85rem;">Last 30 Days (Aug)</span>
            </div>
        </div>

        <div class="row g-4">
            <!-- LEFT: Dark Navy Navigation -->
            <div class="col-lg-3">
                <div class="p-4 rounded-3 text-white h-100" style="background-color: #0f172a;">
                    <ul class="list-unstyled mb-0 d-flex flex-column gap-3 py-2" style="font-size: 0.95rem;">
                        <li>
                            <a href="AdminDashboard.aspx" class="text-decoration-none text-white d-block opacity-90">
                                <i class="bi bi-speedometer2 me-2"></i>Dashboard
                            </a>
                        </li>
                        <li>
                            <a href="AdminManagement.aspx?tab=Farmers" class="text-decoration-none text-white d-block opacity-90">
                                <i class="bi bi-people me-2"></i>Farmers List
                            </a>
                        </li>
                        <li>
                            <a href="AdminManagement.aspx?tab=Buyers" class="text-decoration-none text-white d-block opacity-90">
                                <i class="bi bi-shop me-2"></i>Buyers List
                            </a>
                        </li>
                        <li>
                            <a href="AdminManagement.aspx?tab=Moderation" class="text-decoration-none text-white d-block opacity-90">
                                <i class="bi bi-shield-check me-2"></i>Moderation
                            </a>
                        </li>
                        <li>
                            <a href="AdminAnalytics.aspx" class="text-decoration-none fw-bold d-block" style="color: #22c55e;">
                                <i class="bi bi-graph-up me-2"></i>• Analytics
                            </a>
                        </li>
                        <li>
                            <a href="AdminSettings.aspx" class="text-decoration-none text-white d-block opacity-90">
                                <i class="bi bi-gear me-2"></i>Settings
                            </a>
                        </li>
                    </ul>
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
