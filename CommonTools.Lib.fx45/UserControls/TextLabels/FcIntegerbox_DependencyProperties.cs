using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CommonTools.Lib.fx45.UserControls.TextLabels
{
    public partial class FcIntegerbox : UserControl
    {
        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }
        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register("Label", typeof(string), typeof(FcIntegerbox));

        public int? Value
        {
            get { return (int?)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(int?), typeof(FcIntegerbox));

        public string Watermark
        {
            get { return (string)GetValue(WatermarkProperty); }
            set { SetValue(WatermarkProperty, value); }
        }
        public static readonly DependencyProperty WatermarkProperty =
            DependencyProperty.Register("Watermark", typeof(string), typeof(FcIntegerbox));



        public string ValueFormat
        {
            get { return (string)GetValue(ValueFormatProperty); }
            set { SetValue(ValueFormatProperty, value); }
        }
        public static readonly DependencyProperty ValueFormatProperty =
            DependencyProperty.Register("ValueFormat", typeof(string), typeof(FcIntegerbox));



        public GridLength? LabelWidth
        {
            get { return (GridLength?)GetValue(LabelWidthProperty); }
            set { SetValue(LabelWidthProperty, value); }
        }
        public static readonly DependencyProperty LabelWidthProperty =
            DependencyProperty.Register("LabelWidth", typeof(GridLength?), typeof(FcIntegerbox));

        public GridLength? GapWidth
        {
            get { return (GridLength?)GetValue(GapWidthProperty); }
            set { SetValue(GapWidthProperty, value); }
        }
        public static readonly DependencyProperty GapWidthProperty =
            DependencyProperty.Register("GapWidth", typeof(GridLength?), typeof(FcIntegerbox));

        public GridLength? InputWidth
        {
            get { return (GridLength?)GetValue(InputWidthProperty); }
            set { SetValue(InputWidthProperty, value); }
        }
        public static readonly DependencyProperty InputWidthProperty =
            DependencyProperty.Register("InputWidth", typeof(GridLength?), typeof(FcIntegerbox));



        public TextAlignment? LabelAlignment
        {
            get { return (TextAlignment?)GetValue(LabelAlignmentProperty); }
            set { SetValue(LabelAlignmentProperty, value); }
        }
        public static readonly DependencyProperty LabelAlignmentProperty =
            DependencyProperty.Register("LabelAlignment", typeof(TextAlignment?), typeof(FcIntegerbox));

        public TextAlignment? InputAlignment
        {
            get { return (TextAlignment?)GetValue(InputAlignmentProperty); }
            set { SetValue(InputAlignmentProperty, value); }
        }
        public static readonly DependencyProperty InputAlignmentProperty =
            DependencyProperty.Register("InputAlignment", typeof(TextAlignment?), typeof(FcIntegerbox));



        public TextWrapping LabelWrapping
        {
            get { return (TextWrapping)GetValue(LabelWrappingProperty); }
            set { SetValue(LabelWrappingProperty, value); }
        }
        public static readonly DependencyProperty LabelWrappingProperty =
            DependencyProperty.Register("LabelWrapping", typeof(TextWrapping), typeof(FcIntegerbox));

        //public TextWrapping InputWrapping
        //{
        //    get { return (TextWrapping)GetValue(InputWrappingProperty); }
        //    set { SetValue(InputWrappingProperty, value); }
        //}
        //public static readonly DependencyProperty InputWrappingProperty =
        //    DependencyProperty.Register("InputWrapping", typeof(TextWrapping), typeof(FcIntegerbox));



        public Brush LabelBrush
        {
            get { return (Brush)GetValue(LabelBrushProperty); }
            set { SetValue(LabelBrushProperty, value); }
        }
        public static readonly DependencyProperty LabelBrushProperty =
            DependencyProperty.Register("LabelBrush", typeof(Brush), typeof(FcIntegerbox));

        public Brush InputBrush
        {
            get { return (Brush)GetValue(InputBrushProperty); }
            set { SetValue(InputBrushProperty, value); }
        }
        public static readonly DependencyProperty InputBrushProperty =
            DependencyProperty.Register("InputBrush", typeof(Brush), typeof(FcIntegerbox));



        public FontWeight? LabelWeight
        {
            get { return (FontWeight?)GetValue(LabelWeightProperty); }
            set { SetValue(LabelWeightProperty, value); }
        }
        public static readonly DependencyProperty LabelWeightProperty =
            DependencyProperty.Register("LabelWeight", typeof(FontWeight?), typeof(FcIntegerbox));

        public FontWeight? InputWeight
        {
            get { return (FontWeight?)GetValue(InputWeightProperty); }
            set { SetValue(InputWeightProperty, value); }
        }
        public static readonly DependencyProperty InputWeightProperty =
            DependencyProperty.Register("InputWeight", typeof(FontWeight?), typeof(FcIntegerbox));



        public FontStyle LabelFontStyle
        {
            get { return (FontStyle)GetValue(LabelFontStyleProperty); }
            set { SetValue(LabelFontStyleProperty, value); }
        }
        public static readonly DependencyProperty LabelFontStyleProperty =
            DependencyProperty.Register("LabelFontStyle", typeof(FontStyle), typeof(FcIntegerbox));

        public FontStyle InputFontStyle
        {
            get { return (FontStyle)GetValue(InputFontStyleProperty); }
            set { SetValue(InputFontStyleProperty, value); }
        }
        public static readonly DependencyProperty InputFontStyleProperty =
            DependencyProperty.Register("InputFontStyle", typeof(FontStyle), typeof(FcIntegerbox));



        public double? LabelSize
        {
            get { return (double?)GetValue(LabelSizeProperty); }
            set { SetValue(LabelSizeProperty, value); }
        }
        public static readonly DependencyProperty LabelSizeProperty =
            DependencyProperty.Register("LabelSize", typeof(double?), typeof(FcIntegerbox));

        public double? InputSize
        {
            get { return (double?)GetValue(InputSizeProperty); }
            set { SetValue(InputSizeProperty, value); }
        }
        public static readonly DependencyProperty InputSizeProperty =
            DependencyProperty.Register("InputSize", typeof(double?), typeof(FcIntegerbox));



        public VerticalAlignment VerticalAlign
        {
            get { return (VerticalAlignment)GetValue(VerticalAlignProperty); }
            set { SetValue(VerticalAlignProperty, value); }
        }
        public static readonly DependencyProperty VerticalAlignProperty =
            DependencyProperty.Register("VerticalAlign", typeof(VerticalAlignment), typeof(FcIntegerbox));
    }
}
