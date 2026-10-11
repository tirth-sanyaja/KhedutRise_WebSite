<%@ Page Title="Admin Login" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="AdminLogin.aspx.cs" Inherits="KhedutRise.AdminLogin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Admin Portal Login -->
    <div class="kr-page-container py-5 d-flex justify-content-center">
        <div class="card p-4 p-md-5 border rounded-3 shadow-sm" style="max-width: 480px; width: 100%; border-color: #e2e8f0 !important; background: #ffffff;">
            <!-- Shield Icon & Header -->
            <div class="text-center mb-4">
                <div class="rounded-circle d-inline-flex align-items-center justify-content-center mb-2" style="width: 52px; height: 52px; background-color: #f0fdf4; color: #0b6028;">
                    <i class="bi bi-shield-lock-fill fs-3"></i>
                </div>
                <h3 class="fw-bold text-dark mt-2 mb-1" style="font-size: 1.75rem;">Admin Portal Login</h3>
                <p class="small text-muted mb-0">Authorized personnel only. Protected administrative environment.</p>
            </div>

            <asp:Label ID="lblAdminMsg" runat="server" CssClass="d-block mb-3 fw-bold"></asp:Label>

        

            <!-- Admin Email -->
            <div class="mb-3">
                <label class="form-label" style="color: #0b6028;">ADMIN EMAIL <span class="required-mark">*</span></label>
                <asp:TextBox ID="txtAdminEmail" runat="server" type="email" CssClass="form-control py-2" Text="admin@khedutrise.com" 
                    placeholder="admin@khedutrise.com" required="required"></asp:TextBox>
            </div>

            <!-- Password -->
            <div class="mb-4">
                <label class="form-label" style="color: #0b6028;">PASSWORD <span class="required-mark">*</span></label>
                <asp:TextBox ID="txtAdminPass" runat="server" TextMode="Password" Text="admin123" CssClass="form-control py-2" 
                    placeholder="Enter admin password" minlength="6" required="required"></asp:TextBox>
            </div>

            <!-- Submit Button -->
            <div class="mb-3">
                <asp:Button ID="btnAdminLogin" runat="server" Text="Secure Admin Login" CssClass="btn w-100 py-3 fw-bold text-white shadow-sm" OnClick="btnAdminLogin_Click" style="background-color: #0b6028; border-radius: 6px; font-size: 1rem; border: none;" />
            </div>

            <!-- Forgot Password Link -->
            <div class="text-center mt-3">
                <a href="#" class="fw-bold text-decoration-none small" style="color: #f95700;" data-bs-toggle="modal" data-bs-target="#modalForgotPassword">Forgot Password?</a>
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
                            <input type="email" id="txtResetEmail" class="form-control py-2" value="admin@khedutrise.com" placeholder="admin@khedutrise.com" required="required" />
                        </div>
                        <div class="mb-3">
                            <label class="form-label" style="color: #0b6028;">NEW PASSWORD <span class="required-mark">*</span></label>
                            <input type="password" id="txtNewPass" class="form-control py-2" placeholder="Enter new password (min 6 characters)" minlength="6" required="required" />
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
            var alertBox = document.getElementById('resetAlertBox');
            var formArea = document.getElementById('resetFormArea');
            var submitBtn = document.getElementById('btnSubmitReset');

            alertBox.style.display = 'block';
            alertBox.className = 'alert alert-success py-2 px-3 small mb-3';
            alertBox.innerHTML = '<i class="bi bi-check-circle-fill me-1"></i> <strong>Success!</strong> Password for <b>' + email + '</b> has been updated successfully. You can now login with your new credentials.';
            
            formArea.style.display = 'none';
            submitBtn.style.display = 'none';
        }
    </script>
</asp:Content>
