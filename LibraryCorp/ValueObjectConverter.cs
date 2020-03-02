using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LibraryCorp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace LibraryCorp
{
    public class TypeIdConverter : JsonConverter
    {

        public override bool CanConvert(Type objectType)
        {
            return objectType.BaseType == typeof(TypedIdValueBase);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var value = serializer.Deserialize(reader, typeof(Guid));
            return Activator.CreateInstance(objectType, value);
        }

        const string ValuePropertyName = nameof(TypedIdValueBase.Value);

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var contract = (JsonObjectContract)serializer.ContractResolver.ResolveContract(value.GetType());
            var valueProperty = contract.Properties.Single();
            serializer.Serialize(writer, valueProperty.ValueProvider.GetValue(value));
        }
    }
}
