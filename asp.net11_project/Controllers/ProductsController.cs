using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using asp.net11_project.Models;
using X.PagedList;
using System.IO;
using Microsoft.AspNetCore.Http;
namespace asp.net11_project.Controllers
{
    public class ProductsController : Controller
    {
        public MyDbContext db = new MyDbContext();
        public IActionResult Category(int? id,int? page)
        {

            int categoryID = id ?? 0;
            ViewBag.intCategoryID = categoryID;
            int _currentPage = page ?? 1;
            //lấy tất cả bản ghi
            int sobanghitren1trang = 20;
           
            string _order = !string.IsNullOrEmpty(Request.Query["order"]) ? Request.Query["order"] : "";
            List<ProductsRecord> listRecord = new List<ProductsRecord>();
            switch (_order)
            {
                case "priceAsc":
                    listRecord = db.Products.Where(c => c.CategoryID == categoryID).OrderBy(p => p.Price).ToList();
                    break;
                case "priceDesc":
                    listRecord = db.Products.Where(c => c.CategoryID == categoryID).OrderByDescending(p => p.Price).ToList();
                    break;
                case "nameAsc":
                    listRecord = db.Products.Where(c => c.CategoryID == categoryID).OrderBy(p => p.Name).ToList();
                    break;
                case "nameDesc":
                    listRecord = db.Products.Where(c => c.CategoryID == categoryID).OrderByDescending(p => p.Name).ToList();
                    break;
                default:
                    listRecord = db.Products.Where(c => c.CategoryID == categoryID).OrderByDescending(p => p.ID).ToList();
                    break;
            }
            return View("Category", listRecord.ToPagedList(_currentPage, sobanghitren1trang));
        }
        //chi tiết sản phẩm
        public IActionResult Detail(int? id)
        {
            int _id = id ?? 0;
            ProductsRecord record = db.Products.Where(p => p.ID == _id).FirstOrDefault();
            return View("Detail", record);
        }
        //đánh giá số sao
        public IActionResult Star(int? id)
        {
            int _id = id ?? 0;
            int intStar = !string.IsNullOrEmpty(Request.Query["star"]) ? Convert.ToInt32(Request.Query["star"]) : 0;
            RatingRecord record = new RatingRecord();
            record.ProductID = _id;
            record.Star = intStar;
            db.Rating.Add(record);
            db.SaveChanges();
            return Redirect("/Products/Detail/" + _id);
        }
       
    }
}
