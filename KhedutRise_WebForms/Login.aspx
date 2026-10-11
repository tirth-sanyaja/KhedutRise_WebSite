<%@ Page Title="Sign In" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="KhedutRise.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Simple, Clean Mobile OTP Login Screen -->
    <style>
        .role-pill-group {
            display: flex;
            background-color: #f1f5f9;
            border-radius: 8px;
            padding: 4px;
            gap: 4px;
        }
        .role-pill-group .form-check {
            flex: 1;
            margin: 0;
            padding: 0;
        }
        .role-pill-group input[type="radio"] {
            display: none;
        }
        .role-pill-group label {
            display: block;
            width: 100%;
            text-align: center;
            padding: 0.55rem 0.75rem;
            font-weight: 700;
            font-size: 0.92rem;
            color: #475569;
            cursor: pointer;
            border-radius: 6px;
            margin: 0;
            transition: all 0.15s ease;
        }
        .role-pill-group input[type="radio"]:checked + label {
            background-color: #0b6028;
            color: #ffffff;
            box-shadow: 0 1px 3px rgba(0,0,0,0.1);
        }
    </style>

    <div class="kr-page-container py-5">
        <div class="row g-4 align-items-stretch justify-content-center">
            <!-- LEFT HERO BANNER -->
            <div class="col-lg-5 col-md-6 d-none d-md-block">
                <div class="card border-0 rounded-4 overflow-hidden position-relative text-white shadow-sm h-100" style="min-height: 520px; background: linear-gradient(to bottom, rgba(0,0,0,0.15) 0%, rgba(0,0,0,0.8) 100%), url('Images/hero_farmer.jpg') center/cover no-repeat;">
                    <div class="p-4 p-lg-5 mt-auto" style="text-shadow: 0 2px 8px rgba(0,0,0,0.7);">
                        <span class="badge bg-success mb-2 px-3 py-1">KhedutRise Portal</span>
                        <h3 class="fw-bold mb-1 fs-2">Welcome Back</h3>
                        <p class="small text-white-50 mb-3">Sign in with Mobile OTP to access your dashboard, crop listings, mandi rates, and rental services.</p>
                        <hr class="border-light opacity-50 my-3" />
                        <span class="small text-white-50">Supported Roles: Farmer, Buyer, Machinery Operator</span>
                    </div>
                </div>
            </div>

            <!-- RIGHT LOGIN FORM -->
            <div class="col-lg-5 col-md-6">
                <div class="card p-4 p-md-5 border rounded-3 shadow-none h-100 d-flex flex-column" style="border-color: #e2e8f0 !important; background: #ffffff;">
                    <div class="mb-4">
                        <small class="text-muted fw-bold" style="font-size: 0.75rem; letter-spacing: 0.5px;">ACCOUNT SIGN IN</small>
                        <h3 class="fw-bold mt-1 text-dark" style="font-size: 1.85rem;">Sign In to KhedutRise</h3>
                    </div>

                    <!-- Server Message Alert -->
                    <asp:Label ID="lblLoginMsg" runat="server" CssClass="d-block mb-3 fw-bold"></asp:Label>

                    <!-- Role selector pills -->
                    <div class="mb-4">
                        <label class="form-label" style="color: #0b6028;">SELECT YOUR ROLE <span class="required-mark">*</span></label>
                        <div class="role-pill-group">
                            <div class="form-check">
                                <asp:RadioButton ID="rbFarmer" runat="server" GroupName="RoleGroup" Text="Farmer" Checked="true" />
                            </div>
                            <div class="form-check">
                                <asp:RadioButton ID="rbBuyer" runat="server" GroupName="RoleGroup" Text="Buyer" />
                            </div>
                            <div class="form-check">
                                <asp:RadioButton ID="rbOperator" runat="server" GroupName="RoleGroup" Text="Operator" />
                            </div>
                        </div>
                    </div>

                    <!-- Mobile Number Field -->
                    <div class="mb-3">
                        <label class="form-label" style="color: #0b6028;">MOBILE NUMBER <span class="required-mark">*</span></label>
                        <div class="input-group">
                            <span class="input-group-text bg-light text-muted fw-semibold">+91</span>
                            <asp:TextBox ID="txtMobile" runat="server" CssClass="form-control" 
                                placeholder="Enter 10-digit mobile number" MaxLength="10" 
                                type="tel" pattern="[6-9][0-9]{9}" required="required"></asp:TextBox>
                        </div>
                    </div>

                    <!-- Separate Send OTP Button -->
                    <div class="mb-3">
                        <asp:Button ID="btnSendOtp" runat="server" Text="Send OTP" CssClass="btn w-100 py-2 fw-bold" 
                            OnClick="btnSendOtp_Click" formnovalidate="formnovalidate"
                            style="background-color: #f0fdf4; color: #0b6028; border: 1.5px solid #0b6028; border-radius: 6px; font-size: 0.95rem;" />
                    </div>

                    <!-- OTP Input -->
                    <div class="mb-4">
                        <div class="d-flex justify-content-between align-items-center mb-1">
                            <label class="form-label mb-0" style="color: #0b6028;">ENTER OTP <span class="required-mark">*</span></label>
                            <small class="text-muted">Test OTP: <strong class="text-dark">482190</strong></small>
                        </div>
                        <asp:TextBox ID="txtOtp" runat="server" CssClass="form-control text-center fw-bold fs-5 py-2 letter-spacing-2" 
                            placeholder="• • • • • •" MaxLength="6" Text="482190"
                            pattern="[0-9]{4,6}" required="required"></asp:TextBox>
                    </div>

                    <!-- Submit OTP Button -->
                    <div class="mb-3">
                        <asp:Button ID="btnLogin" runat="server" Text="Verify & Sign In" 
                            CssClass="btn w-100 py-3 fw-bold text-white shadow-sm" 
                            OnClick="btnLogin_Click" 
                            style="background-color: #0b6028; border-radius: 6px; font-size: 1rem; border: none;" />
                    </div>

                    <!-- Footer Links -->
                    <div class="text-center mt-auto pt-3 border-top small">
                        <span class="text-muted">New to KhedutRise? </span>
                        <a href="SelectRole.aspx" class="fw-bold text-decoration-none" style="color: #0b6028;">Register Account</a>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
