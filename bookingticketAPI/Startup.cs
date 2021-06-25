using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using bookingticketAPI.Controllers;
using bookingticketAPI.Hubs;
using bookingticketAPI.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;

namespace bookingticketAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        
        }

        public IConfiguration Configuration { get; }
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //dependency injection
            services.AddTransient<dbRapChieuPhimContext, dbRapChieuPhimContext>();

         



            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            //swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
            });
            services.ConfigureSwaggerGen(options =>
            {
               options.OperationFilter<swagger.AuthorizationHeaderParameterOperationFilter>();
            });
            //cau hinh jwt
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "JwtCybersoft"; //Tên authorize => phân biệt token 
                options.DefaultChallengeScheme = "JwtCybersoft";
            }).AddJwtBearer("JwtCybersoft", jwtOptions => {
                jwtOptions.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    IssuerSigningKey = QuanLyNguoiDungController.SIGNING_KEY, //Mật khẩu mã hóa
                    ValidateIssuer = false, //Đơn vị phát hành không có 
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromDays(365)

                };
            });

            //signalR
            services.AddSignalR();
            //Cho phép sử dụng cross
            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                builder =>
                {
                    builder.WithOrigins("http://crm.myclass.vn/", "http://127.0.0.1:5500", "https://localhost:5001/swagger/index.html", "http://localhost:3000", "http://localhost:3001", "http://localhost:3002", "http://localhost:3003", "http://localhost:4200", "*").AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().AllowCredentials().Build();
                });
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {


            //Cho phép xem hình
            app.UseStaticFiles();



            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            //swagger

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");

            });

            //authen token
            app.UseAuthentication();

            //allow origin
            //allow origin
            app.UseCors(MyAllowSpecificOrigins);

            //signalr
            app.UseSignalR(route =>
            {
                route.MapHub<DatVeHub>("/datvehub");
            });

            app.UseHttpsRedirection();
            app.UseMvc();
            //khai bao su dung  quyen folder hinh
            app.UseDirectoryBrowser(new DirectoryBrowserOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot", "hinhanh")),
                RequestPath = new PathString("/hinhanh")
            });
        
        }
    }
}
