using Microsoft.FSharp.Core;
using System;

namespace CommonTools.Lib.fx45.FSharpTools
{
    public static class FSharpOptionExtensions
    {
        public static FSharpOption<T> Create<T>(T value)
            => new FSharpOption<T>(value);

        public static bool IsSome<T>(this FSharpOption<T> opt)
            => FSharpOption<T>.get_IsSome(opt);

        public static bool IsNone<T>(this FSharpOption<T> opt)
            => FSharpOption<T>.get_IsNone(opt);

        public static Nullable<T> ToNullable<T>(this FSharpOption<T> opt)
            where T : struct
            => opt.IsSome() ? opt.Value : (Nullable<T>)null;
    }
}
