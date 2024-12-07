using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models
{
    public class ApplicationLoginDbContext : DbContext
    {
        // 构造函数接收 DbContextOptions，并传递给基类 DbContext
        public ApplicationLoginDbContext(DbContextOptions<ApplicationLoginDbContext> options) : base(options) { }

        // DbSet 属性表示数据库中的表
        public DbSet<Login> Login { get; set; }
    }
}
