<%@ Page Title="Fresh From Farmers" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Marketplace.aspx.cs" Inherits="KhedutRise.Marketplace" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .product-card {
            border: 1px solid #e5e7eb;
            border-radius: 14px;
            background: #ffffff;
            transition: all 0.25s cubic-bezier(0.16, 1, 0.3, 1);
            overflow: hidden;
        }
        .product-card:hover {
            transform: translateY(-4px);
            box-shadow: 0 14px 28px -10px rgba(11, 96, 40, 0.15), 0 4px 10px rgba(0, 0, 0, 0.04);
            border-color: #86efac;
        }
        .product-img-wrapper {
            position: relative;
            height: 200px;
            width: 100%;
            overflow: hidden;
            background: #f1f5f9;
        }
        .product-img-wrapper img {
            width: 100%;
            height: 100%;
            object-fit: cover;
            transition: transform 0.4s ease;
        }
       
        
        .filter-sidebar-card {
            border: 1px solid #e5e7eb;
            border-radius: 14px;
            background: #ffffff;
            box-shadow: 0 2px 6px rgba(0, 0, 0, 0.02);
        }
        .category-chip {
            transition: all 0.2s ease;
            font-size: 0.88rem;
            padding: 0.45rem 1.25rem;
            border-radius: 9999px;
            text-decoration: none;
            display: inline-flex;
            align-items: center;
            gap: 0.4rem;
        }
        .category-chip:hover {
            transform: translateY(-1px);
        }
    </style>

    <!-- Fresh From Farmers - 100% Dynamic Database Driven Marketplace -->
    <div class="kr-page-container py-4">
        <!-- Header -->
        <div class="d-flex flex-wrap justify-content-between align-items-center mb-4 pb-2 border-bottom">
            <div>
                <small class="text-muted"><a href="Default.aspx" class="text-decoration-none text-muted">Home</a> / Marketplace / <span class="text-dark fw-semibold">Crop Catalog</span></small>
                <h2 class="fw-bold mt-1 text-dark" style="font-size: 2rem; letter-spacing: -0.5px;">Fresh From Farmers</h2>
                <p class="text-muted small mb-0">Direct procurement from verified farmers across Gujarat APMCs — 0% middlemen commission.</p>
            </div>
            <div class="mt-3 mt-md-0 d-flex align-items-center gap-2">
                <asp:Label ID="lblProductCount" runat="server" CssClass="badge bg-success-subtle text-success border border-success-subtle px-3 py-2 fw-bold rounded-pill" style="font-size: 0.85rem;">
                    <i class="bi bi-box-seam me-1"></i> Loading Listings...
                </asp:Label>
            </div>
        </div>

        <!-- Filter Category Chips -->
        <div class="d-flex flex-wrap gap-2 mb-4">
            <asp:LinkButton ID="btnAll" runat="server" CssClass="category-chip fw-bold text-white shadow-sm" style="background-color: #0b6028; border: 1px solid #0b6028;" OnClick="FilterCategory_Click" CommandArgument="">
                <i class="bi bi-grid-fill"></i> All Crops
            </asp:LinkButton>
            <asp:LinkButton ID="btnGroundnut" runat="server" CssClass="category-chip fw-semibold bg-white text-dark" style="border: 1px solid #d1d5db;" OnClick="FilterCategory_Click" CommandArgument="Groundnut">
                 Groundnut
            </asp:LinkButton>
            <asp:LinkButton ID="btnCotton" runat="server" CssClass="category-chip fw-semibold bg-white text-dark" style="border: 1px solid #d1d5db;" OnClick="FilterCategory_Click" CommandArgument="Cotton">
                 Cotton
            </asp:LinkButton>
            <asp:LinkButton ID="btnWheat" runat="server" CssClass="category-chip fw-semibold bg-white text-dark" style="border: 1px solid #d1d5db;" OnClick="FilterCategory_Click" CommandArgument="Wheat">
                 Wheat
            </asp:LinkButton>
            <asp:LinkButton ID="btnVegetables" runat="server" CssClass="category-chip fw-semibold bg-white text-dark" style="border: 1px solid #d1d5db;" OnClick="FilterCategory_Click" CommandArgument="Vegetables">
                 Vegetables
            </asp:LinkButton>
            <asp:LinkButton ID="btnSpices" runat="server" CssClass="category-chip fw-semibold bg-white text-dark" style="border: 1px solid #d1d5db;" OnClick="FilterCategory_Click" CommandArgument="Spices">
                 Spices (Cumin)
            </asp:LinkButton>
        </div>

        <div class="row g-4">
            <!-- LEFT: Filter Sidebar & Featured Farmers -->
            <div class="col-lg-3">
                <!-- Filter Products Card -->
                <div class="filter-sidebar-card p-4 mb-4">
                    <div class="d-flex align-items-center justify-content-between mb-3">
                        <h6 class="fw-bold mb-0 text-dark">
                            <i class="bi bi-sliders me-1 text-success"></i> Filter Products
                        </h6>
                        <asp:LinkButton ID="btnResetFilterSidebar" runat="server" CssClass="small text-muted text-decoration-none" OnClick="btnResetFilter_Click" ToolTip="Reset Filters">
                            <i class="bi bi-arrow-counterclockwise"></i> Reset
                        </asp:LinkButton>
                    </div>

                    <div class="mb-3">
                        <label class="form-label small text-muted fw-bold" style="font-size: 0.72rem; letter-spacing: 0.5px;">LOCATION / DISTRICT</label>
                        <div class="input-group input-group-sm">
                            <span class="input-group-text bg-light text-muted border-end-0"><i class="bi bi-geo-alt"></i></span>
                            <asp:TextBox ID="txtLocationFilter" runat="server" CssClass="form-control border-start-0 ps-0" placeholder="e.g. Rajkot, Gondal"></asp:TextBox>
                        </div>
                    </div>

                    <div class="mb-4">
                        <label class="form-label small text-muted fw-bold" style="font-size: 0.72rem; letter-spacing: 0.5px;">SORT BY PRICE</label>
                        <asp:DropDownList ID="ddlSort" runat="server" CssClass="form-select form-select-sm" AutoPostBack="true" OnSelectedIndexChanged="ddlSort_SelectedIndexChanged">
                            <asp:ListItem Value="" Text="Latest Listings First"></asp:ListItem>
                            <asp:ListItem Value="price_asc" Text="Price: Low to High"></asp:ListItem>
                            <asp:ListItem Value="price_desc" Text="Price: High to Low"></asp:ListItem>
                        </asp:DropDownList>
                    </div>

                    <asp:Button ID="btnApplyFilter" runat="server" Text="Apply Filter" CssClass="btn btn-sm w-100 fw-bold text-white shadow-sm" OnClick="btnSearch_Click" style="background-color: #0b6028; border-color: #0b6028; border-radius: 8px; padding: 0.5rem;" />
                </div>

                <!-- Verified Farmers Guarantee Card -->
                <div class="card card-custom p-3 border-start border-4 border-success shadow-sm mb-4">
                    <small class="text-muted fw-bold mb-2 d-block" style="font-size: 0.75rem; letter-spacing: 0.5px;">DIRECT FROM FARMERS</small>
                    <div class="d-flex align-items-center gap-3">
                        <div class="text-white rounded-circle d-flex align-items-center justify-content-center fw-bold shadow-sm" style="width: 44px; height: 44px; background-color: #0b6028; font-size: 0.95rem;">KR</div>
                        <div>
                            <strong class="d-block text-dark" style="font-size: 0.92rem;">100% Verified Quality</strong>
                            <small class="text-muted" style="font-size: 0.8rem;">Direct mandi price transparency</small>
                        </div>
                    </div>
                </div>

                <!-- Escrow Security Strip -->
                <div class="p-3 rounded-3 border bg-light small text-muted">
                    <div class="d-flex gap-2 align-items-center mb-1">
                        <i class="bi bi-shield-check text-success fs-5"></i>
                        <strong class="text-dark">KhedutRise Escrow Protection</strong>
                    </div>
                    <p class="mb-0" style="font-size: 0.78rem;">Payment is safely deposited in escrow and released to the farmer only upon delivery inspection.</p>
                </div>
            </div>

            <!-- RIGHT: Dynamic Product Catalog Cards -->
            <div class="col-lg-9">
                <!-- Empty State Message when no products match -->
                <asp:Panel ID="pnlNoProducts" runat="server" Visible="false" CssClass="text-center py-5 bg-white rounded-4 border shadow-sm">
                    <div class="rounded-circle d-inline-flex align-items-center justify-content-center mb-3" style="width: 64px; height: 64px; background-color: #f0fdf4; color: #16a34a;">
                        <i class="bi bi-basket3 fs-2"></i>
                    </div>
                    <h5 class="fw-bold text-dark mb-1">No Crop Listings Found</h5>
                    <p class="text-muted small mb-3">There are currently no active harvest listings matching your selected filter.</p>
                    <asp:LinkButton ID="btnResetFilter" runat="server" CssClass="btn btn-sm btn-outline-success px-4 py-2 fw-semibold rounded-pill" OnClick="btnResetFilter_Click">
                        <i class="bi bi-arrow-counterclockwise me-1"></i> View All Available Produce
                    </asp:LinkButton>
                </asp:Panel>

                <div class="row g-4">
                    <!-- 100% Dynamic Repeater Loading Products from Database -->
                    <asp:Repeater ID="rptProducts" runat="server">
                        <ItemTemplate>
                            <div class="col-md-6 col-xl-4">
                                <div class="product-card h-100 d-flex flex-column">
                                    <!-- Product Real-World Photo -->
                                    <div class="product-img-wrapper">
                                        <img src='<%# ResolveUrl(Eval("ImagePath").ToString()) %>'
                                             alt='<%# Eval("ProductName") %>'
                                             onerror='this.onerror=null; this.src="<%# ResolveUrl(GetFallbackImage(Eval("Category"), Eval("ProductName"))) %>";' />
                                        <div class="position-absolute top-0 end-0 m-2">
                                            <span class='badge <%# GetCategoryBadgeClass(Eval("Category")) %> fw-bold shadow-sm' style="font-size: 0.72rem; border-radius: 6px;">
                                                <%# Eval("Category") %>
                                            </span>
                                        </div>
                                    </div>

                                    <div class="p-3 d-flex flex-column flex-grow-1">
                                        <!-- Grade & Verification -->
                                        <div class="d-flex justify-content-between align-items-center mb-1">
                                            <span class="badge bg-light text-secondary border small fw-semibold">
                                                <%# Eval("Grade") %>
                                            </span>
                                            <small class="text-success fw-semibold">
                                                <i class="bi bi-patch-check-fill me-1"></i>Verified
                                            </small>
                                        </div>

                                        <!-- Product Name -->
                                        <h5 class="fw-bold mb-1 text-dark" style="font-size: 1.15rem; line-height: 1.3;">
                                            <%# Eval("ProductName") %>
                                        </h5>

                                        <!-- Farmer Name & Location -->
                                        <small class="text-muted mb-2 d-block" style="font-size: 0.84rem;">
                                            <i class="bi bi-person-fill text-success me-1"></i><%# Eval("FarmerName") %> 
                                            <span class="mx-1">•</span> 
                                            <i class="bi bi-geo-alt-fill text-danger me-1"></i><%# Eval("Location") %>
                                        </small>

                                        <!-- Stock Availability -->
                                        <div class="mb-3">
                                            <span class="badge bg-light text-dark border px-2 py-1" style="font-size: 0.8rem;">
                                                <i class="bi bi-boxes me-1 text-primary"></i>Stock: <strong class="text-dark"><%# Eval("QuantityInQuintal") %> Quintals</strong>
                                            </span>
                                        </div>

                                        <!-- Price & Buy Button -->
                                        <div class="d-flex justify-content-between align-items-center mt-auto pt-3 border-top">
                                            <div>
                                                <h4 class="fw-bold mb-0" style="color: #0b6028; font-size: 1.35rem;">
                                                    ₹<%# Eval("PricePerQuintal", "{0:N0}") %>
                                                </h4>
                                                <small class="text-muted" style="font-size: 0.72rem;">per Quintal (100 kg)</small>
                                            </div>
                                            <a href='ProductDetails.aspx?id=<%# Eval("ProductId") %>' class="btn btn-sm px-3 py-2 fw-bold text-white shadow-sm" style="background-color: #0b6028; border-color: #0b6028; border-radius: 8px;">
                                                <i class="bi bi-bag-check-fill me-1"></i> Buy Now
                                            </a>
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
