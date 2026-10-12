<%@ Page Title="Farm Weather & AI Community Insights" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Community.aspx.cs" Inherits="KhedutRise.Community" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Farm Weather & AI Community Insights -->
    <div class="kr-page-container">
        <!-- Header -->
        <div class="d-flex flex-wrap justify-content-between align-items-center mb-4">
            <div>
                <small class="text-muted"><a href="Default.aspx" class="text-decoration-none text-muted">Home</a> / Community / Weather & Insights</small>
                <h2 class="fw-bold mt-1 text-dark" style="font-size: 2rem;">Farm Weather & AI Community Insights</h2>
            </div>
            <div class="mt-2 mt-md-0">
                <button type="button" class="btn btn-kr-primary px-4 py-2 fw-bold text-white shadow-sm" data-bs-toggle="modal" data-bs-target="#modalAskQuestion" style="background-color: #0b6028; border-radius: 6px;">
                    <i class="bi bi-chat-dots me-1"></i>Ask Community
                </button>
            </div>
        </div>

        <asp:Label ID="lblForumMsg" runat="server" CssClass="d-block mb-3 fw-bold"></asp:Label>

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

                    <button type="button" class="btn btn-kr-primary w-100 mt-auto py-2 fw-bold" data-bs-toggle="modal" data-bs-target="#modalAskQuestion" style="background-color: #0b6028 !important; border-color: #0b6028 !important; border-radius: 6px;">
                        Join Community Discussion
                    </button>
                </div>
            </div>
        </div>
    </div>

    <!-- Bootstrap Modal for Ask Question -->
    <div class="modal fade" id="modalAskQuestion" tabindex="-1" aria-labelledby="modalAskQuestionLabel" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content rounded-3 border-0 shadow">
                <div class="modal-header" style="background-color: #0b6028; color: #ffffff;">
                    <h5 class="modal-title fw-bold" id="modalAskQuestionLabel">Post Question to Farmer Community</h5>
                    <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body p-4">
                    <div class="mb-3">
                        <label class="form-label" style="color: #0b6028;">DISCUSSION TOPIC / TITLE <span class="required-mark">*</span></label>
                        <asp:TextBox ID="txtTopic" runat="server" CssClass="form-control" 
                            placeholder="e.g. Best organic treatment for groundnut wilt" required="required"></asp:TextBox>
                    </div>
                    <div class="mb-3">
                        <label class="form-label" style="color: #0b6028;">CATEGORY <span class="required-mark">*</span></label>
                        <asp:DropDownList ID="ddlTopicCategory" runat="server" CssClass="form-select" required="required">
                            <asp:ListItem Text="Crop Disease & Organic Remedies" Value="Pest & Disease" Selected="True" />
                            <asp:ListItem Text="APMC Market Rate Trends" Value="Market Trends" />
                            <asp:ListItem Text="Weather & Irrigation Planning" Value="Irrigation" />
                            <asp:ListItem Text="Government Subsidies & Schemes" Value="Government Schemes" />
                        </asp:DropDownList>
                    </div>
                    <div class="mb-3">
                        <label class="form-label" style="color: #0b6028;">DETAILS / QUESTION <span class="required-mark">*</span></label>
                        <asp:TextBox ID="txtQuestionDetails" runat="server" TextMode="MultiLine" Rows="4" CssClass="form-control" 
                            placeholder="Describe your farming observation or question (minimum 10 characters)..." 
                            minlength="10" required="required"></asp:TextBox>
                    </div>
                </div>
                <div class="modal-footer border-0 p-3 pt-0">
                    <button type="button" class="btn btn-light px-4 fw-semibold text-secondary" data-bs-dismiss="modal">Cancel</button>
                    <asp:Button ID="btnPostQuestion" runat="server" Text="Post to Community" CssClass="btn text-white fw-bold px-4" OnClick="btnPostQuestion_Click" style="background-color: #0b6028; border-radius: 6px; border: none;" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
