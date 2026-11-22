<%@ Page Title="Empowering Farmers" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="KhedutRise.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- HERO SECTION -->
    <section class="position-relative text-white py-5" style="background: url('Images/hero_farmer.jpg') center/cover no-repeat; min-height: 520px; display: flex; align-items: center;">
        <div class="kr-page-container">
            <div class="col-lg-8" style="text-shadow: 0 2px 10px rgba(0, 0, 0, 0.65);">
                <h1 class="display-3 fw-bold mb-3 text-white" style="letter-spacing: -0.5px;">Empowering Farmers.<br />Connecting Opportunities.</h1>
                <p class="lead mb-4 fs-5 text-white" style="max-width: 650px; line-height: 1.5;">KhedutRise brings farmers, agricultural markets, products, land and essential services together in one connected platform.</p>
                
                <!-- Action Buttons: Rounded Pill CTA buttons -->
                <div class="d-flex flex-wrap gap-3">
                    <a href="SelectRole.aspx" class="btn btn-kr-primary btn-lg px-4 shadow rounded-pill fw-bold" style="background-color: #0b6028 !important; border: none; padding: 0.65rem 2rem;">Get Started</a>
                    <a href="Marketplace.aspx" class="btn btn-outline-light btn-lg px-4 shadow-sm fw-bold rounded-pill" style="border: 2px solid #ffffff; color: #ffffff; padding: 0.65rem 2rem;">Explore Marketplace</a>
                </div>
            </div>
        </div>
    </section>

    <!-- KEY STATS & ABOUT KHEDUTRISE SECTION -->
    <section class="bg-white border-bottom py-5 shadow-sm">
        <div class="kr-page-container">
            <!-- About KhedutRise Introduction -->
            <div class="text-center mb-4">
                <span class="badge px-3 py-1 rounded-pill fw-bold text-uppercase mb-2" style="background-color: #ecfdf5; color: #0b6028; font-size: 0.8rem; letter-spacing: 0.5px; border: 1px solid #a7f3d0;">
                    <i class="bi bi-patch-check-fill me-1"></i> About KhedutRise &bull; ગુજરાતનું પોતાનું ડિજિટલ એગ્રી પ્લેટફોર્મ
                </span>
                <h3 class="fw-bold text-dark mt-1 mb-2" style="font-size: 1.85rem; letter-spacing: -0.5px;">
                    Empowering Gujarat's Agricultural Ecosystem
                </h3>
                <p class="text-secondary mx-auto mb-0" style="max-width: 720px; font-size: 0.98rem; line-height: 1.6;">
                    <strong>KhedutRise</strong> is a dedicated digital agri-commerce platform designed to connect farmers directly with wholesale buyers, transparent APMC market prices, farm machinery rentals, and verified agricultural land &mdash; ensuring fair pricing, zero middleman exploitation, and digital prosperity for every khedut.
                </p>
            </div>

            <!-- 4 Key Impact Stat Cards -->
            <div class="row g-3 text-center align-items-stretch">
                <!-- Stat 1: Farmers -->
                <div class="col-6 col-md-3">
                    <div class="p-4 rounded-3 border bg-light h-100 d-flex flex-column align-items-center shadow-none" style="border-color: #e2e8f0 !important;">
                        <div class="rounded-circle d-flex align-items-center justify-content-center mb-3 shadow-sm" style="width: 52px; height: 52px; background-color: #ecfdf5; color: #0b6028;">
                            <i class="bi bi-people-fill fs-4"></i>
                        </div>
                        <h2 class="fw-bold mb-1 fs-1" style="color: #0b6028;">50K+</h2>
                        <p class="text-dark fw-bold small mb-1">Farmers Onboarded</p>
                        <small class="text-muted" style="font-size: 0.78rem;">Across 33 Gujarat Districts</small>
                    </div>
                </div>

                <!-- Stat 2: Products Listed -->
                <div class="col-6 col-md-3">
                    <div class="p-4 rounded-3 border bg-light h-100 d-flex flex-column align-items-center shadow-none" style="border-color: #e2e8f0 !important;">
                        <div class="rounded-circle d-flex align-items-center justify-content-center mb-3 shadow-sm" style="width: 52px; height: 52px; background-color: #eff6ff; color: #0284c7;">
                            <i class="bi bi-basket2-fill fs-4"></i>
                        </div>
                        <h2 class="fw-bold mb-1 fs-1" style="color: #0284c7;">10K+</h2>
                        <p class="text-dark fw-bold small mb-1">Products Listed</p>
                        <small class="text-muted" style="font-size: 0.78rem;">Cotton, Groundnut &amp; Cumin</small>
                    </div>
                </div>

                <!-- Stat 3: APMC Markets -->
                <div class="col-6 col-md-3">
                    <div class="p-4 rounded-3 border bg-light h-100 d-flex flex-column align-items-center shadow-none" style="border-color: #e2e8f0 !important;">
                        <div class="rounded-circle d-flex align-items-center justify-content-center mb-3 shadow-sm" style="width: 52px; height: 52px; background-color: #fff7ed; color: #ea580c;">
                            <i class="bi bi-graph-up-arrow fs-4"></i>
                        </div>
                        <h2 class="fw-bold mb-1 fs-1" style="color: #ea580c;">50+</h2>
                        <p class="text-dark fw-bold small mb-1">Live APMC Markets</p>
                        <small class="text-muted" style="font-size: 0.78rem;">Rajkot, Gondal, Unjha Mandis</small>
                    </div>
                </div>

                <!-- Stat 4: Agri Services -->
                <div class="col-6 col-md-3">
                    <div class="p-4 rounded-3 border bg-light h-100 d-flex flex-column align-items-center shadow-none" style="border-color: #e2e8f0 !important;">
                        <div class="rounded-circle d-flex align-items-center justify-content-center mb-3 shadow-sm" style="width: 52px; height: 52px; background-color: #fdf4ff; color: #a855f7;">
                            <i class="bi bi-truck fs-4"></i>
                        </div>
                        <h2 class="fw-bold mb-1 fs-1" style="color: #a855f7;">100+</h2>
                        <p class="text-dark fw-bold small mb-1">Agri Services</p>
                        <small class="text-muted" style="font-size: 0.78rem;">Tractor &amp; Harvester Rentals</small>
                    </div>
                </div>
            </div>

            <!-- 4 Key Highlights Badge Strip -->
            <div class="row g-2 mt-3 pt-3 border-top text-center">
                <div class="col-6 col-md-3">
                    <span class="small fw-semibold text-secondary">
                        <i class="bi bi-check-circle-fill text-success me-1"></i> Direct Farmer Trade
                    </span>
                </div>
                <div class="col-6 col-md-3">
                    <span class="small fw-semibold text-secondary">
                        <i class="bi bi-check-circle-fill text-success me-1"></i> Daily Mandi Updates
                    </span>
                </div>
                <div class="col-6 col-md-3">
                    <span class="small fw-semibold text-secondary">
                        <i class="bi bi-check-circle-fill text-success me-1"></i> On-Demand Machinery
                    </span>
                </div>
                <div class="col-6 col-md-3">
                    <span class="small fw-semibold text-secondary">
                        <i class="bi bi-check-circle-fill text-success me-1"></i> Verified Farm Listings
                    </span>
                </div>
            </div>
        </div>
    </section>

    <!-- 3 PILLARS SECTION: 3 Horizontal Cards -->
    <section class="py-5" style="background-color: #f8fafc;">
        <div class="kr-page-container">
            <div class="row g-4">
                <!-- Pillar 1: Live APMC Rates Card -->
                <div class="col-md-4">
                    <div class="card card-custom h-100 p-4 border shadow-sm">
                        <div class="d-flex gap-3 mb-3">
                            <img src="Images/apmc_yard.jpg" class="rounded-3 object-fit-cover shadow-sm" style="width: 120px; height: 90px; flex-shrink: 0;" alt="Live APMC Rates" />
                            <div>
                                <h5 class="fw-bold mb-1 text-dark">Live APMC Rates</h5>
                                <p class="text-muted small mb-0" style="line-height: 1.4;">Real-time market rates from Gujarat yards.</p>
                            </div>
                        </div>
                        <a href="ApmcRates.aspx" class="text-success fw-bold text-decoration-none mt-auto" style="color: #0b6028 !important;">Check APMC Rates &gt;</a>
                    </div>
                </div>

                <!-- Pillar 2: Farmer Marketplace Card -->
                <div class="col-md-4">
                    <div class="card card-custom h-100 p-4 border shadow-sm">
                        <div class="d-flex gap-3 mb-3">
                            <img src="Images/groundnut.jpg" class="rounded-3 object-fit-cover shadow-sm" style="width: 120px; height: 90px; flex-shrink: 0;" alt="Farmer Marketplace" />
                            <div>
                                <h5 class="fw-bold mb-1 text-dark">Farmer Marketplace</h5>
                                <p class="text-muted small mb-0" style="line-height: 1.4;">Direct trade platform for cotton, groundnut & seeds.</p>
                            </div>
                        </div>
                        <a href="Marketplace.aspx" class="text-success fw-bold text-decoration-none mt-auto" style="color: #0b6028 !important;">Explore Crops &gt;</a>
                    </div>
                </div>

                <!-- Pillar 3: Land Buy & Sell Card -->
                <div class="col-md-4">
                    <div class="card card-custom h-100 p-4 border shadow-sm">
                        <div class="d-flex gap-3 mb-3">
                            <img src="Images/green_rows.jpg" class="rounded-3 object-fit-cover shadow-sm" style="width: 120px; height: 90px; flex-shrink: 0;" alt="Land Buy & Sell" />
                            <div>
                                <h5 class="fw-bold mb-1 text-dark">Land Buy & Sell</h5>
                                <p class="text-muted small mb-0" style="line-height: 1.4;">Verified agricultural land listings near highways.</p>
                            </div>
                        </div>
                        <a href="LandPortal.aspx" class="text-success fw-bold text-decoration-none mt-auto" style="color: #0b6028 !important;">View Land Listings &gt;</a>
                    </div>
                </div>
            </div>
        </div>
    </section>
</asp:Content>
