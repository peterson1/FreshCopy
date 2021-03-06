﻿using CommonTools.Lib.fx45.ThreadTools;
using CommonTools.Lib.ns11.ExceptionTools;
using CommonTools.Lib.ns11.LoggingTools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CommonTools.Lib.fx45.LoggingTools
{
    public static class LogListExtensions
    {
        public static void Add(this ObservableCollection<LogEntry> logs, string message)
            => UIThread.Run(() 
                => logs.Add(new LogEntry(message)));


        public static void Add(this ObservableCollection<LogEntry> logs, Exception ex)
            => logs.Add(ex.Info(true, true));


        public static void Add<T>(this ObservableCollection<T> logs, IEnumerable<T> entries)
        {
            foreach (var entry in entries)
                logs.Add(entry);
        }
    }
}
