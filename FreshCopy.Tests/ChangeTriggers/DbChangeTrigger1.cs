using FreshCopy.Tests.SampleClasses;
using LiteDB;
using System;

namespace FreshCopy.Tests.ChangeTriggers
{
    class DbChange
    {
        public static void Trigger(string filepath)
        {
            using (var repo = ConnectToRepo(filepath))
            {
                var txt = DateTime.Now.ToLongTimeString();
                repo.Insert(new SampleRecord(txt));
            }
        }


        private static LiteRepository ConnectToRepo(string filepath)
        {
            var mapr = new BsonMapper();
            var conn = $"Filename={filepath}";

            mapr.RegisterAutoId<ulong>(v => v == 0,
                (db, col) => (ulong)db.Count(col) + 1);

            return new LiteRepository(conn, mapr);
        }
    }
}
