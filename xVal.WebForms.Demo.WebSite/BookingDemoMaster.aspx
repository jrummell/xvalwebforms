<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="BookingDemoMaster.aspx.cs" Inherits="BookingDemoMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script src="http://ajax.microsoft.com/ajax/jQuery/jquery-1.3.2.min.js" type="text/javascript"></script>

    <script src="http://ajax.microsoft.com/ajax/jquery.validate/1.7/jquery.validate.min.js" type="text/javascript"></script>

    <script src="js/ClientSidePlugins/xVal.jquery.validate.js" type="text/javascript"></script>
    
    <script src="js/ClientSidePlugins/xVal.WebForms.jquery.validate.js" type="text/javascript"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h1>
        Booking Demo in an Master Page</h1>
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
    <div id="divSuccess" runat="server" visible="false" class="success">
        Your trip has been booked!</div>
</asp:Content>
