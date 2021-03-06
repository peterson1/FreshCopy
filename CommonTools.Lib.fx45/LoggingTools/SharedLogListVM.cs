﻿using CommonTools.Lib.fx45.ThreadTools;
using CommonTools.Lib.fx45.ViewModelTools;
using CommonTools.Lib.ns11.ExceptionTools;
using CommonTools.Lib.ns11.LoggingTools;
using System;
using System.Collections.ObjectModel;

namespace CommonTools.Lib.fx45.LoggingTools
{
    public class SharedLogListVM : ViewModelBase, ILogList
    {

        public ObservableCollection<LogEntry> List { get; } = new ObservableCollection<LogEntry>();


        public void Add(string message)
        {
            //UIThread.Run(() => List.Add(new LogEntry(message)));
            List.Add(message);
        }


        public void Add(Exception ex)
        {
            //Add(ex.Info(true, true));
            List.Add(ex);
        }
    }
}
