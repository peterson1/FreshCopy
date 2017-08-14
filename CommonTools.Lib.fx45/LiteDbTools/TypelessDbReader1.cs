using LiteDB;
using System.Collections.Generic;

namespace CommonTools.Lib.fx45.LiteDbTools
{
    public class TypelessDbReader1
    {
        public long GetLatestId(string dbFilepath, string collectionName)
        {
            using (var db = CreateConnection(dbFilepath))
            {
                if (!db.CollectionExists(collectionName))
                    return 0;

                return db.GetCollection(collectionName).Max();
            }
        }


        public List<string> GetRecords(string dbFilepath, 
            string collectionName, long startId, long endId)
        {
            var list = new List<string>();
            using (var db = CreateConnection(dbFilepath))
            {
                var coll    = db.GetCollection(collectionName);
                var matches = coll.Find(Query.Between("_id", startId, endId));

                foreach (var bson in matches)
                    list.Add(Serialize(bson));
            }
            return list;
        }


        private string Serialize(BsonDocument bson)
        {
            return JsonSerializer.Serialize(bson, false, false);
        }


        protected LiteDatabase CreateConnection(string filepath)
        {
            var connStr = GetConnectionString(filepath);
            return new LiteDatabase(connStr);
        }


        protected virtual string GetConnectionString(string filepath)
        {
            return $"Filename={filepath};Mode=ReadOnly;";
        }
    }
}
