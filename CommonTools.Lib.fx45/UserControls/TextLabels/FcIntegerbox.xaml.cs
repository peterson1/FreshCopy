using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using Xceed.Wpf.Toolkit;

namespace CommonTools.Lib.fx45.UserControls.TextLabels
{
    public partial class FcIntegerbox : UserControl
    {
        public FcIntegerbox()
        {
            InitializeComponent();

            LabelWrapping = TextWrapping.Wrap;
            VerticalAlign = VerticalAlignment.Center;

            label.HandleClick();
            intgrBox.HandleClick();

            label.Bind4(nameof(Label), TextBlock.TextProperty);
            intgrBox.Bind4(nameof(Value), IntegerUpDown.ValueProperty);
            intgrBox.Bind4(nameof(Watermark), IntegerUpDown.WatermarkProperty);

            label.Bind4(nameof(LabelWrapping), TextBlock.TextWrappingProperty);

            label.Bind4(nameof(LabelBrush), TextBlock.ForegroundProperty);
            intgrBox.Bind4(nameof(InputBrush), IntegerUpDown.ForegroundProperty);

            label.Bind4(nameof(LabelFontStyle), TextBlock.FontStyleProperty);
            intgrBox.Bind4(nameof(InputFontStyle), IntegerUpDown.FontStyleProperty);

            label.Bind4(nameof(VerticalAlign), FrameworkElement.VerticalAlignmentProperty);
            intgrBox.Bind4(nameof(VerticalAlign), IntegerUpDown.VerticalAlignmentProperty);

            Loaded += (s, e) =>
            {
                colDef1.Width = LabelWidth ?? new GridLength(70);
                colDefGap.Width = GapWidth ?? new GridLength(8);
                colDef2.Width = InputWidth ?? new GridLength(70);

                if (LabelBrush == null) LabelBrush = Brushes.Gray;
                if (InputBrush == null) InputBrush = Brushes.Black;

                label.FontWeight = LabelWeight ?? FontWeights.Medium;
                intgrBox.FontWeight = InputWeight ?? FontWeights.Normal;

                label.FontSize = LabelSize ?? 12;
                intgrBox.FontSize = InputSize ?? 12;

                label.TextAlignment = LabelAlignment ?? TextAlignment.Right;
                intgrBox.TextAlignment = InputAlignment ?? TextAlignment.Left;

                intgrBox.FormatString = ValueFormat ?? "N0";
            };
        }
    }


    internal static class FcIntegerboxExtensions
    {
        internal static void Bind4(this FrameworkElement elm, string path, DependencyProperty dependencyProp)
        {
            var binding = new Binding(path);
            binding.RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(FcIntegerbox), 1);
            elm.SetBinding(dependencyProp, binding);
        }

        public static void HandleClick(this IntegerUpDown pickr)
        {
            //pickr.MouseRightButtonDown += (s, e) =>
            //{
            //    if (Keyboard.IsKeyDown(Key.LeftShift))
            //        Clipboard.SetText(pickr.Text);
            //};
        }
    }
}
