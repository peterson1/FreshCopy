using CommonTools.Lib.fx45.ByteCompression;
using CommonTools.Lib.fx45.FileSystemTools;
using FluentAssertions;
using System.IO;
using Xunit;

namespace FreshCopy.Tests.CompressionTests
{
    [Trait("Batch", "2")]
    [Trait("Managed LZMA", "Temp IO")]
    public class ManagedLzmaFacts
    {
        [Fact(DisplayName = "Lzma Encode As")]
        public void TestMethod()
        {
            var orig = GetSampleFile(out string origHash);
            var targ = Path.GetTempFileName();
            var newF = Path.GetTempFileName();

            orig.LzmaEncodeAs(targ);
            new FileInfo(targ).Length.Should().BeGreaterThan(0);

            targ.LzmaDecodeAs(newF);
            new FileInfo(newF).Length.Should().BeGreaterThan(0);

            newF.SHA1ForFile().Should().Be(origHash);
        }


        private string GetSampleFile(out string sha1)
        {
            var src = "FluentAssertions.Core.pdb";
            File.Exists(src).Should().BeTrue();
            sha1 = src.SHA1ForFile();
            return Path.GetFullPath(src);
        }
    }
}
