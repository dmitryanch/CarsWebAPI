using CarsApp.Model;
using CarsApp.Model.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CarsApp.MongoORM;

namespace CarsWebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging()
                .AddSingleton<ICarsService, MongoCarsDataService>(s =>
                    new MongoCarsDataService(Configuration["ConnectionStrings:Mongo"], Configuration["MongoSettings:db"]))
                .AddMvc();
            services.Configure<FieldsConfig>(Configuration.GetSection("FieldsConfig"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "Default",
                    template: "{controller}/{action}/{id?}");
            });
        }
    }
}
