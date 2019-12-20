using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using Microsoft.Extensions.Configuration;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace LibraryCorp.Tests.Integration
{
    public class TestFixture : IDisposable
    {
        private readonly Process _funcHostProcess;

        public readonly HttpClient Client = new HttpClient();

        public TestFixture(IMessageSink sink)
        {

            var config = new ConfigurationBuilder()
                .AddJsonFile("settings.json")
                .AddEnvironmentVariables()
                .Build();

            _funcHostProcess = StartFunction(config, sink);

            Client.BaseAddress = new Uri($"http://localhost:{Port}");

        }

        public Process StartFunction(IConfigurationRoot config, IMessageSink sink)
        {
            var dotnetExePath = Environment.ExpandEnvironmentVariables(config["DotnetExecutablePath"]);
            var functionHostPath = Environment.ExpandEnvironmentVariables(config["FunctionHostPath"]);
            var functionAppFolder = Environment.ExpandEnvironmentVariables(config["FunctionApplicationPath"]);

            var configMessage = new DiagnosticMessage("DotnetExePath: {0}, FunctionHostPath: {1}, FunctionApplicationPath: {2}", 
                dotnetExePath, functionHostPath, functionAppFolder);

            sink.OnMessage(configMessage);

            var funcHostProcess = new Process
            {
                StartInfo =
                {
                    FileName = dotnetExePath,
                    Arguments = $"\"{functionHostPath}\" start -p {Port} --csharp",
                    WorkingDirectory = functionAppFolder,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                }
            };
            
            var success = funcHostProcess.Start();
            if (!success)
            {
                throw new InvalidOperationException("Could not start Azure Functions host.");
            }

            ConfigureFunctionLogging(sink, funcHostProcess);

            if (funcHostProcess.HasExited)
            {
                var error = _funcHostProcess.StandardError.ReadToEnd();
                throw new InvalidOperationException(error);
            }

            // ugly hack to let azure function start before test begun
            Thread.Sleep(200);

            return funcHostProcess;
        }

        private static void ConfigureFunctionLogging(IMessageSink sink, Process funcHostProcess)
        {
            void LogToOutput(object sender, DataReceivedEventArgs args)
            {
                var message = new DiagnosticMessage(args.Data);
                sink.OnMessage(message);
            }

            funcHostProcess.ErrorDataReceived += LogToOutput;
            funcHostProcess.OutputDataReceived += LogToOutput;

            funcHostProcess.BeginOutputReadLine();
            funcHostProcess.BeginErrorReadLine();
        }

        public int Port => 7071;

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
