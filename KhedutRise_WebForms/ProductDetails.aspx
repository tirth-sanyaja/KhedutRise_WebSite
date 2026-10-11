<%@ Page Title="Product Details" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="ProductDetails.aspx.cs" Inherits="KhedutRise.ProductDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Product Details-->
    <div class="kr-page-container">
        <!-- Header -->
        <div class="mb-4">
            <small class="text-muted"><a href="Default.aspx" class="text-decoration-none text-muted">Home</a> / <a href="Marketplace.aspx" class="text-decoration-none text-muted">Marketplace</a> / Product Details</small>
            <h2 class="fw-bold mt-1 text-dark" style="font-size: 2rem;">Product Details</h2>
        </div>

        <div class="row g-5">
            <!-- LEFT: Image Gallery with 3 Thumbnails -->
            <div class="col-lg-6">
                <div class="card card-custom overflow-hidden mb-3 border">
                    <img id="mainProductImg" src="Images/groundnut_harvest.jpg" class="img-fluid object-fit-cover" style="height: 350px; width: 100%;" alt="Groundnut" />
                </div>
                <div class="row g-2">
                    <div class="col-4">
                        <img src="Images/groundnut_harvest.jpg" class="img-fluid rounded border object-fit-cover" style="height: 85px; width: 100%; cursor: pointer;" alt="thumb1" onclick="document.getElementById('mainProductImg').src=this.src;" />
                    </div>
                    <div class="col-4">
                        <img src="Images/hero_farmer.jpg" class="img-fluid rounded border object-fit-cover" style="height: 85px; width: 100%; cursor: pointer;" alt="thumb2" onclick="document.getElementById('mainProductImg').src=this.src;" />
                    </div>
                    <div class="col-4">
                        <img src="Images/green_rows.jpg" class="img-fluid rounded border object-fit-cover" style="height: 85px; width: 100%; cursor: pointer;" alt="thumb3" onclick="document.getElementById('mainProductImg').src=this.src;" />
                    </div>
                </div>
            </div>

            <!-- RIGHT: Product Specifications & Purchase Actions -->
            <div class="col-lg-6">
                <div class="d-flex flex-column h-100">
                    <h3 class="fw-bold text-dark mb-1" style="font-size: 1.85rem;">Premium Groundnut — Grade A</h3>
                    <p class="text-muted mb-3" style="font-size: 0.95rem;">Sown in organic Rajkot farms. Zero chemical pesticides.</p>

                    <div class="mb-4">
                        <h2 class="fw-bold mb-1" style="color: #0b6028; font-size: 2.2rem;">₹5,900 <small class="fs-5 text-muted fw-normal">/ Quintal</small></h2>
                        <small class="text-muted" style="font-size: 0.88rem;">Available Stock: 4.5 Tons (45 Quintal)</small>
                    </div>

                    <!-- Farmer Detail Box -->
                    <div class="card bg-light border-0 p-3 mb-4 rounded-3">
                        <small class="text-muted fw-bold mb-2 d-block" style="font-size: 0.72rem; letter-spacing: 0.5px;">FARMER DETAIL</small>
                        <div class="d-flex align-items-center gap-3">
                            <div class="text-white rounded-circle d-flex align-items-center justify-content-center fw-bold" style="width: 48px; height: 48px; background-color: #1e293b; font-size: 1.1rem;">RP</div>
                            <div>
                                <strong class="d-block text-dark" style="font-size: 1rem;">Ramesh Patel (Verified Farmer)</strong>
                                <small class="text-muted" style="font-size: 0.82rem;">Rajkot Bypass, Gujarat | Experience: 12 Years</small>
                            </div>
                        </div>
                    </div>

                    <!-- Side-by-Side Action Buttons -->
                    <div class="d-flex gap-3 mb-4">
                        <a href="Wishlist.aspx" class="btn w-50 py-2 fw-bold" style="background-color: #ffffff; color: #0b6028; border: 1.5px solid #0b6028; border-radius: 8px;">Add to Cart</a>
                        <a href="OrderDetails.aspx" class="btn btn-kr-primary w-50 py-2 fw-bold" style="background-color: #0b6028 !important; border-color: #0b6028 !important; border-radius: 8px;">Buy Now Securely</a>
                    </div>

                    <!-- Estimated Delivery Box -->
                    <div class="p-3 bg-light rounded-3 mt-auto border">
                        <small class="text-muted fw-bold d-block" style="font-size: 0.72rem; letter-spacing: 0.5px;">ESTIMATED DELIVERY</small>
                        <span class="small text-dark fw-semibold" style="font-size: 0.88rem;">Deliver by Friday, 14 August (Rajkot region)</span>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
