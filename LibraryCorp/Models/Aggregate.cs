using System;

namespace LibraryCorp.Models
{
    public class Aggregate
    {
        public Aggregate(string id = null)
        {
            Id = id ?? Guid.NewGuid().ToString();
        }

        public string Id { get; private set; }
    }
}
