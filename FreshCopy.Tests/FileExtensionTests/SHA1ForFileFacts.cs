using CommonTools.Lib.fx45.FileSystemTools;
using FluentAssertions;
using FreshCopy.Tests.FileFactories;
using System.IO;
using Xunit;

namespace FreshCopy.Tests.FileExtensionTests
{
    [Trait("SHA1", "Temp IO")]
    public class SHA1ForFileFacts
    {
        [Fact(DisplayName = "File Closed")]
        public void FileClosed()
        {
            var tmp = CreateFile.WithText("abcdef");
            var sha = tmp.SHA1ForFile();
            File.Delete(tmp);
            sha.Should().Be("1f8ac10f-23c5b5bc-1167bda8-4b833e5c-057a77d2");
        }


        [Fact(DisplayName = "File In Use")]
        public void FileInUse()
        {
            var tmp = CreateFile.WithText("abcdef");
            //var loc = new FileStream(tmp, FileMode.Open, FileAccess.Read, FileShare.None);
            var loc = new FileStream(tmp, FileMode.Open, FileAccess.ReadWrite);
            var sha = tmp.SHA1ForFile();
            loc.Dispose();
            File.Delete(tmp);
            sha.Should().Be("1f8ac10f-23c5b5bc-1167bda8-4b833e5c-057a77d2");
        }
    }
}
