<%@ Page Title="My Bookings & Service History" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="MyBookings.aspx.cs" Inherits="KhedutRise.MyBookings" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- My Bookings -->
    <div class="kr-page-container">
        <!-- Header -->
        <div class="mb-4">
            <small class="text-muted"><a href="Default.aspx" class="text-decoration-none text-muted">Home</a> / <a href="AgriServices.aspx" class="text-decoration-none text-muted">Agri Services</a> / My Bookings</small>
            <h2 class="fw-bold mt-1 text-dark" style="font-size: 2rem;">My Bookings & Service History</h2>
        </div>

        <!-- 2 Top KPI Cards -->
        <div class="row g-3 mb-4">
            <div class="col-6 col-md-3">
                <div class="card card-custom p-3 text-start h-100">
                    <small class="text-muted fw-bold" style="font-size: 0.72rem; letter-spacing: 0.5px;">UPCOMING RENTALS</small>
                    <h4 class="fw-bold mt-2 mb-0" style="color: #0b6028; font-size: 1.4rem;">1 Scheduled</h4>
                </div>
            </div>
            <div class="col-6 col-md-3">
                <div class="card card-custom p-3 text-start h-100">
                    <small class="text-muted fw-bold" style="font-size: 0.72rem; letter-spacing: 0.5px;">TOTAL SPENT ON SERVICES</small>
                    <h4 class="fw-bold text-dark mt-2 mb-0" style="font-size: 1.4rem;">₹18,200</h4>
                </div>
            </div>
        </div>

        <!-- Active Scheduled Booking Card-->
        <div class="card p-3 mb-4 bg-white" style="border: 2px solid #0b6028; border-radius: 8px;">
            <div class="d-flex flex-wrap gap-3 justify-content-between align-items-center">
                <div class="d-flex gap-3 align-items-center">
                    <img src="Images/massey_tractor.jpg" class="rounded-3 object-fit-cover flex-shrink-0" width="140" height="95" alt="Tractor" />
                    <div>
                        <h5 class="fw-bold mb-1 text-dark" style="font-size: 1.2rem;">Massey Ferguson 241DI Tractor Booking</h5>
                        <small class="text-muted d-block mb-1" style="font-size: 0.85rem;">Scheduled Date: 14 Aug 2026 | Time: 10:00 AM | Duration: 6 Hours</small>
                        <strong style="color: #0b6028; font-size: 0.92rem;">Status: Confirmed | Escrow Hold: ₹5,300</strong>
                    </div>
                </div>
                <a href="Contact.aspx" class="btn btn-kr-primary px-4 py-2 fw-bold text-nowrap" style="background-color: #0b6028 !important; border-color: #0b6028 !important; border-radius: 6px;">Contact Owner</a>
            </div>
        </div>

        <!-- Historical Logs Table -->
        <div class="card card-custom p-4">
            <h5 class="fw-bold mb-3 text-dark">Historical Logs</h5>
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
                            <td><strong style="color: <%# Eval("Status").ToString() == "Completed" ? "#0b6028" : "#ea580c" %>;"><%# Eval("Status") %></strong></td>
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
