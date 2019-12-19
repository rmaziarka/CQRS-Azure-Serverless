using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LibraryCorp
{
    public class Estate
    {
        public Estate(string companyId, string name, string location, decimal price, List<Floor> floors)
        {
            CompanyId = companyId;
            Name = name;
            Location = location;
            Price = price;
            Floors = floors;
        }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        public String CompanyId { get; set; }

        public string Name { get; set; }

        public string Location { get; set; }

        public decimal Price { get; set; }

        public List<Floor> Floors { get; set; }
    }
}
