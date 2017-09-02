using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace CommonTools.Lib.ns11.ExceptionTools
{
    public static class Fault
    {
        public static InvalidOperationException CallFirst(string requiredMethod, [CallerMemberName] string callerMemberName = null)
            => new InvalidOperationException(
                $"Please call method “{requiredMethod}” before calling “{callerMemberName}”.");


        public static InvalidDataException BadData(string description)
            => new InvalidDataException(description);


        public static NullReferenceException NullRef(string variableName)
            => new NullReferenceException(
                $"Variable [{variableName}] should NOT be NULL.");


        public static ArgumentException BadArg(string argumentName, object argumentValue)
            => new ArgumentException(
                $"Invalid [{argumentName}]: “{argumentValue}”");
    }
}
