using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Microsoft.Azure.Documents;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace LibraryCorp
{
    public sealed class CosmosDocument<T>: Document where T: Aggregate
    {
        public CosmosDocument(string partitionKey, T data)
        {
            this.PartitionKey = partitionKey;
            this.Data = data;
        }

        public CosmosDocument(JsonReader reader, JsonSerializerSettings settings)
        {
            this.LoadFrom(reader, settings);

            this.PartitionKey = this.GetPropertyValue<string>("partitionKey");
            this.Data = this.GetPropertyValue<T>("data");
        }

        [JsonProperty(PropertyName = "id")]
        public string Id => this.Data.Id;

        [JsonProperty(PropertyName = "partitionKey")]
        public string PartitionKey { get; private set; }


        [JsonProperty(PropertyName = "documentType")]
        public string DocumentType => typeof(T).Name;

        [JsonProperty(PropertyName = "data")]
        public T Data { get; }

        [JsonProperty(PropertyName = "_etag")]
        public string ETag { get; }

        [OnSerializing]
        internal void OnSerializingMethod(StreamingContext context)
        {
            this.SetPropertyValue("data", this.Data);
        }

    }
}
