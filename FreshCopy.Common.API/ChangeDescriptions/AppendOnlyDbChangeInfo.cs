namespace FreshCopy.Common.API.ChangeDescriptions
{
    public class AppendOnlyDbChangeInfo : ITargetChangeInfo
    {
        public string   FileKey   { get; set; }
        public long     MaxId     { get; set; }
    }
}
