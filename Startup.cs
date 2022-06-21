using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using MVCDemo.Data;
using MVCDemo.Controllers;

namespace MVCDemo
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
            

            services.AddDbContext<MVCDemoContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("MVCDemoContext")));

            //singleton 单例进程，每一次请求都是同一个instance
            //如果类型或工厂注册为单一实例，则容器自动释放单一实例。
            //调用IDisposable的Dispose,由服务容器创建服务，并自动释放
            //1.write;2.write;3.write;
            //若1，3为service.AddSingleton<Service1>();,2为service.AddScoped<Service2>();
            //则只会2.dispose，因为1，3已经自动释放


            services.AddSingleton<IOperationSingleton, Operation>();
            //作用域单例 一个作用域里共享一个单例，一个请求，一个单例
            services.AddScoped<IOperationScope, Operation>();
            //transient，每次需要创建一个新单例
            services.AddTransient<IOperationTransient, Operation>();
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
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
    }
}
