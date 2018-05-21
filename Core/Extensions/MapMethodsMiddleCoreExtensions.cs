using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Http;

namespace ASPNETCORE.Core.Extensions
{
    public static class MapMethodsMiddleCoreExtensions
    {
        public static IApplicationBuilder UseMapMethods(this IApplicationBuilder builder)
        {
            var routeBuilder = new RouteBuilder(builder);
   
            routeBuilder.MapMiddlewareGet("core/{classname}/{methodname}/{id?}", app => app.UseMiddleware<MapMethodsMiddleCore>());            

            return builder.UseRouter(routeBuilder.Build());
        }
    }
}
