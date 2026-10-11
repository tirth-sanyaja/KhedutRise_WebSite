<%@ Page Title="Live APMC Market Rates" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="ApmcRates.aspx.cs" Inherits="KhedutRise.ApmcRates" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Live APMC Rates -->
    <div class="kr-page-container">
        <!-- Header & Live Update Status -->
        <div class="d-flex flex-wrap justify-content-between align-items-center mb-4">
            <div>
                <small class="text-muted"><a href="Default.aspx" class="text-decoration-none text-muted">Home</a> / APMC Rates / Live Market</small>
                <h2 class="fw-bold mt-1 text-dark" style="font-size: 2rem;">Live APMC Market Rates</h2>
                <p class="text-muted small mb-0">Track real-time bidding rates across regulated Gujarat mandis.</p>
            </div>
            <div class="d-flex align-items-center gap-2 mt-2 mt-md-0">
                <span class="badge bg-success-subtle text-success px-3 py-2 fw-semibold" style="font-size: 0.82rem;"><i class="bi bi-circle-fill fs-6 text-success me-1" style="font-size: 0.6rem !important;"></i> LIVE UPDATES ACTIVE</span>
                <small class="text-muted ms-1" style="font-size: 0.85rem;">Last Updated: Today 12:45 PM | Market Status: Open</small>
            </div>
        </div>

        <!-- 4 Top Stat Cards -->
        <div class="row g-3 mb-4">
            <div class="col-6 col-md-3">
                <div class="card card-custom p-3 text-center h-100">
                    <small class="text-muted fw-bold" style="font-size: 0.72rem; letter-spacing: 0.5px;">HIGHEST COTTON PRICE</small>
                    <h4 class="fw-bold mt-2 mb-0" style="color: #dc2626; font-size: 1.35rem;">₹7,250 / Q</h4>
                    <small class="text-muted" style="font-size: 0.78rem;">Gondal APMC</small>
                </div>
            </div>
            <div class="col-6 col-md-3">
                <div class="card card-custom p-3 text-center h-100">
                    <small class="text-muted fw-bold" style="font-size: 0.72rem; letter-spacing: 0.5px;">AVERAGE GROUNDNUT RATE</small>
                    <h4 class="fw-bold mt-2 mb-0" style="color: #16a34a; font-size: 1.35rem;">₹5,800 / Q</h4>
                </div>
            </div>
            <div class="col-6 col-md-3">
                <div class="card card-custom p-3 text-center h-100">
                    <small class="text-muted fw-bold" style="font-size: 0.72rem; letter-spacing: 0.5px;">TOTAL ARRIVALS TODAY</small>
                    <h4 class="fw-bold text-dark mt-2 mb-0" style="color: #0284c7; font-size: 1.35rem;">4,250 Tons</h4>
                </div>
            </div>
            <div class="col-6 col-md-3">
                <div class="card card-custom p-3 text-center h-100">
                    <small class="text-muted fw-bold" style="font-size: 0.72rem; letter-spacing: 0.5px;">MARKETS TRACKED</small>
                    <h4 class="fw-bold text-dark mt-2 mb-0" style="font-size: 1.35rem;">12 Gujarat Yards</h4>
                </div>
            </div>
        </div>

        <div class="row g-4">
            <!-- LEFT: Search & Live Market Rate Table -->
            <div class="col-lg-8">
                <div class="card card-custom p-4">
                    <div class="input-group mb-3">
                        <span class="input-group-text bg-light text-muted border-end-0"><i class="bi bi-search"></i></span>
                        <asp:TextBox ID="txtSearchCommodity" runat="server" CssClass="form-control border-start-0 ps-0" placeholder="Search commodity (e.g. Groundnut)..." AutoPostBack="true" OnTextChanged="txtSearchCommodity_TextChanged"></asp:TextBox>
                    </div>

                    <div class="table-responsive">
                        <asp:GridView ID="gvApmc" runat="server" AutoGenerateColumns="False" CssClass="table table-hover align-middle mb-0">
                            <Columns>
                                <asp:BoundField DataField="Commodity" HeaderText="COMMODITY" ItemStyle-CssClass="fw-bold text-dark" />
                                <asp:BoundField DataField="MarketYard" HeaderText="MARKET YARD" />
                                <asp:BoundField DataField="MinPrice" HeaderText="MIN PRICE" DataFormatString="₹{0:N0}" />
                                <asp:BoundField DataField="MaxPrice" HeaderText="MAX PRICE" DataFormatString="₹{0:N0}" />
                                <asp:TemplateField HeaderText="AVG PRICE">
                                    <ItemTemplate>
                                        <strong class="text-success">₹<%# Eval("AvgPrice", "{0:N0}") %> / Q</strong>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>

            <!-- RIGHT: Gondal APMC Spotlight Card -->
            <div class="col-lg-4">
                <div class="card card-custom overflow-hidden h-100 d-flex flex-column">
                    <img src="Images/apmc_yard.jpg" class="img-fluid object-fit-cover" style="height: 200px; width: 100%;" alt="Gondal APMC" />
                    <div class="p-4 d-flex flex-column flex-grow-1">
                        <h5 class="fw-bold mb-2 text-dark">Gondal APMC Market Yard</h5>
                        <p class="text-muted small mb-4" style="line-height: 1.5;">Located 38 km from your boundary. Open for Cotton bidding.</p>
                        <a href="MarketDetails.aspx" class="btn btn-kr-primary w-100 mt-auto py-2 fw-bold" style="background-color: #0b6028 !important; border-color: #0b6028 !important;">View Market Details</a>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
