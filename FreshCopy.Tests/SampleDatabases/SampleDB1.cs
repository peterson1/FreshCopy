using CommonTools.Lib.fx45.LiteDbTools;
using FreshCopy.Tests.SampleClasses;
using LiteDB;
using System;
using System.IO;

namespace FreshCopy.Tests.SampleDatabases
{
    public static class SampleDB1
    {
        internal static string CreateInTemp(int initialRecords = 2)
        {
            var tmp = Path.GetTempFileName() + ".LiteDB";
            tmp.AddRecords(initialRecords);
            return tmp;
        }


        private static LiteRepository ConnectToRepo(string filepath)
        {
            var mapr = new BsonMapper();
            var conn = LiteDbConn.Str(filepath, LiteDbMode.Shared);

            mapr.RegisterAutoId<ulong>(v => v == 0,
                (db, col) => (ulong)db.Count(col) + 1);

            return new LiteRepository(conn, mapr);
        }


        public static void AddRecords(this string dbPath, int recordCount)
        {
            using (var db = ConnectToRepo(dbPath))
            {
                for (int i = 0; i < recordCount; i++)
                    db.Insert(new SampleRecord(DateTime.Now.ToLongTimeString()));
            }
        }
    }
}
