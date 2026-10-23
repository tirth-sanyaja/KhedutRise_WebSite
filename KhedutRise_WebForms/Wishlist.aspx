<%@ Page Title="My Wishlist" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Wishlist.aspx.cs" Inherits="KhedutRise.Wishlist" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Wishlist -->
    <div class="kr-page-container">
        <!--  Header -->
        <div class="mb-4">
            <small class="text-muted"><a href="Default.aspx" class="text-decoration-none text-muted">Home</a> / <a href="Marketplace.aspx" class="text-decoration-none text-muted">Marketplace</a> / My Wishlist</small>
            <h2 class="fw-bold mt-1 text-dark" style="font-size: 2rem;">My Wishlist</h2>
        </div>

        <!-- 2 Top KPI Cards -->
        <div class="row g-3 mb-4">
            <div class="col-6 col-md-3">
                <div class="card card-custom p-3 text-start h-100">
                    <small class="text-muted fw-bold" style="font-size: 0.72rem; letter-spacing: 0.5px;">SAVED PRODUCTS</small>
                    <h4 class="fw-bold mt-2 mb-0" style="color: #0b6028; font-size: 1.4rem;">8 Items</h4>
                </div>
            </div>
            <div class="col-6 col-md-3">
                <div class="card card-custom p-3 text-start h-100">
                    <small class="text-muted fw-bold" style="font-size: 0.72rem; letter-spacing: 0.5px;">PRICE DROPS ACTIVE</small>
                    <h4 class="fw-bold mt-2 mb-0" style="color: #ea580c; font-size: 1.4rem;">2 Drops</h4>
                </div>
            </div>
        </div>

        <div class="d-flex flex-column gap-3">
            <!-- Wishlist Item 1: Premium Groundnut  -->
            <div class="card p-3 bg-white" style="border: 2px solid #ea580c; border-radius: 8px;">
                <div class="d-flex flex-wrap gap-3 justify-content-between align-items-center">
                    <div class="d-flex gap-3 align-items-center">
                        <img src="Images/groundnut.jpg" class="rounded-3 object-fit-cover flex-shrink-0" width="140" height="95" alt="Groundnut" />
                        <div>
                            <h5 class="fw-bold mb-1 text-dark" style="font-size: 1.2rem;">Premium Groundnut — Grade A</h5>
                            <small class="text-muted d-block mb-2" style="font-size: 0.85rem;">Farmer: Ramesh Patel | Rajkot</small>
                            <div class="d-flex align-items-center gap-2">
                                <span class="badge bg-danger-subtle text-danger px-2 py-1" style="font-size: 0.75rem;">PRICE DROP</span>
                                <small class="text-muted" style="font-size: 0.85rem;">Old: ₹6,100 / New: ₹5,900 / Q</small>
                            </div>
                        </div>
                    </div>
                    <a href="OrderDetails.aspx" class="btn btn-kr-primary px-4 py-2 fw-bold text-nowrap" style="background-color: #0b6028 !important; border-color: #0b6028 !important; border-radius: 6px;">Buy Now</a>
                </div>
            </div>

            <!-- Wishlist Item 2: Organic Bt Cotton -->
            <div class="card card-custom p-3 bg-white">
                <div class="d-flex flex-wrap gap-3 justify-content-between align-items-center">
                    <div class="d-flex gap-3 align-items-center">
                        <img src="Images/cotton.jpg" class="rounded-3 object-fit-cover flex-shrink-0" width="140" height="95" alt="Cotton" />
                        <div>
                            <h5 class="fw-bold mb-1 text-dark" style="font-size: 1.2rem;">Organic Bt Cotton</h5>
                            <small class="text-muted d-block" style="font-size: 0.85rem;">Farmer: Savjibhai | Gondal | ₹7,150 / Q</small>
                        </div>
                    </div>
                    <a href="OrderDetails.aspx" class="btn btn-kr-primary px-4 py-2 fw-bold text-nowrap" style="background-color: #0b6028 !important; border-color: #0b6028 !important; border-radius: 6px;">Buy Now</a>
                </div>
            </div>

            <!-- Wishlist Item 3: Lokwan Golden Wheat -->
            <div class="card card-custom p-3 bg-white">
                <div class="d-flex flex-wrap gap-3 justify-content-between align-items-center">
                    <div class="d-flex gap-3 align-items-center">
                        <img src="Images/wheat.jpg" class="rounded-3 object-fit-cover flex-shrink-0" width="140" height="95" alt="Wheat" />
                        <div>
                            <h5 class="fw-bold mb-1 text-dark" style="font-size: 1.2rem;">Lokwan Golden Wheat</h5>
                            <small class="text-muted d-block" style="font-size: 0.85rem;">Farmer: Hareshbhai | Junagadh | ₹2,450 / Q</small>
                        </div>
                    </div>
                    <a href="OrderDetails.aspx" class="btn btn-kr-primary px-4 py-2 fw-bold text-nowrap" style="background-color: #0b6028 !important; border-color: #0b6028 !important; border-radius: 6px;">Buy Now</a>
                </div>
            </div>

            <!-- Wishlist Item 4: Fresh Desi Tomatoes -->
            <div class="card card-custom p-3 bg-white">
                <div class="d-flex flex-wrap gap-3 justify-content-between align-items-center">
                    <div class="d-flex gap-3 align-items-center">
                        <img src="Images/tomato.jpg" class="rounded-3 object-fit-cover flex-shrink-0" width="140" height="95" alt="Tomatoes" />
                        <div>
                            <h5 class="fw-bold mb-1 text-dark" style="font-size: 1.2rem;">Fresh Desi Tomatoes (Grade A)</h5>
                            <small class="text-muted d-block" style="font-size: 0.85rem;">Farmer: Ramesh Patel | Rajkot Rural | ₹1,850 / Q</small>
                        </div>
                    </div>
                    <a href="OrderDetails.aspx" class="btn btn-kr-primary px-4 py-2 fw-bold text-nowrap" style="background-color: #0b6028 !important; border-color: #0b6028 !important; border-radius: 6px;">Buy Now</a>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
