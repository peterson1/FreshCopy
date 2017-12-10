using System.IO;

namespace FreshCopy.Tests.TestTools
{
    public class Fake
    {
        public static string Text
            => Path.GetRandomFileName().Replace(".", "");
    }
}
