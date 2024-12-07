using Microsoft.AspNetCore.Mvc;
using System.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    // 继承 BaseController 让 Seesion 成功配置
    public class HomeController : BaseController
    {
        // 系统专用
        // 声明一个私有的只读字段 _context，用来保存数据库上下文
        private readonly ApplicationDbContext _context;

        // 通过构造函数接收数据库上下文（context），并赋值给 _context 字段
        //public HomeController(ApplicationDbContext context)
        //{
        //    _context = context;  // 将传入的 context 保存在 _context 中
        //}

        // 登录专用
        // 声明一个私有的只读字段 _context，用来保存数据库上下文
        private readonly ApplicationLoginDbContext _contextLogin;

        // 构造函数注入多个上下文
        public HomeController(ApplicationDbContext context, ApplicationLoginDbContext contextLogin)
        {
            _context = context; // 初始化系统数据库上下文
            _contextLogin = contextLogin; // 初始化登录数据库上下文
        }


        // 登录页面拉取
        [HttpGet]
        public IActionResult Login()
        {
            ViewData["Title"] = "登录验证";
            return View();
        }

        // 登录方法
        [HttpPost]
        public IActionResult Login(Login LoginObj)
        {
            if (string.IsNullOrWhiteSpace(LoginObj.UserName) || string.IsNullOrWhiteSpace(LoginObj.PassWord))
            {
                ViewBag.Message = "不能为空！";
            }
            if (ValidateUser(LoginObj.UserName, LoginObj.PassWord))
            {
                HttpContext.Session.SetString("UserName", LoginObj.UserName);
                ViewBag.Message = "登录成功。";

                ViewBag.IsSuccess = true;
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.IsSuccess = false;
                ViewBag.Message = "登录失败，请检查用户名和密码。";
                return View();
            }
        }

        // 登录验证方法封装
        public bool ValidateUser(string UserName, string PassWord)
        {
                var count = _contextLogin.Login.Where(x => x.UserName == UserName && x.PassWord == PassWord).Count();
                return count > 0;
        }

        public IActionResult LoginOut()
        {
            // 清除 Session 数据
            HttpContext.Session.Clear();

            // 跳转到登录页面或者首页
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Index()
        {            
            // 标题更改
            ViewData["Title"] = "学生信息首页";

            // 数据库结果转换成集合返回（Tolist）
            var studentList = _context.StudentInfo.ToList();
            return View(studentList);
        }

        [HttpPost]
        public IActionResult Index(string searchQuery)
        {
            if (string.IsNullOrEmpty(searchQuery))
            {
                var studentList = _context.StudentInfo.ToList();
                return View(studentList);
            }

            searchQuery = searchQuery.Trim();

            var list = _context.StudentInfo.Where(x => x.Name.Contains(searchQuery) ||
                                                       x.Gender.Contains(searchQuery) ||
                                                       x.Grade.Contains(searchQuery) ||
                                                       x.Major.Contains(searchQuery) ||
                                                       x.Province.Contains(searchQuery));
            return View(list);
        }

        [HttpGet]
        public IActionResult Add()
        {
            ViewData["Title"] = "添加页面";
            return View();
        }

        [HttpPost]
        [HttpPost]
        public IActionResult Add(Student StuObj)
        {
            if (!ModelState.IsValid)
            {
                // 模型验证失败，返回到添加视图，显示错误信息
                return View(StuObj); // 这里传递 StuObj 以便在表单中保留用户输入
            }

            // 模型验证通过，保存学生信息
            _context.StudentInfo.Add(StuObj);
            _context.SaveChanges();

            // 保存成功，重定向回列表页
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult DeleteConfirm(int ID)
        {
            ViewData["Title"] = "删除确认页面";
            var studentList = _context.StudentInfo.FirstOrDefault(x => x.StudentID == ID);
            return View(studentList);
        }

        [HttpPost]
        public IActionResult Delete(int ID)
        {
            var student = _context.StudentInfo.FirstOrDefault(x => x.StudentID == ID);
            if (student != null)
            {
                _context.StudentInfo.Remove(student);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        public IActionResult Edit(int ID)
        {
            ViewData["Title"] = "编辑页面";
            var studentList = _context.StudentInfo.FirstOrDefault(x => x.StudentID == ID);
            return View(studentList);
        }

        [HttpPost]
        public IActionResult Edit(Student StuObj)
        {
            var studentList = _context.StudentInfo.FirstOrDefault(x => x.StudentID == StuObj.StudentID);
            if (studentList!= null)
            {
                studentList.Name = StuObj.Name;
                studentList.Gender = StuObj.Gender;
                studentList.Age = StuObj.Age;
                studentList.Grade = StuObj.Grade;
                studentList.Major = StuObj.Major;
                studentList.Province = StuObj.Province;

                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return NotFound();
            }
        }
    }
}
