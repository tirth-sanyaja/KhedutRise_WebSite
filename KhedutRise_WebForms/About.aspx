<%@ Page Title="About Us" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="About.aspx.cs" Inherits="KhedutRise.About" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- About Us: Page-->
   
    <div class="kr-page-container">
        <!--Header -->
        <div class="mb-4">
            <small class="text-muted"><a href="Default.aspx" class="text-decoration-none text-muted">Home</a> / About Us</small>
            <h2 class="fw-bold mt-1 text-dark" style="font-size: 2rem;">About <span class="brand-khedut">Khedut</span><span class="brand-rise">Rise</span></h2>
            <p class="text-muted small mb-0">Revolutionizing Indian agriculture through transparent technology.</p>
        </div>

        <!-- Wide Hero Banner  -->
        <div class="card card-custom p-4 p-md-5 mb-4 text-white overflow-hidden position-relative" style="background: linear-gradient(rgba(0,0,0,0.5), rgba(0,0,0,0.65)), url('Images/farmer_family.jpg') center/cover no-repeat; min-height: 220px; border-radius: 12px;">
            <div class="my-auto">
                <h2 class="display-6 fw-bold mb-2 text-white" style="font-size: 2.1rem;">Bridging the Gap Between Farmers & Buyers</h2>
                <p class="text-white-50 mb-0" style="font-size: 1rem;">Empowering over 50,000 farmers in Gujarat with fair pricing and verified services.</p>
            </div>
        </div>

        <div class="row g-4">
            <!-- LEFT: Our Vision & Story -->
            <div class="col-lg-6">
                <div class="card card-custom p-4 p-md-5 h-100">
                    <h5 class="fw-bold mb-3" style="color: #0b6028; font-size: 1.25rem;">Our Vision & Story</h5>
                    <p class="text-dark mb-3" style="line-height: 1.6; font-size: 0.95rem;">
                        <strong>Founded in 2026 in Rajkot, Gujarat, KhedutRise was born with a single mission:</strong> to eliminate exploitation in agricultural trading by putting control back in the hands of farmers.
                    </p>
                    <p class="text-muted mb-0" style="line-height: 1.6; font-size: 0.95rem;">
                        Through integrated Escrow Protection, transparent live APMC market data, and P2P tractor rentals, we build trust in every transaction.
                    </p>
                </div>
            </div>

            <!-- RIGHT: Platform Milestones -->
            <div class="col-lg-6">
                <div class="card card-custom p-4 p-md-5 h-100 d-flex flex-column">
                    <h5 class="fw-bold mb-4 text-dark" style="font-size: 1.25rem;">Platform Milestones</h5>

                    <div class="row g-3 mb-4">
                        <div class="col-6">
                            <div class="p-3 rounded-2 border bg-light text-start">
                                <h3 class="fw-bold mb-1" style="color: #0b6028; font-size: 1.6rem;">50,000+</h3>
                                <small class="text-muted" style="font-size: 0.85rem;">Active Farmers</small>
                            </div>
                        </div>
                        <div class="col-6">
                            <div class="p-3 rounded-2 border bg-light text-start">
                                <h3 class="fw-bold mb-1" style="color: #0b6028; font-size: 1.6rem;">₹14.2 L+</h3>
                                <small class="text-muted" style="font-size: 0.85rem;">Ecosystem GMV</small>
                            </div>
                        </div>
                    </div>

                    <div class="p-3 rounded-2 border bg-light mt-auto">
                        <strong class="d-block text-dark mb-1" style="font-size: 0.92rem;">Governed by Gujarat APMC regulatory trade compliances.</strong>
                        <small class="text-muted" style="font-size: 0.82rem;">Verified land deeds and registered machinery audits.</small>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
