using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
//using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Serilog;
using RabbitMQ.Client;
using VoucherService.MQ;
using VoucherServiceBL.HangFire;
using VoucherServiceBL.Repository;
using VoucherServiceBL.Repository.Mongo;
using VoucherServiceBL.Repository.SqlServer;
using VoucherServiceBL.Service;

namespace VoucherService
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
            services.AddMvc();
            services.AddSingleton<IHostedService, Subscribers>();
            services.AddHangfire(config =>
                config.UseSqlServerStorage(Configuration.GetConnectionString("DefaultConnection")));

            //services.AddSingleton<IHostedService, HostRunner>();
            services.AddSingleton(new ConnectionFactory()
            {
                HostName = "192.168.99.100",
                Port = 5672,
                UserName = "guest",
                RequestedHeartbeat = 120,
                Password = "guest"

            });
            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));
            services.AddTransient<IGiftVoucherService,GiftVoucherService>();

            services.AddTransient<IDiscountVoucherService,DiscountVoucherService>();

            services.AddTransient<IDiscountVoucherService, DiscountVoucherService>();

            services.AddTransient<IVoucherService, BaseService>();
            services.AddTransient<IValueVoucherService, ValueVoucherService>();

            services.AddTransient<IVoucherRepository, MongoVoucherRepository>();
            services.AddTransient<IGiftRepository, MongoGiftRepository>();
            services.AddTransient<IDiscountRepository, MongoDiscountRepository>();
            services.AddTransient<IValueRepository, MongoValueRepository>();
            
            services.AddMongo(Configuration);


            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication(options =>
            {
                // Store the session to cookies
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                // OpenId authentication
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie("Cookies")
            .AddOpenIdConnect(options =>
            {
                // URL of the Keycloak server
                options.Authority = "http://localhost:8080/auth/realms/Voucherz";
                // Client configured in the Keycloak
                options.ClientId = "voucher";

                // For testing we disable https (should be true for production)
                options.RequireHttpsMetadata = false;
                options.SaveTokens = true;

                // Client secret shared with Keycloak
                options.ClientSecret = "040519ee-bd56-4ec6-8d98-cc3f900f736b";
                options.GetClaimsFromUserInfoEndpoint = true;

                // OpenID flow to use
                options.ResponseType = OpenIdConnectResponseType.CodeIdToken;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseCors("MyPolicy");
            // Add this line to ensure authentication is enabled
            app.UseAuthentication();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            //logging
            loggerFactory.AddSerilog();
            app.UseHttpsRedirection();
            app.UseMvc();
            
            app.UseHangfireServer();
            app.UseHangfireDashboard();
        }
    }
}
