<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="Default.aspx.cs" Inherits="_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h1>
        xVal for WebForms Demo</h1>
    Demos:
    <ul>
        <li><a href="BookingDemo.aspx">Booking demo without a master page</a></li>
        <li><a href="BookingDemoMaster.aspx">Booking demo in a master page</a></li>
        <li><a href="BookingValidationGroups.aspx">Booking demo with validation groups</a></li>
        <li><a href="BookingValidationSummary.aspx">Booking demo with a validation summary in
            the master page</a></li>
        <li><a href="BookingDemoEnableClientScript.aspx">Booking demo with EnableClientScript
            set to false</a></li>
    </ul>
</asp:Content>
