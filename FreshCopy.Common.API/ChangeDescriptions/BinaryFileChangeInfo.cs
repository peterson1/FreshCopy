﻿namespace FreshCopy.Common.API.ChangeDescriptions
{
    public class BinaryFileChangeInfo : ITargetChangeInfo
    {
        public string   FileKey   { get; set; }
        public string   NewSHA1   { get; set; }
    }
}
