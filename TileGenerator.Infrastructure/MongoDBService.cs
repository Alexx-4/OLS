using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;

namespace TileGenerator.Infrastucture
{
   public class MongoDBService
    {
        const string layer = "layer";
        const string fila = "fila";
        const string columna = "column";

        public MongoClient InitConection(string csM, string DbMongo)
        {
            
            MongoClient DB = new MongoClient(csM);
            return DB;
        }
        public void InsertTile(List<string> layers, byte[] tile, string collection, int pos, int f, int cl, string csM,string nameDB)
        {
           var DB = new MongoClient(csM);
            var db = DB.GetDatabase(nameDB);
            var c = db.GetCollection<BsonDocument>(collection);
            var doc = new BsonDocument
            {
                { layer,layers[pos]},
                {fila,f},
                {columna,cl},
                {"tile",tile } };
            c.InsertOneAsync(doc);
        }
    }
}
