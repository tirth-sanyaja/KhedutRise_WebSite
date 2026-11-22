<%@ Page Title="Farm Weather & AI Community Insights" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Community.aspx.cs" Inherits="KhedutRise.Community" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Farm Weather & AI Community Insights: 100% Dynamic -->
    <div class="kr-page-container py-4">
        <!-- Header -->
        <div class="d-flex flex-wrap justify-content-between align-items-center mb-4">
            <div>
                <small class="text-muted"><a href="Default.aspx" class="text-decoration-none text-muted">Home</a> / Community / Weather &amp; Insights</small>
                <h2 class="fw-bold mt-1 text-dark" style="font-size: 2rem;">Farm Weather &amp; AI Community Insights</h2>
                <p class="text-muted small mb-0">Live hyper-local agro-weather advisories and peer discussions among Gujarati farmers &amp; agronomists.</p>
            </div>
            <div class="mt-2 mt-md-0">
                <button type="button" class="btn btn-kr-primary px-4 py-2 fw-bold text-white shadow-sm" data-bs-toggle="modal" data-bs-target="#modalAskQuestion" style="background-color: #0b6028; border-radius: 6px;">
                    <i class="bi bi-chat-dots me-1"></i> Ask Community Question
                </button>
            </div>
        </div>

        <asp:Label ID="lblForumMsg" runat="server" CssClass="d-block mb-3 fw-bold" EnableViewState="false"></asp:Label>

        <div class="row g-4">
            <!-- LEFT: Weather Hero Card & 7-Day Forecast Grid -->
            <div class="col-lg-5">
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
                <div class="card card-custom p-4 text-white position-relative overflow-hidden mb-4 shadow-sm" style="background: linear-gradient(rgba(0,0,0,0.45), rgba(0,0,0,0.7)), url('Images/weather_sunbeam.jpg') center/cover no-repeat; min-height: 220px; border-radius: 12px;">
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

                <!-- AI Decision Support Advisory Box -->
                <div class="card p-4 rounded-3 border-0 shadow-sm mb-4" style="background-color: #f0f9ff; border-left: 5px solid #0284c7 !important;">
                    <div class="d-flex align-items-center mb-2">
                        <i class="bi bi-robot fs-5 text-primary me-2"></i>
                        <small class="fw-bold text-primary text-uppercase" style="letter-spacing: 0.5px;">AGRONOMIC AI CROP ADVISORY</small>
                    </div>
                    <p class="fw-semibold text-dark mb-0" style="font-size: 0.95rem; line-height: 1.5;">
                        <asp:Literal ID="litAdvisory" runat="server">
                            "Monsoon rain probability is expected to increase over Saurashtra tomorrow evening. Consider completing groundnut irrigation before the rainfall window."
                        </asp:Literal>
                    </p>
                </div>
            </div>

            <!-- RIGHT: Community Discussions Repeater -->
            <div class="col-lg-7">
                <div class="card p-4 border rounded-3 bg-white shadow-sm h-100 d-flex flex-column">
                    <div class="d-flex flex-wrap justify-content-between align-items-center mb-3 pb-2 border-bottom">
                        <div>
                            <h5 class="fw-bold text-dark mb-0">Farmer Discussions &amp; Insights</h5>
                            <small class="text-success fw-bold">
                                <asp:Literal ID="litDiscussionCount" runat="server">4 Active Discussions</asp:Literal>
                            </small>
                        </div>
                        <div class="col-auto">
                            <asp:DropDownList ID="ddlFilterCategory" runat="server" AutoPostBack="true" 
                                OnSelectedIndexChanged="ddlFilterCategory_SelectedIndexChanged" CssClass="form-select form-select-sm fw-semibold">
                                <asp:ListItem Text="All Topics" Value="All" Selected="True" />
                                <asp:ListItem Text="Crops & Sowing" Value="Crops & Sowing" />
                                <asp:ListItem Text="Mandi & APMC" Value="Mandi & APMC" />
                                <asp:ListItem Text="Organic Farming" Value="Organic Farming" />
                                <asp:ListItem Text="Machinery & Tools" Value="Machinery & Tools" />
                            </asp:DropDownList>
                        </div>
                    </div>

                    <!-- Repeater of Community Posts -->
                    <div class="d-flex flex-column gap-3 mb-4 flex-grow-1">
                        <asp:Repeater ID="rptDiscussions" runat="server">
                            <ItemTemplate>
                                <div class="card p-3 rounded-2 border bg-light shadow-none">
                                    <div class="d-flex justify-content-between align-items-start mb-1">
                                        <h6 class="fw-bold text-dark mb-0"><%# Eval("Title") %></h6>
                                        <span class="badge bg-success-subtle text-success small"><%# Eval("Category") %></span>
                                    </div>
                                    <p class="small text-secondary mb-2" style="line-height: 1.4;"><%# Eval("Details") %></p>
                                    <div class="d-flex justify-content-between align-items-center small text-muted pt-2 border-top">
                                        <span><i class="bi bi-person me-1"></i><strong><%# Eval("AuthorName") %></strong> (<%# Eval("AuthorRole") %>)</span>
                                        <span><i class="bi bi-chat-left-text me-1"></i><%# Eval("RepliesCount") %> replies &bull; <%# Convert.ToDateTime(Eval("CreatedDate")).ToString("dd MMM yyyy, HH:mm") %></span>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>

                        <asp:Panel ID="pnlNoPosts" runat="server" Visible="false" CssClass="text-center py-4">
                            <i class="bi bi-chat-square-dots text-muted fs-3 d-block mb-2"></i>
                            <p class="text-muted small mb-0">No discussions found in this category. Click 'Ask Community Question' to be the first!</p>
                        </asp:Panel>
                    </div>

                    <button type="button" class="btn btn-kr-primary w-100 mt-auto py-2 fw-bold text-white shadow-sm" data-bs-toggle="modal" data-bs-target="#modalAskQuestion" style="background-color: #0b6028 !important; border-color: #0b6028 !important; border-radius: 6px;">
                        <i class="bi bi-pencil-square me-1"></i> Post a Question or Advisory
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
                    <h5 class="modal-title fw-bold" id="modalAskQuestionLabel">
                        <i class="bi bi-chat-dots me-1"></i> Post Question to Farmer Community
                    </h5>
                    <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body p-4">
                    <div class="mb-3">
                        <label class="form-label" style="color: #0b6028;">CATEGORY <span class="text-danger">*</span></label>
                        <asp:DropDownList ID="ddlTopicCategory" runat="server" CssClass="form-select">
                            <asp:ListItem Text="Crops & Sowing" Value="Crops & Sowing" Selected="True" />
                            <asp:ListItem Text="Mandi & APMC" Value="Mandi & APMC" />
                            <asp:ListItem Text="Organic Farming" Value="Organic Farming" />
                            <asp:ListItem Text="Machinery & Tools" Value="Machinery & Tools" />
                        </asp:DropDownList>
                    </div>
                    <div class="mb-3">
                        <label class="form-label" style="color: #0b6028;">TOPIC / QUESTION TITLE <span class="text-danger">*</span></label>
                        <asp:TextBox ID="txtTopic" runat="server" CssClass="form-control" placeholder="e.g. Recommended fertilizer dosage for Bt Cotton"></asp:TextBox>
                    </div>
                    <div class="mb-4">
                        <label class="form-label" style="color: #0b6028;">DETAILED QUESTION OR OBSERVATION <span class="text-danger">*</span></label>
                        <asp:TextBox ID="txtQuestionDetails" runat="server" TextMode="MultiLine" Rows="4" CssClass="form-control" placeholder="Describe symptoms, soil conditions, acreage, and query details..."></asp:TextBox>
                    </div>
                    <asp:Button ID="btnPostQuestion" runat="server" Text="Post Question to Community" CssClass="btn w-100 py-2 fw-bold text-white shadow-sm" OnClick="btnPostQuestion_Click" style="background-color: #0b6028; border-radius: 6px;" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
