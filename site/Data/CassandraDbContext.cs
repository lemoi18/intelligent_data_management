using System;
using System.Threading.Tasks;
using Cassandra;
using Microsoft.Extensions.Configuration;

namespace Site.Data
{
    public class CassandraService
    {
        private Cluster _cluster;
        private ISession _session;

        public CassandraService(IConfiguration configuration)
        {
            var contactPoints = configuration["CassandraConnection:ContactPoints"]; // e.g., "cassandra"
            var port = Convert.ToInt32(configuration["CassandraConnection:Port"]); // e.g., 9042
            var keyspace = configuration["CassandraConnection:Keyspace"]; // e.g., "IKT453"

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

        // ... other Cassandra-related methods ...
    }

}