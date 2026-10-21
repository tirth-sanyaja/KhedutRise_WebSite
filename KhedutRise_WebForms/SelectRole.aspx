<%@ Page Title="Choose Workspace" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="SelectRole.aspx.cs" Inherits="KhedutRise.SelectRole" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Role Selection Screen -->
    <div class="kr-page-container py-5">
        <% if (Request.QueryString["msg"] == "register_required") { %>
        <div class="alert alert-warning alert-dismissible fade show text-center border-0 shadow-sm mb-4 mx-auto" role="alert" style="max-width: 900px; background-color: #fffbeb; color: #92400e; border-left: 5px solid #f59e0b !important;">
            <i class="bi bi-person-check-fill me-2 fs-5"></i>
            <strong>Account Required:</strong> Please select your role or register below to proceed with purchase, booking, or services.
            <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
        </div>
        <% } %>

        <div class="text-center mb-5">
            <h2 class="fw-bold mb-2 text-dark" style="font-size: 2.2rem;">How would you like to use <span class="brand-khedut">Khedut</span><span class="brand-rise">Rise</span>?</h2>
            <p class="text-secondary" style="font-size: 1.05rem;">Choose your customized workspace configuration to proceed onboarding.</p>
        </div>

        <div class="row g-4 justify-content-center">
            <!-- 1. FARMER ROLE CARD -->
            <div class="col-lg-4 col-md-6">
                <div class="card h-100 p-4 border rounded-3 shadow-none d-flex flex-column" style="border-color: #e2e8f0 !important; background: #ffffff;">
                    <img src="Images/hero_farmer.jpg" class="rounded-3 mb-4 object-fit-cover shadow-sm" height="190" alt="Farmer" />
                    <h4 class="fw-bold mb-3" style="color: #0b6028; font-size: 1.35rem; letter-spacing: 0.5px;">FARMER</h4>
                    <p class="text-secondary mb-4" style="font-size: 0.95rem; line-height: 1.5;">Manage your farm, sell your produce and, access agricultural markets and services.</p>
                    <ul class="list-unstyled mb-5" style="font-size: 0.95rem; line-height: 1.9;">
                        <li class="fw-bold text-dark">• Farm Management & Soil Profiles</li>
                        <li class="fw-bold text-dark">• APMC Live Rates & Weather Alerts</li>
                        <li class="fw-bold text-dark">• Land Listings & Tractor Bookings</li>
                    </ul>
                    <div class="mt-auto">
                        <a href="RegisterFarmer.aspx" class="btn w-100 py-2 fw-bold text-white shadow-sm" style="background-color: #0b6028; border-radius: 6px; font-size: 0.95rem;">Register as Farmer</a>
                    </div>
                </div>
            </div>

            <!-- 2. BUYER ROLE CARD -->
            <div class="col-lg-4 col-md-6">
                <div class="card h-100 p-4 border rounded-3 shadow-none d-flex flex-column" style="border-color: #e2e8f0 !important; background: #ffffff;">
                    <img src="Images/groundnut_harvest.jpg" class="rounded-3 mb-4 object-fit-cover shadow-sm" height="190" alt="Buyer" />
                    <h4 class="fw-bold mb-3" style="color: #0b6028; font-size: 1.35rem; letter-spacing: 0.5px;">BUYER</h4>
                    <p class="text-secondary mb-4" style="font-size: 0.95rem; line-height: 1.5;">Discover and purchase agricultural products directly, from verified farmers.</p>
                    <ul class="list-unstyled mb-5" style="font-size: 0.95rem; line-height: 1.9;">
                        <li class="fw-bold text-dark">• Browse Cotton & Groundnut Catalogs</li>
                        <li class="fw-bold text-dark">• Secure Escrow Checkout System</li>
                        <li class="fw-bold text-dark">• Direct Farmers Chat Integration</li>
                    </ul>
                    <div class="mt-auto">
                        <a href="RegisterBuyer.aspx" class="btn w-100 py-2 fw-bold text-white shadow-sm" style="background-color: #0b6028; border-radius: 6px; font-size: 0.95rem;">Register as Buyer</a>
                    </div>
                </div>
            </div>

            <!-- 3. OPERATOR ROLE CARD -->
            <div class="col-lg-4 col-md-6">
                <div class="card h-100 p-4 border rounded-3 shadow-none d-flex flex-column" style="border-color: #e2e8f0 !important; background: #ffffff;">
                    <img src="Images/massey_tractor.jpg" class="rounded-3 mb-4 object-fit-cover shadow-sm" height="190" alt="Operator" />
                    <h4 class="fw-bold mb-3" style="color: #f95700; font-size: 1.35rem; letter-spacing: 0.5px;">OPERATOR</h4>
                    <p class="text-secondary mb-4" style="font-size: 0.95rem; line-height: 1.5;">Provide tractor & equipment services to local farmers.</p>
                    <ul class="list-unstyled mb-5" style="font-size: 0.95rem; line-height: 1.9;">
                        <li class="fw-bold text-dark">• Machinery Rent Listings Creation</li>
                        <li class="fw-bold text-dark">• Track Active Service Bookings</li>
                        <li class="fw-bold text-dark">• Verified Payouts & Earnings Log</li>
                    </ul>
                    <div class="mt-auto">
                        <a href="RegisterOperator.aspx" class="btn w-100 py-2 fw-bold text-white shadow-sm" style="background-color: #f95700; border-radius: 6px; font-size: 0.95rem;">Register as Operator</a>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
