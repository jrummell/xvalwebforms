using System;
using System.Web.UI;
using xVal.ServerSide;
using xVal.WebForms;
using xVal.WebForms.Demo;

public partial class BookingValidationGroups : Page
{
    protected void btnSubmit1_Click(object sender, EventArgs e)
    {
        if (!IsValid)
        {
            return;
        }

        if (txtNumberOfGuests1.Text.Length == 0)
        {
            txtNumberOfGuests1.Text = "1";
        }

        Booking booking = new Booking
                              {
                                  ClientName = txtClientName1.Text,
                                  NumberOfGuests = Convert.ToInt32(txtNumberOfGuests1.Text),
                                  ArrivalDate = Convert.ToDateTime(txtArrivalDate1.Text)
                              };

        try
        {
            BookingManager.PlaceBooking(booking);
        }
        catch (RulesException ex)
        {
            // add each error to the page's validator collection - these will display in a ValidationSummary.
            foreach (ErrorInfo error in ex.Errors)
            {
                Validators.Add(new ValidationError(error.ErrorMessage, "Form1"));
            }

            return;
        }

        divForm1.Visible = false;
        divSuccess1.Visible = true;
    }

    protected void btnSubmit2_Click(object sender, EventArgs e)
    {
        if (!IsValid)
        {
            return;
        }

        if (txtNumberOfGuests2.Text.Length == 0)
        {
            txtNumberOfGuests2.Text = "1";
        }

        Booking booking = new Booking
                              {
                                  ClientName = txtClientName2.Text,
                                  NumberOfGuests = Convert.ToInt32(txtNumberOfGuests2.Text),
                                  ArrivalDate = Convert.ToDateTime(txtArrivalDate2.Text)
                              };

        try
        {
            BookingManager.PlaceBooking(booking);
        }
        catch (RulesException ex)
        {
            // add each error to the page's validator collection - these will display in a ValidationSummary.
            foreach (ErrorInfo error in ex.Errors)
            {
                Validators.Add(new ValidationError(error.ErrorMessage, "Form2"));
            }

            return;
        }

        divForm2.Visible = false;
        divSuccess2.Visible = true;
    }
}