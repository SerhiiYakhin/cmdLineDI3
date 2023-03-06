// public class Startup
// {
//     public void ConfigureServices(IServiceCollection services)
//     {
//         var configurationBuilder = new ConfigurationBuilder()
//             .SetBasePath(Directory.GetCurrentDirectory())
//             .AddJsonFile("appsettings.json", optional: true);

//         var configuration = configurationBuilder.Build();
//         services.AddSingleton(configuration);

//         services.AddSingleton<IConfigurationService, ConfigurationService>();
//     }
// }
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;

//namespace ConsoleApp
//{
//    public class Startup
//    {
//        public static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
//        {
//            services.AddSingleton<IConfiguration>(context.Configuration);
//            services.AddSingleton<IConfigurationService, ConfigurationService>();
//        }
//    }
//}
