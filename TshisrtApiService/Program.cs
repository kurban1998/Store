using Database;
using Database.Repositories;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using SharedModels;
using TshisrtApiService;

namespace ThisrtApiService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            string connectionString = @"Server=DESKTOP-OMU44NR\SQLEXPRESS;Database=TshirtStore;Trusted_Connection=True;";
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddTransient<ICustomerRepository, CustomerRepository>();
            builder.Services.AddTransient<ITshirtRepository, TshirtRepository>();
            builder.Services.AddDbContext<StoreContext>(option => option.UseSqlServer(connectionString));
            builder.Services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host("rabbitmq://localhost");
                });
            });

            var redisConnectionString = builder.Configuration.GetConnectionString("Redis");
            builder.Services.AddSingleton<IRedisService>(new RedisService(redisConnectionString));
            builder.Services.AddHostedService<TshirtCacheUpdaterService>();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
