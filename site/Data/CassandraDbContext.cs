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
            var connectionString = configuration.GetConnectionString("CassandraConnection");
            // Parse the connection string and create a Cluster instance
            _cluster = Cluster.Builder()
                .AddContactPoints("cassandra")
                .WithPort(9042)
                .WithDefaultKeyspace("IKT453")
                .Build();
            // Connect to the cluster
            _session = _cluster.ConnectAndCreateDefaultKeyspaceIfNotExists();
        }

        public ISession GetSession()
        {
            return _session;
        }

        // ... other Cassandra-related methods ...
    }

}