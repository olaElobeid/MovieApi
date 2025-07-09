using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace MovieApi.MiddleWare
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class DemoMiddleware
    {
        private readonly RequestDelegate _next;

        public DemoMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            Console.WriteLine("in request");
            await _next(httpContext);
            Console.WriteLine("in response");
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class DemoMiddlewareExtensions
    {
        public static IApplicationBuilder UseDemoMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<DemoMiddleware>();
        }
    }
}
