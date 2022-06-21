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
using Microsoft.AspNetCore.Http;

namespace MVCDemo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        //IConfiguration调用appsettings.json中的配置
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

            //app.UseStatusCodePages(
            //"text/plain", "Status code page, status code: {0}");
            //app.UseStatusCodePagesWithRedirects("/StatusCode?code={0}");

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            //UseRouting 向中间件管道添加路由匹配。
            //此中间件会查看应用中定义的终结点集，并根据请求选择最佳匹配。
            app.UseRouting();

            app.UseAuthorization();
            //UseEndpoints 向中间件管道添加终结点执行。 它会运行与所选终结点关联的委托。
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
            //map:实现分支，根据不同分支改用不同的app.use
            //localhost:port/map1
            app.Map("/map1", HandleMapTest1);
            //localhost:port/map2
            app.Map("/map2", HandleMapTest2);
            //localhost:port/map3或其他
            //localhost:port
            app.Run(async context =>
            {
                await context.Response.WriteAsync("Hello from non-Map delegate. <p>");
            });
            //localhost:port/level1/level2a==>level2AApp
            //localhost:port/level1/level2b==>level2BApp
            app.Map("/level1", level1App => {
                level1App.Map("/level2a", level2AApp => {
                    // "/level1/level2a" processing
                });
                level1App.Map("/level2b", level2BApp => {
                    // "/level1/level2b" processing
                });
            });
            //localhost:port/?branch=main,筛选主键包含branch字符串的变量
            app.MapWhen(context => context.Request.Query.ContainsKey("branch"),
                               HandleBranch);


        }
        private static void HandleBranch(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                var branchVer = context.Request.Query["branch"];
                await context.Response.WriteAsync($"Branch used = {branchVer}");
            });
        }


        private static void HandleMapTest1(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                await context.Response.WriteAsync("Map Test 1");
            });
        }

        private static void HandleMapTest2(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                await context.Response.WriteAsync("Map Test 2");
            });
        }

        

    }
}
