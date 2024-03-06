using System;
using Newtonsoft.Json;

namespace Site.Models
{
    public class SampleData
    {
        public Guid Id { get; set; }
        public DateTime Created { get; set; }

        public SampleData()
        {
            Id = Guid.NewGuid();
            Created = DateTime.UtcNow;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        }
    }
}