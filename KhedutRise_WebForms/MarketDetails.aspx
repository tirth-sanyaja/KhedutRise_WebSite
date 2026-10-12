<%@ Page Title="Rajkot APMC Market Yard" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="MarketDetails.aspx.cs" Inherits="KhedutRise.MarketDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- APMC Market Yard Details-->
    <div class="kr-page-container">
        <!-- Hero Banner: Panoramic APMC Yard -->
        <div class="card card-custom p-4 p-md-5 mb-4 text-white position-relative overflow-hidden" style="background: linear-gradient(rgba(0,0,0,0.55), rgba(0,0,0,0.7)), url('Images/apmc_yard.jpg') center/cover no-repeat; min-height: 200px;">
            <small class="text-light opacity-90"><a href="Default.aspx" class="text-decoration-none text-light">Home</a> / <a href="ApmcRates.aspx" class="text-decoration-none text-light">APMC Rates</a> / Market Details</small>
            <h2 class="display-6 fw-bold mt-2 text-white">Rajkot APMC Market Yard</h2>
            <p class="small text-white-50 mb-2"><i class="bi bi-geo-alt me-1"></i> Location: Rajkot Bypass Road | Open Hours: 08:00 AM - 05:00 PM</p>
            <div>
                <span class="badge bg-warning text-dark fw-bold px-3 py-1" style="font-size: 0.8rem;">Verified government regulated agricultural trading market</span>
            </div>
        </div>

        <div class="row g-4">
            <!-- LEFT: Today's Arrivals & Trading Prices -->
            <div class="col-lg-8">
                <div class="card card-custom p-4">
                    <h5 class="fw-bold mb-3 text-dark">Today's Arrivals & Trading Prices</h5>
                    <div class="table-responsive">
                        <table class="table table-hover align-middle mb-0">
                            <thead class="text-muted" style="font-size: 0.75rem; letter-spacing: 0.5px;">
                                <tr>
                                    <th>COMMODITY</th>
                                    <th>ARRIVALS (TONS)</th>
                                    <th>MIN PRICE</th>
                                    <th>MAX PRICE</th>
                                    <th>AVG PRICE</th>
                                </tr>
                            </thead>
                            <tbody style="font-size: 0.92rem;">
                                <tr>
                                    <td><strong class="text-dark">Groundnut</strong></td>
                                    <td>250 Tons</td>
                                    <td>₹5,400</td>
                                    <td>₹5,900</td>
                                    <td><strong class="text-success">₹5,650 / Q</strong></td>
                                </tr>
                                <tr>
                                    <td><strong class="text-dark">Wheat</strong></td>
                                    <td>420 Tons</td>
                                    <td>₹2,400</td>
                                    <td>₹2,800</td>
                                    <td><strong class="text-success">₹2,600 / Q</strong></td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>

            <!-- RIGHT: APMC Information -->
            <div class="col-lg-4">
                <div class="card card-custom p-4 h-100 d-flex flex-column">
                    <h5 class="fw-bold mb-3 text-dark">APMC Information</h5>

                    <div class="mb-3">
                        <small class="text-muted fw-bold d-block" style="font-size: 0.75rem; letter-spacing: 0.5px;">YARD ADDRESS</small>
                        <p class="mb-0 text-dark fw-semibold" style="font-size: 0.92rem;">Rajkot Gondal Highway Bypass, Rajkot</p>
                    </div>

                    <div class="mb-3">
                        <small class="text-muted fw-bold d-block" style="font-size: 0.75rem; letter-spacing: 0.5px;">CONTACT DESK</small>
                        <p class="mb-0 fw-bold" style="color: #0284c7; font-size: 0.95rem;">+91 12345 67899</p>
                    </div>

                    <div class="mb-4">
                        <small class="text-muted fw-bold d-block" style="font-size: 0.75rem; letter-spacing: 0.5px;">DISTANCE</small>
                        <p class="mb-0 text-dark" style="font-size: 0.92rem;">14.2 km from your location</p>
                    </div>

                    <a href="https://maps.google.com" target="_blank" class="btn btn-kr-primary w-100 mt-auto py-2 fw-bold" style="background-color: #0b6028 !important; border-color: #0b6028 !important;">Get Directions</a>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
