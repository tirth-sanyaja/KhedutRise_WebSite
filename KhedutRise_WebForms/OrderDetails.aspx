<%@ Page Title="Order Details" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="OrderDetails.aspx.cs" Inherits="KhedutRise.OrderDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Order Details & Escrow Fulfillment-->
    <div class="kr-page-container">
        <!--Header -->
        <div class="mb-4">
            <small class="text-muted"><a href="Default.aspx" class="text-decoration-none text-muted">Home</a> / <a href="Marketplace.aspx" class="text-decoration-none text-muted">Marketplace</a> / <a href="MyPurchases.aspx" class="text-decoration-none text-muted">My Purchases</a> / Order Details</small>
            <h2 class="fw-bold mt-1 text-dark" style="font-size: 2rem;">Order Details — 10482</h2>
        </div>

        <!-- Horizontal Stepper-->
        <div class="card card-custom p-4 mb-4">
            <div class="position-relative py-2">
                <!-- Background Connection Line -->
                <div class="position-absolute top-50 start-0 w-100 translate-middle-y" style="height: 4px; z-index: 1;">
                    <div class="d-flex h-100">
                        <div style="width: 75%; background-color: #0b6028;"></div>
                        <div style="width: 25%; background-color: #e2e8f0;"></div>
                    </div>
                </div>

                <!-- 5 Stepper Milestone Nodes -->
                <div class="d-flex justify-content-between position-relative" style="z-index: 2;">
                    <div class="text-center bg-white px-2">
                        <span class="rounded-circle d-inline-flex align-items-center justify-content-center text-white" style="width: 22px; height: 22px; background-color: #0b6028; font-size: 0.65rem;">•</span>
                        <small class="d-block fw-bold text-dark mt-2" style="font-size: 0.75rem;">Order Placed</small>
                    </div>
                    <div class="text-center bg-white px-2">
                        <span class="rounded-circle d-inline-flex align-items-center justify-content-center text-white" style="width: 22px; height: 22px; background-color: #0b6028; font-size: 0.65rem;">•</span>
                        <small class="d-block fw-bold text-dark mt-2" style="font-size: 0.75rem;">Escrow Paid</small>
                    </div>
                    <div class="text-center bg-white px-2">
                        <span class="rounded-circle d-inline-flex align-items-center justify-content-center text-white" style="width: 22px; height: 22px; background-color: #0b6028; font-size: 0.65rem;">•</span>
                        <small class="d-block fw-bold text-dark mt-2" style="font-size: 0.75rem;">Shipped</small>
                    </div>
                    <div class="text-center bg-white px-2">
                        <span class="rounded-circle d-inline-flex align-items-center justify-content-center text-white" style="width: 22px; height: 22px; background-color: #0b6028; font-size: 0.65rem;">•</span>
                        <small class="d-block fw-bold text-dark mt-2" style="font-size: 0.75rem;">Out for Delivery</small>
                    </div>
                    <div class="text-center bg-white px-2">
                        <span class="rounded-circle d-inline-flex align-items-center justify-content-center border" style="width: 22px; height: 22px; background-color: #ffffff; border-color: #cbd5e1 !important;"></span>
                        <small class="d-block text-muted mt-2" style="font-size: 0.75rem;">Verified Arrival</small>
                    </div>
                </div>
            </div>
        </div>

        <div class="row g-4">
            <!-- LEFT: Product & Escrow Financial Summary -->
            <div class="col-lg-7">
                <div class="card card-custom p-4 h-100 d-flex flex-column">
                    <div class="d-flex gap-3 align-items-center mb-4">
                        <img src="Images/groundnut_harvest.jpg" class="rounded-3 object-fit-cover flex-shrink-0" width="160" height="110" alt="Groundnut" />
                        <div>
                            <h5 class="fw-bold mb-1 text-dark" style="font-size: 1.25rem;">Premium Groundnut — Grade A</h5>
                            <small class="text-muted d-block mb-2" style="font-size: 0.88rem;">Farmer: Ramesh Patel | Rajkot</small>
                            <p class="mb-0 text-dark fw-bold" style="color: #0b6028 !important; font-size: 0.95rem;">Quantity: 15 Quintal</p>
                            <p class="mb-0 text-dark fw-bold" style="color: #0b6028 !important; font-size: 0.95rem;">Rate: ₹5,900 / Quintal</p>
                        </div>
                    </div>

                    <div class="border-top pt-3 mt-auto">
                        <div class="d-flex justify-content-between py-2 text-dark" style="font-size: 0.95rem;">
                            <span>Subtotal</span>
                            <span class="fw-bold">₹88,500</span>
                        </div>
                        <div class="d-flex justify-content-between py-2 text-dark" style="font-size: 0.95rem;">
                            <span>Logistics Carrier Fee</span>
                            <span class="fw-bold">₹3,200</span>
                        </div>
                        <div class="d-flex justify-content-between py-2 border-top mt-2" style="font-size: 1.2rem;">
                            <span class="fw-bold text-dark">Total Paid (in Escrow)</span>
                            <span class="fw-bold" style="color: #0b6028;">₹91,700</span>
                        </div>
                    </div>
                </div>
            </div>

            <!-- RIGHT: Logistics & Delivery Address -->
            <div class="col-lg-5">
                <div class="card card-custom p-4 h-100 d-flex flex-column">
                    <h5 class="fw-bold mb-4 text-dark">Logistics & Delivery Address</h5>

                    <div class="mb-3">
                        <small class="text-muted fw-bold d-block" style="font-size: 0.72rem; letter-spacing: 0.5px;">BUYER DETAILS</small>
                        <p class="mb-0 text-dark fw-bold" style="font-size: 0.95rem;">Dev Patel | Patel Organics Ltd</p>
                    </div>

                    <div class="mb-4">
                        <small class="text-muted fw-bold d-block" style="font-size: 0.72rem; letter-spacing: 0.5px;">DELIVERY HUB LOCATION</small>
                        <p class="mb-0 text-dark fw-bold" style="font-size: 0.95rem; line-height: 1.4;">Warehouse Section C, Gondal Bypass road, Rajkot</p>
                    </div>

                    <div class="d-flex flex-column gap-2 mt-auto">
                        <button type="button" class="btn btn-kr-primary w-100 py-2 fw-bold" style="background-color: #0b6028 !important; border-color: #0b6028 !important;">Contact Farmer</button>
                        <button type="button" class="btn w-100 py-2 fw-bold" style="background-color: #ffffff; color: #0b6028; border: 1.5px solid #0b6028;">Download Invoice</button>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
