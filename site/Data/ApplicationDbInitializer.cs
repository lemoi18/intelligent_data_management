using System;
using System.Linq;
using Site.Models;

namespace Site.Data
{
    public class ApplicationDbInitializer
    {
        public static void Initialize(ApplicationDbContext db)
        {
            
            db.Database.EnsureCreated();

            

            if (!db.Stubs.Any())
            {
                var Postgress = new[]
                {
                    new Stub(1, "Postgress 1", "Postgress 1", new DateTime(1981, 1, 1)),
                    new Stub(2, "Postgress 2", "Postgress 2", new DateTime(1981, 1, 1)),
                    new Stub(3, "Postgress 3", "Postgress 3", new DateTime(1981, 1, 1)),
                };

                db.AddRange(Postgress); // Add the new records
        
                db.SaveChanges(); // Finally save the added records
            }
        }
    }
}