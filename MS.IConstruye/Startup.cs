using Autofac;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MS.IConstruye.Domain;
using System.Diagnostics.CodeAnalysis;

namespace MS.IConstruye
{
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        public IConfiguration _configuration { get; }
        public Startup(IConfiguration configuration) => _configuration = configuration;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors()
                            .AddCustomMvc<IConstruyeBaseException>()
                             .AddCustomAuthentication(_configuration)
                              .AddSwaggerDoc(_configuration)
                                .AddApplicationInsightsTelemetry();
            services.AddMemoryCache();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new ApplicationModule(_configuration["ConnectionString"]));
            builder.RegisterModule(new MediatorModule());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            UseTelemetryFilter(app);

            app.UseAllCors();

            app.UseAuthentication();

            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseSwaggerDoc(_configuration);

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void UseTelemetryFilter(IApplicationBuilder app)
        {
            var configuration = app.ApplicationServices.GetService<TelemetryConfiguration>();
            configuration.TelemetryProcessorChainBuilder.Use(next => new TelemetryProcessor(next));
            configuration.TelemetryProcessorChainBuilder.Build();
        }
    }
}
