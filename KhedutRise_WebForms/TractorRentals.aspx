<%@ Page Title="Tractor & Agricultural Equipment" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="TractorRentals.aspx.cs" Inherits="KhedutRise.TractorRentals" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- 100% Dynamic Tractor & Agricultural Equipment Marketplace -->
    <div class="kr-page-container py-4">
        <!-- Header -->
        <div class="mb-4">
            <small class="text-muted"><a href="Default.aspx" class="text-decoration-none text-muted">Home</a> / Agri Services / Equipment Marketplace</small>
            <h2 class="fw-bold mt-1 text-dark" style="font-size: 2rem;">Tractor & Agricultural Equipment</h2>
            <p class="text-muted small mb-0">Find reliable high-horsepower tractors, harvesters, and implements from verified local owners.</p>
        </div>

        <!-- Search & Filter Banner -->
        <div class="card card-custom p-4 mb-4 border shadow-sm">
            <h5 class="fw-bold mb-1" style="color: #0b6028;">Rent Verified High-HP Tractors & Harvesters</h5>
            <p class="text-muted small mb-3">Direct booking with local verified machinery operators and fleet owners.</p>
            <div class="row g-3">
                <div class="col-md-9">
                    <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" placeholder="Search tractor model or village location (e.g. Massey, Rajkot, Swaraj)..."></asp:TextBox>
                </div>
                <div class="col-md-3">
                    <asp:Button ID="btnSearch" runat="server" Text="Search Equipment" CssClass="btn btn-kr-primary w-100 py-2 fw-bold" OnClick="btnSearch_Click" style="background-color: #0b6028 !important; border-color: #0b6028 !important;" />
                </div>
            </div>
        </div>

        <!-- Empty State -->
        <asp:Panel ID="pnlNoEquipment" runat="server" Visible="false" CssClass="card p-5 text-center bg-white border mb-4">
            <i class="bi bi-truck text-muted display-4 mb-3"></i>
            <h5 class="fw-bold text-dark">No Equipment Listings Found</h5>
            <p class="text-muted small mb-3">Try clearing your search query to see all available agricultural machinery.</p>
            <div>
                <asp:LinkButton ID="btnResetSearch" runat="server" CssClass="btn btn-sm btn-outline-secondary" OnClick="btnResetSearch_Click">View All Machinery</asp:LinkButton>
            </div>
        </asp:Panel>

        <!-- Dynamic Equipment Catalog Cards -->
        <div class="row g-4 mb-4">
            <asp:Repeater ID="rptEquipments" runat="server">
                <ItemTemplate>
                    <div class="col-lg-4 col-md-6">
                        <div class="card card-custom p-4 h-100 d-flex flex-column border shadow-sm">
                            <img src='<%# ResolveUrl(Eval("ImagePath").ToString()) %>' class="rounded-3 mb-3 img-fluid object-fit-cover" style="height: 200px; width: 100%;" alt='<%# Eval("ModelName") %>' onerror="this.src='Images/massey_tractor.jpg';" />
                            <div class="d-flex justify-content-between align-items-center mb-1">
                                <span class="badge bg-success-subtle text-success border border-success-subtle small fw-bold"><%# Eval("Horsepower") %></span>
                                <small class="text-warning fw-bold"><i class="bi bi-star-fill me-1"></i><%# Eval("Rating", "{0:F1}") %></small>
                            </div>
                            <h5 class="fw-bold mb-1 text-dark" style="font-size: 1.25rem;"><%# Eval("ModelName") %></h5>
                            <small class="text-muted mb-3 d-block" style="font-size: 0.85rem;">
                                Owner: <strong><%# Eval("OwnerName") %></strong> | Location: <%# Eval("Location") %>
                            </small>
                            <h3 class="fw-bold mb-3" style="color: #0b6028; font-size: 1.6rem;">
                                ₹<%# Eval("HourlyRate", "{0:N0}") %> <small class="fs-6 text-muted fw-normal">/ Hour</small>
                            </h3>

                            <div class="mt-auto pt-2 border-top">
                                <a href='BookingConfirm.aspx?eqId=<%# Eval("EquipmentId") %>' class="btn btn-kr-primary w-100 py-2 fw-bold" style="background-color: #0b6028 !important; border-color: #0b6028 !important; border-radius: 6px;">Book Rental Now</a>
                            </div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>

        <!-- Verified Guarantee -->
        <div class="card p-3 rounded-3 bg-light border">
            <div class="d-flex align-items-center gap-3">
                <i class="bi bi-shield-check text-success display-6"></i>
                <div>
                    <h6 class="fw-bold text-dark mb-0">Operator & Equipment Guarantee</h6>
                    <small class="text-muted">All registered tractors and drivers are vetted with valid agricultural driving permits and clean maintenance records.</small>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
