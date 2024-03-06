using System;
using System.Linq;
using System.Threading.Tasks;
using Cassandra;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Site.Data;
using Site.Models;

namespace Site.Controllers
{
    public class DataController : Controller
    {
        private readonly ILogger<DataController> _logger;
        private readonly ApplicationDbContext _dbContext; // For PostgreSQL
        private readonly CassandraService _cassandraService; // For Cassandra
        private readonly IMongoClient _mongoClient; // For MongoDB
        public DataController(ILogger<DataController> logger, 
            ApplicationDbContext dbContext,
            CassandraService cassandraService, 
            IMongoClient mongoClient)
        {
            _logger = logger;
            _dbContext = dbContext;
            _cassandraService = cassandraService;
            _mongoClient = mongoClient;
        }

        public async Task<IActionResult> PostgresData()
        {
            var data = await _dbContext.Stubs.ToListAsync();
            return View(data);
        }

        public async Task<IActionResult> MongoData()
        {
            var database = _mongoClient.GetDatabase("MyAppDatabase"); // replace with your database name
            var collection = database.GetCollection<Stub>("stubs"); // replace with your collection name and model
            var data = await collection.Find(_ => true).ToListAsync();
            return View("MongoData", data);
        }

        public async Task<IActionResult> CassandraData()
        {
            // Fetch data from the 'stubs' table in the 'my_keyspace' keyspace
            var rs = await _cassandraService.ExecuteAsync(new SimpleStatement("SELECT * FROM my_keyspace.stubs"));

            // Map the data from Cassandra rows to your Stub model
            var data = rs.Select(row => new Stub
            {
                Id = row.GetValue<int>("id"),
                FirstName = row.GetValue<string>("first_name"),
                LastName = row.GetValue<string>("last_name"),
                Birthdate = row.GetValue<DateTime>("birthdate")
            }).ToList();

            return View("CassandraData", data); // Pass the data to the 'CassandraData' view
        }
    }

}