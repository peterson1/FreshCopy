namespace CommonTools.Lib.ns11.SignalRClients
{
    public interface IHubClientSettings
    {
        string   ServerURL   { get; }
        string   SharedKey   { get; }
        string   UserAgent   { get; }
    }
}
