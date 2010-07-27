<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="BookingDemoEnableClientScript.aspx.cs" Inherits="BookingDemoEnableClientScript" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="divForm" runat="server">
        <fieldset class="booking">
            <legend>Booking</legend>
            <asp:ValidationSummary ID="valSummary" runat="server" />
            <label for="txtClientName">
                Your name:</label>
            <asp:TextBox ID="txtClientName" runat="server" />
            <label for="txtNumberOfGuests">
                Number of guests:</label>
            <asp:TextBox ID="txtNumberOfGuests" runat="server" />
            <label for="txtArrivalDate">
                Arrival date:</label>
            <asp:TextBox ID="txtArrivalDate" runat="server" />
            <div>
                <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" /></div>
            <val:ModelValidator ID="valBooking" runat="server" ModelType="xVal.WebForms.Demo.Booking, xVal.WebForms.Demo"
                ValidationSummaryID="valSummary" EnableClientScript="false">
                <ModelProperties>
                    <val:ModelProperty PropertyName="ClientName" ControlToValidate="txtClientName" />
                    <val:ModelProperty PropertyName="NumberOfGuests" ControlToValidate="txtNumberOfGuests" />
                    <val:ModelProperty PropertyName="ArrivalDate" ControlToValidate="txtArrivalDate" />
                </ModelProperties>
            </val:ModelValidator>
        </fieldset>
    </div>
    <div id="divSuccess" runat="server" class="success" visible="false">
        Your trip has been booked!</div>
</asp:Content>
