using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Linq;
using System.Threading.Tasks;
using ASPNETCORE.Core.Interfaces;
using System;
using System.Data.Common;

public class MapMethodsMiddleCore
{
    private readonly IMapMethods _MapMethods;
    private RequestDelegate _next;

    public IMapMethods MapMethods { get; }

    public MapMethodsMiddleCore(RequestDelegate next, IMapMethods MM)
    {
        _next = next;
        _MapMethods = MM;
    }
    
    public async Task InvokeAsync(HttpContext context)
    {
        var routevalues = context.GetRouteData().Values;

        if (routevalues.Count > 0)
        {
            RequestDelegate _tempNext = _next;
            var classname = routevalues["classname"].ToString();
            var methodname = routevalues["methodname"].ToString();
            var id = int.Parse(routevalues["id"] == null ? "0" : routevalues["id"].ToString());

            var MethodsInfo = _MapMethods.LoadMethodsfromClass(classname);

            object result = null;
            try {
                var MI = _MapMethods.SelectMethod(methodname);

                if (MI.GetParameters().Length == 0)
                    result = _MapMethods.ExecuteMap(classname, methodname, null);
                else
                    result = _MapMethods.ExecuteMap(classname, methodname, new object[] { id });

                await context.Response.WriteAsync(result == null ? "" : result.ToString());
            }
            catch (Exception e)
            {
                _tempNext = async ctx => await context.Response.WriteAsync($"Failure! Method name {methodname} in class name {classname}; Error:" + e.Message);
            }
            await _tempNext(context);
        }
    }
}
