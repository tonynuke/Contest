using Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Persistence;
using Services;
using Services.Contest;

namespace WebService
{
    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">Configuration.</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var databaseConfiguration = Configuration
                .GetSection(DatabaseConfiguration.Key)
                .Get<DatabaseConfiguration>();
            ConfigureDatabase<ApplicationDbContext>(services, databaseConfiguration);

            services.AddScoped<IValidator>();
            services.AddScoped<IContestService>();
            services.AddScoped<IVkCallbackHandler>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebService", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebService v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static void ConfigureDatabase<T>(
            IServiceCollection services, DatabaseConfiguration configuration)
            where T : DbContext
        {
            services.AddDbContext<T>(
                optionsBuilder =>
                {
                    optionsBuilder.UseNpgsql(
                        configuration.ConnectionString,
                        builder =>
                        {
                            builder.EnableRetryOnFailure();
                        });

                    if (!configuration.LogQueries)
                    {
                        return;
                    }

                    optionsBuilder.EnableSensitiveDataLogging();
                    optionsBuilder.EnableDetailedErrors();
                }, ServiceLifetime.Scoped);
        }
    }
}
