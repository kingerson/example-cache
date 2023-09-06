using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace MS.IConstruye
{
    public static class IApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseSwaggerDoc(this IApplicationBuilder app, IConfiguration configuration)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/V1/swagger.json", configuration["Microservice"]);
            });
            return app;
        }

        public static IApplicationBuilder UseAllCors(this IApplicationBuilder app)
        {
            app.UseCors(builder => builder
                       .AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader());
            return app;
        }
    }
}
