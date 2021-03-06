﻿using System;
using System.Collections.ObjectModel;

namespace CommonTools.Lib.ns11.LoggingTools
{
    public interface ILogList
    {
        ObservableCollection<LogEntry> List { get; }

        void  Add  (string message);
        void  Add  (Exception exception);
    }
}
