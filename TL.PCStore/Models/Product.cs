using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace TL.PCStore.Models { 

    public class Product
    {

        [Key]
        public int Id { get; set; }

        [StringLength(255)]
        public string Name { get; set; }
        public int Stock { get; set; }
        public decimal Price { get; set; }
        public decimal? Discount { get; set; }
        [AllowHtml]
        public string Description { get; set; }

        [DataType(DataType.Upload)]
        public string Picture { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        [AllowHtml]
        public string Review { get; set; }
        public bool DeleteFlag { get; set; }
        public string Url { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public int BrandId { get; set; }
        public Brand Brand { get; set; }
       
        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
