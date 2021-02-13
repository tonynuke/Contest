using Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Persistence;

namespace WebService
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
            // services.Configure<DatabaseConfiguration>(
            //    Configuration.GetSection(nameof(DatabaseConfiguration)));
            // services.AddSingleton(provider =>
            //    provider.GetRequiredService<IOptions<DatabaseConfiguration>>().Value);
            var databaseConfiguration = Configuration.GetSection(nameof(DatabaseConfiguration)).Get<DatabaseConfiguration>();
            ConfigureDatabase<ApplicationDbContext>(services, databaseConfiguration);

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
                    optionsBuilder.UseNpgsql(configuration.ConnectionString, builder =>
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
