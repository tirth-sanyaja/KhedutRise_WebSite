<%@ Page Title="Agricultural Services" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="AgriServices.aspx.cs" Inherits="KhedutRise.AgriServices" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Agricultural Services-->
    <div class="kr-page-container">
        <!-- Header -->
        <div class="mb-4">
            <small class="text-muted"><a href="Default.aspx" class="text-decoration-none text-muted">Home</a> / Agri Services</small>
            <h2 class="fw-bold mt-1 text-dark" style="font-size: 2rem;">Agricultural Services</h2>
            <p class="text-muted small mb-0">Get essential services to manage your farm efficiently.</p>
        </div>

        <!-- 4 Top Service Chips -->
        <div class="card card-custom p-3 mb-4">
            <div class="row g-3">
                <div class="col-6 col-md-3">
                    <div class="p-3 text-start rounded-2" style="background-color: #f8fafc;">
                        <h6 class="fw-bold mb-1" style="color: #0b6028; font-size: 1rem;">Soil Testing</h6>
                        <small class="text-muted" style="font-size: 0.8rem;">Lab Visit</small>
                    </div>
                </div>
                <div class="col-6 col-md-3">
                    <div class="p-3 text-start rounded-2" style="background-color: #f8fafc;">
                        <h6 class="fw-bold mb-1" style="color: #0b6028; font-size: 1rem;">Irrigation Setup</h6>
                        <small class="text-muted" style="font-size: 0.8rem;">Drip & Sprinkler</small>
                    </div>
                </div>
                <div class="col-6 col-md-3">
                    <div class="p-3 text-start rounded-2" style="background-color: #f8fafc;">
                        <h6 class="fw-bold mb-1" style="color: #0b6028; font-size: 1rem;">Land Survey</h6>
                        <small class="text-muted" style="font-size: 0.8rem;">GPS Boundary</small>
                    </div>
                </div>
                <div class="col-6 col-md-3">
                    <div class="p-3 text-start rounded-2" style="background-color: #f8fafc;">
                        <h6 class="fw-bold mb-1" style="color: #0b6028; font-size: 1rem;">Logistics</h6>
                        <small class="text-muted" style="font-size: 0.8rem;">Crop Transport</small>
                    </div>
                </div>
            </div>
        </div>

        <div class="row g-4">
            <!-- LEFT: Verified Service Providers -->
            <div class="col-lg-7">
                <div class="card card-custom p-4 h-100">
                    <h5 class="fw-bold mb-4 text-dark">Verified Service Providers</h5>
                    <div class="card p-3 d-flex flex-row justify-content-between align-items-center gap-3 border rounded-3 mb-3" style="background-color: #ffffff;">
                        <div class="d-flex align-items-center gap-3">
                            <img src="Images/weather_sunbeam.jpg" class="rounded-3 object-fit-cover flex-shrink-0" width="120" height="85" alt="Lab Test" />
                            <div>
                                <h6 class="fw-bold mb-1 text-dark" style="font-size: 1rem;">Digital Soil Testing Service</h6>
                                <small class="text-muted d-block mb-1" style="font-size: 0.8rem;">Provider: Rajkot Agri Labs | Verified Provider</small>
                                <strong style="color: #0b6028; font-size: 0.95rem;">Starting Price: ₹450 / Sample</strong>
                            </div>
                        </div>
                        <a href="#request" class="btn btn-kr-primary px-3 py-2 fw-bold text-nowrap" style="background-color: #0b6028 !important; border-color: #0b6028 !important;">Book Now</a>
                    </div>

                    <div class="card p-3 d-flex flex-row justify-content-between align-items-center gap-3 border rounded-3" style="background-color: #ffffff;">
                        <div class="d-flex align-items-center gap-3">
                            <img src="Images/aerial_plots.jpg" class="rounded-3 object-fit-cover flex-shrink-0" width="120" height="85" alt="Land Survey" />
                            <div>
                                <h6 class="fw-bold mb-1 text-dark" style="font-size: 1rem;">GPS Farmland Boundary Survey</h6>
                                <small class="text-muted d-block mb-1" style="font-size: 0.8rem;">Provider: Gujarat Geospatial Services</small>
                                <strong style="color: #0b6028; font-size: 0.95rem;">Price: ₹1,200 / Plot</strong>
                            </div>
                        </div>
                        <a href="#request" class="btn btn-kr-primary px-3 py-2 fw-bold text-nowrap" style="background-color: #0b6028 !important; border-color: #0b6028 !important;">Book Now</a>
                    </div>
                </div>
            </div>

            <!-- RIGHT: Request Agri Service -->
            <div class="col-lg-5" id="request">
                <div class="card card-custom p-4 p-md-5 h-100 d-flex flex-column">
                    <h5 class="fw-bold mb-2 text-dark">Request Agri Service</h5>
                    <p class="small text-muted mb-3">Book an on-site expert visit to your farm.</p>

                    <asp:Label ID="lblServiceMsg" runat="server" CssClass="d-block mb-3 fw-bold"></asp:Label>

                    <div class="mb-3">
                        <label class="form-label" style="color: #0b6028;">SELECT SERVICE <span class="required-mark">*</span></label>
                        <asp:DropDownList ID="ddlService" runat="server" CssClass="form-select" required="required">
                            <asp:ListItem Text="Soil Testing Lab Visit (₹450)" Value="Soil Testing Lab Visit" Selected="True" />
                            <asp:ListItem Text="Irrigation Setup Consultation (₹800)" Value="Irrigation Setup" />
                            <asp:ListItem Text="GPS Land Boundary Survey (₹1,200)" Value="Land Survey" />
                            <asp:ListItem Text="Crop Logistics & Truck Transport" Value="Logistics" />
                        </asp:DropDownList>
                    </div>

                    <div class="mb-3">
                        <label class="form-label" style="color: #0b6028;">FARMER / CONTACT NAME <span class="required-mark">*</span></label>
                        <asp:TextBox ID="txtCustomerName" runat="server" CssClass="form-control" 
                            placeholder="e.g. Sanyaja Tirth" required="required"></asp:TextBox>
                    </div>

                    <div class="mb-3">
                        <label class="form-label" style="color: #0b6028;">MOBILE NUMBER <span class="required-mark">*</span></label>
                        <asp:TextBox ID="txtCustomerMobile" runat="server" type="tel" pattern="[6-9][0-9]{9}" CssClass="form-control" 
                            placeholder="10-digit mobile number" MaxLength="10" required="required"></asp:TextBox>
                    </div>

                    <div class="mb-4">
                        <label class="form-label" style="color: #0b6028;">PREFERRED VISIT DATE & LOCATION <span class="required-mark">*</span></label>
                        <asp:TextBox ID="txtDateTime" runat="server" CssClass="form-control" 
                            placeholder="e.g. 15 Aug 2026, Plot A, Rajkot" required="required"></asp:TextBox>
                    </div>

                    <asp:Button ID="btnConfirmService" runat="server" Text="Confirm Service Request" 
                        CssClass="btn btn-kr-primary w-100 mt-auto py-2 fw-bold" 
                        style="background-color: #0b6028 !important; border-color: #0b6028 !important;" 
                        OnClick="btnConfirmService_Click" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
