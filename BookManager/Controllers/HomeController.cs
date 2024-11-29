using Microsoft.AspNetCore.Mvc;
using BookManager.Models;
using Microsoft.Data.SqlClient;
using System.Data.SqlTypes;

namespace BookManager.Controllers;

public class HomeController : BaseController
{

    // 数据库连接字符串，用于连接到 SQL Server 数据库
    public string ConString = "Server=xxx;Database=xx;User Id=xx;Password=xx;TrustServerCertificate=True";

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }
    [HttpPost]
    public IActionResult Login(LoginModel loginModel)
    {
        if (string.IsNullOrWhiteSpace(loginModel.UserName) || string.IsNullOrWhiteSpace(loginModel.PassWord))
        {
            ViewBag.Message = "不能为空！";
        }
        if (ValidateUser(loginModel.UserName, loginModel.PassWord))
        {
            HttpContext.Session.SetString("Username", loginModel.UserName);
            ViewBag.Message = "登录成功。";
            return RedirectToAction("PageNavigation", "Home");
        }
        else
        {
            ViewBag.Message = "登录失败，请检查用户名和密码。";
            return View();
        }
    }

    [HttpPost]
    // 登出方法
    public IActionResult Logout()
    {
        // 清除 Session 数据
        HttpContext.Session.Clear();

        // 跳转到登录页面或者首页
        return RedirectToAction("Login", "Home");
    }

    public bool ValidateUser(string UserName, string PassWord)
    {
        using (var link = new SqlConnection(ConString))
        {
            link.Open();
            string query = "select count(*) from Login Where UserName = @UserName and PassWord = @PassWord";

            using (SqlCommand Command = new SqlCommand(query, link))
            {
                Command.Parameters.AddWithValue("@UserName", UserName);
                Command.Parameters.AddWithValue("@PassWord", PassWord);

                int count = (int)Command.ExecuteScalar();

                if (count > 0)
                {
                    return true;
                }
            }
        }
        return false;
    }

    // 页面导航
    public IActionResult PageNavigation()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Index()
    {
        List<BookModel> Books = new List<BookModel>();

        // 表是book_borrow_record
        using (var link = new SqlConnection(ConString))
        {
            link.Open();
            string query = "SELECT borrow_id, book_title, stu_num, creat_date, remark FROM book_borrow_record";
            using (SqlCommand Command = new SqlCommand(query, link))
            {
                using (SqlDataReader Reader = Command.ExecuteReader())
                {
                    while (Reader.Read())
                    {
                        BookModel book = new BookModel
                        {
                            borrow_id = Reader.GetInt32(0),  // 读取为字符串
                            book_tile = Reader.GetString(1),  // 书名
                            stu_num = Reader.GetString(2),   // 学号
                            creat_date = Reader.GetDateTime(3).ToString("yyyy-MM-dd"),// 借书日期
                            remark = Reader.GetString(4)     // 备注
                        };
                        Books.Add(book);
                    }
                }
            }
        }
        return View(Books);
    }

    public IActionResult StuIndex()
    {
        List<StuModel> StuList = new List<StuModel>();
        using (var link = new SqlConnection(ConString))
        {
            link.Open();
            string query = "select stu_num, stu_name, stu_age, stu_class, stu_gender from stu";
            using (SqlCommand command = new SqlCommand(query, link))
            {
                using (SqlDataReader Reader = command.ExecuteReader())
                {
                    while (Reader.Read())
                    {
                        StuModel stuModel = new StuModel
                        {
                            stu_num = Reader.GetInt32(0),
                            stu_name = Reader.GetString(1),
                            stu_age = Reader.GetInt32(2),
                            stu_class = Reader.GetString(3),
                            stu_gender = Reader.GetString(4),
                        };
                        StuList.Add(stuModel);
                    }
                }
            }
        }
        return View(StuList);
    }
    [HttpGet]
    public IActionResult AddStudent()
    {
        return View();
    }

    [HttpPost]
    public IActionResult AddStudent(StuModel model)
    {
        if (StudentExist(model.stu_num))
        {
            ViewBag.Message = "警告：冲突！您输入的id和数据库里数据id冲突，请重试！";
            return View();
        }
        using (var link = new SqlConnection(ConString))
        {
            link.Open();
            string query = "INSERT INTO stu (stu_num, stu_name, stu_age, stu_class, stu_gender) VALUES (@stu_num, @stu_name, @stu_age, @stu_class, @stu_gender)";
            using (SqlCommand command = new SqlCommand(query, link))
            {
                command.Parameters.AddWithValue("@stu_num", model.stu_num);
                command.Parameters.AddWithValue("@stu_name", model.stu_name);
                command.Parameters.AddWithValue("@stu_age", model.stu_age);
                command.Parameters.AddWithValue("@stu_class", model.stu_class);
                command.Parameters.AddWithValue("@stu_gender", model.stu_gender);

                if (command.ExecuteNonQuery() > 0)
                {
                    ViewBag.Message = "成功！";
                    return View();
                }
                else
                {
                    ViewBag.Message = "失败！";
                    return View();
                }

            }
        }
    }

    [HttpPost]
    public IActionResult DelStudent(int id)
    {
        using (var link = new SqlConnection(ConString))
        {
            link.Open();
            string query = "delete from stu where stu_num = @stu_num";
            using (SqlCommand command = new SqlCommand(query, link))
            {
                command.Parameters.AddWithValue("@stu_num", id);

                if (command.ExecuteNonQuery() > 0)
                {
                    ViewBag.Message = "成功！";
                    return RedirectToAction("StuIndex");
                }
                else
                {
                    ViewBag.Message = "失败！";
                    return RedirectToAction("StuIndex");
                }
            }
        }
    }

    // 查询学生是否存在的方法
    // 待验证
    public bool StudentExist(int stu_num)
    {
        using (var link = new SqlConnection(ConString))
        {
            link.Open();
            string query = "select COUNT(1) from stu where stu_num = @stu_num";
            using (SqlCommand command = new SqlCommand(query, link))
            {
                command.Parameters.AddWithValue("@stu_num", stu_num);
                if ((int)command.ExecuteScalar() > 0)
                {
                    //查到了
                    return true;
                }
                else
                {
                    //没查到
                    return false;
                }
            }
        }
    }

    //出版社显示增加删除方法
    //显示出版社列表方法
    [HttpGet]
    public IActionResult PublisherIndex()
    {
        List<PublisherModel> publisherList = new List<PublisherModel>();
        using (var link = new SqlConnection(ConString))
        {
            link.Open();
            string query = "SELECT publisher_num , publisher_name,publisher_contact, publisher_place FROM Publishers";
            using (SqlCommand command = new SqlCommand(query, link))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        PublisherModel publisher = new PublisherModel()
                        {
                            publisher_num = reader.GetInt32(0),
                            publisher_name = reader.GetString(1),
                            publisher_contact = reader.GetString(2),
                            publisher_place = reader.GetString(3),
                        };
                        publisherList.Add(publisher);
                    }
                }
            }
        }
        return View(publisherList);
    }

    [HttpGet]
    public IActionResult AddPublisher()
    {
        return View();
    }

    [HttpPost]
    public IActionResult AddPublisher(PublisherModel model)
    {
        using (var link = new SqlConnection(ConString))
        {
            link.Open();
            string query = "INSERT INTO Publishers (publisher_num, publisher_name, publisher_contact, publisher_place) VALUES (@publisher_num, @publisher_name, @publisher_contact, @publisher_place)";
            using (SqlCommand command = new SqlCommand(query, link))
            {
                command.Parameters.AddWithValue("@publisher_num", model.publisher_num);
                command.Parameters.AddWithValue("@publisher_name", model.publisher_name);
                command.Parameters.AddWithValue("@publisher_contact", model.publisher_contact);
                command.Parameters.AddWithValue("@publisher_place", model.publisher_place);

                if (command.ExecuteNonQuery() > 0)
                {
                    ViewBag.Message = "出版社添加成功！";
                    return View();
                }
                else
                {
                    ViewBag.Message = "出版社添加失败！";
                    return View();
                }
            }
        }
    }

    [HttpPost]
    public IActionResult DelPublisher(int id)
    {
        using (var link = new SqlConnection(ConString))
        {
            link.Open();
            string query = "DELETE FROM Publishers WHERE publisher_num = @publisher_num";
            using (SqlCommand command = new SqlCommand(query, link))
            {
                command.Parameters.AddWithValue("@publisher_num", id);

                if (command.ExecuteNonQuery() > 0)
                {
                    ViewBag.Message = "成功！";
                    return RedirectToAction("PublisherIndex");
                }
                else
                {
                    ViewBag.Message = "失败！";
                    return RedirectToAction("PublisherIndex");
                }
            }
        }
    }

    // 添加书本
    [HttpGet]
    public IActionResult AddBook()
    {
        return View();
    }

    [HttpPost]
    public IActionResult AddBook(BookModel model)
    {
        using (var link = new SqlConnection(ConString))
        {
            link.Open();
            string query = "INSERT INTO book_borrow_record (book_title, stu_num, creat_date, remark) VALUES (@book_title, @stu_num, @creat_date, @remark)";
            using (SqlCommand command = new SqlCommand(query, link))
            {
                command.Parameters.AddWithValue("@book_title", model.book_tile);
                command.Parameters.AddWithValue("@stu_num", model.stu_num);
                command.Parameters.AddWithValue("@creat_date", model.creat_date);
                command.Parameters.AddWithValue("@remark", model.remark);

                if (command.ExecuteNonQuery() > 0)
                {
                    ViewBag.Message = "学生添加成功！"; // 设置成功消息
                    return View(); // 返回添加页面，显示成功消息
                }
                else
                {
                    ViewBag.Message = "学生添加失败！"; // 设置成功消息
                    return View(); // 返回添加页面，显示成功消息
                }
            }
        }
    }

    // 删除书本

    [HttpPost]
    public IActionResult DelBook(int id)
    {
        using (var link = new SqlConnection(ConString))
        {
            link.Open();
            string query = "DELETE FROM book_borrow_record WHERE borrow_id = @borrow_id";
            using (SqlCommand command = new SqlCommand(query, link))
            {
                command.Parameters.AddWithValue("@borrow_id", id);

                if (command.ExecuteNonQuery() > 0)
                {
                    ViewBag.Message = "成功！";
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Message = "失败！";
                    return RedirectToAction("Index");
                }
            }
        }
    }

    [HttpGet]
    public IActionResult UserIndex()
    {
        List<UserModel> userList = new List<UserModel>();
        using (var link = new SqlConnection(ConString))
        {
            link.Open();
            string query = "SELECT UserName, PassWord FROM Login";
            using (SqlCommand command = new SqlCommand(query, link))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        UserModel user = new UserModel()
                        {
                            UserName = reader.GetString(0),
                            Password = reader.GetString(1)
                        };
                        userList.Add(user);
                    }
                }
            }
        }
        return View(userList);
    }
}