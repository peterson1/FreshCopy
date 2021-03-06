﻿using System;

namespace CommonTools.Lib.ns11.LoggingTools
{
    public struct LogEntry
    {
        public LogEntry(string message)
        {
            Timestamp = DateTime.Now;
            Message   = message;
        }


        public DateTime  Timestamp  { get; }
        public string    Message    { get; }
    }
}
