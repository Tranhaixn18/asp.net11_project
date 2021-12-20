using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;

namespace asp.net11_project.Models
{
    public class OrdersRecord
    {
        [Key]
        public int ID { get; set; }
        public int CustomerID { get; set; }
        public DateTime Create { get; set; }
        public double Price { get; set; }
        public int Status { get; set; }
    }
}
