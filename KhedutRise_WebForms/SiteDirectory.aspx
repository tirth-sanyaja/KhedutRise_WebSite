<%@ Page Title="Site Directory & Page Index" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="SiteDirectory.aspx.cs" Inherits="KhedutRise.SiteDirectory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="kr-page-container py-5">
        <!-- Directory Header -->
        <div class="text-center mb-5">
            <span class="badge px-3 py-2 rounded-pill fw-bold mb-2" style="background-color: #f0fdf4; color: #0b6028; border: 1px solid #bbf7d0; font-size: 0.85rem;">
                <i class="bi bi-compass-fill me-1"></i> MASTER SITEMAP & PAGE DIRECTORY
            </span>
            <h2 class="fw-bold text-dark mb-2" style="font-size: 2.2rem;">All 34 Pages Navigation Hub</h2>
            <p class="text-secondary mx-auto" style="max-width: 700px; font-size: 1.05rem;">
                Comprehensive directory linking every single page in KhedutRise, organized by project role distribution (Tirth, Milan, Dharm) and platform functionality.
            </p>
        </div>

        <!-- Role Legend Banner -->
        <div class="card p-3 mb-5 border-0 rounded-3 shadow-sm bg-white" style="border-left: 5px solid #0b6028 !important;">
            <div class="d-flex flex-wrap align-items-center justify-content-between gap-3">
                <div class="d-flex align-items-center gap-2">
                    <i class="bi bi-people-fill text-success fs-4"></i>
                    <div>
                        <strong class="text-dark d-block">CIE-2 Project Presentation Role Mapping</strong>
                        <small class="text-muted">Click any page card below to test live routing and functionality</small>
                    </div>
                </div>
                <div class="d-flex flex-wrap gap-2">
                    <span class="badge bg-success px-3 py-2"><i class="bi bi-person-badge me-1"></i> Tirth: Admin & Farmer (11 Pages)</span>
                    <span class="badge bg-primary px-3 py-2"><i class="bi bi-person-badge me-1"></i> Milan: Buyer & Marketplace (8 Pages)</span>
                    <span class="badge bg-warning text-dark px-3 py-2"><i class="bi bi-person-badge me-1"></i> Dharm: Operator & Services (10 Pages)</span>
                    <span class="badge bg-secondary px-3 py-2"><i class="bi bi-globe me-1"></i> Public / Shared (5 Pages)</span>
                </div>
            </div>
        </div>

        <div class="row g-4">
            <!-- 1. PUBLIC & AUTHENTICATION PAGES -->
            <div class="col-lg-6">
                <div class="card h-100 border rounded-3 p-4 bg-white shadow-sm" style="border-top: 4px solid #64748b !important;">
                    <div class="d-flex justify-content-between align-items-center mb-3">
                        <h4 class="fw-bold text-dark mb-0 fs-5"><i class="bi bi-globe me-2 text-secondary"></i>Public & Core Access</h4>
                        <span class="badge bg-secondary">5 Pages</span>
                    </div>
                    <div class="list-group list-group-flush">
                        <a href="Default.aspx" class="list-group-item list-group-item-action d-flex justify-content-between align-items-center py-3">
                            <div>
                                <strong class="text-dark d-block">1. Home / Landing Page</strong>
                                <small class="text-muted"><code>Default.aspx</code> • Hero banner, features, live stats & testimonials</small>
                            </div>
                            <span class="badge bg-light text-secondary border">Visit &rarr;</span>
                        </a>
                        <a href="SelectRole.aspx" class="list-group-item list-group-item-action d-flex justify-content-between align-items-center py-3">
                            <div>
                                <strong class="text-dark d-block">2. Choose Workspace / Role Selector</strong>
                                <small class="text-muted"><code>SelectRole.aspx</code> • Onboarding router for Farmer, Buyer & Operator</small>
                            </div>
                            <span class="badge bg-light text-secondary border">Visit &rarr;</span>
                        </a>
                        <a href="Login.aspx" class="list-group-item list-group-item-action d-flex justify-content-between align-items-center py-3">
                            <div>
                                <strong class="text-dark d-block">3. User Sign In (Mobile OTP)</strong>
                                <small class="text-muted"><code>Login.aspx</code> • Unified 10-digit mobile + OTP authentication</small>
                            </div>
                            <span class="badge bg-light text-secondary border">Visit &rarr;</span>
                        </a>
                        <a href="About.aspx" class="list-group-item list-group-item-action d-flex justify-content-between align-items-center py-3">
                            <div>
                                <strong class="text-dark d-block">4. About Us</strong>
                                <small class="text-muted"><code>About.aspx</code> • Mission, technology stack, and vision statement</small>
                            </div>
                            <span class="badge bg-light text-secondary border">Visit &rarr;</span>
                        </a>
                        <a href="Contact.aspx" class="list-group-item list-group-item-action d-flex justify-content-between align-items-center py-3">
                            <div>
                                <strong class="text-dark d-block">5. Contact & Support</strong>
                                <small class="text-muted"><code>Contact.aspx</code> • Help desk ticket submission and support hotline</small>
                            </div>
                            <span class="badge bg-light text-secondary border">Visit &rarr;</span>
                        </a>
                    </div>
                </div>
            </div>

            <!-- 2. SUPER ADMIN MODULE (TIRTH) -->
            <div class="col-lg-6">
                <div class="card h-100 border rounded-3 p-4 bg-white shadow-sm" style="border-top: 4px solid #dc2626 !important;">
                    <div class="d-flex justify-content-between align-items-center mb-3">
                        <h4 class="fw-bold text-dark mb-0 fs-5"><i class="bi bi-shield-lock-fill me-2 text-danger"></i>Super Administrator Portal</h4>
                        <span class="badge bg-danger">Tirth (5 Pages)</span>
                    </div>
                    <div class="list-group list-group-flush">
                        <a href="AdminLogin.aspx" class="list-group-item list-group-item-action d-flex justify-content-between align-items-center py-3">
                            <div>
                                <strong class="text-dark d-block">6. Admin Login</strong>
                                <small class="text-muted"><code>AdminLogin.aspx</code> • Credential: admin@khedutrise.com / admin123</small>
                            </div>
                            <span class="badge bg-light text-danger border">Visit &rarr;</span>
                        </a>
                        <a href="AdminDashboard.aspx" class="list-group-item list-group-item-action d-flex justify-content-between align-items-center py-3">
                            <div>
                                <strong class="text-dark d-block">7. Admin Dashboard</strong>
                                <small class="text-muted"><code>AdminDashboard.aspx</code> • Platform moderation metrics & pending queues</small>
                            </div>
                            <span class="badge bg-light text-danger border">Visit &rarr;</span>
                        </a>
                        <a href="AdminManagement.aspx" class="list-group-item list-group-item-action d-flex justify-content-between align-items-center py-3">
                            <div>
                                <strong class="text-dark d-block">8. Platform Moderation Management</strong>
                                <small class="text-muted"><code>AdminManagement.aspx</code> • User KYC, 7/12 land ROR, and crop approval</small>
                            </div>
                            <span class="badge bg-light text-danger border">Visit &rarr;</span>
                        </a>
                        <a href="AdminAnalytics.aspx" class="list-group-item list-group-item-action d-flex justify-content-between align-items-center py-3">
                            <div>
                                <strong class="text-dark d-block">9. Platform Analytics</strong>
                                <small class="text-muted"><code>AdminAnalytics.aspx</code> • Mandi turnover, transaction trends, and volume</small>
                            </div>
                            <span class="badge bg-light text-danger border">Visit &rarr;</span>
                        </a>
                        <a href="AdminSettings.aspx" class="list-group-item list-group-item-action d-flex justify-content-between align-items-center py-3">
                            <div>
                                <strong class="text-dark d-block">10. Admin System Settings</strong>
                                <small class="text-muted"><code>AdminSettings.aspx</code> • Auto-approvals, maintenance mode & DB tools</small>
                            </div>
                            <span class="badge bg-light text-danger border">Visit &rarr;</span>
                        </a>
                    </div>
                </div>
            </div>

            <!-- 3. FARMER MODULE (TIRTH) -->
            <div class="col-lg-6">
                <div class="card h-100 border rounded-3 p-4 bg-white shadow-sm" style="border-top: 4px solid #16a34a !important;">
                    <div class="d-flex justify-content-between align-items-center mb-3">
                        <h4 class="fw-bold text-dark mb-0 fs-5"><i class="bi bi-flower1 me-2 text-success"></i>Farmer Core Module</h4>
                        <span class="badge bg-success">Tirth (6 Pages)</span>
                    </div>
                    <div class="list-group list-group-flush">
                        <a href="RegisterFarmer.aspx" class="list-group-item list-group-item-action d-flex justify-content-between align-items-center py-3">
                            <div>
                                <strong class="text-dark d-block">11. Register as Farmer</strong>
                                <small class="text-muted"><code>RegisterFarmer.aspx</code> • 7/12 land KYC & crop acreage onboarding</small>
                            </div>
                            <span class="badge bg-light text-success border">Visit &rarr;</span>
                        </a>
                        <a href="FarmerDashboard.aspx" class="list-group-item list-group-item-action d-flex justify-content-between align-items-center py-3">
                            <div>
                                <strong class="text-dark d-block">12. Farmer Dashboard</strong>
                                <small class="text-muted"><code>FarmerDashboard.aspx</code> • Sowing metrics, harvest alerts & APMC widget</small>
                            </div>
                            <span class="badge bg-light text-success border">Visit &rarr;</span>
                        </a>
                        <a href="FarmPlots.aspx" class="list-group-item list-group-item-action d-flex justify-content-between align-items-center py-3">
                            <div>
                                <strong class="text-dark d-block">13. Farm Land Plots Registry</strong>
                                <small class="text-muted"><code>FarmPlots.aspx</code> • GPS coordinates, soil health & acreage mapping</small>
                            </div>
                            <span class="badge bg-light text-success border">Visit &rarr;</span>
                        </a>
                        <a href="MyCrops.aspx" class="list-group-item list-group-item-action d-flex justify-content-between align-items-center py-3">
                            <div>
                                <strong class="text-dark d-block">14. My Crops Registry</strong>
                                <small class="text-muted"><code>MyCrops.aspx</code> • Active seasonal harvest inventory & growth stages</small>
                            </div>
                            <span class="badge bg-light text-success border">Visit &rarr;</span>
                        </a>
                        <a href="SellHarvest.aspx" class="list-group-item list-group-item-action d-flex justify-content-between align-items-center py-3">
                            <div>
                                <strong class="text-dark d-block">15. Sell Harvest Form</strong>
                                <small class="text-muted"><code>SellHarvest.aspx</code> • Publish crops with quantity, grade & target rate</small>
                            </div>
                            <span class="badge bg-light text-success border">Visit &rarr;</span>
                        </a>
                        <a href="LandPortal.aspx" class="list-group-item list-group-item-action d-flex justify-content-between align-items-center py-3">
                            <div>
                                <strong class="text-dark d-block">16. Agricultural Land Portal</strong>
                                <small class="text-muted"><code>LandPortal.aspx</code> • Farmland leasing & contract farming registry</small>
                            </div>
                            <span class="badge bg-light text-success border">Visit &rarr;</span>
                        </a>
                    </div>
                </div>
            </div>

            <!-- 4. BUYER & MARKETPLACE MODULE (MILAN) -->
            <div class="col-lg-6">
                <div class="card h-100 border rounded-3 p-4 bg-white shadow-sm" style="border-top: 4px solid #2563eb !important;">
                    <div class="d-flex justify-content-between align-items-center mb-3">
                        <h4 class="fw-bold text-dark mb-0 fs-5"><i class="bi bi-cart-check-fill me-2 text-primary"></i>Buyer & Marketplace Module</h4>
                        <span class="badge bg-primary">Milan (8 Pages)</span>
                    </div>
                    <div class="list-group list-group-flush">
                        <a href="RegisterBuyer.aspx" class="list-group-item list-group-item-action d-flex justify-content-between align-items-center py-3">
                            <div>
                                <strong class="text-dark d-block">17. Register as Buyer</strong>
                                <small class="text-muted"><code>RegisterBuyer.aspx</code> • GSTIN & APMC wholesale trader onboarding</small>
                            </div>
                            <span class="badge bg-light text-primary border">Visit &rarr;</span>
                        </a>
                        <a href="BuyerDashboard.aspx" class="list-group-item list-group-item-action d-flex justify-content-between align-items-center py-3">
                            <div>
                                <strong class="text-dark d-block">18. Buyer Dashboard</strong>
                                <small class="text-muted"><code>BuyerDashboard.aspx</code> • Active bids, escrow transactions & order logs</small>
                            </div>
                            <span class="badge bg-light text-primary border">Visit &rarr;</span>
                        </a>
                        <a href="Marketplace.aspx" class="list-group-item list-group-item-action d-flex justify-content-between align-items-center py-3">
                            <div>
                                <strong class="text-dark d-block">19. Crop Marketplace</strong>
                                <small class="text-muted"><code>Marketplace.aspx</code> • Live catalog with price & category filtering</small>
                            </div>
                            <span class="badge bg-light text-primary border">Visit &rarr;</span>
                        </a>
                        <a href="MarketDetails.aspx" class="list-group-item list-group-item-action d-flex justify-content-between align-items-center py-3">
                            <div>
                                <strong class="text-dark d-block">20. Mandi Batch Specifications</strong>
                                <small class="text-muted"><code>MarketDetails.aspx</code> • Consignment quality details & origin trace</small>
                            </div>
                            <span class="badge bg-light text-primary border">Visit &rarr;</span>
                        </a>
                        <a href="ProductDetails.aspx" class="list-group-item list-group-item-action d-flex justify-content-between align-items-center py-3">
                            <div>
                                <strong class="text-dark d-block">21. Product Checkout View</strong>
                                <small class="text-muted"><code>ProductDetails.aspx</code> • Quintal pricing, seller badge & Buy Now</small>
                            </div>
                            <span class="badge bg-light text-primary border">Visit &rarr;</span>
                        </a>
                        <a href="MyPurchases.aspx" class="list-group-item list-group-item-action d-flex justify-content-between align-items-center py-3">
                            <div>
                                <strong class="text-dark d-block">22. My Orders & Purchases</strong>
                                <small class="text-muted"><code>MyPurchases.aspx</code> • Historical orders & real-time dispatch status</small>
                            </div>
                            <span class="badge bg-light text-primary border">Visit &rarr;</span>
                        </a>
                        <a href="OrderDetails.aspx" class="list-group-item list-group-item-action d-flex justify-content-between align-items-center py-3">
                            <div>
                                <strong class="text-dark d-block">23. Order Invoice Details</strong>
                                <small class="text-muted"><code>OrderDetails.aspx</code> • Freight breakdown & escrow payment receipt</small>
                            </div>
                            <span class="badge bg-light text-primary border">Visit &rarr;</span>
                        </a>
                        <a href="Wishlist.aspx" class="list-group-item list-group-item-action d-flex justify-content-between align-items-center py-3">
                            <div>
                                <strong class="text-dark d-block">24. Buyer Wishlist</strong>
                                <small class="text-muted"><code>Wishlist.aspx</code> • Saved favorite crop lots & seasonal alerts</small>
                            </div>
                            <span class="badge bg-light text-primary border">Visit &rarr;</span>
                        </a>
                    </div>
                </div>
            </div>

            <!-- 5. OPERATOR & SERVICES MODULE (DHARM) -->
            <div class="col-lg-6">
                <div class="card h-100 border rounded-3 p-4 bg-white shadow-sm" style="border-top: 4px solid #f59e0b !important;">
                    <div class="d-flex justify-content-between align-items-center mb-3">
                        <h4 class="fw-bold text-dark mb-0 fs-5"><i class="bi bi-truck me-2 text-warning"></i>Operator & Agri Services</h4>
                        <span class="badge bg-warning text-dark">Dharm (8 Pages)</span>
                    </div>
                    <div class="list-group list-group-flush">
                        <a href="RegisterOperator.aspx" class="list-group-item list-group-item-action d-flex justify-content-between align-items-center py-3">
                            <div>
                                <strong class="text-dark d-block">25. Register as Operator</strong>
                                <small class="text-muted"><code>RegisterOperator.aspx</code> • Machinery horsepower & hourly rate setup</small>
                            </div>
                            <span class="badge bg-light text-warning border">Visit &rarr;</span>
                        </a>
                        <a href="TractorRentals.aspx" class="list-group-item list-group-item-action d-flex justify-content-between align-items-center py-3">
                            <div>
                                <strong class="text-dark d-block">26. Tractor & Machinery Rentals</strong>
                                <small class="text-muted"><code>TractorRentals.aspx</code> • Harvester & tractor catalog with hourly rates</small>
                            </div>
                            <span class="badge bg-light text-warning border">Visit &rarr;</span>
                        </a>
                        <a href="MyBookings.aspx" class="list-group-item list-group-item-action d-flex justify-content-between align-items-center py-3">
                            <div>
                                <strong class="text-dark d-block">27. Operator Service Bookings</strong>
                                <small class="text-muted"><code>MyBookings.aspx</code> • Service appointments & verified payouts log</small>
                            </div>
                            <span class="badge bg-light text-warning border">Visit &rarr;</span>
                        </a>
                        <a href="BookingConfirm.aspx" class="list-group-item list-group-item-action d-flex justify-content-between align-items-center py-3">
                            <div>
                                <strong class="text-dark d-block">28. Machinery Booking Confirm</strong>
                                <small class="text-muted"><code>BookingConfirm.aspx</code> • Hours calculation & escrow deposit checkout</small>
                            </div>
                            <span class="badge bg-light text-warning border">Visit &rarr;</span>
                        </a>
                        <a href="AgriServices.aspx" class="list-group-item list-group-item-action d-flex justify-content-between align-items-center py-3">
                            <div>
                                <strong class="text-dark d-block">29. Soil Testing & Drone Services</strong>
                                <small class="text-muted"><code>AgriServices.aspx</code> • Soil nutrient lab bookings & drone spraying</small>
                            </div>
                            <span class="badge bg-light text-warning border">Visit &rarr;</span>
                        </a>
                        <a href="ApmcRates.aspx" class="list-group-item list-group-item-action d-flex justify-content-between align-items-center py-3">
                            <div>
                                <strong class="text-dark d-block">30. Live Gujarat APMC Rates</strong>
                                <small class="text-muted"><code>ApmcRates.aspx</code> • Real-time mandi yard minimum & maximum prices</small>
                            </div>
                            <span class="badge bg-light text-warning border">Visit &rarr;</span>
                        </a>
                        <a href="WeatherForecast.aspx" class="list-group-item list-group-item-action d-flex justify-content-between align-items-center py-3">
                            <div>
                                <strong class="text-dark d-block">31. Weather Forecast & Alerts</strong>
                                <small class="text-muted"><code>WeatherForecast.aspx</code> • Sowing rain probability & micro-climate alerts</small>
                            </div>
                            <span class="badge bg-light text-warning border">Visit &rarr;</span>
                        </a>
                        <a href="Community.aspx" class="list-group-item list-group-item-action d-flex justify-content-between align-items-center py-3">
                            <div>
                                <strong class="text-dark d-block">32. Farmer Community Forum</strong>
                                <small class="text-muted"><code>Community.aspx</code> • Peer-to-peer discussion & pest control advice</small>
                            </div>
                            <span class="badge bg-light text-warning border">Visit &rarr;</span>
                        </a>
                    </div>
                </div>
            </div>

            <!-- 6. ACCOUNT & PROFILE SETTINGS -->
            <div class="col-lg-6">
                <div class="card h-100 border rounded-3 p-4 bg-white shadow-sm" style="border-top: 4px solid #8b5cf6 !important;">
                    <div class="d-flex justify-content-between align-items-center mb-3">
                        <h4 class="fw-bold text-dark mb-0 fs-5"><i class="bi bi-person-gear me-2 text-primary"></i>Account & Security</h4>
                        <span class="badge bg-primary">Shared (2 Pages)</span>
                    </div>
                    <div class="list-group list-group-flush">
                        <a href="ProfileSettings.aspx" class="list-group-item list-group-item-action d-flex justify-content-between align-items-center py-3">
                            <div>
                                <strong class="text-dark d-block">33. User Profile & Password Security</strong>
                                <small class="text-muted"><code>ProfileSettings.aspx</code> • Unified profile settings, avatar & password update</small>
                            </div>
                            <span class="badge bg-light text-primary border">Visit &rarr;</span>
                        </a>
                        <a href="SiteDirectory.aspx" class="list-group-item list-group-item-action d-flex justify-content-between align-items-center py-3 active bg-light text-dark">
                            <div>
                                <strong class="text-dark d-block">34. Site Directory & Navigation Hub</strong>
                                <small class="text-muted"><code>SiteDirectory.aspx</code> • Current master directory linking all pages</small>
                            </div>
                            <span class="badge bg-success text-white">Active</span>
                        </a>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
