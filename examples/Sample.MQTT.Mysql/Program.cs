using DotNetCore.CAP.Messages;
using jinzaz.CAP.MQTT;

namespace Sample.MQTT.Mysql
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddDbContext<AppDbContext>(); //Options, If you are using EF as the ORM
            // Add services to the container.
            builder.Services.AddCap(x => {
                x.UseEntityFramework<AppDbContext>();
                x.UseMQTT(options => {
                    options.Server = "localhost";
                    options.Port = 6000;
                    options.UserName = "admin";
                    options.Password = "password";
                    options.ClientId = "z5fas51fs";
                });
                x.UseDashboard();
                x.FailedRetryCount = 5;
                x.FailedThresholdCallback = failed =>
                {
                    var logger = failed.ServiceProvider.GetService<ILogger<Program>>();
                    logger.LogError($@"A message of type {failed.MessageType} failed after executing {x.FailedRetryCount} several times, 
                        requiring manual troubleshooting. Message name: {failed.Message.GetName()}");
                };
            });
            builder.Services.AddControllers();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}