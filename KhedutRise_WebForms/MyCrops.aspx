<%@ Page Title="My Crops" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="MyCrops.aspx.cs" Inherits="KhedutRise.MyCrops" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- 100% Dynamic Crop Monitoring Portal for Authenticated Farmers -->
    <div class="kr-page-container py-4">
        <!-- Page Title -->
        <div class="d-flex flex-wrap justify-content-between align-items-center mb-4">
            <div>
                <small class="text-muted"><a href="Default.aspx" class="text-decoration-none text-muted">Home</a> / My Crops</small>
                <h2 class="fw-bold mt-1 text-dark" style="font-size: 2rem;">My Cultivated Crops & Farm Plots</h2>
                <p class="text-muted small mb-0">Live farm monitoring, sowing cycles, and expected harvests for your fields.</p>
            </div>
            <div class="mt-3 mt-md-0">
                <a href="FarmPlots.aspx" class="btn btn-sm btn-success px-3 py-2 fw-bold" style="background-color: #0b6028; border-color: #0b6028;">
                    <i class="bi bi-plus-circle me-1"></i> Add / Manage Farm Plots
                </a>
            </div>
        </div>

        <!-- 4 Top KPI Cards from Database -->
        <div class="row g-3 mb-4">
            <div class="col-6 col-md-3">
                <div class="card card-custom p-3 text-center h-100 border-start border-4 border-success shadow-sm">
                    <small class="text-muted fw-bold" style="font-size: 0.75rem; letter-spacing: 0.5px;">ACTIVE CROPS SOWN</small>
                    <h4 class="fw-bold mt-2 mb-0" style="color: #16a34a; font-size: 1.35rem;">
                        <asp:Label ID="lblTotalCrops" runat="server" Text="0"></asp:Label> Varieties
                    </h4>
                </div>
            </div>
            <div class="col-6 col-md-3">
                <div class="card card-custom p-3 text-center h-100 border-start border-4 border-primary shadow-sm">
                    <small class="text-muted fw-bold" style="font-size: 0.75rem; letter-spacing: 0.5px;">TOTAL CULTIVATED AREA</small>
                    <h4 class="fw-bold text-dark mt-2 mb-0" style="font-size: 1.35rem;">
                        <asp:Label ID="lblTotalArea" runat="server" Text="0"></asp:Label> Acres
                    </h4>
                </div>
            </div>
            <div class="col-6 col-md-3">
                <div class="card card-custom p-3 text-center h-100 border-start border-4 border-info shadow-sm">
                    <small class="text-muted fw-bold" style="font-size: 0.75rem; letter-spacing: 0.5px;">RECORDED PLOTS</small>
                    <h4 class="fw-bold mt-2 mb-0" style="color: #0284c7; font-size: 1.35rem;">
                        <asp:Label ID="lblActivePlots" runat="server" Text="0"></asp:Label> Active Plots
                    </h4>
                </div>
            </div>
            <div class="col-6 col-md-3">
                <div class="card card-custom p-3 text-center h-100 border-start border-4 border-warning shadow-sm" style="background-color: #fefce8;">
                    <small class="text-muted fw-bold" style="font-size: 0.75rem; letter-spacing: 0.5px;">NEXT HARVEST CYCLE</small>
                    <h4 class="fw-bold text-dark mt-2 mb-0" style="font-size: 1.25rem;">
                        <asp:Label ID="lblNextHarvest" runat="server" Text="Active Growth"></asp:Label>
                    </h4>
                </div>
            </div>
        </div>

        <div class="row g-4">
            <!-- LEFT: Dynamic Farmer Crop Progress Cards -->
            <div class="col-lg-6">
                <!-- Empty State -->
                <asp:Panel ID="pnlNoCrops" runat="server" Visible="false" CssClass="card card-custom p-5 text-center bg-white border">
                    <i class="bi bi-flower1 text-muted display-4 mb-3"></i>
                    <h5 class="fw-bold text-dark">No Farm Plots Registered Yet</h5>
                    <p class="text-muted small mb-3">You haven't added any active farm plot or sowing records to your account.</p>
                    <div>
                        <a href="FarmPlots.aspx" class="btn btn-kr-primary btn-sm px-4 fw-bold">+ Register A Farm Plot</a>
                    </div>
                </asp:Panel>

                <!-- Repeater for Authenticated Farmer's Own Plots -->
                <asp:Repeater ID="rptFarmerCrops" runat="server">
                    <ItemTemplate>
                        <div class="card card-custom p-3 mb-3 d-flex flex-row gap-3 align-items-center border shadow-sm">
                            <img src='<%# Eval("Crop").ToString().ToLower().Contains("cotton") ? "Images/green_rows.jpg" : "Images/groundnut_harvest.jpg" %>' 
                                 class="rounded-3 object-fit-cover flex-shrink-0" width="130" height="95" alt="Crop" />
                            <div class="flex-grow-1">
                                <div class="d-flex justify-content-between align-items-center mb-1">
                                    <h5 class="fw-bold mb-0 text-dark" style="font-size: 1.15rem;"><%# Eval("Crop") %></h5>
                                    <span class="badge bg-success-subtle text-success border border-success-subtle small"><%# Eval("Status") %></span>
                                </div>
                                <small class="text-muted d-block" style="font-size: 0.85rem;">
                                    <strong><%# Eval("PlotName") %></strong> | <%# Eval("AreaAcres") %> Acres | Soil: <%# Eval("SoilType") %>
                                </small>
                                <div class="d-flex justify-content-between align-items-center mt-2 mb-1">
                                    <small class="fw-bold text-success" style="font-size: 0.82rem;">
                                        Stage: <%# Eval("Stage") %>
                                    </small>
                                </div>
                                <div class="progress" style="height: 6px; background-color: #e2e8f0;">
                                    <div class="progress-bar" role="progressbar" 
                                         style='<%# "width: " + (Eval("Stage").ToString() == "Flowering" ? "75" : (Eval("Stage").ToString() == "Boll Dev" ? "60" : "45")) + "%; background-color: #16a34a;" %>'></div>
                                </div>
                                <small class="text-secondary d-block mt-2" style="font-size: 0.82rem;">
                                    Sown: <%# Eval("SowingDate") != DBNull.Value ? Convert.ToDateTime(Eval("SowingDate")).ToString("dd MMM yyyy") : "Recent" %> 
                                    <span class="mx-1">•</span>
                                    Est. Harvest: <%# Eval("ExpectedHarvestDate") != DBNull.Value ? Convert.ToDateTime(Eval("ExpectedHarvestDate")).ToString("dd MMM yyyy") : "In 45 Days" %>
                                </small>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>

            <!-- RIGHT: Dynamic Farmland Sowing Registry -->
            <div class="col-lg-6">
                <div class="card card-custom p-4 h-100 d-flex flex-column border shadow-sm">
                    <h5 class="fw-bold mb-3 text-dark">Farmland Growth Stages</h5>

                    <!-- 5-Step Growth Timeline -->
                    <div class="d-flex justify-content-between align-items-center mb-4 px-2 position-relative" style="padding-top: 5px;">
                        <div class="text-center position-relative" style="z-index: 2;">
                            <span class="rounded-circle d-inline-flex align-items-center justify-content-center text-white" style="width: 24px; height: 24px; background-color: #16a34a; font-size: 0.75rem;"><i class="bi bi-check"></i></span>
                            <small class="d-block text-muted" style="font-size: 0.7rem; margin-top: 4px;">Sowing</small>
                        </div>
                        <div class="text-center position-relative" style="z-index: 2;">
                            <span class="rounded-circle d-inline-flex align-items-center justify-content-center text-white" style="width: 24px; height: 24px; background-color: #16a34a; font-size: 0.75rem;"><i class="bi bi-check"></i></span>
                            <small class="d-block text-muted" style="font-size: 0.7rem; margin-top: 4px;">Germination</small>
                        </div>
                        <div class="text-center position-relative" style="z-index: 2;">
                            <span class="rounded-circle d-inline-flex align-items-center justify-content-center text-white" style="width: 24px; height: 24px; background-color: #16a34a; font-size: 0.75rem;"><i class="bi bi-check"></i></span>
                            <small class="d-block text-muted" style="font-size: 0.7rem; margin-top: 4px;">Vegetative</small>
                        </div>
                        <div class="text-center position-relative" style="z-index: 2;">
                            <span class="rounded-circle d-inline-flex align-items-center justify-content-center text-white border border-2 border-white shadow-sm" style="width: 26px; height: 26px; background-color: #16a34a; font-size: 0.75rem;"><i class="bi bi-dot" style="font-size: 1.5rem;"></i></span>
                            <small class="d-block fw-bold text-success" style="font-size: 0.7rem; margin-top: 4px;">Flowering</small>
                        </div>
                        <div class="text-center position-relative" style="z-index: 2;">
                            <span class="rounded-circle d-inline-flex align-items-center justify-content-center text-muted border" style="width: 24px; height: 24px; background-color: #f1f5f9; font-size: 0.75rem;">5</span>
                            <small class="d-block text-muted" style="font-size: 0.7rem; margin-top: 4px;">Harvest</small>
                        </div>
                    </div>

                    <!-- Farmer Actions -->
                    <div class="p-3 bg-light rounded-3 border mb-3">
                        <h6 class="fw-bold text-dark mb-1">Ready to Sell Your Harvest?</h6>
                        <p class="small text-muted mb-2">Once your crop reaches maturity, list it directly on the marketplace to connect with wholesale buyers.</p>
                        <a href="SellHarvest.aspx" class="btn btn-sm btn-outline-success fw-bold px-3">Publish To Marketplace</a>
                    </div>

                    <!-- Manage Plots Action Button -->
                    <a href="FarmPlots.aspx" class="btn btn-kr-primary w-100 mt-auto py-2 fw-bold" style="background-color: #0b6028 !important; border-color: #0b6028 !important;">+ Add Or Update Farmland Plots</a>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
