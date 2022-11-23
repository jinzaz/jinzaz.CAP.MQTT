# jinzaz.CAP.MQTT
dotnetcore/CAP的MQTT扩展支持

```
// select a transport provider you are using, event log table will integrate into.

PM> Install-Package jinzaz.CAP.MQTT
```

### Configuration

#### .Net Core 
```cs
public void ConfigureServices(IServiceCollection services)
{
    //......

    services.AddDbContext<AppDbContext>(); //Options, If you are using EF as the ORM

    services.AddCap(x =>
    {
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
}

```

#### .Net 6、7
```cs
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
```
