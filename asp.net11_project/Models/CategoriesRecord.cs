using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace asp.net11_project.Models
{
    [Table("Categories")]
    public class CategoriesRecord
    {
        [Key]
        public int ID { get; set; }
        public int ParentID { get; set; }
        public string Name { get; set; }

    }
}
