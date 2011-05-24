using System;
using System.Web.UI;
using xVal.ServerSide;
using xVal.WebForms;
using xVal.WebForms.Demo;

public partial class BookingDemo : Page
{
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (!IsValid)
        {
            return;
        }

        if (txtNumberOfGuests.Text.Length == 0)
        {
            txtNumberOfGuests.Text = "1";
        }

        Booking booking = new Booking
        {
            ClientName = txtClientName.Text,
            NumberOfGuests = Convert.ToInt32(txtNumberOfGuests.Text),
            ArrivalDate = Convert.ToDateTime(txtArrivalDate.Text),
            EmailAddress = txtEmailAddress.Text
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