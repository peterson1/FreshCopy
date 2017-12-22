using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CommonTools.Lib.fx45.UserControls.TextLabels
{
    public partial class FcTextbox : UserControl
    {
        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }
        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register("Label", typeof(string), typeof(FcTextbox));

        public string Input
        {
            get { return (string)GetValue(InputProperty); }
            set { SetValue(InputProperty, value); }
        }
        public static readonly DependencyProperty InputProperty =
            DependencyProperty.Register("Input", typeof(string), typeof(FcTextbox));

        public string Watermark
        {
            get { return (string)GetValue(WatermarkProperty); }
            set { SetValue(WatermarkProperty, value); }
        }
        public static readonly DependencyProperty WatermarkProperty =
            DependencyProperty.Register("Watermark", typeof(string), typeof(FcTextbox));



        public GridLength? LabelWidth
        {
            get { return (GridLength?)GetValue(LabelWidthProperty); }
            set { SetValue(LabelWidthProperty, value); }
        }
        public static readonly DependencyProperty LabelWidthProperty =
            DependencyProperty.Register("LabelWidth", typeof(GridLength?), typeof(FcTextbox));

        public GridLength? GapWidth
        {
            get { return (GridLength?)GetValue(GapWidthProperty); }
            set { SetValue(GapWidthProperty, value); }
        }
        public static readonly DependencyProperty GapWidthProperty =
            DependencyProperty.Register("GapWidth", typeof(GridLength?), typeof(FcTextbox));

        public GridLength? InputWidth
        {
            get { return (GridLength?)GetValue(InputWidthProperty); }
            set { SetValue(InputWidthProperty, value); }
        }
        public static readonly DependencyProperty InputWidthProperty =
            DependencyProperty.Register("InputWidth", typeof(GridLength?), typeof(FcTextbox));



        public TextAlignment? LabelAlignment
        {
            get { return (TextAlignment?)GetValue(LabelAlignmentProperty); }
            set { SetValue(LabelAlignmentProperty, value); }
        }
        public static readonly DependencyProperty LabelAlignmentProperty =
            DependencyProperty.Register("LabelAlignment", typeof(TextAlignment?), typeof(FcTextbox));

        public TextAlignment? InputAlignment
        {
            get { return (TextAlignment?)GetValue(InputAlignmentProperty); }
            set { SetValue(InputAlignmentProperty, value); }
        }
        public static readonly DependencyProperty InputAlignmentProperty =
            DependencyProperty.Register("InputAlignment", typeof(TextAlignment?), typeof(FcTextbox));



        public TextWrapping LabelWrapping
        {
            get { return (TextWrapping)GetValue(LabelWrappingProperty); }
            set { SetValue(LabelWrappingProperty, value); }
        }
        public static readonly DependencyProperty LabelWrappingProperty =
            DependencyProperty.Register("LabelWrapping", typeof(TextWrapping), typeof(FcTextbox));

        public TextWrapping InputWrapping
        {
            get { return (TextWrapping)GetValue(InputWrappingProperty); }
            set { SetValue(InputWrappingProperty, value); }
        }
        public static readonly DependencyProperty InputWrappingProperty =
            DependencyProperty.Register("InputWrapping", typeof(TextWrapping), typeof(FcTextbox));



        public Brush LabelBrush
        {
            get { return (Brush)GetValue(LabelBrushProperty); }
            set { SetValue(LabelBrushProperty, value); }
        }
        public static readonly DependencyProperty LabelBrushProperty =
            DependencyProperty.Register("LabelBrush", typeof(Brush), typeof(FcTextbox));

        public Brush InputBrush
        {
            get { return (Brush)GetValue(InputBrushProperty); }
            set { SetValue(InputBrushProperty, value); }
        }
        public static readonly DependencyProperty InputBrushProperty =
            DependencyProperty.Register("InputBrush", typeof(Brush), typeof(FcTextbox));



        public FontWeight? LabelWeight
        {
            get { return (FontWeight?)GetValue(LabelWeightProperty); }
            set { SetValue(LabelWeightProperty, value); }
        }
        public static readonly DependencyProperty LabelWeightProperty =
            DependencyProperty.Register("LabelWeight", typeof(FontWeight?), typeof(FcTextbox));

        public FontWeight? InputWeight
        {
            get { return (FontWeight?)GetValue(InputWeightProperty); }
            set { SetValue(InputWeightProperty, value); }
        }
        public static readonly DependencyProperty InputWeightProperty =
            DependencyProperty.Register("InputWeight", typeof(FontWeight?), typeof(FcTextbox));



        public FontStyle LabelFontStyle
        {
            get { return (FontStyle)GetValue(LabelFontStyleProperty); }
            set { SetValue(LabelFontStyleProperty, value); }
        }
        public static readonly DependencyProperty LabelFontStyleProperty =
            DependencyProperty.Register("LabelFontStyle", typeof(FontStyle), typeof(FcTextbox));

        public FontStyle InputFontStyle
        {
            get { return (FontStyle)GetValue(InputFontStyleProperty); }
            set { SetValue(InputFontStyleProperty, value); }
        }
        public static readonly DependencyProperty InputFontStyleProperty =
            DependencyProperty.Register("InputFontStyle", typeof(FontStyle), typeof(FcTextbox));



        public double? LabelSize
        {
            get { return (double?)GetValue(LabelSizeProperty); }
            set { SetValue(LabelSizeProperty, value); }
        }
        public static readonly DependencyProperty LabelSizeProperty =
            DependencyProperty.Register("LabelSize", typeof(double?), typeof(FcTextbox));

        public double? InputSize
        {
            get { return (double?)GetValue(InputSizeProperty); }
            set { SetValue(InputSizeProperty, value); }
        }
        public static readonly DependencyProperty InputSizeProperty =
            DependencyProperty.Register("InputSize", typeof(double?), typeof(FcTextbox));



        public VerticalAlignment VerticalAlign
        {
            get { return (VerticalAlignment)GetValue(VerticalAlignProperty); }
            set { SetValue(VerticalAlignProperty, value); }
        }
        public static readonly DependencyProperty VerticalAlignProperty =
            DependencyProperty.Register("VerticalAlign", typeof(VerticalAlignment), typeof(FcTextbox));
    }
}
