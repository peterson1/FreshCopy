using CommonTools.Lib.fx45.InputTools;
using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace CommonTools.Lib.fx45.UIExtensions
{
    public static class MenuItemExtensions
    {
        public static void AddDisabledItem(this ItemCollection items, string header)
            => items.Add(new MenuItem
            {
                Header    = header,
                IsEnabled = false
            });


        public static void AddCommandItem(this ItemCollection items, string header, ICommand command)
            => items.Add(new MenuItem
            {
                Header  = header,
                Command = command
            });


        public static void AddCommandItem(this ItemCollection items, 
            string header, Action<object> action, Predicate<object> canExecute = null, string buttonLabel = null)
            => items.Add(new MenuItem
            {
                Header  = header,
                Command = R2Command.Relay(action, canExecute, buttonLabel)
            });
    }
}
