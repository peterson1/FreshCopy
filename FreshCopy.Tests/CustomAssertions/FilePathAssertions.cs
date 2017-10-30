using CommonTools.Lib.fx45.FileSystemTools;
using CommonTools.Lib.fx45.LiteDbTools;
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


        public static void MustMatchMaxIdOf(this string sutDB, string masterDB)
        {
            var expctd = AnyLiteDB.GetMaxId(masterDB);
            var actual = AnyLiteDB.GetMaxId(sutDB);
            actual.Should().Be(expctd);
        }
    }
}
