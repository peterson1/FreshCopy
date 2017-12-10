using CommonTools.Lib.fx45.FileSystemTools;
using CommonTools.Lib.fx45.FirebaseTools;
using CommonTools.Lib.ns11.GoogleTools;
using FluentAssertions;
using FreshCopy.Tests.SampleClasses;
using FreshCopy.Tests.TestTools;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace FreshCopy.Tests.FirebaseTests
{
    [Trait("Internet", "local cfg")]
    public class FirebaseConnectionFacts
    {
        [Theory(DisplayName = "NodeFound")]
        [InlineData("agents", true)]
        [InlineData("agents_x", false)]
        [InlineData("agents/Debug_Update_Checker", true)]
        [InlineData("agents/Debug_Update_Checker_v", false)]
        [InlineData("agents/Debug_Update_Checker/JobOrder/observable", true)]
        public async Task NodeFound(string path, bool expctd)
        {
            var sut = await GetConnectedSUT();
            var found = await sut.NodeFound(path);
            found.Should().Be(expctd);
        }


        [Fact(DisplayName = "Create-Delete Node")]
        public async Task CreateDeleteNode()
        {
            var path = $"{Fake.Text}/{Fake.Text}/{Fake.Text}";
            var obj  = new List<int> { 1, 2, 3 };
            var sut  = await GetConnectedSUT();
            await sut.CreateNode(obj, path);

            var found = await sut.NodeFound(path);
            found.Should().BeTrue();

            var deletd = await sut.DeleteNode(path);
            deletd.Should().BeTrue();
        }


        [Fact(DisplayName = "Add Subscriber")]
        public async Task AddSubscriber()
        {
            var path1 = $"{Fake.Text}/{Fake.Text}";
            var path2 = Fake.Text;
            var sut   = await GetConnectedSUT();
            var rec   = new SampleRecord(Fake.Text);
            await sut.CreateNode(rec, path1, path2);
            var res   = string.Empty;

            sut.AddSubscriber<SampleRecord>(async arg =>
            {
                await sut.DeleteNode(path1, path2);
                res = $"from event: {arg.Text1}";
            }, 
            path1);

            await Task.Delay(1000 * 2);
            res.Should().Be($"from event: {rec.Text1}");
        }


        private async Task<FirebaseConnection> GetConnectedSUT()
        {
            var cred = JsonFile.Read<FirebaseCredentials>("firebaseConxn.cfg");
            var sut  = new FirebaseConnection();
            var isOK = await sut.Open(cred);
            isOK.Should().BeTrue("Can't connect");
            sut.IsConnected.Should().BeTrue();
            return sut;
        }
    }
}
