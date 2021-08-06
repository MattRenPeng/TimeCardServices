using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeCardServices.Model;
using TimeCardServices.Repository;
using TimeCardServices.Services;
using TimeCardServices.Utility;

namespace TimeCardServices
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
            services.AddControllers();
       
            
            //    services.AddEntityFrameworkSqlServer()
            //.AddDbContext<TimeCardDBContext>(options =>
            //{
            //    options.UseSqlServer(Configuration["data:ConnectionString"]);
            //});
            services.AddDbContext<TimeCardDBContext>(opt =>
            opt.UseSqlServer(Configuration.GetConnectionString("TimeCardDB")));

            services.AddTransient<IRepository<User>, Repository<User>>();
            services.AddTransient<IUserService, UserService>();



            #region JWT

            //services.AddAuthentication();
            var ValidAudience = SecurityUtity._audience;
            var ValidIssuer = SecurityUtity._issuer;
            var SecurityKey = SecurityUtity._securityKey;
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)                                      
                     .AddJwtBearer(options =>
                     {
                         options.TokenValidationParameters = new TokenValidationParameters
                         {
                             ValidateIssuer = true,
                             ValidateAudience = true,
                             ValidateLifetime = true,
                             ValidateIssuerSigningKey = true,
                             ValidAudience = ValidAudience,
                             ValidIssuer = ValidIssuer,
                             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecurityKey))
                             //AudienceValidator = (m, n, z) =>
                             //{
                             //    return m != null && m.FirstOrDefault().Equals(this.Configuration["audience"]);
                             //},
                         };
                     });
            #endregion

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
           
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
