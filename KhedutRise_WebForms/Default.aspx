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

    <!-- KEY STATS SECTION: Horizontal divided metrics card -->
    <section class="bg-white border-bottom py-4 shadow-sm">
        <div class="kr-page-container">
            <div class="row text-center align-items-center g-0">
                <div class="col-6 col-md-3 border-end">
                    <h2 class="fw-bold text-dark mb-0 fs-1">50K+</h2>
                    <p class="text-muted small mb-0 fw-semibold">Farmers Onboarded</p>
                </div>
                <div class="col-6 col-md-3 border-end">
                    <h2 class="fw-bold text-dark mb-0 fs-1">10K+</h2>
                    <p class="text-muted small mb-0 fw-semibold">Products Listed</p>
                </div>
                <div class="col-6 col-md-3 border-end">
                    <h2 class="fw-bold text-dark mb-0 fs-1">50+</h2>
                    <p class="text-muted small mb-0 fw-semibold">Live APMC Markets</p>
                </div>
                <div class="col-6 col-md-3">
                    <h2 class="fw-bold text-dark mb-0 fs-1">100+</h2>
                    <p class="text-muted small mb-0 fw-semibold">Agri Services</p>
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
