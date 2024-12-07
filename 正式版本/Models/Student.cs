using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Student
    {
        // 学生ID，自增且为主键
        [Key]
        public int StudentID { get; set; }

        // 学生姓名
        public required string Name { get; set; }

        // 性别
        public required string Gender { get; set; }

        // 年龄
        public int Age { get; set; }

        // 年级
        public required string Grade { get; set; }

        // 专业
        public required string Major { get; set; }

        // 所在省份
        public required string Province { get; set; }
    }
}
