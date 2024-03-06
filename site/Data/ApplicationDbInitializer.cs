using System;
using Site.Models;

namespace Site.Data
{
    public class ApplicationDbInitializer
    {
        public static void Initialize(ApplicationDbContext db)
        {
            

            

            var Postgress = new[]
            {
                new Stub(1,"Postgress 1", "Postgress 1", new DateTime(1981, 1, 1)),
                new Stub(2,"Postgress 2", "Postgress 2", new DateTime(1981, 1, 1)),
                new Stub(3,"Postgress 3", "Postgress 3", new DateTime(1981, 1, 1)),
            };
            
            db.AddRange(Postgress);
            
            // Finally save the added relationships
            db.SaveChanges();
        }
    }
}