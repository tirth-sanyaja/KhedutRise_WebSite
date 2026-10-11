<%@ Page Title="My Purchases" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="MyPurchases.aspx.cs" Inherits="KhedutRise.MyPurchases" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- My Purchases -->
    <div class="kr-page-container">
        <!--  Header -->
        <div class="mb-4">
            <small class="text-muted"><a href="Default.aspx" class="text-decoration-none text-muted">Home</a> / Marketplace / My Purchases</small>
            <h2 class="fw-bold mt-1 text-dark" style="font-size: 2rem;">My Purchases</h2>
            <p class="text-muted small mb-0">Manage and track your agricultural direct purchases.</p>
        </div>

        <!-- Verified Escrow Protection System Banner  -->
        <div class="card card-custom p-4 p-md-5 mb-4 text-white overflow-hidden position-relative" style="background: linear-gradient(rgba(0,0,0,0.55), rgba(0,0,0,0.7)), url('Images/apmc_yard.jpg') center/cover no-repeat; min-height: 180px; border-radius: 12px;">
            <div class="my-auto">
                <h3 class="fw-bold mb-2 text-white" style="font-size: 1.75rem;">Verified Escrow Protection System</h3>
                <p class="text-white-50 mb-0" style="font-size: 0.95rem;">Payments are held securely until you confirm the crop grade arrival.</p>
            </div>
        </div>

        <!-- Purchases Table -->
        <div class="card card-custom p-4">
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
                                <td><strong class="text-dark"><%# Eval("OrderId") %></strong></td>
                                <td><%# Eval("Commodity") %></td>
                                <td><%# Eval("FarmerName") %></td>
                                <td><%# Eval("Quantity") %></td>
                                <td><strong style="color: #16a34a;">₹<%# string.Format("{0:N0}", Eval("TotalPrice")) %></strong></td>
                                <td><%# Convert.ToDateTime(Eval("OrderDate")).ToString("dd MMM yyyy") %></td>
                                <td><strong style="color: <%# Eval("Status").ToString() == "Delivered" ? "#16a34a" : "#ea580c" %>;"><%# Eval("Status") %></strong></td>
                                <td><a href="OrderDetails.aspx" class="fw-bold text-decoration-none" style="color: #0284c7;">Track Order</a></td>
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
