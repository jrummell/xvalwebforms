<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BookingDemo.aspx.cs" Inherits="BookingDemo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>xVal.WebForms Demo</title>
</head>
<body>

    <script src="http://ajax.microsoft.com/ajax/jQuery/jquery-1.3.2.min.js" type="text/javascript"></script>

    <script src="http://ajax.microsoft.com/ajax/jquery.validate/1.7/jquery.validate.min.js" type="text/javascript"></script>

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
            <label for="txtNumberOfGuests">
                Number of guests:</label>
            <asp:TextBox ID="txtNumberOfGuests" runat="server" />
            <label for="txtArrivalDate">
                Arrival date:</label>
            <asp:TextBox ID="txtArrivalDate" runat="server" />
            <div>
                <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" /></div>
            <val:ModelValidator ID="valBooking" runat="server" ModelType="xVal.WebForms.Demo.Booking, xVal.WebForms.Demo"
                ValidationSummaryID="valSummary">
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
    </form>
</body>
</html>
