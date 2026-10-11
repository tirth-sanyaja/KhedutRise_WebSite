<%@ Page Title="Farmland GPS & Plots" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="FarmPlots.aspx.cs" Inherits="KhedutRise.FarmPlots" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Farmland Registry & GPS -->
    <div class="kr-page-container py-4">
        <h3 class="fw-bold mb-4 text-dark" style="font-size: 2rem;">My Farmland GPS & Plots</h3>

        <div class="row g-4 align-items-stretch">
            <!-- LEFT: Big Farmland Perspective Image -->
            <div class="col-lg-6">
                <div class="card border rounded-3 overflow-hidden shadow-none h-100" style="border-color: #e2e8f0 !important;">
                    <img src="Images/green_rows.jpg" class="object-fit-cover w-100 h-100" style="min-height: 520px;" alt="Farmland GPS" />
                </div>
            </div>

            <!-- RIGHT: Plots Registry, Farm Documents & Soil Profile-->
            <div class="col-lg-6">
                <div class="card p-4 p-md-5 border rounded-3 shadow-none h-100 d-flex flex-column" style="border-color: #e2e8f0 !important; background: #ffffff;">
                    <!-- Table Header -->
                    <h5 class="fw-bold mb-3" style="color: #0b6028;">Plots Registry Table</h5>

                    <!-- Repeater for Dynamic Plots Table -->
                    <asp:Repeater ID="rptPlots" runat="server">
                        <HeaderTemplate>
                            <div class="table-responsive mb-4">
                                <table class="table table-hover align-middle mb-0" style="font-size: 0.95rem;">
                                    <thead class="text-secondary small fw-bold" style="border-bottom: 2px solid #e2e8f0;">
                                        <tr>
                                            <th>PLOT</th>
                                            <th>AREA</th>
                                            <th>CROP</th>
                                            <th>STAGE</th>
                                            <th>STATUS</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td class="fw-bold text-dark"><%# Eval("PlotName") %></td>
                                <td class="text-secondary"><%# Eval("AreaAcres") %> Ac</td>
                                <td class="text-dark"><%# Eval("Crop") %></td>
                                <td class="fw-bold" style="color: <%# Eval("Stage").ToString() == "Flowering" ? "#16a34a" : (Eval("Stage").ToString() == "Boll Dev" ? "#ea580c" : "#0284c7") %>;"><%# Eval("Stage") %></td>
                                <td class="fw-bold" style="color: #16a34a;"><%# Eval("Status") %></td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                                    </tbody>
                                </table>
                            </div>
                        </FooterTemplate>
                    </asp:Repeater>

                    <!-- Farm Documents -->
                    <div class="mb-4">
                        <h6 class="fw-bold text-dark mb-2">Farm Documents</h6>
                        <div class="row g-2">
                            <div class="col-6">
                                <div class="p-3 rounded-2 text-center fw-bold" style="background-color: #f1f5f9; color: #1e293b; font-size: 0.92rem; border: 1px solid #e2e8f0;">
                                    Land 7/12 ROR.pdf
                                </div>
                            </div>
                            <div class="col-6">
                                <div class="p-3 rounded-2 text-center fw-bold" style="background-color: #f1f5f9; color: #1e293b; font-size: 0.92rem; border: 1px solid #e2e8f0;">
                                    Soil Lab Certificate.pdf
                                </div>
                            </div>
                        </div>
                    </div>

                    <!-- Agronomical Details & Soil Profile -->
                    <div class="mb-4">
                        <h6 class="fw-bold text-dark mb-2">Agronomical Details & Soil Profile</h6>
                        <div class="p-3 rounded-3" style="background-color: #f8fafc; border: 1px solid #e2e8f0; font-size: 0.92rem; line-height: 1.8;">
                            <div>• Soil Texture: <strong style="color: #16a34a;">Clayey Loam (Deep Fertile Soil)</strong></div>
                            <div>• Irrigation Source: <strong style="color: #0284c7;">Solar Powered Tube Well (Drip Connected)</strong></div>
                            <div>• Soil pH Level: <strong style="color: #ea580c;">7.2 (Slightly Alkaline - Optimal for Groundnut)</strong></div>
                            <div>• Grid Electricity Schedule: <strong class="text-dark">Agriculture Line (8 Hrs Day Feed)</strong></div>
                        </div>
                    </div>

                    <!-- Action Buttons -->
                    <div class="d-flex gap-3 mt-auto pt-2">
                        <button type="button" class="btn flex-fill py-2 text-white fw-bold shadow-sm" data-bs-toggle="modal" data-bs-target="#modalAddPlot" style="background-color: #0b6028; border-radius: 6px;">Add Plot</button>
                        <a href="#" class="btn flex-fill py-2 fw-bold" style="border: 2px solid #0b6028; color: #0b6028; background: #ffffff; border-radius: 6px;">Measure Land Area</a>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Bootstrap Modal for Add Plot (CRUD INSERT) -->
    <div class="modal fade" id="modalAddPlot" tabindex="-1" aria-labelledby="modalAddPlotLabel" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content rounded-3 border-0 shadow">
                <div class="modal-header" style="background-color: #0b6028; color: #ffffff;">
                    <h5 class="modal-title fw-bold" id="modalAddPlotLabel">Add New Farm Plot</h5>
                    <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body p-4">
                    <div class="mb-3">
                        <label class="form-label" style="color: #0b6028;">PLOT NAME <span class="required-mark">*</span></label>
                        <asp:TextBox ID="txtPlotName" runat="server" CssClass="form-control" 
                            placeholder="e.g. North Plot C" required="required"></asp:TextBox>
                    </div>
                    <div class="mb-3">
                        <label class="form-label" style="color: #0b6028;">AREA IN ACRES <span class="required-mark">*</span></label>
                        <asp:TextBox ID="txtArea" runat="server" type="number" step="0.1" min="0.1" CssClass="form-control" 
                            placeholder="e.g. 4.0" required="required"></asp:TextBox>
                    </div>
                    <div class="mb-3">
                        <label class="form-label" style="color: #0b6028;">PRIMARY CROP <span class="required-mark">*</span></label>
                        <asp:TextBox ID="txtCrop" runat="server" CssClass="form-control" 
                            placeholder="e.g. Wheat, Castor, Groundnut" required="required"></asp:TextBox>
                    </div>
                    <div class="mb-3">
                        <label class="form-label" style="color: #0b6028;">CURRENT GROWTH STAGE <span class="required-mark">*</span></label>
                        <asp:TextBox ID="txtStage" runat="server" CssClass="form-control" 
                            placeholder="e.g. Sowing, Flowering, Boll Dev" required="required"></asp:TextBox>
                    </div>
                </div>
                <div class="modal-footer border-0 p-3 pt-0">
                    <button type="button" class="btn btn-light px-4 fw-semibold text-secondary" data-bs-dismiss="modal">Cancel</button>
                    <asp:Button ID="btnAddPlot" runat="server" Text="Save Plot" CssClass="btn text-white fw-bold px-4" OnClick="btnAddPlot_Click" style="background-color: #0b6028; border-radius: 6px; border: none;" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
