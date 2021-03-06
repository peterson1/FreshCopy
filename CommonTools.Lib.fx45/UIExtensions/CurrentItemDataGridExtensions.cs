﻿using CommonTools.Lib.ns11.DataStructures;
using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace CommonTools.Lib.fx45.UIExtensions
{
    public static class CurrentItemDataGridExtensions
    {
        public static void EnableOpenCurrent<T>(this DataGrid dg)
        {
            Action act = () => RaiseItemOpened<T>(dg);
            dg.MouseDoubleClick += (s, e) => act.Invoke();
            dg.PreviewKeyDown   += (s, e) =>
            {
                if (e.Key == Key.Enter) act.Invoke();
            };
        }


        private static void RaiseItemOpened<T>(DataGrid dg)
        {
            if (dg.SelectedIndex == -1) return;
            var item = (T)dg.SelectedItem;
            var vm   = dg.DataContext as UIList<T>;
            vm?.RaiseItemOpened(item);
        }
    }
}
