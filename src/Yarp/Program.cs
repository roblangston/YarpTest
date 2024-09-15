using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore;
using Serilog;

namespace Yarp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var log = new LoggerConfiguration()
            .WriteTo.File("logs/log.txt",
            rollingInterval: RollingInterval.Day,
            rollOnFileSizeLimit: true)
            .CreateLogger();

            try
            {

                var builder = WebApplication.CreateBuilder(args);

                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();

                var a = builder.Configuration.GetSection("ReverseProxy");

                builder.Services.AddReverseProxy()
                    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

                var app = builder.Build();

                //if (app.Environment.IsDevelopment())
                //{
                //   app.UseSwagger();
                //  app.UseSwaggerUI();
                //}

                //app.UseHttpsRedirection();
                //app.UseStaticFiles();
                //app.UseHttpLogging();
                app.UseRouting();
                app.MapReverseProxy();

                app.Run();
            }
            catch(Exception ex)
            {
                log.Fatal(ex, "Application terminated");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
