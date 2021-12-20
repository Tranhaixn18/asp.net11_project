using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using asp.net11_project.Areas.Admin.Attributes;
using asp.net11_project.Models;
using X.PagedList;
using Microsoft.EntityFrameworkCore;

namespace asp.net11_project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [CheckLogin]
    public class OrdersController : Controller
    {
        MyDbContext db = new MyDbContext();
        public IActionResult Index(int? page)
        {
            int currentPage = page ?? 1;
            int perPage = 3;
            List<OrdersRecord> list = db.Orders.OrderByDescending(o => o.ID).ToList();

            return View("Index",list.ToPagedList(currentPage,perPage));
        }
        public IActionResult Detail(int? id)
        {
            int _id = id ?? 0;
            ViewBag.orderID = _id;
            List<OrdersDetailRecord> listRecord = db.OrdersDetail.Where(tbl => tbl.OrderID == _id).ToList();
            
            return View("Detail", listRecord);
        }
        public IActionResult Delivery(int? id)
        {
            int _id = id ?? 0;
            OrdersRecord record = db.Orders.Where(o => o.ID == _id).FirstOrDefault();
            record.Status = 1;
            db.SaveChanges();
            return RedirectToAction("Index", "Orders");
        }
    }
}
