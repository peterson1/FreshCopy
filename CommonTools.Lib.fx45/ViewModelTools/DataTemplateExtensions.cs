using System;
using System.Windows;

namespace CommonTools.Lib.fx45.ViewModelTools
{
    public static class DataTemplateExtensions
    {
        public static void SetTemplate<TData, TUiElement>(this Application app)
            => app.SetTemplate(typeof(TData), typeof(TUiElement));


        public static void SetTemplate(this Application app, Type dataType, Type uiElementType)
        {
            var dt = new DataTemplate(dataType);
            dt.VisualTree = new FrameworkElementFactory(uiElementType);
            var key = new DataTemplateKey(dataType);
            app.Resources.Add(key, dt);
        }
    }
}
