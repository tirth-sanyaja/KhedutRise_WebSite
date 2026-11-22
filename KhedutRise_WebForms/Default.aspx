<%@ Page Title="Empowering Farmers" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="KhedutRise.Default" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- HERO SECTION -->
    <section class="position-relative text-white py-5" style="background: linear-gradient(to right, rgba(0,0,0,0.7) 0%, rgba(0,0,0,0.4) 100%), url('Images/hero_farmer.jpg') center/cover no-repeat; min-height: 520px; display: flex; align-items: center;">
        <div class="kr-page-container w-100">
            <div class="col-lg-8" style="text-shadow: 0 2px 10px rgba(0, 0, 0, 0.65);">
                <div class="d-inline-flex align-items-center gap-2 px-3 py-1 rounded-pill bg-success text-white small fw-bold mb-3 shadow-sm">
                    <i class="bi bi-shield-check"></i> 100% Verified Gujarat Agricultural Portal
                </div>
                <h1 class="display-4 fw-bold mb-3 text-white" style="letter-spacing: -0.5px; line-height: 1.15;">
                    Empowering Farmers.<br />Connecting Opportunities.
                </h1>
                <p class="lead mb-4 fs-5 text-white" style="max-width: 650px; line-height: 1.5;">
                    KhedutRise brings farmers, wholesale buyers, agricultural markets, farmland, and tractor services together in one secure, connected ecosystem.
                </p>
                
                <!-- Action Buttons -->
                <div class="d-flex flex-wrap gap-3">
                    <a href="SelectRole.aspx" class="btn btn-kr-primary btn-lg px-4 shadow rounded-pill fw-bold" style="background-color: #0b6028 !important; border: none; padding: 0.65rem 2rem;">
                        <i class="bi bi-person-plus me-1"></i> Register Account
                    </a>
                    <a href="Marketplace.aspx" class="btn btn-outline-light btn-lg px-4 shadow-sm fw-bold rounded-pill" style="border: 2px solid #ffffff; color: #ffffff; padding: 0.65rem 2rem;">
                        <i class="bi bi-shop me-1"></i> Explore Marketplace
                    </a>
                    <a href="ApmcRates.aspx" class="btn btn-warning btn-lg px-4 shadow-sm fw-bold rounded-pill text-dark" style="padding: 0.65rem 1.75rem;">
                        <i class="bi bi-graph-up-arrow me-1"></i> Live Mandi Rates
                    </a>
                </div>
            </div>
        </div>
    </section>

    <!-- KEY STATS SECTION: Dynamic Platform Metrics -->
    <section class="bg-white border-bottom py-4 shadow-sm">
        <div class="kr-page-container">
            <div class="row text-center align-items-center g-3 g-md-0">
                <div class="col-6 col-md-3 border-end-md">
                    <h2 class="fw-bold mb-0 fs-1" style="color: #0b6028;">
                        <asp:Literal ID="litFarmerCount" runat="server">50,000+</asp:Literal>
                    </h2>
                    <p class="text-muted small mb-0 fw-semibold">Farmers Onboarded</p>
                </div>
                <div class="col-6 col-md-3 border-end-md">
                    <h2 class="fw-bold mb-0 fs-1" style="color: #0284c7;">
                        <asp:Literal ID="litProductCount" runat="server">1,000+</asp:Literal>
                    </h2>
                    <p class="text-muted small mb-0 fw-semibold">Live Crops Listed</p>
                </div>
                <div class="col-6 col-md-3 border-end-md">
                    <h2 class="fw-bold mb-0 fs-1" style="color: #ea580c;">
                        <asp:Literal ID="litApmcCount" runat="server">16+ Yards</asp:Literal>
                    </h2>
                    <p class="text-muted small mb-0 fw-semibold">Live APMC Mandis</p>
                </div>
                <div class="col-6 col-md-3">
                    <h2 class="fw-bold mb-0 fs-1" style="color: #15803d;">
                        <asp:Literal ID="litServiceCount" runat="server">100+ Services</asp:Literal>
                    </h2>
                    <p class="text-muted small mb-0 fw-semibold">Tractor & Agri Services</p>
                </div>
            </div>
        </div>
    </section>

    <!-- 3 PILLARS SECTION -->
    <section class="py-5" style="background-color: #f8fafc;">
        <div class="kr-page-container">
            <div class="text-center mb-4">
                <small class="text-uppercase fw-bold text-success" style="font-size: 0.78rem; letter-spacing: 1px;">Core Agricultural Solutions</small>
                <h3 class="fw-bold text-dark mt-1">Everything You Need To Grow &amp; Trade</h3>
            </div>

            <div class="row g-4">
                <!-- Pillar 1: Live APMC Rates Card -->
                <div class="col-md-4">
                    <div class="card card-custom h-100 p-4 border shadow-sm rounded-3">
                        <div class="d-flex gap-3 mb-3">
                            <img src="Images/apmc_yard.jpg" class="rounded-3 object-fit-cover shadow-sm" style="width: 110px; height: 85px; flex-shrink: 0;" alt="Live APMC Rates" />
                            <div>
                                <h5 class="fw-bold mb-1 text-dark">Live APMC Mandi Rates</h5>
                                <p class="text-muted small mb-0" style="line-height: 1.4;">Real-time auction prices from Rajkot, Gondal, Unjha, and major Gujarat yards.</p>
                            </div>
                        </div>
                        <a href="ApmcRates.aspx" class="text-success fw-bold text-decoration-none mt-auto">
                            Check Live APMC Rates <i class="bi bi-arrow-right"></i>
                        </a>
                    </div>
                </div>

                <!-- Pillar 2: Farmer Marketplace Card -->
                <div class="col-md-4">
                    <div class="card card-custom h-100 p-4 border shadow-sm rounded-3">
                        <div class="d-flex gap-3 mb-3">
                            <img src="Images/groundnut.jpg" class="rounded-3 object-fit-cover shadow-sm" style="width: 110px; height: 85px; flex-shrink: 0;" alt="Farmer Marketplace" />
                            <div>
                                <h5 class="fw-bold mb-1 text-dark">Fresh Produce Marketplace</h5>
                                <p class="text-muted small mb-0" style="line-height: 1.4;">Direct trade platform for groundnut, Bt cotton, wheat, cumin, and fresh vegetables.</p>
                            </div>
                        </div>
                        <a href="Marketplace.aspx" class="text-success fw-bold text-decoration-none mt-auto">
                            Explore Crop Listings <i class="bi bi-arrow-right"></i>
                        </a>
                    </div>
                </div>

                <!-- Pillar 3: Land & Tractor Rentals Card -->
                <div class="col-md-4">
                    <div class="card card-custom h-100 p-4 border shadow-sm rounded-3">
                        <div class="d-flex gap-3 mb-3">
                            <img src="Images/green_rows.jpg" class="rounded-3 object-fit-cover shadow-sm" style="width: 110px; height: 85px; flex-shrink: 0;" alt="Land & Tractors" />
                            <div>
                                <h5 class="fw-bold mb-1 text-dark">Land Portal &amp; Machinery</h5>
                                <p class="text-muted small mb-0" style="line-height: 1.4;">Verified farmland sale listings and nearby modern tractor rentals on hourly basis.</p>
                            </div>
                        </div>
                        <a href="TractorRentals.aspx" class="text-success fw-bold text-decoration-none mt-auto">
                            Book Tractor &amp; Services <i class="bi bi-arrow-right"></i>
                        </a>
                    </div>
                </div>
            </div>
        </div>
    </section>

    <!-- DYNAMIC SECTION: Live Mandi Rates Snapshot -->
    <asp:Panel ID="pnlLiveMandi" runat="server" Visible="false" CssClass="py-5 bg-white border-top border-bottom">
        <div class="kr-page-container">
            <div class="d-flex flex-wrap justify-content-between align-items-center mb-4">
                <div>
                    <span class="badge bg-danger-subtle text-danger px-2 py-1 rounded-pill small fw-bold mb-1">
                        <i class="bi bi-broadcast me-1"></i> LIVE TELEMETRY
                    </span>
                    <h3 class="fw-bold text-dark mb-0">Gujarat Mandi Rates Snapshot</h3>
                    <p class="text-muted small mb-0">Real-time daily arrivals and modal auction rates across Gujarat APMC yards.</p>
                </div>
                <div class="mt-2 mt-md-0">
                    <a href="ApmcRates.aspx" class="btn btn-outline-success btn-sm px-3 fw-bold rounded-pill">
                        View All Mandi Rates <i class="bi bi-arrow-right"></i>
                    </a>
                </div>
            </div>

            <div class="row g-3">
                <asp:Repeater ID="rptLiveMandiRates" runat="server">
                    <ItemTemplate>
                        <div class="col-md-6 col-lg-4">
                            <div class="p-3 rounded-3 border bg-light h-100 d-flex flex-column justify-content-between">
                                <div class="d-flex justify-content-between align-items-start mb-2">
                                    <div>
                                        <h6 class="fw-bold text-dark mb-0"><%# Eval("Commodity") %></h6>
                                        <small class="text-muted"><i class="bi bi-geo-alt me-1"></i><%# Eval("MarketYard") %></small>
                                    </div>
                                    <span class="badge bg-success-subtle text-success px-2 py-1 rounded-pill font-monospace fw-bold">
                                        &#8377;<%# Convert.ToDecimal(Eval("AvgPrice")).ToString("N0") %> / Q
                                    </span>
                                </div>
                                <div class="d-flex justify-content-between align-items-center pt-2 border-top small text-muted">
                                    <span>Min: &#8377;<%# Convert.ToDecimal(Eval("MinPrice")).ToString("N0") %></span>
                                    <span>Max: &#8377;<%# Convert.ToDecimal(Eval("MaxPrice")).ToString("N0") %></span>
                                    <span>Arrivals: <strong><%# Eval("ArrivalTons") %> Tons</strong></span>
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
    </asp:Panel>

    <!-- DYNAMIC SECTION: Featured Crops Direct from Farmers -->
    <asp:Panel ID="pnlFeaturedCrops" runat="server" Visible="false" CssClass="py-5" style="background-color: #f8fafc;">
        <div class="kr-page-container">
            <div class="d-flex flex-wrap justify-content-between align-items-center mb-4">
                <div>
                    <span class="badge bg-success-subtle text-success px-2 py-1 rounded-pill small fw-bold mb-1">
                        <i class="bi bi-bag-check me-1"></i> FRESH HARVEST
                    </span>
                    <h3 class="fw-bold text-dark mb-0">Featured Crops for Direct Purchase</h3>
                    <p class="text-muted small mb-0">High-grade agricultural produce listed directly by verified Gujarati farmers.</p>
                </div>
                <div class="mt-2 mt-md-0">
                    <a href="Marketplace.aspx" class="btn btn-kr-primary btn-sm px-3 fw-bold rounded-pill text-white" style="background-color: #0b6028;">
                        Browse All Crops <i class="bi bi-arrow-right"></i>
                    </a>
                </div>
            </div>

            <div class="row g-4">
                <asp:Repeater ID="rptFeaturedCrops" runat="server">
                    <ItemTemplate>
                        <div class="col-sm-6 col-lg-3">
                            <div class="card h-100 border rounded-3 shadow-sm overflow-hidden bg-white d-flex flex-column">
                                <div class="position-relative">
                                    <img src="<%# Eval("ImagePath") %>" class="w-100 object-fit-cover" style="height: 180px;" alt="<%# Eval("ProductName") %>" onerror="this.src='Images/groundnut.jpg';" />
                                    <span class="badge bg-success position-absolute top-0 start-0 m-2 px-2 py-1 shadow-sm">
                                        <%# Eval("Category") %>
                                    </span>
                                    <span class="badge bg-dark bg-opacity-75 position-absolute bottom-0 end-0 m-2 px-2 py-1 small">
                                        <%# Eval("Grade") %>
                                    </span>
                                </div>
                                <div class="p-3 d-flex flex-column flex-grow-1">
                                    <h6 class="fw-bold text-dark mb-1 text-truncate" title="<%# Eval("ProductName") %>"><%# Eval("ProductName") %></h6>
                                    <small class="text-muted d-block mb-2"><i class="bi bi-person me-1"></i><%# Eval("FarmerName") %> &bull; <%# Eval("Location") %></small>
                                    
                                    <div class="d-flex justify-content-between align-items-baseline mb-3 mt-auto">
                                        <div>
                                            <span class="fs-5 fw-bold" style="color: #0b6028;">&#8377;<%# Convert.ToDecimal(Eval("PricePerQuintal")).ToString("N0") %></span>
                                            <span class="text-muted small">/ Q</span>
                                        </div>
                                        <small class="text-muted">Stock: <strong><%# Eval("QuantityInQuintal") %> Q</strong></small>
                                    </div>

                                    <a href="ProductDetails.aspx?id=<%# Eval("ProductId") %>" class="btn btn-outline-success btn-sm w-100 fw-bold rounded-2">
                                        View &amp; Buy Now
                                    </a>
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
    </asp:Panel>

    <!-- DYNAMIC SECTION: Modern Tractor & Machinery Spotlight -->
    <asp:Panel ID="pnlMachinerySpotlight" runat="server" Visible="false" CssClass="py-5 bg-white border-top">
        <div class="kr-page-container">
            <div class="d-flex flex-wrap justify-content-between align-items-center mb-4">
                <div>
                    <span class="badge bg-warning-subtle text-warning px-2 py-1 rounded-pill small fw-bold mb-1">
                        <i class="bi bi-truck me-1"></i> FIELD MECHANIZATION
                    </span>
                    <h3 class="fw-bold text-dark mb-0">Tractor &amp; Harvester Rentals</h3>
                    <p class="text-muted small mb-0">Book verified tractors with implements from local machinery operators.</p>
                </div>
                <div class="mt-2 mt-md-0">
                    <a href="TractorRentals.aspx" class="btn btn-outline-dark btn-sm px-3 fw-bold rounded-pill">
                        All Machinery <i class="bi bi-arrow-right"></i>
                    </a>
                </div>
            </div>

            <div class="row g-4">
                <asp:Repeater ID="rptFeaturedMachinery" runat="server">
                    <ItemTemplate>
                        <div class="col-md-4">
                            <div class="card h-100 border rounded-3 shadow-sm overflow-hidden bg-white">
                                <img src="<%# Eval("ImagePath") %>" class="w-100 object-fit-cover" style="height: 190px;" alt="<%# Eval("ModelName") %>" onerror="this.src='Images/massey_tractor.jpg';" />
                                <div class="p-3 d-flex flex-column flex-grow-1">
                                    <div class="d-flex justify-content-between align-items-start mb-1">
                                        <h5 class="fw-bold text-dark mb-0"><%# Eval("ModelName") %></h5>
                                        <span class="badge bg-success-subtle text-success fw-bold"><%# Eval("Horsepower") %></span>
                                    </div>
                                    <small class="text-muted mb-3"><i class="bi bi-geo-alt me-1"></i><%# Eval("Location") %> &bull; Owner: <%# Eval("OwnerName") %></small>

                                    <div class="d-flex justify-content-between align-items-center mt-auto pt-2 border-top">
                                        <div>
                                            <span class="fs-5 fw-bold text-dark">&#8377;<%# Convert.ToDecimal(Eval("HourlyRate")).ToString("N0") %></span>
                                            <span class="text-muted small">/ hr</span>
                                        </div>
                                        <a href="BookingConfirm.aspx?eqId=<%# Eval("EquipmentId") %>" class="btn btn-sm text-white fw-bold px-3 rounded-2" style="background-color: #0b6028;">
                                            Book Now
                                        </a>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
    </asp:Panel>
</asp:Content>
