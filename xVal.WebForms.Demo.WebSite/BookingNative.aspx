<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BookingNative.aspx.cs" Inherits="BookingNative" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>xVal.WebForms Demo</title>
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
            <br />
            <val:ModelObjectValidator ID="valBooking" runat="server" CssClass="validator" Display="Dynamic"
                ModelType="xVal.WebForms.Demo.Booking">
                <ModelProperties>
                    <val:ModelProperty PropertyName="ArrivalDate" ControlToValidate="txtArrivalDate" />
                </ModelProperties>
            </val:ModelObjectValidator>
            <div>
                <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" /></div>
        </fieldset>
    </div>
    <div id="divSuccess" runat="server" class="success" visible="false">
        Your trip has been booked!</div>
    </form>
</body>
</html>
