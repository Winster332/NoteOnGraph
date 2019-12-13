using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace NoteOnGraph.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args);
            host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
            host.ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>(); 
            });

            return host;
        }
    }
}