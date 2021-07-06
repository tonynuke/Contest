using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Persistence;
using Services.Contest;
using Services.Scheduler;
using Services.Vk;
using VkNet;
using VkNet.Abstractions;

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
            ConfigureDatabase<ApplicationDbContext>(services);
            ConfigureHangfire(services);

            services.AddSingleton<IVkApi>(new VkApi());
            services.AddSingleton<IScheduler, HangfireScheduler>();
            services.AddScoped<IValidator, Validator>();
            services.AddScoped<IContestService, ContestService>();
            services.AddScoped<VkConfigurationsService>();
            services.AddScoped<IVkCallbackHandler, VkCallbackHandler>();

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

            app.UseHangfireServer();
            app.UseHangfireDashboard();
            RecurringJob.AddOrUpdate(() => Foo(), Cron.Minutely);

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void ConfigureDatabase<T>(
            IServiceCollection services)
            where T : DbContext
        {
            services.AddDbContext<T>(
                optionsBuilder =>
                {
                    optionsBuilder.UseNpgsql(
                        Configuration.GetConnectionString("DatabaseConnection"),
                        builder =>
                        {
                            builder.EnableRetryOnFailure();
                        });

                    if (!Configuration.GetValue<bool>("EnableQueryLogging"))
                    {
                        return;
                    }

                    optionsBuilder.EnableSensitiveDataLogging();
                    optionsBuilder.EnableDetailedErrors();
                }, ServiceLifetime.Scoped);
        }

        private void ConfigureHangfire(IServiceCollection services)
        {
            services.AddHangfire(configuration => configuration
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UsePostgreSqlStorage(Configuration.GetConnectionString("HangfireConnection")));
            services.AddHangfireServer();
        }

        public static void Foo()
        {

        }
    }
}
