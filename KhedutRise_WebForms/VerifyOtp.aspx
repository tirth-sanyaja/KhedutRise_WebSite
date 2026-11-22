<%@ Page Title="Verify Email OTP" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="VerifyOtp.aspx.cs" Inherits="KhedutRise.VerifyOtp" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container py-5" style="max-width: 540px;">
        <div class="card border-0 shadow-sm rounded-4 overflow-hidden">
            <!-- Header Banner -->
            <div class="p-4 text-center text-white" style="background: linear-gradient(135deg, #0b6028 0%, #15803d 100%);">
                <div class="d-inline-flex align-items-center justify-content-center bg-white text-success rounded-circle shadow-sm mb-3" style="width: 64px; height: 64px;">
                    <i class="bi bi-shield-lock-fill fs-2" style="color: #0b6028;"></i>
                </div>
                <h4 class="fw-bold mb-1">Email Verification</h4>
                <p class="small text-white-50 mb-0">Two-Step Security Authentication</p>
            </div>

            <!-- Body Content -->
            <div class="card-body p-4 p-md-5 bg-white">
                <asp:Label ID="lblVerifyMsg" runat="server" EnableViewState="false"></asp:Label>

                <div class="text-center mb-4">
                    <span id="badgePurpose" runat="server" class="badge rounded-pill bg-light text-success border px-3 py-2 fw-semibold mb-2" style="font-size: 0.82rem;">
                        <i class="bi bi-patch-check-fill me-1 text-success"></i> Account Verification
                    </span>
                    <p class="text-muted small mb-1">
                        We have sent a 6-digit verification code to:
                    </p>
                    <div class="fw-bold text-dark fs-6" id="divTargetEmail" runat="server">
                        user@example.com
                    </div>
                    <small class="text-muted d-block mt-1" style="font-size: 0.78rem;">
                        <i class="bi bi-clock-history me-1"></i> Code expires in <b>5 minutes</b>. Please check your inbox and spam folder.
                    </small>
                </div>

                <!-- OTP Input Form -->
                <div class="mb-4">
                    <label class="form-label fw-bold small text-muted text-uppercase text-center d-block" style="letter-spacing: 0.5px;">
                        Enter 6-Digit Verification Code
                    </label>
                    <asp:TextBox ID="txtOtpCode" runat="server" CssClass="form-control text-center fw-bold text-success fs-3 py-2 shadow-sm"
                        MaxLength="6" autocomplete="one-time-code" inputmode="numeric" placeholder="------"
                        style="letter-spacing: 0.6rem; border: 2px solid #cbd5e1; border-radius: 10px; font-family: 'Courier New', Courier, monospace;" />
                    <asp:RequiredFieldValidator ID="rfvOtp" runat="server" ControlToValidate="txtOtpCode"
                        ErrorMessage="Please enter the 6-digit OTP." Display="Dynamic" CssClass="text-danger small mt-1 d-block text-center"
                        ValidationGroup="OtpVerify" />
                    <asp:RegularExpressionValidator ID="revOtp" runat="server" ControlToValidate="txtOtpCode"
                        ValidationExpression="^\d{6}$" ErrorMessage="OTP must be exactly 6 numeric digits."
                        Display="Dynamic" CssClass="text-danger small mt-1 d-block text-center"
                        ValidationGroup="OtpVerify" />
                </div>

                <!-- Submit Button -->
                <asp:Button ID="btnConfirmOtp" runat="server" Text="Verify & Complete"
                    CssClass="btn btn-success w-100 py-2 fw-bold shadow-sm mb-3"
                    ValidationGroup="OtpVerify" OnClick="btnConfirmOtp_Click"
                    style="background-color: #0b6028; border-color: #0b6028; font-size: 1rem; border-radius: 8px;" />

                <!-- Resend & Cooldown Section -->
                <div class="text-center pt-2 border-top">
                    <div id="divCooldown" class="small text-muted mb-2">
                        <span>Didn't receive the email?</span><br />
                        <span id="spnTimer" class="fw-semibold text-secondary">
                            Resend available in <b id="countdownSec" class="text-dark">60</b> seconds
                        </span>
                    </div>

                    <asp:LinkButton ID="btnResendOtp" runat="server" CssClass="btn btn-outline-secondary btn-sm px-3 fw-semibold"
                        OnClick="btnResendOtp_Click" CausesValidation="false" style="border-radius: 6px; display: none;">
                        <i class="bi bi-arrow-repeat me-1"></i> Resend Verification Code
                    </asp:LinkButton>
                </div>

                <!-- Back to Login / Registration link -->
                <div class="text-center mt-4">
                    <asp:HyperLink ID="hlBack" runat="server" NavigateUrl="~/Login.aspx" CssClass="small text-muted text-decoration-none">
                        <i class="bi bi-arrow-left me-1"></i> Back to Sign In
                    </asp:HyperLink>
                </div>
            </div>

            <!-- Security Footer -->
            <div class="card-footer bg-light p-3 text-center border-0">
                <small class="text-muted" style="font-size: 0.74rem;">
                    <i class="bi bi-shield-check text-success me-1"></i> <b>KhedutRise Security:</b> We never share your credentials or ask for your OTP over phone/chat.
                </small>
            </div>
        </div>
    </div>

    <asp:HiddenField ID="hfCooldownRemaining" runat="server" Value="60" />
    <asp:HiddenField ID="hfPurpose" runat="server" Value="REGISTER" />
    <asp:HiddenField ID="hfEmail" runat="server" Value="" />
    <asp:HiddenField ID="hfIdent" runat="server" Value="" />
    <asp:HiddenField ID="hfRole" runat="server" Value="" />

    <script type="text/javascript">
        (function () {
            var hfCooldown = document.getElementById('<%= hfCooldownRemaining.ClientID %>');
            var spnTimer = document.getElementById('spnTimer');
            var countdownSec = document.getElementById('countdownSec');
            var btnResend = document.getElementById('<%= btnResendOtp.ClientID %>');

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

            // Auto-focus OTP input box
            var txtOtp = document.getElementById('<%= txtOtpCode.ClientID %>');
            if (txtOtp) {
                txtOtp.focus();
                txtOtp.addEventListener('input', function () {
                    this.value = this.value.replace(/[^0-9]/g, '');
                });
            }

            updateTimer();
        })();
    </script>
</asp:Content>
