using System.CommandLine;
using System.CommandLine.Invocation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.CommandLine.Builder;
using System.CommandLine.Hosting;
using System.CommandLine.Parsing;

namespace ConsoleApp
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var rootCommand = new RootCommand(
                description: "DM CLI console tool for variative use of DM API and DM Azure services."
            )
            {
                new ConfigCommand()
            };

            await new CommandLineBuilder(rootCommand)
                .UseDefaults()
                .UseHost(CreateHostBuilder)
                .Build()
                .InvokeAsync(args);
        }

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(
                    (hostingContext, config) =>
                    {
                        config.SetBasePath(Directory.GetCurrentDirectory());
                        config.AddJsonFile("appsettings.json", optional: true);
                        config.AddEnvironmentVariables();
                    }
                )
                .ConfigureServices(
                    (hostContext, services) =>
                    {
                        services.AddSingleton<IConfiguration>(hostContext.Configuration);
                        services.AddSingleton<IConfigurationService, ConfigurationService>();
                    }
                );
    }
}
