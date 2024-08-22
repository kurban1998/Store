using Database;
using Database.Models;
using Database.Repositories;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TshirtManagementService.Builder;
using Host = Microsoft.Extensions.Hosting.Host;

namespace TshirtManagementService
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            IHostBuilder builder = Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    services.AddMassTransit(x =>
                    {
                        x.UsingRabbitMq((context, cfg) =>
                        {
                            cfg.Host("rabbitmq://localhost");
                            cfg.ConfigureEndpoints(context);
                        });

                        x.AddConsumer<TshirtAddedConsumer>();
                        x.AddConsumer<TshirtDeletedConsumer>();
                        x.AddConsumer<TshirtGetAllConsumer>();
                        x.AddConsumer<GetTshirtConsumer>();
                        x.AddConsumer<TshirtGetAllConsumer>();
                        x.AddConsumer<ReviewAddedConsumer>();
                        x.AddConsumer<TshirtsDeletedConsumer>();
                    });

                    services.AddDbContext<StoreContext>(x => x.UseSqlServer("Server=DESKTOP-OMU44NR\\SQLEXPRESS;Database=TshirtStore;Trusted_Connection=True;"));
                    services.AddTransient<ITshirtRepository, TshirtRepository>();
                    services.AddTransient<ITshirtBuilder, TshirtBuilder>();
                });

            var app = builder.Build();

            var busControl = app.Services.GetRequiredService<IBusControl>();
            await busControl.StartAsync(new CancellationToken());


            try
            {
                Console.WriteLine("Press enter to exit");
                await Task.Run(() => Console.ReadLine());
            }
            finally
            {
                await busControl.StopAsync();
            }
        }
    }
}
