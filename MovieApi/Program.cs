using Microsoft.EntityFrameworkCore;
using MovieApi.Data;
using MovieApi.Extensions;

namespace MovieApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<MovieApiContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("MovieApiContext") ?? throw new InvalidOperationException("Connection string 'MovieApiContext' not found.")));

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

            builder.Services.AddSwaggerGen(opt =>
            {
                opt.EnableAnnotations();
            });

            builder.Services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<MapperProfile>();
            });

            //builder.Services.AddOpenApi();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                //app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI(opt =>
                {
                    opt.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                });
                await app.InitAsync();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            //app.UseDemoMiddleware();

            app.MapControllers();

            app.Run();
        }
    }
}
