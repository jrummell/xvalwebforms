using System.Collections.Generic;
using System.Web.UI;

namespace xVal.WebForms.Tests
{
    public class FakeValidatorCollection : List<IValidator>, IValidatorCollection
    {
    }
}