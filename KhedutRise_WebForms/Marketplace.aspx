<%@ Page Title="Fresh From Farmers" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Marketplace.aspx.cs" Inherits="KhedutRise.Marketplace" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Fresh From Farmers -->
    <div class="kr-page-container">
        <!--Header -->
        <div class="mb-4">
            <small class="text-muted"><a href="Default.aspx" class="text-decoration-none text-muted">Home</a> / Marketplace / Products</small>
            <h2 class="fw-bold mt-1 text-dark" style="font-size: 2rem;">Fresh From Farmers</h2>
            <p class="text-muted small mb-0">Buy high grade agricultural products directly from verified farmers.</p>
        </div>

        <!-- Filter Category Chips-->
        <div class="d-flex flex-wrap gap-2 mb-4">
            <asp:LinkButton ID="btnGroundnut" runat="server" CssClass="btn btn-sm rounded-pill px-4 fw-bold text-white shadow-none" style="background-color: #0b6028; border: 1px solid #0b6028;" OnClick="FilterCategory_Click" CommandArgument="Groundnut">Groundnut</asp:LinkButton>
            <asp:LinkButton ID="btnCotton" runat="server" CssClass="btn btn-sm rounded-pill px-4 fw-semibold shadow-none bg-white text-dark" style="border: 1px solid #d1d5db;" OnClick="FilterCategory_Click" CommandArgument="Cotton">Cotton</asp:LinkButton>
            <asp:LinkButton ID="btnWheat" runat="server" CssClass="btn btn-sm rounded-pill px-4 fw-semibold shadow-none bg-white text-dark" style="border: 1px solid #d1d5db;" OnClick="FilterCategory_Click" CommandArgument="Wheat">Wheat</asp:LinkButton>
            <asp:LinkButton ID="btnVegetables" runat="server" CssClass="btn btn-sm rounded-pill px-4 fw-semibold shadow-none bg-white text-dark" style="border: 1px solid #d1d5db;" OnClick="FilterCategory_Click" CommandArgument="Vegetables">Vegetables</asp:LinkButton>
        </div>

        <div class="row g-4">
            <!-- LEFT: Filter Sidebar & Featured Farmers -->
            <div class="col-lg-3">
                <!-- Filter Products Card -->
                <div class="card card-custom p-4 mb-4">
                    <h6 class="fw-bold mb-3 text-dark">Filter Products</h6>
                    <div class="mb-3">
                        <label class="form-label small text-muted fw-bold" style="font-size: 0.72rem; letter-spacing: 0.5px;">LOCATION</label>
                        <input type="text" class="form-control form-control-sm" value="Rajkot, Gujarat" placeholder="Filter by location" />
                    </div>
                    <div>
                        <label class="form-label small text-muted fw-bold mb-2" style="font-size: 0.72rem; letter-spacing: 0.5px;">GRADE SELECTOR</label>
                        <div class="form-check">
                            <input class="form-check-input" type="checkbox" checked id="chkGradeA">
                            <label class="form-check-label small text-dark fw-semibold" for="chkGradeA">Grade A Premium</label>
                        </div>
                    </div>
                </div>

                <!-- Featured Farmers Card -->
                <div class="card card-custom p-3">
                    <small class="text-muted fw-bold mb-2 d-block" style="font-size: 0.75rem; letter-spacing: 0.5px;">Featured Farmers</small>
                    <div class="d-flex align-items-center gap-3">
                        <div class="text-white rounded-circle d-flex align-items-center justify-content-center fw-bold" style="width: 44px; height: 44px; background-color: #0b6028; font-size: 0.95rem;">RP</div>
                        <div>
                            <strong class="d-block text-dark" style="font-size: 0.92rem;">Ramesh Patel</strong>
                            <small class="text-muted" style="font-size: 0.8rem;">Rajkot | Rating: 4.9 (120 reviews)</small>
                        </div>
                    </div>
                </div>
            </div>

            <!-- RIGHT: Product Catalog Cards -->
            <div class="col-lg-9">
                <div class="row g-4">
                    <!-- Product 1: Premium Groundnut -->
                    <div class="col-md-6">
                        <div class="card card-custom h-100 overflow-hidden d-flex flex-column">
                            <img src="Images/groundnut_harvest.jpg" class="img-fluid object-fit-cover" style="height: 180px; width: 100%;" alt="Groundnut" />
                            <div class="p-4 d-flex flex-column flex-grow-1">
                                <h5 class="fw-bold mb-1 text-dark" style="font-size: 1.15rem;">Premium Groundnut</h5>
                                <small class="text-muted mb-2" style="font-size: 0.85rem;">Farmer: Ramesh Patel | Rajkot</small>
                                <div>
                                    <span class="badge bg-light text-dark border px-2 py-1 mb-3" style="font-size: 0.8rem;">Grade A | Available: 4.5 Tons</span>
                                </div>
                                <div class="d-flex justify-content-between align-items-center mt-auto pt-2">
                                    <h4 class="fw-bold text-success mb-0" style="font-size: 1.35rem;">₹5,900 <small class="text-muted fs-6">/ Q</small></h4>
                                    <a href="ProductDetails.aspx?id=1" class="btn btn-kr-primary btn-sm px-4 fw-bold" style="background-color: #0b6028 !important; border-color: #0b6028 !important;">Buy Now</a>
                                </div>
                            </div>
                        </div>
                    </div>

                    <!-- Product 2: Organic Bt Cotton -->
                    <div class="col-md-6">
                        <div class="card card-custom h-100 overflow-hidden d-flex flex-column">
                            <img src="Images/green_rows.jpg" class="img-fluid object-fit-cover" style="height: 180px; width: 100%;" alt="Cotton" />
                            <div class="p-4 d-flex flex-column flex-grow-1">
                                <h5 class="fw-bold mb-1 text-dark" style="font-size: 1.15rem;">Organic Bt Cotton</h5>
                                <small class="text-muted mb-2" style="font-size: 0.85rem;">Farmer: Savjibhai | Gondal</small>
                                <div>
                                    <span class="badge bg-light text-dark border px-2 py-1 mb-3" style="font-size: 0.8rem;">Grade A | Available: 12 Tons</span>
                                </div>
                                <div class="d-flex justify-content-between align-items-center mt-auto pt-2">
                                    <h4 class="fw-bold text-success mb-0" style="font-size: 1.35rem;">₹7,150 <small class="text-muted fs-6">/ Q</small></h4>
                                    <a href="ProductDetails.aspx?id=2" class="btn btn-kr-primary btn-sm px-4 fw-bold" style="background-color: #0b6028 !important; border-color: #0b6028 !important;">Buy Now</a>
                                </div>
                            </div>
                        </div>
                    </div>

                    <!-- Dynamic Repeater for remaining database products -->
                    <asp:Repeater ID="rptProducts" runat="server">
                        <ItemTemplate>
                            <div class="col-md-6">
                                <div class="card card-custom h-100 overflow-hidden d-flex flex-column">
                                    <img src="Images/groundnut_harvest.jpg" class="img-fluid object-fit-cover" style="height: 180px; width: 100%;" alt="Product" />
                                    <div class="p-4 d-flex flex-column flex-grow-1">
                                        <h5 class="fw-bold mb-1 text-dark" style="font-size: 1.15rem;"><%# Eval("ProductName") %></h5>
                                        <small class="text-muted mb-2" style="font-size: 0.85rem;">Farmer: <%# Eval("FarmerName") %> | <%# Eval("Location") %></small>
                                        <div>
                                            <span class="badge bg-light text-dark border px-2 py-1 mb-3" style="font-size: 0.8rem;"><%# Eval("Grade") %> | Available: <%# Eval("QuantityInQuintal") %> Quintals</span>
                                        </div>
                                        <div class="d-flex justify-content-between align-items-center mt-auto pt-2">
                                            <h4 class="fw-bold text-success mb-0" style="font-size: 1.35rem;">₹<%# Eval("PricePerQuintal", "{0:N0}") %><small class="text-muted fs-6">/ Q</small></h4>
                                            <a href="ProductDetails.aspx?id=<%# Eval("ProductId") %>" class="btn btn-kr-primary btn-sm px-4 fw-bold" style="background-color: #0b6028 !important; border-color: #0b6028 !important;">Buy Now</a>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
