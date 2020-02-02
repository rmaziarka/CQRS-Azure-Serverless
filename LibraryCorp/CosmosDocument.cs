using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace LibraryCorp
{
    public class CosmosDocument<T>: Docuemt
    {
      public CosmosDocument(T data)
        {
            this.Data = data;
        }

        public const char Separator = '~';


        [JsonProperty(PropertyName = "id")]
        public virtual string Id => this.Data.Id;

        [JsonConverter(typeof(StringEnumConverter))]
        public DocumentType DocumentType => typeof(T).Name; 

        public T Data { get; }

        [JsonProperty(PropertyName = "_etag")]
        public string ETag { get; }
    }
}
