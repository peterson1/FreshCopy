using CommonTools.Lib.fx45.ThreadTools;
using CommonTools.Lib.ns11.DataStructures;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CommonTools.Lib.fx45.UIExtensions
{
    public static class ConfirmToDeleteExtensions
    {
        public static void ConfirmToDelete<T>(this DataGrid dg,
            Func<T, string> confirmMessage, Action<T> actionBeforeDelete = null,
            string caption = "Confirm to Delete")
        {
            dg.PreviewKeyDown += (s, e) =>
            {
                if (e.Key == Key.Delete)
                {
                    var vm   = dg.DataContext as UIList<T>;
                    var item = vm.CurrentItem;
                    var resp = MessageBox.Show(confirmMessage(item), "   " + caption, MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (resp == MessageBoxResult.Yes)
                    {
                        UIThread.Run(() =>
                        {
                            actionBeforeDelete?.Invoke(item);
                            vm.Remove(item);
                        });
                    }
                }
            };
        }
    }
}
