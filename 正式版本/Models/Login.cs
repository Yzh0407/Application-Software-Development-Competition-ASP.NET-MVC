namespace WebApplication1.Models;

//登录模型类
public class Login
{
    public int ID { get; set; }  // 主键ID
    public required string UserName { get; set; }  // 用户名
    public required string PassWord { get; set; }  // 密码
}
