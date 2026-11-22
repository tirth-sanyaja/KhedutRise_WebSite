<%@ Page Title="Forgot Password" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="ForgotPassword.aspx.cs" Inherits="KhedutRise.ForgotPassword" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Premium KhedutRise Password Recovery Screen -->
    <style>
        .reset-card {
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
        .otp-input-field {
            letter-spacing: 0.6rem;
            font-size: 1.4rem;
            font-family: 'Courier New', Courier, monospace;
            font-weight: 800;
        }
    </style>

    <div class="container py-5" style="max-width: 560px;">
        <div class="card reset-card overflow-hidden">
            <!-- Header Banner -->
            <div class="p-4 text-center text-white" style="background: linear-gradient(135deg, #0b6028 0%, #15803d 100%);">
                <div class="d-inline-flex align-items-center justify-content-center bg-white text-success rounded-circle shadow-sm mb-3" style="width: 60px; height: 60px;">
                    <i class="bi bi-key-fill fs-2" style="color: #0b6028;"></i>
                </div>
                <h4 class="fw-bold mb-1">Password Recovery</h4>
                <p class="small text-white-50 mb-0">Secure Email OTP Verification</p>
            </div>

            <!-- Body Content -->
            <div class="card-body p-4 p-md-5">
                <asp:Label ID="lblResetMsg" runat="server" EnableViewState="false"></asp:Label>

                <!-- ================= STEP 1: ENTER REGISTERED EMAIL ================= -->
                <asp:Panel ID="pnlRequestOtp" runat="server">
                    <div class="text-center mb-4">
                        <h5 class="fw-bold text-dark mb-2">Forgot Your Password?</h5>
                        <p class="text-muted small mb-0">
                            Enter your registered email address below. We'll send you a 6-digit verification code to securely reset your password.
                        </p>
                    </div>

                    <div class="mb-4">
                        <label class="form-label fw-semibold small" style="color: #0b6028;">
                            REGISTERED EMAIL ADDRESS <span class="text-danger">*</span>
                        </label>
                        <div class="input-group">
                            <span class="input-group-text bg-light text-success border-end-0"><i class="bi bi-envelope-fill"></i></span>
                            <asp:TextBox ID="txtResetEmail" runat="server" CssClass="form-control border-start-0 ps-2" 
                                placeholder="name@example.com" autocomplete="email"></asp:TextBox>
                        </div>
                        <asp:RequiredFieldValidator ID="rfvResetEmail" ControlToValidate="txtResetEmail" ValidationGroup="EmailRequest"
                            ErrorMessage="Email address is required." ForeColor="#dc2626" Display="Dynamic" 
                            CssClass="small mt-1 text-danger d-block" SetFocusOnError="true" runat="server" />
                        <asp:RegularExpressionValidator ID="revResetEmail" ControlToValidate="txtResetEmail" ValidationGroup="EmailRequest"
                            ValidationExpression="^[\w\.\-]+@[\w\.\-]+\.\w+$" ErrorMessage="Please enter a valid email address."
                            ForeColor="#dc2626" Display="Dynamic" CssClass="small mt-1 text-danger d-block" SetFocusOnError="true" runat="server" />
                    </div>

                    <div class="mb-3">
                        <asp:Button ID="btnSendResetOtp" runat="server" Text="Send Verification Code" 
                            CssClass="btn w-100 py-3 fw-bold text-white shadow-sm" 
                            OnClick="btnSendResetOtp_Click" ValidationGroup="EmailRequest" CausesValidation="true"
                            style="background-color: #0b6028; border-radius: 8px; font-size: 0.95rem; border: none;" />
                    </div>

                    <div class="text-center pt-2">
                        <a href="Login.aspx" class="small text-muted text-decoration-none">
                            <i class="bi bi-arrow-left me-1"></i> Remember your password? Back to Sign In
                        </a>
                    </div>
                </asp:Panel>

                <!-- ================= STEP 2: ENTER OTP & NEW PASSWORD ================= -->
                <asp:Panel ID="pnlVerifyAndReset" runat="server" Visible="false">
                    <div class="text-center mb-4">
                        <span class="badge rounded-pill bg-light text-success border px-3 py-2 fw-semibold mb-2" style="font-size: 0.82rem;">
                            <i class="bi bi-shield-lock-fill me-1 text-success"></i> Verification Code Sent
                        </span>
                        <p class="text-muted small mb-1">
                            We have sent a 6-digit password reset code to:
                        </p>
                        <div class="fw-bold text-dark fs-6" id="divTargetEmail" runat="server">
                            user@example.com
                        </div>
                        <small class="text-muted d-block mt-1" style="font-size: 0.78rem;">
                            Code is valid for <b>15 minutes</b>. Please check your inbox and spam folder.
                        </small>
                    </div>

                    <!-- OTP Code Input -->
                    <div class="mb-3">
                        <label class="form-label fw-bold small text-muted text-uppercase text-center d-block" style="letter-spacing: 0.5px;">
                            Enter 6-Digit Code <span class="text-danger">*</span>
                        </label>
                        <asp:TextBox ID="txtResetOtp" runat="server" CssClass="form-control text-center text-success otp-input-field py-2 shadow-sm"
                            MaxLength="6" autocomplete="one-time-code" inputmode="numeric" placeholder="------" />
                        <asp:RequiredFieldValidator ID="rfvResetOtp" ControlToValidate="txtResetOtp" ValidationGroup="ResetPassAuth"
                            ErrorMessage="Please enter the 6-digit verification code." Display="Dynamic" 
                            CssClass="small mt-1 text-danger d-block text-center" SetFocusOnError="true" runat="server" />
                        <asp:RegularExpressionValidator ID="revResetOtp" ControlToValidate="txtResetOtp" ValidationGroup="ResetPassAuth"
                            ValidationExpression="^\d{6}$" ErrorMessage="Code must be exactly 6 numeric digits."
                            Display="Dynamic" CssClass="small mt-1 text-danger d-block text-center" SetFocusOnError="true" runat="server" />
                    </div>

                    <!-- New Password Input with Show/Hide -->
                    <div class="mb-3">
                        <label class="form-label fw-semibold small" style="color: #0b6028;">
                            NEW PASSWORD <span class="text-danger">*</span>
                        </label>
                        <div class="input-group">
                            <span class="input-group-text bg-light text-success border-end-0"><i class="bi bi-lock-fill"></i></span>
                            <asp:TextBox ID="txtNewPassword" runat="server" TextMode="Password" 
                                CssClass="form-control border-start-0 border-end-0 ps-2" 
                                placeholder="Minimum 6 characters" autocomplete="new-password"></asp:TextBox>
                            <button type="button" class="btn btn-toggle-pwd px-3" onclick="toggleVisibility('<%= txtNewPassword.ClientID %>', 'eyeNew')">
                                <i class="bi bi-eye" id="eyeNew"></i>
                            </button>
                        </div>
                        <asp:RequiredFieldValidator ID="rfvNewPassword" ControlToValidate="txtNewPassword" ValidationGroup="ResetPassAuth"
                            ErrorMessage="New password is required." ForeColor="#dc2626" Display="Dynamic" 
                            CssClass="small mt-1 text-danger d-block" SetFocusOnError="true" runat="server" />
                    </div>

                    <!-- Confirm New Password Input with Show/Hide -->
                    <div class="mb-4">
                        <label class="form-label fw-semibold small" style="color: #0b6028;">
                            CONFIRM NEW PASSWORD <span class="text-danger">*</span>
                        </label>
                        <div class="input-group">
                            <span class="input-group-text bg-light text-success border-end-0"><i class="bi bi-shield-check"></i></span>
                            <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password" 
                                CssClass="form-control border-start-0 border-end-0 ps-2" 
                                placeholder="Re-enter new password" autocomplete="new-password"></asp:TextBox>
                            <button type="button" class="btn btn-toggle-pwd px-3" onclick="toggleVisibility('<%= txtConfirmPassword.ClientID %>', 'eyeConfirm')">
                                <i class="bi bi-eye" id="eyeConfirm"></i>
                            </button>
                        </div>
                        <asp:RequiredFieldValidator ID="rfvConfirmPassword" ControlToValidate="txtConfirmPassword" ValidationGroup="ResetPassAuth"
                            ErrorMessage="Please confirm your new password." ForeColor="#dc2626" Display="Dynamic" 
                            CssClass="small mt-1 text-danger d-block" SetFocusOnError="true" runat="server" />
                        <asp:CompareValidator ID="cvPasswords" ControlToValidate="txtConfirmPassword" ControlToCompare="txtNewPassword"
                            ValidationGroup="ResetPassAuth" ErrorMessage="Passwords do not match." ForeColor="#dc2626" 
                            Display="Dynamic" CssClass="small mt-1 text-danger d-block" SetFocusOnError="true" runat="server" />
                    </div>

                    <!-- Submit Reset Button -->
                    <div class="mb-3">
                        <asp:Button ID="btnSubmitReset" runat="server" Text="Verify & Update Password" 
                            CssClass="btn w-100 py-3 fw-bold text-white shadow-sm" 
                            OnClick="btnSubmitReset_Click" ValidationGroup="ResetPassAuth" CausesValidation="true"
                            style="background-color: #0b6028; border-radius: 8px; font-size: 0.95rem; border: none;" />
                    </div>

                    <!-- Resend OTP & Cooldown -->
                    <div class="text-center pt-2 border-top">
                        <div id="divCooldown" class="small text-muted mb-2">
                            <span>Didn't receive the code?</span><br />
                            <span id="spnTimer" class="fw-semibold text-secondary">
                                Resend available in <b id="countdownSec" class="text-dark">60</b> seconds
                            </span>
                        </div>

                        <asp:LinkButton ID="btnResendResetOtp" runat="server" CssClass="btn btn-outline-secondary btn-sm px-3 fw-semibold"
                            OnClick="btnResendResetOtp_Click" CausesValidation="false" style="border-radius: 6px; display: none;">
                            <i class="bi bi-arrow-repeat me-1"></i> Resend Verification Code
                        </asp:LinkButton>
                    </div>

                    <div class="text-center mt-3">
                        <a href="Login.aspx" class="small text-muted text-decoration-none">
                            <i class="bi bi-arrow-left me-1"></i> Cancel and return to Sign In
                        </a>
                    </div>
                </asp:Panel>
            </div>

            <!-- Footer -->
            <div class="card-footer bg-light p-3 text-center border-0">
                <small class="text-muted" style="font-size: 0.74rem;">
                    <i class="bi bi-shield-check text-success me-1"></i> <b>KhedutRise Security:</b> Password reset requests are logged and monitored.
                </small>
            </div>
        </div>
    </div>

    <asp:HiddenField ID="hfCooldownRemaining" runat="server" Value="60" />
    <asp:HiddenField ID="hfResetEmail" runat="server" Value="" />

    <!-- Client-side Scripts -->
    <script type="text/javascript">
        function toggleVisibility(fieldId, iconId) {
            var f = document.getElementById(fieldId);
            var ico = document.getElementById(iconId);
            if (f) {
                if (f.type === 'password') {
                    f.type = 'text';
                    if (ico) { ico.classList.remove('bi-eye'); ico.classList.add('bi-eye-slash'); }
                } else {
                    f.type = 'password';
                    if (ico) { ico.classList.remove('bi-eye-slash'); ico.classList.add('bi-eye'); }
                }
            }
        }

        (function () {
            var hfCooldown = document.getElementById('<%= hfCooldownRemaining.ClientID %>');
            var spnTimer = document.getElementById('spnTimer');
            var countdownSec = document.getElementById('countdownSec');
            var btnResend = document.getElementById('<%= btnResendResetOtp.ClientID %>');

            var remaining = parseInt(hfCooldown ? hfCooldown.value : "60") || 0;

            function updateTimer() {
                if (remaining > 0) {
                    if (spnTimer) spnTimer.style.display = 'inline';
                    if (countdownSec) countdownSec.innerText = remaining;
                    if (btnResend) btnResend.style.display = 'none';
                    remaining--;
                    if (hfCooldown) hfCooldown.value = remaining;
                    setTimeout(updateTimer, 1000);
                } else {
                    if (spnTimer) spnTimer.style.display = 'none';
                    if (btnResend) {
                        btnResend.style.display = 'inline-block';
                        btnResend.classList.remove('disabled');
                    }
                }
            }

            var txtOtp = document.getElementById('<%= txtResetOtp.ClientID %>');
            if (txtOtp) {
                txtOtp.addEventListener('input', function () {
                    this.value = this.value.replace(/[^0-9]/g, '');
                });
            }

            updateTimer();
        })();
    </script>
</asp:Content>
