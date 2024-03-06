using System;
using MongoDB.Driver;
using Site.Models;

namespace Site.Data
{
    public class MongoDBInitializer
    {
        public static void Initialize(IMongoClient client)
        {
            var database = client.GetDatabase("MyAppDatabase");

            // Optionally: Create collections if they don't exist
            var stubCollection = database.GetCollection<Stub>("stubs");

            // Check if the collection is empty and seed data if necessary
            if (stubCollection.CountDocuments(FilterDefinition<Stub>.Empty) == 0)
            {
                var stubs = new[]
                {
                    new Stub(1,"MongoDB 1", "MongoDB 1", new DateTime(1981, 1, 1)),
                    new Stub(2,"MongoDB 2", "MongoDB 2", new DateTime(1982, 2, 2)),
                    new Stub(3,"MongoDB 3", "MongoDB 3", new DateTime(1983, 3, 3))
                };

                stubCollection.InsertMany(stubs);
            }

            // Optionally: Create indexes
            // var indexKeysDefinition = Builders<Stub>.IndexKeys.Ascending(stub => stub.SomeField);
            // stubCollection.Indexes.CreateOne(new CreateIndexModel<Stub>(indexKeysDefinition));
        }
    }
}