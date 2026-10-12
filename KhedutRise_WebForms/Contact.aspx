<%@ Page Title="Contact Support" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Contact.aspx.cs" Inherits="KhedutRise.Contact" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Contact Support Page -->
    <div class="kr-page-container">
        <!--& Header -->
        <div class="mb-4">
            <small class="text-muted"><a href="Default.aspx" class="text-decoration-none text-muted">Home</a> / Contact Us</small>
            <h2 class="fw-bold mt-1 text-dark" style="font-size: 2rem;">Contact KhedutRise Support</h2>
            <p class="text-muted small mb-0">Have questions about mandi rates, farm rentals, or buying crops? Get in touch with our team.</p>
        </div>

        <div class="row g-4">
            <!-- LEFT: Send Us a Message Form -->
            <div class="col-lg-6">
                <div class="card card-custom p-4 p-md-5 h-100 d-flex flex-column">
                    <h5 class="fw-bold mb-3" style="color: #0b6028; font-size: 1.25rem;">Send Us a Message</h5>
                    <p class="small text-muted mb-4">Fill out the form below. We will get back to you within 24 business hours.</p>

                    <asp:Label ID="lblContactMsg" runat="server" CssClass="d-block mb-3 fw-bold"></asp:Label>

                    <div class="mb-3">
                        <label class="form-label" style="color: #0b6028;">FULL NAME <span class="required-mark">*</span></label>
                        <asp:TextBox ID="txtContactName" runat="server" CssClass="form-control" 
                            placeholder="Enter your full name" required="required"></asp:TextBox>
                    </div>

                    <div class="row g-2 mb-3">
                        <div class="col-md-6">
                            <label class="form-label" style="color: #0b6028;">MOBILE NUMBER <span class="required-mark">*</span></label>
                            <asp:TextBox ID="txtContactMobile" runat="server" type="tel" pattern="[6-9][0-9]{9}" CssClass="form-control" 
                                placeholder="10-digit mobile number" MaxLength="10" required="required"></asp:TextBox>
                        </div>
                        <div class="col-md-6">
                            <label class="form-label" style="color: #0b6028;">EMAIL ADDRESS <span class="required-mark">*</span></label>
                            <asp:TextBox ID="txtContactEmail" runat="server" type="email" CssClass="form-control" 
                                placeholder="name@example.com" required="required"></asp:TextBox>
                        </div>
                    </div>

                    <div class="mb-3">
                        <label class="form-label" style="color: #0b6028;">INQUIRY CATEGORY <span class="required-mark">*</span></label>
                        <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-select" required="required">
                            <asp:ListItem Text="General Support & Inquiries" Value="General Support" Selected="True" />
                            <asp:ListItem Text="Crop Marketplace & Orders" Value="Marketplace" />
                            <asp:ListItem Text="Tractor & Harvester Rentals" Value="Machinery Rentals" />
                            <asp:ListItem Text="Soil Testing & Agri Services" Value="Agri Services" />
                            <asp:ListItem Text="Farmland Listings & Registration" Value="Land Portal" />
                        </asp:DropDownList>
                    </div>

                    <div class="mb-4">
                        <label class="form-label" style="color: #0b6028;">MESSAGE / INQUIRY DETAILS <span class="required-mark">*</span></label>
                        <asp:TextBox ID="txtContactBody" runat="server" TextMode="MultiLine" Rows="4" CssClass="form-control" 
                            placeholder="Please describe your question or requirement in detail (minimum 10 characters)..." 
                            minlength="10" required="required"></asp:TextBox>
                    </div>

                    <asp:Button ID="btnSubmitContact" runat="server" Text="Submit Message" 
                        CssClass="btn btn-kr-primary w-100 mt-auto py-2 fw-bold" 
                        style="background-color: #0b6028 !important; border-color: #0b6028 !important; border-radius: 6px;" 
                        OnClick="btnSubmitContact_Click" />
                </div>
            </div>

            <!-- RIGHT: Our Offices & Support Channels -->
            <div class="col-lg-6">
                <div class="card card-custom p-4 p-md-5 h-100 d-flex flex-column">
                    <h5 class="fw-bold mb-4 text-dark" style="font-size: 1.25rem;">Our Offices & Support Channels</h5>

                    <div class="mb-3">
                        <small class="text-muted fw-bold d-block" style="font-size: 0.72rem; letter-spacing: 0.5px;">TOLL-FREE HELPLINE</small>
                        <h5 class="fw-bold mb-0" style="color: #0b6028; font-size: 1.15rem;">+91 1800 233 5555 <small class="text-dark fw-normal" style="font-size: 0.95rem;">(8:00 AM – 8:00 PM)</small></h5>
                    </div>

                    <div class="mb-3">
                        <small class="text-muted fw-bold d-block" style="font-size: 0.72rem; letter-spacing: 0.5px;">OFFICIAL EMAIL</small>
                        <p class="mb-0 fw-bold text-dark" style="font-size: 1rem;">support@khedutrise.com</p>
                    </div>

                    <div class="mb-4">
                        <small class="text-muted fw-bold d-block" style="font-size: 0.72rem; letter-spacing: 0.5px;">HEADQUARTERS & AGRI HUB</small>
                        <p class="mb-0 text-dark fw-bold" style="font-size: 0.95rem;">KhedutRise Innovation Labs, Near APMC Market Yard, Rajkot, Gujarat - 360003</p>
                    </div>

                    <!-- Map Location Photo with Pin -->
                    <div class="position-relative rounded-3 overflow-hidden border mt-auto" style="height: 180px;">
                        <img src="Images/aerial_plots.jpg" class="img-fluid object-fit-cover w-100 h-100" alt="HQ Location" />
                        <div class="position-absolute translate-middle" style="top: 50%; left: 50%;">
                            <div class="rounded-circle shadow" style="width: 22px; height: 22px; background-color: #ea580c; border: 2.5px solid #ffffff;"></div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
