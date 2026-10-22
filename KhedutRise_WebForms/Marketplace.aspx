<%@ Page Title="Fresh From Farmers" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Marketplace.aspx.cs" Inherits="KhedutRise.Marketplace" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Fresh From Farmers - 100% Dynamic Database Driven Marketplace -->
    <div class="kr-page-container py-4">
        <!-- Header -->
        <div class="d-flex flex-wrap justify-content-between align-items-center mb-4">
            <div>
                <small class="text-muted"><a href="Default.aspx" class="text-decoration-none text-muted">Home</a> / Marketplace / Products</small>
                <h2 class="fw-bold mt-1 text-dark" style="font-size: 2rem;">Fresh From Farmers</h2>
                <p class="text-muted small mb-0">Buy high grade agricultural products directly from verified farmers across Gujarat.</p>
            </div>
            <div class="mt-3 mt-md-0 d-flex gap-2 align-items-center">
                <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control form-control-sm" placeholder="Search crop or village..." style="max-width: 220px;"></asp:TextBox>
                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-sm btn-success px-3 fw-bold" OnClick="btnSearch_Click" style="background-color: #0b6028; border-color: #0b6028;" />
            </div>
        </div>

        <!-- Filter Category Chips -->
        <div class="d-flex flex-wrap gap-2 mb-4">
            <asp:LinkButton ID="btnAll" runat="server" CssClass="btn btn-sm rounded-pill px-4 fw-bold text-white shadow-none" style="background-color: #0b6028; border: 1px solid #0b6028;" OnClick="FilterCategory_Click" CommandArgument="">All Crops</asp:LinkButton>
            <asp:LinkButton ID="btnGroundnut" runat="server" CssClass="btn btn-sm rounded-pill px-4 fw-semibold shadow-none bg-white text-dark" style="border: 1px solid #d1d5db;" OnClick="FilterCategory_Click" CommandArgument="Groundnut">Groundnut</asp:LinkButton>
            <asp:LinkButton ID="btnCotton" runat="server" CssClass="btn btn-sm rounded-pill px-4 fw-semibold shadow-none bg-white text-dark" style="border: 1px solid #d1d5db;" OnClick="FilterCategory_Click" CommandArgument="Cotton">Cotton</asp:LinkButton>
            <asp:LinkButton ID="btnWheat" runat="server" CssClass="btn btn-sm rounded-pill px-4 fw-semibold shadow-none bg-white text-dark" style="border: 1px solid #d1d5db;" OnClick="FilterCategory_Click" CommandArgument="Wheat">Wheat</asp:LinkButton>
            <asp:LinkButton ID="btnVegetables" runat="server" CssClass="btn btn-sm rounded-pill px-4 fw-semibold shadow-none bg-white text-dark" style="border: 1px solid #d1d5db;" OnClick="FilterCategory_Click" CommandArgument="Vegetables">Vegetables</asp:LinkButton>
        </div>

        <div class="row g-4">
            <!-- LEFT: Filter Sidebar & Featured Farmers -->
            <div class="col-lg-3">
                <!-- Filter Products Card -->
                <div class="card card-custom p-4 mb-4">
                    <h6 class="fw-bold mb-3 text-dark">Filter By Location</h6>
                    <div class="mb-3">
                        <label class="form-label small text-muted fw-bold" style="font-size: 0.72rem; letter-spacing: 0.5px;">LOCATION / DISTRICT</label>
                        <asp:TextBox ID="txtLocationFilter" runat="server" CssClass="form-control form-control-sm" placeholder="e.g. Rajkot, Gondal"></asp:TextBox>
                    </div>
                    <div class="mb-3">
                        <label class="form-label small text-muted fw-bold" style="font-size: 0.72rem; letter-spacing: 0.5px;">SORT BY PRICE</label>
                        <asp:DropDownList ID="ddlSort" runat="server" CssClass="form-select form-select-sm" AutoPostBack="true" OnSelectedIndexChanged="ddlSort_SelectedIndexChanged">
                            <asp:ListItem Value="" Text="Latest Listings First"></asp:ListItem>
                            <asp:ListItem Value="price_asc" Text="Price: Low to High"></asp:ListItem>
                            <asp:ListItem Value="price_desc" Text="Price: High to Low"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <asp:Button ID="btnApplyFilter" runat="server" Text="Apply Filter" CssClass="btn btn-sm btn-outline-success w-100 fw-bold" OnClick="btnSearch_Click" />
                </div>

                <!-- Verified Farmers Guarantee Card -->
                <div class="card card-custom p-3 border-start border-4 border-success">
                    <small class="text-muted fw-bold mb-2 d-block" style="font-size: 0.75rem; letter-spacing: 0.5px;">Direct From Farmers</small>
                    <div class="d-flex align-items-center gap-3">
                        <div class="text-white rounded-circle d-flex align-items-center justify-content-center fw-bold" style="width: 44px; height: 44px; background-color: #0b6028; font-size: 0.95rem;">KR</div>
                        <div>
                            <strong class="d-block text-dark" style="font-size: 0.92rem;">100% Verified Quality</strong>
                            <small class="text-muted" style="font-size: 0.8rem;">Direct mandi price transparency</small>
                        </div>
                    </div>
                </div>
            </div>

            <!-- RIGHT: Dynamic Product Catalog Cards -->
            <div class="col-lg-9">
                <!-- Empty State Message when no products match -->
                <asp:Panel ID="pnlNoProducts" runat="server" Visible="false" CssClass="text-center py-5 bg-white rounded-3 border">
                    <i class="bi bi-inbox text-muted display-4 d-block mb-3"></i>
                    <h5 class="fw-bold text-dark">No Products Available</h5>
                    <p class="text-muted small mb-3">There are currently no active crop listings matching your search criteria.</p>
                    <asp:LinkButton ID="btnResetFilter" runat="server" CssClass="btn btn-sm btn-outline-secondary" OnClick="btnResetFilter_Click">Reset All Filters</asp:LinkButton>
                </asp:Panel>

                <div class="row g-4">
                    <!-- 100% Dynamic Repeater Loading Products from Database -->
                    <asp:Repeater ID="rptProducts" runat="server">
                        <ItemTemplate>
                            <div class="col-md-6 col-xl-4">
                                <div class="card card-custom h-100 overflow-hidden d-flex flex-column border shadow-sm">
                                    <img src='<%# ResolveUrl(Eval("ImagePath").ToString()) %>' class="img-fluid object-fit-cover" style="height: 180px; width: 100%;" alt='<%# Eval("ProductName") %>' onerror="this.src='Images/groundnut_harvest.jpg';" />
                                    <div class="p-3 d-flex flex-column flex-grow-1">
                                        <div class="d-flex justify-content-between align-items-start mb-1">
                                            <span class="badge bg-success-subtle text-success border border-success-subtle small fw-bold"><%# Eval("Category") %></span>
                                            <small class="text-muted"><%# Eval("Grade") %></small>
                                        </div>
                                        <h5 class="fw-bold mb-1 text-dark" style="font-size: 1.15rem;"><%# Eval("ProductName") %></h5>
                                        <small class="text-muted mb-2 d-block" style="font-size: 0.85rem;">
                                            <i class="bi bi-person-fill text-success me-1"></i><%# Eval("FarmerName") %> 
                                            <span class="mx-1">•</span> 
                                            <i class="bi bi-geo-alt-fill text-danger me-1"></i><%# Eval("Location") %>
                                        </small>
                                        <div class="mb-3">
                                            <span class="badge bg-light text-dark border px-2 py-1" style="font-size: 0.8rem;">
                                                Available: <strong class="text-dark"><%# Eval("QuantityInQuintal") %> Quintals</strong>
                                            </span>
                                        </div>
                                        <div class="d-flex justify-content-between align-items-center mt-auto pt-2 border-top">
                                            <h4 class="fw-bold text-success mb-0" style="font-size: 1.35rem;">
                                                ₹<%# Eval("PricePerQuintal", "{0:N0}") %> <small class="text-muted fs-6">/ Q</small>
                                            </h4>
                                            <a href='ProductDetails.aspx?id=<%# Eval("ProductId") %>' class="btn btn-kr-primary btn-sm px-3 fw-bold" style="background-color: #0b6028 !important; border-color: #0b6028 !important; border-radius: 6px;">Buy Now</a>
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
