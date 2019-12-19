using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Azure.Cosmos;

namespace LibraryCorp
{
    public static class CosmosClientFactory
    {
        public static readonly CosmosClient Client;

        static CosmosClientFactory()
        {
            Client = new CosmosClient(Environment.GetEnvironmentVariable("AzureCosmosDBConnection", EnvironmentVariableTarget.Process));
        }

        public static Container GetLibrariesContainer()
        {
            return Client.GetDatabase("LibraryCorp").GetContainer("libraries");
        }
    }
}
