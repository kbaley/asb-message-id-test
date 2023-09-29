using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NServiceBus;

namespace Sales
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.Title = "Sales";
            await CreateHostBuilder(args).RunConsoleAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                       .UseNServiceBus(context =>
                       {
                           var endpointConfiguration = new EndpointConfiguration("Sales");

                           endpointConfiguration.UseSerialization<SystemJsonSerializer>();
                           endpointConfiguration.EnableInstallers();
                           var connectionString = "INSERT CONNECTION STRING";
                           var transport = new AzureServiceBusTransport(connectionString);
                           endpointConfiguration.UseTransport(transport);

                           endpointConfiguration.SendFailedMessagesTo("error");

                           // So that when we test recoverability, we don't have to wait so long
                           // for the failed message to be sent to the error queue
                           var recoverability = endpointConfiguration.Recoverability();
                           recoverability.Delayed(
                               delayed =>
                               {
                                   delayed.TimeIncrease(TimeSpan.FromSeconds(2));
                               }
                           );

                           return endpointConfiguration;
                       });
        }
    }
}
