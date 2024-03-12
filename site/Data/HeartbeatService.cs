using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

// ... other using directives



namespace Site.Data
{
  

    public class HeartbeatService : IHostedService, IDisposable
    {
        private Timer _timer;
        private readonly ILogger<HeartbeatService> _logger;
        // Inject any required services to check their status
        private readonly IServiceProvider _serviceProvider;

        public HeartbeatService(ILogger<HeartbeatService> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Heartbeat Service running.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero, 
                TimeSpan.FromMinutes(5)); // Adjust the interval as needed

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            // This will run the asynchronous method in a non-blocking way
            
                CheckDatabaseAvailability(_serviceProvider);
                CheckMongoDbAvailability(_serviceProvider);
                CheckCassandraAvailability(_serviceProvider); 
                // ... other checks
            
        }



        // Implement your individual check methods
        private void CheckDatabaseAvailability(IServiceProvider services)
        {
            using (var scope = services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                try
                {
                    // You can adjust the logic here to be more specific as needed
                    dbContext.Database.CanConnect();
                    _logger.LogInformation("Postgress is available.");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Postgress check failed: {ex.Message}");
                    // Handle the exception based on your application needs
                }
            }
        }

        private void CheckCassandraAvailability(IServiceProvider services)
        {
            using (var scope = services.CreateScope())
            {
                var cassandraService = scope.ServiceProvider.GetRequiredService<CassandraService>();

                // Call the async method synchronously
                var result = cassandraService.CheckConnectionAsync().GetAwaiter().GetResult();
        
                if (result.IsSuccessful)
                {
                    _logger.LogInformation("Cassandra is available.");
                }
                else
                {
                    // Log the reason why the connection check failed
                    _logger.LogWarning($"Cassandra is not available: {result.ErrorMessage}");
                }
            }
        }




        private void CheckMongoDbAvailability(IServiceProvider services)
        {
            using (var scope = services.CreateScope())
            {
                var mongoClient = scope.ServiceProvider.GetRequiredService<IMongoClient>();
                try
                {
                    var database = mongoClient.GetDatabase("MyAppDatabase");
                    // Perform a minimal operation like listing collections
                    database.ListCollectionNames();
                    _logger.LogInformation("MongoDB is available." );
                }
                catch (Exception ex)
                {
                    _logger.LogError($"MongoDB check failed: {ex.Message}");
                    // Handle the exception as required
                }
            }
        }


        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Heartbeat Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }

}