<%@ Page Title="Admin Settings" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="AdminSettings.aspx.cs" Inherits="KhedutRise.AdminSettings" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .admin-sidebar {
            background: linear-gradient(180deg, #0f172a 0%, #1e293b 100%);
            border-radius: 16px;
            box-shadow: 0 10px 25px -5px rgba(15, 23, 42, 0.3);
        }
        .admin-nav-item {
            color: #94a3b8;
            font-weight: 500;
            font-size: 0.92rem;
            padding: 0.75rem 1rem;
            border-radius: 10px;
            display: flex;
            align-items: center;
            text-decoration: none;
            transition: all 0.2s ease;
            position: relative;
        }
        .admin-nav-item:hover {
            color: #ffffff;
            background: rgba(255, 255, 255, 0.08);
            transform: translateX(4px);
        }
        .admin-nav-item.active {
            color: #4ade80 !important;
            background: rgba(34, 197, 94, 0.15);
            font-weight: 700;
        }
        .admin-nav-item.active::before {
            content: '';
            position: absolute;
            left: 0;
            top: 15%;
            bottom: 15%;
            width: 4px;
            background: #22c55e;
            border-radius: 0 4px 4px 0;
        }
    </style>

    <!-- Admin Settings: Platform configuration, Moderation policies, and Security -->
    <div class="kr-page-container py-4">
        <!-- Header -->
        <div class="mb-4">
            <small class="text-muted"><a href="Default.aspx" class="text-decoration-none text-muted">Home</a> / <a href="AdminDashboard.aspx" class="text-decoration-none text-muted">Admin Portal</a> / Platform Settings</small>
            <h2 class="fw-bold mt-1 text-dark" style="font-size: 2rem;">Platform Settings & Configuration</h2>
            <p class="text-muted small mb-0">Manage moderation policies, platform commission fees, notifications, and security controls.</p>
        </div>

        <div class="row g-4">
            <!-- LEFT: Modern Enterprise Sidebar -->
            <div class="col-lg-3">
                <div class="admin-sidebar p-4 h-100 d-flex flex-column">
                    <!-- Brand Section -->
                    <div class="d-flex align-items-center mb-4 pb-3 border-bottom border-secondary border-opacity-25">
                        <div class="rounded-circle d-flex align-items-center justify-content-center me-3" style="width: 44px; height: 44px; background: rgba(34, 197, 94, 0.2); color: #4ade80;">
                            <i class="bi bi-shield-check fs-4"></i>
                        </div>
                        <div>
                            <h6 class="fw-bold text-white mb-0" style="letter-spacing: 0.5px;"><span style="color: #4ade80;">Khedut</span><span style="color: #22c55e;">Rise</span> Pro</h6>
                            <small class="text-success fw-semibold" style="font-size: 0.75rem;">Super Administrator</small>
                        </div>
                    </div>

                    <!-- Navigation Links Group -->
                    <div class="mb-2">
                        <small class="text-uppercase fw-bold text-muted mb-2 d-block" style="font-size: 0.7rem; letter-spacing: 1px;">Main Console</small>
                    </div>
                    <nav class="d-flex flex-column gap-2 mb-4">
                        <a href="AdminDashboard.aspx" class="admin-nav-item">
                            <i class="bi bi-grid-1x2-fill me-3 fs-5"></i>
                            <span>Dashboard Overview</span>
                        </a>
                        <a href="AdminManagement.aspx?tab=Farmers" class="admin-nav-item">
                            <i class="bi bi-people-fill me-3 fs-5"></i>
                            <span>Farmers Directory</span>
                        </a>
                        <a href="AdminManagement.aspx?tab=Buyers" class="admin-nav-item">
                            <i class="bi bi-shop me-3 fs-5"></i>
                            <span>Buyers & Traders</span>
                        </a>
                        <a href="AdminManagement.aspx?tab=Moderation" class="admin-nav-item">
                            <i class="bi bi-shield-lock-fill me-3 fs-5"></i>
                            <span>All Moderation</span>
                        </a>
                    </nav>

                    <div class="mb-2">
                        <small class="text-uppercase fw-bold text-muted mb-2 d-block" style="font-size: 0.7rem; letter-spacing: 1px;">Analytics & System</small>
                    </div>
                    <nav class="d-flex flex-column gap-2 mb-auto">
                        <a href="AdminAnalytics.aspx" class="admin-nav-item">
                            <i class="bi bi-graph-up-arrow me-3 fs-5"></i>
                            <span>Telemetry & Reports</span>
                        </a>
                        <a href="AdminSettings.aspx" class="admin-nav-item active">
                            <i class="bi bi-sliders me-3 fs-5"></i>
                            <span>Platform Settings</span>
                        </a>
                    </nav>

                    <!-- Sidebar Footer Admin Identity -->
                    <div class="mt-4 pt-3 border-top border-secondary border-opacity-25">
                        <div class="d-flex align-items-center justify-content-between">
                            <div class="d-flex align-items-center">
                                <div class="rounded-circle bg-success text-white fw-bold d-flex align-items-center justify-content-center me-2" style="width: 32px; height: 32px; font-size: 0.8rem;">
                                    SA
                                </div>
                                <div style="line-height: 1.2;">
                                    <span class="d-block text-white small fw-bold">System Admin</span>
                                    <span class="text-muted" style="font-size: 0.72rem;">admin@khedutrise.com</span>
                                </div>
                            </div>
                        </div>
                    </div>
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

                    <div class="d-flex flex-column gap-0">
                        <div class="d-flex justify-content-between align-items-center py-3 border-bottom">
                            <div class="pe-4">
                                <div class="fw-bold text-dark mb-1">Auto-Approve Farmers with Valid 7/12 Land Records</div>
                                <div class="text-muted small">Automatically mark farmer registrations as Approved when document hash verification passes.</div>
                            </div>
                            <div class="form-check form-switch mb-0 ps-0 flex-shrink-0">
                                <asp:CheckBox ID="chkAutoApproveFarmers" runat="server" Checked="true" CssClass="form-check-input" style="width:2.5em;height:1.4em;cursor:pointer;" />
                            </div>
                        </div>

                        <div class="d-flex justify-content-between align-items-center py-3 border-bottom">
                            <div class="pe-4">
                                <div class="fw-bold text-dark mb-1">Require Mandatory GST Verification for Bulk Grain Buyers</div>
                                <div class="text-muted small">Buyers must provide valid GSTIN before placing bulk crop bids above &#8377;1 Lakh.</div>
                            </div>
                            <div class="form-check form-switch mb-0 ps-0 flex-shrink-0">
                                <asp:CheckBox ID="chkGstRequired" runat="server" Checked="true" CssClass="form-check-input" style="width:2.5em;height:1.4em;cursor:pointer;" />
                            </div>
                        </div>

                        <div class="d-flex justify-content-between align-items-center py-3 border-bottom">
                            <div class="pe-4">
                                <div class="fw-bold text-dark mb-1">Auto-Approve Verified Farmer Crop Listings under &#8377;50,000</div>
                                <div class="text-muted small">Allows verified farmers to publish small-to-medium harvest batches directly to the marketplace.</div>
                            </div>
                            <div class="form-check form-switch mb-0 ps-0 flex-shrink-0">
                                <asp:CheckBox ID="chkAutoApproveCrops" runat="server" Checked="false" CssClass="form-check-input" style="width:2.5em;height:1.4em;cursor:pointer;" />
                            </div>
                        </div>

                        <div class="d-flex justify-content-between align-items-center py-3">
                            <div class="pe-4">
                                <div class="fw-bold text-dark mb-1">Send Instant SMS Alerts to Users on Approval / Rejection</div>
                                <div class="text-muted small">Notify farmers and buyers instantly via SMS when admin approves or rejects their account.</div>
                            </div>
                            <div class="form-check form-switch mb-0 ps-0 flex-shrink-0">
                                <asp:CheckBox ID="chkSmsNotification" runat="server" Checked="true" CssClass="form-check-input" style="width:2.5em;height:1.4em;cursor:pointer;" />
                            </div>
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
                            <label class="form-label small fw-bold mb-2" style="color:#0b6028;font-size:0.75rem;">MAINTENANCE MODE</label>
                            <div class="d-flex justify-content-between align-items-center border rounded-3 p-3 bg-light">
                                <div class="pe-3">
                                    <div class="fw-bold text-dark" style="font-size:0.92rem;">Enable Maintenance Mode (System Downtime)</div>
                                    <div class="text-muted small mt-1">When enabled, only administrators can access the portal.</div>
                                </div>
                                <div class="form-check form-switch mb-0 ps-0 flex-shrink-0">
                                    <asp:CheckBox ID="chkMaintenanceMode" runat="server" Checked="false" CssClass="form-check-input" style="width:2.5em;height:1.4em;cursor:pointer;" />
                                </div>
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
