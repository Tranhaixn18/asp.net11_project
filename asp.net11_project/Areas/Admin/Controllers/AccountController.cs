using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//mã hóa password
using BC = BCrypt.Net.BCrypt;
using asp.net11_project.Models;
using Microsoft.EntityFrameworkCore;

namespace asp.net11_project.Areas.Admin.Controllers
{
    //khai báo nó thuộc admin
    [Area("Admin")]
    public class AccountController : Controller
    {
        MyDbContext db = new MyDbContext();
        [HttpGet]
        public IActionResult Login()
        {
            return View("Login");
        }
        [HttpPost]
        public IActionResult Login(IFormCollection fc)
        {
            string _email = fc["email"];
            string _password = fc["password"];
            UsersRecord record = db.Users.Where(u=> u.Email==_email).FirstOrDefault();
            //<=> select top 1 from Users where email=_email
            if (record != null)
            {
                if (BC.Verify(_password, record.Password))
                {
                    //khởi tạo biến session
                    HttpContext.Session.SetString("email", _email);
                    return Redirect("/Admin/Home");
                }
            }
            else
               return Redirect("/Admin/Account/Login");
            return Redirect("/Admin/Account/Login");

        }
        //đăng xuất
        public IActionResult Logout()
        {
            //hủy biến session
            HttpContext.Session.Remove("email");
            return Redirect("/Admin/Account/Login");
        }
    }
}
