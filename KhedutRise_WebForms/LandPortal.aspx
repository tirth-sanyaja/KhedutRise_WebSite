<%@ Page Title="Agricultural Land Marketplace" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="LandPortal.aspx.cs" Inherits="KhedutRise.LandPortal" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Agricultural Land Marketplace -->
    <div class="kr-page-container">
        <!--  Header -->
        <div class="d-flex flex-wrap justify-content-between align-items-center mb-4">
            <div>
                <small class="text-muted"><a href="Default.aspx" class="text-decoration-none text-muted">Home</a> / Land Portal / Buy & Sell</small>
                <h2 class="fw-bold mt-1 text-dark" style="font-size: 2rem;">Agricultural Land Marketplace</h2>
                <p class="text-muted small mb-0">Discover fertile agricultural land or list your own property.</p>
            </div>
            <div class="mt-2 mt-md-0">
                <button type="button" class="btn btn-kr-primary px-4 py-2 fw-bold text-white shadow-sm" data-bs-toggle="modal" data-bs-target="#modalAddLand" style="background-color: #0b6028; border-radius: 6px;">
                    <i class="bi bi-plus-lg me-1"></i>List Your Land
                </button>
            </div>
        </div>

        <asp:Label ID="lblLandMsg" runat="server" CssClass="d-block mb-3 fw-bold"></asp:Label>

        <div class="row g-4">
            <!-- LEFT: Available Farmland Listings -->
            <div class="col-lg-6">
                <h5 class="fw-bold mb-3" style="color: #0b6028; font-size: 1.25rem;">Available Farmland Listings</h5>

                <asp:Repeater ID="rptLandListings" runat="server">
                    <ItemTemplate>
                        <div class="card card-custom p-3 mb-3 d-flex flex-row gap-3 align-items-center">
                            <img src="Images/aerial_plots.jpg" class="rounded-3 object-fit-cover flex-shrink-0" width="140" height="95" alt="Land Plot" />
                            <div class="flex-grow-1">
                                <h6 class="fw-bold mb-1 text-dark" style="font-size: 1.05rem;"><%# Eval("Title") %></h6>
                                <small class="text-muted d-block" style="font-size: 0.8rem;">Location: <%# Eval("Location") %> | Water: <%# Eval("WaterSource") %></small>
                                <small class="text-muted d-block mb-2" style="font-size: 0.8rem;">Area: <%# Eval("AreaAcres") %> Acres | Soil: <%# Eval("SoilType") %></small>
                                <h4 class="fw-bold mb-0" style="color: #0b6028; font-size: 1.3rem;"><%# Eval("Price") %></h4>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>

            <!-- RIGHT: Interactive Aerial Map with Orange Plot Pins -->
            <div class="col-lg-6">
                <div class="card card-custom overflow-hidden h-100 position-relative border" style="min-height: 400px;">
                    <img src="Images/aerial_plots.jpg" class="img-fluid object-fit-cover w-100 h-100" style="min-height: 400px;" alt="Farmland Aerial Map" />

                    <!-- Plot 1 Marker Pin -->
                    <div class="position-absolute translate-middle" style="top: 36%; left: 30%;">
                        <div class="rounded-circle shadow-lg d-flex align-items-center justify-content-center" style="width: 22px; height: 22px; background-color: #ea580c; border: 2.5px solid #ffffff;"></div>
                    </div>

                    <!-- Plot 2 Marker Pin -->
                    <div class="position-absolute translate-middle" style="top: 54%; left: 65%;">
                        <div class="rounded-circle shadow-lg d-flex align-items-center justify-content-center" style="width: 22px; height: 22px; background-color: #ea580c; border: 2.5px solid #ffffff;"></div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Bootstrap Modal for Add Land Listing -->
    <div class="modal fade" id="modalAddLand" tabindex="-1" aria-labelledby="modalAddLandLabel" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content rounded-3 border-0 shadow">
                <div class="modal-header" style="background-color: #0b6028; color: #ffffff;">
                    <h5 class="modal-title fw-bold" id="modalAddLandLabel">Post Agricultural Land Listing</h5>
                    <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body p-4">
                    <div class="mb-3">
                        <label class="form-label" style="color: #0b6028;">PROPERTY TITLE <span class="required-mark">*</span></label>
                        <asp:TextBox ID="txtTitle" runat="server" CssClass="form-control" 
                            placeholder="e.g. 8 Acres Prime Fertile Black Soil Land" required="required"></asp:TextBox>
                    </div>
                    <div class="row g-2 mb-3">
                        <div class="col-6">
                            <label class="form-label" style="color: #0b6028;">AREA (ACRES) <span class="required-mark">*</span></label>
                            <asp:TextBox ID="txtArea" runat="server" type="number" step="0.1" min="0.1" CssClass="form-control" 
                                placeholder="e.g. 8.0" required="required"></asp:TextBox>
                        </div>
                        <div class="col-6">
                            <label class="form-label" style="color: #0b6028;">EXPECTED PRICE <span class="required-mark">*</span></label>
                            <asp:TextBox ID="txtPrice" runat="server" CssClass="form-control" 
                                placeholder="e.g. ₹95 Lakhs" required="required"></asp:TextBox>
                        </div>
                    </div>
                    <div class="mb-3">
                        <label class="form-label" style="color: #0b6028;">LOCATION / TALUKA <span class="required-mark">*</span></label>
                        <asp:TextBox ID="txtLocation" runat="server" CssClass="form-control" 
                            placeholder="e.g. Gondal Bypass, Rajkot" required="required"></asp:TextBox>
                    </div>
                    <div class="row g-2 mb-3">
                        <div class="col-6">
                            <label class="form-label" style="color: #0b6028;">SOIL TYPE <span class="required-mark">*</span></label>
                            <asp:DropDownList ID="ddlSoil" runat="server" CssClass="form-select" required="required">
                                <asp:ListItem Text="Deep Black Cotton Soil" Value="Black soil" Selected="True" />
                                <asp:ListItem Text="Fertile Red Soil" Value="Red soil" />
                                <asp:ListItem Text="Alluvial Loam Soil" Value="Alluvial soil" />
                            </asp:DropDownList>
                        </div>
                        <div class="col-6">
                            <label class="form-label" style="color: #0b6028;">WATER SOURCE <span class="required-mark">*</span></label>
                            <asp:DropDownList ID="ddlWater" runat="server" CssClass="form-select" required="required">
                                <asp:ListItem Text="Tubewell (Solar Connected)" Value="Tubewell" Selected="True" />
                                <asp:ListItem Text="Narmada Canal Link" Value="Canal link" />
                                <asp:ListItem Text="Borewell" Value="Borewell" />
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="modal-footer border-0 p-3 pt-0">
                    <button type="button" class="btn btn-light px-4 fw-semibold text-secondary" data-bs-dismiss="modal">Cancel</button>
                    <asp:Button ID="btnAddLand" runat="server" Text="Publish Land Listing" CssClass="btn text-white fw-bold px-4" OnClick="btnAddLand_Click" style="background-color: #0b6028; border-radius: 6px; border: none;" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
