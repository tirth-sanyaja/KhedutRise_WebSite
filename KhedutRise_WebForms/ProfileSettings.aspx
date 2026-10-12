<%@ Page Title="My Profile & Settings" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="ProfileSettings.aspx.cs" Inherits="KhedutRise.ProfileSettings" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Profile & Settings-->
    <div class="kr-page-container">
        <!-- Header -->
        <div class="mb-4">
            <small class="text-muted"><a href="Default.aspx" class="text-decoration-none text-muted">Home</a> / Profile Settings</small>
            <h2 class="fw-bold mt-1 text-dark" style="font-size: 2rem;">My Profile & Settings</h2>
            <p class="text-muted small mb-0">Manage your personal information, contact numbers, and security credentials.</p>
        </div>

        <!-- Top Profile Banner -->
        <div class="card card-custom p-4 mb-4">
            <div class="d-flex align-items-center gap-3">
                <div class="text-white rounded-circle d-flex align-items-center justify-content-center fw-bold fs-4 flex-shrink-0" style="width: 58px; height: 58px; background-color: #0b6028;">
                    <asp:Literal ID="litAvatar" runat="server">KR</asp:Literal>
                </div>
                <div>
                    <h5 class="fw-bold mb-1 text-dark" style="font-size: 1.25rem;">
                        <asp:Literal ID="litProfileName" runat="server">Sanyaja Tirth</asp:Literal>
                    </h5>
                    <small class="text-muted" style="font-size: 0.88rem;">
                        Role: <span class="badge bg-success-subtle text-success border border-success-subtle"><asp:Literal ID="litUserRole" runat="server">Farmer</asp:Literal></span> | Status: Verified
                    </small>
                </div>
            </div>
        </div>

        <div class="row g-4">
            <!-- LEFT: Category Sidebar -->
            <div class="col-lg-3">
                <div class="card card-custom p-3">
                    <ul class="list-unstyled mb-0 d-flex flex-column gap-3 py-2" style="font-size: 0.85rem; letter-spacing: 0.5px;">
                        <li><a href="ProfileSettings.aspx" class="text-decoration-none fw-bold" style="color: #0b6028;"><i class="bi bi-person-fill me-2"></i>PERSONAL INFORMATION</a></li>
                        <li><a href="FarmPlots.aspx" class="text-decoration-none fw-semibold text-secondary"><i class="bi bi-geo-alt me-2"></i>FARM DETAILS</a></li>
                        <li><a href="MyBookings.aspx" class="text-decoration-none fw-semibold text-secondary"><i class="bi bi-calendar-check me-2"></i>MY BOOKINGS</a></li>
                        <li><a href="Default.aspx" class="text-decoration-none fw-semibold text-secondary"><i class="bi bi-shield-lock me-2"></i>PRIVACY & SECURITY</a></li>
                    </ul>
                </div>
            </div>

            <!-- RIGHT: Forms -->
            <div class="col-lg-9">
                <!-- 1. PERSONAL INFORMATION FORM -->
                <div class="card card-custom p-4 p-md-5 mb-4">
                    <h5 class="fw-bold mb-4 text-dark" style="font-size: 1.25rem;">Personal Details Form</h5>
                    <asp:Label ID="lblProfileMsg" runat="server" CssClass="d-block mb-3 fw-bold"></asp:Label>

                    <div class="row g-3 mb-4">
                        <div class="col-md-6">
                            <label class="form-label" style="color: #0b6028;">FULL NAME <span class="required-mark">*</span></label>
                            <asp:TextBox ID="txtProfileName" runat="server" CssClass="form-control" 
                                placeholder="Enter full name" required="required"></asp:TextBox>
                        </div>
                        <div class="col-md-6">
                            <label class="form-label" style="color: #0b6028;">MOBILE NUMBER <span class="required-mark">*</span></label>
                            <asp:TextBox ID="txtProfileMobile" runat="server" type="tel" pattern="[6-9][0-9]{9}" CssClass="form-control" 
                                placeholder="10-digit mobile number" MaxLength="10" required="required"></asp:TextBox>
                        </div>
                        <div class="col-md-6">
                            <label class="form-label" style="color: #0b6028;">VILLAGE / DISTRICT <span class="required-mark">*</span></label>
                            <asp:TextBox ID="txtProfileVillage" runat="server" CssClass="form-control" 
                                placeholder="e.g. Sardhar, Rajkot" required="required"></asp:TextBox>
                        </div>
                        <div class="col-md-6">
                            <label class="form-label" style="color: #0b6028;">EMAIL ADDRESS</label>
                            <asp:TextBox ID="txtProfileEmail" runat="server" type="email" CssClass="form-control" 
                                placeholder="farmer@khedutrise.com"></asp:TextBox>
                        </div>
                        <div class="col-md-6">
                            <label class="form-label" style="color: #0b6028;">PREFERRED LANGUAGE</label>
                            <asp:DropDownList ID="ddlLang" runat="server" CssClass="form-select">
                                <asp:ListItem Text="English" Value="English" Selected="True" />
                                <asp:ListItem Text="Gujarati " Value="Gujarati" />
                                <asp:ListItem Text="Hindi " Value="Hindi" />
                            </asp:DropDownList>
                        </div>
                    </div>

                    <div class="pt-2">
                        <asp:Button ID="btnSaveProfile" runat="server" Text="Save Changes" 
                            CssClass="btn btn-kr-primary px-5 py-2 fw-bold" 
                            style="background-color: #0b6028 !important; border-color: #0b6028 !important; border-radius: 6px;" 
                            OnClick="btnSaveProfile_Click" />
                    </div>
                </div>

                <!-- 2. CHANGE PASSWORD FORM -->
                <div class="card card-custom p-4 p-md-5">
                    <h5 class="fw-bold mb-4 text-dark" style="font-size: 1.25rem;">Security & Password</h5>
                    <asp:Label ID="lblPasswordMsg" runat="server" CssClass="d-block mb-3 fw-bold"></asp:Label>

                    <div class="row g-3 mb-4">
                        <div class="col-md-4">
                            <label class="form-label" style="color: #0b6028;">CURRENT PASSWORD <span class="required-mark">*</span></label>
                            <asp:TextBox ID="txtCurrentPassword" runat="server" TextMode="Password" CssClass="form-control" 
                                placeholder="Enter current password"></asp:TextBox>
                        </div>
                        <div class="col-md-4">
                            <label class="form-label" style="color: #0b6028;">NEW PASSWORD <span class="required-mark">*</span></label>
                            <asp:TextBox ID="txtNewPassword" runat="server" TextMode="Password" CssClass="form-control" 
                                placeholder="Min 6 characters" minlength="6"></asp:TextBox>
                        </div>
                        <div class="col-md-4">
                            <label class="form-label" style="color: #0b6028;">CONFIRM NEW PASSWORD <span class="required-mark">*</span></label>
                            <asp:TextBox ID="txtConfirmNewPassword" runat="server" TextMode="Password" CssClass="form-control" 
                                placeholder="Re-enter new password" minlength="6"></asp:TextBox>
                        </div>
                    </div>

                    <div class="pt-2">
                        <asp:Button ID="btnChangePassword" runat="server" Text="Update Password" 
                            CssClass="btn btn-outline-dark px-4 py-2 fw-bold" 
                            OnClick="btnChangePassword_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
