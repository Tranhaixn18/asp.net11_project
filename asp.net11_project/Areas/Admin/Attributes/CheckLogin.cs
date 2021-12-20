using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;
namespace asp.net11_project.Areas.Admin.Attributes
{
    public class CheckLogin:ActionFilterAttribute
    {
        //lấy đối tượng httpContext khi đã đăng kí ở file startup.cs
        public HttpContext _httpContext => new HttpContextAccessor().HttpContext;
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //_httpContext.Session.GetString("email"): lấy biến session email
            //để sử dụng duongf dưới phải cài gói nuget session
            string _email = _httpContext.Session.GetString("email");
            if (string.IsNullOrEmpty(_email))
           
                _httpContext.Response.Redirect("/Admin/Account/Login");
           
            base.OnActionExecuting(context);
        }

    }
}
