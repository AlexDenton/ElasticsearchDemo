using System;
using Elasticsearch.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MovieSearchApi.Application;
using MovieSearchApi.Domain;
using Nest;
using SearchMovieApi.Infrastructure;

namespace MovieSearchApi
{
    public class Startup
    {
        private IConfigurationRoot Configuration { get; }
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            Configuration = builder.Build();
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();

            services.Configure<AppSettings>(Configuration);

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });

            services.AddMvc();

            services.AddSingleton<ISearchManager, SearchManager>();
            services.AddSingleton<ISearchRepository, SearchRepository>();
            services.AddSingleton(isp =>
            {
                var uri = new Uri("http://localhost:9201");
                var connectionPool = new SingleNodeConnectionPool(uri);
                var connectionSettings = new ConnectionSettings(connectionPool);
                connectionSettings.DefaultIndex("movies");
                return new ElasticClient(connectionSettings);
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("CorsPolicy");

            //app.UseStaticFiles();
            app.UseMvc();
        }
    }
}
