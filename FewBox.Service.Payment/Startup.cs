using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FewBox.Core.Web.Filter;
using FewBox.Service.Payment.Model.Configs;
using FewBox.Service.Payment.Model.Service;
using FewBox.Service.Payment.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FewBox.Service.Payment
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options=>{
                options.Filters.Add<ExceptionAsyncFilter>();
                //options.Filters.Add<TransactionAsyncFilter>();
                options.Filters.Add<TraceAsyncFilter>();
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
            .ConfigureApplicationPartManager(apm =>
            {
                var dependentLibrary = apm.ApplicationParts
                    .FirstOrDefault(part => part.Name == "FewBox.Core.Web");
                if (dependentLibrary != null)
                {
                    apm.ApplicationParts.Remove(dependentLibrary);
                }
            }); // Note: Remove AuthenticationController.
            services.Configure<RouteOptions>(options=>{
                options.LowercaseUrls=true;
            });
            var paypalConfig = this.Configuration.GetSection("PaypalConfig").Get<PaypalConfig>();
            services.AddSingleton(paypalConfig);
            services.AddScoped<IPaymentLogService, PaymentLogService>();
            services.AddScoped<IExceptionHandler, ConsoleExceptionHandler>(); // Todo: Change to remote log service.
            services.AddScoped<ITraceHandler, ConsoleTraceHandler>(); // Todo: Change to remote log service.
            services.AddCors(
                options =>
                {
                    options.AddDefaultPolicy(
                        builder =>
                        {
                            builder.SetIsOriginAllowedToAllowWildcardSubdomains().WithOrigins("https://fewbox.com").AllowAnyMethod().AllowAnyHeader();
                        });
                    options.AddPolicy("all",
                        builder =>
                        {
                            builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                        });

                });
            services.AddOpenApiDocument(config => {
                config.PostProcess = document =>
                {
                    document.Info.Version = "v1";
                    document.Info.Title = "FewBox Payment API";
                    document.Info.Description = "A simple ASP.NET Core web API";
                    document.Info.TermsOfService = "https://fewbox.com/terms";
                    document.Info.Contact = new NSwag.SwaggerContact
                    {
                        Name = "XL Pang",
                        Email = "support@fewbox.com",
                        Url = "https://fewbox.com/support"
                    };
                    document.Info.License = new NSwag.SwaggerLicense
                    {
                        Name = "Use under license",
                        Url = "https://raw.githubusercontent.com/FewBox/FewBox.Service.Mail/master/LICENSE"
                    };
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            
            app.UseStaticFiles();
            app.UseSwagger();
            if (env.IsDevelopment() || env.IsStaging())  
            {
                app.UseCors("all");
                app.UseSwaggerUi3();  
            }
            else
            {
                app.UseCors("all");
                app.UseReDoc();
            }
            app.UseCors();
            app.UseMvc();
        }
    }
}
