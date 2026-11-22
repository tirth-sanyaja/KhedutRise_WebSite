<%@ Page Title="My Wishlist" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Wishlist.aspx.cs" Inherits="KhedutRise.Wishlist" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Wishlist: 100% Dynamic -->
    <div class="kr-page-container py-4">
        <!-- Header -->
        <div class="d-flex flex-wrap justify-content-between align-items-center mb-4">
            <div>
                <small class="text-muted"><a href="Default.aspx" class="text-decoration-none text-muted">Home</a> / <a href="Marketplace.aspx" class="text-decoration-none text-muted">Marketplace</a> / My Wishlist</small>
                <h2 class="fw-bold mt-1 text-dark" style="font-size: 2rem;">My Saved Crops &amp; Wishlist</h2>
                <p class="text-muted small mb-0">Track your favorite agricultural commodities, live mandi rate changes, and place direct buy orders.</p>
            </div>
            <div class="mt-2 mt-md-0">
                <a href="Marketplace.aspx" class="btn btn-outline-success px-4 py-2 fw-bold rounded-pill">
                    <i class="bi bi-plus-circle me-1"></i> Add More Crops
                </a>
            </div>
        </div>

        <asp:Label ID="lblWishlistMsg" runat="server" CssClass="d-block mb-3 fw-bold" EnableViewState="false"></asp:Label>

        <!-- 2 Top KPI Cards -->
        <div class="row g-3 mb-4">
            <div class="col-6 col-md-3">
                <div class="card card-custom p-3 text-start h-100 border rounded-3 bg-white shadow-sm">
                    <small class="text-muted fw-bold" style="font-size: 0.72rem; letter-spacing: 0.5px;">SAVED CROPS</small>
                    <h4 class="fw-bold mt-2 mb-0" style="color: #0b6028; font-size: 1.4rem;">
                        <asp:Literal ID="litTotalSaved" runat="server">0 Items</asp:Literal>
                    </h4>
                </div>
            </div>
            <div class="col-6 col-md-3">
                <div class="card card-custom p-3 text-start h-100 border rounded-3 bg-white shadow-sm">
                    <small class="text-muted fw-bold" style="font-size: 0.72rem; letter-spacing: 0.5px;">MARKET PRICE DROPS</small>
                    <h4 class="fw-bold mt-2 mb-0" style="color: #ea580c; font-size: 1.4rem;">
                        <asp:Literal ID="litPriceDrops" runat="server">0 Active</asp:Literal>
                    </h4>
                </div>
            </div>
        </div>

        <!-- Wishlist List -->
        <div class="d-flex flex-column gap-3">
            <asp:Repeater ID="rptWishlist" runat="server" OnItemCommand="rptWishlist_ItemCommand">
                <ItemTemplate>
                    <div class="card p-3 bg-white border rounded-3 shadow-sm">
                        <div class="d-flex flex-wrap gap-3 justify-content-between align-items-center">
                            <div class="d-flex gap-3 align-items-center">
                                <img src="<%# Eval("ImagePath") %>" class="rounded-3 object-fit-cover flex-shrink-0" width="140" height="95" alt="<%# Eval("ProductName") %>" onerror="this.src='Images/groundnut.jpg';" />
                                <div>
                                    <div class="d-flex align-items-center gap-2 mb-1">
                                        <h5 class="fw-bold mb-0 text-dark" style="font-size: 1.15rem;"><%# Eval("ProductName") %></h5>
                                        <span class="badge bg-success-subtle text-success small"><%# Eval("Category") %></span>
                                        <span class="badge bg-light text-dark border small"><%# Eval("Grade") %></span>
                                    </div>
                                    <small class="text-muted d-block mb-1" style="font-size: 0.85rem;">
                                        <i class="bi bi-person me-1"></i>Farmer: <strong><%# Eval("FarmerName") %></strong> &bull; <i class="bi bi-geo-alt me-1"></i><%# Eval("Location") %>
                                    </small>
                                    <div class="d-flex align-items-center gap-3">
                                        <span class="fs-5 fw-bold" style="color: #0b6028;">
                                            &#8377;<%# Convert.ToDecimal(Eval("PricePerQuintal")).ToString("N0") %> / Q
                                        </span>
                                        <small class="text-muted">Available: <strong><%# Eval("QuantityInQuintal") %> Q</strong></small>
                                    </div>
                                </div>
                            </div>
                            
                            <div class="d-flex gap-2">
                                <a href="ProductDetails.aspx?id=<%# Eval("ProductId") %>" class="btn btn-kr-primary px-4 py-2 fw-bold text-nowrap text-white rounded-2 shadow-sm" style="background-color: #0b6028;">
                                    <i class="bi bi-cart-check me-1"></i> Buy Now
                                </a>
                                <asp:LinkButton ID="btnRemove" runat="server" CommandName="Remove" CommandArgument='<%# Eval("WishlistId") %>' CssClass="btn btn-outline-danger px-3 py-2 fw-bold text-nowrap rounded-2">
                                    <i class="bi bi-trash"></i>
                                </asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>

            <asp:Panel ID="pnlEmptyWishlist" runat="server" Visible="false" CssClass="text-center py-5 bg-white border rounded-3 p-4">
                <i class="bi bi-heartbreak text-muted display-4 d-block mb-3"></i>
                <h4 class="fw-bold text-dark mb-2">Your Wishlist is Empty</h4>
                <p class="text-muted small mb-4">You have not saved any crop products to your wishlist yet.</p>
                <a href="Marketplace.aspx" class="btn btn-kr-primary px-4 py-2 fw-bold text-white rounded-pill" style="background-color: #0b6028;">
                    <i class="bi bi-shop me-1"></i> Browse Marketplace
                </a>
            </asp:Panel>
        </div>
    </div>
</asp:Content>
