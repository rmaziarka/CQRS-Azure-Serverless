using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryCorp
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
