<%@ Page Title="Farm Weather & AI Community Insights" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="WeatherForecast.aspx.cs" Inherits="KhedutRise.WeatherForecast" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Farm Weather & AI Community Insights-->
    <div class="kr-page-container">
        <!-- Header -->
        <div class="mb-4">
            <small class="text-muted"><a href="Default.aspx" class="text-decoration-none text-muted">Home</a> / Community / Weather & Insights</small>
            <h2 class="fw-bold mt-1 text-dark" style="font-size: 2rem;">Farm Weather & AI Community Insights</h2>
        </div>

        <div class="row g-4">
            <!-- LEFT: Weather Hero Card & 7-Day Forecast Grid -->
            <div class="col-lg-6">
                <!-- Weather Banner -->
                <div class="card card-custom p-4 text-white position-relative overflow-hidden mb-4" style="background: linear-gradient(rgba(0,0,0,0.35), rgba(0,0,0,0.6)), url('Images/weather_sunbeam.jpg') center/cover no-repeat; min-height: 220px; border-radius: 12px;">
                    <h1 class="display-3 fw-bold mb-0 text-white" style="letter-spacing: -1px;">28°C</h1>
                    <h4 class="fw-bold mb-2 text-white">Partly Cloudy</h4>
                    <p class="small mb-0 text-white-50">Wind: 14 km/h | Humidity: 65% | Rain Prob: 78%</p>
                </div>

                <!-- 7-Day Forecast Grid -->
                <div class="card card-custom p-4">
                    <h6 class="fw-bold mb-3 text-dark">7-Day Forecast Grid</h6>
                    <div class="row g-2 text-center">
                        <div class="col-4">
                            <div class="p-3 rounded-2 border bg-light">
                                <small class="text-muted fw-bold d-block mb-1" style="font-size: 0.72rem;">MON</small>
                                <h4 class="fw-bold text-dark mb-1">28°</h4>
                                <small class="fw-bold" style="color: #0284c7; font-size: 0.75rem;">Rain 78%</small>
                            </div>
                        </div>
                        <div class="col-4">
                            <div class="p-3 rounded-2 border bg-light">
                                <small class="text-muted fw-bold d-block mb-1" style="font-size: 0.72rem;">TUE</small>
                                <h4 class="fw-bold text-dark mb-1">27°</h4>
                                <small class="fw-bold" style="color: #0284c7; font-size: 0.75rem;">Rain 60%</small>
                            </div>
                        </div>
                        <div class="col-4">
                            <div class="p-3 rounded-2 border bg-light">
                                <small class="text-muted fw-bold d-block mb-1" style="font-size: 0.72rem;">WED</small>
                                <h4 class="fw-bold text-dark mb-1">29°</h4>
                                <small class="text-muted fw-semibold" style="font-size: 0.75rem;">Cloudy</small>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <!-- RIGHT: AI Weather Insight & Community Discussions -->
            <div class="col-lg-6">
                <div class="card card-custom p-4 p-md-5 h-100 d-flex flex-column">
                    <h5 class="fw-bold mb-3" style="color: #0b6028;">AI Weather Insight</h5>

                    <!-- Decision Support Advisory Box -->
                    <div class="p-4 rounded-3 mb-4" style="background-color: #f0f9ff; border: 1.5px solid #38bdf8;">
                        <small class="fw-bold d-block mb-2" style="color: #0284c7; font-size: 0.72rem; letter-spacing: 0.5px;">DECISION SUPPORT ADVISORY</small>
                        <p class="fw-bold text-dark mb-0" style="font-size: 1rem; line-height: 1.5;">"Rain probability is expected to increase tomorrow evening. Consider completing groundnut irrigation before the rainfall window."</p>
                    </div>

                    <!-- Trending Community Discussions -->
                    <h6 class="fw-bold mb-3 text-dark">Trending Community Discussions</h6>

                    <div class="card p-3 mb-2 rounded-2 border bg-light">
                        <strong class="d-block text-dark" style="font-size: 0.92rem;">Groundnut Yellow Leaf disease advisory</strong>
                        <small class="text-muted" style="font-size: 0.78rem;">42 replies | Last update: 10 mins ago</small>
                    </div>

                    <div class="card p-3 mb-4 rounded-2 border bg-light">
                        <strong class="d-block text-dark" style="font-size: 0.92rem;">Is cumin crop market price expected to rise in Unjha?</strong>
                        <small class="text-muted" style="font-size: 0.78rem;">18 replies | Last update: 1 hour ago</small>
                    </div>

                    <!-- Open Community Forums CTA -->
                    <a href="Community.aspx" class="btn btn-kr-primary w-100 mt-auto py-2 fw-bold" style="background-color: #0b6028 !important; border-color: #0b6028 !important; border-radius: 6px;">Open Community Forums</a>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
