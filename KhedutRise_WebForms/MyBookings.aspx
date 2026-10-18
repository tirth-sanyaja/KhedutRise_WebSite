<%@ Page Title="My Bookings & Service History" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="MyBookings.aspx.cs" Inherits="KhedutRise.MyBookings" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- My Bookings -->
    <div class="kr-page-container py-4">
        <!-- Header -->
        <div class="mb-4">
            <small class="text-muted"><a href="Default.aspx" class="text-decoration-none text-muted">Home</a> / <a href="TractorRentals.aspx" class="text-decoration-none text-muted">Tractor Rentals</a> / My Bookings</small>
            <h2 class="fw-bold mt-1 text-dark" style="font-size: 2rem;">My Bookings & Service History</h2>
            <p class="text-muted small mb-0">Track your verified agricultural machinery reservations and payment records.</p>
        </div>

        <!-- 2 Top Dynamic KPI Cards -->
        <div class="row g-3 mb-4">
            <div class="col-6 col-md-3">
                <div class="card card-custom p-3 text-start h-100 border shadow-sm">
                    <small class="text-muted fw-bold" style="font-size: 0.72rem; letter-spacing: 0.5px;">ACTIVE RENTALS</small>
                    <h4 class="fw-bold mt-2 mb-0" style="color: #0b6028; font-size: 1.4rem;">
                        <asp:Literal ID="litUpcomingCount" runat="server">0 Scheduled</asp:Literal>
                    </h4>
                </div>
            </div>
            <div class="col-6 col-md-3">
                <div class="card card-custom p-3 text-start h-100 border shadow-sm">
                    <small class="text-muted fw-bold" style="font-size: 0.72rem; letter-spacing: 0.5px;">TOTAL SPENT ON SERVICES</small>
                    <h4 class="fw-bold text-dark mt-2 mb-0" style="font-size: 1.4rem;">
                        <asp:Literal ID="litTotalSpent" runat="server">₹0</asp:Literal>
                    </h4>
                </div>
            </div>
        </div>

        <!-- Dynamic Active Scheduled Booking Card -->
        <asp:Panel ID="pnlActiveBooking" runat="server" Visible="false" CssClass="card p-3 mb-4 bg-white border shadow-sm" style="border-left: 5px solid #0b6028 !important; border-radius: 8px;">
            <div class="d-flex flex-wrap gap-3 justify-content-between align-items-center">
                <div class="d-flex gap-3 align-items-center">
                    <asp:Image ID="imgActiveBooking" runat="server" ImageUrl="Images/massey_tractor.jpg" CssClass="rounded-3 object-fit-cover flex-shrink-0" Width="140" Height="95" />
                    <div>
                        <h5 class="fw-bold mb-1 text-dark" style="font-size: 1.2rem;">
                            <asp:Literal ID="litActiveTitle" runat="server">Machinery Booking</asp:Literal>
                        </h5>
                        <small class="text-muted d-block mb-1" style="font-size: 0.85rem;">
                            <asp:Literal ID="litActiveDetails" runat="server"></asp:Literal>
                        </small>
                        <strong style="color: #0b6028; font-size: 0.92rem;">
                            <asp:Literal ID="litActiveStatus" runat="server">Status: Confirmed</asp:Literal>
                        </strong>
                    </div>
                </div>
                <a href="Contact.aspx" class="btn btn-kr-primary px-4 py-2 fw-bold text-nowrap" style="background-color: #0b6028 !important; border-color: #0b6028 !important; border-radius: 6px;">Contact Provider</a>
            </div>
        </asp:Panel>

        <!-- Empty State Panel -->
        <asp:Panel ID="pnlNoBookings" runat="server" Visible="false" CssClass="card p-5 text-center bg-white border shadow-sm mb-4">
            <i class="bi bi-calendar-x text-muted display-4 mb-3"></i>
            <h5 class="fw-bold text-dark">No Equipment Bookings Found</h5>
            <p class="text-muted small mb-3">You have not reserved any tractors or agricultural machinery yet.</p>
            <div>
                <a href="TractorRentals.aspx" class="btn btn-kr-primary px-4 py-2 fw-bold" style="background-color: #0b6028 !important; border-color: #0b6028 !important; border-radius: 6px;">Browse Available Machinery</a>
            </div>
        </asp:Panel>

        <!-- Historical Logs Table -->
        <div class="card card-custom p-4 border shadow-sm" id="divBookingsTable" runat="server">
            <h5 class="fw-bold mb-3 text-dark">Historical Reservation Logs</h5>
            <div class="table-responsive">
                <asp:Repeater ID="rptBookings" runat="server">
                    <HeaderTemplate>
                        <table class="table table-hover align-middle mb-0">
                            <thead class="text-muted" style="font-size: 0.72rem; letter-spacing: 0.5px;">
                                <tr>
                                    <th>BOOKING ID</th>
                                    <th>SERVICE TYPE</th>
                                    <th>PROVIDER</th>
                                    <th>DATE</th>
                                    <th>AMOUNT</th>
                                    <th>STATUS</th>
                                </tr>
                            </thead>
                            <tbody style="font-size: 0.92rem;">
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td><strong class="text-dark"><%# Eval("BookingId") %></strong></td>
                            <td><%# Eval("ServiceType") %></td>
                            <td><%# Eval("ProviderName") %></td>
                            <td><%# Convert.ToDateTime(Eval("BookingDate")).ToString("dd MMM yyyy") %></td>
                            <td><strong class="text-dark">₹<%# string.Format("{0:N0}", Eval("Amount")) %></strong></td>
                            <td><span class='badge <%# Eval("Status").ToString() == "Confirmed" ? "bg-success-subtle text-success border border-success-subtle" : "bg-warning-subtle text-warning border border-warning-subtle" %> fw-bold'><%# Eval("Status") %></span></td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                            </tbody>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
            </div>
        </div>
    </div>
</asp:Content>
