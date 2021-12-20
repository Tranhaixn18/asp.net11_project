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
using System.IO;

namespace asp.net11_project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [CheckLogin]
    public class ProductsController : Controller
    {
        MyDbContext db = new MyDbContext();
        public IActionResult Index(int? page)
        {
            int _currentPage = page ?? 1;
            //lấy tất cả bản ghi
            int sobanghitren1trang = 4;
            List<ProductsRecord> listRecord = db.Products.OrderByDescending(p => p.ID).ToList();
            return View("Index", listRecord.ToPagedList(_currentPage, sobanghitren1trang));
        }
        public IActionResult Update(int? id)
        {
            int _id = id ?? 0;
            ProductsRecord record = db.Products.Where(u => u.ID == _id).FirstOrDefault();
            return View("CreateUpdate", record);
        }
        [HttpPost]
        public IActionResult Update(IFormCollection fc,int? id)
        {
            string _name = fc["Name"].ToString().Trim();
            string _price =  fc["Price"].ToString().Trim();
            string _discount = fc["Discount"].ToString().Trim();
            string _categoryID = fc["CategoryID"].ToString().Trim();
            string _description = fc["Description"].ToString().Trim();
            string _content = fc["Content"].ToString().Trim();
            int _hot = fc["Hot"] != "" && fc["Hot"] == "on" ? 1 : 0;
            //lấy bản ghi ứng với id truyền vào
            ProductsRecord record = db.Products.Where(p => p.ID == id).FirstOrDefault();
            //update bản ghi 
            record.Name = _name;
            record.Price = Convert.ToDouble(_price);
            record.Discount = Convert.ToDouble(_discount);
            record.CategoryID = int.Parse(_categoryID);
            record.Description = _description;
            record.Content = _content;
            record.Hot = _hot;
            //lấy thông tin file ảnh
            string fileName = "";
            try
            {
                fileName = Request.Form.Files[0].FileName;
            }
            catch {; }
            if (!string.IsNullOrEmpty(fileName))
            {
                //xóa ảnh cũ
                if(record.Photo != null && System.IO.File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploat", "Products", record.Photo)))
                {
                    //dùng Path.Combine sẽ ghép các tham số thành 1 đường link
                    //xóa
                    System.IO.File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploat", "Products", record.Photo));
                }
                //upload file mới
                var timestmp = DateTime.Now.ToFileTime();
                fileName = timestmp + "_" + fileName;
                //lấy đường dẫn của file
                string _path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Upload/Products", fileName);
                //load file
                using (var stream = new FileStream(_path, FileMode.Create))
                {
                    Request.Form.Files[0].CopyTo(stream);
               }
                //upload giá trị vào cột photo trong sql
                record.Photo = fileName;

            }
            db.SaveChanges();
            return Redirect("/Admin/Products");
        }
        public IActionResult Create()
        {
            return View("CreateUpdate");
        }
        [HttpPost]
        public IActionResult Create(IFormCollection fc)
        {
            string _name = fc["Name"].ToString().Trim();
            string _price = fc["Price"].ToString().Trim();
            string _discount = fc["Discount"].ToString().Trim();
            string _categoryID = fc["CategoryID"].ToString().Trim();
            string _description = fc["Description"].ToString().Trim();
            string _content = fc["Content"].ToString().Trim();
            int _hot = fc["Hot"] != "" && fc["Hot"] == "on" ? 1 : 0;
            ProductsRecord record = new ProductsRecord();
            record.Name = _name;
            record.Price = Convert.ToDouble(_price);
            record.Discount = Convert.ToDouble(_discount);
            record.CategoryID = int.Parse(_categoryID);
            record.Description = _description;
            record.Content = _content;
            record.Hot = _hot;
            string fileName = "";
            try
            {
                //lấy thông tin ở thẻ file
                fileName = Request.Form.Files[0].FileName;
            }
            catch {; }
            if (!string.IsNullOrEmpty(fileName))
            {
                //2 dòng code dưới là để tránh trường hợp trùng tên file nhau.Lấy ra thời gian để gán vào đầu tên file
                var timestmp = DateTime.Now.ToFileTime();
                fileName = timestmp + "_" + fileName;
                string _path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Upload/Products",fileName);
                //lớp FileStream là lớp đọc và ghi dữ liệu ra file
                using(var stream= new FileStream(_path,FileMode.Create))
                {
                    Request.Form.Files[0].CopyTo(stream);
                }
                record.Photo = fileName;
            }
            db.Products.Add(record);
            db.SaveChanges();
            return Redirect("/Admin/Products");
        }
        public IActionResult Delete(int? id)
        {
            var record = db.Products.Where(p => p.ID == id).FirstOrDefault();
            if(record.Photo != null && System.IO.File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Upload", "Products", record.Photo)))
            {
                System.IO.File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Upload", "Products", record.Photo));
            }
            db.Products.Remove(record);
            db.SaveChanges();
            return Redirect("/Admin/Products");
        }
    }
}
