using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using Xceed.Wpf.Toolkit;

namespace CommonTools.Lib.fx45.UserControls.TextLabels
{
    public partial class FcTextbox : UserControl
    {
        public FcTextbox()
        {
            InitializeComponent();

            LabelWrapping = TextWrapping.Wrap;
            InputWrapping = TextWrapping.Wrap;
            VerticalAlign = VerticalAlignment.Center;

            label.HandleClick();
            inputText.HandleClick();

            label.FcBind(nameof(Label), TextBlock.TextProperty);
            inputText.FcBind(nameof(Input), WatermarkTextBox.TextProperty);
            inputText.FcBind(nameof(Watermark), WatermarkTextBox.WatermarkProperty);

            label.FcBind(nameof(LabelWrapping), TextBlock.TextWrappingProperty);
            inputText.FcBind(nameof(InputWrapping), WatermarkTextBox.TextWrappingProperty);

            label.FcBind(nameof(LabelBrush), TextBlock.ForegroundProperty);
            inputText.FcBind(nameof(InputBrush), WatermarkTextBox.ForegroundProperty);

            label.FcBind(nameof(LabelFontStyle), TextBlock.FontStyleProperty);
            inputText.FcBind(nameof(InputFontStyle), WatermarkTextBox.FontStyleProperty);

            label.FcBind(nameof(VerticalAlign), FrameworkElement.VerticalAlignmentProperty);
            inputText.FcBind(nameof(VerticalAlign), WatermarkTextBox.VerticalAlignmentProperty);

            Loaded += (s, e) =>
            {
                colDef1.Width = LabelWidth ?? new GridLength(70);
                colDefGap.Width = GapWidth ?? new GridLength(8);
                colDef2.Width = InputWidth ?? new GridLength(300);

                if (LabelBrush == null) LabelBrush = Brushes.Gray;
                if (InputBrush == null) InputBrush = Brushes.Black;

                label.FontWeight = LabelWeight ?? FontWeights.Medium;
                inputText.FontWeight = InputWeight ?? FontWeights.Normal;

                label.FontSize = LabelSize ?? 12;
                inputText.FontSize = InputSize ?? 12;

                label.TextAlignment = LabelAlignment ?? TextAlignment.Right;
                inputText.TextAlignment = InputAlignment ?? TextAlignment.Left;
            };
        }
    }


    internal static class UIExtensions
    {
        internal static void FcBind(this FrameworkElement elm, string path, DependencyProperty dependencyProp)
        {
            var binding = new Binding(path);
            binding.RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(FcTextbox), 1);
            elm.SetBinding(dependencyProp, binding);
        }

        public static void HandleClick(this WatermarkTextBox txt)
        {
            txt.MouseRightButtonDown += (s, e) =>
            {
                if (Keyboard.IsKeyDown(Key.LeftShift))
                    Clipboard.SetText(txt.Text);
            };
        }
    }
}
