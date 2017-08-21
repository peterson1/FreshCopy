namespace CommonTools.Lib.ns11.SignalRServers
{
    public struct GlobalServer
    {
        public static ISignalRServerSettings Settings { get; set; }
    }


    public interface ISignalRServerSettings
    {
        string   ServerURL   { get; }
        string   SharedKey   { get; }
    }
}
