using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using NServiceBus;

namespace ClientUI
{   
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.Title = "ClientUI";
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                        .UseNServiceBus(context =>
                        {
                            var endpointConfiguration = new EndpointConfiguration("ClientUI");
                            endpointConfiguration.UseSerialization<SystemJsonSerializer>();
                            endpointConfiguration.EnableInstallers();
                            var connectionString = "INSERT CONNECTION STRING";
                            var transport = new AzureServiceBusTransport(connectionString);
                            endpointConfiguration.UseTransport(transport);

                            endpointConfiguration.SendFailedMessagesTo("error");
                            endpointConfiguration.AuditProcessedMessagesTo("audit");

                            return endpointConfiguration;

                        })
                       .ConfigureWebHostDefaults(webBuilder =>
                        {
                            webBuilder.UseStartup<Startup>();
                        });
        }
    }
}
