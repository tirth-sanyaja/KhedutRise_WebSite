<%@ Page Title="Admin Settings" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="AdminSettings.aspx.cs" Inherits="KhedutRise.AdminSettings" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        /* Shared Enterprise Admin Panel Styles */
        .admin-sidebar {
            background: #0f172a;
            color: #ffffff;
            border-right: 1px solid #1e293b;
        }
        .admin-sidebar-brand {
            display: flex;
            align-items: center;
            padding-bottom: 1.25rem;
            margin-bottom: 1.25rem;
            border-bottom: 1px solid #1e293b;
        }
        .admin-sidebar-section-title {
            color: #475569;
            font-size: 0.68rem;
            font-weight: 700;
            text-transform: uppercase;
            letter-spacing: 1.2px;
            margin-bottom: 0.5rem;
            padding-left: 0.5rem;
        }
        .admin-nav-item,
        .admin-sidebar a.d-block {
            color: #94a3b8 !important;
            font-weight: 500;
            font-size: 0.88rem;
            padding: 0.68rem 0.85rem;
            border-radius: 8px;
            display: flex;
            align-items: center;
            text-decoration: none !important;
            transition: all 0.15s ease-in-out;
            margin-bottom: 0.25rem;
            border: 1px solid transparent;
        }
        .admin-nav-item:hover,
        .admin-sidebar a.d-block:hover {
            color: #ffffff !important;
            background: rgba(255, 255, 255, 0.06);
        }
        .admin-nav-item.active,
        .admin-sidebar a.d-block.fw-bold {
            color: #22c55e !important;
            background: rgba(34, 197, 94, 0.12) !important;
            border-color: rgba(34, 197, 94, 0.25) !important;
            font-weight: 600 !important;
        }
        .admin-nav-item i,
        .admin-sidebar a.d-block i {
            font-size: 1.15rem;
            margin-right: 0.75rem;
            display: inline-flex;
            align-items: center;
            justify-content: center;
            width: 22px;
        }
        .admin-sidebar-footer {
            margin-top: auto;
            padding-top: 1.25rem;
            border-top: 1px solid #1e293b;
        }

        /* Top Header Bar */
        .admin-header {
            display: flex;
            flex-wrap: wrap;
            justify-content: space-between;
            align-items: center;
            gap: 1rem;
            margin-bottom: 1.75rem;
            padding-bottom: 1.25rem;
            border-bottom: 1px solid #e2e8f0;
        }
        .admin-title {
            font-size: 1.75rem;
            font-weight: 800;
            color: #0f172a;
            letter-spacing: -0.025em;
            margin: 0;
        }
        .admin-subtitle {
            font-size: 0.85rem;
            color: #64748b;
            margin: 0.25rem 0 0 0;
        }

        /* Modern Cards */
        .admin-card {
            background: #ffffff;
            border: 1px solid #e2e8f0;
            border-radius: 14px;
            box-shadow: 0 1px 3px rgba(0, 0, 0, 0.04);
            overflow: hidden;
            margin-bottom: 1.75rem;
        }
        .admin-card-header {
            padding: 1.25rem 1.5rem;
            border-bottom: 1px solid #f1f5f9;
            background: #ffffff;
            display: flex;
            align-items: center;
        }
        .admin-card-title {
            font-size: 1.15rem;
            font-weight: 700;
            color: #0f172a;
            margin: 0;
        }
        .admin-card-subtitle {
            font-size: 0.8rem;
            color: #64748b;
            margin: 0.2rem 0 0 0;
        }
        .admin-card-body {
            padding: 1.5rem;
        }
        .form-section-label {
            font-size: 0.72rem;
            font-weight: 700;
            text-transform: uppercase;
            letter-spacing: 0.6px;
            color: #0b6028;
            margin-bottom: 0.4rem;
            display: block;
        }
        .admin-form-control {
            border: 1px solid #cbd5e1;
            border-radius: 8px;
            padding: 0.65rem 0.95rem;
            font-size: 0.92rem;
            transition: all 0.15s ease-in-out;
        }
        .admin-form-control:focus {
            border-color: #22c55e;
            box-shadow: 0 0 0 3px rgba(34, 197, 94, 0.15);
            outline: none;
        }
        .policy-toggle-row {
            display: flex;
            justify-content: space-between;
            align-items: center;
            padding: 1rem 0;
            border-bottom: 1px solid #f1f5f9;
        }
        .policy-toggle-row:last-child {
            border-bottom: none;
            padding-bottom: 0;
        }

        /* ── Custom Modern Switch Toggle (Zero Checkbox Overlap) ── */
        .admin-switch {
            position: relative;
            display: inline-block;
            width: 48px;
            height: 26px;
            margin: 0;
            cursor: pointer;
            flex-shrink: 0;
            vertical-align: middle;
            user-select: none;
        }
        /* Completely hide the native checkbox input & any wrapper span without breaking form value */
        .admin-switch input[type="checkbox"],
        .admin-switch input,
        .admin-switch span:not(.admin-switch-slider) input {
            position: absolute !important;
            opacity: 0 !important;
            width: 1px !important;
            height: 1px !important;
            margin: -1px !important;
            padding: 0 !important;
            overflow: hidden !important;
            clip: rect(0, 0, 0, 0) !important;
            border: 0 !important;
            pointer-events: none !important;
        }
        .admin-switch-slider {
            position: absolute;
            cursor: pointer;
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            background-color: #cbd5e1;
            transition: all 0.25s cubic-bezier(0.4, 0, 0.2, 1);
            border-radius: 26px;
            box-shadow: inset 0 1px 2px rgba(0, 0, 0, 0.08);
        }
        .admin-switch-slider:before {
            position: absolute;
            content: "";
            height: 20px;
            width: 20px;
            left: 3px;
            bottom: 3px;
            background-color: #ffffff;
            transition: all 0.25s cubic-bezier(0.4, 0, 0.2, 1);
            border-radius: 50%;
            box-shadow: 0 2px 4px rgba(0, 0, 0, 0.2);
        }
        /* Active / Checked State */
        .admin-switch:has(input:checked) .admin-switch-slider,
        .admin-switch-slider.checked {
            background-color: #16a34a !important;
        }
        .admin-switch:has(input:checked) .admin-switch-slider:before,
        .admin-switch-slider.checked:before {
            transform: translateX(22px);
        }
        .admin-switch:hover .admin-switch-slider {
            filter: brightness(0.96);
        }
        .admin-switch:focus-within .admin-switch-slider {
            box-shadow: 0 0 0 3px rgba(34, 197, 94, 0.25);
        }

        @media (max-width: 991.98px) {
            .admin-layout-container {
                flex-direction: column !important;
            }
            .admin-sidebar {
                width: 100% !important;
                border-right: none !important;
                border-bottom: 1px solid #1e293b;
            }
            .admin-main {
                padding: 1.25rem 1rem !important;
            }
        }
    </style>

    <div class="admin-layout-container" style="display: flex; min-height: calc(100vh - 80px);">
        <!-- Sidebar: fixed 240px wide -->
        <div class="admin-sidebar" style="width: 240px; flex-shrink: 0; background: #0f172a; color: #ffffff; display: flex; flex-direction: column; padding: 1.5rem 1rem; border-right: 1px solid #1e293b;">
            <!-- Brand Section -->
            <div class="admin-sidebar-brand">
                <div class="rounded-circle d-flex align-items-center justify-content-center me-3 flex-shrink-0" style="width: 40px; height: 40px; background: rgba(34, 197, 94, 0.15); color: #22c55e; border: 1px solid rgba(34, 197, 94, 0.3);">
                    <i class="bi bi-shield-check fs-5"></i>
                </div>
                <div style="min-width: 0;">
                    <h6 class="fw-bold text-white mb-0" style="letter-spacing: 0.5px; font-size: 0.95rem;">
                        <span style="color: #4ade80;">Khedut</span><span style="color: #22c55e;">Rise</span> <span class="badge bg-success-subtle text-success ms-1 px-1 py-0" style="font-size: 0.65rem;">PRO</span>
                    </h6>
                    <small class="text-white-50 d-block text-truncate" style="font-size: 0.72rem;">Admin Console</small>
                </div>
            </div>

            <!-- Navigation Links: Main Console -->
            <div class="admin-sidebar-section-title">Core Console</div>
            <nav class="d-flex flex-column mb-3">
                <a href="AdminDashboard.aspx" class="admin-nav-item">
                    <i class="bi bi-grid-1x2-fill"></i>
                    <span>Dashboard</span>
                </a>
                <a href="AdminManagement.aspx?tab=Farmers" class="admin-nav-item">
                    <i class="bi bi-people-fill"></i>
                    <span>Farmers</span>
                </a>
                <a href="AdminManagement.aspx?tab=Buyers" class="admin-nav-item">
                    <i class="bi bi-shop"></i>
                    <span>Buyers</span>
                </a>
                <a href="AdminManagement.aspx?tab=Moderation" class="admin-nav-item">
                    <i class="bi bi-shield-lock-fill"></i>
                    <span>Moderation</span>
                </a>
            </nav>

            <!-- Navigation Links: System & Analytics -->
            <div class="admin-sidebar-section-title">Analytics & Config</div>
            <nav class="d-flex flex-column mb-auto">
                <a href="AdminAnalytics.aspx" class="admin-nav-item">
                    <i class="bi bi-graph-up-arrow"></i>
                    <span>Analytics</span>
                </a>
                <a href="AdminSettings.aspx" class="admin-nav-item active">
                    <i class="bi bi-sliders"></i>
                    <span>Settings</span>
                </a>
            </nav>

            <!-- Bottom of sidebar: Admin avatar, name, email, logout link -->
            <div class="admin-sidebar-footer">
                <div class="d-flex align-items-center justify-content-between">
                    <div class="d-flex align-items-center" style="min-width: 0;">
                        <div class="rounded-circle text-white fw-bold d-flex align-items-center justify-content-center me-2 flex-shrink-0" style="width: 34px; height: 34px; font-size: 0.8rem; background: #22c55e;">
                            SA
                        </div>
                        <div style="line-height: 1.2; min-width: 0;">
                            <span class="d-block text-white small fw-bold text-truncate">System Admin</span>
                            <span class="text-white-50 d-block text-truncate" style="font-size: 0.72rem;">admin@khedutrise.com</span>
                        </div>
                    </div>
                    <a href="Default.aspx?action=logout" class="btn btn-sm btn-outline-secondary border-0 p-1 text-danger-emphasis ms-2" title="Sign Out" onclick="window.location.href='Default.aspx?action=logout'; return false;">
                        <i class="bi bi-power fs-5"></i>
                    </a>
                </div>
            </div>
        </div>

        <!-- Main content: takes remaining space -->
        <div class="admin-main" style="flex: 1; min-width: 0; padding: 2rem 2.25rem; background-color: #f8fafc;">
            <!-- Top Header Bar inside Main Content -->
            <div class="admin-header">
                <div>
                    <small class="text-muted"><a href="Default.aspx" class="text-decoration-none text-muted">Home</a> / <a href="AdminDashboard.aspx" class="text-decoration-none text-muted">Admin Portal</a> / Platform Settings</small>
                    <h2 class="admin-title mt-1">Platform Settings & Configuration</h2>
                    <p class="admin-subtitle">Manage moderation policies, platform commission fees, notifications, and security controls</p>
                </div>
                <div class="d-flex align-items-center gap-2">
                    <a href="AdminDashboard.aspx" class="btn btn-sm btn-outline-secondary rounded-pill px-3 py-2 fw-semibold shadow-sm bg-white">
                        <i class="bi bi-arrow-left me-1"></i> Back to Dashboard
                    </a>
                </div>
            </div>

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
            <div class="admin-card">
                <div class="admin-card-header">
                    <div class="rounded-circle d-flex align-items-center justify-content-center me-3" style="width: 40px; height: 40px; background-color: #f0fdf4; color: #0b6028;">
                        <i class="bi bi-building fs-5"></i>
                    </div>
                    <div>
                        <h5 class="admin-card-title">General Platform Configuration</h5>
                        <p class="admin-card-subtitle">Basic system identity and public contact details</p>
                    </div>
                </div>
                <div class="admin-card-body">
                    <div class="row g-3">
                        <div class="col-md-6">
                            <label class="form-section-label">PORTAL NAME</label>
                            <asp:TextBox ID="txtPortalName" runat="server" CssClass="form-control admin-form-control" Text="KhedutRise - Smart Agriculture Ecosystem" required="required"></asp:TextBox>
                        </div>
                        <div class="col-md-6">
                            <label class="form-section-label">OFFICIAL ADMIN EMAIL</label>
                            <asp:TextBox ID="txtAdminEmail" runat="server" type="email" CssClass="form-control admin-form-control" Text="admin@khedutrise.com" required="required"></asp:TextBox>
                        </div>
                        <div class="col-md-4">
                            <label class="form-section-label">SUPPORT HELPLINE</label>
                            <asp:TextBox ID="txtHelpline" runat="server" CssClass="form-control admin-form-control" Text="+91 1800 233 5555" required="required"></asp:TextBox>
                        </div>
                        <div class="col-md-4">
                            <label class="form-section-label">MARKET COMMISSION FEE (%)</label>
                            <asp:TextBox ID="txtCommissionFee" runat="server" type="number" step="0.01" min="0" max="100" CssClass="form-control admin-form-control" Text="1.50" required="required"></asp:TextBox>
                        </div>
                        <div class="col-md-4">
                            <label class="form-section-label">PRIMARY APMC MARKET</label>
                            <asp:DropDownList ID="ddlPrimaryApmc" runat="server" CssClass="form-select admin-form-control">
                                <asp:ListItem Text="Unjha APMC (Main Mandi)" Value="Unjha" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="Rajkot APMC" Value="Rajkot"></asp:ListItem>
                                <asp:ListItem Text="Gondal APMC" Value="Gondal"></asp:ListItem>
                                <asp:ListItem Text="Ahmedabad APMC" Value="Ahmedabad"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
            </div>

            <!-- 2. APPROVAL & MODERATION POLICIES -->
            <div class="admin-card">
                <div class="admin-card-header">
                    <div class="rounded-circle d-flex align-items-center justify-content-center me-3" style="width: 40px; height: 40px; background-color: #eff6ff; color: #0284c7;">
                        <i class="bi bi-shield-check fs-5"></i>
                    </div>
                    <div>
                        <h5 class="admin-card-title">Moderation & Approval Policies</h5>
                        <p class="admin-card-subtitle">Configure automated verification thresholds and alert triggers</p>
                    </div>
                </div>
                <div class="admin-card-body">
                    <div class="policy-toggle-row">
                        <div class="pe-4">
                            <div class="fw-bold text-dark mb-1">Auto-Approve Farmers with Valid 7/12 Land Records</div>
                            <div class="text-muted small">Automatically mark farmer registrations as Approved when document hash verification passes.</div>
                        </div>
                        <label class="admin-switch" title="Toggle Auto-Approve Farmers">
                            <asp:CheckBox ID="chkAutoApproveFarmers" runat="server" Checked="true" />
                            <span class="admin-switch-slider"></span>
                        </label>
                    </div>

                    <div class="policy-toggle-row">
                        <div class="pe-4">
                            <div class="fw-bold text-dark mb-1">Require Mandatory GST Verification for Bulk Grain Buyers</div>
                            <div class="text-muted small">Buyers must provide valid GSTIN before placing bulk crop bids above &#8377;1 Lakh.</div>
                        </div>
                        <label class="admin-switch" title="Toggle Mandatory GST Verification">
                            <asp:CheckBox ID="chkGstRequired" runat="server" Checked="true" />
                            <span class="admin-switch-slider"></span>
                        </label>
                    </div>

                    <div class="policy-toggle-row">
                        <div class="pe-4">
                            <div class="fw-bold text-dark mb-1">Auto-Approve Verified Farmer Crop Listings under &#8377;50,000</div>
                            <div class="text-muted small">Allows verified farmers to publish small-to-medium harvest batches directly to the marketplace.</div>
                        </div>
                        <label class="admin-switch" title="Toggle Auto-Approve Crops">
                            <asp:CheckBox ID="chkAutoApproveCrops" runat="server" Checked="false" />
                            <span class="admin-switch-slider"></span>
                        </label>
                    </div>

                    <div class="policy-toggle-row">
                        <div class="pe-4">
                            <div class="fw-bold text-dark mb-1">Send Instant SMS Alerts to Users on Approval / Rejection</div>
                            <div class="text-muted small">Notify farmers and buyers instantly via SMS when admin approves or rejects their account.</div>
                        </div>
                        <label class="admin-switch" title="Toggle SMS Notifications">
                            <asp:CheckBox ID="chkSmsNotification" runat="server" Checked="true" />
                            <span class="admin-switch-slider"></span>
                        </label>
                    </div>
                </div>
            </div>

            <!-- 3. SYSTEM MAINTENANCE & DATABASE TOOLS -->
            <div class="admin-card">
                <div class="admin-card-header">
                    <div class="rounded-circle d-flex align-items-center justify-content-center me-3" style="width: 40px; height: 40px; background-color: #fff7ed; color: #ea580c;">
                        <i class="bi bi-database-gear fs-5"></i>
                    </div>
                    <div>
                        <h5 class="admin-card-title">System Security & Database Maintenance</h5>
                        <p class="admin-card-subtitle">Maintenance mode, session timeouts, and database backups</p>
                    </div>
                </div>
                <div class="admin-card-body">
                    <div class="row g-3 align-items-center mb-4">
                        <div class="col-md-6">
                            <label class="form-section-label">ADMIN SESSION TIMEOUT</label>
                            <asp:DropDownList ID="ddlSessionTimeout" runat="server" CssClass="form-select admin-form-control">
                                <asp:ListItem Text="15 Minutes" Value="15"></asp:ListItem>
                                <asp:ListItem Text="30 Minutes (Recommended)" Value="30" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="1 Hour" Value="60"></asp:ListItem>
                                <asp:ListItem Text="4 Hours" Value="240"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-6">
                            <label class="form-section-label">MAINTENANCE MODE</label>
                            <div class="d-flex justify-content-between align-items-center border rounded-3 p-3 bg-light">
                                <div class="pe-3">
                                    <div class="fw-bold text-dark" style="font-size:0.92rem;">Enable Maintenance Mode (System Downtime)</div>
                                    <div class="text-muted small mt-1">When enabled, only administrators can access the portal.</div>
                                </div>
                                <label class="admin-switch" title="Toggle Maintenance Mode">
                                    <asp:CheckBox ID="chkMaintenanceMode" runat="server" Checked="false" />
                                    <span class="admin-switch-slider"></span>
                                </label>
                            </div>
                        </div>
                    </div>

                    <div class="p-3 rounded-3 bg-light border d-flex flex-wrap align-items-center justify-content-between gap-2">
                        <div>
                            <strong class="text-dark d-block"><i class="bi bi-database-check text-success me-1"></i>Database Maintenance & Snapshots</strong>
                            <small class="text-muted">Take immediate backup of SQL Server database `KhedutRiseDB`</small>
                        </div>
                        <div class="d-flex gap-2">
                            <asp:Button ID="btnBackupDb" runat="server" Text="Backup Database Now" CssClass="btn btn-outline-dark btn-sm fw-semibold rounded-2" formnovalidate="formnovalidate" OnClick="btnBackupDb_Click" />
                            <asp:Button ID="btnClearCache" runat="server" Text="Clear Cache" CssClass="btn btn-outline-secondary btn-sm rounded-2" formnovalidate="formnovalidate" OnClick="btnClearCache_Click" />
                        </div>
                    </div>
                </div>
            </div>

            <!-- SAVE BUTTONS -->
            <div class="d-flex justify-content-end gap-2 mb-4">
                <asp:Button ID="btnResetDefaults" runat="server" Text="Reset to Defaults" CssClass="btn btn-light px-4 fw-semibold text-secondary rounded-2" formnovalidate="formnovalidate" OnClick="btnResetDefaults_Click" />
                <asp:Button ID="btnSaveSettings" runat="server" Text="Save Platform Settings" CssClass="btn text-white fw-bold px-4 rounded-2" style="background-color: #0b6028; border: none;" OnClick="btnSaveSettings_Click" />
            </div>
        </div>
    </div>

    <!-- Toggle Switch Synchronization Script -->
    <script>
        function syncAdminSwitches() {
            document.querySelectorAll('.admin-switch').forEach(function(sw) {
                var chk = sw.querySelector('input[type="checkbox"]');
                var slider = sw.querySelector('.admin-switch-slider');
                if (chk && slider) {
                    if (chk.checked) {
                        slider.classList.add('checked');
                    } else {
                        slider.classList.remove('checked');
                    }
                }
            });
        }
        document.addEventListener('DOMContentLoaded', syncAdminSwitches);
        document.addEventListener('change', function(e) {
            if (e.target && e.target.type === 'checkbox' && e.target.closest('.admin-switch')) {
                syncAdminSwitches();
            }
        });
    </script>
</asp:Content>
