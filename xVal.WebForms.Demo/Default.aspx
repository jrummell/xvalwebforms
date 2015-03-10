<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="Default.aspx.cs" Inherits="_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="Server">
    Demos:
    <ul>
        <li><a href="BookingDemo.aspx">Booking demo</a></li>
        <li><a href="BookingDemoValidationGroup.aspx">Booking demo with validation groups</a></li>
        <li><a href="BookingDemoEnableClientScript.aspx">Booking demo with EnableClientScript
            set to false</a></li>
    </ul>
</asp:Content>
