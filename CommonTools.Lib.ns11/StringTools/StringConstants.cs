using System;

namespace CommonTools.Lib.ns11.StringTools
{
    public struct L
    {
        public static string f => Environment.NewLine;
        public static string F => L.f + L.f;
    }
}
