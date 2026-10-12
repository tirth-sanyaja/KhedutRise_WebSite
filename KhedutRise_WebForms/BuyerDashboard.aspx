<%@ Page Title="Buyer Dashboard" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="BuyerDashboard.aspx.cs" Inherits="KhedutRise.BuyerDashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Buyer Dashboard-->
    <div class="kr-page-container">
        <!-- Header -->
        <div class="mb-4">
            <small class="text-muted"><a href="Default.aspx" class="text-decoration-none text-muted">Home</a> / Buyer Dashboard</small>
            <h2 class="fw-bold mt-1 text-dark" style="font-size: 2rem;">Good Morning, Dev Patel</h2>
            <p class="text-muted small mb-0">Manage your purchases, direct orders and escrow payments.</p>
        </div>

        <!-- 4 Top KPI Cards -->
        <div class="row g-3 mb-4">
            <div class="col-6 col-md-3">
                <div class="card card-custom p-3 text-center h-100">
                    <small class="text-muted fw-bold" style="font-size: 0.72rem; letter-spacing: 0.5px;">ACTIVE ESCROW ORDERS</small>
                    <h4 class="fw-bold mt-2 mb-0" style="color: #16a34a; font-size: 1.35rem;">4 Processing</h4>
                </div>
            </div>
            <div class="col-6 col-md-3">
                <div class="card card-custom p-3 text-center h-100">
                    <small class="text-muted fw-bold" style="font-size: 0.72rem; letter-spacing: 0.5px;">TOTAL DIRECT PURCHASES</small>
                    <h4 class="fw-bold text-dark mt-2 mb-0" style="font-size: 1.35rem;">₹1,85,600</h4>
                </div>
            </div>
            <div class="col-6 col-md-3">
                <div class="card card-custom p-3 text-center h-100">
                    <small class="text-muted fw-bold" style="font-size: 0.72rem; letter-spacing: 0.5px;">FAVORITE FARMERS</small>
                    <h4 class="fw-bold mt-2 mb-0" style="color: #0284c7; font-size: 1.35rem;">8 Verified</h4>
                </div>
            </div>
            <div class="col-6 col-md-3">
                <div class="card card-custom p-3 text-center h-100">
                    <small class="text-muted fw-bold" style="font-size: 0.72rem; letter-spacing: 0.5px;">WISHLIST STOCK ITEMS</small>
                    <h4 class="fw-bold text-dark mt-2 mb-0" style="font-size: 1.35rem;">14 Commodities</h4>
                </div>
            </div>
        </div>

        <div class="row g-4">
            <!-- LEFT: Recent Orders Overview -->
            <div class="col-lg-7">
                <div class="card card-custom p-4 h-100">
                    <h5 class="fw-bold mb-3 text-dark">Recent Orders Overview</h5>
                    <div class="table-responsive">
                        <asp:Repeater ID="rptRecentOrders" runat="server">
                            <HeaderTemplate>
                                <table class="table table-hover align-middle mb-0">
                                    <thead class="text-muted" style="font-size: 0.75rem; letter-spacing: 0.5px;">
                                        <tr>
                                            <th>ORDER ID</th>
                                            <th>CROP</th>
                                            <th>FARMER</th>
                                            <th>QUANTITY</th>
                                            <th>STATUS</th>
                                        </tr>
                                    </thead>
                                    <tbody style="font-size: 0.9rem;">
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td><strong class="text-dark"><%# Eval("OrderId") %></strong></td>
                                    <td><%# Eval("Commodity") %></td>
                                    <td><%# Eval("FarmerName") %></td>
                                    <td><%# Eval("Quantity") %></td>
                                    <td>
                                        <strong style="color: <%# Eval("Status").ToString() == "Delivered" ? "#16a34a" : (Eval("Status").ToString() == "In Transit" ? "#ea580c" : (Eval("Status").ToString() == "Processing" ? "#0284c7" : "#dc2626")) %>;">
                                            <%# Eval("Status") %>
                                        </strong>
                                    </td>
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

            <!-- RIGHT: Monthly Purchase Analytics -->
            <div class="col-lg-5">
                <div class="card card-custom p-4 h-100 d-flex flex-column">
                    <h5 class="fw-bold mb-1 text-dark">Monthly Purchase Analytics</h5>
                    <p class="text-muted small mb-3">Volume distribution and budget utilization graphs.</p>
                    <img src="Images/agri_finance.jpg" class="rounded-3 mb-3 img-fluid object-fit-cover" style="height: 160px; width: 100%;" alt="Analytics" />
                    <ul class="list-unstyled small mb-0 d-flex flex-column gap-2" style="font-size: 0.9rem;">
                        <li class="d-flex align-items-center">
                            <span class="text-dark">• Budget Spent (This Month):&nbsp;</span>
                            <strong style="color: #16a34a;">₹84,500</strong>
                        </li>
                        <li class="d-flex align-items-center">
                            <span class="text-dark">• Escrow Locked Funds:&nbsp;</span>
                            <strong style="color: #ea580c;">₹55,000</strong>
                        </li>
                        <li class="d-flex align-items-center">
                            <span class="text-dark">• Top Sourced Region:&nbsp;</span>
                            <strong style="color: #0284c7;">Rajkot Bypass</strong>
                        </li>
                        <li class="d-flex align-items-center">
                            <span class="text-dark">• Quality Verification Rating:&nbsp;</span>
                            <strong style="color: #eab308;">4.9 / 5.0 </strong>
                        </li>
                    </ul>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
