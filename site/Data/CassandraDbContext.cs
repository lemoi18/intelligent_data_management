using System;
using System.Threading.Tasks;
using Cassandra;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Site.Data
{
    public class CassandraService
    {
        private Cluster _cluster;
        private ISession _session;
        private ILogger _logger;

        public CassandraService(IConfiguration configuration, ILogger<CassandraService> logger)
        {
            var contactPoints = configuration["CassandraConnection:ContactPoints"]; // e.g., "cassandra"
            var port = Convert.ToInt32(configuration["CassandraConnection:Port"]); // e.g., 9042
            var keyspace = configuration["CassandraConnection:Keyspace"]; // e.g., "IKT453"
            _logger = logger;
            // Parse the connection string and create a Cluster instance
            _cluster = Cluster.Builder()
                .AddContactPoints(contactPoints)
                .WithPort(port)
                .WithDefaultKeyspace(keyspace)
                .Build();
            _session = _cluster.ConnectAndCreateDefaultKeyspaceIfNotExists();
        }

        public ISession GetSession()
        {
            return _session;
        }
        public async Task<RowSet> ExecuteAsync(Statement statement)
        {
            return await _session.ExecuteAsync(statement).ConfigureAwait(false);
        }
        
        public async Task<ConnectionResult> CheckConnectionAsync()
        {
            try
            {
                var query = "SELECT now() FROM system.local;";
                var statement = new SimpleStatement(query);
                var resultSet = await ExecuteAsync(statement);

                // Logic to determine if the result indicates a successful connection
                bool isSuccessful = resultSet != null && !resultSet.IsFullyFetched;

                return new ConnectionResult
                {
                    IsSuccessful = isSuccessful,
                    ErrorMessage = isSuccessful ? null : "Connection check did not return expected results."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to check Cassandra connection");
                return new ConnectionResult
                {
                    IsSuccessful = false,
                    ErrorMessage = ex.Message
                };
            }
        }

        // ... other Cassandra-related methods ...
    }

    
    public class ConnectionResult
    {
        public bool IsSuccessful { get; set; }
        public string ErrorMessage { get; set; }
    }
    
}