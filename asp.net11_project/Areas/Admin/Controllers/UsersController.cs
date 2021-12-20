using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using asp.net11_project.Models;
using Microsoft.EntityFrameworkCore;
using asp.net11_project.Areas.Admin.Attributes;
using X.PagedList;
using Microsoft.AspNetCore.Http;
using BC = BCrypt.Net.BCrypt;

namespace asp.net11_project.Areas.Admin.Controllers
{
    //khai báo route admin để nhận dạng trên url đây là area admin
    [Area("Admin")]

    //kiểm tra login
    [CheckLogin]
    public class UsersController : Controller
    {
        MyDbContext db = new MyDbContext();
        public IActionResult Index(int? page)
        {
            //lấy trang hiện tại
            //page ?? 1: nếu page != null thì _current=page còn page=null thì _current=1;
            int _currentPage = page ?? 1;
            //lấy tất cả bản ghi
            int sobanghitren1trang = 4;
            List<UsersRecord> listRecord = db.Users.OrderByDescending(u => u.ID).ToList();
            return View("Index",listRecord.ToPagedList(_currentPage,sobanghitren1trang));
        }
        public IActionResult Update(int? id)
        {
            int _id = id ?? 0;
            UsersRecord record = db.Users.Where(u => u.ID == _id).FirstOrDefault();
            return View("CreateUpdate", record);
        }
        //do khi ấn nút submit thì trang ở trạng thái post
        [HttpPost]
        public IActionResult Update(IFormCollection fc,int? id)
        {
            int _id = id ?? 0;
            //trim():cắt khoảng trắng trước sau của đối tượng
            string _name = fc["name"].ToString().Trim();
            string _password = Request.Form["password"].ToString().Trim();
            UsersRecord record = db.Users.Where(u => u.ID == _id).FirstOrDefault();
            record.Name = _name;
            //nếu pass k rỗng thì update pass
            if (!string.IsNullOrEmpty(_password))
            {
                _password = BC.HashPassword(_password);
                record.Password = _password;
            }
            db.SaveChanges();
            return Redirect("/Admin/Users");

        }
        public IActionResult Create()
        {
            return View("CreateUpdate");
        }
        [HttpPost]
        public IActionResult Create(IFormCollection fc)
        {
            string _name = fc["name"].ToString().Trim();
            string _email = Request.Form["email"].ToString().Trim();
            string _password = fc["password"].ToString().Trim();
            UsersRecord record = new UsersRecord();
            record.Name = _name;
            record.Email = _email;
            record.Password = BC.HashPassword(_password);
            db.Users.Add(record);
            db.SaveChanges();
            return Redirect("/Admin/Users");
        }
        //xóa bản ghi
        public IActionResult Delete(int? id)
        {
            int _id = id ?? 0;
            UsersRecord record = db.Users.Where(u => u.ID == _id).FirstOrDefault();
            db.Users.Remove(record);
            db.SaveChanges();
            return Redirect("/Admin/Users");
        }
    }
}
