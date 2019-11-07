

namespace Sample.Tris.WebApi
{
    using System;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.OpenApi.Models;
    using Sample.Tris.WebApi.Configuration;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddCors()
                .AddTrisApiConfiguration(Configuration)
                .AddRazorPages();

            services
                .AddControllers()
                .ConfigureApiBehaviorOptions(setupAction =>
                {
                    setupAction.InvalidModelStateResponseFactory = context =>
                    {
                        return new BadRequestObjectResult(context.ModelState);
                    };
                });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {
                    Title = "Tris Query API",
                    Version = "v1",
                    Description = "Triangle Grid Query API",
                    Contact = new OpenApiContact() {
                        Name = "Rob Dick",
                        Email = "robdick@gmail.com",
                        Url = new Uri("https://www.linkedin.com/in/robdick/")
                    }
                });
            });

            var mapperInstance = AutomapperConfiguration.CreateMapper();
            services.AddSingleton(mapperInstance);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app
                .UseSwagger()
                .UseSwaggerUI(setupAction => {
                    setupAction.SwaggerEndpoint("/swagger/v1/swagger.json", "Triangle Grid Query API");
                });

            app
                .UseStaticFiles()
                .UseRouting()
                .UseAuthorization()
                .UseCors(configurePolicy =>
                {
                    configurePolicy.AllowAnyOrigin();
                    configurePolicy.AllowAnyHeader();
                    configurePolicy.AllowAnyMethod();
                })
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                    endpoints.MapRazorPages();
                });
        }
    }
}
