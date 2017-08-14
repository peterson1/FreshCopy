using CommonTools.Lib.ns11.ExceptionTools;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;

namespace CommonTools.Lib.fx45.ExceptionTools
{
    public static class ErrorVisualizers
    {
        public static void ShowAlert(this Exception ex, 
                                     bool withTypeNames = false, 
                                     bool withShortStackTrace = false,
                                     [CallerMemberName] string callerMemberName = null,
                                     [CallerFilePath] string callerFilePath = null,
                                     [CallerLineNumber] int callerLineNumber = 0)
        {
            var msg = ex.Info(withTypeNames, withShortStackTrace);
            var typ = GetCallerType(callerFilePath);
            var cap = $"‹{typ}› {callerMemberName} [line:{callerLineNumber}]";
            MessageBox.Show(msg, cap, MessageBoxButton.OK, MessageBoxImage.Error);
        }


        private static string GetCallerType(string callerFilePath)
        {
            return Path.GetFileNameWithoutExtension(callerFilePath);
        }
    }
}
