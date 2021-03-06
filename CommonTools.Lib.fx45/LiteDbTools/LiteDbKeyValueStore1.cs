﻿using CommonTools.Lib.ns11.DataStructures;
using CommonTools.Lib.ns11.StringTools;
using LiteDB;
using System;

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


        public string    GetText(string key) => GetValue(key)?.AsString;
        public DateTime? GetDate(string key) => GetValue(key)?.AsDateTime;



        private BsonValue GetValue(string key)
        {
            using (var db = ConnectToDB(out LiteCollection<BsonDocument> coll))
            {
                //return coll.FindById(key)[VAL];
                var doc = coll.FindById(key);
                if (doc == null) return null;
                return doc.TryGetValue(VAL, 
                    out BsonValue val) ? val : null;
            }
        }


        private void SetValue(string key, object value)
        {
            using (var db = ConnectToDB(out LiteCollection<BsonDocument> coll))
            {
                var doc = new BsonDocument();
                doc[_ID] = key;
                doc[VAL] = new BsonValue(value);
                coll.Upsert(doc);
            }
        }


        private LiteDatabase ConnectToDB(out LiteCollection<BsonDocument> collection)
        {
            if (_file.IsBlank())
                throw new ArgumentNullException("LiteDB filepath should NOT be BLANK");

            var connStr = LiteDbConn.Str(_file, LiteDbMode.Shared);
            var db      = new LiteDatabase(connStr);
            collection  = db.GetCollection(COLLECTION);
            return db;
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
