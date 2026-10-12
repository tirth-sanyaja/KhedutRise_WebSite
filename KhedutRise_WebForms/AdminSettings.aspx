<%@ Page Title="Admin Settings" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="AdminSettings.aspx.cs" Inherits="KhedutRise.AdminSettings" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Admin Settings: Platform configuration, Moderation policies, and Security -->
    <div class="kr-page-container">
        <!-- Header -->
        <div class="mb-4">
            <small class="text-muted"><a href="Default.aspx" class="text-decoration-none text-muted">Home</a> / <a href="AdminDashboard.aspx" class="text-decoration-none text-muted">Admin Portal</a> / Platform Settings</small>
            <h2 class="fw-bold mt-1 text-dark" style="font-size: 2rem;">Platform Settings & Configuration</h2>
            <p class="text-muted small mb-0">Manage moderation policies, platform commission fees, notifications, and security controls.</p>
        </div>

        <div class="row g-4">
            <!-- LEFT: Dark Navy Navigation Sidebar ( -->
            <div class="col-lg-3">
                <div class="p-4 rounded-3 text-white h-100" style="background-color: #0f172a;">
                    <ul class="list-unstyled mb-0 d-flex flex-column gap-3 py-2" style="font-size: 0.95rem;">
                        <li>
                            <a href="AdminDashboard.aspx" class="text-decoration-none text-white d-block opacity-90">
                                <i class="bi bi-speedometer2 me-2"></i>Dashboard
                            </a>
                        </li>
                        <li>
                            <a href="AdminManagement.aspx?tab=Farmers" class="text-decoration-none text-white d-block opacity-90">
                                <i class="bi bi-people me-2"></i>Farmers List
                            </a>
                        </li>
                        <li>
                            <a href="AdminManagement.aspx?tab=Buyers" class="text-decoration-none text-white d-block opacity-90">
                                <i class="bi bi-shop me-2"></i>Buyers List
                            </a>
                        </li>
                        <li>
                            <a href="AdminManagement.aspx?tab=Moderation" class="text-decoration-none text-white d-block opacity-90">
                                <i class="bi bi-shield-check me-2"></i>Moderation
                            </a>
                        </li>
                        <li>
                            <a href="AdminAnalytics.aspx" class="text-decoration-none text-white d-block opacity-90">
                                <i class="bi bi-graph-up me-2"></i>Analytics
                            </a>
                        </li>
                        <li>
                            <a href="AdminSettings.aspx" class="text-decoration-none fw-bold d-block" style="color: #22c55e;">
                                <i class="bi bi-gear-fill me-2"></i>• Settings
                            </a>
                        </li>
                    </ul>
                </div>
            </div>

            <!-- RIGHT: Settings Form Cards -->
            <div class="col-lg-9">
                <!-- Action Alert Banner -->
                <asp:Panel ID="pnlSettingsAlert" runat="server" Visible="false" CssClass="alert alert-success alert-dismissible fade show p-3 mb-4 rounded-3 shadow-sm" role="alert">
                    <div class="d-flex align-items-center">
                        <i class="bi bi-check-circle-fill fs-5 me-2"></i>
                        <div>
                            <asp:Label ID="lblSettingsMsg" runat="server" CssClass="fw-bold"></asp:Label>
                        </div>
                    </div>
                    <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
                </asp:Panel>

                <!-- 1. GENERAL PLATFORM CONFIGURATION -->
                <div class="card card-custom p-4 mb-4">
                    <div class="d-flex align-items-center mb-3">
                        <div class="rounded-circle d-flex align-items-center justify-content-center me-3" style="width: 40px; height: 40px; background-color: #f0fdf4; color: #0b6028;">
                            <i class="bi bi-building fs-5"></i>
                        </div>
                        <div>
                            <h5 class="fw-bold mb-0 text-dark">General Platform Configuration</h5>
                            <small class="text-muted">Basic system identity and contact details</small>
                        </div>
                    </div>
                    <hr class="my-3 text-muted" />

                    <div class="row g-3">
                        <div class="col-md-6">
                            <label class="form-label small fw-bold" style="color: #0b6028; font-size: 0.75rem;">PORTAL NAME</label>
                            <asp:TextBox ID="txtPortalName" runat="server" CssClass="form-control" Text="KhedutRise - Smart Agriculture Ecosystem" required="required"></asp:TextBox>
                        </div>
                        <div class="col-md-6">
                            <label class="form-label small fw-bold" style="color: #0b6028; font-size: 0.75rem;">OFFICIAL ADMIN EMAIL</label>
                            <asp:TextBox ID="txtAdminEmail" runat="server" type="email" CssClass="form-control" Text="admin@khedutrise.com" required="required"></asp:TextBox>
                        </div>
                        <div class="col-md-4">
                            <label class="form-label small fw-bold" style="color: #0b6028; font-size: 0.75rem;">SUPPORT HELPLINE</label>
                            <asp:TextBox ID="txtHelpline" runat="server" CssClass="form-control" Text="+91 1800 233 5555" required="required"></asp:TextBox>
                        </div>
                        <div class="col-md-4">
                            <label class="form-label small fw-bold" style="color: #0b6028; font-size: 0.75rem;">MARKET COMMISSION FEE (%)</label>
                            <asp:TextBox ID="txtCommissionFee" runat="server" type="number" step="0.01" min="0" max="100" CssClass="form-control" Text="1.50" required="required"></asp:TextBox>
                        </div>
                        <div class="col-md-4">
                            <label class="form-label small fw-bold" style="color: #0b6028; font-size: 0.75rem;">PRIMARY APMC MARKET</label>
                            <asp:DropDownList ID="ddlPrimaryApmc" runat="server" CssClass="form-select">
                                <asp:ListItem Text="Unjha APMC (Main Mandi)" Value="Unjha" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="Rajkot APMC" Value="Rajkot"></asp:ListItem>
                                <asp:ListItem Text="Gondal APMC" Value="Gondal"></asp:ListItem>
                                <asp:ListItem Text="Ahmedabad APMC" Value="Ahmedabad"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>

                <!-- 2. APPROVAL & MODERATION POLICIES -->
                <div class="card card-custom p-4 mb-4">
                    <div class="d-flex align-items-center mb-3">
                        <div class="rounded-circle d-flex align-items-center justify-content-center me-3" style="width: 40px; height: 40px; background-color: #eff6ff; color: #0284c7;">
                            <i class="bi bi-shield-check fs-5"></i>
                        </div>
                        <div>
                            <h5 class="fw-bold mb-0 text-dark">Moderation & Approval Policies</h5>
                            <small class="text-muted">Configure automatic approval criteria and notification triggers</small>
                        </div>
                    </div>
                    <hr class="my-3 text-muted" />

                    <div class="d-flex flex-column gap-3">
                        <div class="form-check form-switch d-flex justify-content-between align-items-center ps-0">
                            <div>
                                <label class="form-check-label fw-bold text-dark mb-0" for="chkAutoApproveFarmers">
                                    Auto-Approve Farmers with Valid 7/12 Land Records
                                </label>
                                <div class="text-muted small">Automatically mark farmer registrations as Approved when document hash verification passes.</div>
                            </div>
                            <asp:CheckBox ID="chkAutoApproveFarmers" runat="server" Checked="true" CssClass="form-check-input ms-3" />
                        </div>

                        <div class="form-check form-switch d-flex justify-content-between align-items-center ps-0">
                            <div>
                                <label class="form-check-label fw-bold text-dark mb-0" for="chkGstRequired">
                                    Require Mandatory GST Verification for Bulk Grain Buyers
                                </label>
                                <div class="text-muted small">Buyers must provide valid GSTIN before placing bulk crop bids above ₹1 Lakh.</div>
                            </div>
                            <asp:CheckBox ID="chkGstRequired" runat="server" Checked="true" CssClass="form-check-input ms-3" />
                        </div>

                        <div class="form-check form-switch d-flex justify-content-between align-items-center ps-0">
                            <div>
                                <label class="form-check-label fw-bold text-dark mb-0" for="chkAutoApproveCrops">
                                    Auto-Approve Verified Farmer Crop Listings under ₹50,000
                                </label>
                                <div class="text-muted small">Allows verified farmers to publish small-to-medium harvest batches directly to the marketplace.</div>
                            </div>
                            <asp:CheckBox ID="chkAutoApproveCrops" runat="server" Checked="false" CssClass="form-check-input ms-3" />
                        </div>

                        <div class="form-check form-switch d-flex justify-content-between align-items-center ps-0">
                            <div>
                                <label class="form-check-label fw-bold text-dark mb-0" for="chkSmsNotification">
                                    Send Instant SMS Alerts to Users on Approval / Rejection
                                </label>
                                <div class="text-muted small">Notify farmers and buyers instantly via SMS when admin approves or rejects their account.</div>
                            </div>
                            <asp:CheckBox ID="chkSmsNotification" runat="server" Checked="true" CssClass="form-check-input ms-3" />
                        </div>
                    </div>
                </div>

                <!-- 3. SYSTEM MAINTENANCE & DATABASE TOOLS -->
                <div class="card card-custom p-4 mb-4">
                    <div class="d-flex align-items-center mb-3">
                        <div class="rounded-circle d-flex align-items-center justify-content-center me-3" style="width: 40px; height: 40px; background-color: #fff7ed; color: #ea580c;">
                            <i class="bi bi-database-gear fs-5"></i>
                        </div>
                        <div>
                            <h5 class="fw-bold mb-0 text-dark">System Security & Database Maintenance</h5>
                            <small class="text-muted">Maintenance mode, session timeouts, and database backups</small>
                        </div>
                    </div>
                    <hr class="my-3 text-muted" />

                    <div class="row g-3 align-items-center mb-4">
                        <div class="col-md-6">
                            <label class="form-label small fw-bold" style="color: #0b6028; font-size: 0.75rem;">ADMIN SESSION TIMEOUT</label>
                            <asp:DropDownList ID="ddlSessionTimeout" runat="server" CssClass="form-select">
                                <asp:ListItem Text="15 Minutes" Value="15"></asp:ListItem>
                                <asp:ListItem Text="30 Minutes (Recommended)" Value="30" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="1 Hour" Value="60"></asp:ListItem>
                                <asp:ListItem Text="4 Hours" Value="240"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-6">
                            <div class="form-check form-switch pt-3">
                                <asp:CheckBox ID="chkMaintenanceMode" runat="server" Checked="false" CssClass="form-check-input" />
                                <label class="form-check-label fw-bold text-dark ms-2" for="chkMaintenanceMode">
                                    Enable Maintenance Mode (System Downtime)
                                </label>
                                <div class="text-muted small ps-2">When enabled, only administrators can access the portal.</div>
                            </div>
                        </div>
                    </div>

                    <div class="p-3 rounded-3 bg-light border d-flex flex-wrap align-items-center justify-content-between gap-2">
                        <div>
                            <strong class="text-dark d-block"><i class="bi bi-database-check text-success me-1"></i>Database Maintenance & Snapshots</strong>
                            <small class="text-muted">Take immediate backup of SQL Server database `KhedutRiseDB`</small>
                        </div>
                        <div class="d-flex gap-2">
                            <asp:Button ID="btnBackupDb" runat="server" Text="Backup Database Now" CssClass="btn btn-outline-dark btn-sm fw-semibold" formnovalidate="formnovalidate" OnClick="btnBackupDb_Click" />
                            <asp:Button ID="btnClearCache" runat="server" Text="Clear Cache" CssClass="btn btn-outline-secondary btn-sm" formnovalidate="formnovalidate" OnClick="btnClearCache_Click" />
                        </div>
                    </div>
                </div>

                <!-- SAVE BUTTONS -->
                <div class="d-flex justify-content-end gap-2 mb-4">
                    <asp:Button ID="btnResetDefaults" runat="server" Text="Reset to Defaults" CssClass="btn btn-light px-4 fw-semibold text-secondary" formnovalidate="formnovalidate" OnClick="btnResetDefaults_Click" />
                    <asp:Button ID="btnSaveSettings" runat="server" Text="Save Platform Settings" CssClass="btn text-white fw-bold px-4" style="background-color: #0b6028; border-radius: 6px; border: none;" OnClick="btnSaveSettings_Click" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
