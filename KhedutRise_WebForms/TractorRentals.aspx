<%@ Page Title="Tractor & Agricultural Equipment" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="TractorRentals.aspx.cs" Inherits="KhedutRise.TractorRentals" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Tractor & Agricultural Equipment Marketplace -->
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
                        <div class="card card-custom h-100 d-flex flex-column border shadow-sm overflow-hidden" style="border-radius: 12px;">
                            <!-- Tractor Photo – unique per model -->
                            <img src='<%# ResolveUrl(Eval("ImagePath").ToString()) %>'
                                 class="img-fluid object-fit-cover"
                                 style="height: 210px; width: 100%;"
                                 alt='<%# Eval("ModelName") %>'
                                 onerror='this.onerror=null; this.src="Images/massey_tractor.jpg";' />

                            <div class="p-3 d-flex flex-column flex-grow-1">
                                <!-- Badge + Rating -->
                                <div class="d-flex justify-content-between align-items-center mb-2">
                                    <span class="badge bg-success-subtle text-success border border-success-subtle small fw-bold"><%# Eval("Horsepower") %></span>
                                    <small class="text-warning fw-bold"><i class="bi bi-star-fill me-1"></i><%# Eval("Rating", "{0:F1}") %></small>
                                </div>

                                <!-- Model Name -->
                                <h5 class="fw-bold mb-1 text-dark" style="font-size: 1.2rem;"><%# Eval("ModelName") %></h5>

                                <!-- Description -->
                                <small class="text-muted mb-2 d-block" style="font-size: 0.82rem; line-height: 1.4;"><%# Eval("Description") %></small>

                                <!-- Owner & Location -->
                                <small class="text-muted mb-3 d-block" style="font-size: 0.83rem;">
                                    <i class="bi bi-person-fill me-1 text-success"></i><strong><%# Eval("OwnerName") %></strong>
                                    &nbsp;|&nbsp;<i class="bi bi-geo-alt-fill me-1 text-success"></i><%# Eval("Location") %>
                                </small>

                                <!-- Attachments Section -->
                                <div class="mb-3 p-2 rounded-2" style="background: #f0faf3; border: 1px solid #c3e6cb;">
                                    <small class="fw-bold text-success d-block mb-1" style="font-size: 0.8rem;">
                                        <i class="bi bi-tools me-1"></i> Available Attachments
                                    </small>
                                    <%# BuildAttachmentBadges(Eval("Attachments")) %>
                                </div>

                                <!-- Price -->
                                <h4 class="fw-bold mb-3 mt-auto" style="color: #0b6028; font-size: 1.5rem;">
                                    ₹<%# Eval("HourlyRate", "{0:N0}") %> <small class="fs-6 text-muted fw-normal">/ Hour</small>
                                </h4>

                                <!-- Book Button -->
                                <div class="border-top pt-2">
                                    <a href='BookingConfirm.aspx?eqId=<%# Eval("EquipmentId") %>'
                                       class="btn w-100 py-2 fw-bold text-white"
                                       style="background-color: #0b6028; border-color: #0b6028; border-radius: 6px;">
                                        <i class="bi bi-calendar-check me-1"></i> Book Rental Now
                                    </a>
                                </div>
                            </div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>

        <!-- Attachments Reference Guide -->
        <div class="card card-custom p-4 mb-4 border shadow-sm">
            <h5 class="fw-bold mb-3 text-dark"><i class="bi bi-tools me-2 text-success"></i>Common Tractor Attachments</h5>
            <div class="row g-3">
                <div class="col-md-4 col-sm-6">
                    <div class="d-flex gap-3 align-items-start p-3 rounded-3" style="background:#f8f9fa; border:1px solid #dee2e6;">
                        <span style="font-size:2rem;"></span>
                        <div>
                            <strong class="d-block text-dark" style="font-size:0.9rem;">Rotavator</strong>
                            <small class="text-muted" style="font-size:0.8rem;">Breaks and tills soil, ideal for cotton, wheat & vegetable beds</small>
                        </div>
                    </div>
                </div>
                <div class="col-md-4 col-sm-6">
                    <div class="d-flex gap-3 align-items-start p-3 rounded-3" style="background:#f8f9fa; border:1px solid #dee2e6;">
                        <span style="font-size:2rem;"></span>
                        <div>
                            <strong class="d-block text-dark" style="font-size:0.9rem;">Seed Drill</strong>
                            <small class="text-muted" style="font-size:0.8rem;">Precision row sowing of wheat, cumin, mustard & oilseeds</small>
                        </div>
                    </div>
                </div>
                <div class="col-md-4 col-sm-6">
                    <div class="d-flex gap-3 align-items-start p-3 rounded-3" style="background:#f8f9fa; border:1px solid #dee2e6;">
                        <span style="font-size:2rem;"></span>
                        <div>
                            <strong class="d-block text-dark" style="font-size:0.9rem;">Cultivator</strong>
                            <small class="text-muted" style="font-size:0.8rem;">Weeding between crop rows, inter-cultivation for aeration</small>
                        </div>
                    </div>
                </div>
                <div class="col-md-4 col-sm-6">
                    <div class="d-flex gap-3 align-items-start p-3 rounded-3" style="background:#f8f9fa; border:1px solid #dee2e6;">
                        <span style="font-size:2rem;"></span>
                        <div>
                            <strong class="d-block text-dark" style="font-size:0.9rem;">Boom Sprayer</strong>
                            <small class="text-muted" style="font-size:0.8rem;">Wide-coverage pesticide & fertiliser spraying for large fields</small>
                        </div>
                    </div>
                </div>
                <div class="col-md-4 col-sm-6">
                    <div class="d-flex gap-3 align-items-start p-3 rounded-3" style="background:#f8f9fa; border:1px solid #dee2e6;">
                        <span style="font-size:2rem;"></span>
                        <div>
                            <strong class="d-block text-dark" style="font-size:0.9rem;">MB Plough</strong>
                            <small class="text-muted" style="font-size:0.8rem;">Deep primary tillage; turns soil, buries weeds and crop residue</small>
                        </div>
                    </div>
                </div>
                <div class="col-md-4 col-sm-6">
                    <div class="d-flex gap-3 align-items-start p-3 rounded-3" style="background:#f8f9fa; border:1px solid #dee2e6;">
                        <span style="font-size:2rem;"></span>
                        <div>
                            <strong class="d-block text-dark" style="font-size:0.9rem;">Laser Leveler</strong>
                            <small class="text-muted" style="font-size:0.8rem;">GPS-guided land levelling for paddy and sugarcane fields</small>
                        </div>
                    </div>
                </div>
                <div class="col-md-4 col-sm-6">
                    <div class="d-flex gap-3 align-items-start p-3 rounded-3" style="background:#f8f9fa; border:1px solid #dee2e6;">
                        <span style="font-size:2rem;"></span>
                        <div>
                            <strong class="d-block text-dark" style="font-size:0.9rem;">Disc Harrow</strong>
                            <small class="text-muted" style="font-size:0.8rem;">Secondary tillage to break clods and prepare seedbed</small>
                        </div>
                    </div>
                </div>
                <div class="col-md-4 col-sm-6">
                    <div class="d-flex gap-3 align-items-start p-3 rounded-3" style="background:#f8f9fa; border:1px solid #dee2e6;">
                        <span style="font-size:2rem;"></span>
                        <div>
                            <strong class="d-block text-dark" style="font-size:0.9rem;">Front Loader</strong>
                            <small class="text-muted" style="font-size:0.8rem;">Lifting and loading loose material, manure, sand and gravel</small>
                        </div>
                    </div>
                </div>
                <div class="col-md-4 col-sm-6">
                    <div class="d-flex gap-3 align-items-start p-3 rounded-3" style="background:#f8f9fa; border:1px solid #dee2e6;">
                        <span style="font-size:2rem;"></span>
                        <div>
                            <strong class="d-block text-dark" style="font-size:0.9rem;">Ridger / Bund Former</strong>
                            <small class="text-muted" style="font-size:0.8rem;">Creates furrows for irrigation channels and earthen bunds</small>
                        </div>
                    </div>
                </div>
            </div>
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
