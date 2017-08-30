using CommonTools.Lib.fx45.LiteDbTools;
using CommonTools.Lib.ns11.DataStructures;
using FluentAssertions;
using System;
using System.IO;
using Xunit;

namespace FreshCopy.Tests.LiteDbToolsTests
{
    [Trait("LiteD K-V Store", "Temp IO")]
    public class LiteDbKeyValueStore1Facts
    {
        [Fact(DisplayName = "Set-Get new string")]
        public void StringSetGet()
        {
            var sut = SUT.Create(out string tmp);
            var val = "sample string value";
            var key = DateTime.Now.ToLongTimeString();

            sut[key] = val;
            sut[key].Should().Be(val);
            sut.GetText(key).Should().Be(val);

            File.Delete(tmp);
        }


        [Fact(DisplayName = "Set-Get new Date")]
        public void DateSetGet()
        {
            var sut = SUT.Create(out string tmp);
            var val = DateTime.Now.AddYears(-100);
            var key = DateTime.Now.ToLongTimeString();

            sut[key] = val;
            sut.GetDate(key).Should().BeCloseTo(val, 1);

            File.Delete(tmp);
        }


        [Fact(DisplayName = "Update string")]
        public void Updatestring()
        {
            var sut = SUT.Create(out string tmp);
            var key = DateTime.Now.ToLongTimeString();

            sut[key] = DateTime.Now.ToLongDateString();
            sut[key] = "A Whole New String";

            sut[key].Should().Be("A Whole New String");
            sut.GetText(key).Should().Be("A Whole New String");

            File.Delete(tmp);
        }


        [Fact(DisplayName = "Update Date")]
        public void UpdateDate()
        {
            var sut = SUT.Create(out string tmp);
            var key = DateTime.Now.ToLongTimeString();

            sut[key] = DateTime.Now.AddYears(-100);
            sut[key] = 27.May(1983);

            sut.GetDate(key).Should().Be(27.May(1983));

            File.Delete(tmp);
        }
    }


    class SUT
    {
        public static IKeyValueStore Create(out string filePath)
        {
            filePath = Path.GetTempFileName();
            return new LiteDbKeyValueStore1(filePath);
        }
    }
}
