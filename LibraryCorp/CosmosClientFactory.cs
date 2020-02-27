using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace LibraryCorp
{
    public static class CosmosClientFactory
    {
        public static readonly CosmosClient Client;
        public static readonly JsonSerializerSettings JsonSettings;

        static CosmosClientFactory()
        {
            var connectionString =
                Environment.GetEnvironmentVariable("AzureCosmosDBConnection", EnvironmentVariableTarget.Process);

            JsonSettings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented,
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                Converters = new List<JsonConverter>() { new StringEnumConverter()},
                ContractResolver = new PrivateResolver()
            };

            var serializer = new CustomXCosmosSerializer(JsonSettings);

            Client = new CosmosClientBuilder(connectionString)
                .WithCustomSerializer(serializer)
                .Build();

        }

        public static Container GetLibrariesContainer()
        {
            return Client.GetDatabase("LibraryCorp").GetContainer("libraries");
        }
    }
    public class PrivateResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var prop = base.CreateProperty(member, memberSerialization);
            if (!prop.Writable)
            {
                var property = member as PropertyInfo;
                var hasPrivateSetter = property?.GetSetMethod(true) != null;
                prop.Writable = hasPrivateSetter;
            }
            return prop;
        }
    }
}
