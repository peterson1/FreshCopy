using CommonTools.Lib.ns11.ExceptionTools;
using CommonTools.Lib.ns11.LoggingTools;
using System;
using System.Collections.ObjectModel;

namespace CommonTools.Lib.fx45.ViewModelTools
{
    public class SharedLogListVM : ViewModelBase, ILogList
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
