<%@ Page Title="My Purchases" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="MyPurchases.aspx.cs" Inherits="KhedutRise.MyPurchases" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- My Purchases -->
    <div class="kr-page-container py-4">
        <!--  Header -->
        <div class="mb-4">
            <small class="text-muted"><a href="Default.aspx" class="text-decoration-none text-muted">Home</a> / <a href="Marketplace.aspx" class="text-decoration-none text-muted">Marketplace</a> / My Purchases</small>
            <h2 class="fw-bold mt-1 text-dark" style="font-size: 2rem;">My Purchases</h2>
            <p class="text-muted small mb-0">Manage and track your agricultural crop direct purchases from verified farmers.</p>
        </div>

        <!-- Verified Escrow Protection System Banner  -->
        <div class="card card-custom p-4 p-md-5 mb-4 text-white overflow-hidden position-relative shadow-sm" style="background: linear-gradient(rgba(0,0,0,0.55), rgba(0,0,0,0.7)), url('Images/apmc_yard.jpg') center/cover no-repeat; min-height: 160px; border-radius: 12px;">
            <div class="my-auto">
                <h3 class="fw-bold mb-2 text-white" style="font-size: 1.75rem;">Verified Escrow Protection System</h3>
                <p class="text-white-50 mb-0" style="font-size: 0.95rem;">Payments are held securely in Escrow until you confirm crop physical arrival and quality inspection.</p>
            </div>
        </div>

        <!-- Empty State Panel -->
        <asp:Panel ID="pnlNoPurchases" runat="server" Visible="false" CssClass="card p-5 text-center bg-white border shadow-sm mb-4">
            <i class="bi bi-cart-x text-muted display-4 mb-3"></i>
            <h5 class="fw-bold text-dark">No Crop Purchases Found</h5>
            <p class="text-muted small mb-3">You haven't placed any harvest crop orders yet.</p>
            <div>
                <a href="Marketplace.aspx" class="btn btn-kr-primary px-4 py-2 fw-bold" style="background-color: #0b6028 !important; border-color: #0b6028 !important; border-radius: 6px;">Browse Agricultural Marketplace</a>
            </div>
        </asp:Panel>

        <!-- Purchases Table -->
        <div class="card card-custom p-4 border shadow-sm" id="divPurchasesTable" runat="server">
            <h5 class="fw-bold mb-3 text-dark">Harvest Orders & Escrow Ledger</h5>
            <div class="table-responsive">
                <asp:Repeater ID="rptPurchases" runat="server">
                    <HeaderTemplate>
                        <table class="table table-hover align-middle mb-0">
                            <thead class="text-muted" style="font-size: 0.75rem; letter-spacing: 0.5px;">
                                <tr>
                                    <th>ORDER ID</th>
                                    <th>COMMODITY</th>
                                    <th>FARMER NAME</th>
                                    <th>QUANTITY</th>
                                    <th>TOTAL PRICE</th>
                                    <th>ORDER DATE</th>
                                    <th>ESCROW STATUS</th>
                                    <th>ACTION</th>
                                </tr>
                            </thead>
                            <tbody style="font-size: 0.92rem;">
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td><strong class="text-dark">#<%# Eval("OrderId") %></strong></td>
                            <td><%# Eval("Commodity") %></td>
                            <td><%# Eval("FarmerName") %></td>
                            <td><%# Eval("Quantity") %></td>
                            <td><strong style="color: #16a34a;">₹<%# string.Format("{0:N0}", Eval("TotalPrice")) %></strong></td>
                            <td><%# Convert.ToDateTime(Eval("OrderDate")).ToString("dd MMM yyyy") %></td>
                            <td>
                                <span class='badge <%# Eval("Status").ToString() == "Delivered" ? "bg-success-subtle text-success border border-success-subtle" : (Eval("Status").ToString() == "In Transit" ? "bg-primary-subtle text-primary border border-primary-subtle" : "bg-warning-subtle text-warning border border-warning-subtle") %> fw-bold'>
                                    <%# Eval("Status") %>
                                </span>
                            </td>
                            <td><a href='OrderDetails.aspx?orderId=<%# Eval("OrderId") %>' class="fw-bold text-decoration-none" style="color: #0284c7;">Track Order</a></td>
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
