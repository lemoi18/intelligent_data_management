using System;
using Cassandra;
using Site.Models;

namespace Site.Data
{
    public class CassandraDbInitializer
    {
        public static void Initialize(ISession session)
        {
            // Create keyspace and tables if they do not exist
            session.Execute("CREATE KEYSPACE IF NOT EXISTS my_keyspace WITH replication = {'class': 'SimpleStrategy', 'replication_factor': 1 };");
            session.Execute(@"
                CREATE TABLE IF NOT EXISTS my_keyspace.stubs (
                    id int PRIMARY KEY,
                    first_name text,
                    last_name text,
                    birthdate timestamp
                );");

            // Seed data
            var preparedInsert = session.Prepare("INSERT INTO my_keyspace.stubs (id, first_name, last_name, birthdate) VALUES (?, ?, ?, ?)");
            var preparedSelect = session.Prepare("SELECT id FROM my_keyspace.stubs WHERE id = ?");

            var stubs = new[]
            {
                new Stub(1, "Cassandra 1", "Cassandra 1", new DateTime(1981, 1, 1)),
                new Stub(2, "Cassandra 2", "Cassandra 2", new DateTime(1982, 2, 2)),
                new Stub(3, "Cassandra 3", "Cassandra 3", new DateTime(1983, 3, 3)),
            };

            foreach (var stub in stubs)
            {
                var boundSelect = preparedSelect.Bind(stub.Id);
                var resultSet = session.Execute(boundSelect);

                // Only insert if the ID does not exist
                if (!resultSet.IsExhausted())
                {
                    var boundInsert = preparedInsert.Bind(stub.Id, stub.FirstName, stub.LastName, stub.Birthdate);
                    session.Execute(boundInsert);
                }
            }
        }
    }
}