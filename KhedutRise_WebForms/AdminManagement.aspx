<%@ Page Title="Platform Management" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="AdminManagement.aspx.cs" Inherits="KhedutRise.AdminManagement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Platform Management & Moderation: Approval/Rejection Actions -->
    <div class="kr-page-container">
        <!--Header -->
        <div class="mb-4">
            <small class="text-muted"><a href="Default.aspx" class="text-decoration-none text-muted">Home</a> / <a href="AdminDashboard.aspx" class="text-decoration-none text-muted">Admin Portal</a> / <asp:Literal ID="litBreadcrumb" runat="server">Platform Management</asp:Literal></small>
            <h2 class="fw-bold mt-1 text-dark" style="font-size: 2rem;">
                <asp:Literal ID="litPageTitle" runat="server">Platform Moderation & Management</asp:Literal>
            </h2>
        </div>

        <div class="row g-4">
            <!-- LEFT: Dark Navy Navigation Sidebar-->
            <div class="col-lg-3">
                <div class="p-4 rounded-3 text-white h-100" style="background-color: #0f172a;">
                    <ul class="list-unstyled mb-0 d-flex flex-column gap-3 py-2" style="font-size: 0.95rem;">
                        <li>
                            <a href="AdminDashboard.aspx" class="text-decoration-none text-white d-block opacity-90">
                                <i class="bi bi-speedometer2 me-2"></i>Dashboard
                            </a>
                        </li>
                        <li>
                            <asp:LinkButton ID="lnkNavFarmers" runat="server" OnClick="NavSidebar_Click" CommandArgument="Farmers" CssClass="text-decoration-none d-block">
                                <i class="bi bi-people me-2"></i>Farmers List
                            </asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton ID="lnkNavBuyers" runat="server" OnClick="NavSidebar_Click" CommandArgument="Buyers" CssClass="text-decoration-none d-block">
                                <i class="bi bi-shop me-2"></i>Buyers List
                            </asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton ID="lnkNavModeration" runat="server" OnClick="NavSidebar_Click" CommandArgument="Moderation" CssClass="text-decoration-none d-block">
                                <i class="bi bi-shield-check me-2"></i>Moderation
                            </asp:LinkButton>
                        </li>
                        <li>
                            <a href="AdminAnalytics.aspx" class="text-decoration-none text-white d-block opacity-90">
                                <i class="bi bi-graph-up me-2"></i>Analytics
                            </a>
                        </li>
                        <li>
                            <a href="AdminSettings.aspx" class="text-decoration-none text-white d-block opacity-90">
                                <i class="bi bi-gear me-2"></i>Settings
                            </a>
                        </li>
                    </ul>
                </div>
            </div>

            <!-- RIGHT: Category Filters & Moderation Tables -->
            <div class="col-lg-9">
                <!-- Status KPI summary cards -->
                <div class="row g-3 mb-4">
                    <div class="col-6 col-md-3">
                        <div class="card card-custom p-3 text-start h-100 border-start border-4 border-primary">
                            <small class="text-muted fw-bold" style="font-size: 0.72rem; letter-spacing: 0.5px;">TOTAL ITEMS</small>
                            <h4 class="fw-bold mt-2 mb-0 text-dark" style="font-size: 1.4rem;">
                                <asp:Label ID="lblTotalCount" runat="server" Text="0"></asp:Label>
                            </h4>
                        </div>
                    </div>
                    <div class="col-6 col-md-3">
                        <div class="card card-custom p-3 text-start h-100 border-start border-4 border-success">
                            <small class="text-muted fw-bold" style="font-size: 0.72rem; letter-spacing: 0.5px;">APPROVED</small>
                            <h4 class="fw-bold mt-2 mb-0" style="color: #16a34a; font-size: 1.4rem;">
                                <asp:Label ID="lblApprovedCount" runat="server" Text="0"></asp:Label>
                            </h4>
                        </div>
                    </div>
                    <div class="col-6 col-md-3">
                        <div class="card card-custom p-3 text-start h-100 border-start border-4 border-warning">
                            <small class="text-muted fw-bold" style="font-size: 0.72rem; letter-spacing: 0.5px;">PENDING</small>
                            <h4 class="fw-bold mt-2 mb-0" style="color: #d97706; font-size: 1.4rem;">
                                <asp:Label ID="lblPendingCount" runat="server" Text="0"></asp:Label>
                            </h4>
                        </div>
                    </div>
                    <div class="col-6 col-md-3">
                        <div class="card card-custom p-3 text-start h-100 border-start border-4 border-danger">
                            <small class="text-muted fw-bold" style="font-size: 0.72rem; letter-spacing: 0.5px;">REJECTED</small>
                            <h4 class="fw-bold mt-2 mb-0 text-danger" style="font-size: 1.4rem;">
                                <asp:Label ID="lblRejectedCount" runat="server" Text="0"></asp:Label>
                            </h4>
                        </div>
                    </div>
                </div>

                <!-- Action Feedback Alert -->
                <asp:Panel ID="pnlAlert" runat="server" Visible="false" CssClass="alert alert-success alert-dismissible fade show p-3 mb-4 rounded-3 shadow-sm" role="alert">
                    <div class="d-flex align-items-center">
                        <i class="bi bi-info-circle-fill fs-5 me-2"></i>
                        <div>
                            <asp:Label ID="lblAlertMsg" runat="server" CssClass="fw-bold"></asp:Label>
                        </div>
                    </div>
                    <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
                </asp:Panel>

                <!-- Filter Category Pills-->
                <div class="d-flex flex-wrap align-items-center justify-content-between gap-2 mb-4">
                    <div class="d-flex flex-wrap gap-2">
                        <asp:LinkButton ID="btnFarmers" runat="server" CssClass="btn btn-sm rounded-pill px-4 fw-bold text-white shadow-none" style="background-color: #0b6028; border: 1px solid #0b6028;" OnClick="TabFilter_Click" CommandArgument="Farmer">Farmers</asp:LinkButton>
                        <asp:LinkButton ID="btnBuyers" runat="server" CssClass="btn btn-sm rounded-pill px-4 fw-semibold shadow-none bg-white text-dark" style="border: 1px solid #d1d5db;" OnClick="TabFilter_Click" CommandArgument="Buyer">Buyers</asp:LinkButton>
                        <asp:LinkButton ID="btnModeration" runat="server" CssClass="btn btn-sm rounded-pill px-4 fw-semibold shadow-none bg-white text-dark" style="border: 1px solid #d1d5db;" OnClick="TabFilter_Click" CommandArgument="Moderation">All Moderation</asp:LinkButton>
                        <asp:LinkButton ID="btnProducts" runat="server" CssClass="btn btn-sm rounded-pill px-4 fw-semibold shadow-none bg-white text-dark" style="border: 1px solid #d1d5db;" OnClick="TabFilter_Click" CommandArgument="Products">Crops List</asp:LinkButton>
                        <asp:LinkButton ID="btnLandListings" runat="server" CssClass="btn btn-sm rounded-pill px-4 fw-semibold shadow-none bg-white text-dark" style="border: 1px solid #d1d5db;" OnClick="TabFilter_Click" CommandArgument="LandListings">Land Listings</asp:LinkButton>
                    </div>

                    <!-- Status Filter Toggle Buttons -->
                    <div class="btn-group btn-group-sm" role="group">
                        <asp:LinkButton ID="btnStatusAll" runat="server" CssClass="btn btn-outline-secondary" OnClick="StatusFilter_Click" CommandArgument="ALL">All</asp:LinkButton>
                        <asp:LinkButton ID="btnStatusApproved" runat="server" CssClass="btn btn-outline-success" OnClick="StatusFilter_Click" CommandArgument="Approved">Approved</asp:LinkButton>
                        <asp:LinkButton ID="btnStatusPending" runat="server" CssClass="btn btn-outline-warning text-dark" OnClick="StatusFilter_Click" CommandArgument="Pending">Pending</asp:LinkButton>
                        <asp:LinkButton ID="btnStatusRejected" runat="server" CssClass="btn btn-outline-danger" OnClick="StatusFilter_Click" CommandArgument="Rejected">Rejected</asp:LinkButton>
                    </div>
                </div>

                <!-- VIEW 1: USERS TABLE (Farmers, Buyers, All Moderation), here user can see their details -->
                <asp:Panel ID="pnlUsersView" runat="server" CssClass="card card-custom p-4 mb-4">
                    <div class="d-flex justify-content-between align-items-center mb-3">
                        <h5 class="fw-bold text-dark mb-0">
                            <asp:Literal ID="litTableHeading" runat="server">Accounts & Moderation List</asp:Literal>
                        </h5>
                        <small class="text-muted">Use buttons below to approve or reject accounts</small>
                    </div>

                    <div class="table-responsive">
                        <asp:Repeater ID="rptModeration" runat="server" OnItemCommand="rptModeration_ItemCommand">
                            <HeaderTemplate>
                                <table class="table table-hover align-middle mb-0">
                                    <thead class="text-muted" style="font-size: 0.72rem; letter-spacing: 0.5px;">
                                        <tr>
                                            <th>USER & CONTACT</th>
                                            <th>ROLE</th>
                                            <th>LOCATION</th>
                                            <th>STATUS</th>
                                            <th>DETAILS</th>
                                            <th class="text-end">APPROVAL ACTIONS</th>
                                        </tr>
                                    </thead>
                                    <tbody style="font-size: 0.92rem;">
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <strong class="text-dark d-block"><%# Eval("FullName") %></strong>
                                        <small class="text-muted"><%# Eval("MobileNo") %></small>
                                    </td>
                                    <td>
                                        <span class="badge bg-light text-dark border"><%# Eval("UserRole") %></span>
                                    </td>
                                    <td><%# Eval("Location") %></td>
                                    <td>
                                        <%# (Eval("Status").ToString() == "Verified" || Eval("Status").ToString() == "Approved")
                                            ? "<span class='badge rounded-pill bg-success-subtle text-success border border-success-subtle px-2 py-1'><i class='bi bi-check-circle-fill me-1'></i> Approved</span>" 
                                            : (Eval("Status").ToString() == "Rejected"
                                                ? "<span class='badge rounded-pill bg-danger-subtle text-danger border border-danger-subtle px-2 py-1'><i class='bi bi-x-circle-fill me-1'></i> Rejected</span>"
                                                : "<span class='badge rounded-pill bg-warning-subtle text-warning-emphasis border border-warning-subtle px-2 py-1'><i class='bi bi-clock-history me-1'></i> Pending</span>") %>
                                    </td>
                                    <td>
                                        <small class="text-muted"><%# Eval("SpecificField") ?? "Registered Account" %></small>
                                    </td>
                                    <td class="text-end">
                                        <div class="d-inline-flex gap-1">
                                            <!-- Approve Button -->
                                            <asp:LinkButton ID="btnApprove" runat="server" CommandName="Approve" CommandArgument='<%# Eval("UserId") %>' 
                                                CssClass="btn btn-sm btn-success text-white py-1 px-2 fw-semibold" 
                                                ToolTip="Approve this account">
                                                <i class="bi bi-check-lg me-1"></i>Approve
                                            </asp:LinkButton>
                                            
                                            <!-- Reject Button -->
                                            <asp:LinkButton ID="btnReject" runat="server" CommandName="Reject" CommandArgument='<%# Eval("UserId") %>' 
                                                CssClass="btn btn-sm btn-danger text-white py-1 px-2 fw-semibold" 
                                                ToolTip="Reject this account">
                                                <i class="bi bi-x-lg me-1"></i>Reject
                                            </asp:LinkButton>

                                            <!-- Delete / Remove Button -->
                                            <asp:LinkButton ID="btnDeactivate" runat="server" CommandName="DeleteUser" CommandArgument='<%# Eval("UserId") %>' 
                                                CssClass="btn btn-sm btn-outline-secondary py-1 px-2" 
                                                OnClientClick="return confirm('Are you sure you want to delete this record?');"
                                                ToolTip="Delete record">
                                                <i class="bi bi-trash"></i>
                                            </asp:LinkButton>
                                        </div>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                    </tbody>
                                </table>
                            </FooterTemplate>
                        </asp:Repeater>
                    </div>
                </asp:Panel>

                <!-- VIEW 2: CROPS / PRODUCTS TABLE -->
                <asp:Panel ID="pnlProductsView" runat="server" Visible="false" CssClass="card card-custom p-4 mb-4">
                    <div class="d-flex justify-content-between align-items-center mb-3">
                        <h5 class="fw-bold text-dark mb-0">Crop Listings Moderation</h5>
                        <small class="text-muted">Approve or reject farmer harvest listings</small>
                    </div>

                    <div class="table-responsive">
                        <asp:Repeater ID="rptProducts" runat="server" OnItemCommand="rptProducts_ItemCommand">
                            <HeaderTemplate>
                                <table class="table table-hover align-middle mb-0">
                                    <thead class="text-muted" style="font-size: 0.72rem; letter-spacing: 0.5px;">
                                        <tr>
                                            <th>CROP & CATEGORY</th>
                                            <th>FARMER NAME</th>
                                            <th>PRICE / Q</th>
                                            <th>STATUS</th>
                                            <th class="text-end">APPROVAL ACTIONS</th>
                                        </tr>
                                    </thead>
                                    <tbody style="font-size: 0.92rem;">
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <strong class="text-dark d-block"><%# Eval("ProductName") %></strong>
                                        <small class="text-muted"><%# Eval("Category") %> | <%# Eval("QuantityInQuintal") %> Quintals</small>
                                    </td>
                                    <td>
                                        <span class="fw-semibold text-dark"><%# Eval("FarmerName") %></span><br />
                                        <small class="text-muted"><%# Eval("Location") %></small>
                                    </td>
                                    <td>
                                        <strong style="color: #0b6028;">₹<%# string.Format("{0:N0}", Eval("PricePerQuintal")) %></strong>
                                    </td>
                                    <td>
                                        <%# Convert.ToBoolean(Eval("IsApproved"))
                                            ? "<span class='badge rounded-pill bg-success-subtle text-success border border-success-subtle px-2 py-1'><i class='bi bi-check-circle-fill me-1'></i> Approved</span>" 
                                            : "<span class='badge rounded-pill bg-danger-subtle text-danger border border-danger-subtle px-2 py-1'><i class='bi bi-x-circle-fill me-1'></i> Rejected</span>" %>
                                    </td>
                                    <td class="text-end">
                                        <div class="d-inline-flex gap-1">
                                            <asp:LinkButton ID="btnApproveProduct" runat="server" CommandName="ApproveProduct" CommandArgument='<%# Eval("ProductId") %>' 
                                                CssClass="btn btn-sm btn-success text-white py-1 px-2 fw-semibold" ToolTip="Approve Crop">
                                                <i class="bi bi-check-lg me-1"></i>Approve
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="btnRejectProduct" runat="server" CommandName="RejectProduct" CommandArgument='<%# Eval("ProductId") %>' 
                                                CssClass="btn btn-sm btn-danger text-white py-1 px-2 fw-semibold" ToolTip="Reject Crop">
                                                <i class="bi bi-x-lg me-1"></i>Reject
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="btnDeleteProduct" runat="server" CommandName="DeleteProduct" CommandArgument='<%# Eval("ProductId") %>' 
                                                CssClass="btn btn-sm btn-outline-secondary py-1 px-2" OnClientClick="return confirm('Are you sure?');">
                                                <i class="bi bi-trash"></i>
                                            </asp:LinkButton>
                                        </div>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                    </tbody>
                                </table>
                            </FooterTemplate>
                        </asp:Repeater>
                    </div>
                </asp:Panel>

                <!-- VIEW 3: LAND LISTINGS TABLE -->
                <asp:Panel ID="pnlLandView" runat="server" Visible="false" CssClass="card card-custom p-4 mb-4">
                    <div class="d-flex justify-content-between align-items-center mb-3">
                        <h5 class="fw-bold text-dark mb-0">Land Listings Moderation</h5>
                        <small class="text-muted">Approve or reject agricultural plots</small>
                    </div>

                    <div class="table-responsive">
                        <asp:Repeater ID="rptLand" runat="server" OnItemCommand="rptLand_ItemCommand">
                            <HeaderTemplate>
                                <table class="table table-hover align-middle mb-0">
                                    <thead class="text-muted" style="font-size: 0.72rem; letter-spacing: 0.5px;">
                                        <tr>
                                            <th>LAND TITLE & LOCATION</th>
                                            <th>AREA (ACRES)</th>
                                            <th>PRICE</th>
                                            <th>SOIL & WATER</th>
                                            <th>STATUS</th>
                                            <th class="text-end">APPROVAL ACTIONS</th>
                                        </tr>
                                    </thead>
                                    <tbody style="font-size: 0.92rem;">
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <strong class="text-dark d-block"><%# Eval("Title") %></strong>
                                        <small class="text-muted"><%# Eval("Location") %></small>
                                    </td>
                                    <td><span class="badge bg-light text-dark border"><%# Eval("AreaAcres") %> Acres</span></td>
                                    <td><strong class="text-dark"><%# Eval("Price") %></strong></td>
                                    <td><small class="text-muted"><%# Eval("SoilType") %> | <%# Eval("WaterSource") %></small></td>
                                    <td>
                                        <%# (Eval("Status").ToString() == "Approved" || Eval("Status").ToString() == "Verified")
                                            ? "<span class='badge rounded-pill bg-success-subtle text-success border border-success-subtle px-2 py-1'><i class='bi bi-check-circle-fill me-1'></i> Approved</span>" 
                                            : (Eval("Status").ToString() == "Rejected"
                                                ? "<span class='badge rounded-pill bg-danger-subtle text-danger border border-danger-subtle px-2 py-1'><i class='bi bi-x-circle-fill me-1'></i> Rejected</span>"
                                                : "<span class='badge rounded-pill bg-warning-subtle text-warning-emphasis border border-warning-subtle px-2 py-1'><i class='bi bi-clock-history me-1'></i> Pending</span>") %>
                                    </td>
                                    <td class="text-end">
                                        <div class="d-inline-flex gap-1">
                                            <asp:LinkButton ID="btnApproveLand" runat="server" CommandName="ApproveLand" CommandArgument='<%# Eval("LandId") %>' 
                                                CssClass="btn btn-sm btn-success text-white py-1 px-2 fw-semibold" ToolTip="Approve Land">
                                                <i class="bi bi-check-lg me-1"></i>Approve
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="btnRejectLand" runat="server" CommandName="RejectLand" CommandArgument='<%# Eval("LandId") %>' 
                                                CssClass="btn btn-sm btn-danger text-white py-1 px-2 fw-semibold" ToolTip="Reject Land">
                                                <i class="bi bi-x-lg me-1"></i>Reject
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="btnDeleteLand" runat="server" CommandName="DeleteLand" CommandArgument='<%# Eval("LandId") %>' 
                                                CssClass="btn btn-sm btn-outline-secondary py-1 px-2" OnClientClick="return confirm('Are you sure?');">
                                                <i class="bi bi-trash"></i>
                                            </asp:LinkButton>
                                        </div>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                    </tbody>
                                </table>
                            </FooterTemplate>
                        </asp:Repeater>
                    </div>
                </asp:Panel>
            </div>
        </div>
    </div>
</asp:Content>
