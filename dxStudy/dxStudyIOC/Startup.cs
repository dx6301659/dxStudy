using dxStudyIOCByAssembly.Contract;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;

namespace dxStudyIOC
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
            services.AddControllersWithViews();

            //Method One
            //services.AddSingleton(typeof(DemoService), new DemoService());

            //Method Two
            //services.AddSingleton(typeof(DemoService));

            //Method Three
            services.AddSingleton<DemoService>();

            //Method Three
            services.AddSingleton<IDemo2Service, Demo2Service>();

            AddSingletonServices(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private static void AddSingletonServices(IServiceCollection services)
        {
            var assembly = Assembly.Load(new AssemblyName("dxStudyIOCByAssembly"));
            var serviceTypes = assembly.GetTypes().Where(x => typeof(IServiceSupport).IsAssignableFrom(x) && !x.IsAbstract);

            foreach (var serviceType in serviceTypes)
            {
                foreach (var serviceInterface in serviceType.GetInterfaces())
                {
                    services.AddSingleton(serviceInterface, serviceType);
                }
            }
        }
    }
}
