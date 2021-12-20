using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace asp.net11_project.Models
{
    [Table("Rating")]
    public class RatingRecord
    {
        [Key]
        public int ID { get; set; }
        public int ProductID { get; set; }
        public int Star { get; set; }
    }
}
