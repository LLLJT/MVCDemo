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

        //IConfiguration����appsettings.json�е�����
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            

            services.AddDbContext<MVCDemoContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("MVCDemoContext")));

            //singleton �������̣�ÿһ��������ͬһ��instance
            //������ͻ򹤳�ע��Ϊ��һʵ�����������Զ��ͷŵ�һʵ����
            //����IDisposable��Dispose,�ɷ��������������񣬲��Զ��ͷ�
            //1.write;2.write;3.write;
            //��1��3Ϊservice.AddSingleton<Service1>();,2Ϊservice.AddScoped<Service2>();
            //��ֻ��2.dispose����Ϊ1��3�Ѿ��Զ��ͷ�


            services.AddSingleton<IOperationSingleton, Operation>();
            //�������� һ���������ﹲ��һ��������һ������һ������
            services.AddScoped<IOperationScope, Operation>();
            //transient��ÿ����Ҫ����һ���µ���
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
            //UseRouting ���м���ܵ����·��ƥ�䡣
            //���м����鿴Ӧ���ж�����ս�㼯������������ѡ�����ƥ�䡣
            app.UseRouting();

            app.UseAuthorization();
            //UseEndpoints ���м���ܵ�����ս��ִ�С� ������������ѡ�ս�������ί�С�
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
            //map:ʵ�ַ�֧�����ݲ�ͬ��֧���ò�ͬ��app.use
            //localhost:port/map1
            app.Map("/map1", HandleMapTest1);
            //localhost:port/map2
            app.Map("/map2", HandleMapTest2);
            //localhost:port/map3������
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
            //localhost:port/?branch=main,ɸѡ��������branch�ַ����ı���
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
