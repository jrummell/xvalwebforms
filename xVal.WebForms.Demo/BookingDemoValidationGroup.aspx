<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="BookingDemoValidationGroup.aspx.cs" Inherits="BookingDemoValidationGroup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ContentPlaceHolderID="main" runat="server">
    <div id="divLogOn" runat="server" class="validationGroup">
        <fieldset>
            <legend>Log On</legend>
            <label for="txtUsername">
                Username:</label>
            <asp:TextBox ID="txtUsername" runat="server" />
            <val:ModelPropertyValidator ID="valUsername" runat="server" CssClass="validator"
                ControlToValidate="txtUsername" Display="Dynamic" PropertyName="Username" ModelType="xVal.WebForms.Demo.User"
                ValidationGroup="LogOn" />
            <label for="txtPassword">
                Password:</label>
            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" />
            <val:ModelPropertyValidator ID="valPassword" runat="server" CssClass="validator"
                ControlToValidate="txtPassword" Display="Dynamic" PropertyName="Password" ModelType="xVal.WebForms.Demo.User"
                ValidationGroup="LogOn" />
            <asp:Button ID="btnLogOn" runat="server" Text="Log On" OnClick="btnLogOn_Click" ValidationGroup="LogOn" />
        </fieldset>
    </div>
    <div id="divLoggedOn" runat="server" class="ui-state-highlight" visible="false">
        You are now logged in!</div>
    <div id="divForm" runat="server" class="validationGroup">
        <fieldset class="booking">
            <legend>Booking</legend>
            <val:ModelValidationSummary ID="valSummary" runat="server" CssClass="ui-state-error"
                ValidationGroup="Booking" />
            <val:ModelValidator ID="valBooking" runat="server" CssClass="validator" Display="Dynamic"
                ModelType="xVal.WebForms.Demo.Booking" ValidationGroup="Booking" />
            <label for="txtClientName">
                Your name:</label>
            <asp:TextBox ID="txtClientName" runat="server" />
            <val:ModelPropertyValidator ID="valClientName" runat="server" CssClass="validator"
                ControlToValidate="txtClientName" Display="Dynamic" PropertyName="ClientName"
                ModelType="xVal.WebForms.Demo.Booking" ValidationGroup="Booking" />
            <label for="txtNumberOfGuests">
                Number of guests:</label>
            <asp:TextBox ID="txtNumberOfGuests" runat="server" />
            <val:ModelPropertyValidator ID="valNumberOfGuests" runat="server" CssClass="validator"
                ControlToValidate="txtNumberOfGuests" Display="Dynamic" PropertyName="NumberOfGuests"
                ModelType="xVal.WebForms.Demo.Booking" ValidationGroup="Booking" />
            <label for="txtArrivalDate">
                Arrival date:</label>
            <asp:TextBox ID="txtArrivalDate" runat="server" CssClass="date-picker" />
            <val:ModelPropertyValidator ID="valArrivalDate" runat="server" CssClass="validator"
                ControlToValidate="txtArrivalDate" Display="Dynamic" PropertyName="ArrivalDate"
                ModelType="xVal.WebForms.Demo.Booking" ValidationGroup="Booking" />
            <label for="txtEmailAddress">
                Email address:</label>
            <asp:TextBox ID="txtEmailAddress" runat="server" />
            <val:ModelPropertyValidator ID="valEmailAddress" runat="server" CssClass="validator"
                ControlToValidate="txtEmailAddress" Display="Dynamic" PropertyName="EmailAddress"
                ModelType="xVal.WebForms.Demo.Booking" ValidationGroup="Booking" />
            <div>
                <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click"
                    ValidationGroup="Booking" /></div>
        </fieldset>
    </div>
    <div id="divSuccess" runat="server" class="ui-state-highlight" visible="false">
        Your trip has been booked!</div>
</asp:Content>
