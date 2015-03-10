namespace xVal.WebForms
{
    public interface IValidationScriptManager
    {
        /// <summary>
        /// Registers the scripts.
        /// </summary>
        /// <param name="rules">The rules.</param>
        void RegisterScripts(RuleCollection rules);
    }
}