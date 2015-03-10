using System.Web.UI;

namespace xVal.WebForms
{
    /// <summary>
    /// A <see cref="IValidator"/> with a ValidationGroup property.
    /// </summary>
    public interface IModelValidator : IValidator, IValidationGroup
    {
        
    }
}