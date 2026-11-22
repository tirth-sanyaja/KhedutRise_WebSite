<%@ Page Title="Farm Weather & AI Community Insights" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="WeatherForecast.aspx.cs" Inherits="KhedutRise.WeatherForecast" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Farm Weather & AI Community Insights: 100% Dynamic -->
    <div class="kr-page-container py-4">
        <!-- Header -->
        <div class="d-flex flex-wrap justify-content-between align-items-center mb-4">
            <div>
                <small class="text-muted"><a href="Default.aspx" class="text-decoration-none text-muted">Home</a> / Community / Weather &amp; Insights</small>
                <h2 class="fw-bold mt-1 text-dark" style="font-size: 2rem;">Farm Weather &amp; AI Community Insights</h2>
                <p class="text-muted small mb-0">Live hyper-local agro-meteorology and AI-powered agronomic decision support for Gujarat farmers.</p>
            </div>
            <div class="mt-2 mt-md-0">
                <a href="Community.aspx" class="btn btn-outline-success px-4 py-2 fw-bold shadow-sm" style="border-radius: 6px;">
                    <i class="bi bi-chat-dots me-1"></i> Full Community Forum
                </a>
            </div>
        </div>

        <div class="row g-4">
            <!-- LEFT: Weather Hero Card & 7-Day Forecast Grid -->
            <div class="col-lg-6">
                <!-- District Selector -->
                <div class="card p-3 bg-white border rounded-3 mb-3 shadow-sm">
                    <label class="form-label fw-bold text-dark small mb-1">
                        <i class="bi bi-geo-alt-fill text-success me-1"></i> SELECT GUJARAT DISTRICT:
                    </label>
                    <asp:DropDownList ID="ddlWeatherDistrict" runat="server" AutoPostBack="true" 
                        OnSelectedIndexChanged="ddlWeatherDistrict_SelectedIndexChanged" CssClass="form-select fw-semibold border-success">
                        <asp:ListItem Text="Rajkot (Saurashtra)" Value="Rajkot" Selected="True" />
                        <asp:ListItem Text="Ahmedabad (Central Gujarat)" Value="Ahmedabad" />
                        <asp:ListItem Text="Surat (South Gujarat)" Value="Surat" />
                        <asp:ListItem Text="Junagadh (Gir Foothills)" Value="Junagadh" />
                        <asp:ListItem Text="Mehsana (North Gujarat)" Value="Mehsana" />
                    </asp:DropDownList>
                </div>

                <!-- Weather Banner -->
                <div class="card card-custom p-4 text-white position-relative overflow-hidden mb-4 shadow-sm" style="background: linear-gradient(rgba(0,0,0,0.35), rgba(0,0,0,0.6)), url('Images/weather_sunbeam.jpg') center/cover no-repeat; min-height: 220px; border-radius: 12px;">
                    <h1 class="display-3 fw-bold mb-0 text-white" style="letter-spacing: -1px;">
                        <asp:Literal ID="litTemp" runat="server">28°C</asp:Literal>
                    </h1>
                    <h4 class="fw-bold mb-2 text-white">
                        <asp:Literal ID="litCondition" runat="server">Partly Cloudy</asp:Literal>
                    </h4>
                    <p class="small mb-0 text-white-50">
                        <asp:Literal ID="litWeatherMeta" runat="server">Wind: 14 km/h | Humidity: 65% | Rain Prob: 70%</asp:Literal>
                    </p>
                </div>

                <!-- 7-Day Forecast Grid -->
                <div class="card card-custom p-4 shadow-sm border rounded-3">
                    <h6 class="fw-bold mb-3 text-dark">Upcoming 3-Day Agro Forecast</h6>
                    <div class="row g-2 text-center">
                        <div class="col-4">
                            <div class="p-3 rounded-2 border bg-light">
                                <small class="text-muted fw-bold d-block mb-1" style="font-size: 0.72rem;">TOMORROW</small>
                                <h4 class="fw-bold text-dark mb-1"><asp:Literal ID="litDay1Temp" runat="server">28°</asp:Literal></h4>
                                <small class="fw-bold text-primary" style="font-size: 0.75rem;"><asp:Literal ID="litDay1Rain" runat="server">Rain 70%</asp:Literal></small>
                            </div>
                        </div>
                        <div class="col-4">
                            <div class="p-3 rounded-2 border bg-light">
                                <small class="text-muted fw-bold d-block mb-1" style="font-size: 0.72rem;">DAY AFTER</small>
                                <h4 class="fw-bold text-dark mb-1"><asp:Literal ID="litDay2Temp" runat="server">27°</asp:Literal></h4>
                                <small class="fw-bold text-primary" style="font-size: 0.75rem;"><asp:Literal ID="litDay2Rain" runat="server">Rain 55%</asp:Literal></small>
                            </div>
                        </div>
                        <div class="col-4">
                            <div class="p-3 rounded-2 border bg-light">
                                <small class="text-muted fw-bold d-block mb-1" style="font-size: 0.72rem;">DAY 3</small>
                                <h4 class="fw-bold text-dark mb-1"><asp:Literal ID="litDay3Temp" runat="server">29°</asp:Literal></h4>
                                <small class="text-muted fw-semibold" style="font-size: 0.75rem;"><asp:Literal ID="litDay3Rain" runat="server">Clear Sky</asp:Literal></small>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <!-- RIGHT: AI Weather Insight & Community Discussions -->
            <div class="col-lg-6">
                <div class="card card-custom p-4 p-md-5 h-100 d-flex flex-column shadow-sm border rounded-3">
                    <div class="d-flex align-items-center mb-3">
                        <i class="bi bi-robot fs-4 text-success me-2"></i>
                        <h5 class="fw-bold mb-0" style="color: #0b6028;">Agronomic AI Decision Support</h5>
                    </div>

                    <!-- Decision Support Advisory Box -->
                    <div class="p-4 rounded-3 mb-4" style="background-color: #f0f9ff; border: 1.5px solid #38bdf8;">
                        <small class="fw-bold d-block mb-2" style="color: #0284c7; font-size: 0.72rem; letter-spacing: 0.5px;">HYPER-LOCAL CROP ADVISORY</small>
                        <p class="fw-bold text-dark mb-0" style="font-size: 1rem; line-height: 1.5;">
                            "<asp:Literal ID="litAdvisory" runat="server">Rain probability is expected to increase tomorrow evening. Consider completing groundnut irrigation before the rainfall window.</asp:Literal>"
                        </p>
                    </div>

                    <!-- Trending Community Discussions -->
                    <div class="d-flex justify-content-between align-items-center mb-3">
                        <h6 class="fw-bold text-dark mb-0">Trending Community Discussions</h6>
                        <a href="Community.aspx" class="text-success small fw-semibold text-decoration-none">View All</a>
                    </div>

                    <div class="mb-3">
                        <asp:Repeater ID="rptTrendingDiscussions" runat="server">
                            <ItemTemplate>
                                <div class="card p-3 mb-2 rounded-2 border bg-light shadow-none">
                                    <div class="d-flex justify-content-between align-items-start mb-1">
                                        <strong class="text-dark" style="font-size: 0.92rem;"><%# Eval("Title") %></strong>
                                        <span class="badge bg-success-subtle text-success small"><%# Eval("Category") %></span>
                                    </div>
                                    <small class="text-muted" style="font-size: 0.78rem;">
                                        <i class="bi bi-person me-1"></i><%# Eval("AuthorName") %> &bull; 
                                        <i class="bi bi-chat-left-text me-1"></i><%# Eval("RepliesCount") %> replies &bull; 
                                        <%# Convert.ToDateTime(Eval("CreatedDate")).ToString("dd MMM yyyy") %>
                                    </small>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>

                        <asp:Panel ID="pnlNoTrending" runat="server" Visible="false" CssClass="text-center py-3">
                            <p class="text-muted small mb-0">No active community discussions yet.</p>
                        </asp:Panel>
                    </div>

                    <!-- Open Community Forums CTA -->
                    <a href="Community.aspx" class="btn btn-kr-primary w-100 mt-auto py-2 fw-bold text-white shadow-sm" style="background-color: #0b6028 !important; border-color: #0b6028 !important; border-radius: 6px;">
                        <i class="bi bi-chat-square-dots me-1"></i> Open Community Forums &amp; Ask Question
                    </a>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
