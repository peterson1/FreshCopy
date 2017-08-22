using System.Collections.Generic;

namespace CommonTools.Lib.fx45.LiteDbTools
{
    public class AnyLiteDB
    {
        public static long GetMaxId(string dbFilepath, string collectionName = null)
        {
            var readr = new TypelessDbReader1();
            return readr.GetMaxId(dbFilepath, collectionName);
        }


        public static List<string> GetRecords(string dbFilepath,
            long startId, long? endId = null, string collectionName = null)
        {
            var readr = new TypelessDbReader1();
            return readr.GetRecords(dbFilepath, startId, endId, collectionName);
        }


        public static void Insert(string dbFilepath, List<string> records, string collectionName = null)
        {
            var writr = new TypelessDbWriter1();
            writr.Insert(dbFilepath, collectionName, records);
        }
    }
}
