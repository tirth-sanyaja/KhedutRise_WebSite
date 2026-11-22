<%@ Page Title="Sign In" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="KhedutRise.Login" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Premium KhedutRise Sign In Screen -->
    <style>
        .login-card {
            background: #ffffff;
            border: 1px solid #e2e8f0 !important;
            border-radius: 14px;
            box-shadow: 0 4px 20px -2px rgba(0, 0, 0, 0.05);
        }
        .btn-toggle-pwd {
            background-color: #f8fafc;
            border: 1px solid #cbd5e1;
            border-left: none;
            color: #64748b;
            cursor: pointer;
            transition: color 0.15s ease;
        }
        .btn-toggle-pwd:hover {
            color: #0b6028;
            background-color: #f1f5f9;
        }
        .form-control:focus {
            border-color: #10b981;
            box-shadow: 0 0 0 0.2rem rgba(16, 185, 129, 0.15);
        }
    </style>

    <div class="kr-page-container py-5">
        <div class="row g-4 align-items-stretch justify-content-center">
            <!-- LEFT HERO BANNER -->
            <div class="col-lg-5 col-md-6 d-none d-md-block">
                <div class="card border-0 rounded-4 overflow-hidden position-relative text-white shadow-sm h-100" style="min-height: 520px; background: linear-gradient(to bottom, rgba(0,0,0,0.2) 0%, rgba(0,0,0,0.85) 100%), url('Images/hero_farmer.jpg') center/cover no-repeat;">
                    <div class="p-4 p-lg-5 mt-auto" style="text-shadow: 0 2px 8px rgba(0,0,0,0.7);">
                        <span class="badge bg-success mb-2 px-3 py-1">KhedutRise Portal</span>
                        <h3 class="fw-bold mb-1 fs-2">Welcome Back</h3>
                        <p class="small text-white-50 mb-3">Sign in securely to your KhedutRise account to access live APMC mandi rates, direct harvest contracts, tractor rentals, and escrow protection.</p>
                        <hr class="border-light opacity-50 my-3" />
                        <span class="small text-white-50"><i class="bi bi-shield-check me-1 text-success"></i> Secure Role-Based Authentication</span>
                    </div>
                </div>
            </div>

            <!-- RIGHT LOGIN FORM -->
            <div class="col-lg-5 col-md-6">
                <div class="card p-4 p-md-5 login-card h-100 d-flex flex-column">
                    <div class="mb-4">
                        <small class="text-muted fw-bold" style="font-size: 0.75rem; letter-spacing: 0.5px;">ACCOUNT SIGN IN</small>
                        <h3 class="fw-bold mt-1 text-dark" style="font-size: 1.85rem;">Sign In to <span class="brand-khedut">Khedut</span><span class="brand-rise">Rise</span></h3>
                        <p class="small text-muted mb-0">Enter your registered email and password to access your dashboard.</p>
                    </div>

                    <!-- Server Message Alert -->
                    <asp:Label ID="lblLoginMsg" runat="server" CssClass="d-block mb-3 fw-bold" EnableViewState="false"></asp:Label>

                    <!-- LOGIN CREDENTIALS FORM -->
                    <div class="mb-3">
                        <label class="form-label fw-semibold small" style="color: #0b6028;">
                            REGISTERED EMAIL ADDRESS <span class="text-danger">*</span>
                        </label>
                        <div class="input-group">
                            <span class="input-group-text bg-light text-success border-end-0"><i class="bi bi-envelope-fill"></i></span>
                            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control border-start-0 ps-2" 
                                placeholder="name@example.com" autocomplete="email"></asp:TextBox>
                        </div>
                        <asp:RequiredFieldValidator ID="rfvEmail" ControlToValidate="txtEmail" ValidationGroup="LoginAuth" 
                            ErrorMessage="Email address is required." ForeColor="#dc2626" Display="Dynamic" 
                            CssClass="small mt-1 text-danger d-block" SetFocusOnError="true" runat="server" />
                        <asp:RegularExpressionValidator ID="revEmail" ControlToValidate="txtEmail" ValidationGroup="LoginAuth"
                            ValidationExpression="^[\w\.\-]+@[\w\.\-]+\.\w+$" ErrorMessage="Please enter a valid email address."
                            ForeColor="#dc2626" Display="Dynamic" CssClass="small mt-1 text-danger d-block" SetFocusOnError="true" runat="server" />
                    </div>

                    <!-- Account Password Field with Show/Hide Toggle -->
                    <div class="mb-3">
                        <div class="d-flex justify-content-between align-items-center mb-1">
                            <label class="form-label fw-semibold small mb-0" style="color: #0b6028;">
                                PASSWORD <span class="text-danger">*</span>
                            </label>
                            <a href="ForgotPassword.aspx" class="text-decoration-none small fw-semibold" style="color: #0b6028; font-size: 0.82rem;">
                                Forgot Password?
                            </a>
                        </div>
                        <div class="input-group">
                            <span class="input-group-text bg-light text-success border-end-0"><i class="bi bi-lock-fill"></i></span>
                            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" 
                                CssClass="form-control border-start-0 border-end-0 ps-2" 
                                placeholder="Enter account password" autocomplete="current-password"></asp:TextBox>
                            <button type="button" class="btn btn-toggle-pwd px-3" id="btnTogglePassword" title="Show/Hide Password" onclick="togglePasswordVisibility()">
                                <i class="bi bi-eye" id="eyeIcon"></i>
                            </button>
                        </div>
                        <asp:RequiredFieldValidator ID="rfvPassword" ControlToValidate="txtPassword" ValidationGroup="LoginAuth" 
                            ErrorMessage="Password is required." ForeColor="#dc2626" Display="Dynamic" 
                            CssClass="small mt-1 text-danger d-block" SetFocusOnError="true" runat="server" />
                    </div>

                    <!-- Remember Me Option -->
                    <div class="mb-4 d-flex justify-content-between align-items-center">
                        <div class="form-check">
                            <asp:CheckBox ID="chkRememberMe" runat="server" CssClass="form-check-input" />
                            <label class="form-check-label small text-muted user-select-none" for="<%= chkRememberMe.ClientID %>">
                                Remember my email on this device
                            </label>
                        </div>
                    </div>

                    <!-- Submit Login Button -->
                    <div class="mb-4">
                        <asp:Button ID="btnLogin" runat="server" Text="Sign In" 
                            CssClass="btn w-100 py-3 fw-bold text-white shadow-sm" 
                            OnClick="btnLogin_Click" ValidationGroup="LoginAuth" CausesValidation="true"
                            style="background-color: #0b6028; border-radius: 8px; font-size: 1rem; border: none;" />
                    </div>

                    <!-- Footer Links -->
                    <div class="text-center mt-auto pt-3 border-top small">
                        <span class="text-muted">Don't have an account yet? </span>
                        <a href="SelectRole.aspx" class="fw-bold text-decoration-none" style="color: #0b6028;">
                            Register as Farmer, Buyer or Operator &rarr;
                        </a>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Show/Hide Password Client Script -->
    <script type="text/javascript">
        function togglePasswordVisibility() {
            var pwdField = document.getElementById('<%= txtPassword.ClientID %>');
            var eyeIcon = document.getElementById('eyeIcon');
            if (pwdField) {
                if (pwdField.type === 'password') {
                    pwdField.type = 'text';
                    if (eyeIcon) {
                        eyeIcon.classList.remove('bi-eye');
                        eyeIcon.classList.add('bi-eye-slash');
                    }
                } else {
                    pwdField.type = 'password';
                    if (eyeIcon) {
                        eyeIcon.classList.remove('bi-eye-slash');
                        eyeIcon.classList.add('bi-eye');
                    }
                }
            }
        }
    </script>
</asp:Content>
