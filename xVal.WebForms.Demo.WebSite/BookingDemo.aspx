<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BookingDemo.aspx.cs" Inherits="BookingDemo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>xVal.WebForms Demo</title>
    <script src="http://ajax.microsoft.com/ajax/jQuery/jquery-1.5.min.js" type="text/javascript"></script>
    <script src="http://ajax.microsoft.com/ajax/jquery.validate/1.8/jquery.validate.min.js"
        type="text/javascript"></script>
    <style type="text/css">
        label.error
        {
            color: Red;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div id="divForm" runat="server">
        <fieldset class="booking">
            <legend>Booking</legend>
            <asp:ValidationSummary ID="valSummary" runat="server" CssClass="validation-summary" />
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
            <asp:TextBox ID="txtArrivalDate" runat="server" />
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
    <div id="divSuccess" runat="server" class="success" visible="false">
        Your trip has been booked!</div>
    </form>
</body>
</html>
