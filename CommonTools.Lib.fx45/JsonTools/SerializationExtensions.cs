using CommonTools.Lib.ns11.StringTools;
using Newtonsoft.Json;

namespace CommonTools.Lib.fx45.JsonTools
{
    public static class SerializationExtensions
    {
        public static string SHA1OfJson(this object obj)
            => JsonConvert.SerializeObject(obj).SHA1ForUTF8();
    }
}
