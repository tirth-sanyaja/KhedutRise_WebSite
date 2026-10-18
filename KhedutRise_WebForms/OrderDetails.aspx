<%@ Page Title="Order Details" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="OrderDetails.aspx.cs" Inherits="KhedutRise.OrderDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Order Details & Escrow Fulfillment-->
    <div class="kr-page-container py-4">
        <!--Header -->
        <div class="mb-4">
            <small class="text-muted"><a href="Default.aspx" class="text-decoration-none text-muted">Home</a> / <a href="Marketplace.aspx" class="text-decoration-none text-muted">Marketplace</a> / <a href="MyPurchases.aspx" class="text-decoration-none text-muted">My Purchases</a> / Order Details</small>
            <h2 class="fw-bold mt-1 text-dark" style="font-size: 2rem;">Order Details — #<asp:Literal ID="litOrderIdHeader" runat="server">10482</asp:Literal></h2>
            <p class="text-muted small mb-0">Live escrow milestone tracking and physical dispatch verification.</p>
        </div>

        <asp:Label ID="lblOrderMsg" runat="server" CssClass="d-block mb-3 fw-bold"></asp:Label>

        <!-- Dynamic Horizontal Stepper-->
        <div class="card card-custom p-4 mb-4 border shadow-sm">
            <div class="position-relative py-2">
                <!-- Background Connection Line -->
                <div class="position-absolute top-50 start-0 w-100 translate-middle-y" style="height: 4px; z-index: 1;">
                    <div class="d-flex h-100">
                        <asp:Literal ID="litStepperProgressLine" runat="server">
                            <div style="width: 75%; background-color: #0b6028;"></div>
                            <div style="width: 25%; background-color: #e2e8f0;"></div>
                        </asp:Literal>
                    </div>
                </div>

                <!-- 5 Stepper Milestone Nodes -->
                <div class="d-flex justify-content-between position-relative" style="z-index: 2;">
                    <div class="text-center bg-white px-2">
                        <span id="node1" runat="server" class="rounded-circle d-inline-flex align-items-center justify-content-center text-white" style="width: 22px; height: 22px; background-color: #0b6028; font-size: 0.65rem;">✓</span>
                        <small class="d-block fw-bold text-dark mt-2" style="font-size: 0.75rem;">Order Placed</small>
                    </div>
                    <div class="text-center bg-white px-2">
                        <span id="node2" runat="server" class="rounded-circle d-inline-flex align-items-center justify-content-center text-white" style="width: 22px; height: 22px; background-color: #0b6028; font-size: 0.65rem;">✓</span>
                        <small class="d-block fw-bold text-dark mt-2" style="font-size: 0.75rem;">Escrow Funded</small>
                    </div>
                    <div class="text-center bg-white px-2">
                        <span id="node3" runat="server" class="rounded-circle d-inline-flex align-items-center justify-content-center text-white" style="width: 22px; height: 22px; background-color: #0b6028; font-size: 0.65rem;">✓</span>
                        <small class="d-block fw-bold text-dark mt-2" style="font-size: 0.75rem;">Shipped</small>
                    </div>
                    <div class="text-center bg-white px-2">
                        <span id="node4" runat="server" class="rounded-circle d-inline-flex align-items-center justify-content-center text-white" style="width: 22px; height: 22px; background-color: #0b6028; font-size: 0.65rem;">✓</span>
                        <small class="d-block fw-bold text-dark mt-2" style="font-size: 0.75rem;">Out for Delivery</small>
                    </div>
                    <div class="text-center bg-white px-2">
                        <span id="node5" runat="server" class="rounded-circle d-inline-flex align-items-center justify-content-center border" style="width: 22px; height: 22px; background-color: #ffffff; border-color: #cbd5e1 !important; font-size: 0.65rem;">•</span>
                        <small class="d-block text-muted mt-2" style="font-size: 0.75rem;">Verified Arrival</small>
                    </div>
                </div>
            </div>
        </div>

        <div class="row g-4">
            <!-- LEFT: Product & Escrow Financial Summary -->
            <div class="col-lg-7">
                <div class="card card-custom p-4 h-100 d-flex flex-column border shadow-sm">
                    <div class="d-flex gap-3 align-items-center mb-4">
                        <asp:Image ID="imgProduct" runat="server" ImageUrl="Images/groundnut_harvest.jpg" CssClass="rounded-3 object-fit-cover flex-shrink-0" Width="160" Height="110" />
                        <div>
                            <h5 class="fw-bold mb-1 text-dark" style="font-size: 1.25rem;">
                                <asp:Literal ID="litProductName" runat="server">Premium Groundnut</asp:Literal>
                            </h5>
                            <small class="text-muted d-block mb-2" style="font-size: 0.88rem;">
                                Farmer: <asp:Literal ID="litFarmerInfo" runat="server">Ramesh Patel | Rajkot</asp:Literal>
                            </small>
                            <p class="mb-0 text-dark fw-bold" style="color: #0b6028 !important; font-size: 0.95rem;">
                                Quantity: <asp:Literal ID="litQuantity" runat="server">15 Quintal</asp:Literal>
                            </p>
                            <p class="mb-0 text-muted small">
                                Order Date: <asp:Literal ID="litOrderDate" runat="server"></asp:Literal>
                            </p>
                        </div>
                    </div>

                    <div class="border-top pt-3 mt-auto">
                        <div class="d-flex justify-content-between py-2 text-dark" style="font-size: 0.95rem;">
                            <span>Subtotal</span>
                            <span class="fw-bold">₹<asp:Literal ID="litSubtotal" runat="server">88,500</asp:Literal></span>
                        </div>
                        <div class="d-flex justify-content-between py-2 text-dark" style="font-size: 0.95rem;">
                            <span>Escrow & Logistics Fee</span>
                            <span class="fw-bold">₹<asp:Literal ID="litLogisticsFee" runat="server">0</asp:Literal></span>
                        </div>
                        <div class="d-flex justify-content-between py-2 border-top mt-2" style="font-size: 1.2rem;">
                            <span class="fw-bold text-dark">Total Paid (in Escrow)</span>
                            <span class="fw-bold" style="color: #0b6028;">₹<asp:Literal ID="litTotalPaid" runat="server">88,500</asp:Literal></span>
                        </div>
                    </div>
                </div>
            </div>

            <!-- RIGHT: Logistics & Delivery Address -->
            <div class="col-lg-5">
                <div class="card card-custom p-4 h-100 d-flex flex-column border shadow-sm">
                    <div class="d-flex justify-content-between align-items-center mb-4">
                        <h5 class="fw-bold mb-0 text-dark">Logistics & Delivery</h5>
                        <asp:Literal ID="litStatusBadge" runat="server"></asp:Literal>
                    </div>

                    <div class="mb-3">
                        <small class="text-muted fw-bold d-block" style="font-size: 0.72rem; letter-spacing: 0.5px;">BUYER IDENTITY</small>
                        <p class="mb-0 text-dark fw-bold" style="font-size: 0.95rem;">
                            <asp:Literal ID="litBuyerDetails" runat="server">Verified Buyer</asp:Literal>
                        </p>
                    </div>

                    <div class="mb-4">
                        <small class="text-muted fw-bold d-block" style="font-size: 0.72rem; letter-spacing: 0.5px;">DELIVERY HUB LOCATION</small>
                        <p class="mb-0 text-dark fw-bold" style="font-size: 0.95rem; line-height: 1.4;">
                            <asp:Literal ID="litDeliveryAddress" runat="server">Buyer Verified Address, Gujarat</asp:Literal>
                        </p>
                    </div>

                    <div class="d-flex flex-column gap-2 mt-auto">
                        <a href="Contact.aspx" class="btn btn-kr-primary w-100 py-2 fw-bold text-center" style="background-color: #0b6028 !important; border-color: #0b6028 !important;">Contact Support / Farmer</a>
                        <a href="MyPurchases.aspx" class="btn w-100 py-2 fw-bold text-center btn-outline-secondary">Back to My Purchases</a>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
