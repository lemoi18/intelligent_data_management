using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Site.Data;

namespace Site
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var services = host.Services.CreateScope())
            {
                var postgress = services.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                ApplicationDbInitializer.Initialize(postgress);
                var Cassandra = services.ServiceProvider.GetRequiredService<CassandraService>();
                CassandraDbInitializer.Initialize(Cassandra.GetSession());
                var mongoClient = services.ServiceProvider.GetRequiredService<IMongoClient>();
                MongoDBInitializer.Initialize(mongoClient);
                
            }
  
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
