<%@ Page Title="Admin Login" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="AdminLogin.aspx.cs" Inherits="KhedutRise.AdminLogin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .admin-login-wrapper {
            min-height: calc(100vh - 120px);
            display: flex;
            align-items: center;
            justify-content: center;
            padding: 2.5rem 1rem;
            background: #f1f5f9;
        }
        .admin-auth-card {
            max-width: 980px;
            width: 100%;
            background: #ffffff;
            border-radius: 20px;
            overflow: hidden;
            border: 1px solid #e2e8f0;
            box-shadow: 0 20px 40px -15px rgba(15, 23, 42, 0.12);
        }
        .admin-auth-brand {
            background: linear-gradient(150deg, #0b1329 0%, #0f172a 60%, #1e293b 100%);
            color: #ffffff;
            padding: 3.5rem 3rem;
            position: relative;
            display: flex;
            flex-direction: column;
            justify-content: space-between;
        }
        .admin-auth-brand::before {
            content: '';
            position: absolute;
            top: -50px;
            right: -50px;
            width: 220px;
            height: 220px;
            background: radial-gradient(circle, rgba(34, 197, 94, 0.18) 0%, rgba(34, 197, 94, 0) 70%);
            border-radius: 50%;
            pointer-events: none;
        }
        .admin-auth-form {
            padding: 3.5rem 3rem;
            background: #ffffff;
            display: flex;
            flex-direction: column;
            justify-content: center;
        }
        .brand-badge {
            display: inline-flex;
            align-items: center;
            gap: 0.5rem;
            background: rgba(34, 197, 94, 0.12);
            color: #4ade80;
            border: 1px solid rgba(34, 197, 94, 0.25);
            padding: 0.4rem 0.85rem;
            border-radius: 50px;
            font-size: 0.75rem;
            font-weight: 700;
            letter-spacing: 0.5px;
            text-transform: uppercase;
        }
        .quote-box {
            background: rgba(255, 255, 255, 0.04);
            border-left: 3px solid #22c55e;
            padding: 1rem 1.25rem;
            border-radius: 0 10px 10px 0;
            margin: 2rem 0;
        }
        .form-floating-label {
            font-size: 0.74rem;
            font-weight: 700;
            text-transform: uppercase;
            letter-spacing: 0.6px;
            color: #0b6028;
            margin-bottom: 0.35rem;
            display: block;
        }
        .admin-form-input {
            border: 1px solid #cbd5e1;
            border-radius: 8px;
            padding: 0.75rem 1rem;
            font-size: 0.95rem;
            transition: all 0.2s ease;
        }
        .admin-form-input:focus {
            border-color: #22c55e;
            box-shadow: 0 0 0 3px rgba(34, 197, 94, 0.18);
            outline: none;
        }
        .btn-admin-submit {
            background: linear-gradient(135deg, #0b6028 0%, #15803d 100%);
            border: none;
            color: #ffffff;
            font-weight: 700;
            font-size: 1rem;
            padding: 0.85rem 1.5rem;
            border-radius: 8px;
            letter-spacing: 0.3px;
            transition: all 0.2s ease;
            box-shadow: 0 4px 12px rgba(11, 96, 40, 0.25);
        }
        .btn-admin-submit:hover {
            background: linear-gradient(135deg, #08481e 0%, #166534 100%);
            transform: translateY(-1px);
            box-shadow: 0 6px 16px rgba(11, 96, 40, 0.35);
            color: #ffffff;
        }
        @media (max-width: 991.98px) {
            .admin-auth-brand {
                padding: 2.25rem 2rem;
            }
            .admin-auth-form {
                padding: 2.25rem 2rem;
            }
        }
    </style>

    <div class="admin-login-wrapper">
        <div class="admin-auth-card">
            <div class="row g-0">
                <!-- Left Panel: Dark Brand Showcase -->
                <div class="col-lg-5 admin-auth-brand">
                    <div>
                        <div class="d-flex align-items-center gap-2 mb-4">
                            <div class="rounded-circle d-flex align-items-center justify-content-center" style="width: 44px; height: 44px; background: rgba(34, 197, 94, 0.2); color: #4ade80;">
                                <i class="bi bi-shield-lock-fill fs-4"></i>
                            </div>
                            <div>
                                <h5 class="fw-bold text-white mb-0" style="letter-spacing: -0.3px;">
                                    <span style="color: #4ade80;">Khedut</span><span style="color: #22c55e;">Rise</span> Pro
                                </h5>
                                <small class="text-white-50" style="font-size: 0.72rem;">Unified Administrative Portal</small>
                            </div>
                        </div>

                        <div class="brand-badge mb-3">
                            <i class="bi bi-patch-check-fill"></i> Enterprise Governance
                        </div>

                        <h3 class="fw-bold text-white mb-2" style="font-size: 1.65rem; line-height: 1.3;">
                            Empowering Gujarat Agriculture with Modern Intelligence
                        </h3>

                        <div class="quote-box">
                            <p class="text-white-50 small mb-0 fst-italic" style="line-height: 1.6;">
                                "Data-driven governance, verified producer security, and transparent APMC market connectivity across all 33 districts."
                            </p>
                        </div>

                        <div class="d-flex flex-column gap-2 mt-4">
                            <div class="d-flex align-items-center gap-2 text-white-50 small">
                                <i class="bi bi-shield-check text-success fs-6"></i>
                                <span>Multi-tier Role-Based Access Control</span>
                            </div>
                            <div class="d-flex align-items-center gap-2 text-white-50 small">
                                <i class="bi bi-database-check text-success fs-6"></i>
                                <span>Real-time Mandi & Escrow Transaction Telemetry</span>
                            </div>
                            <div class="d-flex align-items-center gap-2 text-white-50 small">
                                <i class="bi bi-lock-fill text-success fs-6"></i>
                                <span>End-to-End Cryptographic Session Security</span>
                            </div>
                        </div>
                    </div>

                    <div class="mt-4 pt-3 border-top border-secondary border-opacity-25 d-flex align-items-center justify-content-between text-white-50" style="font-size: 0.75rem;">
                        <span>&copy; 2026 KhedutRise Platform</span>
                        <span class="badge bg-secondary bg-opacity-25 text-white-50">v4.8 Pro</span>
                    </div>
                </div>

                <!-- Right Panel: Login Form -->
                <div class="col-lg-7 admin-auth-form">
                    <div class="mb-4">
                        <div class="d-inline-flex align-items-center gap-2 mb-2">
                            <span class="badge bg-success-subtle text-success px-2 py-1 rounded-pill small fw-bold">RESTRICTED ACCESS</span>
                        </div>
                        <h2 class="fw-bold text-dark mb-1" style="font-size: 1.85rem; letter-spacing: -0.5px;">Sign In to Admin Console</h2>
                        <p class="text-muted small mb-0">Authorized personnel only. Protected administrative environment.</p>
                    </div>

                    <asp:Label ID="lblAdminMsg" runat="server" CssClass="d-block mb-3 fw-bold"></asp:Label>

                    <!-- Default Credential Helper Badge -->
                    <div class="alert alert-light border rounded-3 d-flex align-items-center py-2 px-3 mb-4" style="background-color: #f8fafc; border-color: #e2e8f0; font-size: 0.85rem;">
                        <div class="rounded-circle bg-success-subtle text-success d-flex align-items-center justify-content-center me-3 flex-shrink-0" style="width: 32px; height: 32px;">
                            <i class="bi bi-key-fill"></i>
                        </div>
                        <div class="flex-grow-1">
                            <span class="text-muted d-block small fw-semibold">Default System Credentials</span>
                            <div class="d-flex flex-wrap gap-3 mt-1">
                                <span><strong>Email:</strong> <code class="text-dark">admin@khedutrise.com</code></span>
                                <span><strong>Pass:</strong> <code class="text-dark">admin123</code></span>
                            </div>
                        </div>
                    </div>

                    <asp:HiddenField ID="hfAdminCustomPass" runat="server" />

                    <div class="mb-3">
                        <label class="form-floating-label">ADMIN EMAIL ADDRESS <span class="text-danger">*</span></label>
                        <div class="input-group">
                            <span class="input-group-text bg-light border-end-0 text-muted"><i class="bi bi-envelope"></i></span>
                            <asp:TextBox ID="txtAdminEmail" runat="server" CssClass="form-control admin-form-input border-start-0 ps-0" Text="admin@khedutrise.com" placeholder="admin@khedutrise.com"></asp:TextBox>
                        </div>
                        <asp:RequiredFieldValidator ID="rfvAdminEmail" ControlToValidate="txtAdminEmail" ErrorMessage="Admin email is required." ForeColor="#dc2626" Display="Dynamic" CssClass="kr-val-error" EnableClientScript="false" runat="server" />
                        <asp:RegularExpressionValidator ID="revAdminEmail" ControlToValidate="txtAdminEmail" ValidationExpression="^[\w\.-]+@[\w\.-]+\.\w+$" ErrorMessage="Please enter a valid email address." ForeColor="#dc2626" Display="Dynamic" CssClass="kr-val-error" EnableClientScript="false" runat="server" />
                    </div>

                    <!-- Password -->
                    <div class="mb-4">
                        <div class="d-flex justify-content-between align-items-center mb-1">
                            <label class="form-floating-label mb-0">ADMIN PASSWORD <span class="text-danger">*</span></label>
                            <a href="#" class="small fw-semibold text-decoration-none" style="color: #0b6028; font-size: 0.78rem;" data-bs-toggle="modal" data-bs-target="#modalForgotPassword">Forgot Password?</a>
                        </div>
                        <div class="input-group">
                            <span class="input-group-text bg-light border-end-0 text-muted"><i class="bi bi-lock"></i></span>
                            <asp:TextBox ID="txtAdminPass" runat="server" TextMode="Password" CssClass="form-control admin-form-input border-start-0 ps-0" placeholder="Enter admin password"></asp:TextBox>
                        </div>
                        <asp:RequiredFieldValidator ID="rfvAdminPass" ControlToValidate="txtAdminPass" ErrorMessage="Admin password is required." ForeColor="#dc2626" Display="Dynamic" CssClass="kr-val-error" EnableClientScript="false" runat="server" />
                    </div>

                    <!-- Submit Button -->
                    <div class="mb-3">
                        <asp:Button ID="btnAdminLogin" runat="server" Text="Sign In to Admin Portal" CausesValidation="false" CssClass="btn w-100 btn-admin-submit" OnClick="btnAdminLogin_Click" />
                    </div>

                    <div class="text-center mt-3 text-muted small" style="font-size: 0.78rem;">
                        <i class="bi bi-shield-lock me-1 text-success"></i> Sessions are monitored and encrypted with TLS 1.3
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Forgot Password Modal Popup -->
    <div class="modal fade" id="modalForgotPassword" tabindex="-1" aria-labelledby="modalForgotPasswordLabel" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content border-0 shadow-lg rounded-4 p-2">
                <div class="modal-header border-bottom-0 pb-0">
                    <div class="d-flex align-items-center">
                        <div class="rounded-circle d-flex align-items-center justify-content-center me-3" style="width: 44px; height: 44px; background-color: #f0fdf4; color: #0b6028;">
                            <i class="bi bi-shield-lock fs-5"></i>
                        </div>
                        <div>
                            <h5 class="modal-title fw-bold text-dark" id="modalForgotPasswordLabel" style="font-size: 1.25rem;">Forgot Password</h5>
                            <small class="text-muted">Reset administrator account credentials</small>
                        </div>
                    </div>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body pt-3 pb-2">
                    <div id="resetAlertBox" class="alert alert-success py-2 px-3 small mb-3" style="display: none;"></div>
                    <div id="resetFormArea">
                        <p class="text-muted small mb-3">
                            Enter your registered administrator email address and your new password to reset your login credentials.
                        </p>
                        <div class="mb-3">
                            <label class="form-label" style="color: #0b6028;">ADMINISTRATOR EMAIL <span class="required-mark">*</span></label>
                            <input type="email" id="txtResetEmail" class="form-control py-2" value="admin@khedutrise.com" placeholder="admin@khedutrise.com" />
                        </div>
                        <div class="mb-3">
                            <label class="form-label" style="color: #0b6028;">NEW PASSWORD <span class="required-mark">*</span></label>
                            <input type="password" id="txtNewPass" class="form-control py-2" placeholder="Enter new password (min 6 characters)" />
                        </div>
                    </div>
                </div>
                <div class="modal-footer border-top-0 pt-0">
                    <button type="button" class="btn btn-light px-4 fw-semibold text-secondary" data-bs-dismiss="modal">Cancel</button>
                    <button type="button" id="btnSubmitReset" class="btn text-white fw-bold px-4" style="background-color: #0b6028; border-radius: 6px; border: none;" onclick="handlePasswordReset()">Reset Password</button>
                </div>
            </div>
        </div>
    </div>

    <script>
        function handlePasswordReset() {
            var emailInput = document.getElementById('txtResetEmail');
            var passInput = document.getElementById('txtNewPass');
            if (!emailInput.checkValidity()) {
                emailInput.reportValidity();
                return;
            }
            if (!passInput.checkValidity()) {
                passInput.reportValidity();
                return;
            }
            var email = emailInput.value.trim();
            var newPass = passInput.value.trim();
            var alertBox = document.getElementById('resetAlertBox');
            var formArea = document.getElementById('resetFormArea');
            var submitBtn = document.getElementById('btnSubmitReset');

            // Sync with hidden field and password input on login form
            var hfPass = document.getElementById('<%= hfAdminCustomPass.ClientID %>');
            if (hfPass) {
                hfPass.value = newPass;
            }
            var txtPass = document.getElementById('<%= txtAdminPass.ClientID %>');
            if (txtPass) {
                txtPass.value = newPass;
            }
            var txtEmail = document.getElementById('<%= txtAdminEmail.ClientID %>');
            if (txtEmail && email) {
                txtEmail.value = email;
            }

            alertBox.style.display = 'block';
            alertBox.className = 'alert alert-success py-2 px-3 small mb-3';
            alertBox.innerHTML = '<i class="bi bi-check-circle-fill me-1"></i> <strong>Success!</strong> Password for <b>' + email + '</b> has been updated successfully. You can now login with your new credentials.';
            
            formArea.style.display = 'none';
            submitBtn.style.display = 'none';
        }
    </script>
</asp:Content>