using CommonTools.Lib.fx45.FileSystemTools;
using CommonTools.Lib.ns11.ExceptionTools;
using System;
using System.Threading;
using System.Windows;

namespace CommonTools.Lib.fx45.ThreadTools
{
    public class Alert
    {
        public static void Show(string message,
                                MessageBoxImage messageBoxImage = MessageBoxImage.Information,
                                MessageBoxButton messageBoxButton = MessageBoxButton.OK)
            => Show(DateTime.Now.ToShortDateString(),
                    message, messageBoxImage, messageBoxButton);


        public static void Show(Exception ex,
                                string context,
                                bool withTypeNames = true, 
                                bool withShortStackTrace = true,
                                MessageBoxImage messageBoxImage = MessageBoxImage.Error,
                                MessageBoxButton messageBoxButton = MessageBoxButton.OK)
            => Show($"Error on “{context}”",
                    $"{ex.Info(withTypeNames, withShortStackTrace)}", 
                    messageBoxImage, messageBoxButton);


        public static void Show(string caption, 
                                string message,
                                MessageBoxImage messageBoxImage = MessageBoxImage.Information,
                                MessageBoxButton messageBoxButton = MessageBoxButton.OK)
            => new Thread(new ThreadStart(delegate
            {
                var longCap = $"   {caption}  [{DateTime.Now.ToShortTimeString()}]  -  {CurrentExe.GetShortName()} v.{CurrentExe.GetVersion()}";
                MessageBox.Show(message, longCap, messageBoxButton, messageBoxImage);
            }
            )).Start();
    }
}
