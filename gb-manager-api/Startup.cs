using gb_manager.Data;
using gb_manager.Data.Interfaces;
using gb_manager.Domain.Models;
using gb_manager.Infraestructure.ExternalServices.MercadoPago;
using gb_manager.Infraestructure.ExternalServices.MercadoPago.Serialization;
using gb_manager.Service;
using gb_manager.Service.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Net.Http.Headers;
using System.Text;

namespace gb_manager
{
    public class Startup
    {

        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddMvc().AddJsonOptions(options =>
            {
                //options.JsonSerializerOptions.PropertyNamingPolicy = null;
                options.JsonSerializerOptions.DictionaryKeyPolicy = null;
                options.JsonSerializerOptions.IgnoreNullValues = true;
                //options.JsonSerializerOptions.IgnoreReadOnlyProperties = true;
            });

            var key = Encoding.ASCII.GetBytes(Utils.TokenService.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero,
                    ValidateLifetime = true
                };

                x.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("Token-Expired", "true");
                        }
                        return System.Threading.Tasks.Task.CompletedTask;
                    }
                };
            });

            services.AddHttpClient("MercadoPago", c =>
            {
                c.BaseAddress = new Uri(Configuration["ExternalServices:MercadoPago:BaseHost"]);
                c.DefaultRequestHeaders.Add("Accept", "application/json");
                c.DefaultRequestHeaders.Add("User-Agent", "pay-as-go");
                c.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue
                    ("Bearer", Configuration["ExternalServices:MercadoPago:AccessToken"]);
            });

            // services
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IPersonService, PersonService>();
            services.AddScoped<IPlanService, PlanService>();
            services.AddScoped<IContractService, ContractService>();
            services.AddScoped<IPaymentService, PaymentService>();

            // models/repositories
            services.AddScoped<IPersonRepository, PersonRepository>();
            services.AddScoped<IPlanRepository, PlanRepository>();
            services.AddScoped<IContractRepository, ContractRepository>();
            services.AddScoped<Persistence, Persistence>();

            // providers
            services.AddScoped<IMercadoPagoService, MercadoPagoService>();
            services.AddScoped<ISerializer, DefaultSerializer>();

            // db factories
            services.AddSingleton<PersistenceBase<Person>, PersonRepository>();
            services.AddSingleton<PersistenceBase<Plan>, PlanRepository>();
            services.AddSingleton<PersistenceBase<Contract>, ContractRepository>();

            Dapper.SqlMapper.AddTypeHandler(new Utils.MySqlGuidTypeHandler());
            Dapper.SqlMapper.RemoveTypeMap(typeof(Guid));
            Dapper.SqlMapper.RemoveTypeMap(typeof(Guid?));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseHttpsRedirection();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            // global cors policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}