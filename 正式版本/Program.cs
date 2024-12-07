using WebApplication1.Models;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 添加数据库上下文（DbContext）配置
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // 注册 ApplicationLoginDbContext
            builder.Services.AddDbContext<ApplicationLoginDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("LoginConnection")));


            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // 添加 Session 服务
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // 设置 Session 超时
                options.Cookie.HttpOnly = true; // 只有 HTTP 可访问
                options.Cookie.IsEssential = true; // 确保 Cookie 必须存在
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            // 启用 Session 中间件
            app.UseSession();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
