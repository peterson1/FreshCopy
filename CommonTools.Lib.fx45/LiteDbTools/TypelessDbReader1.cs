using CommonTools.Lib.ns11.StringTools;
using LiteDB;
using System.Collections.Generic;
using System.Linq;

namespace CommonTools.Lib.fx45.LiteDbTools
{
    public class TypelessDbReader1
    {
        public long GetMaxId(string dbFilepath, string collectionName = null)
        {
            using (var db = CreateConnection(dbFilepath))
            {
                if (collectionName.IsBlank())
                    collectionName = GetSingleCollectionName(db);

                if (!db.CollectionExists(collectionName))
                    return 0;

                return db.GetCollection(collectionName).Max();
            }
        }


        protected string GetSingleCollectionName(LiteDatabase db)
        {
            var names = db.GetCollectionNames().ToList();
            return names.Count == 1 ? names[0] : string.Empty;
        }


        public List<string> GetRecords(string dbFilepath, 
            long startId, long? endId = null, string collectionName = null)
        {
            var list = new List<string>();
            using (var db = CreateConnection(dbFilepath))
            {
                if (collectionName.IsBlank())
                    collectionName = GetSingleCollectionName(db);

                var coll    = db.GetCollection(collectionName);

                if (!endId.HasValue) endId = coll.Max();

                var matches = coll.Find(Query.Between("_id", startId, endId.Value));

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
            => LiteDbConn.Str(filepath, LiteDbMode.ReadOnly);
    }
}
