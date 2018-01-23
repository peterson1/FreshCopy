using CommonTools.Lib.ns11.StringTools;
using System.Globalization;
using System.Windows.Controls;

namespace CommonTools.Lib.fx45.ValidationRules
{
    public class NotBlankValidationRule : ValidationRule
    {
        public NotBlankValidationRule()
        {
            ValidatesOnTargetUpdated = true;
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var failed = new ValidationResult(false, "should NOT be blank");
            if (value == null) return failed;
            if (value is string text)
                return text.IsBlank() ? failed : ValidationResult.ValidResult;
            else
                return ValidationResult.ValidResult;
            //return string.IsNullOrWhiteSpace((value ?? "").ToString())
            //    ? new ValidationResult(false, "Field is required.")
            //    : ValidationResult.ValidResult;
        }
    }
}
