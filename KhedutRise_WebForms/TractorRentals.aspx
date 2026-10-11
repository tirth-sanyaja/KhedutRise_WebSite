<%@ Page Title="Tractor & Agricultural Equipment" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="TractorRentals.aspx.cs" Inherits="KhedutRise.TractorRentals" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Tractor & Agricultural Equipment-->
    <div class="kr-page-container">
        <!--  Header -->
        <div class="mb-4">
            <small class="text-muted"><a href="Default.aspx" class="text-decoration-none text-muted">Home</a> / Agri Services / Equipment Marketplace</small>
            <h2 class="fw-bold mt-1 text-dark" style="font-size: 2rem;">Tractor & Agricultural Equipment</h2>
            <p class="text-muted small mb-0">Find reliable equipment for your farming needs.</p>
        </div>

        <!-- Rent Verified High-HP Tractors & Harvesters Search Banner -->
        <div class="card card-custom p-4 mb-4">
            <h5 class="fw-bold mb-1" style="color: #0b6028;">Rent Verified High-HP Tractors & Harvesters</h5>
            <p class="text-muted small mb-3">Direct booking with local verified machinery owners.</p>
            <div class="row g-3">
                <div class="col-md-5">
                    <input type="text" class="form-control" placeholder="What equipment do you need?" />
                </div>
                <div class="col-md-5">
                    <input type="text" class="form-control" placeholder="Location?" />
                </div>
                <div class="col-md-2">
                    <button type="button" class="btn btn-kr-primary w-100 py-2 fw-bold" style="background-color: #0b6028 !important; border-color: #0b6028 !important;">Search</button>
                </div>
            </div>
        </div>

        <div class="row g-4">
            <!-- LEFT: Massey Ferguson Card -->
            <div class="col-lg-5">
                <div class="card card-custom p-4 h-100 d-flex flex-column">
                    <img src="Images/massey_tractor.jpg" class="rounded-3 mb-3 img-fluid object-fit-cover" style="height: 220px; width: 100%;" alt="Massey Ferguson" />
                    <h5 class="fw-bold mb-1 text-dark" style="font-size: 1.25rem;">Massey Ferguson 241DI (41 HP)</h5>
                    <small class="text-muted mb-2 d-block" style="font-size: 0.85rem;">Year: 2023 | Location: Rajkot Bypass | Rating: 4.8</small>
                    <h3 class="fw-bold mb-4" style="color: #0b6028; font-size: 1.7rem;">₹800 / Hour</h3>

                    <div class="d-flex gap-2 mt-auto">
                        <a href="BookingConfirm.aspx" class="btn w-50 py-2 fw-bold" style="background-color: #ffffff; color: #0b6028; border: 1.5px solid #0b6028; border-radius: 6px;">View Details</a>
                        <a href="BookingConfirm.aspx" class="btn btn-kr-primary w-50 py-2 fw-bold" style="background-color: #0b6028 !important; border-color: #0b6028 !important; border-radius: 6px;">Book Now</a>
                    </div>
                </div>
            </div>

            <!-- RIGHT: Compare Popular Equipment -->
            <div class="col-lg-7">
                <div class="card card-custom p-4 h-100 d-flex flex-column">
                    <h5 class="fw-bold mb-4" style="color: #0b6028;">Compare Popular Equipment</h5>
                    <div class="table-responsive">
                        <table class="table table-hover align-middle mb-0">
                            <thead class="text-muted" style="font-size: 0.72rem; letter-spacing: 0.5px;">
                                <tr>
                                    <th>SPEC</th>
                                    <th>MASSEY FERGUSON 241DI</th>
                                    <th>MAHINDRA YUVO 575</th>
                                    <th>SWARAJ 744 FE</th>
                                </tr>
                            </thead>
                            <tbody style="font-size: 0.92rem;">
                                <tr>
                                    <td class="fw-semibold text-dark">Horsepower</td>
                                    <td>41 HP</td>
                                    <td>45 HP</td>
                                    <td>48 HP</td>
                                </tr>
                                <tr>
                                    <td class="fw-semibold text-dark">Rate / Hour</td>
                                    <td><strong style="color: #0b6028;">₹800 / hr</strong></td>
                                    <td><strong style="color: #0b6028;">₹750 / hr</strong></td>
                                    <td><strong style="color: #0b6028;">₹780 / hr</strong></td>
                                </tr>
                                <tr>
                                    <td class="fw-semibold text-dark">Rating</td>
                                    <td>4.8 </td>
                                    <td>4.6 </td>
                                    <td>4.7 </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
