﻿using FewBox.Service.Payment.Model.Configs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FewBox.Core.Web.Extension;
using NSwag.Generation.AspNetCore;
using NSwag.Generation.Processors.Security;
using Microsoft.Extensions.Hosting;
using NSwag;
using Microsoft.AspNetCore.Mvc;
using FewBox.Service.Payment.Domain.Services;

namespace FewBox.Service.Payment
{
    public class Startup
    {

        public Startup(IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            this.HostingEnvironment = hostingEnvironment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment HostingEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddFewBox(FewBoxDBType.None, new ApiVersion(1, 0, "alpha1"));
            var paypalConfig = this.Configuration.GetSection("PaypalConfig").Get<PaypalConfig>();
            services.AddSingleton(paypalConfig);
            // Biz
            services.AddScoped<PaypalServiceBuilder>();
            services.AddOpenApiDocument(config =>
            {
                this.InitAspNetCoreOpenApiDocumentGeneratorSettings(config, "v1", new[] { "1-alpha1", "1-beta1", "1" }, "v1");
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseOpenApi();
            app.UseStaticFiles();
            if (env.IsDevelopment())
            {
                app.UseCors("dev");
                app.UseSwaggerUi3();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseCors();
            }
            if (env.IsStaging())
            {
                app.UseSwaggerUi3();
                app.UseDeveloperExceptionPage();
            }
            if (env.IsProduction())
            {
                app.UseReDoc(c => c.DocumentPath = "/swagger/v1/swagger.json");
                app.UseHsts();
            }
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void InitAspNetCoreOpenApiDocumentGeneratorSettings(AspNetCoreOpenApiDocumentGeneratorSettings config, string documentName, string[] apiGroupNames, string documentVersion)
        {
            config.DocumentName = documentName;
            config.ApiGroupNames = apiGroupNames;
            config.PostProcess = document =>
            {
                this.InitDocumentInfo(document, documentVersion);
            };
            config.OperationProcessors.Add(new OperationSecurityScopeProcessor("JWT"));
            config.DocumentProcessors.Add(
                new SecurityDefinitionAppender("JWT", new OpenApiSecurityScheme
                {
                    Type = OpenApiSecuritySchemeType.ApiKey,
                    Name = "Authorization",
                    Description = "Bearer [Token]",
                    In = OpenApiSecurityApiKeyLocation.Header
                })
            );
        }

        private void InitDocumentInfo(OpenApiDocument document, string version)
        {
            document.Info.Version = version;
            document.Info.Title = "FewBox Payment Api";
            document.Info.Description = "FewBox Payment, for more information please visit the 'https://fewbox.com'";
            document.Info.TermsOfService = "https://fewbox.com/terms";
            document.Info.Contact = new OpenApiContact
            {
                Name = "FewBox",
                Email = "support@fewbox.com",
                Url = "https://fewbox.com/support"
            };
            document.Info.License = new OpenApiLicense
            {
                Name = "Use under license",
                Url = "https://fewbox.com/license"

            };
        }
    }
}
