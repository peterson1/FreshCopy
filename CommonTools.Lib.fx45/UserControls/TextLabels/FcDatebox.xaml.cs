using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using Xceed.Wpf.Toolkit;

namespace CommonTools.Lib.fx45.UserControls.TextLabels
{
    public partial class FcDatebox : UserControl
    {
        public FcDatebox()
        {
            InitializeComponent();

            LabelWrapping = TextWrapping.Wrap;
            VerticalAlign = VerticalAlignment.Center;

            label.HandleClick();
            dateBox.HandleClick();

            label.Bind3(nameof(Label), TextBlock.TextProperty);
            dateBox.Bind3(nameof(Value), DateTimePicker.ValueProperty);
            dateBox.Bind3(nameof(Watermark), DateTimePicker.WatermarkProperty);

            label.Bind3(nameof(LabelWrapping), TextBlock.TextWrappingProperty);

            label.Bind3(nameof(LabelBrush), TextBlock.ForegroundProperty);
            dateBox.Bind3(nameof(InputBrush), DateTimePicker.ForegroundProperty);

            label.Bind3(nameof(LabelFontStyle), TextBlock.FontStyleProperty);
            dateBox.Bind3(nameof(InputFontStyle), DateTimePicker.FontStyleProperty);

            label.Bind3(nameof(VerticalAlign), FrameworkElement.VerticalAlignmentProperty);
            dateBox.Bind3(nameof(VerticalAlign), DateTimePicker.VerticalAlignmentProperty);

            Loaded += (s, e) =>
            {
                colDef1.Width = LabelWidth ?? new GridLength(70);
                colDefGap.Width = GapWidth ?? new GridLength(8);
                colDef2.Width = InputWidth ?? new GridLength();

                if (LabelBrush == null) LabelBrush = Brushes.Gray;
                if (InputBrush == null) InputBrush = Brushes.Black;

                label.FontWeight = LabelWeight ?? FontWeights.Medium;
                dateBox.FontWeight = InputWeight ?? FontWeights.Normal;

                label.FontSize = LabelSize ?? 12;
                dateBox.FontSize = InputSize ?? 12;

                label.TextAlignment = LabelAlignment ?? TextAlignment.Right;
                dateBox.TextAlignment = InputAlignment ?? TextAlignment.Left;

                dateBox.Format = DateTimeFormat.Custom;
                dateBox.FormatString = ValueFormat ?? "dddd, MMMM d, yyyy";
            };
        }
    }


    internal static class FcDateboxExtensions
    {
        internal static void Bind3(this FrameworkElement elm, string path, DependencyProperty dependencyProp)
        {
            var binding = new Binding(path);
            binding.RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(FcDatebox), 1);
            elm.SetBinding(dependencyProp, binding);
        }

        public static void HandleClick(this DateTimePicker pickr)
        {
            //pickr.MouseRightButtonDown += (s, e) =>
            //{
            //    if (Keyboard.IsKeyDown(Key.LeftShift))
            //        Clipboard.SetText(pickr.Text);
            //};
        }
    }
}
