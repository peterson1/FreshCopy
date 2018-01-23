using CommonTools.Lib.fx45.ValidationRules;
using System.Windows.Controls;
using System.Windows.Data;

namespace CommonTools.Lib.fx45.UIExtensions
{
    public static class TextBoxExtensions
    {
        public static void ValidateNonBlank(this TextBox txt)
        {
            if (txt.Tag == null) return;
            var b = new Binding(txt.Tag.ToString());
            b.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            b.ValidationRules.Add(new NotBlankValidationRule());
            txt.SetBinding(TextBox.TextProperty, b);
        }
    }
}
