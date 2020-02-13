using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LibraryCorp
{
    public class CosmosDocumentConverter : JsonConverter
    {

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            var document = Activator.CreateInstance(objectType, reader, CosmosClientFactory.JsonSettings);
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
