using Microsoft.AspNetCore.SignalR;
using RabbitMQ.Client;
using RabbitMQTopicConsumer.Hubs;
using RabbitMQTopicConsumer.Services;

namespace RabbitMQTopicConsumer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();


            var factory = new ConnectionFactory() { Uri=new Uri("amqps://tnwekfim:BtEntszuzgThFjJTyaJF9caqQs_pLe5k@goose.rmq2.cloudamqp.com/tnwekfim") };
            var connection = factory.CreateConnection();


            builder.Services.AddSignalR();
            builder.Services.AddSingleton(provider => new TopicConsumerService(connection, provider.GetRequiredService<IHubContext<MessageHub>>()));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            //app.MapControllerRoute(
            //    name: "default",
            //    pattern: "{controller=Home}/{action=Index}/{id?}");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapHub<MessageHub>("/messageHub");
            });

            app.Run();
        }
    }
}