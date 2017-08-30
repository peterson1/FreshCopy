namespace CommonTools.Lib.fx45.LiteDbTools
{
    public enum LiteDbMode
    {
        Exclusive,
        ReadOnly,
        Shared
    }

    public class LiteDbConn
    {
        public static string Str(string filePath, LiteDbMode mode)
            => $"Filename={filePath};Mode={mode};";
    }
}
