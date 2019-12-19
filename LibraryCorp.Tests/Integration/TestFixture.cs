using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text;
using Microsoft.Extensions.Configuration;
namespace LibraryCorp.Tests.Integration
{
    public class TestFixture : IDisposable
    {
        private readonly Process _funcHostProcess;

        public readonly HttpClient Client = new HttpClient();

        public TestFixture()
        {

            var config = new ConfigurationBuilder()
                .AddJsonFile("settings.json")
                .Build();

            

            var dotnetExePath = Environment.ExpandEnvironmentVariables(config["DotnetExecutablePath"]);
            var functionHostPath = Environment.ExpandEnvironmentVariables(config["FunctionHostPath"]);
            var functionAppFolder = Environment.ExpandEnvironmentVariables(config["FunctionApplicationPath"]);

            _funcHostProcess = new Process
            {
                StartInfo =
                {
                    FileName = dotnetExePath,
                    Arguments = $"\"{functionHostPath}\" start -p {Port} --csharp",
                    WorkingDirectory = functionAppFolder,
                    RedirectStandardError = true,
                }
            };
            var success = _funcHostProcess.Start();
            if (!success)
            {
                throw new InvalidOperationException("Could not start Azure Functions host.");
            }

            Client.BaseAddress = new Uri($"http://localhost:{Port}");
        }

        public int Port { get; } = 7071;

        public virtual void Dispose()
        {
            if (!_funcHostProcess.HasExited)
            {
                _funcHostProcess.Kill();
            }

            _funcHostProcess.Dispose();
        }
    }
}
