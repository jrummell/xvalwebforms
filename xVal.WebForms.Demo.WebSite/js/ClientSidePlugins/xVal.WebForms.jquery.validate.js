/*
* xVal.WebForms.jquery.validate.js
*
* Created by Carlos Mendible
* 
* Dual licensed under the MIT and GPL licenses:
*   http://www.opensource.org/licenses/mit-license.php
*   http://www.gnu.org/licenses/gpl.html
*/

// Function to enable the CauseValidation behavior for ASP.NET
function SupressValidation(controls) {
    $(document).ready(function() {
        $.each(controls, function(i, control) {
            $("*[id$='" + control + "']").addClass("cancel");
        });
    });
}

// Overrides jQuery validation plug-in functions so ASP.NET validation groups are supported.
(function($) {

    $.fn.validate = function(options) {
        // if nothing is selected, return nothing; can't chain anyway
        if (!this.length) {
            options && options.debug && window.console && console.warn("nothing selected, can't validate, returning nothing");
            return;
        }

        // check if a validator for this form was already created
        var validator = $.data(this[0], 'validator');
        if (validator) {
            return validator;
        }

        validator = new $.validator(options, this[0]);
        $.data(this[0], 'validator', validator);

        if (validator.settings.onsubmit) {

            // allow suppresing validation by adding a cancel class to the submit button
            this.find("input, button").filter(".cancel").click(function() {
                validator.cancelSubmit = true;
            });

            this.find("input, button").filter(":not(.cancel)").click(function() {
                if (validator.settings.valgroups)
                    validator.buttonName = this.name;
            });

            // validate the form on submit
            this.submit(function(event) {
                if (validator.settings.debug)
                // prevent form submit to be able to see console output
                    event.preventDefault();

                function handle() {
                    if (validator.settings.submitHandler) {
                        validator.settings.submitHandler.call(validator, validator.currentForm);
                        return false;
                    }
                    return true;
                }

                // prevent submit for invalid forms or custom submit handlers
                if (validator.cancelSubmit) {
                    validator.cancelSubmit = false;
                    return handle();
                }
                if (validator.buttonName) {
                    $.each(validator.settings.valgroups, function(prop, item) {
                        //if (!item) return;

                        if (item.buttons && item.buttons.length) {
                            $.each(item.buttons, function(i, button) {
                                if (button == validator.buttonName)
                                    validator.fieldSubset = item.fields;
                            });
                        }
                    });
                }
                if (validator.form()) {
                    if (validator.pendingRequest) {
                        validator.formSubmitted = true;
                        return false;
                    }
                    return handle();
                } else {
                    validator.focusInvalid();
                    return false;
                }
            });
        }

        return validator;
    };

    $.extend($.validator.defaults, { valgroups: {} });

    $.extend($.validator.prototype, {
        fullelements: function() {
            var validator = this,
				    rulesCache = {};

            // select all valid inputs inside the form (no submit or reset buttons)
            // workaround $Query([]).add until http://dev.jquery.com/ticket/2114 is solved
            return $([]).add(this.currentForm.elements)
			        .filter(":input")
			        .not(":submit, :reset, :image, [disabled]")
			        .not(this.settings.ignore)
			        .filter(function() {
			            !this.name && validator.settings.debug && window.console && console.error("%o has no name assigned", this);

			            // select only the first element for each name, and only those with rules specified
			            if (this.name in rulesCache || !validator.objectLength($(this).rules()))
			                return false;

			            rulesCache[this.name] = true;
			            return true;
			        })

        },

        elements: function(subset) {
            var validator = this;
            if (!subset) subset = this.fullelements();
            return subset.filter(function() {
                if (validator.fieldSubset && validator.fieldSubset.length > 0) {
                    return $.inArray(this.name, validator.fieldSubset) > -1;
                }

                return true;
            })
        },

        validElements: function() {
            return this.fullelements().not(this.invalidElements());
        }

    });

})(jQuery);

// Overrides xVal.jquery.validate.js plug-in functions so it work with ASP.NET winforms.
(function($) {
    xVal.Plugins["jquery.validate"].AttachValidator = function(elementPrefix, rulesConfig, options) {
        var self = this;
        self._ensureCustomFunctionsRegistered();
        $(function() {
            self._ensureValidationSummaryContainerExistsIfRequired(options);

            for (var i = 0; i < rulesConfig.Fields.length; i++) {
                var fieldName = rulesConfig.Fields[i].FieldName;
                var controlID = rulesConfig.Fields[i].ControlID;
                var fieldRules = rulesConfig.Fields[i].FieldRules;
                var elem = document.getElementById(controlID);

                if (elem) {
                    for (var j = 0; j < fieldRules.length; j++) {
                        var rule = fieldRules[j];
                        if (rule != null) {
                            var ruleName = rule.RuleName;
                            var ruleParams = rule.RuleParameters;
                            var errorText = (typeof (rule.Message) == 'undefined' ? null : rule.Message);
                            self._attachRuleToDOMElement(ruleName, ruleParams, errorText, $(elem), elementPrefix, options);
                        }
                    }
                }
            }
        });

        var _jquery_validate_attachRuleToDOMElement = xVal.Plugins["jquery.validate"]._attachRuleToDOMElement;
        
        xVal.Plugins["jquery.validate"]._attachRuleToDOMElement = function(ruleName, ruleParams, errorText, element, elementPrefix, options) {
            if (ruleName == "Comparison") // Comparison is not yet supported.
                return;

            _jquery_validate_attachRuleToDOMElement.apply(this, [ruleName, ruleParams, errorText, element, elementPrefix, options]);
        };

        xVal.Plugins["jquery.validate"]._ensureFormIsMarkedForValidation = function(formElement, options) {
            if (!formElement.data("isMarkedForValidation")) {
                formElement.data("isMarkedForValidation", true);
                var validationOptions = {
                    errorClass: "field-validation-error",
                    errorElement: "span",
                    highlight: function(element) { $(element).addClass("input-validation-error"); },
                    unhighlight: function(element) { $(element).removeClass("input-validation-error"); }
                };
                if (options.ValidationSummary) {
                    validationOptions.wrapper = "li";
                    validationOptions.errorLabelContainer = "#" + options.ValidationSummary.ElementID + " ul:first";
                }

                // Carlos Mendible: This was added to support Validation Groups.
                if (options.valgroups) {
                    validationOptions.valgroups = options.valgroups;
                }
                // Carlos Mendible: This was added to support Validation Groups.
                
                var validator = formElement.validate(validationOptions);
                if (options.ValidationSummary)
                    this._modifyJQueryValidationElementHidingBehaviourToSupportValidationSummary(validator, options);
            }
        }
    }

})(jQuery);
