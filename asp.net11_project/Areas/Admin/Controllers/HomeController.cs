using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//sử dụng các class bên trong Models
using asp.net11_project.Models;
//để nhìn thấy thư mục attribute
using asp.net11_project.Areas.Admin.Attributes;
namespace asp.net11_project.Areas.Admin.Controllers
{
    [Area("Admin")]
    //gọi attribute checklogin(vì nó kế thừa lại ActionFilterAttribute)
    [CheckLogin]
    public class HomeController : Controller
    {
       
        public IActionResult Index()
        {
            return View();
        }
    }
}
