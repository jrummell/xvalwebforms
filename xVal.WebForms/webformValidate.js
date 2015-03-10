// based on the example by Dave Ward at http://encosia.com/asp-net-webforms-validation-groups-with-jquery-validation/

var $webformValidate =
{
    init: function (formSelector)
    {
        if (typeof (jQuery) != "function")
        {
            throw "jQuery is required for $webformValidate."
        }

        if (typeof ($(formSelector).validate) != "function")
        {
            throw "jQuery Validate is required for $webformValidate.";
        }

        // Initialize validation on the entire ASP.NET form.
        $(formSelector).validate({
            // This prevents validation from running on every
            //  form submission by default.
            onsubmit: false
        });

        // Search for controls that are not marked with disableValdiation, 
        // and wire their click event up.
        $(":submit").not(".disableValidation").click(this.validate);

        // Select any input[type=text] elements within a validation group
        //  and attach keydown handlers to all of them.
        $(".validationGroup :text").keydown(function (evt)
        {
            // Only execute validation if the key pressed was enter.
            if (evt.keyCode == 13)
            {
                // Find and store the next input element that comes after the
                //  one in which the enter key was pressed.
                var $nextInput = $(this).nextAll(":input:first");

                // If the next input is a submit button, go into validation.
                //  Else, focus the next form element as if enter == tab.
                if ($nextInput.is(":submit"))
                {
                    $webformValidate.validate(evt);
                }
                else
                {
                    evt.preventDefault();
                    $nextInput.focus();
                }
            }
        });
    },

    validate: function (evt)
    {
        // Ascend from the button or input element that triggered the 
        //  event until we find a container element flagged with 
        //  .validationGroup and store a reference to that element.
        var $group = $(this).parents(".validationGroup");

        if ($group.length == 0)
        {
            // if this is not part of a validation group, get the parent form.
            $group = $(this).parents("form");
        }

        var isValid = true;

        // Descending from that .validationGroup element, find any input
        //  elements within it, iterate over them, and run validation on 
        //  each of them.
        $group.find(":input").each(function (i, item)
        {
            if (!$(item).valid())
            {
                isValid = false;
            }
        });

        // If any fields failed validation, prevent the button's click 
        //  event from triggering form submission.
        if (!isValid)
        {
            evt.preventDefault();

            return false;
        }
    }
};