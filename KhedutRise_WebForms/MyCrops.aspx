<%@ Page Title="My Crops" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="MyCrops.aspx.cs" Inherits="KhedutRise.MyCrops" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Crop Monitoring Portal-->
    <div class="kr-page-container">
        <!-- Page Title -->
        <div class="mb-4">
            <small class="text-muted"><a href="Default.aspx" class="text-decoration-none text-muted">Home</a> / My Crops</small>
            <h2 class="fw-bold mt-1 text-dark" style="font-size: 2rem;">My Crops</h2>
            <p class="text-muted small mb-0">Monitor your crops, farming activities and upcoming harvests.</p>
        </div>

        <!-- 4 Top KPI Cards -->
        <div class="row g-3 mb-4">
            <div class="col-6 col-md-3">
                <div class="card card-custom p-3 text-center h-100">
                    <small class="text-muted fw-bold" style="font-size: 0.75rem; letter-spacing: 0.5px;">TOTAL CROPS SOWN</small>
                    <h4 class="fw-bold mt-2 mb-0" style="color: #16a34a; font-size: 1.35rem;">4 Active Varieties</h4>
                </div>
            </div>
            <div class="col-6 col-md-3">
                <div class="card card-custom p-3 text-center h-100">
                    <small class="text-muted fw-bold" style="font-size: 0.75rem; letter-spacing: 0.5px;">TOTAL FARM AREA</small>
                    <h4 class="fw-bold text-dark mt-2 mb-0" style="font-size: 1.35rem;">8.5 Acres Sown</h4>
                </div>
            </div>
            <div class="col-6 col-md-3">
                <div class="card card-custom p-3 text-center h-100">
                    <small class="text-muted fw-bold" style="font-size: 0.75rem; letter-spacing: 0.5px;">ACTIVE PLOTS</small>
                    <h4 class="fw-bold mt-2 mb-0" style="color: #0284c7; font-size: 1.35rem;">Plot A, B, C & D</h4>
                </div>
            </div>
            <div class="col-6 col-md-3">
                <div class="card card-custom p-3 text-center h-100" style="border: 1.5px solid #facc15; background-color: #fefce8;">
                    <small class="text-muted fw-bold" style="font-size: 0.75rem; letter-spacing: 0.5px;">UPCOMING HARVEST</small>
                    <h4 class="fw-bold text-dark mt-2 mb-0" style="font-size: 1.35rem;">Groundnut — 18 Days</h4>
                </div>
            </div>
        </div>

        <div class="row g-4">
            <!-- LEFT: Crop Progress Cards -->
            <div class="col-lg-6">
                <div class="d-flex flex-column gap-3">
                    <!-- Crop Card 1: Groundnut -->
                    <div class="card card-custom p-3 d-flex flex-row gap-3 align-items-center">
                        <img src="Images/groundnut_harvest.jpg" class="rounded-3 object-fit-cover flex-shrink-0" width="130" height="95" alt="Groundnut" />
                        <div class="flex-grow-1">
                            <h5 class="fw-bold mb-1 text-dark" style="font-size: 1.15rem;">Groundnut (BT -32)</h5>
                            <small class="text-muted d-block" style="font-size: 0.85rem;">Plot A | 3.2 Acres | Sown: 12 June</small>
                            <div class="d-flex justify-content-between align-items-center mt-2 mb-1">
                                <small class="fw-bold" style="color: #16a34a; font-size: 0.82rem;">Growth: Flowering Stage (78%)</small>
                            </div>
                            <div class="progress" style="height: 6px; background-color: #e2e8f0;">
                                <div class="progress-bar" role="progressbar" style="width: 78%; background-color: #16a34a;" aria-valuenow="78" aria-valuemin="0" aria-valuemax="100"></div>
                            </div>
                            <small class="text-secondary d-block mt-2" style="font-size: 0.82rem;">Expected Harvest: 28 September</small>
                        </div>
                    </div>

                    <!-- Crop Card 2: Cotton -->
                    <div class="card card-custom p-3 d-flex flex-row gap-3 align-items-center">
                        <img src="Images/green_rows.jpg" class="rounded-3 object-fit-cover flex-shrink-0" width="130" height="95" alt="Cotton" />
                        <div class="flex-grow-1">
                            <h5 class="fw-bold mb-1 text-dark" style="font-size: 1.15rem;">Cotton (Bt Cotton)</h5>
                            <small class="text-muted d-block" style="font-size: 0.85rem;">Plot B | 5.3 Acres | Sown: 20 June</small>
                            <div class="d-flex justify-content-between align-items-center mt-2 mb-1">
                                <small class="fw-bold" style="color: #0284c7; font-size: 0.82rem;">Growth: Boll Dev (62%)</small>
                            </div>
                            <div class="progress" style="height: 6px; background-color: #e2e8f0;">
                                <div class="progress-bar" role="progressbar" style="width: 62%; background-color: #0284c7;" aria-valuenow="62" aria-valuemin="0" aria-valuemax="100"></div>
                            </div>
                            <small class="text-secondary d-block mt-2" style="font-size: 0.82rem;">Expected Harvest: 15 October</small>
                        </div>
                    </div>
                </div>
            </div>

            <!-- RIGHT: Farming Timeline & Progress -->
            <div class="col-lg-6">
                <div class="card card-custom p-4 h-100 d-flex flex-column">
                    <h5 class="fw-bold mb-3 text-dark">Farming Timeline & Progress</h5>

                    <!-- 5-Step Progress Timeline -->
                    <div class="d-flex justify-content-between align-items-center mb-4 px-2 position-relative" style="padding-top: 5px;">
                        <div class="text-center position-relative" style="z-index: 2;">
                            <span class="rounded-circle d-inline-flex align-items-center justify-content-center text-white" style="width: 24px; height: 24px; background-color: #16a34a; font-size: 0.75rem;"><i class="bi bi-check"></i></span>
                            <small class="d-block text-muted" style="font-size: 0.7rem; margin-top: 4px;">Sowing</small>
                        </div>
                        <div class="text-center position-relative" style="z-index: 2;">
                            <span class="rounded-circle d-inline-flex align-items-center justify-content-center text-white" style="width: 24px; height: 24px; background-color: #16a34a; font-size: 0.75rem;"><i class="bi bi-check"></i></span>
                            <small class="d-block text-muted" style="font-size: 0.7rem; margin-top: 4px;">Germination</small>
                        </div>
                        <div class="text-center position-relative" style="z-index: 2;">
                            <span class="rounded-circle d-inline-flex align-items-center justify-content-center text-white" style="width: 24px; height: 24px; background-color: #16a34a; font-size: 0.75rem;"><i class="bi bi-check"></i></span>
                            <small class="d-block text-muted" style="font-size: 0.7rem; margin-top: 4px;">Vegetative</small>
                        </div>
                        <div class="text-center position-relative" style="z-index: 2;">
                            <span class="rounded-circle d-inline-flex align-items-center justify-content-center text-white border border-2 border-white shadow-sm" style="width: 26px; height: 26px; background-color: #16a34a; font-size: 0.75rem;"><i class="bi bi-dot" style="font-size: 1.5rem;"></i></span>
                            <small class="d-block fw-bold text-success" style="font-size: 0.7rem; margin-top: 4px;">Flowering</small>
                        </div>
                        <div class="text-center position-relative" style="z-index: 2;">
                            <span class="rounded-circle d-inline-flex align-items-center justify-content-center text-muted border" style="width: 24px; height: 24px; background-color: #f1f5f9; font-size: 0.75rem;">5</span>
                            <small class="d-block text-muted" style="font-size: 0.7rem; margin-top: 4px;">Maturity</small>
                        </div>
                    </div>

                    <!-- Activity Log Table -->
                    <div class="table-responsive mb-4">
                        <table class="table table-sm align-middle mb-0">
                            <thead>
                                <tr class="text-muted" style="font-size: 0.75rem; letter-spacing: 0.5px;">
                                    <th>DATE</th>
                                    <th>CROP</th>
                                    <th>ACTIVITY</th>
                                    <th>PLOT</th>
                                    <th>STATUS</th>
                                </tr>
                            </thead>
                            <tbody style="font-size: 0.88rem;">
                                <tr>
                                    <td class="fw-semibold">10 Aug</td>
                                    <td>Groundnut</td>
                                    <td>Weeding</td>
                                    <td>Plot A</td>
                                    <td><span class="badge bg-success-subtle text-success px-2 py-1">Completed</span></td>
                                </tr>
                                <tr>
                                    <td class="fw-semibold">18 Aug</td>
                                    <td>Cotton</td>
                                    <td>Fertilizer Spray</td>
                                    <td>Plot B</td>
                                    <td><span class="badge bg-warning-subtle text-warning px-2 py-1">Scheduled</span></td>
                                </tr>
                            </tbody>
                        </table>
                    </div>

                    <!-- Action Button -->
                    <a href="FarmPlots.aspx" class="btn btn-kr-primary w-100 mt-auto py-2 fw-bold" style="background-color: #0b6028 !important; border-color: #0b6028 !important;">+ Add Farming Activity</a>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
