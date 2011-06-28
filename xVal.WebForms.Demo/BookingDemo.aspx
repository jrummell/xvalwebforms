<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BookingDemo.aspx.cs" Inherits="BookingDemo"
    MasterPageFile="~/Site.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ContentPlaceHolderID="main" runat="server">
    <div id="divForm" runat="server">
        <fieldset class="booking">
            <legend>Booking</legend>
            <asp:ValidationSummary ID="valSummary" runat="server" CssClass="ui-state-error" />
            <val:ModelValidator ID="valBooking" runat="server" CssClass="validator" Display="Dynamic"
                ModelType="xVal.WebForms.Demo.Booking" />
            <label for="txtClientName">
                Your name:</label>
            <asp:TextBox ID="txtClientName" runat="server" />
            <val:ModelPropertyValidator ID="valClientName" runat="server" CssClass="validator"
                ControlToValidate="txtClientName" Display="Dynamic" PropertyName="ClientName"
                ModelType="xVal.WebForms.Demo.Booking" />
            <label for="txtNumberOfGuests">
                Number of guests:</label>
            <asp:TextBox ID="txtNumberOfGuests" runat="server" />
            <val:ModelPropertyValidator ID="valNumberOfGuests" runat="server" CssClass="validator"
                ControlToValidate="txtNumberOfGuests" Display="Dynamic" PropertyName="NumberOfGuests"
                ModelType="xVal.WebForms.Demo.Booking" />
            <label for="txtArrivalDate">
                Arrival date:</label>
            <asp:TextBox ID="txtArrivalDate" runat="server" CssClass="date-picker" />
            <val:ModelPropertyValidator ID="valArrivalDate" runat="server" CssClass="validator"
                ControlToValidate="txtArrivalDate" Display="Dynamic" PropertyName="ArrivalDate"
                ModelType="xVal.WebForms.Demo.Booking" />
            <label for="txtEmailAddress">
                Email address:</label>
            <asp:TextBox ID="txtEmailAddress" runat="server" />
            <val:ModelPropertyValidator ID="valEmailAddress" runat="server" CssClass="validator"
                ControlToValidate="txtEmailAddress" Display="Dynamic" PropertyName="EmailAddress"
                ModelType="xVal.WebForms.Demo.Booking" />
            <div>
                <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" /></div>
        </fieldset>
    </div>
    <div id="divSuccess" runat="server" class="ui-state-highlight" visible="false">
        Your trip has been booked!</div>
</asp:Content>
