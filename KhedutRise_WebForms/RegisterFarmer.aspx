<%@ Page Title="Create Farmer Account" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="RegisterFarmer.aspx.cs" Inherits="KhedutRise.RegisterFarmer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Farmer Registration: Clean, Professional & Fully Validated -->
    <div class="kr-page-container py-5">
        <div class="row g-4 align-items-stretch justify-content-center">
            <!-- LEFT BANNER -->
            <div class="col-lg-4 col-md-5 d-none d-md-block">
                <div class="card border-0 rounded-4 overflow-hidden position-relative text-white shadow-sm h-100" style="min-height: 640px; background: linear-gradient(to bottom, rgba(0,0,0,0.15) 0%, rgba(0,0,0,0.8) 100%), url('Images/green_rows.jpg') center/cover no-repeat;">
                    <div class="p-4 p-lg-5 mt-auto" style="text-shadow: 0 2px 8px rgba(0,0,0,0.7);">
                        <span class="badge bg-success mb-2 px-3 py-1">Farmer Registration</span>
                        <h3 class="fw-bold mb-1 fs-2">Join KhedutRise</h3>
                        <p class="small text-white-50 mb-3">Sell crops directly at fair APMC rates, rent modern tractors, and get soil test advisories.</p>
                        <div class="d-flex flex-column gap-2 small text-white-70 mt-3">
                            <div><i class="bi bi-check-circle-fill text-success me-2"></i>Zero middleman commission</div>
                            <div><i class="bi bi-check-circle-fill text-success me-2"></i>Live APMC Mandi updates</div>
                            <div><i class="bi bi-check-circle-fill text-success me-2"></i>Guaranteed escrow payments</div>
                        </div>
                    </div>
                </div>
            </div>

            <!-- RIGHT REGISTRATION FORM -->
            <div class="col-lg-8 col-md-7">
                <div class="card p-4 p-md-5 border rounded-3 shadow-none h-100" style="border-color: #e2e8f0 !important; background: #ffffff;">
                    <div class="mb-4">
                        <small class="text-muted fw-bold" style="font-size: 0.75rem; letter-spacing: 0.5px;">STEP 1 OF 1 • PROFILE CREATION</small>
                        <h3 class="fw-bold mt-1 text-dark" style="font-size: 1.85rem;">Create Your Farmer Account</h3>
                        <p class="small text-muted mb-0">Please fill in your authentic farming details. All fields marked with <span class="required-mark">*</span> are required.</p>
                    </div>

                    <asp:Label ID="lblRegMsg" runat="server" CssClass="d-block mb-3 fw-bold"></asp:Label>

                    <div class="row g-3">
                        <!-- Full Name -->
                        <div class="col-md-6">
                            <label class="form-label" style="color: #0b6028;">FULL NAME <span class="required-mark">*</span></label>
                            <asp:TextBox ID="txtFullName" runat="server" CssClass="form-control" 
                                placeholder="e.g. Ramesh Patel"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvFullName" ControlToValidate="txtFullName" ErrorMessage="Full name is required." ForeColor="#dc2626" Display="Dynamic" CssClass="kr-val-error" SetFocusOnError="true" runat="server" />
                        </div>

                        <!-- Mobile Number -->
                        <div class="col-md-6">
                            <label class="form-label" style="color: #0b6028;">MOBILE NUMBER <span class="required-mark">*</span></label>
                            <asp:TextBox ID="txtMobile" runat="server" CssClass="form-control" 
                                placeholder="+91  10-digit mobile number" MaxLength="10" 
                                type="tel"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvMobile" ControlToValidate="txtMobile" ErrorMessage="Mobile number is required." ForeColor="#dc2626" Display="Dynamic" CssClass="kr-val-error" SetFocusOnError="true" runat="server" />
                            <asp:RegularExpressionValidator ID="revMobile" ControlToValidate="txtMobile" ValidationExpression="^[6-9]\d{9}$" ErrorMessage="Please enter a valid 10-digit Indian mobile number." ForeColor="#dc2626" Display="Dynamic" CssClass="kr-val-error" SetFocusOnError="true" runat="server" />
                        </div>

                        <!-- Primary Sowing Crop -->
                        <div class="col-md-6">
                            <label class="form-label" style="color: #0b6028;">PRIMARY CROP <span class="required-mark">*</span></label>
                            <asp:TextBox ID="txtCrop" runat="server" CssClass="form-control" 
                                placeholder="e.g. Groundnut, Cotton, Wheat"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvCrop" ControlToValidate="txtCrop" ErrorMessage="Primary crop is required." ForeColor="#dc2626" Display="Dynamic" CssClass="kr-val-error" SetFocusOnError="true" runat="server" />
                        </div>

                        <!-- Farm Area (Acres) -->
                        <div class="col-md-6">
                            <label class="form-label" style="color: #0b6028;">FARM AREA (ACRES) <span class="required-mark">*</span></label>
                            <asp:TextBox ID="txtArea" runat="server" CssClass="form-control" 
                                placeholder="e.g. 5.5"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvArea" ControlToValidate="txtArea" ErrorMessage="Farm area is required." ForeColor="#dc2626" Display="Dynamic" CssClass="kr-val-error" SetFocusOnError="true" runat="server" />
                            <asp:RangeValidator ID="rngArea" ControlToValidate="txtArea" MinimumValue="0.1" MaximumValue="9999.0" Type="Double" ErrorMessage="Area must be between 0.1 and 9999 acres." ForeColor="#dc2626" Display="Dynamic" CssClass="kr-val-error" SetFocusOnError="true" runat="server" />
                        </div>

                        <!-- Password -->
                        <div class="col-md-6">
                            <label class="form-label" style="color: #0b6028;">CREATE PASSWORD <span class="required-mark">*</span></label>
                            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control" 
                                placeholder="Min 6 characters"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvPassword" ControlToValidate="txtPassword" ErrorMessage="Password is required." ForeColor="#dc2626" Display="Dynamic" CssClass="kr-val-error" SetFocusOnError="true" runat="server" />
                            <asp:RegularExpressionValidator ID="revPassword" ControlToValidate="txtPassword" ValidationExpression="^.{6,}$" ErrorMessage="Password must be at least 6 characters." ForeColor="#dc2626" Display="Dynamic" CssClass="kr-val-error" SetFocusOnError="true" runat="server" />
                        </div>

                        <!-- Confirm Password -->
                        <div class="col-md-6">
                            <label class="form-label" style="color: #0b6028;">CONFIRM PASSWORD <span class="required-mark">*</span></label>
                            <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password" CssClass="form-control" 
                                placeholder="Re-enter password"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvConfirmPassword" ControlToValidate="txtConfirmPassword" ErrorMessage="Please confirm password." ForeColor="#dc2626" Display="Dynamic" CssClass="kr-val-error" SetFocusOnError="true" runat="server" />
                            <asp:CompareValidator ID="cmpPassword" ControlToValidate="txtConfirmPassword" ControlToCompare="txtPassword" Operator="Equal" ErrorMessage="Passwords do not match." ForeColor="#dc2626" Display="Dynamic" CssClass="kr-val-error" SetFocusOnError="true" runat="server" />
                        </div>

                        <!-- Farming Interests & Services -->
                        <div class="col-12 pt-2">
                            <label class="form-label text-dark mb-2">FARMING INTERESTS & SERVICES</label>
                            <div class="d-flex flex-wrap gap-4 pt-1">
                                <div class="form-check">
                                    <asp:CheckBox ID="chkSell" runat="server" Checked="true" Text=" Sell Harvest" CssClass="form-check-label fw-semibold" />
                                </div>
                                <div class="form-check">
                                    <asp:CheckBox ID="chkApmc" runat="server" Checked="true" Text=" Check APMC Mandi Rates" CssClass="form-check-label fw-semibold" />
                                </div>
                                <div class="form-check">
                                    <asp:CheckBox ID="chkTractor" runat="server" Checked="true" Text=" Tractor & Harvester Booking" CssClass="form-check-label fw-semibold" />
                                </div>
                            </div>
                        </div>

                        <!-- Upload Land 7/12 ROR Document -->
                        <div class="col-12 pt-2">
                            <label class="form-label text-muted mb-2">LAND 7/12 ROR / AADHAAR VERIFICATION (OPTIONAL)</label>
                            <div class="border border-dashed rounded-3 p-3 text-center" style="border-color: #cbd5e1 !important; background-color: #f8fafc;">
                                <p class="small text-muted mb-2 fw-semibold">Upload 7/12 land document or government ID for verified farmer badge</p>
                                <asp:FileUpload ID="fuDoc" runat="server" CssClass="form-control form-control-sm mx-auto" style="max-width: 320px;" />
                            </div>
                        </div>

                        <!-- Submit Button -->
                        <div class="col-12 pt-3">
                            <asp:Button ID="btnSubmitFarmer" runat="server" Text="Create Farmer Account" 
                                CssClass="btn w-100 py-3 fw-bold text-white shadow-sm" 
                                OnClick="btnSubmitFarmer_Click" 
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
