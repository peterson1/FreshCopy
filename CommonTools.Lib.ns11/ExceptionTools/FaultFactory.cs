using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace CommonTools.Lib.ns11.ExceptionTools
{
    public static class Fault
    {
        public static InvalidOperationException CallFirst(string requiredMethod, [CallerMemberName] string callerMemberName = null)
        {
            var msg = $"Please call method “{requiredMethod}” before calling “{callerMemberName}”.";
            return new InvalidOperationException(msg);
        }


        public static InvalidDataException BadData(string description)
        {
            return new InvalidDataException(description);
        }


        public static ArgumentException BadArg(string argumentName, object argumentValue)
        {
            var msg = $"Invalid [{argumentName}]: “{argumentValue}”";
            return new ArgumentException(msg);
        }
    }
}
