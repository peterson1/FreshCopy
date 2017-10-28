using CommonTools.Lib.fx45.FileSystemTools;
using FluentAssertions;

namespace FreshCopy.Tests.CustomAssertions
{
    public static class FilePathAssertions
    {
        public static void MustMatchHashOf(this string sutFile, string masterFile)
        {
            var expctd = masterFile.SHA1ForFile();
            var actual = sutFile.SHA1ForFile();
            actual.Should().Be(expctd);
        }
    }
}
