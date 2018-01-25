using CommonTools.Lib.ns11.StringTools;
using System;
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
            var failed = new ValidationResult(false, "is required");
            var passed = ValidationResult.ValidResult;
            if (value == null) return failed;
            if (value is string text)
                return text.IsBlank() ? failed : passed;
            else if (value is DateTime date)
                return date == DateTime.MinValue ? failed : passed;
            else
                return passed;
        }
    }
}
