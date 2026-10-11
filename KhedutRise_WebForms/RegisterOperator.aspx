<%@ Page Title="Register Machinery Operator" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="RegisterOperator.aspx.cs" Inherits="KhedutRise.RegisterOperator" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Machinery Operator Registration-->
    <div class="kr-page-container py-5">
        <div class="row g-4 align-items-stretch justify-content-center">
            <!-- LEFT BANNER -->
            <div class="col-lg-4 col-md-5 d-none d-md-block">
                <div class="card border rounded-4 overflow-hidden h-100 shadow-sm d-flex flex-column" style="border-color: #e2e8f0 !important; background: #ffffff;">
                    <img src="Images/massey_tractor.jpg" class="object-fit-cover w-100" style="height: 240px;" alt="Tractor" />
                    <div class="p-4 p-lg-5 text-white flex-grow-1 d-flex flex-column justify-content-center" style="background-color: #0b6028;">
                        <span class="badge bg-warning text-dark mb-2 px-3 py-1 align-self-start fw-bold">Machinery Registry</span>
                        <h3 class="fw-bold mb-1 fs-2">Operator Registry</h3>
                        <p class="small text-white-50 mb-3">List your tractors, rotavators, and harvesters. Connect with nearby farmers needing field services.</p>
                        <div class="d-flex flex-column gap-2 small text-white-70 mt-3">
                            <div><i class="bi bi-geo-alt-fill text-warning me-2"></i>Targeted local village radius</div>
                            <div><i class="bi bi-calendar-check text-warning me-2"></i>Direct booking schedules</div>
                            <div><i class="bi bi-cash-stack text-warning me-2"></i>Fair hourly or per-acre earnings</div>
                        </div>
                    </div>
                </div>
            </div>

            <!-- RIGHT REGISTRATION FORM -->
            <div class="col-lg-8 col-md-7">
                <div class="card p-4 p-md-5 border rounded-3 shadow-none h-100" style="border-color: #e2e8f0 !important; background: #ffffff;">
                    <div class="mb-4">
                        <small class="text-muted fw-bold" style="font-size: 0.75rem; letter-spacing: 0.5px;">MACHINERY OPERATOR ONBOARDING</small>
                        <h3 class="fw-bold mt-1 text-dark" style="font-size: 1.85rem;">Register as Machinery Operator</h3>
                        <p class="small text-muted mb-0">Fill in your tractor and service details. All fields marked with <span class="required-mark">*</span> are required.</p>
                    </div>

                    <asp:Label ID="lblOpMsg" runat="server" CssClass="d-block mb-3 fw-bold"></asp:Label>

                    <div class="row g-3">
                        <!-- Full Name -->
                        <div class="col-md-6">
                            <label class="form-label" style="color: #0b6028;">OPERATOR FULL NAME <span class="required-mark">*</span></label>
                            <asp:TextBox ID="txtName" runat="server" CssClass="form-control" 
                                placeholder="e.g. Dilipbhai Parmar" required="required"></asp:TextBox>
                        </div>

                        <!-- Mobile Number with Send OTP -->
                        <div class="col-md-6">
                            <label class="form-label" style="color: #0b6028;">MOBILE NUMBER <span class="required-mark">*</span></label>
                            <div class="input-group">
                                <span class="input-group-text bg-light text-muted border-end-0 fw-semibold">+91</span>
                                <asp:TextBox ID="txtMobile" runat="server" type="tel" pattern="[6-9][0-9]{9}" CssClass="form-control border-start-0 ps-1" 
                                    placeholder="10-digit mobile number" MaxLength="10" required="required"></asp:TextBox>
                            </div>
                        </div>

                        <!-- Equipment / Tractor Model -->
                        <div class="col-md-6">
                            <label class="form-label" style="color: #0b6028;">EQUIPMENT / TRACTOR MODEL <span class="required-mark">*</span></label>
                            <asp:TextBox ID="txtModel" runat="server" CssClass="form-control" 
                                placeholder="e.g. Massey Ferguson 241DI (41 HP)" required="required"></asp:TextBox>
                        </div>

                        <!-- Rental Rate -->
                        <div class="col-md-6">
                            <label class="form-label" style="color: #0b6028;">HOURLY RENTAL RATE (INR) <span class="required-mark">*</span></label>
                            <div class="input-group">
                                <span class="input-group-text bg-light text-muted">₹</span>
                                <asp:TextBox ID="txtRate" runat="server" type="number" min="100" CssClass="form-control" 
                                    placeholder="800" required="required"></asp:TextBox>
                                <span class="input-group-text bg-light text-muted">/ Hour</span>
                            </div>
                        </div>

                        <!-- Available Attachments -->
                        <div class="col-md-6">
                            <label class="form-label" style="color: #0b6028;">ATTACHMENTS AVAILABLE <span class="required-mark">*</span></label>
                            <asp:TextBox ID="txtAttachments" runat="server" CssClass="form-control" 
                                placeholder="e.g. Rotavator, Cultivator, MB Plough" required="required"></asp:TextBox>
                        </div>

                        <!-- Operating Radius -->
                        <div class="col-md-6">
                            <label class="form-label" style="color: #0b6028;">SERVICE VILLAGE RADIUS <span class="required-mark">*</span></label>
                            <asp:TextBox ID="txtRadius" runat="server" CssClass="form-control" 
                                placeholder="e.g. 15 km around Rajkot" required="required"></asp:TextBox>
                        </div>

                        <!-- Password -->
                        <div class="col-md-6">
                            <label class="form-label" style="color: #0b6028;">CREATE PASSWORD <span class="required-mark">*</span></label>
                            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control" 
                                placeholder="Min 6 characters" minlength="6" required="required"></asp:TextBox>
                        </div>

                        <!-- Confirm Password -->
                        <div class="col-md-6">
                            <label class="form-label" style="color: #0b6028;">CONFIRM PASSWORD <span class="required-mark">*</span></label>
                            <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password" CssClass="form-control" 
                                placeholder="Re-enter password" minlength="6" required="required"></asp:TextBox>
                        </div>

                        <!-- Upload Documents -->
                        <div class="col-12 pt-2">
                            <label class="form-label text-muted mb-2">DRIVER LICENSE & TRACTOR RC (OPTIONAL)</label>
                            <div class="border border-dashed rounded-3 p-3 text-center" style="border-color: #cbd5e1 !important; background-color: #f8fafc;">
                                <p class="small text-muted mb-2 fw-semibold">Upload Commercial Driving License or Tractor RC for verified operator badge</p>
                                <asp:FileUpload ID="fuOpDoc" runat="server" CssClass="form-control form-control-sm mx-auto" style="max-width: 320px;" />
                            </div>
                        </div>

                        <!-- Submit Button -->
                        <div class="col-12 pt-3">
                            <asp:Button ID="btnSubmitOp" runat="server" Text="Create Operator Account" 
                                CssClass="btn w-100 py-3 fw-bold text-white shadow-sm" 
                                OnClick="btnSubmitOp_Click" 
                                style="background-color: #0b6028; border-radius: 6px; font-size: 1rem; border: none;" />
                        </div>

                        <!-- Already have account -->
                        <div class="col-12 text-center pt-2">
                            <span class="small text-muted">Already registered? </span>
                            <a href="Login.aspx" class="small fw-bold text-decoration-none" style="color: #0b6028;">Sign In to your account</a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
