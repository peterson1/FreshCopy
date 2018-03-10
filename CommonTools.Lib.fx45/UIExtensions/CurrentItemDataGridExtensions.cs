using CommonTools.Lib.ns11.DataStructures;
using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace CommonTools.Lib.fx45.UIExtensions
{
    public static class CurrentItemDataGridExtensions
    {
        public static void EnableOpenCurrent<T>(this DataGrid dg)
        {
            var vm = dg.DataContext as UIList<T>;
            Action act = () => vm.RaiseCurrentItemOpened();

            dg.MouseDoubleClick += (s, e) => act.Invoke();
            dg.PreviewKeyDown += (s, e) =>
            {
                if (e.Key == Key.Enter) act.Invoke();
            };
        }
    }
}
