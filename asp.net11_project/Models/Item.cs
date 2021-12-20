using asp.net11_project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace asp.net11_project.Models
{
    public class Item
    {
        //thuong tin san pham
        public ProductsRecord ProductItem { get; set; }
        //so luong
        public int Quantity { get; set; }
       
    }
}
