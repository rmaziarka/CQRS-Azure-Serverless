using System;
using System.Collections.Generic;
using System.Text;
using LibraryCorp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace LibraryCorp
{
    public class CosmosDocumentConverter : JsonConverter
    {
        private JsonSerializer _converterSerialier;

        public CosmosDocumentConverter()
        {
            _converterSerialier = new JsonSerializer()
            {
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                ContractResolver = new PrivateResolver()
            };
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            var obj = JObject.Load(reader);

            var document = obj.ToObject(objectType, _converterSerialier);
            
            return document;
        }

        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            return
                objectType.IsGenericType
                && objectType.GetGenericTypeDefinition() == typeof(CosmosDocument<>);
        }
    }
}
