using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using asp.net11_project.Models;
using Microsoft.AspNetCore.Http;

namespace asp.net11_project.Controllers
{
    public class CartController : Controller
    {
        public MyDbContext db = new MyDbContext();
        public IActionResult Index()
        {
            //lấy sản phẩm trong giỏ hàng
            List<Item> _cart = Cart.GetCart(HttpContext.Session);
            if(_cart != null)
            {
                ViewBag._cart = _cart;
                ViewBag._total = _cart.Sum(tbl => (tbl.ProductItem.Price - (tbl.ProductItem.Price * tbl.ProductItem.Discount / 100)) * tbl.Quantity);
            }
            return View();
        }
        //thêm sản phẩm vào cart
        public IActionResult Buy(int id)
        {
            Cart.CartAdd(HttpContext.Session, id);
            return RedirectToAction("Index", "Cart");
        }
        //xóa sản phẩm khỏi giỏ hàng
        public IActionResult Remove(int id)
        {
            Cart.CartRemove(HttpContext.Session, id);
            return RedirectToAction("Index", "Cart");
        }
        public IActionResult Destroy()
        {
            Cart.CartDestroy(HttpContext.Session);
            return RedirectToAction("Index", "Cart");
        }
        //cập nhật
        public IActionResult Update()
        {
            List<Item> _cart = Cart.GetCart(HttpContext.Session);
            foreach(var item in _cart)
            {
                int quantity = Convert.ToInt32(Request.Form["product_" + item.ProductItem.ID]);
                Cart.CartUpdate(HttpContext.Session, item.ProductItem.ID, quantity);
            }
            return RedirectToAction("Index", "Cart");
        }
        //thanh toan
        public IActionResult Checkout()
        {
            if(HttpContext.Session.GetString("email")== null)
            {
                return Redirect("/Account/Login");
            }
            else
            {
                List<Item> _cart = Cart.GetCart(HttpContext.Session);
                //lấy id của customer
                int customer_id = int.Parse(HttpContext.Session.GetString("customer_id"));
                //insert vào bảng order
                OrdersRecord recordOrder = new OrdersRecord();
                recordOrder.CustomerID = customer_id;
                recordOrder.Create = DateTime.Now;
                recordOrder.Price = _cart.Sum(tbl=>(double.Parse(((tbl.ProductItem.Price - (tbl.ProductItem.Price * tbl.ProductItem.Discount/100))*tbl.Quantity).ToString())));
                db.Orders.Add(recordOrder);
                db.SaveChanges();
                //lấy id vừa insert
                int order_id = recordOrder.ID;
                foreach(var item in _cart)
                {
                    OrdersDetailRecord recordOrderDetail = new OrdersDetailRecord();
                    recordOrderDetail.OrderID = order_id;
                    recordOrderDetail.ProductID = item.ProductItem.ID;
                    recordOrderDetail.Price =double.Parse((item.ProductItem.Price - (item.ProductItem.Price * item.ProductItem.Discount)/100).ToString());
                    recordOrderDetail.Quantity = item.Quantity;
                    db.OrdersDetail.Add(recordOrderDetail);
                    db.SaveChanges();
                }
              
            }
            //xóa tất cả item trong cart
            Cart.CartDestroy(HttpContext.Session);
            return Redirect("/Cart?Checkout=success");
        }
    }
}
