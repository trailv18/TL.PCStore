using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TL.PCStore.Models
{
    public class News
    {
        [Key]
        public int Id { get; set; }

        [StringLength(255)]
        public string Title { get; set; }

        [StringLength(1024)]
        public string ShortDescription { get; set; }

        [Column(TypeName = "ntext")]
        [AllowHtml]
        public string PostContent { get; set; }

        [StringLength(255)]
        public string Url { get; set; }

        public bool Published { get; set; }

        public DateTime PostedOn { get; set; }

        public DateTime? Modified { get; set; }
        public int? ViewCount { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}