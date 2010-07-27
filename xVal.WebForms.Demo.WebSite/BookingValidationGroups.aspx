<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BookingValidationGroups.aspx.cs"
    Inherits="BookingValidationGroups" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>xVal.WebForms Demo</title>
</head>
<body>

    <script src="http://ajax.microsoft.com/ajax/jQuery/jquery-1.3.2.min.js" type="text/javascript"></script>

    <script src="http://ajax.microsoft.com/ajax/jquery.validate/1.7/jquery.validate.min.js" type="text/javascript"></script>

    <script src="js/ClientSidePlugins/xVal.jquery.validate.js" type="text/javascript"></script>

    <script src="js/ClientSidePlugins/xVal.WebForms.jquery.validate.js" type="text/javascript"></script>

    <form id="form1" runat="server">
    <div id="validationGroup1">
        <div id="divForm1" runat="server">
            <fieldset class="booking">
                <legend>Booking 1</legend>
                <asp:ValidationSummary ID="valSummary1" runat="server" ValidationGroup="Form1" />
                <label for="txtClientName1">
                    Your name:</label>
                <asp:TextBox ID="txtClientName1" runat="server" ValidationGroup="Form1" />
                <label for="txtNumberOfGuests1">
                    Number of guests:</label>
                <asp:TextBox ID="txtNumberOfGuests1" runat="server" ValidationGroup="Form1" />
                <label for="txtArrivalDate1">
                    Arrival date:</label>
                <asp:TextBox ID="txtArrivalDate1" runat="server" ValidationGroup="Form1" />
                <div>
                    <asp:Button ID="btnSubmit1" runat="server" Text="Submit" OnClick="btnSubmit1_Click"
                        ValidationGroup="Form1" />
                    <asp:Button ID="btnSumbitWithoutValidation" runat="server" Text="Submit without Validation"
                        CausesValidation="false" /></div>
                <val:ModelValidator ID="valBooking1" runat="server" ModelType="xVal.WebForms.Demo.Booking, xVal.WebForms.Demo"
                    ValidationSummaryID="valSummary1" ValidationGroup="Form1">
                    <ModelProperties>
                        <val:ModelProperty PropertyName="ClientName" ControlToValidate="txtClientName1" />
                        <val:ModelProperty PropertyName="NumberOfGuests" ControlToValidate="txtNumberOfGuests1" />
                        <val:ModelProperty PropertyName="ArrivalDate" ControlToValidate="txtArrivalDate1" />
                    </ModelProperties>
                </val:ModelValidator>
            </fieldset>
        </div>
        <div id="divSuccess1" runat="server" class="success" visible="false">
            Your trip has been booked!</div>
    </div>
    <div id="validationGroup2">
        <div id="divForm2" runat="server">
            <fieldset class="booking">
                <legend>Booking 2</legend>
                <asp:ValidationSummary ID="valSummary2" runat="server" ValidationGroup="Form2" />
                <label for="txtClientName2">
                    Your name:</label>
                <asp:TextBox ID="txtClientName2" runat="server" ValidationGroup="Form2" />
                <label for="txtNumberOfGuests2">
                    Number of guests:</label>
                <asp:TextBox ID="txtNumberOfGuests2" runat="server" ValidationGroup="Form2" />
                <label for="txtArrivalDate2">
                    Arrival date:</label>
                <asp:TextBox ID="txtArrivalDate2" runat="server" ValidationGroup="Form2" />
                <div>
                    <asp:Button ID="btnSubmit2" runat="server" Text="Submit" OnClick="btnSubmit2_Click"
                        ValidationGroup="Form2" /></div>
                <val:ModelValidator ID="valBooking2" runat="server" ModelType="xVal.WebForms.Demo.Booking, xVal.WebForms.Demo"
                    ValidationSummaryID="valSummary2" ValidationGroup="Form2">
                    <ModelProperties>
                        <val:ModelProperty PropertyName="ClientName" ControlToValidate="txtClientName2" />
                        <val:ModelProperty PropertyName="NumberOfGuests" ControlToValidate="txtNumberOfGuests2" />
                        <val:ModelProperty PropertyName="ArrivalDate" ControlToValidate="txtArrivalDate2" />
                    </ModelProperties>
                </val:ModelValidator>
            </fieldset>
        </div>
        <div id="divSuccess2" runat="server" class="booking" visible="false">
            Your trip has been booked!</div>
    </div>
    </form>
</body>
</html>
