using MongoDB.Driver;

namespace Site.Data
{
    public class MyMongoDbService
    {
        private readonly IMongoClient _client;
        private readonly IMongoDatabase _database;

        public MyMongoDbService(IMongoClient client)
        {
            _client = client;
            _database = _client.GetDatabase("IKT453"); // Replace with your database name
        }
        
    }

}