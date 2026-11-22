<%@ Page Title="APMC Market Yard Details" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="MarketDetails.aspx.cs" Inherits="KhedutRise.MarketDetails" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- APMC Market Yard Details: 100% Dynamic -->
    <div class="kr-page-container py-4">
        <!-- Hero Banner -->
        <div class="card card-custom p-4 p-md-5 mb-4 text-white position-relative overflow-hidden" style="background: linear-gradient(rgba(0,0,0,0.65), rgba(0,0,0,0.75)), url('Images/apmc_yard.jpg') center/cover no-repeat; min-height: 220px; border-radius: 12px;">
            <small class="text-light opacity-90"><a href="Default.aspx" class="text-decoration-none text-light">Home</a> / <a href="ApmcRates.aspx" class="text-decoration-none text-light">APMC Rates</a> / Market Details</small>
            <h2 class="display-6 fw-bold mt-2 text-white">
                <asp:Literal ID="litHeroMarketName" runat="server">Rajkot APMC Market Yard</asp:Literal>
            </h2>
            <p class="small text-white-50 mb-2">
                <i class="bi bi-clock-history me-1"></i> <asp:Literal ID="litTiming" runat="server">08:00 AM - 05:00 PM (Mon - Sat)</asp:Literal>
            </p>
            <div>
                <span class="badge bg-warning text-dark fw-bold px-3 py-1 shadow-sm" style="font-size: 0.8rem;">
                    <i class="bi bi-shield-check me-1"></i> Government Regulated Gujarat Agricultural Mandi
                </span>
            </div>
        </div>

        <!-- Filter / Switch Market Yard Bar -->
        <div class="card p-3 bg-white border rounded-3 mb-4 shadow-sm">
            <div class="row g-3 align-items-center">
                <div class="col-md-6">
                    <label class="form-label fw-bold text-dark mb-0 small">
                        <i class="bi bi-funnel text-success me-1"></i> SWITCH GUJARAT APMC MANDI YARD:
                    </label>
                </div>
                <div class="col-md-6">
                    <asp:DropDownList ID="ddlMarketYards" runat="server" AutoPostBack="true" 
                        OnSelectedIndexChanged="ddlMarketYards_SelectedIndexChanged" CssClass="form-select fw-semibold border-success">
                    </asp:DropDownList>
                </div>
            </div>
        </div>

        <asp:Label ID="lblMsg" runat="server" CssClass="d-block mb-3 fw-bold" EnableViewState="false"></asp:Label>

        <!-- KPI Metrics for the Yard -->
        <div class="row g-3 mb-4">
            <div class="col-6 col-md-4">
                <div class="card p-3 border rounded-3 bg-white shadow-sm">
                    <small class="text-muted fw-bold" style="font-size: 0.72rem; letter-spacing: 0.5px;">COMMODITIES TRADED</small>
                    <h4 class="fw-bold mt-1 mb-0" style="color: #0b6028;">
                        <asp:Literal ID="litCommodityCount" runat="server">0 Commodities</asp:Literal>
                    </h4>
                </div>
            </div>
            <div class="col-6 col-md-4">
                <div class="card p-3 border rounded-3 bg-white shadow-sm">
                    <small class="text-muted fw-bold" style="font-size: 0.72rem; letter-spacing: 0.5px;">TODAY'S ESTIMATED ARRIVALS</small>
                    <h4 class="fw-bold mt-1 mb-0" style="color: #0284c7;">
                        <asp:Literal ID="litTotalArrivals" runat="server">0 Tons</asp:Literal>
                    </h4>
                </div>
            </div>
            <div class="col-12 col-md-4">
                <div class="card p-3 border rounded-3 bg-white shadow-sm">
                    <small class="text-muted fw-bold" style="font-size: 0.72rem; letter-spacing: 0.5px;">YARD MODAL PRICE INDEX</small>
                    <h4 class="fw-bold mt-1 mb-0" style="color: #ea580c;">
                        <asp:Literal ID="litAvgPriceIndex" runat="server">₹0</asp:Literal>
                    </h4>
                </div>
            </div>
        </div>

        <div class="row g-4">
            <!-- LEFT: Today's Arrivals & Trading Prices Table -->
            <div class="col-lg-8">
                <div class="card p-4 border rounded-3 bg-white shadow-sm">
                    <div class="d-flex justify-content-between align-items-center mb-3">
                        <h5 class="fw-bold text-dark mb-0">Today's Live Commodity Auction Rates</h5>
                        <small class="text-muted"><i class="bi bi-clock me-1"></i>Updated Daily</small>
                    </div>

                    <div class="table-responsive">
                        <asp:Repeater ID="rptCommodities" runat="server">
                            <HeaderTemplate>
                                <table class="table table-hover align-middle mb-0">
                                    <thead class="bg-light text-muted" style="font-size: 0.75rem; letter-spacing: 0.5px;">
                                        <tr>
                                            <th>COMMODITY</th>
                                            <th>ARRIVALS (TONS)</th>
                                            <th>MIN PRICE</th>
                                            <th>MAX PRICE</th>
                                            <th>MODAL / AVG</th>
                                            <th>ACTION</th>
                                        </tr>
                                    </thead>
                                    <tbody style="font-size: 0.92rem;">
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <strong class="text-dark"><%# Eval("Commodity") %></strong>
                                    </td>
                                    <td>
                                        <span class="badge bg-light text-dark border"><%# Eval("ArrivalTons") %> Tons</span>
                                    </td>
                                    <td class="text-muted">&#8377;<%# Convert.ToDecimal(Eval("MinPrice")).ToString("N0") %></td>
                                    <td class="text-muted">&#8377;<%# Convert.ToDecimal(Eval("MaxPrice")).ToString("N0") %></td>
                                    <td>
                                        <strong class="text-success">&#8377;<%# Convert.ToDecimal(Eval("AvgPrice")).ToString("N0") %> / Q</strong>
                                    </td>
                                    <td>
                                        <a href="Marketplace.aspx?cat=<%# Server.UrlEncode(Eval("Commodity").ToString()) %>" class="btn btn-outline-success btn-sm py-1 px-2 fw-semibold" style="font-size: 0.8rem;">
                                            Trade Crop
                                        </a>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                    </tbody>
                                </table>
                            </FooterTemplate>
                        </asp:Repeater>

                        <asp:Panel ID="pnlNoData" runat="server" Visible="false" CssClass="text-center py-4">
                            <i class="bi bi-info-circle text-muted fs-3 d-block mb-2"></i>
                            <p class="text-muted small mb-0">No commodity rate telemetry reported for this market yard today.</p>
                        </asp:Panel>
                    </div>
                </div>
            </div>

            <!-- RIGHT: APMC Information -->
            <div class="col-lg-4">
                <div class="card p-4 border rounded-3 bg-white shadow-sm h-100 d-flex flex-column">
                    <h5 class="fw-bold mb-3 text-dark">Mandi Administration &amp; Helpdesk</h5>

                    <div class="mb-3">
                        <small class="text-muted fw-bold d-block" style="font-size: 0.75rem; letter-spacing: 0.5px;">APMC YARD NAME</small>
                        <p class="mb-0 text-dark fw-bold" style="font-size: 1rem;">
                            <asp:Literal ID="litMarketName" runat="server">Rajkot APMC</asp:Literal>
                        </p>
                    </div>

                    <div class="mb-3">
                        <small class="text-muted fw-bold d-block" style="font-size: 0.75rem; letter-spacing: 0.5px;">YARD ADDRESS</small>
                        <p class="mb-0 text-dark" style="font-size: 0.92rem;">
                            <asp:Literal ID="litAddress" runat="server">Rajkot - Gondal Highway Bypass Road, Rajkot, Gujarat - 360004</asp:Literal>
                        </p>
                    </div>

                    <div class="mb-3">
                        <small class="text-muted fw-bold d-block" style="font-size: 0.75rem; letter-spacing: 0.5px;">CONTACT DESK / AUCTION HELPLINE</small>
                        <p class="mb-0 fw-bold" style="color: #0284c7; font-size: 0.95rem;">
                            <asp:Literal ID="litContact" runat="server">+91 281 2387120 / +91 98250 12345</asp:Literal>
                        </p>
                    </div>

                    <div class="p-3 bg-light rounded-3 border mt-auto">
                        <small class="fw-bold d-block text-dark mb-1">Gujarat APMC Trade Rule:</small>
                        <span class="text-muted small">All auctions governed under the Gujarat Agricultural Produce Markets Act. Guaranteed payment receipt upon harvest weighment.</span>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
