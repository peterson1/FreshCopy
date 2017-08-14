using CommonTools.Lib.ns11.ExceptionTools;
using System;
using System.Collections.ObjectModel;

namespace CommonTools.Lib.fx45.ViewModelTools
{
    public class CommonLogListVM : ViewModelBase
    {

        public ObservableCollection<string> List { get; } = new ObservableCollection<string>();


        public void Add(string message)
        {
            AsUI(_ => List.Add(message));
        }


        public void Add(Exception ex)
        {
            Add(ex.Info(true, true));
        }
    }
}
