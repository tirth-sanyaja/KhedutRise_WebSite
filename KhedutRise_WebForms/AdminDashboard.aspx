<%@ Page Title="Admin Dashboard" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="AdminDashboard.aspx.cs" Inherits="KhedutRise.AdminDashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Admin Dashboard: -->
    <div class="kr-page-container">
        <!-- Header -->
        <div class="mb-4">
            <small class="text-muted"><a href="Default.aspx" class="text-decoration-none text-muted">Home</a> / Admin Portal / Dashboard</small>
            <h2 class="fw-bold mt-1 text-dark" style="font-size: 2rem;">KhedutRise Admin Dashboard</h2>
        </div>

        <div class="row g-4">
            <!-- LEFT: Dark Navy Navigation Sidebar -->
            <div class="col-lg-3">
                <div class="p-4 rounded-3 text-white h-100" style="background-color: #0f172a;">
                    <ul class="list-unstyled mb-0 d-flex flex-column gap-3 py-2" style="font-size: 0.95rem;">
                        <li><a href="AdminDashboard.aspx" class="text-decoration-none fw-bold d-block" style="color: #22c55e;">• Dashboard</a></li>
                        <li><a href="AdminManagement.aspx?tab=Farmers" class="text-decoration-none text-white d-block opacity-90"><i class="bi bi-people me-2"></i>Farmers List</a></li>
                        <li><a href="AdminManagement.aspx?tab=Buyers" class="text-decoration-none text-white d-block opacity-90"><i class="bi bi-shop me-2"></i>Buyers List</a></li>
                        <li><a href="AdminManagement.aspx?tab=Moderation" class="text-decoration-none text-white d-block opacity-90"><i class="bi bi-shield-check me-2"></i>Moderation</a></li>
                        <li><a href="AdminAnalytics.aspx" class="text-decoration-none text-white d-block opacity-90"><i class="bi bi-graph-up me-2"></i>Analytics</a></li>
                        <li><a href="AdminSettings.aspx" class="text-decoration-none text-white d-block opacity-90"><i class="bi bi-gear me-2"></i>Settings</a></li>
                    </ul>
                </div>
            </div>

            <!-- RIGHT: Metrics & Pending Registrations -->
            <div class="col-lg-9">
                <!-- 4 Top KPI Cards -->
                <div class="row g-3 mb-4">
                    <div class="col-6 col-md-3">
                        <div class="card card-custom p-3 text-start h-100">
                            <small class="text-muted fw-bold" style="font-size: 0.72rem; letter-spacing: 0.5px;">TOTAL FARMERS</small>
                            <h4 class="fw-bold mt-2 mb-0" style="color: #0b6028; font-size: 1.4rem;">42,500</h4>
                        </div>
                    </div>
                    <div class="col-6 col-md-3">
                        <div class="card card-custom p-3 text-start h-100">
                            <small class="text-muted fw-bold" style="font-size: 0.72rem; letter-spacing: 0.5px;">TOTAL BUYERS</small>
                            <h4 class="fw-bold mt-2 mb-0" style="color: #0b6028; font-size: 1.4rem;">8,400</h4>
                        </div>
                    </div>
                    <div class="col-6 col-md-3">
                        <div class="card card-custom p-3 text-start h-100">
                            <small class="text-muted fw-bold" style="font-size: 0.72rem; letter-spacing: 0.5px;">APPROVED USERS</small>
                            <h4 class="fw-bold mt-2 mb-0" style="color: #16a34a; font-size: 1.4rem;">
                                <asp:Label ID="lblApprovedCount" runat="server" Text="0"></asp:Label>
                            </h4>
                        </div>
                    </div>
                    <div class="col-6 col-md-3">
                        <div class="card card-custom p-3 text-start h-100">
                            <small class="text-muted fw-bold" style="font-size: 0.72rem; letter-spacing: 0.5px;">PENDING / REJECTED</small>
                            <h4 class="fw-bold mt-2 mb-0" style="color: #ea580c; font-size: 1.4rem;">
                                <asp:Label ID="lblPendingRejectedCount" runat="server" Text="0"></asp:Label>
                            </h4>
                        </div>
                    </div>
                </div>

                <!-- Action Notification Banner -->
                <asp:Panel ID="pnlActionAlert" runat="server" Visible="false" CssClass="alert alert-success alert-dismissible fade show p-3 mb-4 rounded-3 shadow-sm" role="alert">
                    <div class="d-flex align-items-center">
                        <i class="bi bi-info-circle-fill fs-5 me-2"></i>
                        <div>
                            <asp:Label ID="lblActionMsg" runat="server" CssClass="fw-bold"></asp:Label>
                        </div>
                    </div>
                    <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
                </asp:Panel>

                <!-- Registrations Approval Table -->
                <div class="card card-custom p-4">
                    <div class="d-flex flex-wrap justify-content-between align-items-center mb-3">
                        <div>
                            <h5 class="fw-bold mb-1 text-dark">Registrations Approval & Moderation</h5>
                            <small class="text-muted">Review, approve, or reject new user registrations across the platform.</small>
                        </div>
                        <div class="btn-group btn-group-sm mt-2 mt-md-0" role="group">
                            <asp:LinkButton ID="btnFilterAll" runat="server" CssClass="btn btn-outline-secondary" OnClick="FilterStatus_Click" CommandArgument="ALL">All</asp:LinkButton>
                            <asp:LinkButton ID="btnFilterPending" runat="server" CssClass="btn btn-warning text-dark fw-bold" OnClick="FilterStatus_Click" CommandArgument="Pending">Pending</asp:LinkButton>
                            <asp:LinkButton ID="btnFilterApproved" runat="server" CssClass="btn btn-outline-success" OnClick="FilterStatus_Click" CommandArgument="Verified">Approved</asp:LinkButton>
                            <asp:LinkButton ID="btnFilterRejected" runat="server" CssClass="btn btn-outline-danger" OnClick="FilterStatus_Click" CommandArgument="Rejected">Rejected</asp:LinkButton>
                        </div>
                    </div>

                    <div class="table-responsive">
                        <asp:Repeater ID="rptPending" runat="server" OnItemCommand="rptPending_ItemCommand">
                            <HeaderTemplate>
                                <table class="table table-hover align-middle mb-0">
                                    <thead class="text-muted" style="font-size: 0.72rem; letter-spacing: 0.5px;">
                                        <tr>
                                            <th>NAME</th>
                                            <th>ROLE</th>
                                            <th>LOCATION</th>
                                            <th>STATUS</th>
                                            <th>DOCUMENT</th>
                                            <th class="text-end">ACTION</th>
                                        </tr>
                                    </thead>
                                    <tbody style="font-size: 0.92rem;">
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td><strong class="text-dark"><%# Eval("FullName") %></strong></td>
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
                                        <a href="#" class="text-decoration-none fw-semibold small" style="color: #0284c7;">
                                            <i class="bi bi-file-earmark-pdf me-1"></i><%# Eval("UserRole").ToString() == "Farmer" ? "Land_7_12.pdf" : (Eval("UserRole").ToString() == "Buyer" ? "GST_Certificate.pdf" : "RC_License.pdf") %>
                                        </a>
                                    </td>
                                    <td class="text-end">
                                        <div class="d-inline-flex gap-1">
                                            <asp:LinkButton ID="btnApprove" runat="server" CommandName="Approve" CommandArgument='<%# Eval("UserId") %>' 
                                                CssClass="btn btn-sm btn-success text-white py-1 px-2 fw-semibold" 
                                                ToolTip="Approve this account">
                                                <i class="bi bi-check-lg me-1"></i>Approve
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="btnReject" runat="server" CommandName="Reject" CommandArgument='<%# Eval("UserId") %>' 
                                                CssClass="btn btn-sm btn-outline-danger py-1 px-2 fw-semibold" 
                                                ToolTip="Reject this account">
                                                <i class="bi bi-x-lg me-1"></i>Reject
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
                </div>
            </div>
        </div>
    </div>
</asp:Content>
