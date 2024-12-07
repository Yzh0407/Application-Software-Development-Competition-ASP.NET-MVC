using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models
{
    public class ApplicationDbContext : DbContext
    {
        // 构造函数接收 DbContextOptions，并传递给基类 DbContext
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        // DbSet 属性表示数据库中的表，表名为 Login
        public DbSet<Student> StudentInfo { get; set; }
    }
}
