<%@ Page Title="Farmer Dashboard" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="FarmerDashboard.aspx.cs" Inherits="KhedutRise.FarmerDashboard" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Farmer Dashboard: 100% Dynamic -->
    <div class="kr-page-container py-4">
        <!-- GREETING & SUMMARY -->
        <div class="mb-4">
            <h2 class="fw-bold mb-1 text-dark" style="font-size: 2.1rem;">Welcome, <asp:Literal ID="litFarmerGreeting" runat="server">Farmer</asp:Literal>!</h2>
            <p class="text-secondary small mb-0">Here is your live farm operations and agricultural marketplace activity summary.</p>
        </div>

        <!-- 6 KEY METRIC WIDGETS -->
        <div class="row g-3 mb-4">
            <div class="col-6 col-md-4 col-lg-2">
                <div class="card p-3 text-start border rounded-3 shadow-none h-100" style="border-color: #e2e8f0 !important; background: #ffffff;">
                    <small class="text-secondary fw-bold" style="font-size: 0.75rem; letter-spacing: 0.5px;">FARM AREA</small>
                    <h3 class="fw-bold text-dark mt-2 mb-0" style="font-size: 1.65rem;">
                        <asp:Literal ID="litTotalAcres" runat="server">0 Acres</asp:Literal>
                    </h3>
                </div>
            </div>
            <div class="col-6 col-md-4 col-lg-2">
                <div class="card p-3 text-start border rounded-3 shadow-none h-100" style="border-color: #e2e8f0 !important; background: #ffffff;">
                    <small class="text-secondary fw-bold" style="font-size: 0.75rem; letter-spacing: 0.5px;">CURRENT CROP</small>
                    <h3 class="fw-bold text-dark mt-2 mb-0" style="font-size: 1.65rem;">
                        <asp:Literal ID="litCurrentCrop" runat="server">Mixed Crops</asp:Literal>
                    </h3>
                </div>
            </div>
            <div class="col-6 col-md-4 col-lg-2">
                <div class="card p-3 text-start border rounded-3 shadow-none h-100" style="border-color: #e2e8f0 !important; background: #ffffff;">
                    <small class="text-secondary fw-bold" style="font-size: 0.75rem; letter-spacing: 0.5px;">COTTON APMC</small>
                    <h3 class="fw-bold mt-2 mb-0" style="color: #ea580c; font-size: 1.65rem;">
                        <asp:Literal ID="litCottonApmcRate" runat="server">₹7,025 / Q</asp:Literal>
                    </h3>
                </div>
            </div>
            <div class="col-6 col-md-4 col-lg-2">
                <div class="card p-3 text-start border rounded-3 shadow-none h-100" style="border-color: #e2e8f0 !important; background: #ffffff;">
                    <small class="text-secondary fw-bold" style="font-size: 0.75rem; letter-spacing: 0.5px;">ACTIVE ORDERS</small>
                    <h3 class="fw-bold mt-2 mb-0" style="color: #0284c7; font-size: 1.65rem;">
                        <asp:Literal ID="litActiveOrders" runat="server">0 Active</asp:Literal>
                    </h3>
                </div>
            </div>
            <div class="col-6 col-md-4 col-lg-2">
                <div class="card p-3 text-start border rounded-3 shadow-none h-100" style="border-color: #e2e8f0 !important; background: #ffffff;">
                    <small class="text-secondary fw-bold" style="font-size: 0.75rem; letter-spacing: 0.5px;">BOOKINGS</small>
                    <h3 class="fw-bold mt-2 mb-0" style="color: #eab308; font-size: 1.65rem;">
                        <asp:Literal ID="litUpcomingBookings" runat="server">0 Bookings</asp:Literal>
                    </h3>
                </div>
            </div>
            <div class="col-6 col-md-4 col-lg-2">
                <div class="card p-3 text-start border rounded-3 shadow-none h-100" style="border-color: #e2e8f0 !important; background: #ffffff;">
                    <small class="text-secondary fw-bold" style="font-size: 0.75rem; letter-spacing: 0.5px;">TOTAL SALES</small>
                    <h3 class="fw-bold mt-2 mb-0" style="color: #16a34a; font-size: 1.65rem;">
                        <asp:Literal ID="litTotalRevenue" runat="server">₹0</asp:Literal>
                    </h3>
                </div>
            </div>
        </div>

        <!-- 3 MAIN COLUMNS: Actions, Live APMC, Trends -->
        <div class="row g-4">
            <!-- COLUMN 1: Quick Farm Actions -->
            <div class="col-lg-3">
                <div class="card p-4 border rounded-3 shadow-none h-100" style="border-color: #e2e8f0 !important; background: #ffffff;">
                    <h5 class="fw-bold mb-4" style="color: #0b6028;">Quick Farm Actions</h5>
                    <div class="d-flex flex-column gap-3">
                        <a href="SellHarvest.aspx" class="btn py-2 text-white fw-bold shadow-sm" style="background-color: #0b6028; border-radius: 6px;">
                            <i class="bi bi-plus-circle me-1"></i> Sell Product
                        </a>
                        <a href="ApmcRates.aspx" class="btn py-2 fw-bold" style="border: 2px solid #0b6028; color: #0b6028; background: #ffffff; border-radius: 6px;">
                            <i class="bi bi-graph-up-arrow me-1"></i> Check APMC Rates
                        </a>
                        <a href="TractorRentals.aspx" class="btn py-2 fw-bold" style="border: 2px solid #0b6028; color: #0b6028; background: #ffffff; border-radius: 6px;">
                            <i class="bi bi-truck me-1"></i> Book Tractor Rental
                        </a>
                        <a href="Community.aspx" class="btn py-2 fw-bold text-secondary" style="border: 1px solid #cbd5e1; background: #f8fafc; border-radius: 6px;">
                            <i class="bi bi-chat-dots me-1"></i> Agri Community &amp; Weather
                        </a>
                    </div>
                </div>
            </div>

            <!-- COLUMN 2: Live APMC Rates -->
            <div class="col-lg-4">
                <div class="card p-4 border rounded-3 shadow-none h-100" style="border-color: #e2e8f0 !important; background: #ffffff;">
                    <div class="d-flex justify-content-between align-items-center mb-3">
                        <h5 class="fw-bold mb-0 text-dark">Live Mandi Rates</h5>
                        <a href="ApmcRates.aspx" class="text-success small fw-semibold text-decoration-none">View All Yards</a>
                    </div>
                    <img src="Images/apmc_yard.jpg" class="rounded-3 object-fit-cover w-100 mb-3 shadow-sm" style="height: 140px;" alt="APMC Yard" />
                    
                    <div class="d-flex flex-column gap-2" style="font-size: 0.92rem;">
                        <asp:Repeater ID="rptDashboardApmc" runat="server">
                            <ItemTemplate>
                                <div class="d-flex justify-content-between align-items-center py-2 border-bottom">
                                    <div>
                                        <span class="fw-bold text-dark d-block"><%# Eval("Commodity") %></span>
                                        <small class="text-muted"><%# Eval("MarketYard") %></small>
                                    </div>
                                    <div class="text-end">
                                        <span class="fw-bold text-success d-block">₹<%# Convert.ToDecimal(Eval("AvgPrice")).ToString("N0") %> / Q</span>
                                        <small class="text-muted">₹<%# Convert.ToDecimal(Eval("MinPrice")).ToString("N0") %> - ₹<%# Convert.ToDecimal(Eval("MaxPrice")).ToString("N0") %></small>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </div>
            </div>

            <!-- COLUMN 3: Farming Expenses & Revenue Trends -->
            <div class="col-lg-5">
                <div class="card p-4 border rounded-3 shadow-none h-100 d-flex flex-column" style="border-color: #e2e8f0 !important; background: #ffffff;">
                    <h5 class="fw-bold mb-3 text-dark">Farming Expenses &amp; Revenue Trends</h5>
                    <img src="Images/groundnut_harvest.jpg" class="rounded-3 object-fit-cover w-100 mb-3 shadow-sm" style="height: 140px;" alt="Finance Trends" />

                    <div class="mb-4">
                        <small class="text-secondary fw-bold d-block mb-2" style="font-size: 0.75rem; letter-spacing: 0.5px;">FINANCIAL SUMMARY</small>
                        <ul class="list-unstyled mb-0" style="line-height: 2; font-size: 0.95rem;">
                            <li>• Total Projected Crop Revenue: <strong class="text-success"><asp:Literal ID="litProjectedRevenue" runat="server">₹6,12,000</asp:Literal></strong></li>
                            <li>• Total Seeds &amp; Fertilizers Cost: <strong class="text-danger"><asp:Literal ID="litInputCost" runat="server">₹1,12,500</asp:Literal></strong></li>
                            <li>• Net Estimated Profit: <strong class="text-success"><asp:Literal ID="litNetProfit" runat="server">₹4,99,500</asp:Literal></strong></li>
                        </ul>
                    </div>

                    <div class="mt-auto p-3 text-center rounded-3 fw-bold" style="background-color: #ecfdf5; border: 1.5px solid #86efac; color: #166534; font-size: 1rem; letter-spacing: 0.5px;">
                        ESTIMATED NET MARGIN: <asp:Literal ID="litMarginPercent" runat="server">81.6%</asp:Literal>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
