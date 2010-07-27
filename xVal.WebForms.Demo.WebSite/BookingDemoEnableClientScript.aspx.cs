using System;
using System.Web.UI;
using xVal.ServerSide;
using xVal.WebForms;
using xVal.WebForms.Demo;

public partial class BookingDemoEnableClientScript : Page
{
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (!IsValid)
        {
            return;
        }

        int numberOfGuests;
        Int32.TryParse(txtNumberOfGuests.Text, out numberOfGuests);

        DateTime arrivalDate;
        DateTime.TryParse(txtArrivalDate.Text, out arrivalDate);

        Booking booking = new Booking
                              {
                                  ClientName = txtClientName.Text,
                                  NumberOfGuests = numberOfGuests,
                                  ArrivalDate = arrivalDate
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
                Validators.Add(new ValidationError(error.ErrorMessage));
            }

            return;
        }

        divForm.Visible = false;
        divSuccess.Visible = true;
    }
}