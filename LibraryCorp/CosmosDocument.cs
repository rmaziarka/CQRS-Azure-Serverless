using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Azure.Documents;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace LibraryCorp
{
    public class CosmosDocument<T>: Document where T: Aggregate
    {
        public CosmosDocument(string partitionKey, T data)
        {
            this.PartitionKey = partitionKey;
            this.Data = data;
        }
        
        [JsonProperty(PropertyName = "id")]
        public string Id => this.Data.Id;

        public string PartitionKey { get; private set; }


        [JsonConverter(typeof(StringEnumConverter))]
        public string DocumentType => typeof(T).Name; 

        public T Data { get; }

        [JsonProperty(PropertyName = "_etag")]
        public string ETag { get; }
    }
}
