using System;
using System.Windows;

namespace CommonTools.Lib.fx45.ThreadTools
{
    public static class UIThread
    {
        public static void Run(Action action)
        {
            Application.Current.Dispatcher.Invoke(action);
        }
    }
}
