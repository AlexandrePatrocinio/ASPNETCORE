using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using ASPNETCORE.Core.Interfaces;
using ASPNETCORE.Core.Extensions;

namespace ASPNETCORE.Configuration
{
    public class StartupDevelopment
    {
        public static IConfigurationRoot Configuration { get; set; }

        public StartupDevelopment(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile($"Configuration\\appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRouting();
            var NameSpace = Configuration.GetSection("MapMethodsMiddleCore")["NameSpace"];
            var AssemblyPath = Configuration.GetSection("MapMethodsMiddleCore")["AssemblyPath"];
            services.AddScoped<IMapMethods, MapMethods>(sp => 
                new MapMethods(string.IsNullOrEmpty(NameSpace) ? "" : NameSpace, string.IsNullOrEmpty(AssemblyPath) ? "" : AssemblyPath)
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app) => app.UseDeveloperExceptionPage().UseMapMethods().Run(DefaultHandlerRouteAsync);

        public async Task DefaultHandlerRouteAsync(HttpContext context)
        {
            context.Response.Headers["Accept-Charset"] = "UTF-8";
            var HTML = new System.Text.StringBuilder("<html>");
            HTML.AppendLine("<!DOCTYPE html>");
            HTML.AppendLine("<head>");
            HTML.AppendLine("</head>");
            HTML.AppendLine("<body>");
            HTML.AppendLine($"<h3>Ambiente de {Configuration["Label"]} executando!</h3>");
            HTML.AppendLine("</body>");
            HTML.AppendLine("</html>");
            await context.Response.WriteAsync(HTML.ToString());
        }
    }
}
