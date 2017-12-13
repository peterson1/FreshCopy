using CommonTools.Lib.fx45.FileSystemTools;
using CommonTools.Lib.fx45.FirebaseTools;
using CommonTools.Lib.ns11.DataStructures;
using CommonTools.Lib.ns11.GoogleTools;
using FluentAssertions;
using FreshCopy.Tests.TestTools;
using System.Threading.Tasks;
using Xunit;

namespace FreshCopy.Tests.FirebaseTests
{
    [Trait("Internet", "local cfg")]
    public class AgentStateUpdaterFacts
    {
        private FirebaseConnection _conn;

        [Fact(DisplayName = "Set Running Task")]
        public async Task SetRunningTask()
        {
            var sut = await GetConnectedSUT();
            var txt = Fake.Text;
            await sut.SetRunningTask(txt);

            var path   = string.Join("/", "agents", _conn.AgentID, "AgentState", nameof(AgentState.RunningTask));
            var actual = await _conn.GetText(path);
            actual.Should().Be(txt);

            await _conn.DeleteNode(path);
        }


        [Fact(DisplayName = "Set State")]
        public async Task SetState()
        {
            var sut = await GetConnectedSUT();
            var ver = Fake.Text;
            var sha = Fake.Text;
            var tsk = Fake.Text;

            await sut.SetState(tsk, sha, ver);
            var actual = await sut.GetState();
            actual.ExeVersion .Should().Be(ver);
            actual.ExeSHA1    .Should().Be(sha);
            actual.RunningTask.Should().Be(tsk);

            await _conn.DeleteNode("agents", _conn.AgentID, "AgentState");
        }


        private async Task<AgentStateUpdater> GetConnectedSUT()
        {
            var cred = JsonFile.Read<FirebaseCredentials>("firebaseConxn.cfg");
            _conn    = new FirebaseConnection(cred);
            var isOK = await _conn.Open();
            isOK.Should().BeTrue("Can't connect");
            _conn.IsConnected.Should().BeTrue();
            var agt = new AgentStateUpdater(_conn, cred);
            _conn.AgentID = Fake.Text;
            return agt;
        }
    }
}
