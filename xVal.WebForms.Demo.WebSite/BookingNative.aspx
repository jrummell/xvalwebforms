<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BookingNative.aspx.cs" Inherits="BookingNative" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>xVal.WebForms Demo</title>
</head>
<body>
    <script src="http://ajax.microsoft.com/ajax/jQuery/jquery-1.3.2.min.js" type="text/javascript"></script>
    <script src="http://ajax.microsoft.com/ajax/jquery.validate/1.7/jquery.validate.min.js"
        type="text/javascript"></script>
    <script src="js/ClientSidePlugins/xVal.jquery.validate.js" type="text/javascript"></script>
    <script src="js/ClientSidePlugins/xVal.WebForms.jquery.validate.js" type="text/javascript"></script>
    <form id="form1" runat="server">
    <div id="divForm" runat="server">
        <fieldset class="booking">
            <legend>Booking</legend>
            <asp:ValidationSummary ID="valSummary" runat="server" />
            <label for="txtClientName">
                Your name:</label>
            <asp:TextBox ID="txtClientName" runat="server" />
            <val:ModelPropertyValidator ID="valClientName" runat="server" ControlToValidate="txtClientName"
                Display="Dynamic" PropertyName="ClientName" TypeName="xVal.WebForms.Demo.Booking" />
            <label for="txtNumberOfGuests">
                Number of guests:</label>
            <asp:TextBox ID="txtNumberOfGuests" runat="server" />
            <val:ModelPropertyValidator ID="valNumberOfGuests" runat="server" ControlToValidate="txtNumberOfGuests"
                Display="Dynamic" PropertyName="NumberOfGuests" TypeName="xVal.WebForms.Demo.Booking" />
            <label for="txtArrivalDate">
                Arrival date:</label>
            <asp:TextBox ID="txtArrivalDate" runat="server" />
            <val:ModelPropertyValidator ID="valArrivalDate" runat="server" ControlToValidate="txtArrivalDate"
                Display="Dynamic" PropertyName="ArrivalDate" TypeName="xVal.WebForms.Demo.Booking" />
            <div>
                <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" /></div>
        </fieldset>
    </div>
    <div id="divSuccess" runat="server" class="success" visible="false">
        Your trip has been booked!</div>
    </form>
</body>
</html>
