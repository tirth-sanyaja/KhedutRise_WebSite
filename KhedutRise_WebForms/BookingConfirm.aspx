<%@ Page Title="Booking Confirmation" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="BookingConfirm.aspx.cs" Inherits="KhedutRise.BookingConfirm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Booking Confirmation -->
    <div class="kr-page-container">
        <!--Header -->
        <div class="mb-4">
            <small class="text-muted"><a href="Default.aspx" class="text-decoration-none text-muted">Home</a> / <a href="TractorRentals.aspx" class="text-decoration-none text-muted">Tractor Rentals</a> / Booking Confirmation</small>
            <h2 class="fw-bold mt-1 text-dark" style="font-size: 2rem;">Machinery Booking Confirmation</h2>
            <p class="text-muted small mb-0">Confirm your schedule, field address, and payment method.</p>
        </div>

        <asp:Label ID="lblBookingMsg" runat="server" CssClass="d-block mb-3 fw-bold"></asp:Label>

        <div class="row g-4">
            <!-- LEFT: Tractor Booking Item -->
            <div class="col-lg-5">
                <div class="card card-custom p-4 h-100 d-flex flex-column">
                    <img src="Images/massey_tractor.jpg" class="rounded-3 mb-3 img-fluid object-fit-cover" style="height: 250px; width: 100%;" alt="Massey Ferguson" />
                    <span class="badge bg-success-subtle text-success border border-success-subtle align-self-start mb-2 px-3 py-1">Verified Machinery</span>
                    <h5 class="fw-bold mb-1 text-dark" style="font-size: 1.25rem;">Massey Ferguson 241DI Tractor</h5>
                    <small class="text-muted mb-3 d-block" style="font-size: 0.88rem;">Owner: Dilipbhai Parmar | Location: Rajkot Bypass | 41 HP</small>
                    <h3 class="fw-bold mb-0 mt-auto" style="color: #0b6028; font-size: 1.8rem;">₹800 / Hour</h3>
                </div>
            </div>

            <!-- RIGHT: Schedule & Details Form -->
            <div class="col-lg-7">
                <div class="card card-custom p-4 p-md-5 h-100 d-flex flex-column">
                    <h5 class="fw-bold mb-3" style="color: #0b6028;">Schedule & Field Details</h5>

                    <div class="row g-3 mb-3">
                        <div class="col-md-6">
                            <label class="form-label" style="color: #0b6028;">BOOKING DATE <span class="required-mark">*</span></label>
                            <asp:TextBox ID="txtBookingDate" runat="server" TextMode="Date" CssClass="form-control" 
                                required="required"></asp:TextBox>
                        </div>
                        <div class="col-md-6">
                            <label class="form-label" style="color: #0b6028;">DURATION (HOURS) <span class="required-mark">*</span></label>
                            <asp:TextBox ID="txtDuration" runat="server" type="number" min="1" max="48" CssClass="form-control" Text="6" 
                                placeholder="e.g. 6" required="required"></asp:TextBox>
                        </div>
                    </div>

                    <div class="mb-3">
                        <label class="form-label" style="color: #0b6028;">FARM DELIVERY ADDRESS <span class="required-mark">*</span></label>
                        <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" 
                            placeholder="e.g. Plot A, Rajkot Bypass Road, Rajkot" 
                            required="required"></asp:TextBox>
                    </div>

                    <div class="mb-4">
                        <label class="form-label" style="color: #0b6028;">PAYMENT METHOD <span class="required-mark">*</span></label>
                        <asp:DropDownList ID="ddlPaymentMode" runat="server" CssClass="form-select" required="required">
                            <asp:ListItem Text="Cash on Field Delivery (Pay to Operator)" Value="Cash on Delivery" Selected="True" />
                            <asp:ListItem Text="UPI Payment / QR Code" Value="UPI" />
                            <asp:ListItem Text="KhedutRise Escrow Protection" Value="Escrow" />
                        </asp:DropDownList>
                    </div>

                    <small class="text-muted fw-bold d-block mb-2" style="font-size: 0.72rem; letter-spacing: 0.5px;">ESTIMATED PRICING</small>
                    <div class="border-top pt-2 mb-4">
                        <div class="d-flex justify-content-between py-1 text-dark" style="font-size: 0.95rem;">
                            <span>Base Rental (₹800/hr)</span>
                            <span class="fw-bold">₹4,800</span>
                        </div>
                        <div class="d-flex justify-content-between py-1 text-dark" style="font-size: 0.95rem;">
                            <span>Machinery Transport Charge</span>
                            <span class="fw-bold">₹500</span>
                        </div>
                        <div class="d-flex justify-content-between py-2 border-top mt-2" style="font-size: 1.25rem;">
                            <span class="fw-bold text-dark">Estimated Total</span>
                            <span class="fw-bold" style="color: #0b6028;">₹5,300</span>
                        </div>
                    </div>

                    <!-- Action Buttons -->
                    <div class="d-flex gap-3 mt-auto">
                        <a href="TractorRentals.aspx" class="btn w-50 py-2 fw-semibold btn-outline-secondary">Cancel</a>
                        <asp:Button ID="btnConfirmBooking" runat="server" Text="Confirm Booking" 
                            CssClass="btn btn-kr-primary w-50 py-2 fw-bold" 
                            style="background-color: #0b6028 !important; border-color: #0b6028 !important;" 
                            OnClick="btnConfirmBooking_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
