using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace LibraryCorp.Tests.Integration
{
    public class TestFixture : IAsyncLifetime
    {
        public readonly HttpClient Client = new HttpClient();
        private IHost _host;
        private int _port => 7071;


        public async Task InitializeAsync()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("settings.json")
                .AddEnvironmentVariables()
                .Build();

            _host = new HostBuilder()
                .ConfigureWebJobs(builder => builder
                    .UseWebJobsStartup<Startup>())
                .Build();

            await _host.StartAsync();

            Client.BaseAddress = new Uri($"http://localhost:{_port}");

        }

        public async Task DisposeAsync()
        {
            await _host.StopAsync();
        }
    }
}
