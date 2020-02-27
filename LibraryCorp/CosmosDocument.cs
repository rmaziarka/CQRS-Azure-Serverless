using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using LibraryCorp.Models;
using Microsoft.Azure.Documents;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace LibraryCorp
{
    public sealed class CosmosDocument<T> :ICosmosDocument where T: Aggregate
    {
        public CosmosDocument(string partitionKey, T data)
        {
            this.PartitionKey = partitionKey;
            this.Data = data;
        }

        private CosmosDocument(){ }

        [JsonProperty(PropertyName = "id")]
        public string Id => this.Data.Id;

        [JsonProperty(PropertyName = "partitionKey")]
        public string PartitionKey { get; private set; }


        [JsonProperty(PropertyName = "documentType")]
        public string DocumentType => typeof(T).Name;

        [JsonProperty(PropertyName = "data")]
        public T Data { get; private set; }

        [JsonProperty(PropertyName = "_etag")]
        public string ETag { get; private set; }
    }

    public interface ICosmosDocument
    {
        string Id { get; }
    }
}
