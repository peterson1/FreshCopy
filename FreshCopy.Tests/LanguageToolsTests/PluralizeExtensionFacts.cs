using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonTools.Lib.ns11.LanguageTools;
using FluentAssertions;
using Xunit;

namespace FreshCopy.Tests.LanguageToolsTests
{
    [Trait("Batch", "1")]
    [Trait("Pluralize", "Solitary")]
    public class PluralizeExtensionFacts
    {
        [Theory]
        [InlineData("dog", 0, "0 dogs")]
        [InlineData("dog", 1, "1 dog")]
        [InlineData("dog", 2, "2 dogs")]
        [InlineData("dog", 1234, "1,234 dogs")]
        [InlineData("dog", null, "NULL dogs")]
        [InlineData("dog", "abc", "NaN dogs")]
        //[InlineData("dog", null, "dogs")]
        public void TestMethod(string singular, object count, string expctd)
        {
            singular.Pluralize(count).Should().Be(expctd);
        }
    }
}
