<%@ Page Title="Create Buyer Account" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="RegisterBuyer.aspx.cs" Inherits="KhedutRise.RegisterBuyer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Buyer Registration Screen: Clean, Professional & Fully Validated -->
    <div class="kr-page-container py-5">
        <div class="row g-4 align-items-stretch justify-content-center">
            <!-- LEFT BANNER -->
            <div class="col-lg-4 col-md-5 d-none d-md-block">
                <div class="card border-0 rounded-4 overflow-hidden position-relative text-white shadow-sm h-100" style="min-height: 640px; background: linear-gradient(to bottom, rgba(0,0,0,0.15) 0%, rgba(0,0,0,0.8) 100%), url('Images/groundnut_harvest.jpg') center/cover no-repeat;">
                    <div class="p-4 p-lg-5 mt-auto" style="text-shadow: 0 2px 8px rgba(0,0,0,0.7);">
                        <span class="badge bg-primary mb-2 px-3 py-1">Buyer & Trader Portal</span>
                        <h3 class="fw-bold mb-1 fs-2">Source Direct from Farmers</h3>
                        <p class="small text-white-50 mb-3">Connect directly with verified Gujarati farmers. Secure transparent wholesale crop contracts with escrow security.</p>
                        <div class="d-flex flex-column gap-2 small text-white-70 mt-3">
                            <div><i class="bi bi-shield-check text-warning me-2"></i>Verified harvest grade testing</div>
                            <div><i class="bi bi-truck text-warning me-2"></i>Integrated farm logistics</div>
                            <div><i class="bi bi-receipt text-warning me-2"></i>Automated GST invoices</div>
                        </div>
                    </div>
                </div>
            </div>

            <!-- RIGHT REGISTRATION FORM -->
            <div class="col-lg-8 col-md-7">
                <div class="card p-4 p-md-5 border rounded-3 shadow-none h-100" style="border-color: #e2e8f0 !important; background: #ffffff;">
                    <div class="mb-4">
                        <small class="text-muted fw-bold" style="font-size: 0.75rem; letter-spacing: 0.5px;">COMMERCIAL BUYER ONBOARDING</small>
                        <h3 class="fw-bold mt-1 text-dark" style="font-size: 1.85rem;">Create Your Buyer Account</h3>
                        <p class="small text-muted mb-0">Fill in your business details to start placing purchase bids. All fields marked with <span class="required-mark">*</span> are required.</p>
                    </div>

                    <asp:Label ID="lblBuyerMsg" runat="server" CssClass="d-block mb-3 fw-bold"></asp:Label>

                    <div class="row g-3">
                        <!-- Full Name / Business Name -->
                        <div class="col-md-6">
                            <label class="form-label" style="color: #0b6028;">CONTACT / TRADER NAME <span class="required-mark">*</span></label>
                            <asp:TextBox ID="txtFullName" runat="server" CssClass="form-control" 
                                placeholder="e.g. Dev Patel"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvFullName" ControlToValidate="txtFullName" ErrorMessage="Contact / Trader name is required." ForeColor="#dc2626" Display="Dynamic" CssClass="small mt-1 d-block fw-semibold" runat="server" />
                        </div>

                        <!-- Business Mobile -->
                        <div class="col-md-6">
                            <label class="form-label" style="color: #0b6028;">BUSINESS MOBILE NUMBER <span class="required-mark">*</span></label>
                            <asp:TextBox ID="txtMobile" runat="server" CssClass="form-control" 
                                placeholder="+91  10-digit mobile number" MaxLength="10" 
                                type="tel"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvMobile" ControlToValidate="txtMobile" ErrorMessage="Mobile number is required." ForeColor="#dc2626" Display="Dynamic" CssClass="small mt-1 d-block fw-semibold" runat="server" />
                            <asp:RegularExpressionValidator ID="revMobile" ControlToValidate="txtMobile" ValidationExpression="^[6-9]\d{9}$" ErrorMessage="Please enter a valid 10-digit Indian mobile number." ForeColor="#dc2626" Display="Dynamic" CssClass="small mt-1 d-block fw-semibold" runat="server" />
                        </div>

                        <!-- Business Email -->
                        <div class="col-md-6">
                            <label class="form-label" style="color: #0b6028;">BUSINESS EMAIL <span class="required-mark">*</span></label>
                            <asp:TextBox ID="txtEmail" runat="server" type="email" CssClass="form-control" 
                                placeholder="buyer@tradecorp.com"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvEmail" ControlToValidate="txtEmail" ErrorMessage="Business email is required." ForeColor="#dc2626" Display="Dynamic" CssClass="small mt-1 d-block fw-semibold" runat="server" />
                            <asp:RegularExpressionValidator ID="revEmail" ControlToValidate="txtEmail" ValidationExpression="^[\w\.-]+@[\w\.-]+\.\w+$" ErrorMessage="Please enter a valid email address." ForeColor="#dc2626" Display="Dynamic" CssClass="small mt-1 d-block fw-semibold" runat="server" />
                        </div>

                        <!-- Business Type Dropdown -->
                        <div class="col-md-6">
                            <label class="form-label" style="color: #0b6028;">BUSINESS TYPE <span class="required-mark">*</span></label>
                            <asp:DropDownList ID="ddlBizType" runat="server" CssClass="form-select">
                                <asp:ListItem Text="Select Business Type" Value="" />
                                <asp:ListItem Text="Grain Wholesaler" Value="Wholesaler" Selected="True" />
                                <asp:ListItem Text="Oil & Pulse Miller" Value="Miller" />
                                <asp:ListItem Text="Food Processing Industry" Value="Food Processor" />
                                <asp:ListItem Text="Agricultural Exporter" Value="Exporter" />
                                <asp:ListItem Text="Retail Chain" Value="Retailer" />
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvBizType" ControlToValidate="ddlBizType" InitialValue="" ErrorMessage="Please select a business type." ForeColor="#dc2626" Display="Dynamic" CssClass="small mt-1 d-block fw-semibold" runat="server" />
                        </div>

                        <!-- Purchase Interests -->
                        <div class="col-md-6">
                            <label class="form-label" style="color: #0b6028;">COMMODITIES OF INTEREST <span class="required-mark">*</span></label>
                            <asp:TextBox ID="txtInterests" runat="server" CssClass="form-control" 
                                placeholder="e.g. Groundnut, Bt Cotton, Cumin"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvInterests" ControlToValidate="txtInterests" ErrorMessage="Commodities of interest are required." ForeColor="#dc2626" Display="Dynamic" CssClass="small mt-1 d-block fw-semibold" runat="server" />
                        </div>

                        <!-- GSTIN / Trade License -->
                        <div class="col-md-6">
                            <label class="form-label" style="color: #0b6028;">GSTIN / APMC LICENSE NO <span class="required-mark">*</span></label>
                            <asp:TextBox ID="txtGstin" runat="server" CssClass="form-control" 
                                placeholder="24AAAAA0000A1Z5"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvGstin" ControlToValidate="txtGstin" ErrorMessage="GSTIN / License number is required." ForeColor="#dc2626" Display="Dynamic" CssClass="small mt-1 d-block fw-semibold" runat="server" />
                        </div>

                        <!-- Password -->
                        <div class="col-md-6">
                            <label class="form-label" style="color: #0b6028;">CREATE PASSWORD <span class="required-mark">*</span></label>
                            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control" 
                                placeholder="Min 6 characters"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvPassword" ControlToValidate="txtPassword" ErrorMessage="Password is required." ForeColor="#dc2626" Display="Dynamic" CssClass="small mt-1 d-block fw-semibold" runat="server" />
                            <asp:RegularExpressionValidator ID="revPassword" ControlToValidate="txtPassword" ValidationExpression="^.{6,}$" ErrorMessage="Password must be at least 6 characters." ForeColor="#dc2626" Display="Dynamic" CssClass="small mt-1 d-block fw-semibold" runat="server" />
                        </div>

                        <!-- Confirm Password -->
                        <div class="col-md-6">
                            <label class="form-label" style="color: #0b6028;">CONFIRM PASSWORD <span class="required-mark">*</span></label>
                            <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password" CssClass="form-control" 
                                placeholder="Re-enter password"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvConfirmPassword" ControlToValidate="txtConfirmPassword" ErrorMessage="Please confirm password." ForeColor="#dc2626" Display="Dynamic" CssClass="small mt-1 d-block fw-semibold" runat="server" />
                            <asp:CompareValidator ID="cmpPassword" ControlToValidate="txtConfirmPassword" ControlToCompare="txtPassword" Operator="Equal" ErrorMessage="Passwords do not match." ForeColor="#dc2626" Display="Dynamic" CssClass="small mt-1 d-block fw-semibold" runat="server" />
                        </div>

                        <!-- Terms Checkbox -->
                        <div class="col-12 pt-2">
                            <div class="form-check">
                                <asp:CheckBox ID="chkAgree" runat="server" Checked="true" Text=" I agree to KhedutRise escrow transaction terms and Mandi trading rules" CssClass="form-check-label small fw-semibold text-muted" />
                            </div>
                        </div>

                        <!-- Submit Button -->
                        <div class="col-12 pt-3">
                            <asp:Button ID="btnSubmitBuyer" runat="server" Text="Create Buyer Account" 
                                CssClass="btn w-100 py-3 fw-bold text-white shadow-sm" 
                                OnClick="btnSubmitBuyer_Click" 
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
