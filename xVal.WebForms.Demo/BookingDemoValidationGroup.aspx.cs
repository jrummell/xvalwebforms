using System;
using xVal.WebForms;
using xVal.WebForms.Demo;

public partial class BookingDemoValidationGroup : ModelPage
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

        BookingManager.PlaceBooking(booking);

        divForm.Visible = false;
        divSuccess.Visible = true;
    }

    protected void btnLogOn_Click(object sender, EventArgs e)
    {
        if (!IsValid)
        {
            return;
        }

        User user = new User {Username = txtUsername.Text, Password = txtPassword.Text};

        UserManager.LogOn(user);

        divLogOn.Visible = false;
        divLoggedOn.Visible = true;
    }
}