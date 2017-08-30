using CommonTools.Lib.ns11.DataStructures;
using LiteDB;
using System;
using System.IO;

namespace CommonTools.Lib.fx45.LiteDbTools
{
    public class LiteDbKeyValueStore1 : IKeyValueStore
    {
        //const string SUB_DIR    = "CommonTools.Lib";
        //const string FILENAME   = "LiteDbKeyValueStore1.LiteDB3";
        const string COLLECTION = "v1";
        const string _ID        = "_id";
        const string VAL        = "val";

        private string _file;


        public LiteDbKeyValueStore1(string filePath)
        {
            _file = filePath;
        }


        public object this[string key]
        {
            get => GetValue(key);
            set => SetValue(key, value);
        }


        public string   GetText(string key) => GetValue(key).AsString;
        public DateTime GetDate(string key) => GetValue(key).AsDateTime;



        private BsonValue GetValue(string key)
        {
            using (var db = ConnectToDB())
            {
                var coll = db.GetCollection(COLLECTION);
                return coll.FindById(key)[VAL];
            }
        }


        private void SetValue(string key, object value)
        {
            using (var db = ConnectToDB())
            {
                var coll = db.GetCollection(COLLECTION);
                var doc = new BsonDocument();
                doc[_ID] = key;
                doc[VAL] = new BsonValue(value);
                coll.Upsert(doc);
            }
        }


        private LiteDatabase ConnectToDB()
        {
            var connStr = LiteDbConn.Str(_file, LiteDbMode.Shared);
            return new LiteDatabase(connStr);
        }


        //private string LocateDatabaseFile()
        //{
        //    var special = Environment.SpecialFolder.LocalApplicationData;
        //    var baseDir = Environment.GetFolderPath(special);
        //    var fullDir = Path.Combine(baseDir, SUB_DIR);
        //    Directory.CreateDirectory(fullDir);
        //    return Path.Combine(fullDir, FILENAME);
        //}
    }
}
