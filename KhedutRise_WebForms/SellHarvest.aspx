<%@ Page Title="Sell Agricultural Product" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="SellHarvest.aspx.cs" Inherits="KhedutRise.SellHarvest" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Sell Harvest-->
    <div class="kr-page-container">
        <!-- Header -->
        <div class="mb-4">
            <small class="text-muted"><a href="Default.aspx" class="text-decoration-none text-muted">Home</a> / <a href="Marketplace.aspx" class="text-decoration-none text-muted">Marketplace</a> / Sell Harvest</small>
            <h2 class="fw-bold mt-1 text-dark" style="font-size: 2rem;">Sell Your Agricultural Product</h2>
            <p class="text-muted small mb-0">List your farm produce directly to wholesale grain buyers and food processors.</p>
        </div>

        <asp:Label ID="lblMsg" runat="server" CssClass="d-block mb-3 fw-bold"></asp:Label>

        <div class="row g-4 mb-4">
            <!-- LEFT: Product Information Form -->
            <div class="col-lg-6">
                <div class="card card-custom p-4 p-md-5 h-100">
                    <h5 class="fw-bold mb-4" style="color: #0b6028; font-size: 1.25rem;">Product Information Form</h5>
                    <asp:HiddenField ID="hfEditId" runat="server" />

                    <div class="mb-3">
                        <label class="form-label" style="color: #0b6028;">PRODUCT / CROP NAME <span class="required-mark">*</span></label>
                        <asp:TextBox ID="txtProdName" runat="server" CssClass="form-control" 
                            placeholder="e.g. Premium Bt Cotton G-10" required="required"></asp:TextBox>
                    </div>

                    <div class="mb-3">
                        <label class="form-label" style="color: #0b6028;">CROP CATEGORY <span class="required-mark">*</span></label>
                        <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-select" required="required">
                            <asp:ListItem Value="Cotton" Selected="True">Cotton (કપાસ)</asp:ListItem>
                            <asp:ListItem Value="Groundnut">Groundnut (મગફળી)</asp:ListItem>
                            <asp:ListItem Value="Wheat">Wheat (ઘઉં)</asp:ListItem>
                            <asp:ListItem Value="Spices">Spices & Oilseeds (જીરું, રાયડો)</asp:ListItem>
                        </asp:DropDownList>
                    </div>

                    <div class="row g-2 mb-3">
                        <div class="col-6">
                            <label class="form-label" style="color: #0b6028;">FARMER / SELLER <span class="required-mark">*</span></label>
                            <asp:TextBox ID="txtFarmer" runat="server" CssClass="form-control" 
                                placeholder="Your name" required="required"></asp:TextBox>
                        </div>
                        <div class="col-6">
                            <label class="form-label" style="color: #0b6028;">FARM LOCATION <span class="required-mark">*</span></label>
                            <asp:TextBox ID="txtLocation" runat="server" CssClass="form-control" 
                                placeholder="e.g. Rajkot, Gujarat" required="required"></asp:TextBox>
                        </div>
                    </div>

                    <div class="row g-2 mb-3">
                        <div class="col-6">
                            <label class="form-label" style="color: #0b6028;">QUANTITY (QUINTALS) <span class="required-mark">*</span></label>
                            <asp:TextBox ID="txtQty" runat="server" type="number" min="1" CssClass="form-control" 
                                placeholder="e.g. 35" required="required"></asp:TextBox>
                        </div>
                        <div class="col-6">
                            <label class="form-label" style="color: #0b6028;">PRICE (INR / QUINTAL) <span class="required-mark">*</span></label>
                            <div class="input-group">
                                <span class="input-group-text bg-light text-muted">₹</span>
                                <asp:TextBox ID="txtPrice" runat="server" type="number" min="100" CssClass="form-control" 
                                    placeholder="e.g. 7150" required="required"></asp:TextBox>
                            </div>
                        </div>
                    </div>

                    <div class="mb-3">
                        <label class="form-label" style="color: #0b6028;">QUALITY GRADE</label>
                        <asp:DropDownList ID="ddlGrade" runat="server" CssClass="form-select">
                            <asp:ListItem Value="Grade A - Premium Export" Selected="True">Grade A - Premium Export Quality</asp:ListItem>
                            <asp:ListItem Value="Grade B - Good Standard">Grade B - Good Standard Mandi Grade</asp:ListItem>
                            <asp:ListItem Value="Grade C - Regular">Grade C - Regular Commercial Grade</asp:ListItem>
                        </asp:DropDownList>
                    </div>

                    <div class="pt-2">
                        <asp:Button ID="btnSaveCrop" runat="server" Text="Publish Crop Listing" 
                            CssClass="btn btn-kr-primary w-100 py-2 fw-bold" 
                            style="background-color: #0b6028 !important; border-color: #0b6028 !important; border-radius: 6px;" 
                            OnClick="btnSaveCrop_Click" />
                    </div>
                </div>
            </div>

            <!-- RIGHT: Live Listing Preview -->
            <div class="col-lg-6">
                <div class="card card-custom p-4 p-md-5 h-100 d-flex flex-column">
                    <h5 class="fw-bold mb-4 text-dark" style="font-size: 1.25rem;">Live Listing Preview (Buyer's view)</h5>

                    <div class="rounded-3 overflow-hidden border mb-4">
                        <img src="Images/green_rows.jpg" class="img-fluid object-fit-cover" style="height: 220px; width: 100%;" alt="Cotton Field" />
                        <div class="p-3 bg-white">
                            <h5 class="fw-bold mb-1 text-dark" style="font-size: 1.2rem;">Premium Cotton G-10 (Grade A)</h5>
                            <small class="text-muted d-block mb-2" style="font-size: 0.85rem;">Seller: Sanjay Tirth | Location: Rajkot, Gujarat</small>
                            <h3 class="fw-bold mb-0" style="color: #0b6028; font-size: 1.6rem;">₹7,150 / Quintal</h3>
                        </div>
                    </div>

                    <div class="p-3 rounded-2 border bg-light mt-auto">
                        <small class="text-muted d-block fw-semibold mb-1">• Zero Commission for registered farmers</small>
                        <small class="text-muted d-block fw-semibold mb-1">• Quality tested upon buyer inspection</small>
                        <small class="text-muted d-block fw-semibold">• Payment secured via KhedutRise Escrow System</small>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
