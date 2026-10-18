<%@ Page Title="Farmer Dashboard" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="FarmerDashboard.aspx.cs" Inherits="KhedutRise.FarmerDashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Farmer Dashboard-->
    <div class="kr-page-container py-4">
        <!-- GREETING & SUMMARY -->
        <div class="mb-4">
            <h2 class="fw-bold mb-1 text-dark" style="font-size: 2.1rem;">Welcome, <asp:Literal ID="litFarmerGreeting" runat="server">Farmer</asp:Literal>!</h2>
            <p class="text-secondary small mb-0">Here is your live farm operations and agricultural marketplace activity summary.</p>
        </div>

        <!-- 6 KEY METRIC WIDGETs-->
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
                    <h3 class="fw-bold mt-2 mb-0" style="color: #ea580c; font-size: 1.65rem;">₹7,150 / Q</h3>
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
                        <a href="SellHarvest.aspx" class="btn py-2 text-white fw-bold shadow-sm" style="background-color: #0b6028; border-radius: 6px;">Sell Product</a>
                        <a href="ApmcRates.aspx" class="btn py-2 fw-bold" style="border: 2px solid #0b6028; color: #0b6028; background: #ffffff; border-radius: 6px;">Check APMC Rates</a>
                        <a href="TractorRentals.aspx" class="btn py-2 fw-bold" style="border: 2px solid #0b6028; color: #0b6028; background: #ffffff; border-radius: 6px;">Book Tractor Rental</a>
                    </div>
                </div>
            </div>

            <!-- COLUMN 2: Live APMC Rates -->
            <div class="col-lg-4">
                <div class="card p-4 border rounded-3 shadow-none h-100" style="border-color: #e2e8f0 !important; background: #ffffff;">
                    <h5 class="fw-bold mb-3 text-dark">Live APMC Rates</h5>
                    <img src="Images/apmc_yard.jpg" class="rounded-3 object-fit-cover w-100 mb-3 shadow-sm" style="height: 160px;" alt="APMC Yard" />
                    
                    <div class="d-flex flex-column gap-2" style="font-size: 0.95rem;">
                        <div class="d-flex justify-content-between align-items-center py-2 border-bottom">
                            <span class="fw-bold text-dark">Cotton (Shankar 6) - Rajkot</span>
                            <span class="fw-bold" style="color: #ea580c;">₹7,150 / Q</span>
                        </div>
                        <div class="d-flex justify-content-between align-items-center py-2 border-bottom">
                            <span class="fw-bold text-dark">Groundnut (Bold) - Gondal</span>
                            <span class="fw-bold" style="color: #16a34a;">₹5,800 / Q</span>
                        </div>
                        <div class="d-flex justify-content-between align-items-center py-2 border-bottom">
                            <span class="fw-bold text-dark">Wheat (Lokwan) - Junagadh</span>
                            <span class="fw-bold" style="color: #0284c7;">₹2,450 / Q</span>
                        </div>
                        <div class="d-flex justify-content-between align-items-center py-2">
                            <span class="fw-bold text-dark">Cumin (Medium) - Atkot</span>
                            <span class="fw-bold" style="color: #1e3a8a;">₹28,500 / Q</span>
                        </div>
                    </div>
                </div>
            </div>

            <!-- COLUMN 3: Farming Expenses & Revenue Trends -->
            <div class="col-lg-5">
                <div class="card p-4 border rounded-3 shadow-none h-100 d-flex flex-column" style="border-color: #e2e8f0 !important; background: #ffffff;">
                    <h5 class="fw-bold mb-3 text-dark">Farming Expenses & Revenue Trends</h5>
                    <img src="Images/groundnut_harvest.jpg" class="rounded-3 object-fit-cover w-100 mb-3 shadow-sm" style="height: 160px;" alt="Finance Trends" />

                    <div class="mb-4">
                        <small class="text-secondary fw-bold d-block mb-2" style="font-size: 0.75rem; letter-spacing: 0.5px;">FINANCIAL SUMMARY</small>
                        <ul class="list-unstyled mb-0" style="line-height: 2; font-size: 0.95rem;">
                            <li>• Total Projected Crop Revenue: <strong style="color: #16a34a;">₹6,12,000</strong></li>
                            <li>• Total Seeds & Fertilizers Cost: <strong style="color: #ea580c;">₹1,12,500</strong></li>
                            <li>• Net Estimated Profit: <strong style="color: #16a34a;">₹4,99,500</strong></li>
                        </ul>
                    </div>

                    <div class="mt-auto p-3 text-center rounded-3 fw-bold" style="background-color: #ecfdf5; border: 1.5px solid #86efac; color: #166534; font-size: 1rem; letter-spacing: 0.5px;">
                        ESTIMATED NET MARGIN: 81.6%
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
