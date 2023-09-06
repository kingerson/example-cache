using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MS.IConstruye
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomMvc<T1>(this IServiceCollection services)where T1 : class
        {
            services.AddControllers(
                    options =>
                    {
                        options.Filters.Add(typeof(RequestExceptionFilter<T1>));
                        options.Filters.Add(typeof(ValidateModelStateAttribute));
                    });

            services.AddAuthorization()
                    .AddOptions();

            return services;
        }

        public static IServiceCollection AddCustomAuthentication(this IServiceCollection services, IConfiguration configuration) =>
           services
           .AddTransient<HttpClientAuthorizationDelegatingHandler>()
           .AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
           .AddAuthentication("Bearer")
           .AddJwtBearer("Bearer", options =>
           {
               options.Authority = configuration["Idsrv:Authority"];
               options.Audience = configuration["Idsrv:ApiAudience"];
           }).Services;

        public static IServiceCollection AddSwaggerDoc(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("V1", new OpenApiInfo
                {
                    Title = configuration["Microservice"],
                    Version = "V1"
                });
                c.CustomSchemaIds((type) => SetCustomSchemaIdSelector(type));
            });
            return services;
        }

        private static Dictionary<string, string> GenerateScopes(IConfiguration configuration)
        {
            Dictionary<string, string> scopeConfiguration = new Dictionary<string, string>();

            var scopes = configuration["Idsrv:ApiResource"].Split(' ');

            foreach (var item in scopes)
            {
                scopeConfiguration.Add(item, string.Empty);
            }
            return scopeConfiguration;
        }

        private static string SetCustomSchemaIdSelector(Type modelType)
        {
            if (!modelType.IsConstructedGenericType)
                return modelType.Name;

            var prefix = modelType.GetGenericArguments()
                .Select(genericArg => SetCustomSchemaIdSelector(genericArg))
                .Aggregate((previous, current) => previous + current);

            return prefix + modelType.Name.Split('`').First();
        }
    }
}
