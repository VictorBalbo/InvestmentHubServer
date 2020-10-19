using InvestmentHub.ServerApplication.Managers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InvestmentHub.ServerApplication
{
    public class Startup
    {
        private readonly IConfigurations _configurations;

        public Startup(IConfiguration configuration)
        {
            _configurations = configuration.GetSection(Configurations.ConfigurationKey).Get<Configurations>();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            RegisterConfiguration(services);

            // Register DI
            var registrable = new Registrable();
            registrable
                .RegisterTo(services);

            services
                .AddMvc()
                .AddJsonOptions(options => options.JsonSerializerOptions.IgnoreNullValues = true)
                .AddControllersAsServices();

            var key = Encoding.ASCII.GetBytes(_configurations.SymmetricKey);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        var cancellationTokenSource = new CancellationTokenSource(_configurations.DefaultCancellationTokenExpiration);
                        var accountManager = context.HttpContext.RequestServices.GetRequiredService<IAccountManager>();
                        var accountEmail = context.Principal.Identity.Name;
                        var account = accountManager.GetAcountAsync(accountEmail, cancellationTokenSource.Token);
                        if (account == null)
                        {
                            // return unauthorized if user no longer exists
                            context.Fail("Unauthorized");
                        }
                        return Task.CompletedTask;
                    }
                };
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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

        private void RegisterConfiguration(IServiceCollection services)
        {
            services.AddSingleton(_configurations);
        }
    }
}
