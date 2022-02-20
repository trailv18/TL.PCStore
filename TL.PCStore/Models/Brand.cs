using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TL.PCStore.Models
{
    public class Brand
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [StringLength(255)]
        public string Url { get; set; }
        public bool DeleteFlag { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
