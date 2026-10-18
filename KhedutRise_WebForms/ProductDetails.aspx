<%@ Page Title="Product Details" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="ProductDetails.aspx.cs" Inherits="KhedutRise.ProductDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- 100% Dynamic Product Details & Direct Purchase Portal -->
    <div class="kr-page-container py-4">
        <!-- Breadcrumb Header -->
        <div class="mb-4">
            <small class="text-muted"><a href="Default.aspx" class="text-decoration-none text-muted">Home</a> / <a href="Marketplace.aspx" class="text-decoration-none text-muted">Marketplace</a> / Product Details</small>
            <h2 class="fw-bold mt-1 text-dark" style="font-size: 2rem;">Product Details</h2>
        </div>

        <!-- Feedback Alert Message -->
        <asp:Label ID="lblOrderMsg" runat="server" CssClass="d-block mb-3 fw-bold"></asp:Label>

        <!-- Product Not Found State -->
        <asp:Panel ID="pnlNotFound" runat="server" Visible="false" CssClass="card p-5 text-center bg-white border">
            <i class="bi bi-exclamation-triangle text-warning display-4 mb-3"></i>
            <h4 class="fw-bold text-dark">Product Listing Not Found</h4>
            <p class="text-muted">The requested crop listing either does not exist or has already been sold out.</p>
            <div>
                <a href="Marketplace.aspx" class="btn btn-kr-primary px-4 fw-bold">Browse All Available Crops</a>
            </div>
        </asp:Panel>

        <!-- Dynamic Product Content -->
        <asp:Panel ID="pnlProductContent" runat="server">
            <div class="row g-5">
                <!-- LEFT: Image Gallery -->
                <div class="col-lg-6">
                    <div class="card card-custom overflow-hidden mb-3 border shadow-sm">
                        <asp:Image ID="imgMainProduct" runat="server" CssClass="img-fluid object-fit-cover" style="height: 380px; width: 100%;" ImageUrl="Images/groundnut_harvest.jpg" AlternateText="Crop Photo" />
                    </div>
                    <div class="row g-2">
                        <div class="col-4">
                            <img src="Images/groundnut_harvest.jpg" class="img-fluid rounded border object-fit-cover" style="height: 85px; width: 100%; cursor: pointer;" alt="thumb1" onclick="document.getElementById('<%= imgMainProduct.ClientID %>').src=this.src;" />
                        </div>
                        <div class="col-4">
                            <img src="Images/hero_farmer.jpg" class="img-fluid rounded border object-fit-cover" style="height: 85px; width: 100%; cursor: pointer;" alt="thumb2" onclick="document.getElementById('<%= imgMainProduct.ClientID %>').src=this.src;" />
                        </div>
                        <div class="col-4">
                            <img src="Images/green_rows.jpg" class="img-fluid rounded border object-fit-cover" style="height: 85px; width: 100%; cursor: pointer;" alt="thumb3" onclick="document.getElementById('<%= imgMainProduct.ClientID %>').src=this.src;" />
                        </div>
                    </div>
                </div>

                <!-- RIGHT: Product Specifications & Purchase Actions -->
                <div class="col-lg-6">
                    <div class="d-flex flex-column h-100">
                        <div class="d-flex align-items-center gap-2 mb-1">
                            <span class="badge bg-success text-white px-2 py-1 small rounded-1 fw-bold">
                                <asp:Label ID="lblCategory" runat="server" Text="Groundnut"></asp:Label>
                            </span>
                            <span class="badge bg-light text-dark border px-2 py-1 small">
                                <asp:Label ID="lblGrade" runat="server" Text="Grade A"></asp:Label>
                            </span>
                        </div>
                        <h3 class="fw-bold text-dark mb-1" style="font-size: 1.85rem;">
                            <asp:Label ID="lblCropTitle" runat="server" Text="Premium Groundnut"></asp:Label>
                        </h3>
                        <p class="text-muted mb-3" style="font-size: 0.95rem;">
                            Direct harvest listing from verified Gujarat agricultural farms. Zero middleman commission.
                        </p>

                        <div class="mb-4 p-3 bg-light rounded-3 border">
                            <div class="d-flex justify-content-between align-items-center">
                                <div>
                                    <small class="text-muted fw-bold d-block" style="font-size: 0.75rem;">PRICE PER QUINTAL</small>
                                    <h2 class="fw-bold mb-0" style="color: #0b6028; font-size: 2.2rem;">
                                        ₹<asp:Label ID="lblPrice" runat="server" Text="0"></asp:Label>
                                        <small class="fs-6 text-muted fw-normal">/ Quintal</small>
                                    </h2>
                                </div>
                                <div class="text-end">
                                    <small class="text-muted fw-bold d-block" style="font-size: 0.75rem;">AVAILABLE STOCK</small>
                                    <span class="badge bg-white text-dark border fs-6 px-3 py-2 fw-bold">
                                        <asp:Label ID="lblAvailableStock" runat="server" Text="0"></asp:Label> Quintals
                                    </span>
                                </div>
                            </div>
                        </div>

                        <!-- Farmer Detail Box -->
                        <div class="card bg-white border p-3 mb-4 rounded-3 shadow-sm">
                            <small class="text-muted fw-bold mb-2 d-block" style="font-size: 0.72rem; letter-spacing: 0.5px;">VERIFIED FARMER SELLER</small>
                            <div class="d-flex align-items-center gap-3">
                                <div class="text-white rounded-circle d-flex align-items-center justify-content-center fw-bold" style="width: 48px; height: 48px; background-color: #0b6028; font-size: 1.1rem;">
                                    <asp:Label ID="lblInitials" runat="server" Text="FP"></asp:Label>
                                </div>
                                <div>
                                    <strong class="d-block text-dark" style="font-size: 1rem;">
                                        <asp:Label ID="lblFarmerName" runat="server" Text="Farmer Name"></asp:Label>
                                    </strong>
                                    <small class="text-muted" style="font-size: 0.85rem;">
                                        <i class="bi bi-geo-alt-fill text-danger me-1"></i>
                                        <asp:Label ID="lblLocation" runat="server" Text="Gujarat Farm"></asp:Label>
                                        <span class="mx-1">•</span>
                                        <i class="bi bi-telephone-fill text-success me-1"></i>
                                        <asp:Label ID="lblFarmerContact" runat="server" Text="+91 98250 XXXXX"></asp:Label>
                                    </small>
                                </div>
                            </div>
                        </div>

                        <!-- Purchase Form Box -->
                        <div class="card p-3 p-md-4 border rounded-3 bg-white shadow-sm mb-4">
                            <h6 class="fw-bold text-dark mb-3">Order & Purchase Request</h6>

                            <!-- Quantity Field -->
                            <div class="row g-3 mb-3">
                                <div class="col-sm-5">
                                    <label class="form-label small fw-bold text-muted">PURCHASE QUANTITY (Q)</label>
                                    <asp:TextBox ID="txtOrderQty" runat="server" CssClass="form-control" Text="1" TextMode="Number" step="0.5" min="0.5"></asp:TextBox>
                                </div>
                                <div class="col-sm-7">
                                    <label class="form-label small fw-bold text-muted">DELIVERY DESTINATION / ADDRESS</label>
                                    <asp:TextBox ID="txtDeliveryAddress" runat="server" CssClass="form-control" placeholder="Warehouse / Yard address in Gujarat"></asp:TextBox>
                                </div>
                            </div>

                            <!-- Buy Button -->
                            <div>
                                <asp:Button ID="btnConfirmOrder" runat="server" Text="Confirm Purchase & Place Order" 
                                    CssClass="btn w-100 py-3 fw-bold text-white shadow-sm" 
                                    OnClick="btnConfirmOrder_Click"
                                    style="background-color: #0b6028; border-radius: 6px; font-size: 1rem; border: none;" />
                            </div>
                        </div>

                        <!-- Estimated Delivery / Guarantee Box -->
                        <div class="p-3 bg-light rounded-3 mt-auto border">
                            <div class="d-flex align-items-center gap-2">
                                <i class="bi bi-shield-check text-success fs-4"></i>
                                <div>
                                    <small class="text-muted fw-bold d-block" style="font-size: 0.72rem; letter-spacing: 0.5px;">SECURE DIRECT ESCROW WORKFLOW</small>
                                    <span class="small text-dark fw-semibold" style="font-size: 0.88rem;">Payment is recorded safely. Produce verified on arrival.</span>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>
    </div>
</asp:Content>
